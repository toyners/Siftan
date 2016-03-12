
namespace Siftan.IntegrationTests
{
  using System;
  using System.IO;
  using FluentAssertions;
  using Jabberwocky.Toolkit.IO;
  using NSubstitute;
  using NUnit.Framework;
  using Shouldly;

  [TestFixture]
  public class InputFileRecordWriter_IntegrationTests
  {
    private String workingDirectory;

    [SetUp]
    public void SetupBeforeEachTest()
    {
      this.workingDirectory = Path.GetTempPath() + @"InputFileRecordWriter_IntegrationTests\";

      if (Directory.Exists(this.workingDirectory))
      {
        Directory.Delete(this.workingDirectory, true);
      }

      Directory.CreateDirectory(this.workingDirectory);
    }

    [Test]
    public void MatchedRecordsFromMultipleInputFilesWrittenToCorrespondingOutputFiles()
    {
      // Arrange
      File.WriteAllLines(this.workingDirectory + "FileA.txt", new String[] { "a", "b", "c", "d", "e", "f" });
      File.WriteAllLines(this.workingDirectory + "FileB.txt", new String[] { "g", "h", "i", "j", "k", "l" });

      var mockStatisticsCollector = Substitute.For<IStatisticsCollector>();
      InputFileRecordWriter writer = new InputFileRecordWriter(mockStatisticsCollector, true, false);
      using (FileReader inputFileA = new FileReader(this.workingDirectory + "FileA.txt"))
      {
        using (FileReader inputFileB = new FileReader(this.workingDirectory + "FileB.txt"))
        {
          // Act
          writer.WriteMatchedRecord(inputFileA, new Record { Start = 0, End = 8 });
          writer.WriteMatchedRecord(inputFileB, new Record { Start = 0, End = 8 });
          writer.WriteMatchedRecord(inputFileA, new Record { Start = 9, End = 18 });
          writer.WriteMatchedRecord(inputFileB, new Record { Start = 9, End = 18 });
          writer.Close();
        }
      }

      // Assert
      File.Exists(this.workingDirectory + "Matched_From_FileA.txt").ShouldBeTrue();
      File.ReadAllLines(this.workingDirectory + "Matched_From_FileA.txt").ShouldBeEquivalentTo(new String[] { "a", "b", "c", "d", "e", "f" });

      File.Exists(this.workingDirectory + "Matched_From_FileB.txt").ShouldBeTrue();
      File.ReadAllLines(this.workingDirectory + "Matched_From_FileB.txt").ShouldBeEquivalentTo(new String[] { "g", "h", "i", "j", "k", "l" });
    }

    [Test]
    public void UnmatchedRecordsFromMultipleInputFilesWrittenToCorrespondingOutputFiles()
    {
      // Arrange
      File.WriteAllLines(this.workingDirectory + "FileA.txt", new String[] { "a", "b", "c", "d", "e", "f" });
      File.WriteAllLines(this.workingDirectory + "FileB.txt", new String[] { "g", "h", "i", "j", "k", "l" });

      var mockStatisticsCollector = Substitute.For<IStatisticsCollector>();
      InputFileRecordWriter writer = new InputFileRecordWriter(mockStatisticsCollector, false, true);
      using (FileReader inputFileA = new FileReader(this.workingDirectory + "FileA.txt"))
      {
        using (FileReader inputFileB = new FileReader(this.workingDirectory + "FileB.txt"))
        {

          // Act
          writer.WriteUnmatchedRecord(inputFileA, new Record { Start = 0, End = 8 });
          writer.WriteUnmatchedRecord(inputFileB, new Record { Start = 0, End = 8 });
          writer.WriteUnmatchedRecord(inputFileA, new Record { Start = 9, End = 18 });
          writer.WriteUnmatchedRecord(inputFileB, new Record { Start = 9, End = 18 });
          writer.Close();
        }
      }

      // Assert
      File.Exists(this.workingDirectory + "Unmatched_From_FileA.txt").ShouldBeTrue();
      File.ReadAllLines(this.workingDirectory + "Unmatched_From_FileA.txt").ShouldBeEquivalentTo(new String[] { "a", "b", "c", "d", "e", "f" });

      File.Exists(this.workingDirectory + "Unmatched_From_FileB.txt").ShouldBeTrue();
      File.ReadAllLines(this.workingDirectory + "Unmatched_From_FileB.txt").ShouldBeEquivalentTo(new String[] { "g", "h", "i", "j", "k", "l" });
    }
  }
}
