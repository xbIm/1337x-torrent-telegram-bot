module App.Parse

open Domain.Bot
open Domain.Parse
open Domain.Request
open Domain.MagneticLink

let toOptStr (obj: Option<obj>): Option<string> =
    match obj with
    | Some s ->
        match s with
        | :? string as str -> Some str
        | _ -> None
    | _ -> None


let parseTorrentTable: Parse =
    fun x startId ->
        let r = x |> Cheerio.cheerio.load

        let torrents: ResizeArray<TorrentTableEntity> = ResizeArray<TorrentTableEntity>()
        let mutable id = startId
        r.Invoke(".table-list tbody").find("tr").each(fun index item ->
         let mutable td = Cheerio.cheerio.load(item).root().find("td:nth-child(1)")
         let anchor = td.find ("a:nth-child(2)")

         let url = toOptStr <| anchor.prop "href"
         td <- td.next()
         let seaders = int <| td.text()
         td <- td.next()
         let leachers = int <| td.text()
         td <- td.next()
         let time = td.text()
         td <- td.next()
         let size = td.text()
         td <- td.next()
         let author = td.find("a").text()
         id <- id + 1
         torrents.Add
             ({ id = id
                url = url
                title = anchor.text()
                size = size
                seaders = seaders
                leachers = leachers
                time = time
                author = author })

         None)
        |> ignore

        let activeLi = r.Invoke(".pagination .active")

        let (prev: Option<string>, next: Option<string>) =
            if activeLi.length > float 0 then
                (if activeLi.prev().length > float 0 then toOptStr <| activeLi.prev().children("a").prop("href")
                 else None),
                (if activeLi.next().length > float 0 then toOptStr <| activeLi.next().children("a").prop("href")
                 else None)
            else
                (None, None)

        Result.Ok
            { torrents = torrents
              prev = prev
              next = next }

let parseMagnetLink (response: Response) =
    let html = Cheerio.cheerio.load response.html
    let link = toOptStr <| html.Invoke(".no-top-radius").find("a").prop("href")
    match link with
    | Some link -> Result.Ok <| MagneticLink link
    | None -> Result.Error <| BotError.ParseError "magnetik link is empty"
