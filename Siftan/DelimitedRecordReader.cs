
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Validation;

  public class DelimitedRecordReader : IRecordReader, IDisposable
  {
    #region Fields
    private IStreamReader streamReader;

    private DelimitedRecordDescriptor descriptor;
    #endregion

    #region Construction
    public DelimitedRecordReader(String path, DelimitedRecordDescriptor descriptor)
    {
      path.VerifyThatStringIsNotNullAndNotEmpty("Parameter 'path' is null or empty.");
      descriptor.VerifyThatObjectIsNotNull("Parameter 'descriptor' is null.");

      this.streamReader = new FileReader(path);
      this.descriptor = descriptor;
    }

    public DelimitedRecordReader(IStreamReader streamReader, DelimitedRecordDescriptor descriptor)
    {
      streamReader.VerifyThatObjectIsNotNull("Parameter 'streamReader' is null.");
      descriptor.VerifyThatObjectIsNotNull("Parameter 'descriptor' is null.");

      this.streamReader = streamReader;
      this.descriptor = descriptor;
    }
    #endregion

    #region Methods
    public Record ReadRecord()
    {
      Record record = null;
      String[] seperator = new [] { descriptor.Delimiter };
      while (!this.streamReader.EndOfStream)
      {
        Int64 position = this.streamReader.Position;
        String line = this.streamReader.ReadLine();

        // + 2 means that the line term index will be second to last in the array e.g. given "H,A,B,C" 
        // with line term index of 0 means setting maximum parameter to 2 (returning "H" and "A,B,C")
        // to get the 0th term ("H")  
        String[] terms = line.Split(seperator, descriptor.LineTermIndex + 2, StringSplitOptions.None);

        if (terms[descriptor.LineTermIndex] != descriptor.HeaderTerm)
        {
          continue;
        }

        if (record == null)
        {
          record = new Record { Start = position };
          continue;
        }

        record.End = position;
        return record;
      };

      if (record != null)
      {
        record.End = this.streamReader.Position;
      }

      return record;
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

    public Int32 LineTermIndex;
  }
}
