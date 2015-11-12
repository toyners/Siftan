
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.String;
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
      while (!this.streamReader.EndOfStream)
      {
        Int64 position = this.streamReader.Position;
        String line = this.streamReader.ReadLine();
        String lineIDTerm = line.ExtractField(descriptor.Delimiter, descriptor.Qualifier, descriptor.LineIDIndex);

        if (lineIDTerm != descriptor.HeaderID)
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
}
