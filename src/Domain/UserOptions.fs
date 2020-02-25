module App.Domain.UserOptions

open Domain.Bot
open Domain.SearchArgs
open Fable.Core

//todo:make dto
type UserOptions =
    { showMagnetic: string option }

type GetUserOptions = ChatId -> JS.Promise<Option<UserOptions>>


let isShowMagnetic (uo: UserOptions option) =
    match uo with
    | None -> false
    | Some uo ->
        match uo.showMagnetic with
        | Some str -> str = "on"
        | _ -> false

type UserOptionsSetter(f: ChatId -> string -> string -> JS.Promise<UserOptions>) =
    interface Setter<UserOptions> with
        member this.Name = "userOptions"
        member this.FriendlyName = "user options"
        member this.Titles = [| "showMagnetic" |]

        member this.Values = dict [ ("showMagnetic", [| "on"; "off" |]) ]

        member this.Update chatId key value = f (chatId) (key) (value)
