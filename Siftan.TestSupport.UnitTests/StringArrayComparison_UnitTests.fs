namespace Siftan.TestSupport.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport

type StringArrayComparison_UnitTests() =

    [<Test>]
    member public this.``File containing expected lines in correct order does not throw exception``() =
        // Assert
        let actualFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedFileLines = [|"BBB"; "CCC"|]

        // Act
        (fun () -> StringArrayComparison.IsMatching(actualFileLines, expectedFileLines) |> ignore)
        |> should not' (throw typeof<System.Exception>)

    [<Test>]
    member public this.``File containing expected lines in wrong order does throw meaningful exception``() =
        // Assert
        let actualFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedFileLines = [|"CCC"; "BBB"|]
        
        // Act
        (fun () -> StringArrayComparison.IsMatching(actualFileLines, expectedFileLines) |> ignore)
        |> should (throwWithMessage "Expected line 'BBB' to follow 'CCC' but 'CCC' follows 'BBB'.") typeof<System.Exception>

    [<Test>]
    member public this.``File does not contain expected line throws meaningful exception``() =
        // Assert
        let actualFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedFileLines = [|"DDD"|]
        
        // Act
        (fun () -> StringArrayComparison.IsMatching(actualFileLines, expectedFileLines) |> ignore)
        |> should (throwWithMessage "Missing line 'DDD' from array.") typeof<System.Exception>

    [<Test>]
    member public this.``File containing duplicate lines does not throw meaningful exception``() =
        // Assert
        let actualFileLines = [|"AAA"; "BBB"; "AAA"; "BBB"|]
        let expectedFileLines = [|"AAA"; "BBB"; "AAA"; "BBB"|]
        
        // Act
        (fun () -> StringArrayComparison.IsMatching(actualFileLines, expectedFileLines) |> ignore)
        |> should not' (throw typeof<System.Exception>)

    [<Test>]
    member public this.``File containing duplicate lines does not throw meaningful exception1``() =
        // Assert
        let actualFileLines = [|"AAA"; "BBB"; "AAA"; "BBB"|]
        let expectedFileLines = [|"AAA"; "BBB"; "BBB"; "AAA"|]
        
        // Act
        (fun () -> StringArrayComparison.IsMatching(actualFileLines, expectedFileLines) |> ignore)
        |> should (throwWithMessage "Expected line 'AAA' to follow 'BBB' but 'BBB' follows 'AAA'.") typeof<System.Exception>

