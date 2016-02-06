
namespace Siftan.TestSupport
{
  using System;
  using System.IO;
  using System.Reflection;
  using Jabberwocky.Toolkit.Assembly;
  using Shouldly;

  /// <summary>
  /// Provides support methods for files created or needed by tests.
  /// </summary>
  public static class TestFileSupport
  {
    public static void CreateFile(String resourceFilePath, String inputFilePath)
    {
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFilePath, inputFilePath);
    }

    public static void AssertFileIsCorrect(String filePath, String[] expectedFileLines)
    {
      File.Exists(filePath).ShouldBeTrue(String.Format("File '{0}' does not exist", filePath));
      StringArrayComparison.IsMatching(File.ReadAllLines(filePath), expectedFileLines);
    }
  }
}
