#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"""{__SOURCE_DIRECTORY__}\input.txt"""
    |> List.ofSeq

let example =
    """3   4
4   3
2   5
1   3
3   9
3   3""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq
   
let lists : int list list = 
    input
    |> List.map (fun line -> line.Split("   ") |> Seq.map int |> Seq.toList)
    |> List.transpose
    |> List.map List.sort
  
let distances =
    List.zip lists.[0] lists.[1]
    |> List.map (fun (a,b) -> abs(a - b))
    
let result = distances |> List.sum

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()