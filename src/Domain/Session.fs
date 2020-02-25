module Domain.Session

open Domain.Parse
open Domain.Bot
open Fable.Core

type Session =
    { torrents: ResizeArray<TorrentTableEntity>
      prev: Option<string>
      next: Option<string>
      userId: int
      messageId: int
      currentPosition: int }

let toSession (parseResult: ParseResult) (chatId: ChatId) (messageId: int): Session =
    { torrents = parseResult.torrents
      prev = parseResult.prev
      next = parseResult.next
      userId = unwrapChatId chatId
      messageId = messageId
      currentPosition = 0 }

type SaveSession = Session -> JS.Promise<Session>

type GetSession = ChatId -> JS.Promise<Session>

type UserInput = {
    text: string
    date: JS.Date
}
type History = {
    userId: int
    records: UserInput array
}

type GetHistory = ChatId -> JS.Promise<History>

