#r "nuget: Unquote"
open Swensen.Unquote

let input =
    System.IO.File.ReadAllText $"""{__SOURCE_DIRECTORY__}\input.txt"""

let example =
    """47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47""".Replace("\n", "\r\n")

let parse (input : string) = 
    let parseRule (rule:string) =
        let [|earlier;later|] = rule.Split("|")
        (int earlier, int later)
    let parseUpdate (update : string) = update.Split(",") |> Seq.map int |> Seq.toList

    let [| rules;updates |] = input.Split("\r\n\r\n")

    let r =
        rules.Split("\n")
        |> Seq.map parseRule
        |> List.ofSeq

    let u =
        updates.Split("\n") |> Seq.map parseUpdate |> Seq.toList
    (r,u)

let checkRulesFor rules t h =
    let rulesToCheck = t |> Seq.map (fun nb -> (nb,h))
    rulesToCheck |> Seq.exists (fun tc -> rules |> Seq.contains tc) |> not
    
let rec checkRules rules update =
    match update with
    | [] -> true
    | [_] -> true
    | h :: t ->
        let rulesToCheck = t |> Seq.map (fun nb -> (nb,h))
        let weHaveAProblem = rulesToCheck |> Seq.exists (fun tc -> rules |> Seq.contains tc)
        
        if weHaveAProblem
        then false
        else checkRules rules t
        
let middlePage (update : 'a list) =
    let l = update |> Seq.length
    let middle = l / 2
    update[middle]    

let correct rules update =
    let rec correct (rules : (int*int) list) acc update =
        match update with
        | [] -> acc |> List.rev
        | _ ->
            let next = 
                seq {
                    for nb in  update do
                        let rest = update |> List.except [nb]
                        if checkRulesFor rules rest nb then yield nb
                } |> Seq.head
            let rest = update |> List.except [next]
            correct rules (next :: acc) rest
    correct rules [] update

let rules, updates = parse input

let invalidUpdates = 
    updates
    |> List.filter (fun update -> not (checkRules rules update))

invalidUpdates
|> List.map (correct rules)
|> List.map middlePage
|> List.sum

let run () =
    printf "Testing.."
    test <@ 1 + 1 = 2 @>
    printfn "...done!"

run ()