
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
      String lastMatchLine;

      foreach (String expectedLine in expectedLines)
      {
        Boolean matched = false;

        for (Int32 logIndex = 0; logIndex < actualLines.Length; logIndex++)
        {
          String actualLine = actualLines[logIndex];
          if (Regex.IsMatch(actualLine, expectedLine))
          {
            if (lastMatchIndex > logIndex)
            {
              throw new Exception(String.Format("Expected line '{0}' to follow '{1}' but '{1}' follows '{0}'.",
                actualLine,
                actualLines[lastMatchIndex]));
            }

            matched = true;
            lastMatchIndex = logIndex;
            lastMatchLine = actualLine;
            break;
          }
        }

        if (!matched)
        {
          throw new Exception(String.Format("Missing line '{0}' from array.", expectedLine));
        }
      }
    }
  }
}
