﻿namespace Siftan.TestSupport.UnitTests

open FsUnit
open NUnit.Framework
open Siftan.TestSupport

type InputBuilderTests() = 

    [<Test>]
    member public this.``Input Builder set to return Single File returns correct argument array``() =
        
        let args = InputBuilder().IsSingleFile().Build() 
        
        args.Length |> should equal 1
        args.[0] |> should equal InputBuilder.SingleFile

    [<Test>]
    member public this.``Input Builder set to return Multiple File pattern returns correct argument array``() = 
    
        let args = InputBuilder().IsMultipleFiles().Build();

        args.Length |> should equal 1
        args.[0] |> should equal InputBuilder.MultipleFiles

    [<Test>]
    member public this.``Input Builder set to return Single File and Search Sub directories flag returns correct argument array``() =
        
        let args = InputBuilder().IsSingleFile().AndSearchSubDirectories().Build()

        args.Length |> should equal 2
        args.[0] |> should equal InputBuilder.SingleFile
        args.[1] |> should equal InputBuilder.SearchSubDirectories
