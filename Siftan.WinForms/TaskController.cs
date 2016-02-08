
namespace Siftan.WinForms
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Jabberwocky.Toolkit.IO;

  /// <summary>
  /// Controller that usestasks to perform the process in a reponsive fashion.
  /// </summary>
  public class TaskController : BaseController
  {
    private readonly CancellationTokenSource cancellationTokenSource;

    private readonly CancellationToken cancellationToken;

    public TaskController(ILogManager logManager) : base(logManager)
    {
      this.cancellationTokenSource = new CancellationTokenSource();
      this.cancellationToken = this.cancellationTokenSource.Token;
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
      }, this.cancellationTokenSource.Token);

      Task finishedTask = task.ContinueWith((antecedent) =>
      {
        this.mainForm.JobFinished();
      }, TaskScheduler.FromCurrentSynchronizationContext());
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
      }).Start(TaskScheduler.FromCurrentSynchronizationContext());
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
