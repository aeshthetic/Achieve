module Achieve.SubCommands.New.TaskCreation
open System
open System.Globalization
open Achieve.Types
open Achieve.Utilities
open Achieve.SubCommands.New.GoalCreation

let existingGoals = allGoals() 

let nameTask goal name =
    match tryFindTask goal name with
    | Some _ -> Error "A task with this name already exists under this goal. Are you sure you don't want to update it?"
    | None -> Ok name


let timeInput object format tryParseFunction toStringFunction =
    printf "(Optional) %s (%s): " object format
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
        |> sprintf "The completion of this task will depend on the completion of the following tasks:\n%s"
        |> Ok
    | None -> Error "The completion of this task will not depend on the completion of any other tasks."


let collectDependencies goal dependencyInput =
    match validDependencyInput dependencyInput with
    | true -> Some (validDependencies dependencyInput goal)
    | false -> None
    
    
let addTaskToGoal goal task =
    let newGoal = {goal with tasks = task :: goal.tasks}
    newGoal :: (existingGoals
    |> List.filter (fun g -> g <> goal))
    |> updateGoalList

let createTask () =
    match List.isEmpty existingGoals with
    | true -> Error "No existing goals"
    | false ->
        printf "Enter the name of a goal to create this task under: "
        let goal =
            Console.ReadLine()
            |> tryFindGoal existingGoals
                   
        match goal with
        | Some goal ->
            printfn "Please provide the following information about the task being created.\nAll provided information can be edited later using the update command."

            printf "Name: "
            let name =
                Console.ReadLine()
                |> nameTask goal
            let dueAt = timeInput "Due Date and Time" "MM/DD/YYYY and optionally HH:MM:SS" DateTime.TryParseOption (fun dt -> dt.ToString("g", CultureInfo.CreateSpecificCulture("en-US")))
            let duration = timeInput "Duration" "days.hours:minutes" TimeSpan.TryParseOption (fun ts -> ts.ToString("g", CultureInfo("en-US")))
            let startsAt = timeInput "Start Date and Time" "MM/DD/YYYY and optionally HH:MM:SS" DateTime.TryParseOption (fun dt -> dt.ToString("g", CultureInfo.CreateSpecificCulture("en-US")))
            printf "Tasks that must be completed before this task can begin (separate tasks with ;): "
            let dependencies =
                Console.ReadLine()
                |> collectDependencies goal
            
            match (confirmDependencies dependencies) with
            | Ok msg -> printfn "%s" msg
            | Error msg -> printfn "%s" msg

            name
            |> Result.bind (fun n ->
                {name = n; dueAt = dueAt; duration = duration; dependsOn = dependencies; startsAt = startsAt; complete = false}
                |> addTaskToGoal goal)
        
        | None -> Error "Goal does not exist"
