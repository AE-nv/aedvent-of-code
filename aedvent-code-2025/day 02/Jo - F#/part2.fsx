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
    
let invalid (id : string) =
    let invaliddd id size =
        let chunks = id |> Seq.chunkBySize size
        chunks |> Seq.distinct |> Seq.length = 1

    let rec invalidd size id =
        if size = 0
        then false
        else
            if invaliddd id size then true
            else invalidd (size - 1) id
    
    let half = id.Length / 2
    invalidd half id

let invalidIds (first, last) = 
    [ first .. last ]
    |> List.map (fun id -> id, string id)
    |> List.filter (fun (_, sid) -> invalid sid)
    |> List.map fst 

let solve (input : string) =
    let ranges = input |> parse
    ranges |> Seq.collect invalidIds |> Seq.sum

let run () =
    printf "Testing.."
    
    test <@ invalid "11" = true @>
    test <@ invalid "22" = true @>
    test <@ invalid "12" = false @>
    test <@ invalid "21" = false @>
    
    test <@ invalid "99" = true @>
    test <@ invalid "111" = true @>
    test <@ invalid "100" = false @>
    test <@ invalid "110" = false @>
    
    test <@ invalid "999" = true @>
    test <@ invalid "1010" = true @>
    test <@ invalid "1000" = false @>
    test <@ invalid "1009" = false @>
    
    test <@ invalid "1188511885" = true @>
    test <@ invalid "1188511880" = false @>
    test <@ invalid "1188511890" = false @>
    
    test <@ invalid "222222" = true @>
    test <@ invalid "222220" = false @>
    test <@ invalid "222223" = false @>
    
    test <@ invalid "1698522" = false @>
    test <@ invalid "1698528" = false @>
    
    test <@ invalid "446446" = true @>
    test <@ invalid "446443" = false @>
    test <@ invalid "446449" = false @>
    
    test <@ invalid "38593859" = true @>
    test <@ invalid "38593856" = false @>
    test <@ invalid "38593862" = false @>
    
    test <@ invalid "565656" = true @>
    test <@ invalid "565653" = false @>
    test <@ invalid "565659" = false @>
    
    test <@ invalid "824824824" = true @>
    test <@ invalid "824824821" = false @>
    test <@ invalid "824824827" = false @>
    
    test <@ invalid "2121212121" = true @>
    test <@ invalid "2121212118" = false @>
    test <@ invalid "2121212124" = false @>
    
    test <@ invalidIds (998L, 1012L) = [999L; 1010L] @>
    
    test <@ solve example = 4174379265L @>
    printfn "...done!"

run ()

#time
solve input