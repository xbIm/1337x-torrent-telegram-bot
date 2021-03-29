module Domain.MagneticLink

open Common
open Common
open Domain.Bot
open Domain.Request
open Domain.Session
open Fable.Core

type MagneticLink = MagneticLink of string

let unwrapMagneticLink (MagneticLink a) = a

type GetIdReq =
    { chatId: ChatId
      getId: int }

type GetIdReqDto =
    { chatId: string
      getId: string }

let ofDto (dto: GetIdReqDto): Result<GetIdReq, BotError> =
    let chatId = ChatId <| int dto.chatId
    match parseInt dto.getId with
    | Some id ->
        Ok
            { chatId = chatId
              getId = id }
    | None -> Error <| ValidationError "not an integer"

type GetMagneticLink =
    GetSession -> Request -> (Response -> Result<MagneticLink, BotError>) -> GetIdReq -> JS.Promise<Result<MagneticLink, BotError>>

type GetMagneticDto =
    GetSession -> Request -> (Response -> Result<MagneticLink, BotError>) -> GetIdReqDto -> JS.Promise<Result<MagneticLink, BotError>>

let toGetMageticLink (func: GetMagneticLink): GetMagneticDto =
    fun getSession request parse dto ->
        match ofDto dto with
        | Ok s -> func getSession request parse s
        | Error e -> Promise.lift <| Error e


//todo: logging
let getMagneticLink: GetMagneticLink =
    fun getSession request parseForMagneticLink req ->
        getSession req.chatId
        |> Promise.map (fun session ->
            match session |> Option.bind (findTorrent req.getId) with
            | Some e -> Ok e.url
            | None -> Error <| NoSession)
        |> PromiseResult.bindResult (fun url ->
            match (url) with
            | Some url -> Ok <| Url.create url
            | None -> Error <| ValidationError "no url")
        |> PromiseResult.bindAsync request
        |> PromiseResult.bindResult parseForMagneticLink
