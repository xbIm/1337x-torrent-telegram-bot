module App.Pagination

open App
open Common
open Domain.Request
open Domain.Session
open Fable.Core
open App.Parse
open Domain.Bot
open App.SearchOnSite

type Direction =
    | Prev
    | Next

let addPage (logger:Logger) (session: Session) (request: Request): JS.Promise<Result<Session, BotError>> =
    //todo: one domain value
    promise {
        let url = Url(sprintf "https://1337x.am%s" session.next.Value)
        logger.LogTrace <| sprintf "url:%s" (unwrapUrl url)

        let! res = request (url)

        return parseTorrentTable res.html session.currentPosition
               |> Result.bind (fun tt ->
                   session.torrents.AddRange(tt.torrents)
                   Ok { session with next = tt.next })
    }



let changePage (logger:Logger) (request: Request) (session: Session) (d: Direction): JS.Promise<Result<Session, BotError>> =
    match (d) with
    | Next ->
        let nextPos = session.currentPosition + 10
        match (session.torrents.Count >= nextPos + 10) with
        | true -> Promise.lift <| Ok { session with currentPosition = nextPos }
        | false -> addPage logger { session with currentPosition = nextPos } request
    | Prev ->
        let prevPos = session.currentPosition - 10
        if prevPos < 0 then Promise.lift (Error <| Text "currentPosition can't be negative")
        else Promise.lift <| Ok { session with currentPosition = prevPos }

let validateMessageId (messageId: int) (session: Session) =
    match (session.messageId > messageId) with
    | true -> Error <| Text "session expired"
    | false -> Ok session

let parseData (req: UserRequest) =
    let str = SearchOnSite.unwrapUserRequest req
    match (str.[0]) with
    | '0' -> Ok Prev
    | '1' -> Ok Next
    | _ -> Error <| Text "Wrong Data"

let pagination (logger: Logger) (request: Request) (saveSession: SaveSession) (getSession: GetSession): BotLogic =
    fun req ->
        getSession req.chatId
        |> bindPromiseToResult (validateMessageId req.messageId)
        |> bindPromiseResult (fun s ->
            parseData req.userRequest |> bindResultToPromiseResult (fun d -> changePage logger request s d))
        |> bindPromise saveSession
        |> tapPromiseResult (fun session -> logger.LogTrace(sprintf "currentPos=%d" session.currentPosition))
        |> mapPromise toBotResponse

