namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open Jabberwocky.Toolkit.IO
open FsUnit

module ``InListExpression_UnitTests`` =

    [<Test>]
    [<TestCase("A", true)>]
    [<TestCase("C", false)>]
    let ``IsMatch returns expected result for different terms``(term: string, expectedResult: bool) =
        let exp = InListExpression([| "A"; "B" |])
        let record = Record(Term = term)
        exp.IsMatch(record) |> should equal expectedResult

    [<Test>]
    let ``HasReachedMatchQuota is true after first match in list``() =
        let exp = InListExpression([| "A"; "B" |], InListExpression.MatchQuotas.FirstMatchInList)
        let record = Record(Term = "A")
        exp.IsMatch(record) |> ignore
        exp.HasReachedMatchQuota |> should be True

    [<Test>]
    [<TestCase(InListExpression.MatchQuotas.FirstMatchOfEachTermInList, true)>]
    [<TestCase(InListExpression.MatchQuotas.None, false)>]
    let ``HasReachedMatchQuota is correct after all terms are matched at least once``(matchQuota: InListExpression.MatchQuotas, expectedResult: bool) =
        let exp = InListExpression([| "A"; "B"; "C" |], matchQuota)
        exp.HasReachedMatchQuota |> should be False

        exp.IsMatch(Record(Term = "A")) |> ignore
        exp.IsMatch(Record(Term = "B")) |> ignore
        exp.IsMatch(Record(Term = "A")) |> ignore
        exp.HasReachedMatchQuota |> should be False

        exp.IsMatch(Record(Term = "C")) |> ignore
        exp.HasReachedMatchQuota |> should equal expectedResult