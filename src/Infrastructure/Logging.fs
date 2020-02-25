module App.Infrastructure.Logging

open Fable.Core
open Common


[<Import("createLogger", "../js/logging.js")>]
let createLogger: string -> Logger = jsNative
