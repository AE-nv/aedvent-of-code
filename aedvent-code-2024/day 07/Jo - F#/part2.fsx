#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"""{__SOURCE_DIRECTORY__}\input.txt"""
    |> List.ofSeq

let example =
    """190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

let parseLine (line : string) =
    let [|result;operands|] = line.Split(": ")
    let operandsParsed = operands.Split(" ") |> Seq.map int64 |> Seq.toList
    (int64 result, operandsParsed)
    
type Operator = Add | Multiply | Concat

let rec combinations nb =
    seq {
        if nb = 0
        then yield []
        else
            let tail = combinations (nb - 1)
            yield! tail |> Seq.map (fun t -> Add :: t)
            yield! tail |> Seq.map (fun t -> Multiply :: t)
            yield! tail |> Seq.map (fun t -> Concat :: t)
    }
    
let apply x y =
    function
    | Add -> x + y
    | Multiply -> x * y
    | Concat -> [string x; string y] |> String.concat "" |> int64

let rec evaluate (result, operands) operators =
    match operators with
    | [] -> result = (operands |> List.head)
    | op :: ops ->
        let x :: y :: tail = operands
        let r = apply x y op
        evaluate (result, r :: tail) ops

let couldBeTrue (result, operands) =
    let combos = combinations ((operands |> Seq.length) - 1)
    combos
    |> Seq.exists (evaluate (result, operands))
    
let parsed = input |> List.map parseLine

let result = 
    parsed
    |> List.filter couldBeTrue
    |> List.sumBy fst    

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()