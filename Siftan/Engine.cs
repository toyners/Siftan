﻿
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

        if (recordWriter.DoWriteMatchedRecords && recordWriter.DoWriteUnmatchedRecords)
        {
          this.SelectMatchedAndUnmatchedRecords(filePaths, streamReaderFactory, recordReader, expression, recordWriter.WriteMatchedRecord, recordWriter.WriteUnmatchedRecord);
        }
        else if (recordWriter.DoWriteMatchedRecords)
        {
          this.SelectMatchedRecordsOnly(filePaths, streamReaderFactory, recordReader, expression, recordWriter.WriteMatchedRecord);
        }
        else if (recordWriter.DoWriteUnmatchedRecords)
        {
          this.SelectUnmatchedRecordsOnly(filePaths, streamReaderFactory, recordReader, expression, recordWriter.WriteUnmatchedRecord);
        }

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

    private void SelectMatchedRecordsOnly(String[] filePaths, IStreamReaderFactory streamReaderFactory, IRecordReader recordReader, IRecordMatchExpression expression, Action<IStreamReader, Record> writeRecordMethod)
    {
      foreach (String filePath in filePaths)
      {
        IStreamReader fileReader = streamReaderFactory.CreateStreamReader(filePath);

        Record record;
        while (!expression.HasReachedMatchQuota && (record = recordReader.ReadRecord(fileReader)) != null)
        {
          if (expression.IsMatch(record))
          {
            writeRecordMethod(fileReader, record);
          }
        }

        fileReader.Close();
      }
    }

    private void SelectMatchedAndUnmatchedRecords(String[] filePaths, IStreamReaderFactory streamReaderFactory, IRecordReader recordReader, IRecordMatchExpression expression, Action<IStreamReader, Record> writeMatchedRecordMethod, Action<IStreamReader, Record> writeUnmatchedRecordMethod)
    {
      foreach (String filePath in filePaths)
      {
        this.logManager.WriteMessageToJobLog("Processing '" + filePath + "'.");
        IStreamReader fileReader = streamReaderFactory.CreateStreamReader(filePath);

        Record record;
        while (!expression.HasReachedMatchQuota && (record = recordReader.ReadRecord(fileReader)) != null)
        {
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
        }

        fileReader.Close();
      }
    }

    private void SelectUnmatchedRecordsOnly(String[] filePaths, IStreamReaderFactory streamReaderFactory, IRecordReader recordReader, IRecordMatchExpression expression, Action<IStreamReader, Record> writeUnmatchedRecordMethod)
    {
      foreach (String filePath in filePaths)
      {
        IStreamReader fileReader = streamReaderFactory.CreateStreamReader(filePath);

        Record record;
        while ((record = recordReader.ReadRecord(fileReader)) != null)
        {
          if (!expression.IsMatch(record))
          {
            writeUnmatchedRecordMethod(fileReader, record);
          }
        }

        fileReader.Close();
      }
    }
    #endregion
  }
}
