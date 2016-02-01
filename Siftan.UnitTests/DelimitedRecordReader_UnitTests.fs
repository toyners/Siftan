namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open Jabberwocky.Toolkit.IO
open FsUnit
open System
open SupportFunctions

module public ``DelimitedRecordReader_UnitTests`` =

    [<Test>]
    let ``Mock file reader with one line``() =
        let reader = CreateMockReader [|"First Line"|]

        reader.EndOfStream |> should be False
        reader.Position |> should equal 0

        reader.ReadLine() |> should equal "First Line"
        reader.EndOfStream |> should be True
        reader.Position |> should equal "First Line".Length

    [<Test>]
    let ``Mock file reader with two lines``() =
        let reader = CreateMockReader [|"First Line"; "Second Line"|]

        reader.EndOfStream |> should be False
        reader.Position |> should equal 0

        reader.ReadLine() |> should equal "First Line"
        reader.EndOfStream |> should be False
        reader.Position |> should equal "First Line  ".Length

        reader.ReadLine() |> should equal "Second Line"
        reader.EndOfStream |> should be True
        reader.Position |> should equal "First Line  Second Line".Length

    [<Test>]
    let ``Mock file reader with three lines``() =
        let reader = CreateMockReader [|"First Line"; "Second Line"; "Third Line"|]
        
        reader.EndOfStream |> should be False
        reader.Position |> should equal 0

        reader.ReadLine() |> should equal "First Line"
        reader.EndOfStream |> should be False
        reader.Position |> should equal "First Line  ".Length

        reader.ReadLine() |> should equal "Second Line"
        reader.EndOfStream |> should be False
        reader.Position |> should equal ("First Line  Second Line  ".Length)

        reader.ReadLine() |> should equal "Third Line"
        reader.EndOfStream |> should be True
        reader.Position |> should equal ("First Line  Second Line  Third Line".Length)

    [<Test>]
    [<TestCase([|"Just a line of text"|])>]
    [<TestCase([|"H1,A,B,C"; "L2,A,B,C"; "L3,A,B,C"|])>]
    let ``No recognisable record so null is returned``(fileLines) =
        let fileReader = CreateMockReader fileLines

        // The new keyword is used to stop the warning about readability when creating a type that implements IDisposable
        let recordReader = new DelimitedRecordReader(CreateSimpleRecordDescriptor) 
        recordReader.ReadRecord(fileReader) |> should equal null

    [<Test>]
    [<TestCase(0, 7,  [|"H,A,B,C"|])>]
    [<TestCase(0, 27, [|"H,A,B,C"; "L1,A,B,C"; "L2,A,B,C"|])>]
    [<TestCase(0, 29, [|"H,A,B,C"; "L1,A,B,C"; "L2,A,B,C"; "H,D,E,F"|])>]
    [<TestCase(9, 16, [|"0,A,B,C"; "H,A,B,C"|])>]
    [<TestCase(9, 26, [|"0,A,B,C"; "H,A,B,C"; "L1,A,B,C"|])>]
    [<TestCase(0, 17, [|"H,A,B,C"; "L1,A,B,C"|])>]
    [<TestCase(0, 25, [|"H,A,B,C"; "L1,A,B,C"; ",D,E,F"|])>] // Next record is missing the "H" line id
    let ``File contains good record``(recordStart, recordEnd, fileLines) =
        
        // Arrange
        let fileReader = CreateMockReader fileLines

        // The new keyword is used to stop the warning about readability when instantiating a type that implements IDisposable
        let recordReader = new DelimitedRecordReader(CreateSimpleRecordDescriptor)

        // Act
        let record = recordReader.ReadRecord(fileReader)

        // Assert
        record |> should not' (equal null)
        record.Start |> should equal recordStart
        record.End |> should equal recordEnd

    [<Test>]
    [<TestCase(0, 17, 0u, "|H,1|,|A|,|B|,|C|")>]
    [<TestCase(0, 17, 1u, "|A|,|H,1|,|B|,|C|")>]
    [<TestCase(0, 17, 2u, "|A|,|B|,|H,1|,|C|")>]
    [<TestCase(0, 17, 3u, "|A|,|C|,|B|,|H,1|")>]
    let ``File containing qualifiers returns good record``(recordStart: int64, recordEnd: int64, lineIDIndex: uint32, fileLine: string) =
        
        // Arrange
        let fileLines = fileLine.Split([|'\000'|], StringSplitOptions.RemoveEmptyEntries)
        let fileReader = CreateMockReader fileLines

        // The new keyword is used to stop the warning about readability when instantiating a type that implements IDisposable
        let recordReader = new DelimitedRecordReader(CreateQualifiedRecordDescriptor lineIDIndex)

        // Act
        let record = recordReader.ReadRecord(fileReader)

        // Assert
        record |> should not' (equal null)
        record.Start |> should equal recordStart
        record.End |> should equal recordEnd

    [<Test>]
    [<TestCase("H", 0u, "H")>]
    [<TestCase("H", 1u, "A")>]
    [<TestCase("L1", 3u, "F")>]
    [<TestCase("L2", 2u, "H")>]
    let ``Term property set to expected value from record``(lineID: string, termIndex: uint32, expectedResult: string) =
        // Arrange
        let fileReader = CreateMockReader [|"H,A,B,C"; "L1,D,E,F"; "L2,G,H,I"|]
        let recordReader = new DelimitedRecordReader(CreateCompleteRecordDescriptor 0u lineID termIndex)

        // Act
        let record = recordReader.ReadRecord(fileReader)

        // Assert
        record |> should not' (equal null)
        record.Term |> should equal expectedResult

