module Achieve.SubCommands.Complete.Command
open Argu
open Achieve.Parser
open Achieve.Utilities

let goalList = allGoals ()

let completeFunctionality (arg: ParseResults<CompleteArgs>) =
    match arg.GetAllResults().Head with
    | Task (goalName, name) ->
        match tryFindGoal goalList goalName with
        | Some goal ->
            match tryFindTask goal name with
            | Some task ->
                match updateTask goalList goal task ({task with complete = true}) with
                | Ok _ -> Ok "Success: Task marked as complete"
                | Error msg -> Error msg
            | None -> Error "Error: Task could not be found"
        | None -> Error "Error: Goal could not be found"