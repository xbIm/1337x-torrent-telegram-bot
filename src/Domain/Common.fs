module Common

open System
open Fable.Core

[<AbstractClass>]
type Logger() =
    abstract LogTrace: string -> unit
    abstract LogDebug: string -> unit
    //todo: add event name
    abstract LogInfo: string -> obj array option -> unit
    abstract LogError: Exception -> unit
    // todo: add tracing
    abstract LogInfoDuration: string -> obj array option -> ('B -> JS.Promise<'A>) -> 'B -> JS.Promise<'A>

let tryParseWith (tryParseFunc: string -> (bool * _)) =
    tryParseFunc
    >> function
    | true, v -> Some v
    | false, _ -> None

let parseInt = tryParseWith System.Int32.TryParse

module PromiseResult =

    let map (fn: 'A -> 'B) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'B, 'E>> =
        Promise.map (Result.map fn) a

    let tap (fn: 'A -> unit) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'A, 'E>> =
        a
        |> Promise.tap (fun result ->
            match result with
            | Ok success -> (fn success)
            | Error e -> e |> ignore)

    let bind (fn: 'A -> JS.Promise<Result<'B, 'E>>) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'B, 'E>> =
        a
        |> Promise.bind (fun a ->
            match a with
            | Ok a ->
                fn a
            | Error e ->
                Promise.lift (Error e))

    let bindResult (fn: 'A -> Result<'B, 'E>) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'B, 'E>> =
        a
        |> Promise.bind (fun result ->
            match result with
            | Ok success ->
                fn success
            | Error e ->
                (Error e)
            |> Promise.lift)

    let bindAsync (fn: 'A -> JS.Promise<'B>) (a: JS.Promise<Result<'A, 'E>>): JS.Promise<Result<'B, 'E>> =
        a
        |> Promise.bind (fun result ->
            match result with
            | Ok success ->
                (fn success) |> Promise.map (fun a -> Result.Ok a)
            | Error e ->
                Promise.lift (Error e))
