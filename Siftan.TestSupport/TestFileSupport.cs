
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
    public static void AssertFileIsCorrect(String filePath, String[] expectedFileLines)
    {
      File.Exists(filePath).ShouldBeTrue(String.Format("File '{0}' does not exist", filePath));
      StringArrayComparison.IsMatching(File.ReadAllLines(filePath), expectedFileLines);
    }
  }
}
