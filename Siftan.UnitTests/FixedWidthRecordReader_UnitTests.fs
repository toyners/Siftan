namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open FsUnit
open SupportFunctions

module ``FixedWidthRecordReader_UnitTests`` =

    [<Test>]
    let ``Read a fixed width record with a valid term``() =
        // Arrange
        let fileLines = [|"01CAR012345Apple"; "02   012346     "; "03   012347     "|]
        let fileReader = CreateMockReader fileLines
        let recordReader = new FixedWidthRecordReader(CreateCompleteFixedWidthRecordDescriptor "02" 5u 6u)

        // Act
        let record = recordReader.ReadRecord(fileReader)

        // Assert
        record |> should not' (equal null)
        record.Start |> should equal 0L
        record.End |> should equal (int64 (fileLines.[0].Length + fileLines.[1].Length + fileLines.[2].Length + 4))
        record.Term |> should equal "012346"

    [<Test>]
    [<TestCase([|"Just a line of text"|])>]
    [<TestCase([|"H1CAR012345Apple"; "H2   012346     "; "H3   012347     "|])>]
    let ``No recognisable record so null is returned``(fileLines) =
        let fileReader = CreateMockReader fileLines

        // The new keyword is used to stop the warning about readability when creating a type that implements IDisposable
        let recordReader = new FixedWidthRecordReader(CreateCompleteFixedWidthRecordDescriptor "02" 0u 2u) 
        recordReader.ReadRecord(fileReader) |> should equal null

    [<Test>]
    [<TestCase(0, 5,  [|"01AAA"|])>]
    [<TestCase(0, 19, [|"01AAA"; "02AAA"; "03AAA"|])>]
    [<TestCase(0, 21, [|"01AAA"; "02AAA"; "03AAA"; "01BBB"|])>]
    [<TestCase(7, 12, [|"00000"; "01AAA"|])>]
    [<TestCase(7, 19, [|"00000"; "01AAA"; "02AAA"|])>]
    let ``Reading records of different lengths returns correct file positions``(recordStart, recordEnd, fileLines) =
        
        // Arrange
        let fileReader = CreateMockReader fileLines

        // The new keyword is used to stop the warning about readability when instantiating a type that implements IDisposable
        let recordReader = new FixedWidthRecordReader(CreateCompleteFixedWidthRecordDescriptor "04" 0u 2u)

        // Act
        let record = recordReader.ReadRecord(fileReader)

        // Assert
        record |> should not' (equal null)
        record.Start |> should equal recordStart
        record.End |> should equal recordEnd