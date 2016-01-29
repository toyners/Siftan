
namespace Siftan
{
  using System;

  public interface IStatisticsCollector
  {
    UInt32 TotalProcessedRecords { get; }

    UInt32 TotalMatchedRecords { get; }

    UInt32 TotalUnmatchedRecords { get; }

    void RecordIsMatched(String inputFilePath);
    
    void RecordIsUnmatched(String inputFilePath);

    void RecordWrittenToOutputFile(String matchedFilePath);
  }
}
