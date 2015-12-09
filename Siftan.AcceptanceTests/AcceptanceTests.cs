
namespace Siftan.AcceptanceTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using FluentAssertions;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using NUnit.Framework;

  [TestFixture]
  public class AcceptanceTests
  {
    #region Methods
    [Test]
    public void MatchedAndUnmatchedRecordsWrittenToOutputFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      try
      {
        inputFilePath = Path.GetTempPath() + Path.GetRandomFileName() + "_test";
        matchedOutputFilePath = Path.GetTempPath() + Path.GetRandomFileName() + "_matched";
        unmatchedOutputFilePath = Path.GetTempPath() + Path.GetRandomFileName() + "_unmatched";

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.TestFile.csv", inputFilePath);

        DelimitedRecordDescriptor recordDescriptor = new DelimitedRecordDescriptor
        {
          Delimiter = "|",
          Qualifier = '\0',
          LineIDIndex = 0,
          HeaderID = "01",
          DelimitedTerm = new DelimitedRecordDescriptor.TermDefinition("01", 3)
        };
        IRecordReader recordReader = new DelimitedRecordReader(recordDescriptor);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath);
        outputWriter.Categories = RecordCategory.Matched | RecordCategory.Unmatched;

        new Engine().Execute(
          new[] { inputFilePath },
          new FileReaderFactory(),
          new DelimitedRecordReader(recordDescriptor),
          new InListExpression(new[] { "12345" }),
          outputWriter);

        File.Exists(matchedOutputFilePath).Should().BeTrue();
        File.Exists(unmatchedOutputFilePath).Should().BeTrue();

        String[] lines = File.ReadAllLines(matchedOutputFilePath);
        lines.Length.Should().Be(5);
        lines[0].Should().Be("01|Ben|Toynbee|12345|1.23");
        lines[1].Should().Be("02|||12345||");
        lines[2].Should().Be("03|||12345||");
        lines[3].Should().Be("03|||12345||");
        lines[4].Should().Be("05|||12345||");

        lines = File.ReadAllLines(unmatchedOutputFilePath);
        lines.Length.Should().Be(4);
        lines[0].Should().Be("01|Sid|Sample|54321|1.23");
        lines[1].Should().Be("02|||12345||");
        lines[2].Should().Be("03|||12345||");
        lines[3].Should().Be("05|||12345||");
      }
      finally
      {
        if (inputFilePath != null && File.Exists(inputFilePath))
        {
          File.Delete(inputFilePath);
        }
      }
    }
    #endregion
  }
}
