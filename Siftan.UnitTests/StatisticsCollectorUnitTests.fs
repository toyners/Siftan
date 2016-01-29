namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open FsUnit
open SupportFunctions

type StatisticsCollectorUnitTests() =

    member private this.FirstInputFilePath = @"C:\InputFile_1.csv"
    
    member private this.SecondInputFilePath = @"C:\InputFile_2.csv"
    
    [<Test>]
    member public this.``Adding a matched record updates the total counts``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordIsMatched(this.FirstInputFilePath)

        statisticsCollector.TotalMatchedRecords |> should equal 1u
        statisticsCollector.TotalProcessedRecords |> should equal 1u

    [<Test>]
    member public this.``Adding a unmatched record updates the total counts``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordIsUnmatched(this.FirstInputFilePath)

        statisticsCollector.TotalUnmatchedRecords |> should equal 1u
        statisticsCollector.TotalProcessedRecords |> should equal 1u

    [<Test>]
    member public this.``Adding records from different input files updates the counts``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordIsMatched(this.FirstInputFilePath)
        statisticsCollector.RecordIsMatched(this.SecondInputFilePath)

        statisticsCollector.TotalMatchedRecords |> should equal 2u
        statisticsCollector.TotalProcessedRecords |> should equal 2u

    [<Test>]
    member public this.``Enumerating over collector returns expected values``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordIsMatched(this.FirstInputFilePath)

        for inputFileCounter in statisticsCollector.InputFileCounters() do
            inputFileCounter.FilePath |> should equal this.FirstInputFilePath
            inputFileCounter.Matched |> should equal 1u
            inputFileCounter.Unmatched |> should equal 0u

    [<Test>]
    member public this.``Enumerating over collector after adding records from different input files returns expected values``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordIsMatched(this.FirstInputFilePath)
        statisticsCollector.RecordIsMatched(this.SecondInputFilePath)

        let mutable expectedInputFilePath = this.FirstInputFilePath

        for inputFileCounter in statisticsCollector.InputFileCounters() do
            inputFileCounter.FilePath |> should equal expectedInputFilePath
            inputFileCounter.Matched |> should equal 1u
            inputFileCounter.Unmatched |> should equal 0u

            expectedInputFilePath <- this.SecondInputFilePath
