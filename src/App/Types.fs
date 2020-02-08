module Achieve.Types
open System

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

