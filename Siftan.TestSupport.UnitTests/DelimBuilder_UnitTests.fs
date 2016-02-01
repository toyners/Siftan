namespace Siftan.TestSupport.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport

type DelimBuilder_UnitTests() = 

    [<Test>]
    member public this.``Delim Builder set with default Delimiter returns correct argument array``() =
        let args = DelimBuilder().HasDelimiter().Build()

        args.Length |> should equal 3
        args.[0] |> should equal DelimBuilder.DelimKey
        args.[1] |> should equal DelimBuilder.DelimiterKey
        args.[2] |> should equal DelimBuilder.DefaultDelimiter

    [<Test>]
    member public this.``Delim Builder set with custom Delimiter returns correct argument array``() =
        let customDelimiter = "|"
        let args = DelimBuilder().HasDelimiter(customDelimiter).Build()

        args.Length |> should equal 3
        args.[0] |> should equal DelimBuilder.DelimKey
        args.[1] |> should equal DelimBuilder.DelimiterKey
        args.[2] |> should equal customDelimiter

    [<Test>]
    member public this.``Delim Builder set with Qualifier returns correct argument array``() =
        let qualifier = '\''
        let expectedQualifier = qualifier.ToString()
        let args = DelimBuilder().HasQualifier(qualifier).Build()

        args.Length |> should equal 3
        args.[0] |> should equal DelimBuilder.DelimKey
        args.[1] |> should equal DelimBuilder.QualifierKey
        args.[2] |> should equal expectedQualifier

    [<Test>]
    member public this.``Delim Builder set with Header Line ID returns correct argument array``() =
        let headerLineID = "01"
        let args = DelimBuilder().HasHeaderLineID(headerLineID).Build()

        args.Length |> should equal 3
        args.[0] |> should equal DelimBuilder.DelimKey
        args.[1] |> should equal DelimBuilder.HeaderLineIDKey
        args.[2] |> should equal headerLineID

    [<Test>]
    member public this.``Delim Builder set with Line ID Index returns correct argument array``() =
        let lineIDIndex = 1u
        let expectedLineIDIndex = lineIDIndex.ToString()
        let args = DelimBuilder().HasLineIDIndex(lineIDIndex).Build()

        args.Length |> should equal 3
        args.[0] |> should equal DelimBuilder.DelimKey
        args.[1] |> should equal DelimBuilder.LineIDIndexKey
        args.[2] |> should equal expectedLineIDIndex
        
    [<Test>]
    member public this.``Delim Builder set with Term Line ID returns correct argument array``() =
        let termLineID = "01"
        let args = DelimBuilder().HasTermLineID(termLineID).Build()

        args.Length |> should equal 3
        args.[0] |> should equal DelimBuilder.DelimKey
        args.[1] |> should equal DelimBuilder.TermLineIDKey
        args.[2] |> should equal termLineID

    [<Test>]
    member public this.``Delim Builder set with Term Index returns correct argument array``() =
        let termIndex = 1u
        let expectedTermIndex = termIndex.ToString()
        let args = DelimBuilder().HasTermIndex(termIndex).Build()

        args.Length |> should equal 3
        args.[0] |> should equal DelimBuilder.DelimKey
        args.[1] |> should equal DelimBuilder.TermIndexKey
        args.[2] |> should equal expectedTermIndex

    [<Test>]
    member public this.``Delim Builder set with all options returns correct argument array``() =
        let customDelimiter = "|"
        let qualifier = '\''
        let expectedQualifier = qualifier.ToString()
        let headerLineID = "01"
        let lineIDIndex = 1u
        let expectedLineIDIndex = lineIDIndex.ToString()
        let termLineID = "02"
        let termIndex = 1u
        let expectedTermIndex = termIndex.ToString()

        let args = 
            DelimBuilder().HasDelimiter(customDelimiter)
                .HasQualifier(qualifier)
                .HasHeaderLineID(headerLineID)
                .HasLineIDIndex(lineIDIndex)
                .HasTermLineID(termLineID)
                .HasTermIndex(termIndex)
                .Build()

        args.Length |> should equal 13
        args.[0] |> should equal DelimBuilder.DelimKey
        args.[1] |> should equal DelimBuilder.DelimiterKey
        args.[2] |> should equal customDelimiter
        args.[3] |> should equal DelimBuilder.QualifierKey
        args.[4] |> should equal expectedQualifier
        args.[5] |> should equal DelimBuilder.HeaderLineIDKey
        args.[6] |> should equal headerLineID
        args.[7] |> should equal DelimBuilder.LineIDIndexKey
        args.[8] |> should equal expectedLineIDIndex
        args.[9] |> should equal DelimBuilder.TermLineIDKey
        args.[10] |> should equal termLineID
        args.[11] |> should equal DelimBuilder.TermIndexKey
        args.[12] |> should equal expectedTermIndex