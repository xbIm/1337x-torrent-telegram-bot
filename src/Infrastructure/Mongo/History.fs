module Infrastructure.Mongo.History

open Domain.Bot
open Domain.Session
open Fable.Core
open App.SearchOnSite

[<Import("addRecord", "../../js/history.js")>]
let private addRecord (int) (string): JS.Promise<History> = jsNative

[<Import("getHistory", "../../js/history.js")>]
let private getHistory (int): JS.Promise<Option<History>> = jsNative

let getHistoryImpl (chatId: ChatId) =
    getHistory <| unwrapChatId chatId

let addRecordImpl (chatId: ChatId) (text: string) =
    addRecord (unwrapChatId chatId) text
