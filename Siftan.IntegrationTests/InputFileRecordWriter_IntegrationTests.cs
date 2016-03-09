
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
    [Test]
    public void RecordsFromMultipleInputFilesWrittenToCorrespondingOutputFiles()
    {
      // Arrange
      File.WriteAllLines("FileA.txt", new String[] { "a", "b", "c", "d", "e", "f" });
      File.WriteAllLines("FileB.txt", new String[] { "g", "h", "i", "j", "k", "l" });

      var mockStatisticsCollector = Substitute.For<IStatisticsCollector>();
      InputFileRecordWriter writer = new InputFileRecordWriter(mockStatisticsCollector);
      FileReader inputFileA = new FileReader("FileA.txt");
      FileReader inputFileB = new FileReader("FileB.txt");

      // Act
      writer.WriteMatchedRecord(inputFileA, new Record { Start = 0, End = 8 });
      writer.WriteMatchedRecord(inputFileB, new Record { Start = 0, End = 8 });
      writer.WriteMatchedRecord(inputFileA, new Record { Start = 9, End = 18 });
      writer.WriteMatchedRecord(inputFileB, new Record { Start = 9, End = 18 });
      writer.Close();

      // Assert
      File.Exists("Matched_From_FileA.txt").ShouldBeTrue();
      File.ReadAllLines("Matched_From_FileA.txt").ShouldBeEquivalentTo(new String[] { "a", "b", "c", "d", "e", "f" });

      File.Exists("Matched_From_FileB.txt").ShouldBeTrue();
      File.ReadAllLines("Matched_From_FileB.txt").ShouldBeEquivalentTo(new String[] { "g", "h", "i", "j", "k", "l" });
    }
  }
}
