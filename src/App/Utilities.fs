module Achieve.Utilities
open System
open System.IO
open Newtonsoft.Json
open Achieve.Types

type OS =
        | OSX            
        | Windows
        | Linux

let getOS = 
        match int Environment.OSVersion.Platform with
        | 4 | 128 -> Linux
        | 6       -> OSX
        | _       -> Windows

type System.DateTime with 
    static member TryParseOption str =
        try
            Some(DateTime.Parse str)
        with e ->
            None

type System.TimeSpan with
    static member TryParseOption str =
        try
            Some(TimeSpan.Parse str)
        with e ->
            None

let dataFile =
    let path = 
        let dataDirectory = match getOS with
                            | Windows -> Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                            | _ -> Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        Path.Combine(dataDirectory, ".achieve")
    if not (Directory.Exists(path)) then Directory.CreateDirectory(path) |> ignore
    let filePath = Path.Combine(path, "goals.json")
    if not (File.Exists(filePath)) then
        File.WriteAllText(filePath, "[]")
    filePath

let allGoals () =
    File.ReadAllLines(dataFile)
    |> Array.reduce (+)
    |> JsonConvert.DeserializeObject<Goal list>

let tryFindGoal goals name =
    goals
    |> List.filter (fun goal -> goal.name = name)
    |> List.tryHead

let tryFindTask goal name =
    goal.tasks
    |> List.filter (fun task -> task.name = name)
    |> List.tryHead

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

let updateList l o n =
    n :: (
        l
        |> List.filter (fun x -> x <> o)
    )

let updateGoal (goals: Goal list) (oldGoal: Goal) (newGoal: Goal) =
    updateList goals oldGoal newGoal
    |> updateGoalList

let updateTask (goals: Goal list) (goal: Goal) (oldTask: Task) (newTask: Task) =
    newTask
    |> updateList goal.tasks oldTask
    |> (fun x -> updateList goals goal {goal with tasks = x})
    |> updateGoalList

