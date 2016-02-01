namespace Siftan.TestSupport.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport
open Siftan

type LogBuilder_UnitTests() = 

    [<Test>]
    member public this.``Log Builder set with application log file path returns correct argument array``() =
        let applicationLogFilePath = @"C:\Directory\Siftan.log"
        let args = LogBuilder().HasApplicationLogFilePath(applicationLogFilePath).Build()

        args.Length |> should equal 3
        args.[0] |> should equal LogBuilder.LogKey
        args.[1] |> should equal LogBuilder.ApplicationKey
        args.[2] |> should equal applicationLogFilePath

    [<Test>]
    member public this.``Log Builder set with job log file path returns correct argument array``() =
        let jobLogFilePath = @"C:\Directory\Job.log"
        let args = LogBuilder().HasJobLogFilePath(jobLogFilePath).Build()

        args.Length |> should equal 3
        args.[0] |> should equal LogBuilder.LogKey
        args.[1] |> should equal LogBuilder.JobKey
        args.[2] |> should equal jobLogFilePath

    [<Test>]
    member public this.``Log Builder set with application log file patht and job log file path returns correct argument array``() =
        let applicationLogFilePath = @"C:\Directory\Siftan.log"
        let jobLogFilePath = @"C:\Directory\Job.log"
        let args = 
            LogBuilder()
                .HasApplicationLogFilePath(applicationLogFilePath)
                .HasJobLogFilePath(jobLogFilePath)
                .Build()

        args.Length |> should equal 5
        args.[0] |> should equal LogBuilder.LogKey
        args.[1] |> should equal LogBuilder.ApplicationKey
        args.[2] |> should equal applicationLogFilePath
        args.[3] |> should equal LogBuilder.JobKey
        args.[4] |> should equal jobLogFilePath
     
