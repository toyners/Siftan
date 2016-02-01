
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
  using TestStack.White.UIItems.Finders;
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
      SetFilePathsForDelimitedJob("Siftan.WinForms_AcceptanceTests");
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
    public void RunDelimitedJobReturnsExpectedOutputFiles()
    {
      var applicationPath = ApplicationPathCreator.GetApplicationPath("Siftan.WinForms");

      CreateInputFileForDelimitedTests(InputFileResourcePath, this.inputFilePath);

      ProcessStartInfo processStartInfo = new ProcessStartInfo(applicationPath, this.applicationLogFilePath);
      Application application = Application.Launch(processStartInfo);

      try
      {
        Window window = application.GetWindow("Siftan");

        var delimiterTextBox = GetTextBoxControl(window, "Delimiter_TextBox");
        delimiterTextBox.Text = Delimiter;

        var qualifierTextBox = GetTextBoxControl(window, "Qualifier_TextBox");
        qualifierTextBox.Text = Qualifier.ToString();

        var headerLineIDTextBox = GetTextBoxControl(window, "HeaderLineID_TextBox");
        headerLineIDTextBox.Text = HeaderLineID;

        var lineIDIndex_Spinner = GetSpinnerControl(window, "LineIDIndex_Spinner");
        lineIDIndex_Spinner.Value = LineIDIndex;

        var termLineID_TextBox = GetTextBoxControl(window, "TermLineID_TextBox");
        termLineID_TextBox.Text = TermLineID;

        var termIndex_Spinner = GetSpinnerControl(window, "TermIndex_Spinner");
        termIndex_Spinner.Value = TermIndex;

        var inputDirectory_TextBox = GetTextBoxControl(window, "InputDirectory_TextBox");
        inputDirectory_TextBox.Text = this.workingDirectory;

        var inputFileName_TextBox = GetTextBoxControl(window, "InputFileName_TextBox");
        inputFileName_TextBox.Text = this.inputFileName;

        var outputDirectory_TextBox = GetTextBoxControl(window, "OutputDirectory_TextBox");
        outputDirectory_TextBox.Text = this.workingDirectory;

        var matchedOutputFileName_TextBox = GetTextBoxControl(window, "MatchedOutputFileName_TextBox");
        matchedOutputFileName_TextBox.Text = this.matchedOutputFileName;

        var unmatchedOutputFileName_TextBox = GetTextBoxControl(window, "UnmatchedOutputFileName_TextBox");
        unmatchedOutputFileName_TextBox.Text = this.unmatchedOutputFileName;

        var inlist_TextBox = GetTextBoxControl(window, "InList_TextBox");
        inlist_TextBox.Text = SingleValuesList;

        var start_Button = GetButtonControl(window, "Start_Button");
        start_Button.Click();

        var p = (ProgressBar)GetControl(window, typeof(ProgressBar), "progressBar1");

        var results_TextBox = window.Get<TextBox>("Results_TextBox");

        MethodRunner.RunForDuration(() => { return results_TextBox.Text.Contains("Finished."); });

        // Assert
        this.Assert();
      }
      finally
      {
        application.Close();
      }
    }

    private void Assert()
    {
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

    private static void CreateInputFileForDelimitedTests(String resourceFilePath, String inputFilePath)
    {
      Assembly.GetExecutingAssembly().CopyEmbeddedResourceToFile(resourceFilePath, inputFilePath);
    }

    private static TextBox GetTextBoxControl(Window window, String id)
    {
      return (TextBox)GetControl(window, typeof(TextBox), id);
    }

    private static Spinner GetSpinnerControl(Window window, String id)
    {
      return (Spinner)GetControl(window, typeof(Spinner), id);
    }

    private static Button GetButtonControl(Window window, String id)
    {
      return (Button)GetControl(window, typeof(Button), id);
    }

    private static IUIItem GetControl(Window window, Type controlType, String id)
    {
      SearchCriteria searchCriteria =
        SearchCriteria.
          ByAutomationId(id).
          AndControlType(controlType, WindowsFramework.WinForms);

      var control = window.Get(searchCriteria);
      window.WaitWhileBusy();
      return control;
    }
  }
}
