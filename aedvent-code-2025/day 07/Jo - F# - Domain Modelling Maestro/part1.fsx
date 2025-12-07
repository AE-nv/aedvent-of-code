#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"{__SOURCE_DIRECTORY__}\input.txt"
    |> List.ofSeq

let example =
    """.......S.......
...............
.......^.......
...............
......^.^......
...............
.....^.^.^.....
...............
....^.^...^....
...............
...^.^...^.^...
...............
..^...^.....^..
...............
.^.^.^.^.^...^.
...............""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

type Location = (int * int)
type State = { CurrentDepth : int; Depth : int; BeamLocation: Set<Location>; Splitters : Set<Location>; NbSplits : int }

let parse (input : string list) : State =
    let startingBeam = (0, input[0] |> Seq.findIndex ((=) 'S'))
    let depth = input.Length - 1
    let splitterLocations =
        [ for (ri, row) in input |> Seq.indexed do
              for (ci, col) in row |> Seq.indexed do
                  if col = '^' then yield (ri,ci) ]
        |> Set.ofList
    { CurrentDepth = 0; Depth = depth;  BeamLocation = Set.singleton startingBeam; Splitters = splitterLocations; NbSplits = 0 }

let tick (state : State) : State =
    let moveBeam (r,c) =
        let next = (r+1,c)
        if Set.contains next state.Splitters then
            [(r+1,c-1); (r+1,c+1)]
        else
            [next]
    let newBeams =
        state.BeamLocation
        |> Seq.collect moveBeam
    let nbSplits = (newBeams |> Seq.length) - (state.BeamLocation |> Seq.length) 
    { state with NbSplits = state.NbSplits + nbSplits; CurrentDepth = state.CurrentDepth + 1; BeamLocation = newBeams |> Set.ofSeq }

let rec simulate state=
    if state.CurrentDepth = state.Depth then
        state
    else
        let next = state |> tick
        simulate next

let parsed = parse input
let result = simulate parsed

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"
example |> List.map List.ofSeq |> List.collect id |> List.filter ((=) '^') |> List.length |> printfn "Example: %d"
run ()