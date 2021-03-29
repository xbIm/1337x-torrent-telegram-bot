module Infrastructure.Mongo.UserOptions

open App.Domain.UserOptions
open Domain.SearchArgs
open Domain.Bot
open Fable.Core

[<Import("getUserOptions", "../../js/userOptions.js")>]
let private getUserOptions (int): JS.Promise<Option<UserOptions>> = jsNative

[<Import("updateUserOptions", "../../js/userOptions.js")>]
let private updateUserOptions (int) (key: string) (value: string): JS.Promise<UserOptions> = jsNative

let getUserOptionsImpl: GetUserOptions =
    fun chatId ->
        chatId
        |> unwrapChatId
        |> getUserOptions

let updateUserOptionsImpl (chatId:ChatId) (key: string) (value: string) =
    updateUserOptions (unwrapChatId chatId) key value
