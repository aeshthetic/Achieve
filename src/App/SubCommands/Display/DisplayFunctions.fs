module Achieve.SubCommands.Display.DisplayFunctions
open Achieve.Utilities
open Achieve.Types
open Utilities

let displayGoal goal =
    [
        sprintf "Goal: %s" goal.name 
        sprintf "%s" (goalTable goal)
    ]
    |> String.concat "\n"

let displayTask task =
    [
        taskHeader
        taskRow task
    ] |> String.concat "\n"



let individualProgress goal =
    goal
    |> progressQuotient
    |> drawProgress
    |> sprintf "%s (%i%% complete): %s" goal.name (((progressQuotient goal) * 100.0) |> int)

let displayProgress goals =
    goals
    |> List.map individualProgress
    |> String.concat "\n"