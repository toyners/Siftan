
namespace Siftan
{
  using System;
  using System.IO;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Object;

  public class OneFileRecordWriter : IRecordWriter
  {
    #region Fields
    private String matchedFilePath;

    private String unmatchedFilePath;

    private StreamWriter matchedWriter;

    private StreamWriter unmatchedWriter;

    private IStatisticsCollector statisticsCollector;

    public Boolean DoWriteMatchedRecords
    {
      get; private set;
    }

    public Boolean DoWriteUnmatchedRecords
    {
      get; private set;
    }
    #endregion

    #region Construction
    public OneFileRecordWriter(String matchedFilePath, String unmatchedFilePath, IStatisticsCollector statisticsCollector)
    {
      statisticsCollector.VerifyThatObjectIsNotNull("Parameter 'statisticsCollector' is null.");

      this.statisticsCollector = statisticsCollector;

      if (!String.IsNullOrEmpty(matchedFilePath))
      {
        this.matchedFilePath = matchedFilePath;
        this.DoWriteMatchedRecords = true;
      }

      if (!String.IsNullOrEmpty(unmatchedFilePath))
      {
        this.unmatchedFilePath = unmatchedFilePath;
        this.DoWriteUnmatchedRecords = true;
      }

      // TODO: Should check that the paths are legal.
    }
    #endregion

    #region Methods
    public void WriteMatchedRecord(IStreamReader reader, Record record)
    {
      reader.VerifyThatObjectIsNotNull("Cannot write matched record. Parameter 'reader' is null.");
      record.VerifyThatObjectIsNotNull("Cannot write matched record. Parameter 'record' is null.");

      if (!this.DoWriteMatchedRecords)
      {
        throw new InvalidOperationException("Writer not set to write out matched record.");
      }

      if (this.matchedWriter == null)
      {
        this.matchedWriter = new StreamWriter(this.matchedFilePath);
      }

      this.statisticsCollector.RecordWrittenToOutputFile(this.matchedFilePath);

      StreamWriteOperations.WriteRecordToStream(this.matchedWriter, reader, record);
    }

    public void WriteUnmatchedRecord(IStreamReader reader, Record record)
    {
      reader.VerifyThatObjectIsNotNull("Cannot write unmatched record. Parameter 'reader' is null.");
      record.VerifyThatObjectIsNotNull("Cannot write unmatched record. Parameter 'record' is null.");

      if (!this.DoWriteUnmatchedRecords)
      {
        throw new InvalidOperationException("Writer not set to write out unmatched record.");
      }

      if (this.unmatchedWriter == null)
      {
        this.unmatchedWriter = new StreamWriter(this.unmatchedFilePath);
      }

      this.statisticsCollector.RecordWrittenToOutputFile(this.unmatchedFilePath);

      StreamWriteOperations.WriteRecordToStream(this.unmatchedWriter, reader, record);
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
