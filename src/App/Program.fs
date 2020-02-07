// Learn more about F# at http://fsharp.org

open System
open Argu

type Task = {
    name: string;
    dueAt: DateTime option;
    duration: TimeSpan option;
    dependsOn: Task list option;
    startsAt: DateTime option;
}

type Goal = {
    name: string;
    tasks: Task list;
}

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    0 // return an integer exit code
