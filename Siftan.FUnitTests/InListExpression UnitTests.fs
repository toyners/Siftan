namespace Siftan.FUnitTests

open NUnit.Framework
open Siftan
open Jabberwocky.Toolkit.IO

module ``InListExpression UnitTests`` =

    [<Test>]
    [<TestCase("A", true)>]
    [<TestCase("C", false)>]
    let ``IsMatch returns expected result for different terms``(term: string, expectedResult: bool) =
        let exp = InListExpression([| "A"; "B" |])
        let record = Record(Term = term)
        let result = exp.IsMatch(record)
        Assert.AreEqual(result, expectedResult)

    [<Test>]
    let ``HasReachedMatchQuota is true after first match in list``() =
        let exp = InListExpression([| "A"; "B" |], InListExpression.MatchQuotas.FirstMatchInList)
        let record = Record(Term = "A")
        exp.IsMatch(record) |> ignore
        Assert.AreEqual(exp.HasReachedMatchQuota, true)

    [<Test>]
    [<TestCase(InListExpression.MatchQuotas.FirstMatchOfEachTermInList, true)>]
    [<TestCase(InListExpression.MatchQuotas.None, false)>]
    let ``HasReachedMatchQuota is correct after all terms are matched at least once``(matchQuota: InListExpression.MatchQuotas, expectedResult: bool) =
        let exp = InListExpression([| "A"; "B"; "C" |], matchQuota)
        Assert.AreEqual(exp.HasReachedMatchQuota, false)

        exp.IsMatch(Record(Term = "A")) |> ignore
        exp.IsMatch(Record(Term = "B")) |> ignore
        exp.IsMatch(Record(Term = "A")) |> ignore
        Assert.AreEqual(exp.HasReachedMatchQuota, false)

        exp.IsMatch(Record(Term = "C")) |> ignore
        Assert.AreEqual(exp.HasReachedMatchQuota, expectedResult)

    let VerifyArrayIsNotEmpty(fileLines: string[], exceptionMessage: string) = 
        if fileLines = null || fileLines.Length = 0 then
            raise (System.Exception(exceptionMessage))

    (*let MockReader(fileLines: string[]) =
    {
        new IStreamReader with member x.EndOfStream = 
    }*)

    let CreateMockReaderInstance(fileLines: string[]) =
        VerifyArrayIsNotEmpty(fileLines, "Parameter 'fileLines' is null or empty.")

        let position = 0
        let fileLineIndex = 0
        ignore
        (*let MockReader(fileLines: string[]) =
        {
            new IStreamReader with 
                member this.EndOfStream = 
                    fileLineIndex = fileLines.Length
                //member this.ReadLine() = 
                    
        }

        let obj2 = MockReader([|"ben"|])*)
        
