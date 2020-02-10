module Achieve.Program
open Parser
open NewSubCommand

[<EntryPoint>]
let main argv =
    try
        let results = parser.ParseCommandLine(inputs = argv, raiseOnUsage = true)
        match results.TryGetSubCommand() with
        | Some command ->
            match command with
            | New arg ->
                newFunctionality arg
            | _ -> Error "Command not found"
        | None -> Error "No subcommand given"
        |> ignore
    with e ->
        printfn "%s" e.Message
    0
