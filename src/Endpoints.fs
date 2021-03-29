module Endpoints

open App
open App.Domain.UserOptions
open System.Text.RegularExpressions
open TelegramBot
open Domain.Bot
open Domain.MagneticLink
open App.SearchOnSite
open Infrastructure.Mongo.Session
open App.Parse
open Infrastructure.Request
open Infrastructure.Bot.bot
open Infrastructure.Mongo.SearchArgs
open Infrastructure.Mongo.UserOptions
open Infrastructure.Mongo.History
open Common
open Domain.SearchArgs
open Infrastructure.Express
open ExpressTypes
open Pagination

let setBotEndpoints (bot: TelegramBot) (logger: Logger) (host: string) =
    let search = SearchTorrentsImpl logger getSearchArgsImpl makeUrlImpl request addRecordImpl saveSession

    addBotText bot logger
        (Regex("/start"), "start",
         String
             ("Hi, I\'m searching for torrents on 1337x.to, "
              + "To start searching write me what torrent link are you looking for or press /help for more information."))

    let helpText =
        "<b>What can I do?</b>\nTell me what do you need to find and I ll do it\n\nBrief list of commands:\n"
        + "/start - the first command \n" + "/help - brief information, which you are reading now\n"
        + "/search - torrents search\n" + "/searchArgs - set search arguments for torrents search\n"
        + "/history - show 10 last requests \n" + "/userOptions option - set user options for torrents search\nP.S. \n"
        + "- This is not official bot of 1337x.to\n" + "- For question please contact @xbimz "

    addBotText bot logger (Regex("/help"), "help", HtmlString helpText)
    addBotText bot logger (Regex("cancel"), "cancel", RemoveKeyBoard "Cancel. Let\' start over")
    addBotLogicAsync bot logger (Regex("/history"), "showHistory", History.showHistory logger getHistoryImpl)

    addBotLogicAsync bot logger (Regex("/next"), "nextResult", History.next logger getHistoryImpl search)
    addBotLogicAsync bot logger (Regex("/search (.+)"), "search", search)

    addBotLogicAsync bot logger
        (Regex("\/get([0-9]+)"), "onGet",
         onGet logger host getUserOptionsImpl getSessionImpl
             (getMagneticLink getSessionImpl request parseMagnetLink))

    addSetter bot logger (SearchArgsSetter(updateSearchArgsImpl))
    addSetter bot logger (UserOptionsSetter(updateUserOptionsImpl))

    addBotText bot logger (Regex("^\/.+"), "notfound", String "Command not found, try /help")
    addBotLogicAsync bot logger (Regex("(.+)"), "search", search)

    bindCallbackQuery bot logger (Regex("(.+)"), "search", pagination logger request saveSession getSessionImpl)
    bot.on_callback_edited (fun msg-> Fable.Core.JS.console.log msg)

let redirectToLink (req: GetIdReqDto) =
    (toGetMageticLink getMagneticLink) getSessionImpl request parseMagnetLink req
    |> Promise.map (fun r ->
        match (r) with
        | Ok(link) -> Redirect(unwrapMagneticLink link)
        | Error _ -> ClientError "error")


let setHttpEndpoints (logger: Logger) (app: Application) =
    addHttpEndpoint app logger ("get", "/xtbot/get/:chatId/:getId", redirectToLink)
