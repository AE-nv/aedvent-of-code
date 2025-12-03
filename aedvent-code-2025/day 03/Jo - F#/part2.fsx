//https://www.tldraw.com/f/BILvalmsPpfpAxgueT0xE?d=v725.112.1749.891.haIhEGqj0icAkSJnoej-x
#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"{__SOURCE_DIRECTORY__}\input.txt"
    |> List.ofSeq

let example =
    """987654321111111
811111111111119
234234234234278
818181911112111""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

let parseBank (line: string) =
    line
    |> Seq.map (string >> int)
    |> Seq.toList
    
let rec solve acc nb (bank : int list) : int list =
    if nb = 0 then acc |> List.rev
    elif nb = bank.Length then (acc |> List.rev) @ bank
    else
        let splitIndex = bank.Length - (nb - 1)
        let candidates = List.take splitIndex bank
        let (idxnext, next) = candidates |> List.indexed |> List.maxBy snd
        let remainingBank = bank |> List.skip (idxnext + 1)
        solve (next :: acc) (nb - 1) remainingBank

let solver example =
    example
    |> List.map parseBank
    |> List.map (solve [] 12)
    |> List.map (List.map string >> String.concat "" >> int64)
    |> List.sum
    
solver input
    
let run () =
    printf "Testing.."
    test <@ solver example = 3121910778619L @>
    printfn "...done!"

run ()