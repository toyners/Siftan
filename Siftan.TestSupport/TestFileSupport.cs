
namespace Siftan.TestSupport
{
  using System;
  using System.IO;
  using Shouldly;

  /// <summary>
  /// Provides support methods for files created or needed by tests.
  /// </summary>
  public static class TestFileSupport
  {
    /// <summary>
    /// Check that the file exists and that it contains the expected file lines.
    /// </summary>
    /// <param name="filePath">Full path of file to check.</param>
    /// <param name="expectedFileLines">Lines expected to be found in file.</param>
    public static void AssertFileIsCorrect(String filePath, String[] expectedFileLines)
    {
      File.Exists(filePath).ShouldBeTrue(String.Format("File '{0}' does not exist", filePath));
      StringArrayComparison.IsMatching(File.ReadAllLines(filePath), expectedFileLines);
    }
  }
}
