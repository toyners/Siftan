
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class DelimitedRecordSource : IRecordSource
  {
    private DelimitedRecordDescriptor descriptor;
    private FileStream stream;

    public DelimitedRecordSource(DelimitedRecordDescriptor descriptor, String filePath)
    {
      this.descriptor = descriptor;
      //this.stream = new FileStream()
    }

    public Int64 GetRecordCount
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public Boolean GotRecord
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public void Close()
    {
      throw new NotImplementedException();
    }

    public Boolean GetRecordData(Byte[] buffer, out Int64 bytesRead)
    {
      throw new NotImplementedException();
    }

    public Boolean MoveToNextRecord()
    {
      throw new NotImplementedException();
    }

    public Boolean MoveToRecord(Int64 index)
    {
      throw new NotImplementedException();
    }
  }
}
