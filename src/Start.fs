module Start

open Fable.Core
open Fable.Import
open Infrastructure.Express
open TelegramBot
open Infrastructure.Logging
open Infrastructure.Config
open Infrastructure.Mongo.Common
open Endpoints

[<Import("createLocalTunnel", "./js/localtunnel.js")>]
let createLocaltunnel: int -> JS.Promise<string> = jsNative

let config = loadHostConfig()
let mongoConfig = loadMongoConfig()
let bot = Create config.token
let logger = config.logLevelConsole |> createLogger


mongoConfig.uri
|> startUpMongo
|> Promise.bind (fun _ ->
    match isProduction with
    | true -> Promise.lift config.host
    | false -> createLocaltunnel config.port |> Promise.tap (fun url -> logger.LogTrace(sprintf "tunnel url:%s" url)))
|> Promise.bind (fun url ->
    bot.setWebHook (sprintf "%s/bot%s" url config.token)
    |> Promise.tap (fun _ -> setBotEndpoints bot logger url)
    |> Promise.map (fun _ -> bot))
|> Promise.bind (startExpress config.token)
|> Promise.tap (setHttpEndpoints logger)
|> Promise.tap (fun _ -> logger.LogInfo "listen" None)
|> Promise.tryStart (fun e -> logger.LogError e)
