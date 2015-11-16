namespace Siftan.UnitTests

open Jabberwocky.Toolkit.IO
open Siftan
//open Foq

module public SupportFunctions =

    let CreateMockReader(fileLines: string[]) =
        let mutable fileLineIndex = 0

        {
            new IStreamReader with
            
                member this.Position
                    with get() = 
                        let mutable position = 0
                        for i = 0 to fileLineIndex - 1 do
                            position <- position + fileLines.[i].Length

                        if fileLines.Length > 1 then
                            let linesConsumed = if fileLineIndex = fileLines.Length then fileLines.Length - 1 else fileLineIndex
                            position <- position + (2 * linesConsumed)

                        int64 position
                    and set(v: int64) = ()

                member this.EndOfStream = 
                    fileLineIndex = fileLines.Length

                member this.Name = "file"

                member this.ReadLine() =
                    let result = fileLines.[fileLineIndex]
                    fileLineIndex <- fileLineIndex + 1
                    result

                member this.Close() = ()

                member this.Dispose() = ()
        }

    let CreateRecordDescriptor delimiter qualifier lineIDIndex headerID =
       DelimitedRecordDescriptor(Delimiter = delimiter, Qualifier = qualifier, LineIDIndex = lineIDIndex, HeaderID = headerID)

    // '\000' is equivalent to '\0' from C#
    let CreateSimpleRecordDescriptor = CreateRecordDescriptor "," '\000' 0u "H"

    // Create a record descriptor for qualified records
    let CreateQualifiedRecordDescriptor(lineIDIndex: uint32) = CreateRecordDescriptor "," '|' lineIDIndex "H,1"

    (*
    let CreateMockReaderInstance(fileLines: string[]) =
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
    *)