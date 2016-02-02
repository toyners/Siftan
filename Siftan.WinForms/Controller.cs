
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

    private BackgroundWorker worker;

    private MainForm mainForm;

    public Controller(ILogManager logManager)
    {
      logManager.VerifyThatObjectIsNotNull("Parameter 'logManager' is null.");
      this.uiLogManager = new UILogManager(logManager);
      this.uiLogManager.MessageLogged += this.MessageLoggedHandler;
    }

    internal void CancelProcess()
    {
      if (this.worker != null)
      {
        this.worker.CancelAsync();
      }
    }

    internal MainForm CreateMainForm()
    {
      this.mainForm = new MainForm(this);
      return this.mainForm;
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
      engine.CheckForCancellation = this.CheckForCancellation;

      this.worker = new BackgroundWorker();
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += (sender, e) =>
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

        if (engine.CheckForCancellation())
        {
          e.Cancel = true;
        }
      };

      this.worker.RunWorkerCompleted += BackgroundWorkCompleted;
      this.worker.RunWorkerAsync();
    }

    private void BackgroundWorkCompleted(Object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        this.uiLogManager.WriteMessagesToLogs("Job FAILED.");
        this.uiLogManager.WriteMessagesToLogs("EXCEPTION: " + e.Error.Message);
        this.uiLogManager.WriteMessagesToLogs("STACK: " + e.Error.StackTrace);
      }
      else if (e.Cancelled)
      {
        this.uiLogManager.WriteMessagesToLogs("CANCELLED.");
      }

      this.recordWriter.Close();
      this.uiLogManager.Close();
      this.worker = null;
      this.mainForm.JobFinished();
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
      this.mainForm.DisplayLogMessage(message);
    }

    private void FileOpenedHandler(Object sender, Int64 size)
    {
      this.mainForm.SetCurrentFileSize(size);
    }

    private void FileReadHandler(Object sender, Int64 position)
    {
      this.mainForm.SetCurrentFilePosition(position);
    }

    private Boolean CheckForCancellation()
    {
      return this.worker.CancellationPending;
    }
  }
}