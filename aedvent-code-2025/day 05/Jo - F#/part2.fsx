#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllText $"{__SOURCE_DIRECTORY__}\input.txt"

let example =
    """3-5
10-14
16-20
12-18

1
5
8
11
17
32"""

let parse (text: string) =
    let sections = text.Split("\r\n\r\n")
    let ranges =
        sections.[0].Split('\n')
        |> Array.map (fun line ->
            let parts = line.Split('-')
            (int64 parts.[0], int64 parts.[1]))
    let numbers =
        sections.[1].Split('\n')
        |> Array.map int64
    (ranges, numbers)
    
let merge (a,b) (c,d) =
    if b < c || d < a then
        [(a,b);(c,d)]
    else 
        [(min a c, max b d)]

let merged ranges =
    let rec loop acc ranges =
        match ranges with
        | a :: b :: rest ->
            let result = merge a b
            match result with
            | [x] -> loop acc (x :: rest)
            | [x;y] -> loop (x :: acc) (y :: rest)
            | _ -> failwith "unreachable"
        | [x] -> List.rev (x :: acc)
        | [] -> List.rev acc
    loop [] ranges
     
#time
//Real: 00:00:00.003, CPU: 00:00:00.000, GC gen0: 0, gen1: 0, gen2: 0

let ranges, numbers = parse input 
let rranges = ranges |> List.ofSeq |> List.sort  
let finalRanges = merged rranges
finalRanges |> List.map (fun (a,b) -> b - a + 1L) |> List.sum

let run () =
    printf "Testing.."
    test <@ merge (1,10) (11, 20) = [(1, 10); (11, 20)] @>
    test <@ merge (11,20) (1, 10) = [(11, 20); (1, 10)] @>
    test <@ merge (1,10) (3, 8) = [(1, 10)] @>
    test <@ merge (3, 8) (1,10) = [(1, 10)] @>
    test <@ merge (1, 10) (5,15) = [(1, 15)] @>
    test <@ merge (5,15) (1, 10) = [(1, 15)] @>
    
    test <@ merged [(3, 5); (10, 20); (12, 20)] = [(3, 5); (10, 20)] @>
    printfn "...done!"

run ()