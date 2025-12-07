#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"{__SOURCE_DIRECTORY__}\input.txt"
    |> List.ofSeq

let example =
    """123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  """.Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

let splitLines = 
    input
    |> List.map (fun line -> line.Split(" ", System.StringSplitOptions.RemoveEmptyEntries) |> List.ofSeq)
let rev = splitLines |> List.rev
let operators = rev[0]
let numbers = rev[1..] |> List.rev |> List.map (List.map int64)

let problems = 
    [ for i in 0 .. operators.Length - 1 do
        let op = operators[i]
        let col = numbers |> List.map (fun row -> row[i])
        yield (op, col) ]

let solve problem =
    let (op, nums) = problem
    match op with
    | "*" -> List.reduce (*) nums
    | "+" -> List.reduce (+) nums
    | _ -> failwith "Unknown operator"

problems |> List.map solve |> List.sum

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()