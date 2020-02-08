module Achieve.Parser
open Argu

type NewArgs =
    | [<CliPrefix(CliPrefix.None)>] Goal of name:string
    | [<CliPrefix(CliPrefix.None)>] Task
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Goal _ -> "Create a new goal with the specified name"
            | Task -> "Create a new task as part of a goal"
and UpdateArgs =
    | [<CliPrefix(CliPrefix.None)>] Goal of name:string
    | [<CliPrefix(CliPrefix.None)>] Task of goal:string * name:string
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Goal _ -> "Update a goal with the specified name"
            | Task _ -> "Update a task within the specified goal and of the specified name"
and DeleteArgs =
    | [<CliPrefix(CliPrefix.None)>] Goal of name:string
    | [<CliPrefix(CliPrefix.None)>] Task of goal:string * name:string
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Goal _ -> "Delete a goal with the specified name"
            | Task _ -> "Delete a task within the specified goal and of the specified name"
and DisplayArgs =
    | [<CliPrefix(CliPrefix.None)>] Goal of name:string
    | [<CliPrefix(CliPrefix.None)>] Task of goal:string * name:string
    | [<CliPrefix(CliPrefix.None)>] Progress
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Goal _ -> "Display information about a goal with the specified name"
            | Task _ -> "Display information about a task within the specified goal and of the specified name"
            | Progress -> "Display your progress across all goals"
and CompleteArgs =
    | [<CliPrefix(CliPrefix.None)>] Task of goal:string * name:string
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Task _ -> "Complete a task within the specified goal and of the specified name"
and AchieveArgs =
    | [<CliPrefix(CliPrefix.None); First>] New of ParseResults<NewArgs>
    | [<CliPrefix(CliPrefix.None); First>] Update of ParseResults<UpdateArgs>
    | [<CliPrefix(CliPrefix.None); First>] Delete of ParseResults<DeleteArgs>
    | [<CliPrefix(CliPrefix.None); First>] Display of ParseResults<DisplayArgs>
    | [<CliPrefix(CliPrefix.None); First>] Complete of ParseResults<CompleteArgs>
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | New _ -> "Create a new goal or task"
            | Update _ -> "Update an existing goal or task"
            | Delete _ -> "Delete an existing goal or task"
            | Display _ -> "Display information about a goal or task, or show progress across goals"
            | Complete _ -> "Mark a task as completed"

let parser = ArgumentParser.Create<AchieveArgs>(programName = "achieve")