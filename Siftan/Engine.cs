
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;

  public class Engine
  {
    #region Methods
    public void Execute(String[] filePaths, IRecordReader recordReader, IRecordMatchExpression expression, IRecordWriter recordWriter)
    {
      foreach (String filePath in filePaths)
      {
        FileReader fileReader = new FileReader(filePath);

        Record record;
        while (!expression.HasReachedMatchQuota && (record = recordReader.ReadRecord(fileReader)) != null)
        {
          if (expression.IsMatch(record))
          {
            recordWriter.WriteRecord(fileReader, record);
          }
          else if (recordWriter.Mode == RecordWriterModes.Unmatched)
          {
            recordWriter.WriteRecord(fileReader, record);
          }
        }

        fileReader.Close();
      }
    }
    #endregion
  }
}
