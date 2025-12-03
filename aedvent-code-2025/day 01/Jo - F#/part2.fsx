//https://en.wikipedia.org/wiki/Modulo

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
 
let inline (mod) a n =
    let r = a % n
    if r >= 0 then r else r + n

type State = { PassedZero: int; Position: int }

let execute (start: State) (instruction: Direction * int) : State =
    let (dir, dist) = instruction
    let offset =
        match dir with
        | L -> -1 * dist
        | R -> dist
    let nextPosition = (start.Position + offset) mod 100
    let passedZero =
        match dir with
        | R ->
            (start.Position + offset) / 100
        | L ->
            if start.Position = 0 then
                dist / 100
            else
                if dist >= start.Position then
                    (dist - start.Position) / 100 + 1
                else
                    0
    
    { PassedZero = start.PassedZero + passedZero; Position = nextPosition }

let solve input =  
    let instructions = input |> List.map parse
    instructions |> List.fold execute { PassedZero = 0; Position = 50 } |> _.PassedZero

let run () =
    printf "Testing.."
    
    test <@ 0 mod 100 = 0 @>
    test <@ 1 mod 100 = 1 @>
    test <@ 100 mod 100 = 0 @>
    test <@ 101 mod 100 = 1 @>
    test <@ -1 mod 100 = 99 @>
    test <@ -101 mod 100 = 99 @>
    test <@ -201 mod 100 = 99 @>
    
    test <@ execute { Position = 1; PassedZero = 0 } (L, 1) = { Position = 0; PassedZero = 1 } @>
    test <@ execute { Position = 50; PassedZero = 0 } (L, 68) = { Position = 82; PassedZero = 1 } @>
    test <@ execute { Position = 50; PassedZero = 0 } (R, 1_000) = { Position = 50; PassedZero = 10 } @>
    
    // Starting from 0
    test <@ execute { Position = 0; PassedZero = 0 } (L, 1) = { Position = 99; PassedZero = 0 } @>   // Start 0, move left small, don't land on 0
    test <@ execute { Position = 0; PassedZero = 0 } (L, 100) = { Position = 0; PassedZero = 1 } @>  // Start 0, move left 100, land on 0 - one full cycle
    test <@ execute { Position = 0; PassedZero = 0 } (L, 150) = { Position = 50; PassedZero = 1 } @> // Start 0, move left >100, one crossing
    test <@ execute { Position = 0; PassedZero = 0 } (L, 200) = { Position = 0; PassedZero = 2 } @>  // Start 0, move left 200, land on 0 - two full cycles
    test <@ execute { Position = 0; PassedZero = 0 } (R, 1) = { Position = 1; PassedZero = 0 } @>    // Start 0, move right small, don't land on 0
    test <@ execute { Position = 0; PassedZero = 0 } (R, 100) = { Position = 0; PassedZero = 1 } @>  // Start 0, move right 100, land on 0
    test <@ execute { Position = 0; PassedZero = 0 } (R, 150) = { Position = 50; PassedZero = 1 } @> // Start 0, move right >100, don't land on 0
    test <@ execute { Position = 0; PassedZero = 0 } (R, 200) = { Position = 0; PassedZero = 2 } @>  // Start 0, move right 200, land on 0
    
    // Not starting from 0, distance < 100
    test <@ execute { Position = 50; PassedZero = 0 } (L, 30) = { Position = 20; PassedZero = 0 } @>  // Don't cross 0
    test <@ execute { Position = 50; PassedZero = 0 } (L, 50) = { Position = 0; PassedZero = 1 } @>   // Land exactly on 0
    test <@ execute { Position = 50; PassedZero = 0 } (L, 60) = { Position = 90; PassedZero = 1 } @>  // Cross 0, don't land on 0
    test <@ execute { Position = 50; PassedZero = 0 } (R, 30) = { Position = 80; PassedZero = 0 } @>  // Don't cross 0
    test <@ execute { Position = 50; PassedZero = 0 } (R, 50) = { Position = 0; PassedZero = 1 } @>   // Land exactly on 0 (wrap)
    test <@ execute { Position = 50; PassedZero = 0 } (R, 60) = { Position = 10; PassedZero = 1 } @>  // Cross 0, don't land on 0
    
    // Not starting from 0, distance > 100
    test <@ execute { Position = 50; PassedZero = 0 } (L, 150) = { Position = 0; PassedZero = 2 } @>  // Cross 0 twice, land on 0
    test <@ execute { Position = 50; PassedZero = 0 } (L, 160) = { Position = 90; PassedZero = 2 } @> // Cross 0 twice, don't land on 0
    test <@ execute { Position = 50; PassedZero = 0 } (L, 250) = { Position = 0; PassedZero = 3 } @>  // Cross 0 three times, land on 0
    test <@ execute { Position = 50; PassedZero = 0 } (R, 150) = { Position = 0; PassedZero = 2 } @>  // Cross 0 twice, land on 0
    test <@ execute { Position = 50; PassedZero = 0 } (R, 160) = { Position = 10; PassedZero = 2 } @> // Cross 0 twice, don't land on 0
    test <@ execute { Position = 50; PassedZero = 0 } (R, 250) = { Position = 0; PassedZero = 3 } @>  // Cross 0 three times, land on 0
    
    // Edge cases: positions near 0
    test <@ execute { Position = 99; PassedZero = 0 } (R, 1) = { Position = 0; PassedZero = 1 } @>    // Just before 0, move to 0
    test <@ execute { Position = 99; PassedZero = 0 } (R, 2) = { Position = 1; PassedZero = 1 } @>    // Just before 0, move past 0
    test <@ execute { Position = 1; PassedZero = 0 } (R, 99) = { Position = 0; PassedZero = 1 } @>    // Move right to land on 0
    test <@ execute { Position = 99; PassedZero = 0 } (L, 99) = { Position = 0; PassedZero = 1 } @>   // Move left to land on 0
    
    // Distance = 0 (no movement)
    test <@ execute { Position = 0; PassedZero = 0 } (L, 0) = { Position = 0; PassedZero = 0 } @>     // No movement from 0
    test <@ execute { Position = 50; PassedZero = 0 } (L, 0) = { Position = 50; PassedZero = 0 } @>   // No movement from 50
    test <@ execute { Position = 50; PassedZero = 0 } (R, 0) = { Position = 50; PassedZero = 0 } @>   // No movement from 50
    
    // Different starting positions
    test <@ execute { Position = 1; PassedZero = 0 } (L, 1) = { Position = 0; PassedZero = 1 } @>     // From 1, move left to 0
    test <@ execute { Position = 1; PassedZero = 0 } (L, 2) = { Position = 99; PassedZero = 1 } @>    // From 1, move left past 0
    test <@ execute { Position = 1; PassedZero = 0 } (L, 101) = { Position = 0; PassedZero = 2 } @>   // From 1, wrap left to 0
    test <@ execute { Position = 25; PassedZero = 0 } (L, 25) = { Position = 0; PassedZero = 1 } @>   // From 25, move left to 0
    test <@ execute { Position = 25; PassedZero = 0 } (L, 125) = { Position = 0; PassedZero = 2 } @>  // From 25, multiple wraps to 0
    test <@ execute { Position = 25; PassedZero = 0 } (R, 75) = { Position = 0; PassedZero = 1 } @>   // From 25, move right to 0
    test <@ execute { Position = 25; PassedZero = 0 } (R, 175) = { Position = 0; PassedZero = 2 } @>  // From 25, multiple wraps to 0
    test <@ execute { Position = 75; PassedZero = 0 } (R, 25) = { Position = 0; PassedZero = 1 } @>   // From 75, move right to 0
    test <@ execute { Position = 75; PassedZero = 0 } (L, 75) = { Position = 0; PassedZero = 1 } @>   // From 75, move left to 0
    
    // Landing on position 99 (boundary before wrap)
    test <@ execute { Position = 50; PassedZero = 0 } (R, 49) = { Position = 99; PassedZero = 0 } @>  // Land on 99 without crossing
    test <@ execute { Position = 0; PassedZero = 0 } (L, 1) = { Position = 99; PassedZero = 0 } @>    // From 0, move left to 99
    test <@ execute { Position = 10; PassedZero = 0 } (L, 11) = { Position = 99; PassedZero = 1 } @>  // Cross 0, land on 99
    
    // Multiple wraps from position 0
    test <@ execute { Position = 0; PassedZero = 0 } (R, 300) = { Position = 0; PassedZero = 3 } @>   // From 0, wrap 3 times
    test <@ execute { Position = 0; PassedZero = 0 } (R, 350) = { Position = 50; PassedZero = 3 } @>  // From 0, wrap 3 times, land at 50
    
    // Large distances from various positions
    test <@ execute { Position = 10; PassedZero = 0 } (L, 310) = { Position = 0; PassedZero = 4 } @>  // Multiple wraps landing on 0
    test <@ execute { Position = 10; PassedZero = 0 } (R, 290) = { Position = 0; PassedZero = 3 } @>  // Multiple wraps landing on 0
    test <@ execute { Position = 90; PassedZero = 0 } (L, 190) = { Position = 0; PassedZero = 2 } @>  // From 90, wrap twice to 0
    test <@ execute { Position = 90; PassedZero = 0 } (R, 110) = { Position = 0; PassedZero = 2 } @>  // From 90, wrap twice to 0
    
    // Additional edge cases to verify the logic
    test <@ execute { Position = 1; PassedZero = 0 } (L, 201) = { Position = 0; PassedZero = 3 } @>   // From 1, cross 3 times, land on 0
    test <@ execute { Position = 99; PassedZero = 0 } (L, 100) = { Position = 99; PassedZero = 1 } @> // From 99, wrap once
    test <@ execute { Position = 99; PassedZero = 0 } (R, 101) = { Position = 0; PassedZero = 2 } @>  // From 99, cross twice, land on 0
    
    test <@ solve example = 6 @>
    printfn "...done!"
    
run ()

let answer = solve input
printfn "Answer: %d" answer
