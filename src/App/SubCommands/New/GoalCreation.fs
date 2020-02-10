module Achieve.SubCommands.New.GoalCreation
open Achieve.Types
open Achieve.Utilities

let createGoal name =
    {name = name; tasks = []} :: allGoals()
    |> updateGoalList