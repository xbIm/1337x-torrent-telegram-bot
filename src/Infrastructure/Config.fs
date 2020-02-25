module App.Infrastructure.Config

open Fable.Core
open Fable.Import
open Fable.Core.JsInterop

[<Import("getEnvVal", "../js/logging.js")>]
let getEnvVar: string -> string option = jsNative

let getEnvVarOrDeFault: string * string -> string =
    fun (param, def) ->
        match (getEnvVar param) with
        | Some string -> string
        | None _ -> def

let nodeEnv = getEnvVarOrDeFault ("NODE_ENV", "development")

let isProduction =
    match nodeEnv with
    | "production" -> true
    | _ -> false

type LogConfig =
    { host: string
      port: int
      logLevelConsole: string
      token: string }

type MongoConfig =
    { uri: string }

let loadLogConfig =
    fun () ->
        { host = getEnvVarOrDeFault ("HOST", "")
          port = getEnvVarOrDeFault ("PORT", "3000") |> int
          logLevelConsole = getEnvVarOrDeFault ("LOG_LEVEL_CONSOLE", "silly")
          token = getEnvVarOrDeFault ("TOKEN", "") }

let loadMongoConfig = fun () -> { uri = getEnvVarOrDeFault ("MONGO_URI", "mongodb://localhost:27017/admin") }
