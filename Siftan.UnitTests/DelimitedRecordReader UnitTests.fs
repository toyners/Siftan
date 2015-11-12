namespace Siftan.FUnitTests

open NUnit.Framework
open Siftan
open Jabberwocky.Toolkit.IO
open FsUnit
open System
//open Foq

module ``DelimitedRecordReader UnitTests`` =

    let CreateMockReader(fileLines: string[]) =
        let mutable fileLineIndex = 0

        {
            new Jabberwocky.Toolkit.IO.IStreamReader with
            
                member this.Position
                    with get() = 
                        let mutable position = 0
                        for i = 0 to fileLineIndex - 1 do
                            position <- position + fileLines.[i].Length

                        if fileLines.Length > 1 then
                            let linesConsumed = if fileLineIndex = fileLines.Length then fileLines.Length - 1 else fileLineIndex
                            position <- position + (2 * linesConsumed)

                        int64 position
                    and set(v: int64) = ()

                member this.EndOfStream = 
                    fileLineIndex = fileLines.Length

                member this.ReadLine() =
                    let result = fileLines.[fileLineIndex]
                    fileLineIndex <- fileLineIndex + 1
                    result

                member this.Close() = ()

                member this.Dispose() = ()
        }

    let CreateRecordDescriptor delimiter qualifier lineIDIndex headerID =
       DelimitedRecordDescriptor(Delimiter = delimiter, Qualifier = qualifier, LineIDIndex = lineIDIndex, HeaderID = headerID)

    // '\000' is equivalent to '\0' in C#
    let CreateSimpleRecordDescriptor = CreateRecordDescriptor "," '\000' 0u "H"

    // Create a record descriptor for qualified records
    let CreateQualifiedRecordDescriptor(lineIDIndex: uint32) = CreateRecordDescriptor "," '|' lineIDIndex "H,1"

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

    (*
    let CreateMockReaderInstance(fileLines: string[]) =
        let mutable fileLineIndex = 0

        Mock<IStreamReader>()
            .Setup(fun x -> <@ x.EndOfStream @>).Returns(fun() -> fileLineIndex = fileLines.Length)
            .Setup(fun x -> <@ x.Position @>).Returns(fun() ->
                let mutable position = 0
                for i = 0 to fileLineIndex - 1 do
                    position <- position + fileLines.[i].Length

                if fileLineIndex > 0 then
                    position <- position + (2 * (fileLineIndex - 1))

                int64 position
            )
            .Setup(fun x -> <@ x.ReadLine() @>).Returns(fun() -> 
                if fileLineIndex = fileLines.Length then
                    ()
                    
                let result = fileLines.[fileLineIndex]
                fileLineIndex <- fileLineIndex + 1
                result
            )
            .Create()
    *)
