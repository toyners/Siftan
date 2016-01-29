
namespace Siftan
{
  using System;
  using System.Collections.Generic;

  public class StatisticsCollector : IStatisticsCollector
  {
    private Dictionary<String, InputFileCounter> inputFileCounters = new Dictionary<String, InputFileCounter>();

    private Dictionary<String, UInt32> outputFileCounts = new Dictionary<String, UInt32>();

    public UInt32 TotalProcessedRecords
    {
      get
      {
        UInt32 result = 0;
        foreach (var inputFileCounter in this.InputFileCounters())
        {
          result += inputFileCounter.Matched + inputFileCounter.Unmatched;
        }

        return result;
      }
    }

    public UInt32 TotalMatchedRecords
    {
      get
      {
        UInt32 result = 0;
        foreach (var inputFileCounter in this.InputFileCounters())
        {
          result += inputFileCounter.Matched;
        }

        return result;
      }
    }

    public UInt32 TotalUnmatchedRecords
    {
      get
      {
        UInt32 result = 0;
        foreach (var inputFileCounter in this.InputFileCounters())
        {
          result += inputFileCounter.Unmatched;
        }

        return result;
      }
    }

    public void RecordIsMatched(String inputFilePath)
    {
      this.GetInputFileCountsStructure(inputFilePath).Matched++;
    }

    public void RecordIsUnmatched(String inputFilePath)
    {
      this.GetInputFileCountsStructure(inputFilePath).Unmatched++;
    }

    public void RecordWrittenToOutputFile(String filePath)
    {
      if (!this.outputFileCounts.ContainsKey(filePath))
      {
        this.outputFileCounts.Add(filePath, 1);
        return;
      }

      this.outputFileCounts[filePath]++;
    }

    public IEnumerable<InputFileCounter> InputFileCounters()
    {
      foreach (var inputFileCounter in this.inputFileCounters.Values)
      {
        yield return inputFileCounter;
      }
    }

    private InputFileCounter GetInputFileCountsStructure(String inputFilePath)
    {
      InputFileCounter i;
      if (!this.inputFileCounters.ContainsKey(inputFilePath))
      {
        i = new InputFileCounter { FilePath = inputFilePath };
        this.inputFileCounters.Add(inputFilePath, i);
        return i;
      }

      return this.inputFileCounters[inputFilePath];
    }

    public class InputFileCounter
    {
      public String FilePath { get; internal set; }

      public UInt32 Matched { get; internal set; }

      public UInt32 Unmatched { get; internal set; }
    }
  }
}
