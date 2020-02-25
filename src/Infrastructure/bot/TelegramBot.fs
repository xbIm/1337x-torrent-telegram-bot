module TelegramBot

open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Core.JS

[<StringEnum>]
[<RequireQualifiedAccess>]
type ChatType =
    | Private
    | Group
    | Supergroup
    | Channel

[<StringEnum>]
[<RequireQualifiedAccess>]
type ChatAction =
    | Typing
    | Upload_photo
    | Record_video
    | Upload_video
    | Record_audio
    | Upload_audio
    | Upload_document
    | Find_location
    | Record_video_note
    | Upload_video_note

[<StringEnum>]
[<RequireQualifiedAccess>]
type ChatMemberStatus =
    | Creator
    | Administrator
    | Member
    | Restricted
    | Left
    | Kicked

//type [<StringEnum>] [<RequireQualifiedAccess>] DocumentMimeType =
//    | Application/pdf
//    | Application/zip

[<StringEnum>]
[<RequireQualifiedAccess>]
type MessageType =
    | Text
    | Animation
    | Audio
    | Channel_chat_created
    | Contact
    | Delete_chat_photo
    | Document
    | Game
    | Group_chat_created
    | Invoice
    | Left_chat_member
    | Location
    | Migrate_from_chat_id
    | Migrate_to_chat_id
    | New_chat_members
    | New_chat_photo
    | New_chat_title
    | Passport_data
    | Photo
    | Pinned_message
    | Sticker
    | Successful_payment
    | Supergroup_chat_created
    | Video
    | Video_note
    | Voice

[<StringEnum>]
[<RequireQualifiedAccess>]
type MessageEntityType =
    | Mention
    | Hashtag
    | Bot_command
    | Url
    | Email
    | Bold
    | Italic
    | Code
    | Pre
    | Text_link
    | Text_mention

[<StringEnum>]
[<RequireQualifiedAccess>]
type ParseMode =
    | [<CompiledName "Markdown">] Markdown
    | [<CompiledName "HTML">] HTML


//[<AllowNullLiteral>]
//type KeyboardButton =
//    abstract text: string with get, set
//    abstract request_contact: bool option with get, set
//    abstract request_location: bool option with get, set

type KeyboardButton =
    { text: string }

type ReplyKeyboardMarkup =
    { keyboard: KeyboardButton [] list
      //resize_keyboard: bool option;
      one_time_keyboard: bool option
      selective: bool option }

type ReplyKeyboardRemove =
    { remove_keyboard: bool option
      selective: bool option }

type InlineKeyboardButton =
    { text: string
      callback_data: string option }

type InlineKeyboardMarkup =
    { inline_keyboard: InlineKeyboardButton [] list }
//[<AllowNullLiteral>]
//type InlineKeyboardButton =
//    abstract text: string with get, set
//    abstract url: string option with get, set
//    // abstract login_url: LoginUrl option with get, set
//    abstract callback_data: string option with get, set
//    abstract switch_inline_query: string option with get, set
//    abstract switch_inline_query_current_chat: string option with get, set
//    // abstract callback_game: CallbackGame option with get, set
//    abstract pay: bool option with get, set



[<AllowNullLiteral>]
type ForceReply =
    abstract force_reply: bool with get, set
    abstract selective: bool option with get, set

[<AllowNullLiteral>]
type SendBasicOptions =
    abstract disable_notification: bool option with get, set
    abstract reply_to_message_id: float option with get, set
    abstract reply_markup: U4<InlineKeyboardMarkup, ReplyKeyboardMarkup, ReplyKeyboardRemove, ForceReply> option with get, set

//[<AllowNullLiteral>]
//type SendMessageOptions =
//    inherit SendBasicOptions
//    abstract parse_mode: ParseMode option with get, set
//    abstract disable_web_page_preview: bool option with get, set

type SendMessageOptions =
    { parse_mode: ParseMode option
      reply_markup: U4<InlineKeyboardMarkup, ReplyKeyboardMarkup, ReplyKeyboardRemove, ForceReply> option }

[<AllowNullLiteral>]
type WebHookOptions() =
    member val host: string option = None with get, set
    member val port: float option = None with get, set
    member val key: string option = None with get, set
    member val cert: string option = None with get, set
    member val pfx: string option = None with get, set
    member val autoOpen: bool option = None with get, set
    // abstract https: ServerOptions option with get, set
    member val healthEndpoint: string option = None with get, set

[<AllowNullLiteral>]
type SetWebHookOptions =
    abstract url: string option with get, set
    // abstract certificate: U2<string, Stream> option with get, set
    abstract max_connections: float option with get, set
    abstract allowed_updates: ResizeArray<string> option with get, set


[<AllowNullLiteral>]
type User =
    abstract id: int with get, set
    abstract is_bot: bool with get, set
    abstract first_name: string with get, set
    abstract last_name: string option with get, set
    abstract username: string option with get, set
    abstract language_code: string option with get, set

