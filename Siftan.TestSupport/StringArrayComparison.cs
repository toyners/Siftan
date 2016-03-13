
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

        for (Int32 actualIndex = 0; actualIndex < actualLines.Length; actualIndex++)
        {
          String actualLine = actualLines[actualIndex];
          if (Regex.IsMatch(actualLine, expectedLine))
          {
            if (lastMatchIndex > actualIndex)
            {
              throw new Exception(String.Format("Expected line '{0}' to follow '{1}' but '{1}' follows '{0}'.",
                actualLine,
                actualLines[lastMatchIndex]));
            }

            matched = true;
            lastMatchIndex = actualIndex;
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
