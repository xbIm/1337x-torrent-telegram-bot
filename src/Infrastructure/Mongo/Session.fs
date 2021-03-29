module Infrastructure.Mongo.Session

open Domain.Bot
open Domain.Session
open Fable.Core


[<Import("saveSession", "../../js/session.js")>]
let saveSession (session: Session): JS.Promise<Session> = jsNative

[<Import("getSession", "../../js/session.js")>]
let private getSession (int): JS.Promise<Option<Session>> = jsNative

let getSessionImpl (chatId: ChatId) =
    getSession <| unwrapChatId chatId
