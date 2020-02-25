module Domain.Bot

open Fable.Core
open Fable.Core.JsInterop

type ChatId = ChatId of int

let unwrapChatId (ChatId a) = a

type UserRequest = UserRequest of string

type TextCallback =
    { text: string
      callbackData: string option }

type ResponseText =
    | String of string
    | HtmlString of string
    | RemoveKeyBoard of string
    | MessageWithKeyBoard of string * string array
    | MessageWithInlineKeyBoard of string * ResizeArray<TextCallback>

type BotRequest =
    { chatId: ChatId
      messageId: int
      userRequest: UserRequest
      secondArg: string option }

type BotResponse =
    { response: ResponseText }

type BotError =
    | ValidationError of string
    | Text of string
    | ParseError of string


type BotLogic = BotRequest -> JS.Promise<Result<BotResponse, BotError>>
