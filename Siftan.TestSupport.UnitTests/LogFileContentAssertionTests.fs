namespace Siftan.TestSupport.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport

type LogFileContentAssertionTests() =

    [<Test>]
    member public this.``Log file lines containing expected lines in correct orders does not throw exception``() =
        // Assert
        let logFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedLogFileLines = [|"BBB"; "CCC"|]

        // Act
        (fun () -> LogFileContentAssertion.IsMatching(logFileLines, expectedLogFileLines) |> ignore)
        |> should not' (throw typeof<System.Exception>)

    [<Test>]
    member public this.``Log file lines contain expected lines but in wrong order throws meaningful exception``() =
        // Assert
        let logFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedLogFileLines = [|"CCC"; "BBB"|]
        
        // Act
        (fun () -> LogFileContentAssertion.IsMatching(logFileLines, expectedLogFileLines) |> ignore)
        |> should (throwWithMessage "Expected log file content lines 'BBB' to follow 'CCC' but 'CCC' follows 'BBB'.") typeof<System.Exception>

    [<Test>]
    member public this.``Log file lines does not contain expected line throws meaningful exception``() =
        // Assert
        let logFileLines = [|"AAA"; "BBB"; "CCC"|]
        let expectedLogFileLines = [|"DDD"|]
        
        // Act
        (fun () -> LogFileContentAssertion.IsMatching(logFileLines, expectedLogFileLines) |> ignore)
        |> should (throwWithMessage "Missing line 'DDD' from log file content.") typeof<System.Exception>
