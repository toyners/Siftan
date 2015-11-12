
namespace Siftan
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.IO;

  public enum RecordWriterModes
  {
    OutputFile,
    InputFile,
    TermFile,
    Unmatched
  }

  public interface IRecordWriter
  {
    RecordWriterModes Mode { get; set; }

    String OutputDirectory { get; set; }

    String InputFile { get;  set; }

    void WriteRecord(IStreamReader reader, Record record);
  }

  public class RecordWriter : IRecordWriter
  {
    #region Fields
    private StreamWriter writer;

    private RecordWriterModes mode;
    #endregion

    #region Properties
    public Boolean IsOpen
    {
      get { return this.writer != null; }
    }

    public RecordWriterModes Mode
    {
      get { return this.mode; }
      set
      {
        this.mode = value;
      }
    }

    public String OutputDirectory
    {
      get; set;
    }

    public String InputFile
    {
      get; set;
    }
    #endregion

    #region Methods
    public void WriteRecord(IStreamReader reader, Record record)
    {
      if (!this.IsOpen)
      {
        throw new Exception("Writer is not open.");
      }

      this.WriteRecordToFile(reader, record);
    }

    public void Close()
    {
      if (!this.IsOpen)
      {
        return;
      }

      this.writer.Close();
      this.writer = null;
    }

    private void WriteRecordToFile(IStreamReader reader, Record record)
    {
      Int64 position = reader.Position;

      reader.Position = record.Start;
      while (reader.Position < record.End)
      {
        this.writer.WriteLine(reader.ReadLine());
      }

      reader.Position = position;
    }
    #endregion
  }
}
