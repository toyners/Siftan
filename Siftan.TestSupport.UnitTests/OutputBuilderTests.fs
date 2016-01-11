namespace Siftan.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport

type OutputBuilderTests() =

    [<Test>]
    member public this.``Output Builder set with the matched output file name returns correct argument array``() =
        let matchedOutputFilePath = @"C:\Directory\MatchedOutputFile.txt"
        let args = OutputBuilder().HasMatchedOutputFile(matchedOutputFilePath).Build()

        args.Length |> should equal 3
        args.[0] |> should equal OutputBuilder.OutputKey
        args.[1] |> should equal OutputBuilder.MatchedOutputKey
        args.[2] |> should equal matchedOutputFilePath

    [<Test>]
    member public this.``Output Builder set with the unmatched output file name returns correct argument array``() =
        let unmatchedOutputFilePath = @"C:\Directory\UnmatchedOutputFile.txt"
        let args = OutputBuilder().HasUnmatchedOutputFile(unmatchedOutputFilePath).Build()

        args.Length |> should equal 3
        args.[0] |> should equal OutputBuilder.OutputKey
        args.[1] |> should equal OutputBuilder.UnmatchedOutputKey
        args.[2] |> should equal unmatchedOutputFilePath

    [<Test>]
    member public this.``Output Builder set with both matched and unmatched output file names will return correct argument array``() =
        let matchedOutputFilePath = @"C:\Directory\MatchedOutputFile.txt"
        let unmatchedOutputFilePath = @"C:\Directory\UnmatchedOutputFile.txt"
        let args = 
            OutputBuilder()
                .HasMatchedOutputFile(matchedOutputFilePath)
                .HasUnmatchedOutputFile(unmatchedOutputFilePath)
                .Build()

        args.Length |> should equal 5
        args.[0] |> should equal OutputBuilder.OutputKey
        args.[1] |> should equal OutputBuilder.MatchedOutputKey
        args.[2] |> should equal matchedOutputFilePath
        args.[3] |> should equal OutputBuilder.UnmatchedOutputKey
        args.[4] |> should equal unmatchedOutputFilePath