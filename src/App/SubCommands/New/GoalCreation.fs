module Achieve.SubCommands.New.GoalCreation
open Achieve.Types
open Achieve.Utilities
open Newtonsoft.Json
open System.IO

let updateGoalList (goals: Goal list) =
    goals
    |> JsonConvert.SerializeObject
    |> (fun it ->
        try
            File.WriteAllText(dataFile, it)
            Ok "Goal list updated successfully"
        with e ->
            Error e.Message
        )

let createGoal name =
    {name = name; tasks = []} :: allGoals()
    |> updateGoalList