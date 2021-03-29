module Infrastructure.Request

open Domain.Request
open Fable.Core
open Fable.Core.JsInterop



[<Import("request", "../js/request.js")>]
let requestImpl : string -> JS.Promise<Response> = jsNative

let request : Request = fun url -> requestImpl <| unwrapUrl url
