
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
  }
}
