
namespace Siftan.Console.AcceptanceTests
{
  using System;
  using System.IO;
  using System.Reflection;
  using System.Text.RegularExpressions;
  using Jabberwocky.Toolkit.Assembly;
  using NUnit.Framework;
  using Shouldly;
  using TestSupport;

  public class DelimitedJob_AcceptanceTests : AcceptanceTestsBase
  {
    private const String ApplicationName = "Siftan.Console";

    private const String DelimitedInputFileResourcePath = "Siftan.Console.AcceptanceTests.DelimitedRecordFile.csv";

    private const String HeaderLineID = "01";

    private const UInt32 LineIDIndex = 0;

    private const String TermLineID = "02";

    private const UInt32 WrongTermIndex = 0;

    private const UInt32 TermIndex = 3;

    private const String SingleValuesList = "12345";

    private const String WrongDelimiter = ",";

    private const String Delimiter = "|";

    private const Char Qualifier = '\'';

    [TestFixtureSetUp]
    public void SetupBeforeAllTests()
    {
      SetFilePathsForDelimitedJob("Siftan.AcceptanceTests");
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
      CreateEmptyWorkingDirectory();
    }

    [Test]
    public void RunDelimitedJobReturnsExpectedOutputFiles()
    {
      // Arrange
      CreateInputFileForDelimitedTests(DelimitedInputFileResourcePath, this.inputFilePath);

      var applicationPath = ApplicationPathCreator.GetApplicationPath(ApplicationName);

      var commandLineArguments =
        CommandLineArgumentsCreator.TranslateArgumentsToString(
          CommandLineArgumentsCreator.CreateArgumentsForDelimitedTests(
            CommandLineArgumentsCreator.CreateSingleFileInputBuilder(this.inputFilePath),
            CommandLineArgumentsCreator.CreateDelimBuilder(Delimiter, Qualifier, HeaderLineID, LineIDIndex, TermLineID, TermIndex),
            SingleValuesList,
            CommandLineArgumentsCreator.CreateOutputBuilder(this.matchedOutputFilePath, this.unmatchedOutputFilePath),
            CommandLineArgumentsCreator.CreateLogBuilder(this.applicationLogFilePath, this.jobLogFilePath)
          )
        );

      // Act
      ConsoleRunner.Run(applicationPath, commandLineArguments);

      // Assert
      File.Exists(this.applicationLogFilePath).ShouldBeTrue();
      File.Exists(this.matchedOutputFilePath).ShouldBeTrue();
      StringArrayComparison.IsMatching(
        File.ReadAllLines(this.matchedOutputFilePath),
        new String[]
        {
          "01|Ben|Toynbee|12345|1.23",
          "02|||12345||",
          "03|||12345||",
          "03|||12345||",
          "05|||12345||"
        });

      File.Exists(this.unmatchedOutputFilePath).ShouldBeTrue();
      StringArrayComparison.IsMatching(
        File.ReadAllLines(this.unmatchedOutputFilePath),
        new String[]
        {
          "01|Sid|Sample|54321|1.23",
          "02|||54321||",
          "03|||54321||",
          "05|||54321||"
        });

      File.Exists(this.jobLogFilePath).ShouldBeTrue();
      StringArrayComparison.IsMatching(
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
          TestConstants.DateTimeStampRegex + "Run Started...",
          TestConstants.DateTimeStampRegex + "Record found at position 0 with Term '12345' matches with List Term '12345'.",
          TestConstants.DateTimeStampRegex + "Record found at position 86 with Term '54321'.",
          TestConstants.DateTimeStampRegex + Regex.Escape("2 Record(s) processed."),
          TestConstants.DateTimeStampRegex + Regex.Escape("1 Record(s) matched."),
          TestConstants.DateTimeStampRegex + Regex.Escape("1 Record(s) not matched."),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("2 Record(s) processed from input file {0}.", this.inputFilePath)),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("1 Record(s) matched from input file {0}.", this.inputFilePath)),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("1 Record(s) not matched from input file {0}.", this.inputFilePath)),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("1 Record(s) written to output file {0}.", this.matchedOutputFilePath)),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("1 Record(s) written to output file {0}.", this.unmatchedOutputFilePath)),
          TestConstants.DateTimeStampRegex + "Run Finished.",
        });
    }

    [Test]
    public void RunDelimitedJobWithWrongDelimiterCreatesNoOutputFiles()
    {
      // Arrange
      CreateInputFileForDelimitedTests(DelimitedInputFileResourcePath, this.inputFilePath);

      var applicationPath = ApplicationPathCreator.GetApplicationPath(ApplicationName);

      var commandLineArguments =
        CommandLineArgumentsCreator.TranslateArgumentsToString(
          CommandLineArgumentsCreator.CreateArgumentsForDelimitedTests(
            CommandLineArgumentsCreator.CreateSingleFileInputBuilder(this.inputFilePath),
            CommandLineArgumentsCreator.CreateDelimBuilder(WrongDelimiter, Qualifier, HeaderLineID, LineIDIndex, TermLineID, TermIndex),
            SingleValuesList,
            CommandLineArgumentsCreator.CreateOutputBuilder(this.matchedOutputFilePath, this.unmatchedOutputFilePath),
            CommandLineArgumentsCreator.CreateLogBuilder(this.applicationLogFilePath, this.jobLogFilePath)
          )
        );

      // Act
      ConsoleRunner.Run(applicationPath, commandLineArguments);

      // Assert
      File.Exists(this.applicationLogFilePath).ShouldBeTrue();
      File.Exists(this.matchedOutputFilePath).ShouldBeFalse();
      File.Exists(this.unmatchedOutputFilePath).ShouldBeFalse();
      File.Exists(this.jobLogFilePath).ShouldBeTrue();

      StringArrayComparison.IsMatching(
        File.ReadAllLines(this.jobLogFilePath),
        new String[]
        {
          TestConstants.DateTimeStampRegex + "Run Started...",
          TestConstants.DateTimeStampRegex + Regex.Escape("0 Record(s) processed."),
          TestConstants.DateTimeStampRegex + "Run Finished.",

        });
    }

    [Test]
    public void RunDelimitedJobWithWrongTermIndexCreatesUnmatchedOutputFileOnly()
    {
      // Arrange
      CreateInputFileForDelimitedTests(DelimitedInputFileResourcePath, this.inputFilePath);

      var applicationPath = ApplicationPathCreator.GetApplicationPath(ApplicationName);

      var commandLineArguments =
        CommandLineArgumentsCreator.TranslateArgumentsToString(
          CommandLineArgumentsCreator.CreateArgumentsForDelimitedTests(
            CommandLineArgumentsCreator.CreateSingleFileInputBuilder(this.inputFilePath),
            CommandLineArgumentsCreator.CreateDelimBuilder(Delimiter, Qualifier, HeaderLineID, LineIDIndex, TermLineID, WrongTermIndex),
            SingleValuesList,
            CommandLineArgumentsCreator.CreateOutputBuilder(this.matchedOutputFilePath, this.unmatchedOutputFilePath),
            CommandLineArgumentsCreator.CreateLogBuilder(this.applicationLogFilePath, this.jobLogFilePath)
          )
        );

      // Act
      ConsoleRunner.Run(applicationPath, commandLineArguments);

      // Assert
      File.Exists(this.applicationLogFilePath).ShouldBeTrue();
      File.Exists(this.matchedOutputFilePath).ShouldBeFalse();

      File.Exists(this.unmatchedOutputFilePath).ShouldBeTrue();
      StringArrayComparison.IsMatching(
        File.ReadAllLines(this.unmatchedOutputFilePath),
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
      StringArrayComparison.IsMatching(
        File.ReadAllLines(this.jobLogFilePath),
        new String[]
        {
          TestConstants.DateTimeStampRegex + "Run Started...",
          TestConstants.DateTimeStampRegex + "Record found at position 0 with Term '02'.",
          TestConstants.DateTimeStampRegex + "Record found at position 86 with Term '02'.",
          TestConstants.DateTimeStampRegex + Regex.Escape("2 Record(s) processed."),
          TestConstants.DateTimeStampRegex + Regex.Escape("0 Record(s) matched."),
          TestConstants.DateTimeStampRegex + Regex.Escape("2 Record(s) not matched."),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("2 Record(s) processed from input file {0}.", this.inputFilePath)),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("0 Record(s) matched from input file {0}.", this.inputFilePath)),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("2 Record(s) not matched from input file {0}.", this.inputFilePath)),
          TestConstants.DateTimeStampRegex + Regex.Escape(String.Format("2 Record(s) written to output file {0}.", this.unmatchedOutputFilePath)),
          TestConstants.DateTimeStampRegex + "Run Finished.",
        });
    }

    private static void CreateInputFileForDelimitedTests(String resourceFilePath, String inputFilePath)
    {
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFilePath, inputFilePath);
    }
  }
}
