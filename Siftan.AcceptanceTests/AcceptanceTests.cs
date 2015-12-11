
namespace Siftan.AcceptanceTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using FluentAssertions;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Path;
  using NUnit.Framework;
  using NSubstitute;

  [TestFixture]
  public class AcceptanceTests
  {
    private const String DateTimeStampRegex = @"\A\[\d{2}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\] ";

    #region Methods
    [Test]
    public void SetToWriteMatchedAndUnmatchedRecords()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        String workingDirectory = PathOperations.CompleteDirectoryPath(Path.GetTempPath() + Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        inputFilePath = workingDirectory + "input_file.csv";
        matchedOutputFilePath = workingDirectory + "matched_output_file.csv";
        unmatchedOutputFilePath = workingDirectory + "unmatched_output_file.csv";
        logFilePath = workingDirectory + "log.log";

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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          new DelimitedRecordReader(recordDescriptor),
          new InListExpression(new[] { "12345" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeTrue();
        String[] lines = File.ReadAllLines(matchedOutputFilePath);
        lines.Length.Should().Be(5);
        lines[0].Should().Be("01|Ben|Toynbee|12345|1.23");
        lines[1].Should().Be("02|||12345||");
        lines[2].Should().Be("03|||12345||");
        lines[3].Should().Be("03|||12345||");
        lines[4].Should().Be("05|||12345||");

        File.Exists(unmatchedOutputFilePath).Should().BeTrue();
        lines = File.ReadAllLines(unmatchedOutputFilePath);
        lines.Length.Should().Be(4);
        lines[0].Should().Be("01|Sid|Sample|54321|1.23");
        lines[1].Should().Be("02|||54321||");
        lines[2].Should().Be("03|||54321||");
        lines[3].Should().Be("05|||54321||");
      }
      finally
      {
        if (inputFilePath != null && File.Exists(inputFilePath))
        {
          File.Delete(inputFilePath);
        }
      }
    }

    [Test]
    [TestCase(0)]
    [TestCase(4)]
    public void NotSetToWriteMatchedOrUnmatchedRecords(Int32 categoryValue)
    {
      // Arrange
      String workingDirectory = PathOperations.CompleteDirectoryPath(Path.GetTempPath() + Path.GetRandomFileName());
      Directory.CreateDirectory(workingDirectory);

      String inputFilePath = workingDirectory + "input_file.csv";
      String logFilePath = workingDirectory + "log.log";

      IRecordWriter mockOutputWriter = Substitute.For<IRecordWriter>();
      mockOutputWriter.Categories.Returns((RecordCategory)categoryValue);

      // Act
      Action action = () => 
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          Substitute.For<IStreamReaderFactory>(),
          Substitute.For<IRecordReader>(),
          Substitute.For<IRecordMatchExpression>(),
          mockOutputWriter);

      // Assert
      action.ShouldThrow<Exception>().WithMessage("IRecordWriter.Categories must return a valid value from RecordCategory enum. Value returned was " + mockOutputWriter.Categories + ".");
    }

    [Test]
    public void SetToWriteMatchedRecordsThatAreFoundInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        String workingDirectory = PathOperations.CompleteDirectoryPath(Path.GetTempPath() + Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        inputFilePath = workingDirectory + "input_file.csv";
        matchedOutputFilePath = workingDirectory + "matched_output_file.csv";
        unmatchedOutputFilePath = workingDirectory + "unmatched_output_file.csv";
        logFilePath = workingDirectory + "log.log";

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
        outputWriter.Categories = RecordCategory.Matched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          new DelimitedRecordReader(recordDescriptor),
          new InListExpression(new[] { "12345" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeTrue();
        String[] lines = File.ReadAllLines(matchedOutputFilePath);
        lines.Length.Should().Be(5);
        lines[0].Should().Be("01|Ben|Toynbee|12345|1.23");
        lines[1].Should().Be("02|||12345||");
        lines[2].Should().Be("03|||12345||");
        lines[3].Should().Be("03|||12345||");
        lines[4].Should().Be("05|||12345||");
      }
      finally
      {
        if (inputFilePath != null && File.Exists(inputFilePath))
        {
          File.Delete(inputFilePath);
        }
      }
    }

    [Test]
    public void SetToWriteMatchedRecordsThatAreNotFoundInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        String workingDirectory = PathOperations.CompleteDirectoryPath(Path.GetTempPath() + Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        inputFilePath = workingDirectory + "input_file.csv";
        matchedOutputFilePath = workingDirectory + "matched_output_file.csv";
        unmatchedOutputFilePath = workingDirectory + "unmatched_output_file.csv";
        logFilePath = workingDirectory + "log.log";

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

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, null);
        outputWriter.Categories = RecordCategory.Matched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          new DelimitedRecordReader(recordDescriptor),
          new InListExpression(new[] { "11111" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeFalse();
      }
      finally
      {
        if (inputFilePath != null && File.Exists(inputFilePath))
        {
          File.Delete(inputFilePath);
        }
      }
    }

    [Test]
    public void SetToWriteUnmatchedRecords()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        String workingDirectory = PathOperations.CompleteDirectoryPath(Path.GetTempPath() + Path.GetRandomFileName());
        Directory.CreateDirectory(workingDirectory);

        inputFilePath = workingDirectory + "input_file.csv";
        matchedOutputFilePath = workingDirectory + "matched_output_file.csv";
        unmatchedOutputFilePath = workingDirectory + "unmatched_output_file.csv";
        logFilePath = workingDirectory + "log.log";

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
        outputWriter.Categories = RecordCategory.Unmatched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          new DelimitedRecordReader(recordDescriptor),
          new InListExpression(new[] { "12345" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeFalse();

        File.Exists(unmatchedOutputFilePath).Should().BeTrue();
        String[] lines = File.ReadAllLines(unmatchedOutputFilePath);
        lines.Length.Should().Be(4);
        lines[0].Should().Be("01|Sid|Sample|54321|1.23");
        lines[1].Should().Be("02|||54321||");
        lines[2].Should().Be("03|||54321||");
        lines[3].Should().Be("05|||54321||");
      }
      finally
      {
        if (inputFilePath != null && File.Exists(inputFilePath))
        {
          File.Delete(inputFilePath);
        }
      }
    }

    private void AssertLogfileIsCorrect(String logFilePath)
    {
      File.Exists(logFilePath).Should().BeTrue();
      String[] logLines = File.ReadAllLines(logFilePath);
      logLines.Length.Should().Be(2);

      logLines[0].Should().MatchRegex(DateTimeStampRegex + "Starting...");
      logLines[1].Should().MatchRegex(DateTimeStampRegex + "Finished.");
    }
    #endregion
  }
}
