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

let coords = 
    [for (rowi, row) in input |> List.indexed do
         for (coli, col) in row |> Seq.indexed do
                if col = '@' then yield (rowi,coli)]
    |> Set.ofSeq

let neighbours (r,c) coords =
    let offsets = 
        [(-1,-1); (-1,0); (-1,1);
         (0,-1);          (0,1);
         (1,-1);   (1,0); (1,1)]
    offsets |> List.map (fun (dr, dc) -> (r+dr, c+dc))
            |> List.filter (fun rc -> Set.contains rc coords)
            
let accessible = 
    coords
    |> Seq.filter (fun (r,c) -> (neighbours (r,c) coords).Length < 4)
    |> Seq.length

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()