
namespace Siftan.WinForms
{
  using System;
  using System.ComponentModel;
  using Jabberwocky.Toolkit.IO;
  using Siftan;

  /// <summary>
  /// Controller that uses a background worker to perform the process in a reponsive fashion.
  /// </summary>
  public class BackgroundController : BaseController
  {
    private IRecordWriter recordWriter;

    private BackgroundWorker worker;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundController"/> class.
    /// </summary>
    /// <param name="logManager">Instance that performs logging of the process.</param>
    public BackgroundController(ILogManager logManager) : base(logManager)
    {
    }

    /// <summary>
    /// Cancels the process.
    /// </summary>
    public override void CancelProcess()
    {
      if (this.worker != null)
      {
        this.worker.CancelAsync();
      }
    }

    /// <summary>
    /// Launches the engine.
    /// </summary>
    /// <param name="inputFilePaths">List of full input files to be processed.</param>
    /// <param name="recordReader">Instance that reads the record from the input files.</param>
    /// <param name="expression">Instance of the expression used to matched against the record.</param>
    public override void LaunchEngine(String[] inputFilePaths, IRecordReader recordReader, IRecordMatchExpression expression)
    {
      this.recordWriter = CreateRecordWriter();

      Engine engine = new Engine();
      engine.FileOpened += this.FileOpenedHandler;
      engine.FileRead += this.FileReadHandler;
      engine.CheckForCancellation = this.CheckForCancellation;

      this.worker = new BackgroundWorker();
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += (sender, e) =>
      {
        engine.Execute(
          inputFilePaths,
          this.uiLogManager,
          new FileReaderFactory(),
          recordReader,
          expression,
          this.recordWriter,
          this.statisticsManager,
          this.statisticsManager);

        if (engine.CheckForCancellation())
        {
          e.Cancel = true;
        }
      };

      this.worker.RunWorkerCompleted += BackgroundWorkCompleted;
      this.worker.RunWorkerAsync();
    }

    /// <summary>
    /// Event handler for message logging. 
    /// </summary>
    /// <param name="sender">Object that raised the message logging event.</param>
    /// <param name="message">Message being logged.</param>
    public override void MessageLoggedHandler(Object sender, String message)
    {
      // This method is always called from a non-UI thread so marshal the call
      // to UI thread here.
      Action action = () => this.mainForm.DisplayLogMessage(message);
      this.mainForm.Invoke(action);
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

    private void FileOpenedHandler(Object sender, Int64 size)
    {
      this.mainForm.SetCurrentFileSize(size);
    }

    private void FileReadHandler(Object sender, Int64 position)
    {
      // This method is always called from a non-UI thread so marshal the call
      // to UI thread here.
      Action action = () => this.mainForm.SetCurrentFilePosition(position);
      this.mainForm.Invoke(action);
    }

    private Boolean CheckForCancellation()
    {
      return this.worker.CancellationPending;
    }
  }
}