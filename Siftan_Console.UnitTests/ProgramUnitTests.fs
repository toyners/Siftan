namespace Siftan_Console.UnitTests

open NUnit.Framework
open Siftan.TestSupport
open Siftan_Console
open FsUnit
open System.IO

[<TestFixture>]
type ProgramUnitTests() =

    let mutable workingDirectory = null
    let mutable delimitedInputFilePath = null;
    let mutable delimitedInputFilePattern = null;
    let mutable matchedDelimitedOutputFilePath = null;
    let mutable unmatchedDelimitedOutputFilePath = null;
    let mutable applicationLogFilePath = null;
    let mutable jobLogFilePath = null;

    [<TestFixtureSetUp>]
    member public this.SetupBeforeAllTests() =
        workingDirectory <- Path.GetTempPath() + @"Siftan.AcceptanceTests\"
        delimitedInputFilePath <- workingDirectory + "Input.csv";
        delimitedInputFilePattern <- workingDirectory + "*.csv";
        matchedDelimitedOutputFilePath <- workingDirectory + "Matched.csv";
        unmatchedDelimitedOutputFilePath <- workingDirectory + "Unmatched.csv";
        applicationLogFilePath <- workingDirectory + "Application.log";
        jobLogFilePath <- workingDirectory + "Job.log";

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
                    CommandLineArgumentsCreator.CreateOutputBuilder(workingDirectory + "Output.csv", null),
                    null)
            
        
        (fun () -> 
        Program.Main(args) |> ignore)
        |> should (throwWithMessage expectedMessage) typeof<System.IO.FileNotFoundException>

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
                    CommandLineArgumentsCreator.CreateOutputBuilder(workingDirectory + "Output.csv", null),
                    null)
            
        
        (fun () -> 
        Program.Main(args) |> ignore)
        |> should (throwWithMessage expectedMessage) typeof<System.IO.FileNotFoundException>
