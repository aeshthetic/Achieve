module Achieve.TaskCreation
open System
open System.Globalization
open Achieve.Types
open Achieve.Utilities
open Achieve.GoalCreation

let existingGoals = allGoals() 

let tryFindGoal name =
    existingGoals
    |> List.filter (fun goal -> goal.name = name)
    |> List.tryHead

let nameTask goal =
    printfn "Name:"
    let name = Console.ReadLine()
    let taskAlreadyExists =
        goal.tasks
        |> List.map (fun t -> t.name)
        |> List.contains name
    
    match taskAlreadyExists with
    | true -> None
    | false ->
        printfn "Task will be named %s" name
        Some name


let timeInput object format tryParseFunction toStringFunction =
    printfn "(Optional) %s (%s)" object format
    let input =
        Console.ReadLine()
        |> tryParseFunction

    match input with
    | Some inp ->
        inp
        |> toStringFunction
        |> printfn "%s set to %s. You can edit this later using the update command." object
    | None ->
        printfn "No due date or time will be set. You can edit this later using the update command."
    input

let validDependencyInput (inputString: string) =
    inputString.Contains(";")

let validDependencies (dependencyString: string) goal =
    // Cut the list of dependencies down to those which actually exist
   dependencyString.Split ';'
   |> Seq.toList
   |> List.filter (fun task ->
        goal.tasks
        |> List.map (fun t -> t.name)
        |> List.contains task
   ) // Then try and convert them to actual Tasks
   |> List.map (fun taskName ->
        goal.tasks
        |> List.filter (fun t -> t.name = taskName)
        |> List.tryHead
   ) // Filter out the invalid ones if any exist
   |> List.choose id

let confirmDependencies (dependencies: Task list option) =
    match dependencies with
    | Some depList ->
        depList
        |> List.map (fun dep -> dep.name)
        |> String.concat " ; "
        |> printfn "The completion of this task will depend on the completion of the following tasks:\n%s"
    | None -> printfn "The completion of this task will not depend on the completion of any other tasks."


let collectDependencies goal =
    printf "(Optional) Tasks that must be completed in order to begin this task (Separate tasks with ;)"
    let dependencyInput = Console.ReadLine()
    let dependencies =
        match validDependencyInput dependencyInput with
        | true -> Some (validDependencies dependencyInput goal)
        | false -> None
    
    confirmDependencies dependencies

    dependencies
    
let addTaskToGoal goal task =
    let newGoal = {goal with tasks = task :: goal.tasks}
    newGoal :: (existingGoals
    |> List.filter (fun g -> g <> goal))
    |> updateGoalList




let createTask () =
    if (List.isEmpty existingGoals) then
        printf "Please create a goal before creating a task"
    else
        printf "Enter the name of a goal to create this task under:"
        let goalName = Console.ReadLine()
        let goal = tryFindGoal goalName
                    
        match goal with
        | Some goal ->
            printf "Please provide the following information about the task being created. All provided information
            can be edited later using the update command."

            let nameOption = nameTask goal
            let dueAt = timeInput "Due Date and Time" "MM/DD/YYYY and optionally HH:MM:SS" DateTime.TryParseOption (fun dt -> dt.ToString("g", CultureInfo.CreateSpecificCulture("en-US")))
            let duration = timeInput "Duration" "days.hours:minutes" TimeSpan.TryParseOption (fun ts -> ts.ToString("g", CultureInfo("en-US")))
            let startsAt = timeInput "Start Date and Time" "MM/DD/YYYY and optionally HH:MM:SS" DateTime.TryParseOption (fun dt -> dt.ToString("g", CultureInfo.CreateSpecificCulture("en-US")))
            let dependencies = collectDependencies goal

            match nameOption with
            | Some name ->
                {name = name; dueAt = dueAt; duration = duration; dependsOn = dependencies; startsAt = startsAt}
                |> addTaskToGoal goal
            | None -> ()
        | None -> ()
