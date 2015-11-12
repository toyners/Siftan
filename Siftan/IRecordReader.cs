
namespace Siftan
{
  using Jabberwocky.Toolkit.IO;

  public interface IRecordReader
  {
    #region Methods
    Record ReadRecord(IStreamReader reader);
    #endregion
  }
}
