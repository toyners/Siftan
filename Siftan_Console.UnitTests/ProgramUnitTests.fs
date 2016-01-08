namespace Siftan_Console.UnitTests

open NUnit.Framework
open Siftan_Console
open FsUnit
open System.IO

[<TestFixture>]
type ProgramUnitTests() =

    let mutable parentDirectory = null

    [<TestFixtureSetUp>]
    member public this.Setup() =
        parentDirectory <- Path.GetTempPath() + Path.GetRandomFileName() + @"\"
        Directory.CreateDirectory(parentDirectory) |> ignore

    [<TestFixtureTearDown>]
    member public this.Teardown() =
        if Directory.Exists(parentDirectory) then
            Directory.Delete(parentDirectory, true)

    [<Test>]
    [<TestCase("FileThatDoesNotExist.csv")>]
    [<TestCase("FileThatDoesNotExist*.csv")>]
    member public this.``Missing input files causes meaningful exception to be thrown``(filePatternThatDoesNotMatchAnyFiles : string) =

        let expectedMessage = "No files found matching pattern '" + parentDirectory + filePatternThatDoesNotMatchAnyFiles + "'."
        let args = [|parentDirectory + filePatternThatDoesNotMatchAnyFiles; "delim"; "-h"; "01"; "-t"; "02"; "inlist"; "-v"; "12345"; "output"; "-fm"; parentDirectory + "Output.csv" |]

        (fun () -> 
        Program.Main(args) |> ignore)
        |> should (throwWithMessage expectedMessage) typeof<System.IO.FileNotFoundException>