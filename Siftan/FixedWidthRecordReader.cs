
namespace Siftan
{
  using System;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Object;

  public class FixedWidthRecordReader :IRecordReader
  {
    #region Fields
    private FixedWidthRecordDescriptor descriptor;
    #endregion

    #region Construction
    public FixedWidthRecordReader(FixedWidthRecordDescriptor descriptor)
    {
      descriptor.VerifyThatObjectIsNotNull("Parameter 'descriptor' is null.");
      this.descriptor = descriptor;
    }
    #endregion

    #region Methods
    public Record ReadRecord(IStreamReader reader)
    {
      Record record = null;
      Int32 lineIDStart = (Int32)this.descriptor.LineIDStart;
      Int32 lineIDLength = (Int32)this.descriptor.LineIDLength;
      Int32 termStart = (Int32)this.descriptor.Term.Start;
      Int32 termLength = (Int32)this.descriptor.Term.Length;

      while (!reader.EndOfStream)
      {
        Int64 position = reader.Position;
        String line = reader.ReadLine();
        String lineID = line.Substring(lineIDStart, lineIDLength);

        if (lineID == descriptor.HeaderID)
        {
          if (record == null)
          {
            record = new Record { Start = position };
          }
          else
          {
            record.End = position;
            reader.Position = position;
            return record;
          }
        }

        if (lineID == descriptor.Term.LineID)
        {
          record.Term = line.Substring(termStart, termLength);
        } 
      }

      if (record != null)
      {
        record.End = reader.Position;
      }

      return record;
    }
    #endregion
  }
}
