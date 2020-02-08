module Achieve.GoalCreation
open Achieve.Types
open Achieve.Utilities
open Newtonsoft.Json
open System.IO

let updateGoalList (goals: Goal list) =
    goals
    |> JsonConvert.SerializeObject
    |> (fun it -> File.WriteAllText(dataFile, it))

let createGoal name =
    {name = name; tasks = []} :: allGoals()
    |> updateGoalList