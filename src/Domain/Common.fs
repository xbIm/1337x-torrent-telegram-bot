module Common

open System
open Fable.Core

[<AbstractClass>]
type Logger() =
    abstract LogTrace: string -> unit
    abstract LogDebug: string -> unit
    abstract LogInfo: string -> obj array option -> unit
    abstract LogError: Exception -> unit
    abstract LogInfoDuration: string -> obj array option -> ('B -> JS.Promise<'A>) -> 'B -> JS.Promise<'A>

//todo: proper monad naming
let bindPromiseResult (fn: 'A -> JS.Promise<Result<'B, 'E>>) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'B, 'E>> =
    a
    |> Promise.bind (fun a ->
        match a with
        | Ok a ->
            fn a
        | Error e ->
            Promise.lift (Error e))

let bindResultToPromiseResult (fn: 'A -> JS.Promise<Result<'B, 'E>>) (a: Result<'A, 'E>): JS.Promise<Result<'B, 'E>> =
    match a with
    | Ok a ->
        fn a
    | Error e ->
        Promise.lift (Error e)

let bindResult (fn: 'A -> Result<'B, 'E>) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'B, 'E>> =
    a
    |> Promise.bind (fun a ->
        match a with
        | Ok a ->
            Promise.lift (fn a)
        | Error e ->
            Promise.lift (Error e))

let mapPromise (fn: 'A -> 'B) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'B, 'E>> =
    Promise.map (Result.map fn) a

let tapPromiseResult (fn: 'A -> unit) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'A, 'E>> =
    a
    |> Promise.tap (fun result ->
        match result with
        | Ok success -> (fn success)
        | Error e -> e |> ignore)

let bindPromise (fn: 'A -> JS.Promise<'B>) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'B, 'E>> =
    a
    |> Promise.bind (fun result ->
        match result with
        | Ok success ->
            (fn success) |> Promise.map (fun a -> Result.Ok a)
        | Error e ->
            Promise.lift (Error e))

let bindRPromise (fn: 'A -> JS.Promise<'B>) (a: Result<'A, 'E>): JS.Promise<Result<'B, 'E>> =
    match a with
    | Ok ok -> fn ok |> Promise.map (fun a -> Result.Ok a)
    | Error e -> Error e |> Promise.lift

let bindPromiseToResult (fn: 'A -> Result<'B, 'E>) (a: JS.Promise<'A>): JS.Promise<Result<'B, 'E>> =
    a |> Promise.map (fun r -> fn r)

//todo: delete
[<Emit("console.log($0)")>]
let log (obj: obj): unit = jsNative
