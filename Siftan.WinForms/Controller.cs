
namespace Siftan.WinForms
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.File;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Object;
  using Siftan;

  public class Controller
  {
    private readonly ILogManager logManager;

    public event MessageLoggedEventHandler MessageLogged;

    public Controller(ILogManager logManager)
    {
      logManager.VerifyThatObjectIsNotNull("Parameter 'logManager' is null.");
      this.logManager = logManager;
    }

    internal void StartProcess(MainForm mainForm)
    {
      VerifyParameters(mainForm);

      String[] inputFiles = FilePatternResolver.ResolveFilePattern(mainForm.InputFilePattern, mainForm.InputFileSearchDepth);

      IRecordReader recordReader = this.CreateRecordReader(mainForm);

      IRecordMatchExpression expression = new InListExpression(mainForm.ValueList);

      StatisticsManager statisticsManager = new StatisticsManager();

      OneFileRecordWriter recordWriter = new OneFileRecordWriter(
        mainForm.MatchedOutputFilePath,
        mainForm.UnmatchedOutputFilePath,
        statisticsManager);

      this.logManager.JobLogFilePath = Path.Combine(mainForm.OutputDirectory, "Job.log");

      UILogManager winFormsLogManager = new UILogManager(this.logManager);
      winFormsLogManager.MessageLogged += MessageLoggedHandler;

      new Engine().Execute(
        inputFiles,
        winFormsLogManager,
        new FileReaderFactory(),
        recordReader,
        expression,
        recordWriter,
        statisticsManager,
        statisticsManager);

      recordWriter.Close();
    }

    internal MainForm CreateMainForm()
    {
      MainForm mainForm = new MainForm(this);

      this.MessageLogged += mainForm.MessageLoggedHandler;

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
      };

      return new DelimitedRecordReader(descriptor);
    }

    private void MessageLoggedHandler(Object sender, String message)
    {
      if (this.MessageLogged != null)
      {
        this.MessageLogged(sender, message);
      }
    }
  }
}