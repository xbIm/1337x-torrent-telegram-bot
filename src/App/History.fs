module App.History

open App
open Common
open Domain.Bot
open Domain.Session

let toBotResponse (history: History) =
    Ok
        { response =
              MessageWithKeyBoard
                  ("Your latest requests:",
                   Array.append (history.records |> Array.map (fun e -> e.text)) ([| "cancel" |])) }

let showHistory (logger: Logger) (getHistory: GetHistory) (req: BotRequest) =
    req.chatId
    |> (getHistory |> logger.LogInfoDuration "mongo:showHistory" None)
    |> Promise.map toBotResponse
