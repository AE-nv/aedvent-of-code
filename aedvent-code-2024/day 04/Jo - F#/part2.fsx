#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"""{__SOURCE_DIRECTORY__}\input.txt"""
    |> List.ofSeq

let example =
    """MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

let puzzle = input |> List.map List.ofSeq
let nbRows = ((puzzle |> Seq.length) - 1)
let nbColumns = (puzzle[0] |> Seq.length)-1
let lookup (puzzle : char list list) (r,c) = puzzle[r][c]

let aLocations =
    [ for row in 1..(nbRows-1) do
          for column in 1 .. (nbColumns-1) -> (row,column) ]
    |> List.filter (fun loc -> lookup puzzle loc = 'A')

let calculateLocations (rl,cl) =
    let offsets =
        [            
            [(-1,-1);(1,1)]
            [(1,-1);(-1,1)]
        ]
    offsets
    |> List.map (List.map (fun (ro,co) -> rl + ro, cl + co))
    
let lookups puzzle diagonals = diagonals |> List.map (List.map (lookup puzzle))

let isXMAS diagonals =
    [
        [['M';'S'];['M';'S']]
        [['M';'S'];['S';'M']]
        [['S';'M'];['M';'S']]
        [['S';'M'];['S';'M']]
    ]
    |> List.contains diagonals
    
let result =
    aLocations
    |> List.map calculateLocations 
    |> List.map (lookups puzzle)
    |> List.filter isXMAS
    |> List.length

let run () =
    printf "Testing.."
    test <@ result = 1809 @>
    printfn "...done!"

run ()