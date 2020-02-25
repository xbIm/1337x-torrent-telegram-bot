module Domain.Parse

open Domain.Bot

type TorrentTableEntity =
    { id: int
      url: Option<string>
      title: string
      size: string
      seaders: int
      leachers: int
      time: string
      author: string }

type ParseResult =
    { torrents: ResizeArray<TorrentTableEntity>
      prev: Option<string>
      next: Option<string> }

type Parse = string -> int -> Result<ParseResult, BotError>
