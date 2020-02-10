module Achieve.SubCommands.Display.Command
open Argu
open Achieve.Parser
open Achieve.Utilities
open DisplayFunctions

let goalList = allGoals ()

let displayFunctionality (arg: ParseResults<DisplayArgs>) =
    match arg.GetAllResults().Head with
    | DisplayArgs.Goal name ->
        match tryFindGoal goalList name with
        | Some goal ->
            printfn "%s" (displayGoal goal)
            Ok "Success: Goal displayed successfully"
        | None -> Error "Error: Goal could not be found"
    | DisplayArgs.Task (goalName, name) ->
        match tryFindGoal goalList goalName with
        | Some goal ->
            match tryFindTask goal name with
            | Some task ->
                printfn "%s" (displayTask task)
                Ok "Success: Task displayed successfully"
            | None -> Error "Error: Task could not be found"
        | None -> Error "Error: Goal could not be found"
    | Progress ->
        printfn "%s" (displayProgress goalList)
        Ok "Success: Progress displayed successfully"