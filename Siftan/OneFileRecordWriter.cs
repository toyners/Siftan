
namespace Siftan
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.IO;

  public class OneFileRecordWriter
  {
    #region Fields
    private String matchedFilePath;

    private String unmatchedFilePath;

    private StreamWriter matchedWriter;

    private StreamWriter unmatchedWriter;
    #endregion

    #region Construction
    public OneFileRecordWriter(String matchedFilePath, String unmatchedFilePath)
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

      StreamWriteOperations.WriteRecordToStream(this.matchedWriter, reader, record);
    }

    public void WriteUnmatchedRecord(IStreamReader reader, Record record)
    {
      if (this.unmatchedWriter == null)
      {
        this.unmatchedWriter = new StreamWriter(this.unmatchedFilePath);
      }

      StreamWriteOperations.WriteRecordToStream(this.matchedWriter, reader, record);
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
