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
let allLocations =
    [ for row in 0..nbRows do
          for column in 0 .. nbColumns -> (row,column) ]

let calculateLocations (rl,cl) =
    let offsets =
        [
            [(0,0);(0,1);(0,2);(0,3)]
            [(0,0);(0,-1);(0,-2);(0,-3)]
            [(0,0);(1,0);(2,0);(3,0)]
            [(0,0);(-1,0);(-2,0);(-3,0)]
            
            [(0,0);(1,1);(2,2);(3,3)]
            [(0,0);(-1,1);(-2,2);(-3,3)]
            [(0,0);(1,-1);(2,-2);(3,-3)]
            [(0,0);(-1,-1);(-2,-2);(-3,-3)]
        ]
    offsets
    |> List.map (List.map (fun (ro,co) -> rl + ro, cl + co))

let inBounds (r,c) =
    r >= 0 && c >= 0 && r <= nbRows && c <= nbColumns
        
let relevantLocations loc =
    calculateLocations loc
    |> List.filter (fun locs -> locs |> List.forall inBounds)

let lookup (puzzle : char list list) (r,c) = puzzle[r][c]

let result =
    allLocations
    |> List.collect relevantLocations
    |> List.map (fun locs -> locs |> List.map (lookup puzzle))
    |> List.filter (fun word -> word = ['X';'M';'A';'S'])
    |> List.length

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()