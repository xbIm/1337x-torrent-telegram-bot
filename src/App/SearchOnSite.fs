module App.SearchOnSite

open System.Text.RegularExpressions
open Common
open Fable.Core
open Domain.Bot
open Domain.Request
open Domain.Parse
open Domain.SearchArgs
open App.Domain.UserOptions
open Domain.Session
open App.Parse
open Domain.MagneticLink

let domain = "https://1337x.am/"

let inline (=>>) fn a = bindResult fn a

type MakeUrl = Option<SearchArgs> -> UserRequest -> Result<Url, BotError>

let unwrapUserRequest (UserRequest a) = a

let makeUrlImpl: MakeUrl =
    fun searchArgsOpt userRequest ->
        let text = (unwrapUserRequest userRequest).Replace(" ", "+")
        match searchArgsOpt with
        | Some(searchArgs) ->
            match (toUrlCategory searchArgs.category, toUrlOrderBy searchArgs.orderby) with
            | (Some category, Some orderby) ->
                sprintf "%ssort-category-search/%s/%s/%s/desc/1/" domain text category orderby
            | (Some category, None) -> sprintf "%scategory-search/%s/%s/1/" domain text category
            | (None, Some orderby) -> sprintf "%ssort-search/%s/%s/desc/1/" domain text orderby
            | (None, None) -> sprintf "%ssearch/%s/1/" domain text
        | None ->
            sprintf "%ssearch/%s/1/" domain text
        |> Url
        |> Result.Ok

type AddRecord = ChatId -> string -> JS.Promise<History>

type SearchTorrents = Logger -> GetSearchArgs -> MakeUrl -> Request -> AddRecord -> SaveSession -> BotRequest -> JS.Promise<Result<BotResponse, BotError>>

[<Emit("$0.reverse()")>]
let reverse (arr: ResizeArray<'T>): ResizeArray<'T> = jsNative

[<Emit("$0.slice($1,$2)")>]
let slice<'T> (arr: ResizeArray<'T>) (start: int) (``end``: int): ResizeArray<'T> = jsNative

let toBotResponse (session: Session): BotResponse =
    match session.torrents with
    | arr when session.torrents.Count > 0 ->
        let size = 10
        let mutable text = "The result of your search \n"
        let mutable i = 1
        let collection = reverse <| slice arr session.currentPosition (session.currentPosition + size)
        for elem in collection do
            text <-
                text
                + sprintf "%d. Name :%s\nTime: %s\nAuthor: %s\nS/L %d/%d Size: %s\nclick for link: /get%d\n\n" i
                      elem.title elem.time elem.author elem.seaders elem.leachers elem.size elem.id
            i <- i + 1

        let prevNext = ResizeArray<TextCallback>()
        if session.currentPosition <> 0 then
            prevNext.Add
                ({ text = "Previous"
                   callbackData = Some(sprintf "%d:%d" 0 session.currentPosition) })

        if session.next.IsSome || session.torrents.Count > session.currentPosition + size then
            prevNext.Add
                ({ text = "Next"
                   callbackData = Some(sprintf "%d:%d" 1 session.currentPosition) })

        { response = MessageWithInlineKeyBoard(text, prevNext) }
    | _ -> { response = String "No results were returned. Please refine your search." }

let validateBotRequest (botRequest: BotRequest) =
    let text = unwrapUserRequest botRequest.userRequest
    match text.Length with
    | l when l > 100 -> Error <| ValidationError "Text should be less than 100 symbols"
    | _ ->
        match Regex("^[A-Za-z0-9_ ]*$").IsMatch(text) with
        | false -> Error <| ValidationError "Only latin sign is allowed"
        | true -> Ok botRequest



let SearchTorrentsImpl: SearchTorrents =
    fun logger getSearchArgs makeUrl request addRecord saveSession botRequest ->
        validateBotRequest botRequest
        |> bindRPromise (fun r -> getSearchArgs r.chatId)
        |> bindResult (fun searchArgs -> makeUrl searchArgs botRequest.userRequest)
        |> tapPromiseResult (fun r -> logger.LogTrace <| sprintf "url:%s" (unwrapUrl r))
        |> bindPromise (request |> logger.LogInfoDuration "server respond" None)
        //|> tapPromiseResult (fun r -> logger.LogTrace <| sprintf "html:%s" r.html)
        |> bindResult (fun response -> parseTorrentTable response.html 0)
        |> mapPromise (fun parseResult -> toSession parseResult botRequest.chatId botRequest.messageId)
        |> bindPromise (fun s ->
            saveSession s
            |> Promise.bind (fun _ -> addRecord botRequest.chatId (unwrapUserRequest botRequest.userRequest))
            //todo: delete this map
            |> Promise.map (fun _ -> s))
        |> mapPromise toBotResponse

let onGet: Logger-> string -> GetUserOptions -> GetSession -> (ChatId -> int -> JS.Promise<Result<MagneticLink, BotError>>) -> BotRequest -> JS.Promise<Result<BotResponse, BotError>> =
    fun logger host getUserOptions getSession getMagneticLink botRequest ->
        let id = (unwrapUserRequest botRequest.userRequest)
        promise {
            let! session = (getSession |> logger.LogInfoDuration "mongo:getSession" None) botRequest.chatId
            let! userOpts = (getUserOptions |> logger.LogInfoDuration "mongo:getUserOptions" None) botRequest.chatId

            match (isShowMagnetic userOpts) with
            | true ->
                let! r = getMagneticLink (botRequest.chatId) (int id)
                return Result.map (fun link -> { response = String(unwrapMagneticLink link) }) (r)
            | false ->
                let link =
                    (sprintf "<a href='%s/xtbot/get/%d/%s'>download</a>" host (unwrapChatId botRequest.chatId) id)
                let torrent = session.torrents.Find(fun e -> e.id = (int id))
                return Ok
                           { response =
                                 HtmlString(sprintf "%s \nlink: %s" torrent.title link) }
        }
