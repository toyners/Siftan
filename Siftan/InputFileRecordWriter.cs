
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;

  public class InputFileRecordWriter : IRecordWriter
  {
    public Boolean DoWriteMatchedRecords
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public Boolean DoWriteUnmatchedRecords
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public void Close()
    {
      throw new NotImplementedException();
    }

    public void WriteMatchedRecord(IStreamReader reader, Record record)
    {
      throw new NotImplementedException();
    }

    public void WriteUnmatchedRecord(IStreamReader reader, Record record)
    {
      throw new NotImplementedException();
    }
  }
}
