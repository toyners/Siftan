namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open FsUnit
open SupportFunctions

module ``OneFileRecordWriter Unit Tests`` =

    [<Test>]
    let ``Nonempty matched and unmatched file names returns Matched and Unmatched categories``() = 
        let recordWriter = OneFileRecordWriter(@"C:\matched.txt", @"C:\unmatched.txt")
        recordWriter.DoWriteMatchedRecords |> should equal true
        recordWriter.DoWriteUnmatchedRecords |> should equal true

    [<Test>]
    let ``Null matched file name returns Unmatched category``() =
        let recordWriter = OneFileRecordWriter(null, @"C:\unmatched.txt")
        recordWriter.DoWriteMatchedRecords |> should equal false
        recordWriter.DoWriteUnmatchedRecords |> should equal true

    [<Test>]
    let ``Empty matched file name returns Unmatched category``() =
        let recordWriter = OneFileRecordWriter("", @"C:\unmatched.txt")
        recordWriter.DoWriteMatchedRecords |> should equal false
        recordWriter.DoWriteUnmatchedRecords |> should equal true

    [<Test>]
    let ``Null unmatched file name returns Matched category``() =
        let recordWriter = OneFileRecordWriter(@"C:\matched.txt", null)
        recordWriter.DoWriteMatchedRecords |> should equal true
        recordWriter.DoWriteUnmatchedRecords |> should equal false

    [<Test>]
    let ``Empty unmatched file name returns Matched category``() =
        let recordWriter = OneFileRecordWriter(@"C:\matched.txt", "")
        recordWriter.DoWriteMatchedRecords |> should equal true
        recordWriter.DoWriteUnmatchedRecords |> should equal false

    [<Test>]
    let ``Writing matched record when no matched file is set throws meaningful exception``() =
        let recordWriter = OneFileRecordWriter(null, null)
        let emptyRecord = Record();
        let mockStreamReader = CreateMockReader [||]
        
        (fun() -> recordWriter.WriteMatchedRecord(mockStreamReader, emptyRecord) |> ignore)
        |> should (throwWithMessage "Writer not set to write out matched record.") typeof<System.InvalidOperationException>

    [<Test>]
    let ``Writing unmatched record when no unmatched file is set throws meaningful exception``() =
        let recordWriter = OneFileRecordWriter(null, null)
        let emptyRecord = Record();
        let mockStreamReader = CreateMockReader [||]
        
        (fun() -> recordWriter.WriteUnmatchedRecord(mockStreamReader, emptyRecord) |> ignore)
        |> should (throwWithMessage "Writer not set to write out unmatched record.") typeof<System.InvalidOperationException>
