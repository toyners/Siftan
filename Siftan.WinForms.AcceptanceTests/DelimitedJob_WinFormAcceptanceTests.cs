﻿
namespace Siftan.WinForms.AcceptanceTests
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Reflection;
  using System.Text.RegularExpressions;
  using Jabberwocky.Toolkit.Assembly;
  using NUnit.Framework;
  using Shouldly;
  using TestStack.White;
  using TestStack.White.UIItems;
  using TestStack.White.UIItems.WindowItems;
  using TestSupport;

  [TestFixture]
  public class DelimitedJob_WinFormAcceptanceTests : AcceptanceTestsBase
  {
    private const String InputFileResourcePath = "Siftan.WinForms.AcceptanceTests.DelimitedRecordFile.csv";

    private const String Delimiter = "|";

    private const Char Qualifier = '\'';

    private const String HeaderLineID = "01";

    private const UInt32 LineIDIndex = 0;

    private const String TermLineID = "02";

    private const UInt32 TermIndex = 3;

    private const String SingleValuesList = "12345";

    private String inputFileName = null;

    private String matchedOutputFileName = null;

    private String unmatchedOutputFileName = null;

    [TestFixtureSetUp]
    public void SetupBeforeAllTests()
    {
      SetFilePathsForDelimitedJob("Siftan.WinForms.AcceptanceTests");
      this.inputFileName = Path.GetFileName(this.inputFilePath);
      this.matchedOutputFileName = Path.GetFileName(this.matchedOutputFilePath);
      this.unmatchedOutputFileName = Path.GetFileName(this.unmatchedOutputFilePath);
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
      CreateEmptyWorkingDirectory();
    }

    [Test]
    public void DelimitedJobReturnsExpectedOutputFiles()
    {
      var applicationPath = ApplicationPathCreator.GetApplicationPath("Siftan.WinForms");

      CreateInputFileForDelimitedTests(InputFileResourcePath, this.inputFilePath);

      ProcessStartInfo processStartInfo = new ProcessStartInfo(applicationPath, this.applicationLogFilePath);
      Application application = Application.Launch(processStartInfo);

      try
      {
        Window window = application.GetWindow("Siftan");
        var results_TextBox = window.Get<TextBox>("Results_TextBox");

        WindowSetter windowSetter = new WindowSetter(window);
        windowSetter
          .SetTextBoxValue("Delimiter_TextBox", Delimiter)
          .SetTextBoxValue("Qualifier_TextBox", Qualifier.ToString())
          .SetTextBoxValue("HeaderLineID_TextBox", HeaderLineID)
          .SetSpinnerValue("LineIDIndex_Spinner", LineIDIndex)
          .SetTextBoxValue("TermLineID_TextBox", TermLineID)
          .SetSpinnerValue("TermIndex_Spinner", TermIndex)
          .SetTextBoxValue("InputDirectory_TextBox", this.workingDirectory)
          .SetTextBoxValue("InputFileName_TextBox", this.inputFileName)
          .SetTextBoxValue("OutputDirectory_TextBox", this.workingDirectory)
          .SetTextBoxValue("MatchedOutputFileName_TextBox", this.matchedOutputFileName)
          .SetCheckBoxChecked("CreateUnmatchedOutput_CheckBox", true)
          .SetTextBoxValue("UnmatchedOutputFileName_TextBox", this.unmatchedOutputFileName)
          .SetTextBoxValue("InList_TextBox", SingleValuesList)
          .ClickButton("Start_Button");

        MethodRunner.RunForDuration(() => { return results_TextBox.Text.Contains("Finished."); });

        // Assert
        File.Exists(this.applicationLogFilePath).ShouldBeTrue();
        this.AssertMatchedOutputFileIsCorrect();
        this.AssertUnmatchedOutputFileIsCorrect();

        AssertFileIsCorrect(
          this.jobLogFilePath,
          new String[]
          {
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
      finally
      {
        application.Close();
      }
    }

    [Test]
    public void DelimitedJobReturnsMatchedOutputFileOnly()
    {
      var applicationPath = ApplicationPathCreator.GetApplicationPath("Siftan.WinForms");

      CreateInputFileForDelimitedTests(InputFileResourcePath, this.inputFilePath);

      ProcessStartInfo processStartInfo = new ProcessStartInfo(applicationPath, this.applicationLogFilePath);
      Application application = Application.Launch(processStartInfo);

      try
      {
        Window window = application.GetWindow("Siftan");
        var results_TextBox = window.Get<TextBox>("Results_TextBox");

        WindowSetter windowSetter = new WindowSetter(window);
        windowSetter
          .SetTextBoxValue("Delimiter_TextBox", Delimiter)
          .SetTextBoxValue("Qualifier_TextBox", Qualifier.ToString())
          .SetTextBoxValue("HeaderLineID_TextBox", HeaderLineID)
          .SetSpinnerValue("LineIDIndex_Spinner", LineIDIndex)
          .SetTextBoxValue("TermLineID_TextBox", TermLineID)
          .SetSpinnerValue("TermIndex_Spinner", TermIndex)
          .SetTextBoxValue("InputDirectory_TextBox", this.workingDirectory)
          .SetTextBoxValue("InputFileName_TextBox", this.inputFileName)
          .SetTextBoxValue("OutputDirectory_TextBox", this.workingDirectory)
          .SetTextBoxValue("MatchedOutputFileName_TextBox", this.matchedOutputFileName)
          .SetCheckBoxChecked("CreateUnmatchedOutput_CheckBox", false)
          .SetTextBoxValue("UnmatchedOutputFileName_TextBox", String.Empty)
          .SetTextBoxValue("InList_TextBox", SingleValuesList)
          .ClickButton("Start_Button");

        MethodRunner.RunForDuration(() => { return results_TextBox.Text.Contains("Finished."); });

        // Assert
        File.Exists(this.applicationLogFilePath).ShouldBeTrue();
        this.AssertMatchedOutputFileIsCorrect();

        AssertFileIsCorrect(
          this.jobLogFilePath,
          new String[]
          {
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
            TestConstants.DateTimeStampRegex + "Run Finished.",
          });
      }
      finally
      {
        application.Close();
      }
    }

    private void AssertMatchedOutputFileIsCorrect()
    {
      AssertFileIsCorrect(
        this.matchedOutputFilePath,
        new String[]
        {
          "01|Ben|Toynbee|12345|1.23",
          "02|||12345||",
          "03|||12345||",
          "03|||12345||",
          "05|||12345||"
        });
    }

    private void AssertUnmatchedOutputFileIsCorrect()
    {
      AssertFileIsCorrect(
        this.unmatchedOutputFilePath,
        new String[]
        {
          "01|Sid|Sample|54321|1.23",
          "02|||54321||",
          "03|||54321||",
          "05|||54321||"
        });
    }

    private static void CreateInputFileForDelimitedTests(String resourceFilePath, String inputFilePath)
    {
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFilePath, inputFilePath);
    }

    private static void AssertFileIsCorrect(String filePath, String[] expectedFileLines)
    {
      File.Exists(filePath).ShouldBeTrue(String.Format("File '{0}' does not exist", filePath));
      StringArrayComparison.IsMatching(File.ReadAllLines(filePath), expectedFileLines);
    }
  }
}
