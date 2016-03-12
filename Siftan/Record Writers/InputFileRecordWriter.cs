
namespace Siftan
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Jabberwocky.Toolkit.IO;

  public class InputFileRecordWriter : IRecordWriter
  {
    private IStatisticsCollector statisticsCollector;

    private Dictionary<String, StreamWriter> writers = new Dictionary<String, StreamWriter>();

    public InputFileRecordWriter(IStatisticsCollector statisticsCollector, Boolean doWriteMatchedRecords, Boolean doWriteUnmatchedRecords)
    {
      this.statisticsCollector = statisticsCollector;
      this.DoWriteMatchedRecords = doWriteMatchedRecords;
      this.DoWriteUnmatchedRecords = doWriteUnmatchedRecords;
    }

    public Boolean DoWriteMatchedRecords
    {
      get; private set;
    }

    public Boolean DoWriteUnmatchedRecords
    {
      get; private set;
    }

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
      StreamWriter writer;
      if (!writers.ContainsKey(reader.Name))
      {
        String outputName = Path.GetDirectoryName(reader.Name) + @"\" + outputPrefix + Path.GetFileName(reader.Name);
        writer = new StreamWriter(outputName);
        writers.Add(reader.Name, writer);
      }
      else
      {
        writer = writers[reader.Name];
      }

      StreamWriteOperations.WriteRecordToStream(writer, reader, record);
    }
  }
}
