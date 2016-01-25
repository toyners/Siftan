namespace Siftan_Console.UnitTests

open NUnit.Framework
open Siftan.TestSupport
open Siftan_Console
open FsUnit
open System.IO
open System.Text.RegularExpressions

[<TestFixture>]
type ProgramUnitTests() =

    let workingDirectory = Path.GetTempPath() + @"Siftan.AcceptanceTests\"
    let delimitedInputFilePath = workingDirectory + "Input.csv";
    let delimitedInputFilePattern = workingDirectory + "*.csv";
    let matchedDelimitedOutputFilePath = workingDirectory + "Matched.csv";
    let unmatchedDelimitedOutputFilePath = workingDirectory + "Unmatched.csv";
    let applicationLogFilePath = workingDirectory + "Application.log";
    let jobLogFilePath = workingDirectory + "Job.log"
    let matchedJobLogFilePath = workingDirectory + "Job.log"
    let unmatchedJobLogFilePath = workingDirectory + "Job.log"

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
            | :? System.Exception -> printfn ""

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
    member public this.``Job log file is created as dictated by custom job log file path passed in``() =
        
        System.IO.File.WriteAllLines(delimitedInputFilePath, [|""|])

        let args = 
            CommandLineArgumentsCreator
                .CreateArgumentsForDelimitedTests(
                    CommandLineArgumentsCreator.CreateSingleFileInputBuilder(delimitedInputFilePath),
                    CommandLineArgumentsCreator.CreateDelimBuilder("|", '\'', "01", 0u, "02", 0u),
                    "12345",
                    CommandLineArgumentsCreator.CreateOutputBuilder(matchedDelimitedOutputFilePath, null),
                    CommandLineArgumentsCreator.CreateLogBuilder(null, jobLogFilePath))

        Program.Main(args)

        File.Exists(jobLogFilePath) |> should be True

    [<Test>]
    member public this.``Job log file is created in same directory as matched output file if no custom job log file path is passed in``() =
        
        System.IO.File.WriteAllLines(delimitedInputFilePath, [|""|])

        let args = 
            CommandLineArgumentsCreator
                .CreateArgumentsForDelimitedTests(
                    CommandLineArgumentsCreator.CreateSingleFileInputBuilder(delimitedInputFilePath),
                    CommandLineArgumentsCreator.CreateDelimBuilder("|", '\'', "01", 0u, "02", 0u),
                    "12345",
                    CommandLineArgumentsCreator.CreateOutputBuilder(matchedDelimitedOutputFilePath, null),
                    null)

        Program.Main(args)

        File.Exists(matchedJobLogFilePath) |> should be True

    [<Test>]
    member public this.``Job log file is created in same directory as unmatched output file if no matched output and no custom job log file path is passed in``() =
        
        System.IO.File.WriteAllLines(delimitedInputFilePath, [|""|])

        let args = 
            CommandLineArgumentsCreator
                .CreateArgumentsForDelimitedTests(
                    CommandLineArgumentsCreator.CreateSingleFileInputBuilder(delimitedInputFilePath),
                    CommandLineArgumentsCreator.CreateDelimBuilder("|", '\'', "01", 0u, "02", 0u),
                    "12345",
                    CommandLineArgumentsCreator.CreateOutputBuilder(null, unmatchedDelimitedOutputFilePath),
                    null)

        Program.Main(args)

        File.Exists(unmatchedJobLogFilePath) |> should be True

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
