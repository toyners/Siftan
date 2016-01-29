
namespace Siftan
{
  using System;

  public interface IStatisticsCollector
  {
    void RecordIsMatched(String inputFilePath);
    
    void RecordIsUnmatched(String inputFilePath);

    void RecordWrittenToOutputFile(String matchedFilePath);
  }
}