[<AllowNullLiteral>]
type MessageEntity =
    abstract ``type``: MessageEntityType with get, set
    abstract offset: float with get, set
    abstract length: float with get, set
    abstract url: string option with get, set
    abstract user: User option with get, set

[<AllowNullLiteral>]
type Chat =
    abstract id: int with get, set
    abstract ``type``: ChatType with get, set
    abstract title: string option with get, set
    abstract username: string option with get, set
    abstract first_name: string option with get, set
    abstract last_name: string option with get, set
    // abstract photo: ChatPhoto option with get, set
    abstract description: string option with get, set
    abstract invite_link: string option with get, set
    //abstract pinned_message: Message option with get, set
    //abstract permissions: ChatPermissions option with get, set
    abstract can_set_sticker_set: bool option with get, set
    abstract sticker_set_name: string option with get, set
    abstract all_members_are_administrators: bool option with get, set

[<AllowNullLiteral>]
type Message =
    abstract message_id: int with get, set
    abstract from: User option with get, set
    abstract date: float with get, set
    abstract chat: Chat with get, set
    abstract forward_from: User option with get, set
    abstract forward_from_chat: Chat option with get, set
    abstract forward_from_message_id: float option with get, set
    abstract forward_signature: string option with get, set
    abstract forward_sender_name: string option with get, set
    abstract forward_date: float option with get, set
    abstract reply_to_message: Message option with get, set
    abstract edit_date: float option with get, set
    abstract media_group_id: string option with get, set
    abstract author_signature: string option with get, set
    abstract text: string option with get, set
    abstract entities: ResizeArray<MessageEntity> option with get, set
    abstract caption_entities: ResizeArray<MessageEntity> option with get, set
    // abstract audio: Audio option with get, set
    //abstract document: Document option with get, set
    //abstract animation: Animation option with get, set
    //abstract game: Game option with get, set
    //abstract photo: ResizeArray<PhotoSize> option with get, set
    //abstract sticker: Sticker option with get, set
    //abstract video: Video option with get, set
    //abstract voice: Voice option with get, set
    //abstract video_note: VideoNote option with get, set
    abstract caption: string option with get, set
    //abstract contact: Contact option with get, set
    //abstract location: Location option with get, set
    //abstract venue: Venue option with get, set
    //abstract poll: Poll option with get, set
    abstract new_chat_members: ResizeArray<User> option with get, set
    abstract left_chat_member: User option with get, set
    abstract new_chat_title: string option with get, set
    //abstract new_chat_photo: ResizeArray<PhotoSize> option with get, set
    abstract delete_chat_photo: bool option with get, set
    abstract group_chat_created: bool option with get, set
    abstract supergroup_chat_created: bool option with get, set
    abstract channel_chat_created: bool option with get, set
    abstract migrate_to_chat_id: float option with get, set
    abstract migrate_from_chat_id: float option with get, set
    abstract pinned_message: Message option with get, set
    //abstract invoice: Invoice option with get, set
    //abstract successful_payment: SuccessfulPayment option with get, set
    abstract connected_website: string option with get, set
//abstract reply_markup: InlineKeyboardMarkup option with get, set

[<AllowNullLiteral>]
type CallbackQuery =
    abstract id: string with get, set
    abstract from: User with get, set
    abstract message: Message option with get, set
    abstract inline_message_id: string option with get, set
    abstract chat_instance: string with get, set
    abstract data: string option with get, set
    abstract game_short_name: string option with get, set

type EditMessageTextOptions =
    { chat_id: int
      message_id: int
      reply_markup: InlineKeyboardMarkup option }
//[<AllowNullLiteral>]
//type ConstructorOptions() =
//  member val polling: bool option = None with get, set
//member val webHook: bool option = None with get, set
//member val onlyFirstMatch: bool option = None with get, set
//member val baseApiUrl: string option = None with get, set
//member val filepath: string option = None with get, set

type ConstructorOptions =
    { polling: bool option
      onlyFirstMatch: bool option }


[<AllowNullLiteral>]
type TelegramBot =
    abstract setWebHook: url:string * ?options:SetWebHookOptions -> Promise<obj option>
    abstract processUpdate: update:obj -> unit
    abstract onText: regexp:Regex * callback:(Message -> string array option -> unit) -> unit
    abstract sendMessage: chatId:U2<int, string> * text:string * ?options:SendMessageOptions -> Promise<Message>
    [<Emit "$0.on('callback_query',$1)">]
    abstract on_callback_query: listener:(CallbackQuery -> unit) -> unit
    abstract editMessageText: text:string * options:EditMessageTextOptions -> Promise<Message>


[<AllowNullLiteral>]
type TelegramBotStatic =
    [<Emit "new $0($1...)">]
    abstract Create: token:string * ConstructorOptions -> TelegramBot

[<Import("default", from = "node-telegram-bot-api")>]
let telegramBot: TelegramBotStatic = jsNative

let Create(token: string) =
    telegramBot.Create
        (token,
         { polling = Some false
           onlyFirstMatch = Some true })
