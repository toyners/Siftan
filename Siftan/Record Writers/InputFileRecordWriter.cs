
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Object;

  public class InputFileRecordWriter : IRecordWriter
  {
    #region Fields
    private readonly IStatisticsCollector statisticsCollector;

    private readonly Dictionary<String, StreamWriter> writers = new Dictionary<String, StreamWriter>();
    #endregion

    #region Construction
    public InputFileRecordWriter(IStatisticsCollector statisticsCollector, Boolean doWriteMatchedRecords, Boolean doWriteUnmatchedRecords)
    {
      this.statisticsCollector = statisticsCollector;
      this.DoWriteMatchedRecords = doWriteMatchedRecords;
      this.DoWriteUnmatchedRecords = doWriteUnmatchedRecords;
    }
    #endregion

    #region Properties
    public Boolean DoWriteMatchedRecords
    {
      get; private set;
    }

    public Boolean DoWriteUnmatchedRecords
    {
      get; private set;
    }
    #endregion

    #region Methods
    public void Close()
    {
      foreach (var writer in this.writers.Values)
      {
        writer.Close();
      }
    }

    public void WriteMatchedRecord(IStreamReader reader, Record record)
    {
      WriteRecordsToFile(reader, record, "Matched_From_");
    }

    public void WriteUnmatchedRecord(IStreamReader reader, Record record)
    {
      WriteRecordsToFile(reader, record, "Unmatched_From_");
    }

    private void WriteRecordsToFile(IStreamReader reader, Record record, String outputPrefix)
    {
      reader.VerifyThatObjectIsNotNull("Cannot write record. Parameter 'reader' is null.");
      record.VerifyThatObjectIsNotNull("Cannot write record. Parameter 'record' is null.");

      StreamWriter writer;
      String outputName = Path.GetDirectoryName(reader.Name) + @"\" + outputPrefix + Path.GetFileName(reader.Name);
      if (!writers.ContainsKey(outputName))
      {
        writer = new StreamWriter(outputName);
        writers.Add(outputName, writer);
      }
      else
      {
        writer = writers[outputName];
      }

      this.statisticsCollector.RecordWrittenToOutputFile(outputName);

      StreamWriteOperations.WriteRecordToStream(writer, reader, record);
    }
    #endregion
  }
}
