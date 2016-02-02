
namespace Siftan.WinForms
{
  using System;
  using System.ComponentModel;
  using System.IO;
  using Jabberwocky.Toolkit.File;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Object;
  using Siftan;

  public class Controller
  {
    private readonly UILogManager uiLogManager;

    private IRecordWriter recordWriter;

    private event MessageLoggedEventHandler MessageLogged;

    private event FileOpenedEventHandler FileOpened;

    private event FileReadEventHandler FileRead;

    public Controller(ILogManager logManager)
    {
      logManager.VerifyThatObjectIsNotNull("Parameter 'logManager' is null.");
      this.uiLogManager = new UILogManager(logManager);
      this.uiLogManager.MessageLogged += MessageLoggedHandler;
    }

    internal void StartProcess(MainForm mainForm)
    {
      VerifyParameters(mainForm);

      String[] inputFiles = FilePatternResolver.ResolveFilePattern(mainForm.InputFilePattern, mainForm.InputFileSearchDepth);

      IRecordReader recordReader = this.CreateRecordReader(mainForm);

      IRecordMatchExpression expression = new InListExpression(mainForm.ValueList);

      StatisticsManager statisticsManager = new StatisticsManager();

      this.recordWriter = new OneFileRecordWriter(
        mainForm.MatchedOutputFilePath,
        mainForm.UnmatchedOutputFilePath,
        statisticsManager);

      this.uiLogManager.JobLogFilePath = Path.Combine(mainForm.OutputDirectory, "Job.log");

      Engine engine = new Engine();
      engine.FileOpened += this.FileOpenedHandler;
      engine.FileRead += this.FileReadHandler;

      BackgroundWorker worker = new BackgroundWorker();
      worker.DoWork += (sender, e) =>
      {
        engine.Execute(
          inputFiles,
          uiLogManager,
          new FileReaderFactory(),
          recordReader,
          expression,
          recordWriter,
          statisticsManager,
          statisticsManager);
      };

      worker.RunWorkerCompleted += BackgroundWorkCompleted;
      worker.RunWorkerAsync();
    }

    private void BackgroundWorkCompleted(Object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        this.uiLogManager.WriteMessagesToLogs("EXCEPTION: " + e.Error.Message);
        this.uiLogManager.WriteMessagesToLogs("STACK: " + e.Error.StackTrace);
      }

      this.recordWriter.Close();
      this.uiLogManager.Close();
    }

    internal MainForm CreateMainForm()
    {
      MainForm mainForm = new MainForm(this);

      this.MessageLogged += mainForm.MessageLoggedHandler;
      this.FileOpened += mainForm.FileOpenedHandler;
      this.FileRead += mainForm.FileReadHandler;

      return mainForm;
    }

    private void VerifyParameters(MainForm mainForm)
    {
    }

    private IRecordReader CreateRecordReader(MainForm mainForm)
    {
      DelimitedRecordDescriptor descriptor = new DelimitedRecordDescriptor
      {
        Delimiter = mainForm.Delimiter,
        Qualifier = mainForm.Qualifier,
        HeaderID = mainForm.HeaderLineID,
        LineIDIndex = mainForm.LineIDIndex,
        DelimitedTerm = new DelimitedRecordDescriptor.TermDefinition(mainForm.TermLineID, mainForm.TermIndex)
      };

      return new DelimitedRecordReader(descriptor);
    }

    private void MessageLoggedHandler(Object sender, String message)
    {
      this.MessageLogged(sender, message);
    }

    private void FileOpenedHandler(Object sender, Int64 size)
    {
      this.FileOpened(sender, size);
    }

    private void FileReadHandler(Object sender, Int64 position)
    {
      this.FileRead(sender, position);
    }
  }
}