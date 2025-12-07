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
    |> List.ofSeq
    
let chunk delim_pred lst =
    let rec chunk acc current = function
        | [] -> (current :: acc) |> List.rev |> List.map List.rev
        | x :: xs when delim_pred x -> chunk (current :: acc) [] xs
        | x :: xs -> chunk acc (x :: current) xs
    chunk [] [] lst
    
let parse (problem : char list list) =
    let [operator], rest = problem[0] |> List.rev |> List.splitAt 1
    let last_line_without_operator = rest |> List.rev
    let numbers =
        last_line_without_operator :: problem[1..]
        |> List.map ((List.map string) >> (String.concat "") >> _.Trim() >> int64)
    operator, numbers
    
let solve (operator, numbers) =
    match operator with
    | '*' -> numbers |> List.reduce (*)
    | '+' -> numbers |> List.reduce (+)

let transposed = example |> Seq.transpose |> Seq.map Seq.toList |> Seq.toList
let problems = transposed |> chunk (List.forall ((=) ' '))
let problem = problems[0]
let solution = problems |> List.map (parse >> solve) |> List.sum

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()