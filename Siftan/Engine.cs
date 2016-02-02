
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Object;

  public class Engine
  {
    private ILogManager logManager;

    private IStatisticsCollector statisticsCollector;

    private IStatisticsReporter statisticsReporter;

    public event FileOpenedEventHandler FileOpened;

    public event FileReadEventHandler FileRead;

    public CheckForCancellationDelegate CheckForCancellation;

    #region Methods
    public void Execute(
      String[] filePaths,
      ILogManager logManager,
      IStreamReaderFactory streamReaderFactory,
      IRecordReader recordReader,
      IRecordMatchExpression expression,
      IRecordWriter recordWriter,
      IStatisticsCollector statisticsCollector,
      IStatisticsReporter statisticsReporter)
    {
      logManager.VerifyThatObjectIsNotNull("Parameter 'logManager' is null.");

      try
      {
        this.logManager = logManager;

        statisticsCollector.VerifyThatObjectIsNotNull("Parameter 'statisticsCollector' is null.");
        this.statisticsCollector = statisticsCollector;

        statisticsReporter.VerifyThatObjectIsNotNull("Parameter 'statisticsReporter' is null.");
        this.statisticsReporter = statisticsReporter;

        this.logManager.WriteMessagesToLogs("Run Started...");

        Action<IStreamReader, Record> writeMatchedRecordMethod, writeUnmatchedRecordMethod;
        this.DetermineOutputMethods(recordWriter, out writeMatchedRecordMethod, out writeUnmatchedRecordMethod);

        this.Process(filePaths, streamReaderFactory, recordReader, expression, writeMatchedRecordMethod, writeUnmatchedRecordMethod);

        recordWriter.Close();

        this.statisticsReporter.WriteToLog(this.logManager);

        this.logManager.WriteMessagesToLogs("Run Finished.");
      }
      catch (Exception exception)
      {
        logManager.WriteMessageToApplicationLog("EXCEPTION: " + exception.Message);
        logManager.WriteMessageToApplicationLog("STACK: " + exception.StackTrace);
        throw exception;
      }
    }

    private void DetermineOutputMethods(IRecordWriter recordWriter, out Action<IStreamReader, Record> writeMatchedRecordMethod, out Action<IStreamReader, Record> writeUnmatchedRecordMethod)
    {
      if (recordWriter.DoWriteMatchedRecords && recordWriter.DoWriteUnmatchedRecords)
      {
        writeMatchedRecordMethod = recordWriter.WriteMatchedRecord;
        writeUnmatchedRecordMethod = recordWriter.WriteUnmatchedRecord;
      }
      else if (recordWriter.DoWriteMatchedRecords)
      {
        writeMatchedRecordMethod = recordWriter.WriteMatchedRecord;
        writeUnmatchedRecordMethod = this.WriteNothing;
      }
      else
      {
        writeMatchedRecordMethod = this.WriteNothing;
        writeUnmatchedRecordMethod = recordWriter.WriteUnmatchedRecord;
      }
    }

    private void Process(String[] filePaths, IStreamReaderFactory streamReaderFactory, IRecordReader recordReader, IRecordMatchExpression expression, Action<IStreamReader, Record> writeMatchedRecordMethod, Action<IStreamReader, Record> writeUnmatchedRecordMethod)
    {
      foreach (String filePath in filePaths)
      {
        this.logManager.WriteImportantMessageToJobLog("Processing '" + filePath + "'.");
        IStreamReader fileReader = streamReaderFactory.CreateStreamReader(filePath);

        this.OnFileOpened(fileReader.Length);

        Record record;
        while (!expression.HasReachedMatchQuota && (record = recordReader.ReadRecord(fileReader)) != null)
        {
          this.OnFileRead(fileReader.Position);

          String message = "Record found at position " + record.Start + " with Term '" + record.Term + "'";
          if (expression.IsMatch(record))
          {
            this.statisticsCollector.RecordIsMatched(filePath);
            message += " matches with List Term '" + record.Term + "'";
            writeMatchedRecordMethod(fileReader, record);
          }
          else
          {
            this.statisticsCollector.RecordIsUnmatched(filePath);
            writeUnmatchedRecordMethod(fileReader, record);
          }

          message += ".";
          this.logManager.WriteMessageToJobLog(message);

          if (this.IsCancelled())
          {
            break;
          }
        }

        fileReader.Close();
      }
    }

    private void OnFileOpened(Int64 fileSize)
    {
      if (this.FileOpened != null)
      {
        this.FileOpened(this, fileSize);
      }
    }

    private void OnFileRead(Int64 filePosition)
    {
      if (this.FileRead != null)
      {
        this.FileRead(this, filePosition);
      }
    }

    private Boolean IsCancelled()
    {
      if (this.CheckForCancellation != null)
      {
        return this.CheckForCancellation();
      }

      return false;
    }

    private void WriteNothing(IStreamReader reader, Record record)
    {
    }
    #endregion
  }
}
