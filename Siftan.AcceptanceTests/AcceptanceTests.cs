
namespace Siftan.AcceptanceTests
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Reflection;
  using System.Threading;
  using FluentAssertions;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using Jabberwocky.Toolkit.Path;
  using NSubstitute;
  using NUnit.Framework;
  using TestStack.White;
  using TestStack.White.UIItems;
  using TestStack.White.UIItems.Finders;
  using TestStack.White.UIItems.WindowItems;

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
        outputWriter.Categories = RecordCategory.Matched | RecordCategory.Unmatched;

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
        outputWriter.Categories = RecordCategory.Matched;

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
        outputWriter.Categories = RecordCategory.Matched;

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
        outputWriter.Categories = RecordCategory.Matched;

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
        outputWriter.Categories = RecordCategory.Matched;

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
        outputWriter.Categories = RecordCategory.Unmatched;

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
        outputWriter.Categories = RecordCategory.Unmatched;

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
            new LogManager(null, null),
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
      String workingDirectory = CreateWorkingDirectory();

      inputFilePath = workingDirectory + inputFileName;
      matchedOutputFilePath = workingDirectory + "matched_output_file." + outputExtension;
      unmatchedOutputFilePath = workingDirectory + "unmatched_output_file." + outputExtension;
      logFilePath = workingDirectory + "Siftan.log";
    }

    private static String CreateWorkingDirectory()
    {
      String workingDirectory = PathOperations.CompleteDirectoryPath(Path.GetTempPath() + Path.GetRandomFileName());
      
      Directory.CreateDirectory(workingDirectory);
      return workingDirectory;
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
      var command = GetApplicationPath("Siftan_Console");
      VerifyApplicationFilePath(command);

      String inputFilePath = null;
      String matchedOutputFilePath = null;
      String unmatchedOutputFilePath = null;
      String logFilePath = null;
      CreateFilePathsForDelimitedTests(out inputFilePath, out matchedOutputFilePath, out unmatchedOutputFilePath, out logFilePath);

      const String DelimitedInputFileResourcePath = "Siftan.AcceptanceTests.DelimitedRecordFile.csv";
      CreateInputFilesForDelimitedTests(DelimitedInputFileResourcePath, inputFilePath);

      var commandArguments = CreateCommandArgumentsForDelimitedFile(inputFilePath, "01", "02", "12345", matchedOutputFilePath);

      ProcessStartInfo processStartInfo = new ProcessStartInfo(command, commandArguments);

      // Act
      Application application = Application.Launch(processStartInfo);
      Boolean hasExited = false;
      Int32 counter = 5;
      while ((hasExited = application.Process.HasExited) == false && (counter--) > 0)
      {
        Thread.Sleep(1000);
      }

      if (!hasExited)
      {
        throw new TimeoutException("Console application has hung.");
      }

      if (application.Process.ExitCode != 0)
      {
        throw new Exception(String.Format("Console application has finished with exit code {0}.", application.Process.ExitCode));
      }

      // Assert
      File.Exists(logFilePath).Should().BeTrue();
      File.Exists(matchedOutputFilePath).Should().BeTrue();
      File.Exists(unmatchedOutputFilePath).Should().BeTrue();
    }

    private static void CreateInputFilesForDelimitedTests(String resourceFilePath, String inputFilePath)
    {
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFilePath, inputFilePath);
    }

    private static String CreateCommandArgumentsForDelimitedFile(
      String inputFilePath, 
      String headerLineID, 
      String termLineID,
      String value,
      String outputFilePath)
    {
      const String CommandLineForDelimitedRunTemplate = "{0} delim -h {1} -t {2} inlist -v {3} output -fm {4}";
      return String.Format(CommandLineForDelimitedRunTemplate, 
        inputFilePath, 
        headerLineID, 
        termLineID,
        value,
        outputFilePath);
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
    #endregion
  }
}
