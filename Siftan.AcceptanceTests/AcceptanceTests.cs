
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
    public void SetToWriteMatchedAndUnmatchedDelimitedRecordsThatAreInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        CreateFilePathsForDelimitedTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.DelimitedRecordFile.csv", inputFilePath);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath);
        outputWriter.Categories = RecordCategory.Matched | RecordCategory.Unmatched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          CreateDelimitedRecordReader(),
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
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    [Test]
    public void SetToWriteMatchedAndUnmatchedFixedWidthRecordsThatAreInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        CreateFilePathsForFixedWidthTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.FixedWidthRecordFile.txt", inputFilePath);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath);
        outputWriter.Categories = RecordCategory.Matched | RecordCategory.Unmatched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          CreateFixedWidthRecordReader(),
          new InListExpression(new[] { "12345" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeTrue();
        String[] lines = File.ReadAllLines(matchedOutputFilePath);
        lines.Length.Should().Be(5);
        lines[0].Should().Be("01Ben Toynbee123451.23");
        lines[1].Should().Be("02           12345");
        lines[2].Should().Be("03           12345");
        lines[3].Should().Be("03           12345");
        lines[4].Should().Be("05           12345");

        File.Exists(unmatchedOutputFilePath).Should().BeTrue();
        lines = File.ReadAllLines(unmatchedOutputFilePath);
        lines.Length.Should().Be(4);
        lines[0].Should().Be("01Sid Sample 543211.23");
        lines[1].Should().Be("02           54321");
        lines[2].Should().Be("03           54321");
        lines[3].Should().Be("05           54321");
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    [Test]
    public void SetToWriteMatchedDelimitedRecordsThatAreInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        CreateFilePathsForDelimitedTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.DelimitedRecordFile.csv", inputFilePath);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath);
        outputWriter.Categories = RecordCategory.Matched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          CreateDelimitedRecordReader(),
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
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    [Test]
    public void SetToWriteMatchedFixedWidthRecordsThatAreInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        CreateFilePathsForFixedWidthTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.FixedWidthRecordFile.txt", inputFilePath);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath);
        outputWriter.Categories = RecordCategory.Matched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          CreateFixedWidthRecordReader(),
          new InListExpression(new[] { "12345" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeTrue();
        String[] lines = File.ReadAllLines(matchedOutputFilePath);
        lines.Length.Should().Be(5);
        lines[0].Should().Be("01Ben Toynbee123451.23");
        lines[1].Should().Be("02           12345");
        lines[2].Should().Be("03           12345");
        lines[3].Should().Be("03           12345");
        lines[4].Should().Be("05           12345");
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    [Test]
    public void SetToWriteMatchedDelimitedRecordsThatAreNotInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        CreateFilePathsForDelimitedTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.DelimitedRecordFile.csv", inputFilePath);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, null);
        outputWriter.Categories = RecordCategory.Matched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          CreateDelimitedRecordReader(),
          new InListExpression(new[] { "11111" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeFalse();
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    [Test]
    public void SetToWriteMatchedFixedWidthRecordsThatAreNotInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        CreateFilePathsForFixedWidthTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.FixedWidthRecordFile.txt", inputFilePath);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, null);
        outputWriter.Categories = RecordCategory.Matched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          CreateDelimitedRecordReader(),
          new InListExpression(new[] { "11111" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeFalse();
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    [Test]
    public void SetToWriteUnmatchedDelimitedRecordsThatAreInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        CreateFilePathsForDelimitedTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.DelimitedRecordFile.csv", inputFilePath);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath);
        outputWriter.Categories = RecordCategory.Unmatched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          CreateDelimitedRecordReader(),
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
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    [Test]
    public void SetToWriteUnmatchedFixedWidthRecordsThatAreInDataFile()
    {
      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;

      try
      {
        // Arrange
        CreateFilePathsForFixedWidthTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

        Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile("Siftan.AcceptanceTests.FixedWidthRecordFile.txt", inputFilePath);

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath);
        outputWriter.Categories = RecordCategory.Unmatched;

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          logFilePath,
          new FileReaderFactory(),
          CreateFixedWidthRecordReader(),
          new InListExpression(new[] { "12345" }),
          outputWriter);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).Should().BeFalse();

        File.Exists(unmatchedOutputFilePath).Should().BeTrue();
        String[] lines = File.ReadAllLines(unmatchedOutputFilePath);
        lines.Length.Should().Be(4);
        lines[0].Should().Be("01Sid Sample 543211.23");
        lines[1].Should().Be("02           54321");
        lines[2].Should().Be("03           54321");
        lines[3].Should().Be("05           54321");
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    [Test]
    [TestCase(0, "IRecordWriter.Categories must return a valid value from RecordCategory enum. Value returned was 0.")]
    [TestCase(4, "IRecordWriter.Categories must return a valid value from RecordCategory enum. Value returned was 4.")]
    public void NotSetToWriteMatchedOrUnmatchedRecords(Int32 categoryValue, String expectedExceptionMessage)
    {
      // Arrange
      String logFilePath = null;

      try
      {
        logFilePath = CreateLogFilePath();
        IRecordWriter mockOutputWriter = Substitute.For<IRecordWriter>();
        mockOutputWriter.Categories.Returns((RecordCategory)categoryValue);

        // Act
        Action action = () =>
          new Engine().Execute(
            null,
            logFilePath,
            Substitute.For<IStreamReaderFactory>(),
            Substitute.For<IRecordReader>(),
            Substitute.For<IRecordMatchExpression>(),
            mockOutputWriter);

        // Assert
        action.ShouldThrow<Exception>().WithMessage(expectedExceptionMessage);
        this.AssertLogfileContainsExpectedException(logFilePath, "EXCEPTION: " + expectedExceptionMessage);
      }
      finally
      {
        DeleteDirectoryContainingFile(logFilePath);
      }
    }

    private void CreateFilePathsForDelimitedTests(out String inputFilePath, out String matchedOutputFilePath, out String unmatchedOutputFilePath, out String logFilePath)
    {
      CreateFilePaths("input_file.csv", "csv", out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);
    }

    private void CreateFilePathsForFixedWidthTests(out String inputFilePath, out String matchedOutputFilePath, out String unmatchedOutputFilePath, out String logFilePath)
    {
      CreateFilePaths("input_file.txt", "txt", out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);
    }

    private void CreateFilePaths(String inputFileName, String outputExtension, out String inputFilePath, out String matchedOutputFilePath, out String unmatchedOutputFilePath, out String logFilePath)
    {
      String workingDirectory = PathOperations.CompleteDirectoryPath(Path.GetTempPath() + Path.GetRandomFileName());
      Directory.CreateDirectory(workingDirectory);

      inputFilePath = workingDirectory + inputFileName;
      matchedOutputFilePath = workingDirectory + "matched_output_file." + outputExtension;
      unmatchedOutputFilePath = workingDirectory + "unmatched_output_file." + outputExtension;
      logFilePath = workingDirectory + "Siftan.log";
    }

    private String CreateLogFilePath()
    {
      String workingDirectory = PathOperations.CompleteDirectoryPath(Path.GetTempPath() + Path.GetRandomFileName());
      Directory.CreateDirectory(workingDirectory);
      return workingDirectory + "Siftan.log";
    }

    private IRecordReader CreateDelimitedRecordReader()
    {
      DelimitedRecordDescriptor recordDescriptor = new DelimitedRecordDescriptor
      {
        Delimiter = "|",
        Qualifier = '\0',
        LineIDIndex = 0,
        HeaderID = "01",
        DelimitedTerm = new DelimitedRecordDescriptor.TermDefinition("01", 3)
      };

      return new DelimitedRecordReader(recordDescriptor);
    }

    private IRecordReader CreateFixedWidthRecordReader()
    {
      FixedWidthRecordDescriptor recordDescriptor = new FixedWidthRecordDescriptor(0, 2, "01", new FixedWidthRecordDescriptor.TermDefinition("02", 13, 5));

      return new FixedWidthRecordReader(recordDescriptor);
    }

    private void DeleteDirectoryContainingFile(String filePath)
    {
      if (filePath != null && File.Exists(filePath))
      {
        Directory.GetParent(filePath).Delete(true);
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

    private void AssertLogfileContainsExpectedException(String logFilePath, String exceptionMessage)
    {
      File.Exists(logFilePath).Should().BeTrue();
      String[] logLines = File.ReadAllLines(logFilePath);
      logLines.Length.Should().Be(1);

      logLines[0].Should().MatchRegex(DateTimeStampRegex + exceptionMessage);
    }
    #endregion
  }
}
