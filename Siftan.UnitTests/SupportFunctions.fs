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
                    and set(position: int64) =
                        let mutable workingPosition = 0l
                        fileLineIndex <- 0
                        while this.Position < position do
                            fileLineIndex <- fileLineIndex + 1

                member this.EndOfStream = 
                    fileLineIndex = fileLines.Length

                member this.Length = 0L

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

    // '\000' is equivalent to '\0' from C#.
    let CreateSimpleRecordDescriptor = 
        CreateRecordDescriptor "," '\000' 0u "H"

    // Create a record descriptor for qualified records.
    let CreateQualifiedRecordDescriptor lineIDIndex = 
        CreateRecordDescriptor "," '|' lineIDIndex "H,1"

    // Create a record descriptor that includes the term definition.
    let CreateCompleteRecordDescriptor lineIDIndex lineID termIndex  = 
        DelimitedRecordDescriptor(Delimiter = ",", Qualifier = '\000', LineIDIndex = lineIDIndex, HeaderID = "H", Term = DelimitedRecordDescriptor.TermDefinition(lineID, termIndex))

    let CreateCompleteFixedWidthRecordDescriptor termLineID termStart termLength =
        FixedWidthRecordDescriptor(LineIDStart = 0u, LineIDLength = 2u, HeaderID = "01", Term = FixedWidthRecordDescriptor.TermDefinition(termLineID, termStart, termLength))
