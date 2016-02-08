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
  public class FixedWidth_WinFormAcceptanceTests : AcceptanceTestsBase
  {
    private const String InputFileResourcePath = "Siftan.WinForms.AcceptanceTests.FixedWidthRecordFile.txt";

    private const UInt32 LineIDStart = 0;

    private const UInt32 LineIDLength = 2;

    private const String HeaderLineID = "01";

    private const String TermLineID = "02";

    private const UInt32 TermStart = 13;

    private const UInt32 TermLength = 5;

    private const String SingleValuesList = "12345";

    private String inputFileName = null;

    private String matchedOutputFileName = null;

    private String unmatchedOutputFileName = null;

    [TestFixtureSetUp]
    public void SetupBeforeAllTests()
    {
      SetFilePathsForFixedWidthJob("Siftan.WinForms.AcceptanceTests");
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
    public void FixedWidthJobReturnsExpectedOutputFiles()
    {
      var applicationPath = ApplicationPathCreator.GetApplicationPath("Siftan.WinForms");

      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(InputFileResourcePath, this.inputFilePath);

      ProcessStartInfo processStartInfo = new ProcessStartInfo(applicationPath, "-a " + this.applicationLogFilePath);
      Application application = Application.Launch(processStartInfo);

      try
      {
        Window window = application.GetWindow("Siftan");
        var results_TextBox = window.Get<TextBox>("Results_TextBox");

        WindowSetter windowSetter = new WindowSetter(window);
        windowSetter
          .SelectTabPage("RecordDescriptors_TabControl", "Fixed Width")
          .SetTextBoxValue("LineIDStart_TextBox", LineIDStart.ToString())
          .SetTextBoxValue("LineIDLength_TextBox", LineIDLength.ToString())
          .SetTextBoxValue("HeaderLineID_FW_TextBox", HeaderLineID)
          .SetTextBoxValue("TermLineID_FW_TextBox", TermLineID)
          .SetTextBoxValue("TermStart_TextBox", TermStart.ToString())
          .SetTextBoxValue("TermLength_TextBox", TermLength.ToString())
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

        TestFileSupport.AssertFileIsCorrect(
          this.jobLogFilePath,
          new String[]
          {
            TestConstants.DateTimeStampRegex + "Run Started...",
            TestConstants.DateTimeStampRegex + "Record found at position 0 with Term '12345' matches with List Term '12345'.",
            TestConstants.DateTimeStampRegex + "Record found at position 104 with Term '54321'.",
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

    private void AssertMatchedOutputFileIsCorrect()
    {
      TestFileSupport.AssertFileIsCorrect(
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
      TestFileSupport.AssertFileIsCorrect(
        this.unmatchedOutputFilePath,
        new String[]
        {
          "01|Sid|Sample|54321|1.23",
          "02|||54321||",
          "03|||54321||",
          "05|||54321||"
        });
    }
  }
}
