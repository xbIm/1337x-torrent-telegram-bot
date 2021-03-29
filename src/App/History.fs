module App.History

open System.Text.RegularExpressions
open Common
open Domain.Bot
open Domain.Session
open Fable.Core

let toBotResponse (history: Option<History>) =
    match history with
    | Some history ->
        { response =
              MessageWithKeyBoard
                  ("Your latest requests:",
                   Array.append
                       (history.records
                        |> Array.rev
                        |> Array.map (fun e -> e.text)) ([| "cancel" |])) }
    | None ->  { response = String "Search something first" }
    |> Ok


let showHistory (logger: Logger) (getHistory: GetHistory) (req: BotRequest) =
    req.chatId
    |> (getHistory |> logger.LogInfoDuration "mongo:showHistory" None)
    |> Promise.map toBotResponse

let addOne (s: string) =
    let num = Regex(".*(?:\D|^)(\d+)").Match(s)
    match num.Success with
    | true ->
        let nextNumPadded =
            ((int num.Groups.[1].Value) + 1)
                .ToString()
                .PadLeft(num.Groups.[1].Value.Length, '0')
        Ok <| s.Replace(num.Groups.[1].Value, nextNumPadded)
    | false ->
        Error <| ValidationError "No last number"


let next (logger: Logger) (getHistory: GetHistory)
    (searchTorrents: BotRequest -> JS.Promise<Result<BotResponse, BotError>>) (req: BotRequest) =
    req.chatId
    |> (getHistory |> logger.LogInfoDuration "mongo:showHistory" None)
    |> Promise.bind (fun h ->
        match h with
        | None -> Promise.lift <| Ok { response = String "Search something first" }
        | Some h ->
            match (h.records) with
            | records when h.records.Length > 0 ->
                match (addOne records.[records.Length - 1].text) with
                | Ok s -> searchTorrents { req with userRequest = UserRequest s }
                | Error e -> Promise.lift <| Error e
            | _ ->
                Promise.lift <| Ok { response = String "Search something first" })
