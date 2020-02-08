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
                            | _ -> Environment.GetEnvironmentVariable("HOME")
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
