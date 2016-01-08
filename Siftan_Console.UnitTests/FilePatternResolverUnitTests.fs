namespace Siftan_Console.UnitTests

open NUnit.Framework
open Siftan_Console
open FsUnit
open System.IO

[<TestFixture>]
type FilePatternResolverUnitTests() =
    
    let FileContent = ""
    let mutable parentDirectory = null
    let mutable subDirectory = null
    let mutable firstFileName = null
    let mutable secondFileName = null
    let mutable thirdFileName = null
    let mutable fourthFileName = null

    [<TestFixtureSetUp>]
    member public this.Setup() =
        parentDirectory <- Path.GetTempPath() + Path.GetRandomFileName() + @"\"
        Directory.CreateDirectory(parentDirectory) |> ignore

        firstFileName <- parentDirectory + "File1A.txt"
        File.WriteAllText(firstFileName, FileContent)

        secondFileName <- parentDirectory + "File2B.txt"
        File.WriteAllText(secondFileName, FileContent)

        thirdFileName <- parentDirectory + "File3C.txt"
        File.WriteAllText(thirdFileName, FileContent)

        subDirectory <- parentDirectory + @"Sub\"
        Directory.CreateDirectory(subDirectory) |> ignore

        fourthFileName <- subDirectory + "File4D.txt"
        File.WriteAllText(fourthFileName, FileContent)

    [<TestFixtureTearDown>]
    member public this.Teardown() =
        if Directory.Exists(parentDirectory) then
            Directory.Delete(parentDirectory, true)

    [<Test>]
    member public this.``Matches all files in the inital directory``() =
        
        let inputFileList = FilePatternResolver().ResolveFilePattern(parentDirectory + "File*.txt", FilePatternResolver.SearchDepths.InitialDirectoryOnly)
        
        inputFileList.Length |> should equal 3
        inputFileList |> should contain firstFileName
        inputFileList |> should contain secondFileName
        inputFileList |> should contain thirdFileName

    [<Test>]
    member public this.``Matches all files in all directories``() =
        
        let inputFileList = FilePatternResolver().ResolveFilePattern(parentDirectory + "File*.txt", FilePatternResolver.SearchDepths.AllDirectories)
        
        inputFileList.Length |> should equal 4
        inputFileList |> should contain firstFileName
        inputFileList |> should contain secondFileName
        inputFileList |> should contain thirdFileName
        inputFileList |> should contain fourthFileName

    [<Test>]
    member public this.``Matches some files in the initial directory``() =

        let inputFileList = FilePatternResolver().ResolveFilePattern(parentDirectory + "File1*.txt", FilePatternResolver.SearchDepths.InitialDirectoryOnly)

        inputFileList.Length |> should equal 1
        inputFileList |> should contain firstFileName

    [<Test>]
    member public this.``Matches no files in the initial directory``() =

        let inputFileList = FilePatternResolver().ResolveFilePattern(parentDirectory + "File9*.txt", FilePatternResolver.SearchDepths.InitialDirectoryOnly)

        inputFileList.Length |> should equal 0

    [<Test>]
    member public this.``Matches file in sub directory``() =
        
        let inputFileList = FilePatternResolver().ResolveFilePattern(parentDirectory + "File4*.txt", FilePatternResolver.SearchDepths.AllDirectories)

        inputFileList.Length |> should equal 1
        inputFileList |> should contain fourthFileName

        //let commandLine = @"C:\Users\Benjamin\AppData\Local\Temp\r10ngnf0.ukj\input_file.csv delim -h 01 -t 02 inlist -v 12345 output -fm C:\Users\Benjamin\AppData\Local\Temp\r10ngnf0.ukj\matched_output_file.csv"
        //commandLine |> ignore