
namespace Siftan
{
  using System;

  public interface IRecordSource
  {
    Int64 GetRecordCount { get; }

    Boolean MoveToNextRecord();

    Boolean MoveToRecord(Int64 index);

    Boolean GetRecordData(Byte[] buffer, out Int64 bytesRead);
  }
}
