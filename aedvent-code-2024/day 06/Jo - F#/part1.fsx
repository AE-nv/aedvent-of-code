#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"""{__SOURCE_DIRECTORY__}\input.txt"""
    |> List.ofSeq

let example =
    """....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

let grid = input
let maxRow = (grid |> Seq.length) - 1
let maxColumn = (grid[0] |> Seq.length) - 1

let obstacles = 
    [ for (rowNumber, row) in grid |> Seq.indexed do
          for (columnNumber, cell) in row |> Seq.indexed do
              if cell = '#' then yield (rowNumber, columnNumber)
    ]
    |> Set.ofSeq
 
let guard = 
    [ for (rowNumber, row) in grid |> Seq.indexed do
          for (columnNumber, cell) in row |> Seq.indexed do
              if cell = '^' then yield (rowNumber, columnNumber)
    ] |> List.head

type Direction = U | R | D | L
type Guard = { Location : int * int; Direction : Direction  }
type State = { Guard : Guard; Visited : Set<int * int>  }

let inBounds (r ,c) =
    0 <= r && r <= maxRow && 0 <= c && c <= maxColumn

let turnRight =
    function
    | U -> R
    | R -> D
    | D -> L
    | L -> U

let next ({ Location = (r,c); Direction = dir } as guard) =
    let inFront =
        let (dr, dc) = 
            match dir with
            | U -> (-1,0)
            | D -> (1,0)
            | R -> (0,1)
            | L -> (0,-1)
        (r + dr, c + dc)
    if obstacles |> Set.contains inFront
    then { guard with Direction = turnRight dir}
    else { guard with Location = inFront }

let rec patrol state =
    // printfn $"State: %A{state}"
    if state.Guard.Location |> inBounds |> not
    then state
    else
        { state with
            Guard = next state.Guard
            Visited = state.Visited |> Set.add state.Guard.Location }
        |> patrol

let init : State = { Guard = { Location = guard; Direction = U}; Visited = Set.empty }
let finalState = patrol init
finalState.Visited |> Set.count

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()