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
module Location =
    let down (r,c) = (r+1,c)
    let split (r,c) = [ (r,c-1); (r,c+1) ]
    
type BeamPath = int64 * Location list
module BeamPath =
    let head (path : BeamPath) = path |> snd |> List.head
    let nbPaths = fst
    
    let dedupe (paths : BeamPath list) : BeamPath list =
        paths
        |> List.groupBy (fun (_, p) -> p |> List.head)
        |> List.map (fun (_, group) ->
                let _, p = List.head group
                let nbs = group |> List.sumBy nbPaths
                (nbs, p))

    let step (splitters : Set<Location>) (path : BeamPath) : BeamPath list =
        let moveBeam loc =
            let next = loc |> Location.down
            if splitters |> Set.contains next then
                next |> Location.split
            else
                [next]
            
        let moved = path |> head |> moveBeam
        moved |> List.map (fun newHead -> nbPaths path, newHead :: (snd path))

type State = { CurrentDepth : int; Depth : int; BeamPaths: BeamPath list; Splitters : Set<Location> }
module State =
    let tick (state : State) : State =
        let step (path : BeamPath) : BeamPath list =
            path |> BeamPath.step state.Splitters

        let newPaths : BeamPath list =
            state.BeamPaths
            |> List.collect step
            |> BeamPath.dedupe
        { state with CurrentDepth = state.CurrentDepth + 1; BeamPaths = newPaths }

    let rec simulate state=
        // printfn $"%d{state.CurrentDepth}"
        if state.CurrentDepth = state.Depth then
            state
        else
            let next = state |> tick
            simulate next

module Parser =
    let parse (input : string list) : State =
        let startingBeam = (0, input[0] |> Seq.findIndex ((=) 'S'))
        let depth = input.Length - 1
        let splitterLocations =
            [ for (ri, row) in input |> Seq.indexed do
                  for (ci, col) in row |> Seq.indexed do
                      if col = '^' then yield (ri,ci) ]
            |> Set.ofList
        { CurrentDepth = 0; Depth = depth;  BeamPaths = [ 1L, [ startingBeam ] ]; Splitters = splitterLocations}

#time
let solve input =
    let parsed = Parser.parse input
    let result = State.simulate parsed
    let solution = result.BeamPaths |> List.sumBy BeamPath.nbPaths
    solution
    
test <@ solve example = 40L @>
test <@ solve input = 17921968177009L @>