module App.Infrastructure.Mongo.Common

open Fable.Core

[<Import("startUpMongo", "../../js/mongo.js")>]
let startUpMongo (string) : JS.Promise<unit> = jsNative
