module Infrastructure.Mongo.SearchArgs

open Domain.SearchArgs
open Domain.Bot
open Fable.Core

[<Import("getSearchArgs", "../../js/searchArgs.js")>]
let private getSearchArgs (int): JS.Promise<Option<SearchArgs>> = jsNative

[<Import("updateSearchArgs", "../../js/searchArgs.js")>]
let private updateSearchArgs (int) (key: string) (value: string): JS.Promise<SearchArgs> = jsNative

let getSearchArgsImpl: GetSearchArgs =
    fun chatId ->
        chatId
        |> unwrapChatId
        |> getSearchArgs

let updateSearchArgsImpl (chatId:ChatId) (key: string) (value: string) =
    updateSearchArgs (unwrapChatId chatId) key value
