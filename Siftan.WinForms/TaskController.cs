
namespace Siftan.WinForms
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Jabberwocky.Toolkit.IO;

  /// <summary>
  /// Controller that uses tasks to perform the process in a reponsive fashion.
  /// </summary>
  public class TaskController : BaseController
  {
    private CancellationTokenSource cancellationTokenSource;

    private CancellationToken cancellationToken;

    private TaskScheduler mainTaskScheduler;

    public TaskController(ILogManager logManager) : base(logManager)
    {
    }

    /// <summary>
    /// Cancels the process.
    /// </summary>
    public override void CancelProcess()
    {
      this.cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// Launches the engine.
    /// </summary>
    /// <param name="inputFilePaths">List of full input files to be processed.</param>
    /// <param name="recordReader">Instance that reads the record from the input files.</param>
    /// <param name="expression">Instance of the expression used to matched against the record.</param>
    public override void LaunchEngine(String[] inputFilePaths, IRecordReader recordReader, IRecordMatchExpression expression)
    {
      StatisticsManager statisticsManager = new StatisticsManager();

      var recordWriter = new OneFileRecordWriter(
        this.mainForm.MatchedOutputFilePath,
        this.mainForm.UnmatchedOutputFilePath,
        statisticsManager);

      Engine engine = new Engine();
      engine.FileOpened += this.FileOpenedHandler;
      engine.FileRead += this.FileReadHandler;
      engine.CheckForCancellation = this.CheckForCancellation;

      // Get the UI thread now because this method is guaranteed to be running on that thread (since this
      // method is called from a winform control handler method)
      this.mainTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

      // Create a new cancellation source and token - the previous one may have been cancelled already.
      this.cancellationTokenSource = new CancellationTokenSource();
      this.cancellationToken = this.cancellationTokenSource.Token;

      Task task = Task.Factory.StartNew(() =>
      {
        engine.Execute(
          inputFilePaths,
          this.uiLogManager,
          new FileReaderFactory(),
          recordReader,
          expression,
          recordWriter,
          statisticsManager,
          statisticsManager);
      }, this.cancellationToken);

      Task finishedTask = task.ContinueWith((antecedent) =>
      {
        if (antecedent.Exception != null)
        {
          this.uiLogManager.WriteMessagesToLogs("Job FAILED.");
          AggregateException ae = antecedent.Exception.Flatten();
          Int32 count = 1;
          foreach (Exception e in ae.InnerExceptions)
          {
            this.uiLogManager.WriteMessagesToLogs(String.Format("EXCEPTION {0}: {1}", count++, e.Message));
            this.uiLogManager.WriteMessagesToLogs("STACK: " + e.StackTrace);
          }
        }
        else if (this.cancellationToken.IsCancellationRequested)
        {
          this.uiLogManager.WriteMessagesToLogs("CANCELLED.");
        }

        this.uiLogManager.Close();
        this.mainForm.JobFinished();
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public override void MessageLoggedHandler(Object sender, String message)
    {
      new Task(() =>
      {
        this.mainForm.DisplayLogMessage(message);
      }).Start(mainTaskScheduler);
    }

    private Boolean CheckForCancellation()
    {
      if (this.cancellationToken.IsCancellationRequested)
      {
        return true;
      }

      return false;
    }

    private void FileOpenedHandler(Object sender, Int64 size)
    {
      this.mainForm.SetCurrentFileSize(size);
    }

    private void FileReadHandler(Object sender, Int64 position)
    {
      new Task(() =>
      {
        this.mainForm.SetCurrentFilePosition(position);
      }).Start(mainTaskScheduler);
    }

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
  }
}
