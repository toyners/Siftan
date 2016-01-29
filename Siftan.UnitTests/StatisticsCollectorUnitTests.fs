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
