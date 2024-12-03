#r "nuget: Unquote"
open Swensen.Unquote
open System.Text.RegularExpressions

let input =
    System.IO.File.ReadAllText $"""{__SOURCE_DIRECTORY__}\input.txt"""

let example =
    """xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))"""

let parse input = 
    let regex = Regex("mul\((\d+),(\d+)\)")
    let m = regex.Matches(input)
    m |> Seq.map (fun mm -> (int mm.Groups.[1].Value, int mm.Groups.[2].Value)) |> Seq.toList

let numbers = input |> parse
numbers |> List.map (fun (a,b) -> a * b) |> List.sum

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()