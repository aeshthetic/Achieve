module Achieve.SubCommands.Display.Utilities
open Achieve.Types

let optionString = function
    | Some x -> x.ToString()
    | None -> "None"

let taskRow (task: Task) =
    [
        task.name
        ; optionString task.dueAt
        ; optionString task.duration
        ; optionString task.startsAt
        ; match task.dependsOn with
          | Some dependencies -> 
              dependencies
              |> List.map (fun t -> t.name)
              |> String.concat "; "
          | None -> "None"
        ; task.complete.ToString()
    ] |> String.concat " | "

let taskHeader =
    [
        "Name"
        "Due Date and Time"
        "Duration"
        "Start Date and Time"
        "Dependent Upon"
        "Complete"
    ] |> String.concat " | "


let goalTable goal =
    let rows =
        goal.tasks
        |> List.map taskRow
        |> String.concat "\n"
    
    [taskHeader ; rows] |> String.concat "\n"

let progressQuotient goal =
    goal.tasks
    |> List.filter (fun t -> t.complete)
    |> List.length
    |> float
    |> (fun completed -> completed / (goal.tasks |> List.length |> float))

let drawProgress quotient =
    let completedPortion =
        (quotient * 100.0)
        |> int
    let completedString =
        completedPortion
        |> ((..) 1)
        |> Seq.toList
        |> List.map (fun x -> "#")
        |> String.concat ""
    let spaces =
        (100 - completedPortion)
        |> ((..) 1)
        |> Seq.toList
        |> List.map (fun x -> " ")
        |> String.concat ""
    
    ["[" ; completedString ; spaces ; "]"] |> String.concat ""

    