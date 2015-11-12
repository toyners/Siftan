
namespace Siftan
{
  public interface IRecordWriter
  {
    void WriteRecord(Record record);
  }

  public delegate void WriteRecordDelegate(Record record);
}
