module Infrastructure.Bot.bot

open System
open App
open Common
open Domain.Bot
open Domain.SearchArgs
open Fable.Core
open TelegramBot
open System.Text.RegularExpressions

[<Import("wrapTrace", "../../js/scopeMiddleware.js")>]
let wrapTrace (logger: Logger) (funcName: string) (msg: Message) (f: unit -> JS.Promise<Message>): unit = jsNative

let reply (bot: TelegramBot) (msg: Message) (response: ResponseText) =
    match response with
    | String (text) ->
        bot.sendMessage (U2.Case1 msg.chat.id, text)
    | HtmlString text ->
        bot.sendMessage
            (U2.Case1 msg.chat.id, text,
             { parse_mode = Some ParseMode.HTML
               reply_markup = None })
    | RemoveKeyBoard text ->
        bot.sendMessage
            (U2.Case1 msg.chat.id, text,
             { reply_markup =
                   Some
                       (U4.Case3
                           { remove_keyboard = Some true
                             selective = Some false })
               parse_mode = None })
    | MessageWithKeyBoard (text, array) ->
        bot.sendMessage
            (U2.Case1 msg.chat.id, text,
             { reply_markup =
                   Some
                       (U4.Case2
                           { keyboard =
                                 array
                                 |> Seq.map (fun e -> [| { text = e } |])
                                 |> Seq.toList
                             one_time_keyboard = Some true
                             selective = Some false })
               parse_mode = None })
    | MessageWithInlineKeyBoard (text, array) ->
        bot.sendMessage
            (U2.Case1 msg.chat.id, text,
             { reply_markup =
                   Some
                       (U4.Case1
                           { inline_keyboard =
                                 array
                                 |> Seq.map (fun e ->
                                     [| { text = e.text
                                          callback_data = e.callbackData } |])
                                 |> Seq.toList })
               parse_mode = Some ParseMode.HTML })

let addBotText (bot: TelegramBot) (logger: Logger) (arg: Regex * string * ResponseText): unit =
    match arg with
    | (regex, name, response) ->
        bot.onText (regex, (fun (msg: Message) _ ->
        if not (msg.chat.``type`` = ChatType.Group && regex.ToString()="/^\/.+/g") then
          wrapTrace logger name msg (fun _ -> reply bot msg response)))

let toBotRequest: Message -> string array option -> BotRequest =
    fun msg rgx ->
        match rgx with
        | None ->
            { chatId = ChatId msg.chat.id
              chatType = msg.chat.``type``
              userRequest = UserRequest ""
              secondArg = None
              messageId = msg.message_id }
        | Some argv ->
            match argv with
            | arr when argv.Length = 2 ->
                let first = arr.[1]

                { chatId = ChatId msg.chat.id
                  chatType = msg.chat.``type``
                  userRequest = UserRequest <| first.ToString()
                  secondArg = None
                  messageId = msg.message_id }
            | arr when argv.Length > 2 ->
                let first = arr.[1]
                let second = arr.[2]
                { chatId = ChatId msg.chat.id
                  chatType = msg.chat.``type``
                  userRequest = UserRequest <| first.ToString()
                  secondArg = Some second
                  messageId = msg.message_id }
            | _ ->
                { chatId = ChatId msg.chat.id
                  chatType = msg.chat.``type``
                  userRequest = UserRequest ""
                  secondArg = None
                  messageId = msg.message_id }


let matchBotError (error: BotError) =
    match (error) with
    | NoSession -> String "Session not found"
    | ValidationError validationError -> String validationError
    | Text text -> String text
    | ParseError _ -> String "Something went wrong."

let logBotError (logger: Logger) (error: BotError): unit =
    match (error) with
    | ValidationError _ -> ()
    | NoSession -> ()
    | Text _ -> ()
    | ParseError text -> logger.LogError(Exception(text))

//todo: refactor logger
//todo: refactor logging result
let addBotLogicAsync (bot: TelegramBot) (logger: Logger) (arg: Regex * string * BotLogicAsync): unit =
    match arg with
    | (command, name, f) ->
        bot.onText
            (command,
             (fun msg args ->
                 wrapTrace logger name msg (fun _ ->
                     toBotRequest msg args
                     |> f
                     |> Promise.bind (fun res ->
                         match res with
                         | Ok ok -> reply bot msg ok.response
                         | Error e ->
                             logBotError logger e
                             reply bot msg (matchBotError e))
                     //todo: logging
                     |> Promise.catchBind (fun e ->
                         logger.LogError e
                         reply bot msg (String "Something went wrong")))))

