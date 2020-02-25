module Endpoints

open App
open App.Domain.UserOptions
open System.Text.RegularExpressions
open TelegramBot
open Domain.Bot
open Domain.MagneticLink
open App.SearchOnSite
open App.Infrastructure.Mongo.Session
open App.Parse
open App.Infrastructure.Request
open App.Infrastructure.Bot.bot
open App.Infrastructure.Mongo.SearchArgs
open App.Infrastructure.Mongo.UserOptions
open Infrastructure.Mongo.History
open Common
open Domain.SearchArgs
open Infrastructure.Express
open ExpressTypes
open Pagination

let setBotEndpoints (bot: TelegramBot) (logger: Logger) (host: string) =
    bindText bot logger
        (Regex("/start"), "start",
         String
             ("Hi, I\'m searching for torrents on 1337x.to, "
              + "To start searching write me what torrent link are you looking for or press /help for more information."))

    let helpText =
        "<b>What can I do?</b>\nTell me what do you need to find and I ll do it\n\nBrief list of commands:\n"
        + "/start - the first command \n"
        + "/help - brief information, which you are reading now\n"
        + "/search - torrents search\n"
        + "/searchArgs - set search arguments for torrents search\n"
        + "/history - show 10 last requests \n"
        + "/userOptions option - set user options for torrents search\nP.S. \n"
        + "- This is not official bot of 1337x.to\n" + "- For question please contact @xbimz "

    bindText bot logger (Regex("/help"), "help", HtmlString helpText)
    bindText bot logger (Regex("cancel"), "cancel", RemoveKeyBoard "Cancel. Let\' start over")
    bindReqRes bot logger (Regex("/history"), "showHistory", History.showHistory logger getHistoryImpl)

    bindReqRes bot logger
        (Regex("\/get(.+)"), "onGet",
         onGet logger host getUserOptionsImpl getSessionImpl (getMagneticLink getSessionImpl request parseMagnetLink))

    bindSetter bot logger (SearchArgsSetter(updateSearchArgsImpl))
    bindSetter bot logger (UserOptionsSetter(updateUserOptionsImpl))

    bindText bot logger (Regex("^\/.+"), "notfound", String "Command not found, try /help")
    bindReqRes bot logger
        (Regex("(.+)"), "search",
         SearchTorrentsImpl logger getSearchArgsImpl makeUrlImpl request addRecordImpl saveSession)

    bindCallbackQuery bot logger (Regex("(.+)"), "search", pagination logger request saveSession getSessionImpl)

let redirectToLink (req: GetIdReqDto) =
    (toGetMageticLink getMagneticLink) getSessionImpl request parseMagnetLink req
    |> Promise.map (fun r ->
        match (r) with
        | Ok(link) -> Redirect(unwrapMagneticLink link)
        | Error _ -> ClientError "error")


let setHttpEndpoints (logger: Logger) (app: Application) =
    addHttpEndpoint app logger ("get", "/xtbot/get/:chatId/:getId", redirectToLink)
