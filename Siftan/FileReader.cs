
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class FileReader : IStreamReader
  {
    #region Fields
    private Boolean disposed;

    private StreamReader reader;
    #endregion

    #region Construction
    public FileReader(String path)
    {
      this.reader = new StreamReader(path);
    }
    #endregion

    #region Properties
    public Boolean EndOfStream
    {
      get 
      { 
        return this.reader.EndOfStream;
      }
    }
    #endregion

    #region Methods
    public String ReadLine()
    {
      return this.reader.ReadLine();
    }

    public void Close()
    {
      this.Dispose(true);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(Boolean disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (disposing)
      {
        if (this.reader != null)
        {
          this.reader.Dispose();
          this.reader = null;
        }
      }

      this.disposed = true;
    }
    #endregion
  }
}
