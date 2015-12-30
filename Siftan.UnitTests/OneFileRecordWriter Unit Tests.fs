namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open FsUnit

module ``OneFileRecordWriter Unit Tests`` =

    [<Test>]
    let ``Nonempty matched and unmatched file names returns Matched and Unmatched categories``() = 
        let recordWriter = OneFileRecordWriter(@"C:\matched.txt", @"C:\unmatched.txt")
        recordWriter.Categories |> should equal (RecordCategory.Matched + RecordCategory.Unmatched)

    [<Test>]
    let ``Null matched file name returns Unmatched category``() =
        let recordWriter = OneFileRecordWriter(null, @"C:\unmatched.txt")
        recordWriter.Categories |> should equal RecordCategory.Unmatched

    [<Test>]
    let ``Empty matched file name returns Unmatched category``() =
        let recordWriter = OneFileRecordWriter("", @"C:\unmatched.txt")
        recordWriter.Categories |> should equal RecordCategory.Unmatched

    [<Test>]
    let ``Null unmatched file name returns Matched category``() =
        let recordWriter = OneFileRecordWriter(@"C:\matched.txt", null)
        recordWriter.Categories |> should equal RecordCategory.Matched

    [<Test>]
    let ``Empty unmatched file name returns Matched category``() =
        let recordWriter = OneFileRecordWriter(@"C:\matched.txt", "")
        recordWriter.Categories |> should equal RecordCategory.Matched