namespace Siftan_Console.UnitTests

open NUnit.Framework
open Siftan.TestSupport
open Siftan_Console
open FsUnit
open System.IO
open System.Text.RegularExpressions

[<TestFixture>]
type ProgramUnitTests() =

    let mutable workingDirectory = null
    let mutable delimitedInputFilePath = null;
    let mutable delimitedInputFilePattern = null;
    let mutable matchedDelimitedOutputFilePath = null;
    let mutable unmatchedDelimitedOutputFilePath = null;
    let mutable applicationLogFilePath = null;

    [<TestFixtureSetUp>]
    member public this.SetupBeforeAllTests() =
        workingDirectory <- Path.GetTempPath() + @"Siftan.AcceptanceTests\"
        delimitedInputFilePath <- workingDirectory + "Input.csv";
        delimitedInputFilePattern <- workingDirectory + "*.csv";
        matchedDelimitedOutputFilePath <- workingDirectory + "Matched.csv";
        unmatchedDelimitedOutputFilePath <- workingDirectory + "Unmatched.csv";
        applicationLogFilePath <- workingDirectory + "Application.log";

    [<SetUp>]
    member public this.SetupBeforeEachTest() =
        if Directory.Exists(workingDirectory) then
            Directory.Delete(workingDirectory, true)

        Directory.CreateDirectory(workingDirectory) |> ignore

    [<Test>]
    member public this.``Missing single input file causes meaningful exception to be thrown``() =

        let expectedMessage = "No files found matching pattern '" + delimitedInputFilePath + "'."
        let args = 
            CommandLineArgumentsCreator
                .CreateArgumentsForDelimitedTests(
                    CommandLineArgumentsCreator.CreateSingleFileInputBuilder(delimitedInputFilePath),
                    "01",
                    "02",
                    "12345",
                    CommandLineArgumentsCreator.CreateOutputBuilder(matchedDelimitedOutputFilePath, null),
                    null)
        
        (fun () -> 
        Program.Main(args) |> ignore)
        |> should (throwWithMessage expectedMessage) typeof<System.IO.FileNotFoundException>

    [<Test>]
    member public this.``Missing single input file causes meaningful exception to be written to application log``() =

        let args = 
            CommandLineArgumentsCreator.CreateArgumentsForDelimitedTests(
                CommandLineArgumentsCreator.CreateSingleFileInputBuilder(delimitedInputFilePath),
                "01",
                "02",
                "12345",
                CommandLineArgumentsCreator.CreateOutputBuilder(matchedDelimitedOutputFilePath, null),
                CommandLineArgumentsCreator.CreateLogBuilder(applicationLogFilePath, null))
        
        try
            Program.Main(args)
        with
            | :? System.Exception -> printfn "Error"

        let applicationLogFileContents = File.ReadAllLines(applicationLogFilePath)
        let DateTimeStampRegex = @"\A\[\d{2}-\d{2}-\d{4} \d{2}:\d{2}:\d{2}\]"

        LogFileContentAssertion.IsMatching(
            applicationLogFileContents, 
            [|
              DateTimeStampRegex + " EXCEPTION: No files found matching pattern '" + Regex.Escape(delimitedInputFilePath) + "'."
            |])

    [<Test>]
    member public this.``Missing multiple input file pattern causes meaningful exception to be thrown``() =

        let expectedMessage = "No files found matching pattern '" + delimitedInputFilePattern + "'."
        let args = 
            CommandLineArgumentsCreator
                .CreateArgumentsForDelimitedTests(
                    CommandLineArgumentsCreator.CreateMultipleFilesInputBuilder(delimitedInputFilePattern),
                    "01",
                    "02",
                    "12345",
                    CommandLineArgumentsCreator.CreateOutputBuilder(matchedDelimitedOutputFilePath, null),
                    null)
            
        (fun () -> 
        Program.Main(args) |> ignore)
        |> should (throwWithMessage expectedMessage) typeof<System.IO.FileNotFoundException>

    [<Test>]
    member public this.``Test``() =
        
        System.IO.File.WriteAllLines(
            delimitedInputFilePath,
            [|
                "01|Ben|Toynbee|12345|1.23";
                "02|||12345||"; 
                "03|||12345||"; 
                "03|||12345||"; 
                "05|||12345||"; 
                "01|Sid|Sample|54321|1.23"; 
                "02|||54321||"; 
                "03|||54321||"; 
                "05|||54321||"|])

        let args = 
            CommandLineArgumentsCreator
                .CreateArgumentsForDelimitedTests(
                    CommandLineArgumentsCreator.CreateSingleFileInputBuilder(delimitedInputFilePath),
                    CommandLineArgumentsCreator.CreateDelimBuilder("|", '\'', "01", 0u, "02", 0u),
                    "12345",
                    CommandLineArgumentsCreator.CreateOutputBuilder(matchedDelimitedOutputFilePath, null),
                    null)

        Program.Main(args)
