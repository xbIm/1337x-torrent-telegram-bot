module Domain.MagneticLink

open Common
open Domain.Bot
open Domain.Request
open Domain.Session
open Fable.Core

type MagneticLink = MagneticLink of string

let unwrapMagneticLink (MagneticLink a) = a

type GetIdReqDto =
    { chatId: string
      getId: string }

type GetMagneticLink = GetSession -> Request -> (Response -> Result<MagneticLink, BotError>) -> ChatId -> int -> JS.Promise<Result<MagneticLink, BotError>>

type GetMagneticDto = GetSession -> Request -> (Response -> Result<MagneticLink, BotError>) -> GetIdReqDto -> JS.Promise<Result<MagneticLink, BotError>>

//todo: proper curring
let toGetMageticLink (func: GetMagneticLink): GetMagneticDto =
    fun getSession request parse dto ->
        let chatId = ChatId <| int dto.chatId
        let id = int dto.getId
        func getSession request parse chatId id


//todo: logging
let getMagneticLink: GetMagneticLink =
    fun getSession request parse chatId getId ->
        getSession chatId
        |> Promise.map (fun session -> session.torrents.Find(fun e -> e.id = getId).url)
        |> Promise.map (fun url ->
            match (url) with
            //todo: refactor address
            | Some url -> Ok <| Url (sprintf "https://1337x.am/%s"  url)
            | None -> Error <| ParseError "no url")
        |> bindPromise request
        |> bindResult parse
