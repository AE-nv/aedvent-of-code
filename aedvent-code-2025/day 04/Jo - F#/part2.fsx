#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"{__SOURCE_DIRECTORY__}\input.txt"
    |> List.ofSeq

let example =
    """..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

let neighbours (r,c) coords =
    let offsets = 
        [(-1,-1); (-1,0); (-1,1);
         (0,-1);          (0,1);
         (1,-1);   (1,0); (1,1)]
    offsets |> List.map (fun (dr, dc) -> (r+dr, c+dc))
            |> List.filter (fun rc -> Set.contains rc coords)
            
let accessible coords = 
    coords
    |> Seq.filter (fun (r,c) -> (neighbours (r,c) coords).Length < 4)

let rec removeRolls coords =
    let toRemove = accessible coords |> Set.ofSeq
    if toRemove.IsEmpty then
        coords
    else
        let remaining = Set.difference coords toRemove
        removeRolls remaining

#time
let coords = 
    [for (rowi, row) in input |> List.indexed do
         for (coli, col) in row |> Seq.indexed do
                if col = '@' then yield (rowi,coli)]
    |> Set.ofSeq
    
let finalState = removeRolls coords
let result = coords.Count - finalState.Count

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()