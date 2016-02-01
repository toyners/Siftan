namespace Siftan.TestSupport.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport
open Siftan

type InListBuilder_UnitTests() = 

    [<Test>]
    member public this.``InList Builder set with value file returns correct argument array``() =
        let fileName = @"C:\Directory\ValueFile.txt"
        let args = InListBuilder().HasValuesFile(fileName).Build()

        args.Length |> should equal 3
        args.[0] |> should equal InListBuilder.InListKey
        args.[1] |> should equal InListBuilder.FileKey
        args.[2] |> should equal fileName

    [<Test>]
    member public this.``InList Builder set with value list returns correct argument array``() =
        let values = "A:B:C"
        let args = InListBuilder().HasValuesList(values).Build()

        args.Length |> should equal 3
        args.[0] |> should equal InListBuilder.InListKey
        args.[1] |> should equal InListBuilder.ValuesKey
        args.[2] |> should equal values

    [<Test>]
    member public this.``InList Builder set with default match quota returns correct argument array``() =
        let args = InListBuilder().HasMatchQuota().Build()

        args.Length |> should equal 3
        args.[0] |> should equal InListBuilder.InListKey
        args.[1] |> should equal InListBuilder.QuotaKey
        args.[2] |> should equal (InListExpression.MatchQuotas.None.ToString())

    [<Test>]
    member public this.``InList Builder set with explicit match quota returns correct argument array``() =
        let quota = InListExpression.MatchQuotas.FirstMatchInList
        let expectedQuota = quota.ToString();
        let args = InListBuilder().HasMatchQuota(quota).Build()

        args.Length |> should equal 3
        args.[0] |> should equal InListBuilder.InListKey
        args.[1] |> should equal InListBuilder.QuotaKey
        args.[2] |> should equal expectedQuota

    [<Test>]
    member public this.``InList Builder set with values and explicit match quota returns correct argument array``() =
        let values = "A:B:C"
        let quota = InListExpression.MatchQuotas.FirstMatchInList
        let expectedQuota = quota.ToString();
        let args = InListBuilder().HasValuesList(values).HasMatchQuota(quota).Build()

        args.Length |> should equal 5
        args.[0] |> should equal InListBuilder.InListKey
        args.[1] |> should equal InListBuilder.ValuesKey
        args.[2] |> should equal values
        args.[3] |> should equal InListBuilder.QuotaKey
        args.[4] |> should equal expectedQuota