let addBotLogic (bot: TelegramBot) (logger: Logger) (arg: Regex * string * BotLogic): unit =
    match arg with
    | (command, name, f) ->
        bot.onText
            (command,
             (fun msg args ->
                 wrapTrace logger name msg (fun _ ->
                     toBotRequest msg args
                     |> f
                     |> (fun res ->
                         match res with
                         | Ok ok -> reply bot msg ok.response
                         | Error e ->
                             logBotError logger e
                             reply bot msg (matchBotError e)))))

let bindCallbackQuery
    (bot: TelegramBot)
    (logger: Logger)
    (arg: Regex * string * (BotRequest -> JS.Promise<Result<BotResponse, BotError>>))
    : unit
    =
    match arg with
    | (command, name, f) ->
        bot.on_callback_query (fun query ->
            logger.LogInfo "onCallback query called" None
            let opts =
                { chat_id = query.message.Value.chat.id
                  message_id = query.message.Value.message_id
                  reply_markup = None }
            wrapTrace logger name query.message.Value (fun _ ->

                let r =
                    { chatId = ChatId query.from.id
                      chatType = query.message.Value.chat.``type``
                      userRequest = UserRequest query.data.Value
                      secondArg = None
                      messageId = query.message.Value.message_id }
                f r
                |> Promise.bind (fun res ->
                    match res with
                    | Ok ok ->
                        match (ok.response) with
                        | String str -> bot.editMessageText (str, opts)
                        | MessageWithInlineKeyBoard (text, array) ->
                            bot.editMessageText
                                (text,
                                 { opts with
                                       reply_markup =
                                           Some
                                               { inline_keyboard =
                                                     array
                                                     |> Seq.map (fun e ->
                                                         [| { text = e.text
                                                              callback_data = e.callbackData } |])
                                                     |> Seq.toList } })
                        | _ -> bot.editMessageText ("Something went wrong", opts)
                    | Error e ->
                        match (matchBotError e) with
                        | String str -> bot.editMessageText (str, opts)
                        | _ -> bot.editMessageText ("Something went wrong", opts))
                |> Promise.catchBind (fun e ->
                    logger.LogError e
                    bot.editMessageText ("Something went wrong", opts))))


let addSetter (bot: TelegramBot) (logger: Logger) (setter: Setter<'A>): unit =
    addBotLogicAsync bot logger
        (Regex(sprintf "/{1}%s (.+) (.+)$" setter.Regex), "onCommand",
         (fun r ->
             let key = SearchOnSite.unwrapUserRequest r.userRequest
             let value = r.secondArg
             let contains = Array.contains key setter.Titles
             match (contains, value) with
             | (true, Some value) ->
                 let contains2 = Array.contains value (setter.Values.Item(key))
                 match contains2 with
                 | true ->
                     setter.Update (r.chatId) (key) (value)
                     |> Promise.map (fun _ ->
                         Ok
                             { response =
                                   RemoveKeyBoard(sprintf "%s %s has been set to %s" setter.FriendlyName key value) })
                 | false ->
                     { response = String "you entered wrong value" }
                     |> Ok
                     |> Promise.lift
             | _ ->
                 { response = String "you entered wrong value" }
                 |> Ok
                 |> Promise.lift))

    addBotLogic bot logger
        (Regex(sprintf "/{1}%s (.+)$" setter.Regex), "onCommand",
         (fun r ->
             let key = SearchOnSite.unwrapUserRequest r.userRequest
             let contains = Array.contains key setter.Titles
             match (contains) with
             | true ->
                 { response =
                       MessageWithKeyBoard
                           (sprintf "Please choose %s %s" setter.FriendlyName key,
                            Array.append
                                (setter.Values.Item(key)
                                 |> Array.map (fun e -> sprintf "/%s %s %s" setter.Name key e)) ([| "cancel" |])) }
             | false -> { response = String "key not found " }
             |> Ok))

    addBotLogic bot logger
        (Regex(sprintf "/{1}%s$" setter.Regex), "onCommand",
         (fun _ ->
             { response =
                   MessageWithKeyBoard
                       (sprintf "Please choose %s" setter.FriendlyName,
                        Array.append (setter.Titles |> Array.map (fun e -> sprintf "/%s %s" setter.Name e))
                            ([| "cancel" |])) } |> Ok))
