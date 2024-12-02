#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllLines $"""{__SOURCE_DIRECTORY__}\input.txt"""
    |> List.ofSeq

let example =
    """7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9""".Split("\n")
    |> Array.map (fun s -> s.Trim())
    |> List.ofSeq

let parse (lines : string list) =
        lines
        |> List.map (fun line -> line.Split(" ") |> Array.map int |> Array.toList)

let isMonotonic report =
    let asc = report |> List.sort
    let desc = report |> List.sortDescending
    report = asc || report = desc
let isWithinBounds report =
    let withinBounds x y =
        let diff = abs (x - y)
        1 <= diff && diff <= 3
    
    report
    |> List.pairwise
    |> List.forall (fun (x,y) -> withinBounds x y)

let shrink report =
    seq {
        yield report
        let length = (report |> List.length) - 1
        for i in [0..length] -> report |> List.removeAt i  
    }
          
let isSafe report =
    isMonotonic report && isWithinBounds report

let isSafeShrunk report = 
    let reports = shrink report
    reports |> Seq.exists isSafe
let reports = parse input
reports |> List.filter isSafeShrunk |> List.length

let run () =
    printf "Testing.."
    test <@ shrink [1..3] |> List.ofSeq = [[1; 2; 3]; [2; 3]; [1; 3]; [1; 2]] @>
    printfn "...done!"

run ()