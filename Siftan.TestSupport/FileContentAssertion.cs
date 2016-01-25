
namespace Siftan.TestSupport
{
  using System;
  using System.Text.RegularExpressions;

  public static class FileContentAssertion
  {
    public static void IsMatching(String[] fileLines, String[] expectedFileLines)
    {
      try
      {
        Int32 lastMatchIndex = -1;
        String lastMatchLine;

        foreach (String expectedLogFileLine in expectedFileLines)
        {
          Boolean matched = false;

          for (Int32 logIndex = 0; logIndex < fileLines.Length; logIndex++)
          {
            String logFileLine = fileLines[logIndex];
            if (Regex.IsMatch(logFileLine, expectedLogFileLine))
            {
              if (lastMatchIndex > logIndex)
              {
                throw new Exception(String.Format("Expected file content lines '{0}' to follow '{1}' but '{1}' follows '{0}'.",
                  logFileLine,
                  fileLines[lastMatchIndex]));
              }

              matched = true;
              lastMatchIndex = logIndex;
              lastMatchLine = logFileLine;
              break;
            }
          }

          if (!matched)
          {
            throw new Exception(String.Format("Missing line '{0}' from file content.", expectedLogFileLine));
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
