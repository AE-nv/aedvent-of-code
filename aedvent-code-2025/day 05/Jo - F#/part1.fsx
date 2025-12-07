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

let ranges, numbers = parse input
let fresh (ranges: (int64 * int64)[]) (n: int64) =
    (ranges |> Array.exists (fun (low, high) -> n >= low && n <= high))
    
numbers |> Array.filter (fresh ranges) |> Array.length

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()