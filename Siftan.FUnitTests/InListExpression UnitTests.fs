namespace Siftan.FUnitTests

open NUnit.Framework
open Siftan
open Jabberwocky.Toolkit.IO
open Foq

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

        let mutable position = 0L
        let mutable fileLineIndex = 0

        Mock<IStreamReader>()
            .Setup(fun x -> <@ x.EndOfStream @>).Returns(fun() -> fileLineIndex = fileLines.Length)
            .Setup(fun x -> <@ x.Position @>).Returns(position)
            .Setup(fun x -> <@ x.ReadLine() @>).Returns(fun() -> 
                if fileLineIndex = fileLines.Length then
                    ()
                    
                position <- position + int64 fileLines.[fileLineIndex].Length
                let result = fileLines.[fileLineIndex]
                fileLineIndex <- fileLineIndex + 1
                if fileLineIndex < fileLines.Length then
                    position <- position + 2L
                result
            )
            .Create()

    [<Test>]
    let ``Mock file reader with one line``() =
        let reader = CreateMockReaderInstance [|"First Line"|]

        Assert.AreEqual(reader.EndOfStream, false)
        Assert.AreEqual(reader.Position, 0L)
        Assert.AreEqual(reader.ReadLine(), "First Line")
        Assert.AreEqual(reader.EndOfStream, true)
        Assert.AreEqual(reader.Position, "First Line".Length)

        
