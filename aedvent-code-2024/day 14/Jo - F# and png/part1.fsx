#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"""{__SOURCE_DIRECTORY__}\input.txt"""
    |> List.ofSeq

let example =
    """p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

let parse (line : string) =
    let regex = System.Text.RegularExpressions.Regex("p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")
    let m = regex.Match(line)
    (  m.Groups[1].Value |> int
     , m.Groups[2].Value |> int)
    ,( m.Groups[3].Value |> int
      ,m.Groups[4].Value |> int)

// let nbCols,nbRows = 11,7
let nbCols,nbRows = 101,103

let fix (vc, vr) =
    ( if vc < 0 then nbCols + vc else vc), (if vr < 0 then nbRows + vr else vr)

let finalLocation runs ((pc,pr),(vc,vr) )=
    (pc + (runs * vc)) % nbCols, (pr + (runs * vr)) % nbRows

let robots =
    input
    |> List.map parse
    |> List.map (fun (loc, vel) -> (loc, fix vel))
let locations = robots |> List.map (finalLocation 100)
let quadrants = [0..nbCols]
// 0 1 2 3 4 >5< 6 7 8 9 10
// 0 1 2 >3< 4 5 6
let colMiddle = nbCols / 2
let rowMiddle = nbRows / 2

let upperleft = locations |> List.filter (fun (c,r) -> c < colMiddle && r < rowMiddle) |> List.length
let upperright = locations |> List.filter (fun (c,r) -> c > colMiddle && r < rowMiddle) |> List.length
let lowerleft = locations |> List.filter (fun (c,r) -> c < colMiddle && r > rowMiddle) |> List.length
let lowerright = locations |> List.filter (fun (c,r) -> c > colMiddle && r > rowMiddle) |> List.length

let result = upperleft * upperright * lowerleft * lowerright

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()