module Achieve.Program
open Parser
open Achieve.SubCommands.New.Command
open Achieve.SubCommands.Display.Command
open Achieve.SubCommands.Complete.Command

let reportResults = function
    | Ok msg -> printfn "%s" msg
    | Error msg -> printfn "%s" msg

[<EntryPoint>]
let main argv =
    try
        let results = parser.ParseCommandLine(inputs = argv, raiseOnUsage = true)
        match results.TryGetSubCommand() with
        | Some command ->
            match command with
            | New arg ->
                newFunctionality arg
            | Display arg ->
                displayFunctionality arg
            | Complete arg ->
                completeFunctionality arg
            | _ -> Error "Command not found"
        | None -> Error "No subcommand given"
        |> reportResults
    with e ->
        printfn "%s" e.Message
    0
