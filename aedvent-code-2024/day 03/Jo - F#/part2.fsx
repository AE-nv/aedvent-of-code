#r "nuget: Unquote"
open Swensen.Unquote
open System.Text.RegularExpressions

let input =
    System.IO.File.ReadAllText $"""{__SOURCE_DIRECTORY__}\input.txt"""

let example =
    """xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"""

type Instruction =
    Mul of int * int
    | Do
    | Dont

let parse input = 
    let regex = Regex("mul\((\d+),(\d+)\)|do\(\)|don't\(\)")
    let m = regex.Matches(input)
    
    let parseMatch (mat : Match) =
        match mat.Value with
        | "do()" -> Do
        | "don't()" -> Dont
        | _ -> Mul (int mat.Groups.[1].Value, int mat.Groups.[2].Value)
    
    m |> Seq.map parseMatch |> Seq.toList
    
type Result = { Enabled : bool; Total : int }

let folder result instruction =
    match instruction with
    | Do -> {result with Enabled = true}
    | Dont -> {result with Enabled = false}
    | Mul(a,b) ->
        if result.Enabled then
            { result with Total = result.Total + (a*b) }
        else
            result

let numbers = parse input
let result = numbers |> List.fold folder { Enabled = true; Total = 0 }
result.Total

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()