
namespace Siftan
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.IO;

  public interface IRecordWriter
  {
    void WriteMatchedRecord(IStreamReader reader, Record record);

    void WriteUnmatchedRecord(IStreamReader reader, Record record);
  }

  public static class RecordWriteOperations
  {
    public static void WriteRecordToStream(StreamWriter writer, IStreamReader reader, Record record)
    {
      Int64 position = reader.Position;

      reader.Position = record.Start;
      while (reader.Position < record.End)
      {
        writer.WriteLine(reader.ReadLine());
      }

      reader.Position = position;
    }
  }

  // TODO: Better name
  public class RecordWriter
  {
    #region Fields
    private String matchedFilePath;

    private String unmatchedFilePath;

    private StreamWriter matchedWriter;

    private StreamWriter unmatchedWriter;
    #endregion

    #region Construction
    public RecordWriter(String matchedFilePath, String unmatchedFilePath)
    {
      this.matchedFilePath = matchedFilePath;
      this.unmatchedFilePath = unmatchedFilePath;

      // TODO: Should check that the paths are legal.
    }
    #endregion

    #region Methods
    public void WriteMatchedRecord(IStreamReader reader, Record record)
    {
      if (this.matchedWriter == null)
      {
        this.matchedWriter = new StreamWriter(this.matchedFilePath);
      }

      RecordWriteOperations.WriteRecordToStream(this.matchedWriter, reader, record);
    }

    public void WriteUnmatchedRecord(IStreamReader reader, Record record)
    {
      if (this.unmatchedWriter == null)
      {
        this.unmatchedWriter = new StreamWriter(this.unmatchedFilePath);
      }

      RecordWriteOperations.WriteRecordToStream(this.matchedWriter, reader, record);
    }

    public void Close()
    {
      if (this.matchedWriter != null)
      {
        this.matchedWriter.Close();
        this.matchedWriter = null;
      }

      if (this.unmatchedWriter != null)
      {
        this.unmatchedWriter.Close();
        this.unmatchedWriter = null;
      }
    }
    #endregion
  }
}
