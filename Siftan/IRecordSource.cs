
namespace Siftan
{
  using System;

  public interface IRecordSource
  {
    Int64 GetRecordCount { get; }

    Boolean GotRecord { get; }

    void Close();

    Boolean GetRecordData(Byte[] buffer, out Int64 bytesRead);

    Boolean MoveToNextRecord();

    Boolean MoveToRecord(Int64 index);
  }

  public interface IRecordSourceFactory
  {
    IRecordSource CreateSource(String key);
  }
}
