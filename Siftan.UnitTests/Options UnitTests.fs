namespace Siftan.UnitTests

open NUnit.Framework
open Siftan
open FsUnit
open System

module public ``Options UnitTests`` =

    [<Test>]
    [<TestCase([|"delim"; "-d"; "|"; "-h"; "01"; "-li"; "0"; "-t"; "02"; "-ti"; "3"|])>]
    let ``Standard Delimited command line segment returns valid object``(args: string[]) =
        // Act
        let options = Options args

        // Assert
        options.Delimited |> should not' (equal null)
        options.Delimited.Delimiter |> should equal "|"
        options.Delimited.Qualifier |> should equal '\000'
        options.Delimited.HeaderLineID |> should equal "01"
        options.Delimited.LineIDIndex |> should equal 0
        options.Delimited.TermLineID |> should equal "02"
        options.Delimited.TermIndex |> should equal 3

    [<Test>]
    [<TestCase([|"delim"; "-h"; "01"; "-t"; "02"|])>]
    let ``Minimum Delimited command line segment returns valid object with defaults set``(args: string[]) =
        // Act
        let options = Options args

        // Assert
        options.Delimited |> should not' (equal null)
        options.Delimited.Delimiter |> should equal ","
        options.Delimited.Qualifier |> should equal '\000'
        options.Delimited.HeaderLineID |> should equal "01"
        options.Delimited.LineIDIndex |> should equal 0
        options.Delimited.TermLineID |> should equal "02"
        options.Delimited.TermIndex |> should equal 0

    [<Test>]
    [<TestCase([|"-d"; "|"; "-h"; "01"; "-li"; "A"; "-t"; "01"; "-ti"; "3"|])>]
    let ``Missing Delimited noun throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "No recognised noun found in command line arguments.") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"delim"; "-d"; "|"; "-h"; "01"; "-li"; "A"; "-t"; "01"; "-ti"; "3"|])>]
    let ``Delimited command line segment with bad type throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Value 'A' cannot be cast to type UInt32.") typeof<System.InvalidCastException>

    [<Test>]
    [<TestCase([|"delim"; "-t"; "01"|])>]
    let ``Delimited command line segment with missing required Header ID argument throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Header ID' (-h).") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"delim"; "-h"; "01"|])>]
    let ``Delimited command line segment with missing required Term Line ID argument throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Term Line ID' (-t).") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"delim"; "-d"; "|"; "-h"; "01"; "-li"; "0"; "-t"; "01"; "-ti"|])>]
    let ``Delimited command line segment with last missing value throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing value for field '-ti'.") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"delim"; "-d"; "|"; "-q"; "qual"; "-h"; "-li"; "A"; "-t"; "01"; "-ti"; "3"|])>]
    let ``Qualifier is a string throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Value 'qual' cannot be cast to type Char.") typeof<System.InvalidCastException>

    [<Test>]
    [<TestCase([|"fixed"; "-h"; "01"; "-ls"; "1"; "-ll"; "10"; "-t"; "02"; "-ts"; "12"; "-tl"; "11"|])>]
    let ``Standard Fixed width command line segment returns valid object``(args: string[]) =
        // Act
        let options = Options args

        // Assert
        options.FixedWidth |> should not' (equal null)
        options.FixedWidth.HeaderLineID |> should equal "01"
        options.FixedWidth.LineIDStart |> should equal 1
        options.FixedWidth.LineIDLength |> should equal 10
        options.FixedWidth.TermLineID |> should equal "02"
        options.FixedWidth.TermStart |> should equal 12
        options.FixedWidth.TermLength |> should equal 11

    [<Test>]
    [<TestCase([|"fixed"; "-ls"; "1"; "ll"; "10"; "-t"; "02"; "-ts"; "12"; "-tl"; "11"|])>]
    let ``Fixed width command line with missing required Header ID argument throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Header ID' (-h).") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"fixed"; "-h"; "01"; "-ll"; "10"; "-t"; "02"; "-ts"; "12"; "-tl"; "11"|])>]
    let ``Fixed width command line with missing required Line ID Start argument throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Line ID Start' (-ls).") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"fixed"; "-h"; "01"; "-ls"; "1"; "-t"; "02"; "-ts"; "12"; "-tl"; "11"|])>]
    let ``Fixed width command line with missing required Line ID Length argument throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Line ID Length' (-ll).") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"fixed"; "-h"; "01"; "-ls"; "1"; "-ll"; "10"; "-ts"; "12"; "-tl"; "11"|])>]
    let ``Fixed width command line with missing required Term Line ID argument throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Term Line ID' (-t).") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"fixed"; "-h"; "01"; "-ls"; "1"; "-ll"; "10"; "-t"; "02"; "-tl"; "11"|])>]
    let ``Fixed width command line with missing required Term Start argument throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Term Start' (-ts).") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"fixed"; "-h"; "01"; "-ls"; "1"; "-ll"; "10"; "-t"; "02"; "-ts"; "12"|])>]
    let ``Fixed width command line with missing required Term Length argument throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing required term 'Term Length' (-tl).") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"inlist"; "-f"; @"C:\Test.txt"|])>]
    let ``In list command line segment with filepath returns valid object``(args: string[]) =
        // Act
        let options = Options args

        // Assert
        options.InList |> should not' (equal null)
        options.InList.FilePath |> should equal @"C:\Test.txt"

    [<Test>]
    [<TestCase([|"inlist"; "-v"; "A:B:C"|])>]
    let ``In list command line segment with value list returns valid object``(args: string[]) =
        // Act
        let options = Options args

        // Assert
        options.InList |> should not' (equal null)
        options.InList.Values |> should equal [|"A"; "B"; "C"|]

    [<Test>]
    [<TestCase([|"inlist"|])>]
    let ``In list command line segment with missing value list and filepath throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Missing one required term. Must have 'File Path' (-f) or 'Value List' (-v) but not both.") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"inlist"; "-m"; "badvalue"|])>]
    let ``In list command line segment with bad enum value throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Value 'badvalue' cannot be cast to type Siftan.InListExpression+MatchQuotas.") typeof<System.Exception>

    [<Test>]
    [<TestCase([|"inlist"; "-f"; @"C:\Test.xtx"; "-v"; "A:B:C"|])>]
    let ``In list command line segment with both value list and filepath throws meaningful exception``(args: string[]) =
        // Act && Assert
        (fun () -> Options args |> ignore)
        |> should (throwWithMessage "Cannot have both 'File Path' (-f) and 'Value List' (-v) terms.") typeof<System.Exception>
