
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

      OneFileRecordWriter recordWriter = new OneFileRecordWriter(
        mainForm.MatchedOutputFilePath,
        mainForm.UnmatchedOutputFilePath,
        null);

      StatisticsManager statisticsManager = new StatisticsManager();

      new Engine().Execute(
        inputFiles,
        this.logManager,
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
      return new MainForm(this);
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
  }
}