#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"{__SOURCE_DIRECTORY__}\input.txt"
    |> List.ofSeq

let example =
    """L68
L30
R48
L5
R60
L55
L1
L99
R14
L82""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

type Direction = L | R

let parse (line: string) =
    let dir =
        match line[0] with
        | 'L' -> L
        | 'R' -> R
        | _ -> failwith "Invalid direction"
    let dist = line.[1..] |> int
    (dir, dist)
 
//Note to future self: remainder operator (%) is NOT modulo for negative numbers
let inline (mod) D d =
    let r = D % d
    if r >= 0 then r
    elif d >= 0 then r + d
    else r - d
    
let execute (start: int) (instruction: Direction * int) =
    let (dir, dist) = instruction
    let result = 
        match dir with
        | L -> start - dist
        | R -> start + dist
    result mod 100
let solve input =  
    let instructions = input |> List.map parse
    instructions |> List.scan execute 50 |> List.filter ((=) 0) |> List.length

let run () =
    printf "Testing.."
    test <@ execute 50 (R, 1) = 51 @>
    test <@ execute 99 (R, 1) = 0 @>
    test <@ execute 0 (L, 1) = 99 @>
    test <@ execute 99 (R, 101) = 0 @>
    test <@ execute 0 (L, 101) = 99 @>
    
    test <@ solve example = 3 @>
    printfn "...done!"

run ()

solve input