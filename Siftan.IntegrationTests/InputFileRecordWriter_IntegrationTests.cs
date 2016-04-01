
namespace Siftan.IntegrationTests
{
  using System;
  using System.IO;
  using FluentAssertions;
  using Jabberwocky.Toolkit.IO;
  using NSubstitute;
  using NUnit.Framework;
  using Shouldly;
  using TestSupport;

  [TestFixture]
  public class InputFileRecordWriter_IntegrationTests
  {
    private String workingDirectory;

    [TestFixtureSetUp]
    public void SetupBeforeAllTests()
    {
      this.workingDirectory = Path.GetTempPath() + @"InputFileRecordWriter_IntegrationTests\";
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
      DirectoryOperations.EnsureDirectoryIsEmpty(this.workingDirectory);
    }

    [Test]
    public void RecordsFromMultipleInputFilesWrittenToCorrespondingOutputFiles()
    {
      // Arrange
      File.WriteAllLines(this.workingDirectory + "FileA.txt", new String[] { "a", "b", "c", "d", "e", "f" });
      File.WriteAllLines(this.workingDirectory + "FileB.txt", new String[] { "g", "h", "i", "j", "k", "l" });

      var mockStatisticsCollector = Substitute.For<IStatisticsCollector>();
      InputFileRecordWriter writer = new InputFileRecordWriter(mockStatisticsCollector, true, true);
      using (FileReader inputFileA = new FileReader(this.workingDirectory + "FileA.txt"))
      {
        using (FileReader inputFileB = new FileReader(this.workingDirectory + "FileB.txt"))
        {
          // Act
          writer.WriteMatchedRecord(inputFileA, new Record { Start = 0, End = 8 });
          writer.WriteMatchedRecord(inputFileB, new Record { Start = 0, End = 8 });
          writer.WriteUnmatchedRecord(inputFileA, new Record { Start = 9, End = 18 });
          writer.WriteUnmatchedRecord(inputFileB, new Record { Start = 9, End = 18 });
          writer.Close();
        }
      }

      // Assert
      File.Exists(this.workingDirectory + "Matched_From_FileA.txt").ShouldBeTrue();
      File.ReadAllLines(this.workingDirectory + "Matched_From_FileA.txt").ShouldBeEquivalentTo(new String[] { "a", "b", "c" });

      File.Exists(this.workingDirectory + "Matched_From_FileB.txt").ShouldBeTrue();
      File.ReadAllLines(this.workingDirectory + "Matched_From_FileB.txt").ShouldBeEquivalentTo(new String[] { "g", "h", "i" });

      File.Exists(this.workingDirectory + "Unmatched_From_FileA.txt").ShouldBeTrue();
      File.ReadAllLines(this.workingDirectory + "Unmatched_From_FileA.txt").ShouldBeEquivalentTo(new String[] { "d", "e", "f" });

      File.Exists(this.workingDirectory + "Unmatched_From_FileB.txt").ShouldBeTrue();
      File.ReadAllLines(this.workingDirectory + "Unmatched_From_FileB.txt").ShouldBeEquivalentTo(new String[] { "j", "k", "l" });
    }
  }
}
