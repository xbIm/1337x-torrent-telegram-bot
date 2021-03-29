module Domain.SearchArgs

open System.Collections.Generic
open Domain.Bot
open Fable.Core

type SearchArgs =
    { userId: ChatId
      category: string option
      orderby: string option }

type GetSearchArgs = ChatId -> JS.Promise<Option<SearchArgs>>

type Setter<'A> =
    abstract Regex: string
    abstract Name: string
    abstract FriendlyName: string
    //todo: delete titles
    abstract Titles: string array
    abstract Values: IDictionary<string, string []>
    abstract Update: ChatId -> string -> string -> JS.Promise<'A>

type SearchArgsSetter(f: ChatId -> string -> string -> JS.Promise<SearchArgs>) =
    interface Setter<SearchArgs> with
        member this.Regex = "search[aA]rgs"
        member this.Name = "searchArgs"
        member this.FriendlyName = "search options"
        member this.Titles = [| "category"; "orderby" |]

        member this.Values =
            dict
                [ ("category",
                   [| "All"; "Movies"; "TV"; "Games"; "Music"; "Applications"; "Documentaries"; "Anime"; "Other"; "XXX" |])
                  ("orderby", [| "Default"; "Time"; "Size"; "Seeders"; "Leechers" |]) ]

        member this.Update chatId key value = f (chatId) (key) (value)

let toUrlCategory (text: string option) =
    match (text) with
    | None -> None
    | Some (text) ->
        match (text) with
        | "All" -> None
        | "Movies" -> Some "Movies"
        | "TV" -> Some "TV"
        | "Games" -> Some "Games"
        | "Music" -> Some "Music"
        | "Applications" -> Some "Applications"
        | "Documentaries" -> Some "Documentaries"
        | "Anime" -> Some "Anime"
        | "Other" -> Some "Other"
        | _ -> None

let toUrlOrderBy (text:string option) =
    match (text) with
    | None -> None
    | Some (text) ->
        match (text) with
        | "Default" -> None
        | "Time" -> Some "time"
        | "Size" -> Some "size"
        | "Seeders" -> Some "seeders"
        | "Leechers" -> Some "leechers"
        | _ -> None
