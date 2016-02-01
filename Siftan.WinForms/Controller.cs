
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
      UILogManager uiLogManager = null;

      try
      {
        uiLogManager = new UILogManager(this.logManager);
        uiLogManager.MessageLogged += MessageLoggedHandler;

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

        new Engine().Execute(
          inputFiles,
          uiLogManager,
          new FileReaderFactory(),
          recordReader,
          expression,
          recordWriter,
          statisticsManager,
          statisticsManager);

        recordWriter.Close();
      }
      finally
      {
        uiLogManager.Close();
      }
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
        Qualifier = mainForm.Qualifier,
        HeaderID = mainForm.HeaderLineID,
        LineIDIndex = mainForm.LineIDIndex,
        DelimitedTerm = new DelimitedRecordDescriptor.TermDefinition(mainForm.TermLineID, mainForm.TermIndex)
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