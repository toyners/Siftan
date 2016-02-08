
namespace Siftan.WinForms
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.File;
  using Jabberwocky.Toolkit.Object;

  /// <summary>
  /// Is the base for all controllers used in application.
  /// </summary>
  public abstract class BaseController
  {
    protected readonly UILogManager uiLogManager;

    protected MainForm mainForm;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseController"/> class.
    /// </summary>
    /// <param name="logManager">Instance that performs logging of the process.</param>
    public BaseController(ILogManager logManager)
    {
      logManager.VerifyThatObjectIsNotNull("Parameter 'logManager' is null.");
      this.uiLogManager = new UILogManager(logManager);
      this.uiLogManager.MessageLogged += this.MessageLoggedHandler;
    }

    /// <summary>
    /// Cancels the process.
    /// </summary>
    public abstract void CancelProcess();

    /// <summary>
    /// Creates the main form used as the primary view.
    /// </summary>
    /// <returns>Instance of the main form.</returns>
    public MainForm CreateMainForm()
    {
      this.mainForm = new MainForm(this);
      return this.mainForm;
    }

    /// <summary>
    /// Starts the process and launches the engine.
    /// </summary>
    public void StartProcess()
    {
      this.VerifyParameters();

      String[] inputFilePaths = FilePatternResolver.ResolveFilePattern(this.mainForm.InputFilePattern, this.mainForm.InputFileSearchDepth);

      IRecordReader recordReader = this.CreateRecordReader();

      IRecordMatchExpression expression = new InListExpression(this.mainForm.ValueList);

      this.uiLogManager.JobLogFilePath = Path.Combine(this.mainForm.OutputDirectory, "Job.log");

      this.LaunchEngine(inputFilePaths, recordReader, expression);
    }

    /// <summary>
    /// Launches the engine.
    /// </summary>
    /// <param name="inputFilePaths">List of full input files to be processed.</param>
    /// <param name="recordReader">Instance that reads the record from the input files.</param>
    /// <param name="expression">Instance of the expression used to matched against the record.</param>
    public abstract void LaunchEngine(String[] inputFilePaths, IRecordReader recordReader, IRecordMatchExpression expression);

    private IRecordReader CreateRecordReader()
    {
      if (this.mainForm.HasDelimitedRecord)
      {
        return new DelimitedRecordReader(mainForm.GetDelimitedRecord());
      }

      if (this.mainForm.HasFixedWidthRecord)
      {
        return new FixedWidthRecordReader(mainForm.GetFixedWidthRecord());
      }

      throw new Exception();
    }

    private void MessageLoggedHandler(Object sender, String message)
    {
      this.mainForm.DisplayLogMessage(message);
    }

    private void VerifyParameters()
    {
    }
  }
}
