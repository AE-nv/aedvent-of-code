#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllText $"{__SOURCE_DIRECTORY__}\input.txt"

let example =
    """11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124"""

let parse (input : string) = 
    input.Split(",")
    |> Array.map (fun rangeStr ->
        let parts = rangeStr.Split("-")
        (int64 parts.[0], int64 parts.[1]))

let split (s : string) =
    let half = s.Length / 2
    (s.Substring(0, half), s.Substring(half))

let invalidIds (first, last) = 
    [ first .. last ]
    |> List.map (fun id -> id, string id)
    |> List.map (fun (id, sid) -> id, split sid)
    |> List.filter (fun (_, (a,b)) -> a = b)
    |> List.map fst

let solve (input : string) =
    let ranges = input |> parse
    ranges |> Seq.collect invalidIds |> Seq.sum

let run () =
    printf "Testing.."
    test <@ solve example = 1227775554L @>
    printfn "...done!"

run ()

solve input