
namespace Siftan.TestSupport
{
  using System;
  using System.Text.RegularExpressions;

  /// <summary>
  /// Provides a method for comparing two string arrays using regular expressions.
  /// </summary>
  public static class StringArrayComparison
  {
    public static void IsMatching(String[] actualLines, String[] expectedLines)
    {
      Int32 lastMatchIndex = -1;

      foreach (String expectedLine in expectedLines)
      {
        if (!ScanForwardForMatch(expectedLine, actualLines, ref lastMatchIndex))
        {
          CheckForPriorMatch(expectedLine, actualLines, lastMatchIndex);
          throw new Exception(String.Format("Missing line '{0}' from array.", expectedLine));
        }
      }
    }

    private static Boolean ScanForwardForMatch(String expectedLine, String[] actualLines, ref Int32 lastMatchIndex)
    {
      for (Int32 actualIndex = lastMatchIndex + 1; actualIndex < actualLines.Length; actualIndex++)
      {
        String actualLine = actualLines[actualIndex];
        if (Regex.IsMatch(actualLine, expectedLine))
        {
          lastMatchIndex = actualIndex;
          return true;
        }
      }

      return false;
    }

    private static void CheckForPriorMatch(String expectedLine, String[] actualLines, Int32 lastMatchIndex)
    {
      for (Int32 actualIndex = lastMatchIndex - 1; actualIndex >= 0; actualIndex--)
      {
        String actualLine = actualLines[actualIndex];
        if (Regex.IsMatch(actualLine, expectedLine))
        {
          throw new Exception(String.Format("Expected line '{0}' to follow '{1}' but '{1}' follows '{0}'.",
              actualLine,
              actualLines[lastMatchIndex]));
        }
      }
    }
  }
}
