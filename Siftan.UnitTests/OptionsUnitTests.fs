namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open Siftan.TestSupport
open FsUnit
open System
open Jabberwocky.Toolkit.Path

[<TestFixture>]
type OptionsUnitTests() =

    let InputFileName = @"C:\InputFile.txt"
    let InputFilePattern = "@C:\*.txt"
    let SearchAllDirectories = "-r "
    let CommandLineForDelimitedRunWithInListFile = @"C:\InputFile.txt delim -d | -q ' -h 01 -li 0 -t 02 -ti 3 inlist -f C:\Values.txt output -fm C:\Output\matched.txt"
    let CommandLineForDelimitedRunWithRecursiveInputPattern = "@C:\*.txt -r delim -d | -q ' -h 01 -li 0 -t 02 -ti 3 inlist -f C:\Values.txt output -fm C:\Output\matched.txt"
    let CommandLineForDelimitedRunWithInListValues = @"C:\InputFile.txt delim -d | -q ' -h 01 -li 0 -t 02 -ti 3 inlist -v A:B:C output -fm C:\Output\matched.txt"
    let CommandLineForDelimitedRunWithDefaults = @"C:\InputFile.txt delim -h 01 -t 02 inlist -f C:\Values.txt output -fm C:\Output\matched.txt"
    let CommandLineForFixedWidthRun = @"C:\InputFile.txt fixed -h 01 -ls 1 -ll 10 -t 02 -ts 12 -tl 11 inlist -f C:\Values.txt output -fm C:\Output\matched.txt"

    let ApplicationLogFilePath = @"C:\Output\siftan.log"
    let JobLogFilePath = @"C:\Output\job.log"
    let NotACommandLineNoun = "NotACommandLineNoun"

    member private this.``Build Command Line for Delimited Run with InList file``() =
        CommandLineArgumentsBuilder()
            .WithInput(InputBuilder()
                .IsSingleFile())
            .WithDelim(DelimBuilder()
                .HasDelimiter("|")
                .HasQualifier('\'')
                .HasHeaderLineID("01")
                .HasLineIDIndex(0u)
                .HasTermLineID("02")
                .HasTermIndex(3u))
            .WithInList(InListBuilder()
                .HasValuesFile(@"C:\Values.txt"))
            .WithOutput(OutputBuilder()
                .HasMatchedOutputFile(@"C:\Output\matched.txt"))
            .Build()

    member private this.``Build Command Line for Delimited Run with Logging Information``(logBuilder) =
        
        let mutable commandLineArgumentsBuilder = 
            CommandLineArgumentsBuilder()
                .WithInput(InputBuilder()
                    .IsSingleFile())
                .WithDelim(DelimBuilder()
                    .HasDelimiter("|")
                    .HasQualifier('\'')
                    .HasHeaderLineID("01")
                    .HasLineIDIndex(0u)
                    .HasTermLineID("02")
                    .HasTermIndex(3u))
                .WithInList(InListBuilder()
                    .HasValuesFile(@"C:\Values.txt"))
                .WithOutput(OutputBuilder()
                    .HasMatchedOutputFile(@"C:\Output\matched.txt"))

        if logBuilder <> null then
            commandLineArgumentsBuilder <- commandLineArgumentsBuilder.WithLog(logBuilder)

        commandLineArgumentsBuilder.Build()

    member private this.``Build Command Line with missing delim noun``() =
        let args = 
            CommandLineArgumentsBuilder()
                .WithInput(InputBuilder()
                    .IsSingleFile())
                .Build()

        Array.append args [| NotACommandLineNoun |]

    [<Test>]
    member public this.``Command line containing input file name returns valid object``() =
        // Act
        let options = CommandLineForFixedWidthRun.Split(' ') |> Options

        // Assert
        options.Input |> should not' (equal null)
        options.Input.Pattern |> should equal InputFileName
        options.Input.SearchSubdirectories |> should equal false

    [<Test>]
    member public this.``Command line containing input pattern and recursive flag name returns valid object``() =
        // Act
        let options = CommandLineForDelimitedRunWithRecursiveInputPattern.Split(' ') |> Options

        // Assert
        options.Input |> should not' (equal null)
        options.Input.Pattern |> should equal InputFilePattern
        options.Input.SearchSubdirectories |> should equal true

    [<Test>]
    member public this.``Command line containing delim returns valid object``() =
        // Act
        let options = this.``Build Command Line for Delimited Run with InList file``() |> Options

        // Assert
        options.Delimited |> should not' (equal null)
        options.Delimited.Delimiter |> should equal "|"
        options.Delimited.Qualifier |> should equal '\''
        options.Delimited.HeaderLineID |> should equal "01"
        options.Delimited.LineIDIndex |> should equal 0
        options.Delimited.TermLineID |> should equal "02"
        options.Delimited.TermIndex |> should equal 3

        options.InList |> should not' (equal null)
        options.InList.FilePath |> should equal @"C:\Values.txt"

        options.Output |> should not' (equal null)
        options.Output.FileMatched |> should equal @"C:\Output\matched.txt"

    [<Test>]
    member public this.``Command line containing no logging information returns default logging object``() =
        // Act
        let options = this.``Build Command Line for Delimited Run with InList file``() |> Options

        options.Log |> should not' (equal null)
        options.Log.ApplicationLogFilePath |> should endWith (@"\" + Options.LogOptions.DefaultApplicationLogFileName)

        let expectedJobLogFilePath = PathOperations.CompleteDirectoryPath(System.IO.Path.GetDirectoryName(options.Output.FileMatched)) + Options.LogOptions.DefaultJobLogFileName
        options.Log.JobLogFilePath |> should equal expectedJobLogFilePath

    [<Test>]
    member public this.``Command line containing custom logging information returns expected logging object``() =
        // Act
        let options = 
            CommandLineArgumentsCreator.CreateLogBuilder (ApplicationLogFilePath, JobLogFilePath)
            |> this.``Build Command Line for Delimited Run with Logging Information`` 
            |> Options

        options.Log |> should not' (equal null)
        options.Log.ApplicationLogFilePath |> should equal ApplicationLogFilePath
        options.Log.JobLogFilePath |> should equal JobLogFilePath

    [<Test>]
    member public this.``Command line containing custom application logging information returns expected logging object``() =
        // Act
        let options = 
            CommandLineArgumentsCreator.CreateLogBuilder (ApplicationLogFilePath, JobLogFilePath)
            |> this.``Build Command Line for Delimited Run with Logging Information`` 
            |> Options

        options.Log |> should not' (equal null)
        options.Log.ApplicationLogFilePath |> should equal ApplicationLogFilePath

        let expectedJobLogFilePath = PathOperations.CompleteDirectoryPath(System.IO.Path.GetDirectoryName(options.Output.FileMatched)) + Options.LogOptions.DefaultJobLogFileName
        options.Log.JobLogFilePath |> should equal expectedJobLogFilePath

    [<Test>]
    member public this.``Command line containing custom job logging information returns expected logging object``() =
        // Act
        let options = 
            CommandLineArgumentsCreator.CreateLogBuilder (ApplicationLogFilePath, JobLogFilePath)
            |> this.``Build Command Line for Delimited Run with Logging Information``
            |> Options

        options.Log |> should not' (equal null)
        options.Log.ApplicationLogFilePath |> should endWith (@"\" + Options.LogOptions.DefaultApplicationLogFileName)
        options.Log.JobLogFilePath |> should equal JobLogFilePath

    [<Test>]
    member public this.``Command line containing inlist file path returns valid object``() =
        // Act
        let options = CommandLineForDelimitedRunWithInListFile.Split(' ') |> Options 

        // Assert
        options.InList |> should not' (equal null)
        options.InList.FilePath |> should equal @"C:\Values.txt"

    [<Test>]
    member public this.``Command line containing inlist values returns valid object``() =
        // Act
        let options = CommandLineForDelimitedRunWithInListValues.Split(' ') |> Options

        // Assert
        options.InList |> should not' (equal null)
        options.InList.Values |> should equal [|"A"; "B"; "C"|]

    [<Test>]
    member public this.``Command line containing output returns valid object``() =
        // Act
        let options = CommandLineForDelimitedRunWithInListFile.Split(' ') |> Options 

        options.Output |> should not' (equal null)
        options.Output.FileMatched |> should equal @"C:\Output\matched.txt"

    [<Test>]
    member public this.``Command line containing minimum delim returns valid object with defaults set``() =
        // Act
        let options = CommandLineForDelimitedRunWithDefaults.Split(' ') |> Options

        // Assert
        options.Delimited |> should not' (equal null)
        options.Delimited.Delimiter |> should equal ","
        options.Delimited.Qualifier |> should equal '\000'
        options.Delimited.HeaderLineID |> should equal "01"
        options.Delimited.LineIDIndex |> should equal 0
        options.Delimited.TermLineID |> should equal "02"
        options.Delimited.TermIndex |> should equal 0

    [<Test>]
    member public this.``Command line containing fixed width returns valid object``() =
        // Act
        let options = CommandLineForFixedWidthRun.Split(' ') |> Options

        // Assert
        options.FixedWidth |> should not' (equal null)
        options.FixedWidth.HeaderLineID |> should equal "01"
        options.FixedWidth.LineIDStart |> should equal 1
        options.FixedWidth.LineIDLength |> should equal 10
        options.FixedWidth.TermLineID |> should equal "02"
        options.FixedWidth.TermStart |> should equal 12
        options.FixedWidth.TermLength |> should equal 11

    [<Test>]
    member public this.``Missing Delimited noun throws meaningful exception``() =
        // Arrange
        let args = this.``Build Command Line with missing delim noun``()
        let expectedExceptionMessage = String.Format(Options.UnrecognisedNounMessageTemplate, NotACommandLineNoun)

        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage expectedExceptionMessage) typeof<System.Exception>

    [<Test>]
    member public this.``Delimited command line segment with bad type throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "delim"; "-d"; "|"; "-h"; "01"; "-li"; "A"; "-t"; "01"; "-ti"; "3"|]

        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Value 'A' cannot be cast to type UInt32.") typeof<System.InvalidCastException>

    [<Test>]
    member public this.``Delimited command line segment with missing required Header ID argument throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "delim"; "-t"; "01" |]

        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Header ID' (-h).") typeof<System.Exception>

    [<Test>]
    member public this.``Delimited command line segment with missing required Term Line ID argument throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "delim"; "-h"; "01"|]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Term Line ID' (-t).") typeof<System.Exception>

    [<Test>]
    member public this.``Delimited command line segment with last missing value throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "delim"; "-d"; "|"; "-h"; "01"; "-li"; "0"; "-t"; "01"; "-ti" |]

        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing value for field '-ti'.") typeof<System.Exception>

    [<Test>]
    member public this.``Qualifier is a string throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "delim"; "-d"; "|"; "-q"; "qual"; "-h"; "-li"; "A"; "-t"; "01"; "-ti"; "3" |]

        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Value 'qual' cannot be cast to type Char.") typeof<System.InvalidCastException>

    [<Test>]
    member public this.``Fixed width command line with missing required Header ID argument throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "fixed"; "-ls"; "1"; "ll"; "10"; "-t"; "02"; "-ts"; "12"; "-tl"; "11" |]

        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Header ID' (-h).") typeof<System.Exception>

    [<Test>]
    member public this.``Fixed width command line with missing required Line ID Start argument throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "fixed"; "-h"; "01"; "-ll"; "10"; "-t"; "02"; "-ts"; "12"; "-tl"; "11" |]

        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Line ID Start' (-ls).") typeof<System.Exception>

    [<Test>]
    member public this.``Fixed width command line with missing required Line ID Length argument throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "fixed"; "-h"; "01"; "-ls"; "1"; "-t"; "02"; "-ts"; "12"; "-tl"; "11" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Line ID Length' (-ll).") typeof<System.Exception>

    [<Test>]
    member public this.``Fixed width command line with missing required Term Line ID argument throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "fixed"; "-h"; "01"; "-ls"; "1"; "-ll"; "10"; "-ts"; "12"; "-tl"; "11" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Term Line ID' (-t).") typeof<System.Exception>

    [<Test>]
    member public this.``Fixed width command line with missing required Term Start argument throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "fixed"; "-h"; "01"; "-ls"; "1"; "-ll"; "10"; "-t"; "02"; "-tl"; "11" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Term Start' (-ts).") typeof<System.Exception>

    [<Test>]
    member public this.``Fixed width command line with missing required Term Length argument throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "fixed"; "-h"; "01"; "-ls"; "1"; "-ll"; "10"; "-t"; "02"; "-ts"; "12" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Term Length' (-tl).") typeof<System.Exception>

    [<Test>]
    member public this.``In list command line segment with missing value list and filepath throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "inlist" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing one required term. Must have 'File Path' (-f) or 'Value List' (-v) but not both.") typeof<System.Exception>

    [<Test>]
    member public this.``In list command line segment with bad enum value throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "inlist"; "-m"; "badvalue" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Value 'badvalue' cannot be cast to type Siftan.InListExpression+MatchQuotas.") typeof<System.Exception>

    [<Test>]
    member public this.``In list command line segment with both value list and filepath throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "inlist"; "-f"; @"C:\Test.txt"; "-v"; "A:B:C" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Cannot have both 'File Path' (-f) and 'Value List' (-v) terms.") typeof<System.Exception>

    [<Test>]
    member public this.``Output command line segment with missing file name throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "output" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required file term. Use either '-fm' or '-fu' or both.") typeof<System.Exception>

    [<Test>]
    member public this.``Command line with missing record descriptor throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "inlist"; "-f"; @"C:\Values.txt"; "output"; "-fm"; @"C:\Output\file.txt" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required record descriptor term. Use either 'delim' or 'fixed'.") typeof<System.Exception>

    [<Test>]
    member public this.``Command line with missing match descriptor throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "delim"; "-h"; "01"; "-t"; "02"; "output"; "-fm"; @"C:\Output\file.txt" |]

        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required match descriptor term. Use 'inlist'.") typeof<System.Exception>

    [<Test>]
    member public this.``Command line with missing output descriptor throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "delim"; "-h"; "01"; "-t"; "02"; "inlist"; "-f"; @"C:\Values.txt" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required output descriptor term. Use 'output'.") typeof<System.Exception>

    [<Test>]
    member public this.``Command line with both delim and fixed record descriptor throws meaningful exception``() =
        // Arrange
        let args = [| @"C:\InputFile.txt"; "delim"; "-h"; "01"; "-t"; "02"; "fixed"; "-h"; "01"; "-ls"; "1"; "-ll"; "10"; "-t"; "02"; "-ts"; "12"; "-tl"; "11"; "inlist"; "-f"; @"C:\Values.txt"; "output"; "-fm"; @"C:\Output\file.txt" |]
        
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Cannot have both 'delim' and 'fixed' record descriptor terms.") typeof<System.Exception>

    [<Test>]
    member public this.``Empty Command line throws meaningful exception``() =
        // Act && Assert
        (fun () -> Options [||] |> ignore)
        |> should (throwWithMessage "No command line arguments.") typeof<System.Exception>
