namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open FsUnit
open SupportFunctions

type StatisticsCollectorUnitTests() =

    member private this.FirstInputFilePath = @"C:\InputFile_1.csv"
    
    member private this.SecondInputFilePath = @"C:\InputFile_2.csv"

    member private this.FirstOutputFilePath = @"C:\OutputFile_1.csv"
    
    member private this.SecondOutputFilePath = @"C:\OutputFile_2.csv"
    
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
    member public this.``Enumerating over input file object after adding record from one file returns expected values``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordIsMatched(this.FirstInputFilePath)

        for inputFileCounter in statisticsCollector.InputFileCounters() do
            inputFileCounter.FilePath |> should equal this.FirstInputFilePath
            inputFileCounter.Matched |> should equal 1u
            inputFileCounter.Unmatched |> should equal 0u

    [<Test>]
    member public this.``Enumerating over input file objects after adding records from different input files returns expected values``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordIsMatched(this.FirstInputFilePath)
        statisticsCollector.RecordIsMatched(this.SecondInputFilePath)

        let mutable expectedInputFilePath = this.FirstInputFilePath

        for inputFileCounter in statisticsCollector.InputFileCounters() do
            inputFileCounter.FilePath |> should equal expectedInputFilePath
            inputFileCounter.Matched |> should equal 1u
            inputFileCounter.Unmatched |> should equal 0u

            // Set the expected file to be the second input file. Not pretty.
            expectedInputFilePath <- this.SecondInputFilePath

    [<Test>]
    member public this.``Adding a written record to one file updates the total counts``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordWrittenToOutputFile(this.FirstOutputFilePath)

        statisticsCollector.TotalWrittenRecords |> should equal 1u

    [<Test>]
    member public this.``Adding a written record to multiple files updates the total counts``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordWrittenToOutputFile(this.FirstOutputFilePath)
        statisticsCollector.RecordWrittenToOutputFile(this.SecondOutputFilePath)

        statisticsCollector.TotalWrittenRecords |> should equal 2u

    [<Test>]
    member public this.``Enumerating over output file object after adding record for one output file returns expected values``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordWrittenToOutputFile(this.FirstOutputFilePath)

        for outputFileCounter in statisticsCollector.OutputFileCounters() do
            outputFileCounter.FilePath |> should equal this.FirstOutputFilePath
            outputFileCounter.Total |> should equal 1u

    [<Test>]
    member public this.``Enumerating over output file objects after adding records for multiple output files returns expected values``() =
        let statisticsCollector = StatisticsCollector()
        statisticsCollector.RecordWrittenToOutputFile(this.FirstOutputFilePath)
        statisticsCollector.RecordWrittenToOutputFile(this.SecondOutputFilePath)

        let mutable expectedInputFilePath = this.FirstOutputFilePath

        for outputFileCounter in statisticsCollector.OutputFileCounters() do
            outputFileCounter.FilePath |> should equal expectedInputFilePath
            outputFileCounter.Total |> should equal 1u

            // Set the expected file to be the second output file. Not pretty.
            expectedInputFilePath <- this.SecondOutputFilePath
