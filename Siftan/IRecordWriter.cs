
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;

  public interface IRecordWriter
  {
    Boolean DoWriteMatchedRecords { get; }

    Boolean DoWriteUnmatchedRecords { get; }

    void WriteMatchedRecord(IStreamReader reader, Record record);

    void WriteUnmatchedRecord(IStreamReader reader, Record record);

    void Close();
  }

  public interface IRecordWriter2
  {
    void WriteMatchedRecord(IRecordSource recordSource);

    void WriteUnmatchedRecord(IRecordSource recordSource);
  }
}
