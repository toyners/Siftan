
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;

  public interface IRecordReader
  {
    #region Methods
    Record ReadRecord(IStreamReader reader);
    #endregion
  }

  public interface IRecordSource
  {
    Int64 GetRecordCount { get; }

    Boolean MoveToNextRecord();

    Boolean MoveToRecord(Int64 index);

    Boolean GetRecordData(Byte[] buffer, out Int64 bytesRead);
  }
}
