namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open FsUnit

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