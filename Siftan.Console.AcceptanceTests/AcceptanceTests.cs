
namespace Siftan.Console.AcceptanceTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using Jabberwocky.Toolkit.Assembly;
  using Jabberwocky.Toolkit.IO;
  using NUnit.Framework;
  using Shouldly;
  using TestStack.White;
  using TestStack.White.UIItems;
  using TestStack.White.UIItems.Finders;
  using TestStack.White.UIItems.WindowItems;

  public class AcceptanceTests
  {
    private const String DateTimeStampRegex = @"\A\[\d{2}-\d{2}-\d{4} \d{2}:\d{2}:\d{2}\] ";

    private const String DelimitedInputFileResourcePath = "Siftan.AcceptanceTests.DelimitedRecordFile.csv";

    private const String HeaderLineID = "01";

    private const UInt32 LineIDIndex = 0;

    private const String TermLineID = "02";

    private const UInt32 WrongTermIndex = 0;

    private const UInt32 TermIndex = 3;

    private const String SingleValuesList = "12345";

    private const String WrongDelimiter = ",";

    private const String Delimiter = "|";

    private const Char Qualifier = '\'';

    private String workingDirectory = null;

    private String delimitedInputFilePath = null;

    private String matchedDelimitedOutputFilePath = null;

    private String unmatchedDelimitedOutputFilePath = null;

    private String applicationLogFilePath = null;

    private String jobLogFilePath = null;

    public void SetupBeforeAllTests()
    {
      this.workingDirectory = Path.GetTempPath() + @"Siftan.AcceptanceTests\";
      this.delimitedInputFilePath = this.workingDirectory + "Input.csv";
      this.matchedDelimitedOutputFilePath = this.workingDirectory + "Matched.csv";
      this.unmatchedDelimitedOutputFilePath = this.workingDirectory + "Unmatched.csv";
      this.applicationLogFilePath = this.workingDirectory + "Application.log";
      this.jobLogFilePath = this.workingDirectory + "Job.log";
    }

    public void SetupBeforeEachTest()
    {
      if (Directory.Exists(this.workingDirectory))
      {
        Directory.Delete(this.workingDirectory, true);
      }

      Directory.CreateDirectory(this.workingDirectory);
    }

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

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath, new StatisticsManager());

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
          new FileReaderFactory(),
          CreateFixedWidthRecordReader(),
          new InListExpression(new[] { "12345" }),
          outputWriter,
          null,
          null);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).ShouldBeTrue();
        String[] lines = File.ReadAllLines(matchedOutputFilePath);
        lines.Length.ShouldBe(5);
        lines[0].ShouldBe("01Ben Toynbee123451.23");
        lines[1].ShouldBe("02           12345");
        lines[2].ShouldBe("03           12345");
        lines[3].ShouldBe("03           12345");
        lines[4].ShouldBe("05           12345");

        File.Exists(unmatchedOutputFilePath).ShouldBeTrue();
        lines = File.ReadAllLines(unmatchedOutputFilePath);
        lines.Length.ShouldBe(4);
        lines[0].ShouldBe("01Sid Sample 543211.23");
        lines[1].ShouldBe("02           54321");
        lines[2].ShouldBe("03           54321");
        lines[3].ShouldBe("05           54321");
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

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

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath, new StatisticsManager());

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
          new FileReaderFactory(),
          CreateFixedWidthRecordReader(),
          new InListExpression(new[] { "12345" }),
          outputWriter,
          null,
          null);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).ShouldBeTrue();
        String[] lines = File.ReadAllLines(matchedOutputFilePath);
        lines.Length.ShouldBe(5);
        lines[0].ShouldBe("01Ben Toynbee123451.23");
        lines[1].ShouldBe("02           12345");
        lines[2].ShouldBe("03           12345");
        lines[3].ShouldBe("03           12345");
        lines[4].ShouldBe("05           12345");
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

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

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, null, new StatisticsManager());

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
          new FileReaderFactory(),
          CreateDelimitedRecordReader(),
          new InListExpression(new[] { "11111" }),
          outputWriter,
          null,
          null);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).ShouldBeFalse();
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

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

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, null, new StatisticsManager());

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
          new FileReaderFactory(),
          CreateDelimitedRecordReader(),
          new InListExpression(new[] { "11111" }),
          outputWriter,
          null,
          null);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).ShouldBeFalse();
      }
      finally
      {
        DeleteDirectoryContainingFile(inputFilePath);
      }
    }

    
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

        OneFileRecordWriter outputWriter = new OneFileRecordWriter(matchedOutputFilePath, unmatchedOutputFilePath, new StatisticsManager());

        // Act
        new Engine().Execute(
          new[] { inputFilePath },
          new LogManager(null, null),
          new FileReaderFactory(),
          CreateFixedWidthRecordReader(),
          new InListExpression(new[] { "12345" }),
          outputWriter,
          null,
          null);

        // Assert
        this.AssertLogfileIsCorrect(logFilePath);

        File.Exists(matchedOutputFilePath).ShouldBeFalse();

        File.Exists(unmatchedOutputFilePath).ShouldBeTrue();
        String[] lines = File.ReadAllLines(unmatchedOutputFilePath);
        lines.Length.ShouldBe(4);
        lines[0].ShouldBe("01Sid Sample 543211.23");
        lines[1].ShouldBe("02           54321");
        lines[2].ShouldBe("03           54321");
        lines[3].ShouldBe("05           54321");
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
      File.Exists(logFilePath).ShouldBeTrue();
      String[] logLines = File.ReadAllLines(logFilePath);
      logLines.Length.ShouldBe(2);

      logLines[0].ShouldMatch(DateTimeStampRegex + "Starting...");
      logLines[1].ShouldMatch(DateTimeStampRegex + "Finished.");
    }
        
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

        button.ShouldNotBeNull();
      }
      finally
      {
        window.Close();
      }
    }
  }
}
