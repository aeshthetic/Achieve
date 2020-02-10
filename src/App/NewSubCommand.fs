module Achieve.NewSubCommand
open Argu
open Achieve.Parser
open Achieve.GoalCreation
open Achieve.TaskCreation


let newFunctionality (arg: ParseResults<NewArgs>) =
    match arg.GetAllResults().Head with
    | NewArgs.Goal name ->
        printfn "Created goal with name %s" name
        createGoal name
    | NewArgs.Task -> 
        let taskCreation = createTask ()
        match taskCreation with
        | Ok msg -> printfn "Success: %s" msg
        | Error msg -> printfn "Error: %s" msg
        taskCreation
