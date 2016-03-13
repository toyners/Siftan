
namespace Siftan
{
  using System;

  public interface IStatisticsCollector
  {
    /// <summary>
    /// Resets the statistics in the collector.
    /// </summary>
    void Reset();

    void RecordIsMatched(String inputFilePath);
    
    void RecordIsUnmatched(String inputFilePath);

    void RecordWrittenToOutputFile(String matchedFilePath);
  }
}
