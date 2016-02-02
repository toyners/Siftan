
namespace Siftan
{
  using System;
  using System.Collections.Generic;

  public class StatisticsManager : IStatisticsCollector, IStatisticsReporter
  {
    private Dictionary<String, InputFileCounter> inputFileCounters = new Dictionary<String, InputFileCounter>();

    private Dictionary<String, OutputFileCounter> outputFileCounters = new Dictionary<String, OutputFileCounter>();

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

    public void WriteToLog(ILogManager logManager)
    {
      // Enumerate over the input file counters and total up all the matched and unmatched totals.
      UInt32 matchedTotal = 0, unmatchedTotal = 0;
      PerformActionOverInputFileCounters((i) =>
      {
        matchedTotal += i.Matched;
        unmatchedTotal += i.Unmatched;
      });

      logManager.WriteImportantMessageToJobLog(String.Format("{0} Record(s) processed.", matchedTotal + unmatchedTotal));

      if (matchedTotal + unmatchedTotal == 0)
      {
        // No records processed so nothing more to report.
        return;
      }

      logManager.WriteImportantMessageToJobLog(String.Format("{0} Record(s) matched.", matchedTotal));
      logManager.WriteImportantMessageToJobLog(String.Format("{0} Record(s) not matched.", unmatchedTotal));

      // Enumerate over the input file counters and write out the individual totals to the log.
      PerformActionOverInputFileCounters((i) =>
      {
        logManager.WriteImportantMessageToJobLog(String.Format("{0} Record(s) processed from input file {1}.", i.Matched + i.Unmatched, i.FilePath));
        logManager.WriteImportantMessageToJobLog(String.Format("{0} Record(s) matched from input file {1}.", i.Matched, i.FilePath));
        logManager.WriteImportantMessageToJobLog(String.Format("{0} Record(s) not matched from input file {1}.", i.Unmatched, i.FilePath));
      });


      // Enumerate over the output file counters and write out the individual totals to the log.
      foreach (var outputFileCounter in this.outputFileCounters.Values)
      {
        logManager.WriteImportantMessageToJobLog(String.Format("{0} Record(s) written to output file {1}.", outputFileCounter.Total, outputFileCounter.FilePath));
      }
    }

    private void PerformActionOverInputFileCounters(Action<InputFileCounter> action)
    {
      foreach (var inputFileCounter in this.inputFileCounters.Values)
      {
        action(inputFileCounter);
      }
    }

    private void GetTotals(out UInt32 matchedTotal, out UInt32 unmatchedTotal)
    {
      matchedTotal = unmatchedTotal = 0;

      foreach (var inputFileCounter in this.inputFileCounters.Values)
      {
        matchedTotal += inputFileCounter.Matched;
        unmatchedTotal += inputFileCounter.Unmatched;
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

    private class InputFileCounter
    {
      public String FilePath { get; internal set; }

      public UInt32 Matched { get; internal set; }

      public UInt32 Unmatched { get; internal set; }
    }

    private class OutputFileCounter
    {
      public String FilePath { get; internal set; }

      public UInt32 Total { get; internal set; }
    }
  }
}
