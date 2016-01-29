
namespace Siftan
{
  using System;
  using System.Collections.Generic;

  public class StatisticsCollector : IStatisticsCollector
  {
    private Dictionary<String, InputFileCounter> inputFileCounters = new Dictionary<String, InputFileCounter>();

    private Dictionary<String, OutputFileCounter> outputFileCounters = new Dictionary<String, OutputFileCounter>();

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

    public UInt32 TotalWrittenRecords
    {
      get
      {
        UInt32 result = 0;
        foreach (var outputFileCounter in this.OutputFileCounters())
        {
          result += outputFileCounter.Total;
        }

        return result;
      }
    }

    public void RecordIsMatched(String inputFilePath)
    {
      this.GetInputFileCounter(inputFilePath).Matched++;
    }

    public void RecordIsUnmatched(String inputFilePath)
    {
      this.GetInputFileCounter(inputFilePath).Unmatched++;
    }

    public void RecordWrittenToOutputFile(String filePath)
    {
      if (!this.outputFileCounters.ContainsKey(filePath))
      {
        this.outputFileCounters.Add(filePath, new OutputFileCounter { FilePath = filePath, Total = 1 });
        return;
      }

      this.outputFileCounters[filePath].Total++;
    }

    public IEnumerable<InputFileCounter> InputFileCounters()
    {
      foreach (var inputFileCounter in this.inputFileCounters.Values)
      {
        yield return inputFileCounter;
      }
    }

    public IEnumerable<OutputFileCounter> OutputFileCounters()
    {
      foreach (var outputFileCounter in this.outputFileCounters.Values)
      {
        yield return outputFileCounter;
      }
    }

    private InputFileCounter GetInputFileCounter(String inputFilePath)
    {
      if (!this.inputFileCounters.ContainsKey(inputFilePath))
      {
        var inputFileCounter = new InputFileCounter { FilePath = inputFilePath };
        this.inputFileCounters.Add(inputFilePath, inputFileCounter);
        return inputFileCounter;
      }

      return this.inputFileCounters[inputFilePath];
    }

    public class InputFileCounter
    {
      public String FilePath { get; internal set; }

      public UInt32 Matched { get; internal set; }

      public UInt32 Unmatched { get; internal set; }
    }

    public class OutputFileCounter
    {
      public String FilePath { get; internal set; }

      public UInt32 Total { get; internal set; }
    }
  }
}
