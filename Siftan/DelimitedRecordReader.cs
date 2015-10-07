
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class DelimitedRecordReader : IRecordReader, IDisposable
  {
    #region Fields
    private IStreamReader streamReader;

    private DelimitedRecordDescriptor descriptor;
    #endregion

    #region Construction
    public DelimitedRecordReader(String path, DelimitedRecordDescriptor descriptor)
    {
      this.streamReader = new FileReader(path);
      this.descriptor = descriptor;
    }

    public DelimitedRecordReader(IStreamReader streamReader)
    {
      this.streamReader = streamReader;
    }
    #endregion

    #region Methods
    public IRecord ReadRecord()
    {
      throw new NotImplementedException();
    }

    public void Close()
    {
      if (this.streamReader == null)
      {
        return;
      }

      this.streamReader.Close();
      this.streamReader = null;
    }

    public void Dispose()
    {
      this.Close();
    }
    #endregion
  }

  public class DelimitedRecordDescriptor
  {
    public String Delimiter;

    public String Qualifier;

    public String HeaderTerm;

    public Int32 HeaderTermIndex;
  }
}
