
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.String;
  using Jabberwocky.Toolkit.Validation;

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
        String line = streamReader.ReadLine();
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
        record.End = streamReader.Position;
      }

      return record;
    }
    #endregion
  }
}
