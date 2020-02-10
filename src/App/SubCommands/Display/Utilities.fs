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
    ] |> String.concat " | "

let taskHeader =
    [
        "Name"
        "Due Date and Time"
        "Duration"
        "Start Date and Time"
        "Dependent Upon"
    ] |> String.concat " | "


let goalTable goal =
    let rows =
        goal.tasks
        |> List.map taskRow
        |> String.concat "\n"
    
    [taskHeader ; rows] |> String.concat "\n"
