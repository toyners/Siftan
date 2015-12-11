
namespace Siftan
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.IO;

  [Flags]
  public enum RecordCategory
  {
    Matched = 1,
    Unmatched
  }

  public interface IRecordWriter
  {
    RecordCategory Categories { get; }

    void WriteMatchedRecord(IStreamReader reader, Record record);

    void WriteUnmatchedRecord(IStreamReader reader, Record record);

    void Close();
  }

  public class Engine
  {
    #region Methods
    public void Execute(
      String[] filePaths, 
      String logFilePath,
      IStreamReaderFactory streamReaderFactory, 
      IRecordReader recordReader, 
      IRecordMatchExpression expression, 
      IRecordWriter recordWriter)
    {
      if (recordWriter.Categories < RecordCategory.Matched || recordWriter.Categories >= (RecordCategory)((Int32)RecordCategory.Unmatched << (Int32)RecordCategory.Matched))
      {
        throw new Exception(String.Format("IRecordWriter.Categories must return a valid value from RecordCategory enum. Value returned was {0}.", recordWriter.Categories));
      }

      using (StreamWriter log = new StreamWriter(logFilePath))
      {
        log.WriteLine("[" + DateTime.Now.ToString("dd-MM-yy HH:mm:ss") + "] Starting...");

        if ((recordWriter.Categories & (RecordCategory.Matched | RecordCategory.Unmatched)) == (RecordCategory.Matched | RecordCategory.Unmatched))
        {
          this.SelectMatchedAndUnmatchedRecords(filePaths, streamReaderFactory, recordReader, expression, recordWriter.WriteMatchedRecord, recordWriter.WriteUnmatchedRecord);
        }
        else if ((recordWriter.Categories & RecordCategory.Matched) == RecordCategory.Matched)
        {
          this.SelectMatchedRecordsOnly(filePaths, streamReaderFactory, recordReader, expression, recordWriter.WriteMatchedRecord);
        }
        else if ((recordWriter.Categories & RecordCategory.Unmatched) == RecordCategory.Unmatched)
        {
          this.SelectUnmatchedRecordsOnly(filePaths, streamReaderFactory, recordReader, expression, recordWriter.WriteUnmatchedRecord);
        }

        recordWriter.Close();

        log.WriteLine("[" + DateTime.Now.ToString("dd-MM-yy HH:mm:ss") + "] Finished.");
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
        IStreamReader fileReader = streamReaderFactory.CreateStreamReader(filePath);

        Record record;
        while (!expression.HasReachedMatchQuota && (record = recordReader.ReadRecord(fileReader)) != null)
        {
          if (expression.IsMatch(record))
          {
            writeMatchedRecordMethod(fileReader, record);
          }
          else
          {
            writeUnmatchedRecordMethod(fileReader, record);
          }
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
