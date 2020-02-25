module Infrastructure.Express

open Fable.Core
open System
open Common
open ExpressTypes
open TelegramBot

[<Import("default", "express")>]
let express: unit -> Application = jsNative

[<AbstractClass>]
type BodyParser() =
    abstract json: unit -> obj

[<Import("default", from = "body-parser")>]
let bodyParser: BodyParser = jsNative

let startExpress =
    fun (token: string) (bot: TelegramBot) ->
        let app = express()

        app.``use`` <| bodyParser.json()

        app.post (sprintf "/bot%s" token) (fun req res ->
            bot.processUpdate req.body
            res.sendStatus 200)

        app.get "/healthcheck" (fun _ res -> res.sendStatus (200))
        app.get "/xtbot/healthcheck" (fun _ res -> res.sendStatus (200))

        Promise.create (fun success fail -> app.listen 3000 (fun e -> success (app)))

type HttpResponse =
    | ClientError of string
    | Redirect of string

type HttpLogic<'A> = 'A -> JS.Promise<HttpResponse>

//todo: logging
[<Emit("$0.headers['user-agent']")>]
let getUserAgent (req: IRequest): string = jsNative

let handle (httpLogic: HttpLogic<'A>): RouterHandler =
    fun req res ->
        match (getUserAgent(req).Contains("TelegramBot")) with
        | true -> res.sendStatus 400
        | false ->
            httpLogic (req.``params`` :?> 'A)
            |> Promise.iter (fun response ->
                match (response) with
                | ClientError _ -> res.sendStatus 400
                | Redirect adress -> res.redirect adress)

let addHttpEndpoint<'A> (app: Application) (logger: Logger) (arg: string * string * HttpLogic<'A>): unit =
    match arg with
    | (method, path, f) ->
        match (method) with
        | "get" -> app.get path (handle f)
        | "post" -> app.post path (handle f)
        | _ -> raise (Exception("no such method"))
