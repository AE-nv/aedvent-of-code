#r "nuget: Unquote"
open Swensen.Unquote

#r "nuget: System.Drawing.Common, 9.0.0"
open System.Drawing

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

let print locations =
    [for r in 0..(nbRows-1) do
         [for c in 0..(nbCols - 1) do
              let symbol = if locations |> Seq.contains (c,r) then 'X' else ' '
              sprintf $"%c{symbol}"]
         |> String.concat ""
    ] |> String.concat "\n"


let render iteration locations = 
    let bmp = new Bitmap(nbCols, nbRows)
    for (c,r) in locations do
         bmp.SetPixel(c,r,Color.Green)
    //needs folder write access or you'll get vague GDI+ exceptions
    //System.Runtime.InteropServices.ExternalException (0x80004005): A generic error occurred in GDI+.

    bmp.Save($"c:\\tmp\\tree-{iteration}.png", System.Drawing.Imaging.ImageFormat.Png)

//png uses compression, "regular" images can be compressed tighter than "random noise". I generate a png for every iteration and look for the smallest filesize.
let mutable i = 0
while true do
    i <- i + 1
    printfn "Iteration %d" i
    robots
    |> List.map (finalLocation i)
    |> render i

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()  