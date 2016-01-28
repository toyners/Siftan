namespace Siftan.TestSupport.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport

type FileContentAssertionTests() =

    [<Test>]
    member public this.``File containing expected lines in correct order does not throw exception``() =
        // Assert
        let actualFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedFileLines = [|"BBB"; "CCC"|]

        // Act
        (fun () -> FileContentAssertion.IsMatching(actualFileLines, expectedFileLines) |> ignore)
        |> should not' (throw typeof<System.Exception>)

    [<Test>]
    member public this.``File containing expected lines in wrong order does throw meaningful exception``() =
        // Assert
        let actualFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedFileLines = [|"CCC"; "BBB"|]
        
        // Act
        (fun () -> FileContentAssertion.IsMatching(actualFileLines, expectedFileLines) |> ignore)
        |> should (throwWithMessage "Expected file content lines 'BBB' to follow 'CCC' but 'CCC' follows 'BBB'.") typeof<System.Exception>

    [<Test>]
    member public this.``File does not contain expected line throws meaningful exception``() =
        // Assert
        let actualFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedFileLines = [|"DDD"|]
        
        // Act
        (fun () -> FileContentAssertion.IsMatching(actualFileLines, expectedFileLines) |> ignore)
        |> should (throwWithMessage "Missing line 'DDD' from file content.") typeof<System.Exception>
