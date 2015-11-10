namespace Siftan.FUnitTests

open NUnit.Framework
open Siftan
open Jabberwocky.Toolkit.IO
open Foq
open FsUnit

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

    let CreateMockReader(fileLines: string[]) =
        let mutable fileLineIndex = 0

        {
            new Jabberwocky.Toolkit.IO.IStreamReader with
            
                member this.Position
                    with get() = 
                        let mutable position = 0
                        for i = 0 to fileLineIndex - 1 do
                            position <- position + fileLines.[i].Length

                        if fileLineIndex > 0 then
                            position <- position + (2 * (fileLineIndex - 1))

                        int64 position
                    and set(v: int64) = ()

                member this.EndOfStream = 
                    fileLineIndex = fileLines.Length

                member this.ReadLine() =
                    let result = fileLines.[fileLineIndex]
                    fileLineIndex <- fileLineIndex + 1
                    result

                member this.Close() = ()

                member this.Dispose() = ()
        }

    let CreateMockReaderInstance(fileLines: string[]) =
        VerifyArrayIsNotEmpty(fileLines, "Parameter 'fileLines' is null or empty.")

        let mutable fileLineIndex = 0

        Mock<IStreamReader>()
            .Setup(fun x -> <@ x.EndOfStream @>).Returns(fun() -> fileLineIndex = fileLines.Length)
            .Setup(fun x -> <@ x.Position @>).Returns(fun() ->
                let mutable position = 0
                for i = 0 to fileLineIndex - 1 do
                    position <- position + fileLines.[i].Length

                if fileLineIndex > 0 then
                    position <- position + (2 * (fileLineIndex - 1))

                int64 position
            )
            .Setup(fun x -> <@ x.ReadLine() @>).Returns(fun() -> 
                if fileLineIndex = fileLines.Length then
                    ()
                    
                let result = fileLines.[fileLineIndex]
                fileLineIndex <- fileLineIndex + 1
                result
            )
            .Create()

    [<Test>]
    let ``Mock file reader with one line``() =
        let reader = CreateMockReader [|"First Line"|]

        reader.EndOfStream |> should be False
        reader.Position |> should equal 0

        reader.ReadLine() |> should equal "First Line"
        reader.EndOfStream |> should be True
        reader.Position |> should equal "First Line".Length

        
