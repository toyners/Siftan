namespace Siftan.TestSupport.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport

type InputBuilder_UnitTests() = 

    [<Test>]
    member public this.``Input Builder set to return default Single File returns correct argument array``() =
        
        let args = InputBuilder().IsSingleFile().Build() 
        
        args.Length |> should equal 1
        args.[0] |> should equal InputBuilder.SingleFile

    [<Test>]
    member public this.``Input Builder set to return custom Single File returns correct argument array``() =
       
        let expectedName = @"C:\Input\Input.txt"; 
        let args = InputBuilder().IsSingleFile(expectedName).Build() 
        
        args.Length |> should equal 1
        args.[0] |> should equal expectedName

    [<Test>]
    member public this.``Input Builder set to return default Multiple File pattern returns correct argument array``() = 
    
        let args = InputBuilder().IsMultipleFiles().Build();

        args.Length |> should equal 1
        args.[0] |> should equal InputBuilder.MultipleFiles

    [<Test>]
    member public this.``Input Builder set to return custom Multiple File pattern returns correct argument array``() = 
    
        let expectedPattern = @"C:\Input\*.txt";
        let args = InputBuilder().IsMultipleFiles(expectedPattern).Build();

        args.Length |> should equal 1
        args.[0] |> should equal expectedPattern

    [<Test>]
    member public this.``Input Builder set to return Single File and Search Sub directories flag returns correct argument array``() =
        
        let args = InputBuilder().IsSingleFile().AndSearchSubDirectories().Build()

        args.Length |> should equal 2
        args.[0] |> should equal InputBuilder.SingleFile
        args.[1] |> should equal InputBuilder.SearchSubDirectories

