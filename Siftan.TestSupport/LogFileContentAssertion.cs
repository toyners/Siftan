
namespace Siftan.TestSupport
{
  using System;
  using System.Text.RegularExpressions;
  using Shouldly;

  public static class LogFileContentAssertion
  {
    public static void IsMatch(String[] logFileLines, String[] expectedLogFileLines)
    {
      logFileLines.Length.ShouldBe(expectedLogFileLines.Length);
      for (Int32 index = 0; index < logFileLines.Length; index++)
      {
        Regex.IsMatch(logFileLines[index], expectedLogFileLines[index]).ShouldBeTrue();
      }
    }

    public static void IsMatching(String[] logFileLines, String[] expectedLogFileLines)
    {
      try
      {
        Int32 lastMatchIndex = -1;
        String lastMatchLine;

        foreach (String expectedLogFileLine in expectedLogFileLines)
        {
          Boolean matched = false;

          for (Int32 logIndex = 0; logIndex < logFileLines.Length; logIndex++)
          {
            if (Regex.IsMatch(logFileLines[logIndex], expectedLogFileLine))
            {
              String logFileLine = logFileLines[logIndex];
              if (lastMatchIndex > logIndex)
              {
                throw new Exception(String.Format("Expected log file content lines '{0}' to follow '{1}' but '{1}' follows '{0}'.",
                  logFileLine,
                  logFileLines[lastMatchIndex]));
              }

              matched = true;
              lastMatchIndex = logIndex;
              lastMatchLine = logFileLine;
              break;
            }
          }

          if (!matched)
          {
            throw new Exception(String.Format("Missing line '{0}' from log file content.", expectedLogFileLine));
          }
        }
      }
      catch (Exception exception)
      {
        throw new Exception(exception.Message);
      }
    }
  }
}
