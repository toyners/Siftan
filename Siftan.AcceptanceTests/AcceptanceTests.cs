
namespace Siftan.AcceptanceTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using FluentAssertions;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Path;
  using NSubstitute;
  using NUnit.Framework;
  using Siftan.TestSupport;
  using TestStack.White;
  using TestStack.White.UIItems;
  using TestStack.White.UIItems.Finders;
  using TestStack.White.UIItems.WindowItems;

  [TestFixture]
  public class AcceptanceTests
  {
    private const String DateTimeStampRegex = @"\A\[\d{2}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\] ";

    private const String DelimitedInputFileResourcePath = "Siftan.AcceptanceTests.DelimitedRecordFile.csv";

    private String workingDirectory = null;

    private String delimitedInputFilePath = null;

    private String matchedDelimitedOutputFilePath = null;

    private String unmatchedDelimitedOutputFilePath = null;

    private String applicationLogFilePath = null;

    private String jobLogFilePath = null;

    [TestFixtureSetUp]
    public void SetupBeforeAllTests()
    {
      this.workingDirectory = Path.GetTempPath() + @"Siftan.AcceptanceTests\";
      this.delimitedInputFilePath = this.workingDirectory + "Input.csv";
      this.matchedDelimitedOutputFilePath = this.workingDirectory + "Matched.csv";
      this.unmatchedDelimitedOutputFilePath = this.workingDirectory + "Unmatched.csv";
      this.applicationLogFilePath = this.workingDirectory + "Application.log";
      this.jobLogFilePath = this.workingDirectory + "Job.log";
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
      if (Directory.Exists(this.workingDirectory))
      {
        Directory.Delete(this.workingDirectory, true);
      }

      Directory.CreateDirectory(this.workingDirectory);
    }

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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
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

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
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

    private static void CreateFilePathsForDelimitedTests(out String inputFilePath, out String matchedOutputFilePath, out String unmatchedOutputFilePath, out String logFilePath)
    {
      CreateFilePaths("input_file.csv", "csv", out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);
    }

    private static void CreateFilePathsForFixedWidthTests(out String inputFilePath, out String matchedOutputFilePath, out String unmatchedOutputFilePath, out String logFilePath)
    {
      CreateFilePaths("input_file.txt", "txt", out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);
    }

    private static void CreateFilePaths(String inputFileName, String outputExtension, out String inputFilePath, out String matchedOutputFilePath, out String unmatchedOutputFilePath, out String logFilePath)
    {
      String workingDirectory = null;

      inputFilePath = workingDirectory + inputFileName;
      matchedOutputFilePath = workingDirectory + "matched_output_file." + outputExtension;
      unmatchedOutputFilePath = workingDirectory + "unmatched_output_file." + outputExtension;
      logFilePath = workingDirectory + "Siftan.log";
    }

    private static String CreateLogFilePath()
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

    [Test]
    public void TestWPFApplication()
    {

      var applicationPath = TestContext.CurrentContext.TestDirectory;
      if (applicationPath.Contains("Release"))
      {
        throw new Exception();
      }

      applicationPath = @"C:\C#\Siftan\Siftan_WPF\bin\Debug\Siftan.exe";

      Application application = Application.Launch(applicationPath);
      Window window = application.GetWindow("Siftan");

      try
      {
        //SearchCriteria 
        SearchCriteria searchCriteria = SearchCriteria.
          ByAutomationId("mybutton").
          AndControlType(typeof(Button), WindowsFramework.Wpf);

        Button button = (Button)window.Get(searchCriteria);

        button.Should().NotBeNull();
      }
      finally
      {
        window.Close();
      }
    }

    [Test]
    public void TestConsoleApplication()
    {
      // Arrange
      CreateInputFileForDelimitedTests(DelimitedInputFileResourcePath, this.delimitedInputFilePath);

      var applicationPath = ApplicationPathCreator.GetApplicationPath("Siftan_Console");

      const String HeaderLineID = "01";
      const String TermLineID = "02";
      const String ValuesList = "12345";

      var commandLineArguments = CommandLineArgumentsCreator.CreateForDelimitedTests(
        this.delimitedInputFilePath,
        HeaderLineID,
        TermLineID,
        ValuesList,
        this.matchedDelimitedOutputFilePath,
        this.unmatchedDelimitedOutputFilePath,
        this.applicationLogFilePath,
        this.jobLogFilePath);
      
      // Act
      ConsoleRunner.Run(applicationPath, commandLineArguments);

      // Assert
      File.Exists(this.applicationLogFilePath).Should().BeTrue();
      File.Exists(this.matchedDelimitedOutputFilePath).Should().BeTrue();
      File.Exists(this.unmatchedDelimitedOutputFilePath).Should().BeTrue();
      File.Exists(this.jobLogFilePath).Should().BeTrue();
    }

    private static void CreateInputFileForDelimitedTests(String resourceFilePath, String inputFilePath)
    {
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFilePath, inputFilePath);
    }

    private static String GetApplicationPath(String applicationName)
    {
      const String ApplicationPathTemplate = @"C:\C#\Siftan\{0}\bin\{1}\{0}.exe";

      var applicationPath = String.Format(ApplicationPathTemplate,
        applicationName,
        (TestContext.CurrentContext.TestDirectory.Contains("Release") ? "Release" : "Debug"));

      return applicationPath;
    }

    private static void VerifyApplicationFilePath(String applicationPath)
    {
      if (!File.Exists(applicationPath))
      {
        throw new FileNotFoundException(String.Format("File '{0}' not found.", applicationPath));
      }
    }
  }

  public static class CommandLineArgumentsCreator
  {
    public static String CreateForDelimitedTests(
      String inputFilePath,
      String headerLineID,
      String termLineID,
      String value,
      String matchedOutputFilePath,
      String unmatchedOutputFilePath,
      String applicationLogFilePath,
      String jobLogFilePath)
    {
      var commandLineArgumentsBuilder = new CommandLineArgumentsBuilder()
        .WithInput(new InputBuilder()
          .IsSingleFile(inputFilePath))
        .WithDelim(new DelimBuilder()
          .HasHeaderLineID(headerLineID)
          .HasTermLineID(termLineID))
        .WithInList(new InListBuilder()
          .HasValuesList(value))
        .WithOutput(new OutputBuilder()
          .HasMatchedOutputFile(matchedOutputFilePath)
          .HasUnmatchedOutputFile(unmatchedOutputFilePath))
        .WithLog(new LogBuilder()
          .HasApplicationLogFilePath(applicationLogFilePath)
          .HasJobLogFilePath(jobLogFilePath));

      return String.Join(" ", commandLineArgumentsBuilder.Build());
    }
  }

  public static class ApplicationPathCreator
  {
    public static String GetApplicationPath(String applicationName)
    {
      const String ApplicationPathTemplate = @"C:\C#\Siftan\{0}\bin\{1}\{0}.exe";

      var applicationPath = String.Format(ApplicationPathTemplate,
        applicationName,
        (TestContext.CurrentContext.TestDirectory.Contains("Release") ? "Release" : "Debug"));

      VerifyApplicationExists(applicationPath);

      return applicationPath;
    }

    private static void VerifyApplicationExists(String applicationPath)
    {
      if (!File.Exists(applicationPath))
      {
        throw new FileNotFoundException(String.Format("File '{0}' not found.", applicationPath));
      }
    }
  }
}
