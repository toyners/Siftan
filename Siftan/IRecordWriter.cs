
namespace Siftan
{
  using Jabberwocky.Toolkit.IO;

  public interface IRecordWriter
  {
    RecordCategory Categories { get; }

    void WriteMatchedRecord(IStreamReader reader, Record record);

    void WriteUnmatchedRecord(IStreamReader reader, Record record);

    void Close();
  }
}
