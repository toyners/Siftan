
namespace Siftan.AcceptanceTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using Jabberwocky.Toolkit.Assembly;
  using NUnit.Framework;
  using Shouldly;
  using TestSupport;

  public class DelimitedJob_AcceptanceTests
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
    public void RunDelimitedJobReturnsExpectedOutputFiles()
    {
      // Arrange
      CreateInputFileForDelimitedTests(DelimitedInputFileResourcePath, this.delimitedInputFilePath);

      var applicationPath = ApplicationPathCreator.GetApplicationPath("Siftan_Console");

      var commandLineArguments =
        CommandLineArgumentsCreator.TranslateArgumentsToString(
          CommandLineArgumentsCreator.CreateArgumentsForDelimitedTests(
            CommandLineArgumentsCreator.CreateSingleFileInputBuilder(this.delimitedInputFilePath),
            CommandLineArgumentsCreator.CreateDelimBuilder(Delimiter, Qualifier, HeaderLineID, LineIDIndex, TermLineID, TermIndex),
            SingleValuesList,
            CommandLineArgumentsCreator.CreateOutputBuilder(this.matchedDelimitedOutputFilePath, this.unmatchedDelimitedOutputFilePath),
            CommandLineArgumentsCreator.CreateLogBuilder(this.applicationLogFilePath, this.jobLogFilePath)
          )
        );

      // Act
      ConsoleRunner.Run(applicationPath, commandLineArguments);

      // Assert
      File.Exists(this.applicationLogFilePath).ShouldBeTrue();
      File.Exists(this.matchedDelimitedOutputFilePath).ShouldBeTrue();
      FileContentAssertion.IsMatching(
        File.ReadAllLines(this.matchedDelimitedOutputFilePath),
        new String[]
        {
          "01|Ben|Toynbee|12345|1.23",
          "02|||12345||",
          "03|||12345||",
          "03|||12345||",
          "05|||12345||"
        });

      File.Exists(this.unmatchedDelimitedOutputFilePath).ShouldBeTrue();
      FileContentAssertion.IsMatching(
        File.ReadAllLines(this.unmatchedDelimitedOutputFilePath),
        new String[]
        {
          "01|Sid|Sample|54321|1.23",
          "02|||54321||",
          "03|||54321||",
          "05|||54321||"
        });

      File.Exists(this.jobLogFilePath).ShouldBeTrue();
      FileContentAssertion.IsMatching(
        File.ReadAllLines(this.jobLogFilePath),
        new String[]
        {
          //DateTimeStampRegex + " Input Files: " + this.delimitedInputFilePath,
          //DateTimeStampRegex + " Delimited Record Format",
          //DateTimeStampRegex + " Delimiter: " + Delimiter,
          //DateTimeStampRegex + " Qualifier: " + Qualifier,
          //DateTimeStampRegex + " Line ID Index: " + LineIDIndex,
          //DateTimeStampRegex + " Header Line ID: " + HeaderLineID,
          //DateTimeStampRegex + " Term Line ID: " + TermLineID,
          //DateTimeStampRegex + " Term Index: " + TermIndex,
          //DateTimeStampRegex + " Matched Records Output File: " + this.matchedDelimitedOutputFilePath,
          DateTimeStampRegex + "Run Started...",
          DateTimeStampRegex + "Record found at position 0 with Term '12345' matches with List Term '12345'.",
          DateTimeStampRegex + "Record found at position 86 with Term '54321'.",
          DateTimeStampRegex + "Run Finished."
          //DateTimeStampRegex + String.Format(" 2 Record(s) processed."),
          //DateTimeStampRegex + String.Format(" 2 Record(s) processed from input file {0}." + this.delimitedInputFilePath),
          //DateTimeStampRegex + String.Format(" 1 Record(s) matched."),
          //DateTimeStampRegex + String.Format(" 1 Record(s) not matched."),
          //DateTimeStampRegex + String.Format(" 1 Record(s) matched from input file {0}." + this.delimitedInputFilePath),
          //DateTimeStampRegex + String.Format(" 1 Record(s) not matched from input file {0}." + this.delimitedInputFilePath),
          //DateTimeStampRegex + String.Format(" 1 Record(s) written to output file {0}." + this.matchedDelimitedOutputFilePath),
        });
    }

    [Test]
    public void RunDelimitedJobWithWrongDelimiterCreatesNoOutputFiles()
    {
      // Arrange
      CreateInputFileForDelimitedTests(DelimitedInputFileResourcePath, this.delimitedInputFilePath);

      var applicationPath = ApplicationPathCreator.GetApplicationPath("Siftan_Console");

      var commandLineArguments =
        CommandLineArgumentsCreator.TranslateArgumentsToString(
          CommandLineArgumentsCreator.CreateArgumentsForDelimitedTests(
            CommandLineArgumentsCreator.CreateSingleFileInputBuilder(this.delimitedInputFilePath),
            CommandLineArgumentsCreator.CreateDelimBuilder(WrongDelimiter, Qualifier, HeaderLineID, LineIDIndex, TermLineID, TermIndex),
            SingleValuesList,
            CommandLineArgumentsCreator.CreateOutputBuilder(this.matchedDelimitedOutputFilePath, this.unmatchedDelimitedOutputFilePath),
            CommandLineArgumentsCreator.CreateLogBuilder(this.applicationLogFilePath, this.jobLogFilePath)
          )
        );

      // Act
      ConsoleRunner.Run(applicationPath, commandLineArguments);

      // Assert
      File.Exists(this.applicationLogFilePath).ShouldBeTrue();
      File.Exists(this.matchedDelimitedOutputFilePath).ShouldBeFalse();
      File.Exists(this.unmatchedDelimitedOutputFilePath).ShouldBeFalse();
      File.Exists(this.jobLogFilePath).ShouldBeTrue();

      FileContentAssertion.IsMatching(
        File.ReadAllLines(this.jobLogFilePath),
        new String[]
        {
          DateTimeStampRegex + "Run Started...",
          DateTimeStampRegex + "Run Finished."
        });
    }

    [Test]
    public void RunDelimitedJobWithWrongTermIndexCreatesUnmatchedOutputFileOnly()
    {
      // Arrange
      CreateInputFileForDelimitedTests(DelimitedInputFileResourcePath, this.delimitedInputFilePath);

      var applicationPath = ApplicationPathCreator.GetApplicationPath("Siftan_Console");

      var commandLineArguments =
        CommandLineArgumentsCreator.TranslateArgumentsToString(
          CommandLineArgumentsCreator.CreateArgumentsForDelimitedTests(
            CommandLineArgumentsCreator.CreateSingleFileInputBuilder(this.delimitedInputFilePath),
            CommandLineArgumentsCreator.CreateDelimBuilder(Delimiter, Qualifier, HeaderLineID, LineIDIndex, TermLineID, WrongTermIndex),
            SingleValuesList,
            CommandLineArgumentsCreator.CreateOutputBuilder(this.matchedDelimitedOutputFilePath, this.unmatchedDelimitedOutputFilePath),
            CommandLineArgumentsCreator.CreateLogBuilder(this.applicationLogFilePath, this.jobLogFilePath)
          )
        );

      // Act
      ConsoleRunner.Run(applicationPath, commandLineArguments);

      // Assert
      File.Exists(this.applicationLogFilePath).ShouldBeTrue();
      File.Exists(this.matchedDelimitedOutputFilePath).ShouldBeFalse();

      File.Exists(this.unmatchedDelimitedOutputFilePath).ShouldBeTrue();
      FileContentAssertion.IsMatching(
        File.ReadAllLines(this.unmatchedDelimitedOutputFilePath),
        new String[]
        {
          "01|Ben|Toynbee|12345|1.23",
          "02|||12345||",
          "03|||12345||",
          "03|||12345||",
          "05|||12345||",
          "01|Sid|Sample|54321|1.23",
          "02|||54321||",
          "03|||54321||",
          "05|||54321||"
        });

      File.Exists(this.jobLogFilePath).ShouldBeTrue();
      FileContentAssertion.IsMatching(
        File.ReadAllLines(this.jobLogFilePath),
        new String[]
        {
          DateTimeStampRegex + "Run Started...",
          DateTimeStampRegex + "Record found at position 0 with Term '02'.",
          DateTimeStampRegex + "Record found at position 86 with Term '02'.",
          DateTimeStampRegex + "Run Finished."
        });
    }

    private static void CreateInputFileForDelimitedTests(String resourceFilePath, String inputFilePath)
    {
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFilePath, inputFilePath);
    }
  }
}
