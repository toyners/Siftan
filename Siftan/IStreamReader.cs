
namespace Siftan
{
  using System;

  public interface IStreamReader : IDisposable
  {
    #region Properties
    Boolean EndOfStream { get; }
    #endregion

    #region Methods
    String ReadLine();

    void Close();
    #endregion
  }
}
