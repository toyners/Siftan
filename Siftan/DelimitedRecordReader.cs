
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.String;
  using Jabberwocky.Toolkit.Object;

  public class DelimitedRecordReader : IRecordReader
  {
    #region Fields
    private DelimitedRecordDescriptor descriptor;
    #endregion

    #region Construction
    public DelimitedRecordReader(DelimitedRecordDescriptor descriptor)
    {
      descriptor.VerifyThatObjectIsNotNull("Parameter 'descriptor' is null.");
      this.descriptor = descriptor;
    }
    #endregion

    #region Methods
    public Record ReadRecord(IStreamReader streamReader)
    {
      Record record = null;
      while (!streamReader.EndOfStream)
      {
        Int64 position = streamReader.Position;
        var line = streamReader.ReadLine();
        var lineIDTerm = line.ExtractField(this.descriptor.Delimiter, this.descriptor.Qualifier, this.descriptor.LineIDIndex);

        if (lineIDTerm == this.descriptor.HeaderID)
        {
          if (record == null)
          {
            record = new Record { Start = position };
          }
          else
          {
            record.End = position;
            streamReader.Position = position;
            return record;
          }
        }

        if (lineIDTerm != this.descriptor.Term.LineID)
        {
          continue;
        }

        record.Term = line.ExtractField(this.descriptor.Delimiter, this.descriptor.Qualifier, this.descriptor.Term.Index);
      }

      if (record != null)
      {
        record.End = streamReader.Position;
      }

      return record;
    }
    #endregion
  }
}
