
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;

  public class Engine
  {
    #region Methods
    public void Execute(String[] filePaths, IRecordReader recordReader, IRecordMatchExpression expression, Action<IStreamReader, Record> writeMatchedRecordMethod, Action<IStreamReader, Record> writeUnmatchedRecordMethod)
    {
      if (writeMatchedRecordMethod != null && writeUnmatchedRecordMethod != null)
      {
        this.SelectMatchedAndUnmatchedRecords(filePaths, recordReader, expression, writeMatchedRecordMethod, writeUnmatchedRecordMethod);
      }
      else if (writeMatchedRecordMethod != null)
      {
        this.SelectMatchedRecordsOnly(filePaths, recordReader, expression, writeMatchedRecordMethod);
      }
      else if (writeUnmatchedRecordMethod != null)
      {
        this.SelectUnmatchedRecordsOnly(filePaths, recordReader, expression, writeUnmatchedRecordMethod);
      }
      else
      {
        throw new Exception("No write methods passed in."); // TODO - better message.
      }
    }

    private void SelectMatchedRecordsOnly(String[] filePaths, IRecordReader recordReader, IRecordMatchExpression expression, Action<IStreamReader, Record> writeRecordMethod)
    {
      foreach (String filePath in filePaths)
      {
        FileReader fileReader = new FileReader(filePath);

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

    private void SelectMatchedAndUnmatchedRecords(String[] filePaths, IRecordReader recordReader, IRecordMatchExpression expression, Action<IStreamReader, Record> writeMatchedRecordMethod, Action<IStreamReader, Record> writeUnmatchedRecordMethod)
    {
      foreach (String filePath in filePaths)
      {
        FileReader fileReader = new FileReader(filePath);

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
      }
    }

    private void SelectUnmatchedRecordsOnly(String[] filePaths, IRecordReader recordReader, IRecordMatchExpression expression, Action<IStreamReader, Record> writeUnmatchedRecordMethod)
    {
      foreach (String filePath in filePaths)
      {
        FileReader fileReader = new FileReader(filePath);
        Record record;
        while ((record = recordReader.ReadRecord(fileReader)) != null)
        {
          if (!expression.IsMatch(record))
          {
            writeUnmatchedRecordMethod(fileReader, record);
          }
        }
      }
    }
    #endregion
  }
}
