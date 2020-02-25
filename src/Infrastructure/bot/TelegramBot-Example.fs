// ts2fable 0.6.2
module rec TBot
open System
open Fable.Core
open Fable.Import.JS

type EventEmitter = Events.EventEmitter
type Stream = Stream.Stream
type Readable = Stream.Readable
type ServerOptions = Https.ServerOptions
type Options = Request.Options
let [<Import("*","module")>] telegramBot: TelegramBot.IExports = jsNative

type [<AllowNullLiteral>] IExports =
    abstract TelegramBot: TelegramBotStatic

module TelegramBot =

    type [<AllowNullLiteral>] TextListener =
        abstract regexp: RegExp with get, set
        abstract callback: msg: Message * ``match``: RegExpExecArray option -> unit

    type [<AllowNullLiteral>] ReplyListener =
        abstract id: float with get, set
        abstract chatId: U2<float, string> with get, set
        abstract messageId: U2<float, string> with get, set
        abstract callback: msg: Message -> unit

    type [<StringEnum>] [<RequireQualifiedAccess>] ChatType =
        | Private
        | Group
        | Supergroup
        | Channel

    type [<StringEnum>] [<RequireQualifiedAccess>] ChatAction =
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

    type [<StringEnum>] [<RequireQualifiedAccess>] ChatMemberStatus =
        | Creator
        | Administrator
        | Member
        | Restricted
        | Left
        | Kicked

    type [<StringEnum>] [<RequireQualifiedAccess>] DocumentMimeType =
        | Application/pdf
        | Application/zip

    type [<StringEnum>] [<RequireQualifiedAccess>] MessageType =
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

    type [<StringEnum>] [<RequireQualifiedAccess>] MessageEntityType =
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

    type [<StringEnum>] [<RequireQualifiedAccess>] ParseMode =
        | [<CompiledName "Markdown">] Markdown
        | [<CompiledName "HTML">] HTML

    type [<AllowNullLiteral>] PollingOptions =
        abstract interval: U2<string, float> option with get, set
        abstract autoStart: bool option with get, set
        abstract ``params``: GetUpdatesOptions option with get, set

    type [<AllowNullLiteral>] WebHookOptions =
        abstract host: string option with get, set
        abstract port: float option with get, set
        abstract key: string option with get, set
        abstract cert: string option with get, set
        abstract pfx: string option with get, set
        abstract autoOpen: bool option with get, set
        abstract https: ServerOptions option with get, set
        abstract healthEndpoint: string option with get, set

    type [<AllowNullLiteral>] ConstructorOptions =
        abstract polling: bool option with get, set
        abstract webHook: U2<bool, WebHookOptions> option with get, set
        abstract onlyFirstMatch: bool option with get, set
        abstract request: Options option with get, set
        abstract baseApiUrl: string option with get, set
        abstract filepath: bool option with get, set

    type [<AllowNullLiteral>] StartPollingOptions =
        inherit ConstructorOptions
        abstract restart: bool option with get, set

    type [<AllowNullLiteral>] StopPollingOptions =
        abstract cancel: bool option with get, set
        abstract reason: string option with get, set

    type [<AllowNullLiteral>] SetWebHookOptions =
        abstract url: string option with get, set
        abstract certificate: U2<string, Stream> option with get, set
        abstract max_connections: float option with get, set
        abstract allowed_updates: ResizeArray<string> option with get, set

    type [<AllowNullLiteral>] GetUpdatesOptions =
        abstract offset: float option with get, set
        abstract limit: float option with get, set
        abstract timeout: float option with get, set
        abstract allowed_updates: ResizeArray<string> option with get, set

    type [<AllowNullLiteral>] SendBasicOptions =
        abstract disable_notification: bool option with get, set
        abstract reply_to_message_id: float option with get, set
        abstract reply_markup: U4<InlineKeyboardMarkup, ReplyKeyboardMarkup, ReplyKeyboardRemove, ForceReply> option with get, set

    type [<AllowNullLiteral>] SendMessageOptions =
        inherit SendBasicOptions
        abstract parse_mode: ParseMode option with get, set
        abstract disable_web_page_preview: bool option with get, set

    type [<AllowNullLiteral>] AnswerInlineQueryOptions =
        abstract cache_time: float option with get, set
        abstract is_personal: bool option with get, set
        abstract next_offset: string option with get, set
        abstract switch_pm_text: string option with get, set
        abstract switch_pm_parameter: string option with get, set

    type [<AllowNullLiteral>] ForwardMessageOptions =
        abstract disable_notification: bool option with get, set

    type [<AllowNullLiteral>] SendPhotoOptions =
        inherit SendBasicOptions
        abstract caption: string option with get, set

    type [<AllowNullLiteral>] SendAudioOptions =
        inherit SendBasicOptions
        abstract caption: string option with get, set
        abstract duration: float option with get, set
        abstract performer: string option with get, set
        abstract title: string option with get, set

    type [<AllowNullLiteral>] SendDocumentOptions =
        inherit SendBasicOptions
        abstract caption: string option with get, set

    type [<AllowNullLiteral>] SendMediaGroupOptions =
        abstract disable_notification: bool option with get, set
        abstract reply_to_message_id: float option with get, set

    type SendStickerOptions =
        SendBasicOptions

    type [<AllowNullLiteral>] SendVideoOptions =
        inherit SendBasicOptions
        abstract duration: float option with get, set
        abstract width: float option with get, set
        abstract height: float option with get, set
        abstract caption: string option with get, set

    type [<AllowNullLiteral>] SendVoiceOptions =
        inherit SendBasicOptions
        abstract caption: string option with get, set
        abstract duration: float option with get, set

    type [<AllowNullLiteral>] SendVideoNoteOptions =
        inherit SendBasicOptions
        abstract duration: float option with get, set
        abstract length: float option with get, set

    type SendLocationOptions =
        SendBasicOptions

    type EditMessageLiveLocationOptions =
        EditMessageCaptionOptions

    type StopMessageLiveLocationOptions =
        EditMessageCaptionOptions

    type [<AllowNullLiteral>] SendVenueOptions =
        inherit SendBasicOptions
        abstract foursquare_id: string option with get, set

    type [<AllowNullLiteral>] SendContactOptions =
        inherit SendBasicOptions
        abstract last_name: string option with get, set

    type SendGameOptions =
        SendBasicOptions

    type [<AllowNullLiteral>] SendInvoiceOptions =
        inherit SendBasicOptions
        abstract provider_data: string option with get, set
        abstract photo_url: string option with get, set
        abstract photo_size: float option with get, set
        abstract photo_width: float option with get, set
        abstract photo_height: float option with get, set
        abstract need_name: bool option with get, set
        abstract need_phone_number: bool option with get, set
        abstract need_email: bool option with get, set
        abstract need_shipping_address: bool option with get, set
        abstract is_flexible: bool option with get, set

    type [<AllowNullLiteral>] RestrictChatMemberOptions =
        abstract until_date: float option with get, set
        abstract can_send_messages: bool option with get, set
        abstract can_send_media_messages: bool option with get, set
        abstract can_send_other_messages: bool option with get, set
        abstract can_add_web_page_previews: bool option with get, set

    type [<AllowNullLiteral>] PromoteChatMemberOptions =
        abstract can_change_info: bool option with get, set
        abstract can_post_messages: bool option with get, set
        abstract can_edit_messages: bool option with get, set
        abstract can_delete_messages: bool option with get, set
        abstract can_invite_users: bool option with get, set
        abstract can_restrict_members: bool option with get, set
        abstract can_pin_messages: bool option with get, set
        abstract can_promote_members: bool option with get, set

    type [<AllowNullLiteral>] AnswerCallbackQueryOptions =
        abstract callback_query_id: string with get, set
        abstract text: string option with get, set
        abstract show_alert: bool option with get, set
        abstract url: string option with get, set
        abstract cache_time: float option with get, set

    type [<AllowNullLiteral>] EditMessageTextOptions =
        inherit EditMessageCaptionOptions
        abstract parse_mode: ParseMode option with get, set
        abstract disable_web_page_preview: bool option with get, set

    type [<AllowNullLiteral>] EditMessageCaptionOptions =
        inherit EditMessageReplyMarkupOptions
        abstract reply_markup: InlineKeyboardMarkup option with get, set

    type [<AllowNullLiteral>] EditMessageReplyMarkupOptions =
        abstract chat_id: U2<float, string> option with get, set
        abstract message_id: float option with get, set
        abstract inline_message_id: string option with get, set

    type [<AllowNullLiteral>] GetUserProfilePhotosOptions =
        abstract offset: float option with get, set
        abstract limit: float option with get, set

    type [<AllowNullLiteral>] SetGameScoreOptions =
        abstract force: bool option with get, set
        abstract disable_edit_message: bool option with get, set
        abstract chat_id: float option with get, set
        abstract message_id: float option with get, set
        abstract inline_message_id: string option with get, set

    type [<AllowNullLiteral>] GetGameHighScoresOptions =
        abstract chat_id: float option with get, set
        abstract message_id: float option with get, set
        abstract inline_message_id: string option with get, set

    type [<AllowNullLiteral>] AnswerShippingQueryOptions =
        abstract shipping_options: ResizeArray<ShippingOption> option with get, set
        abstract error_message: string option with get, set

    type [<AllowNullLiteral>] AnswerPreCheckoutQueryOptions =
        abstract error_message: string option with get, set

    type [<AllowNullLiteral>] Update =
        abstract update_id: float with get, set
        abstract message: Message option with get, set
        abstract edited_message: Message option with get, set
        abstract channel_post: Message option with get, set
        abstract edited_channel_post: Message option with get, set
        abstract inline_query: InlineQuery option with get, set
        abstract chosen_inline_result: ChosenInlineResult option with get, set
        abstract callback_query: CallbackQuery option with get, set
        abstract shipping_query: ShippingQuery option with get, set
        abstract pre_checkout_query: PreCheckoutQuery option with get, set

    type [<AllowNullLiteral>] WebhookInfo =
        abstract url: string with get, set
        abstract has_custom_certificate: bool with get, set
        abstract pending_update_count: float with get, set
        abstract last_error_date: float option with get, set
        abstract last_error_message: string option with get, set
        abstract max_connections: float option with get, set
        abstract allowed_updates: ResizeArray<string> option with get, set

    type [<AllowNullLiteral>] User =
        abstract id: float with get, set
        abstract is_bot: bool with get, set
        abstract first_name: string with get, set
        abstract last_name: string option with get, set
        abstract username: string option with get, set
        abstract language_code: string option with get, set

    type [<AllowNullLiteral>] Chat =
        abstract id: float with get, set
        abstract ``type``: ChatType with get, set
        abstract title: string option with get, set
        abstract username: string option with get, set
        abstract first_name: string option with get, set
        abstract last_name: string option with get, set
        abstract photo: ChatPhoto option with get, set
        abstract description: string option with get, set
        abstract invite_link: string option with get, set
        abstract pinned_message: Message option with get, set
        abstract permissions: ChatPermissions option with get, set
        abstract can_set_sticker_set: bool option with get, set
        abstract sticker_set_name: string option with get, set
        abstract all_members_are_administrators: bool option with get, set

    type [<AllowNullLiteral>] Message =
        abstract message_id: float with get, set
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
        abstract audio: Audio option with get, set
        abstract document: Document option with get, set
        abstract animation: Animation option with get, set
        abstract game: Game option with get, set
        abstract photo: ResizeArray<PhotoSize> option with get, set
        abstract sticker: Sticker option with get, set
        abstract video: Video option with get, set
        abstract voice: Voice option with get, set
        abstract video_note: VideoNote option with get, set
        abstract caption: string option with get, set
        abstract contact: Contact option with get, set
        abstract location: Location option with get, set
        abstract venue: Venue option with get, set
        abstract poll: Poll option with get, set
        abstract new_chat_members: ResizeArray<User> option with get, set
        abstract left_chat_member: User option with get, set
        abstract new_chat_title: string option with get, set
        abstract new_chat_photo: ResizeArray<PhotoSize> option with get, set
        abstract delete_chat_photo: bool option with get, set
        abstract group_chat_created: bool option with get, set
        abstract supergroup_chat_created: bool option with get, set
        abstract channel_chat_created: bool option with get, set
        abstract migrate_to_chat_id: float option with get, set
        abstract migrate_from_chat_id: float option with get, set
        abstract pinned_message: Message option with get, set
        abstract invoice: Invoice option with get, set
        abstract successful_payment: SuccessfulPayment option with get, set
        abstract connected_website: string option with get, set
        abstract reply_markup: InlineKeyboardMarkup option with get, set

    type [<AllowNullLiteral>] MessageEntity =
        abstract ``type``: MessageEntityType with get, set
        abstract offset: float with get, set
        abstract length: float with get, set
        abstract url: string option with get, set
        abstract user: User option with get, set

    type [<AllowNullLiteral>] FileBase =
        abstract file_id: string with get, set
        abstract file_size: float option with get, set

    type [<AllowNullLiteral>] PhotoSize =
        inherit FileBase
        abstract width: float with get, set
        abstract height: float with get, set

    type [<AllowNullLiteral>] Audio =
        inherit FileBase
        abstract duration: float with get, set
        abstract performer: string option with get, set
        abstract title: string option with get, set
        abstract mime_type: string option with get, set
        abstract thumb: PhotoSize option with get, set

    type [<AllowNullLiteral>] Document =
        inherit FileBase
        abstract thumb: PhotoSize option with get, set
        abstract file_name: string option with get, set
        abstract mime_type: string option with get, set

    type [<AllowNullLiteral>] Video =
        inherit FileBase
        abstract width: float with get, set
        abstract height: float with get, set
        abstract duration: float with get, set
        abstract thumb: PhotoSize option with get, set
        abstract mime_type: string option with get, set

    type [<AllowNullLiteral>] Voice =
        inherit FileBase
        abstract duration: float with get, set
        abstract mime_type: string option with get, set

    type [<AllowNullLiteral>] InputMediaBase =
        abstract media: string with get, set
        abstract caption: string option with get, set
        abstract parse_mode: ParseMode option with get, set

    type [<AllowNullLiteral>] InputMediaPhoto =
        inherit InputMediaBase
        abstract ``type``: string with get, set

    type [<AllowNullLiteral>] InputMediaVideo =
        inherit InputMediaBase
        abstract ``type``: string with get, set
        abstract width: float option with get, set
        abstract height: float option with get, set
        abstract duration: float option with get, set
        abstract supports_streaming: bool option with get, set

    type InputMedia =
        U2<InputMediaPhoto, InputMediaVideo>

    [<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module InputMedia =
        let ofInputMediaPhoto v: InputMedia = v |> U2.Case1
        let isInputMediaPhoto (v: InputMedia) = match v with U2.Case1 _ -> true | _ -> false
        let asInputMediaPhoto (v: InputMedia) = match v with U2.Case1 o -> Some o | _ -> None
        let ofInputMediaVideo v: InputMedia = v |> U2.Case2
        let isInputMediaVideo (v: InputMedia) = match v with U2.Case2 _ -> true | _ -> false
        let asInputMediaVideo (v: InputMedia) = match v with U2.Case2 o -> Some o | _ -> None

    type [<AllowNullLiteral>] VideoNote =
        inherit FileBase
        abstract length: float with get, set
        abstract duration: float with get, set
        abstract thumb: PhotoSize option with get, set

    type [<AllowNullLiteral>] Contact =
        abstract phone_number: string with get, set
        abstract first_name: string with get, set
        abstract last_name: string option with get, set
        abstract user_id: float option with get, set
        abstract vcard: string option with get, set

    type [<AllowNullLiteral>] Location =
        abstract longitude: float with get, set
        abstract latitude: float with get, set

    type [<AllowNullLiteral>] Venue =
        abstract location: Location with get, set
        abstract title: string with get, set
        abstract address: string with get, set
        abstract foursquare_id: string option with get, set
        abstract foursquare_type: string option with get, set

    type [<AllowNullLiteral>] PollOption =
        abstract text: string with get, set
        abstract voter_count: float with get, set

    type [<AllowNullLiteral>] Poll =
        abstract id: string with get, set
        abstract question: string with get, set
        abstract options: ResizeArray<PollOption> with get, set
        abstract is_closed: bool with get, set

    type [<AllowNullLiteral>] UserProfilePhotos =
        abstract total_count: float with get, set
        abstract photos: ResizeArray<ResizeArray<PhotoSize>> with get, set

    type [<AllowNullLiteral>] File =
        inherit FileBase
        abstract file_path: string option with get, set

    type [<AllowNullLiteral>] ReplyKeyboardMarkup =
        abstract keyboard: ResizeArray<ResizeArray<KeyboardButton>> with get, set
        abstract resize_keyboard: bool option with get, set
        abstract one_time_keyboard: bool option with get, set
        abstract selective: bool option with get, set

    type [<AllowNullLiteral>] KeyboardButton =
        abstract text: string with get, set
        abstract request_contact: bool option with get, set
        abstract request_location: bool option with get, set

    type [<AllowNullLiteral>] ReplyKeyboardRemove =
        abstract remove_keyboard: bool with get, set
        abstract selective: bool option with get, set

    type [<AllowNullLiteral>] InlineKeyboardMarkup =
        abstract inline_keyboard: ResizeArray<ResizeArray<InlineKeyboardButton>> with get, set

    type [<AllowNullLiteral>] InlineKeyboardButton =
        abstract text: string with get, set
        abstract url: string option with get, set
        abstract login_url: LoginUrl option with get, set
        abstract callback_data: string option with get, set
        abstract switch_inline_query: string option with get, set
        abstract switch_inline_query_current_chat: string option with get, set
        abstract callback_game: CallbackGame option with get, set
        abstract pay: bool option with get, set

    type [<AllowNullLiteral>] LoginUrl =
        abstract url: string with get, set
        abstract forward_text: string option with get, set
        abstract bot_username: string option with get, set
        abstract request_write_acces: bool option with get, set

    type [<AllowNullLiteral>] CallbackQuery =
        abstract id: string with get, set
        abstract from: User with get, set
        abstract message: Message option with get, set
        abstract inline_message_id: string option with get, set
        abstract chat_instance: string with get, set
        abstract data: string option with get, set
        abstract game_short_name: string option with get, set

    type [<AllowNullLiteral>] ForceReply =
        abstract force_reply: bool with get, set
        abstract selective: bool option with get, set

    type [<AllowNullLiteral>] ChatPhoto =
        abstract small_file_id: string with get, set
        abstract big_file_id: string with get, set

    type [<AllowNullLiteral>] ChatMember =
        abstract user: User with get, set
        abstract status: ChatMemberStatus with get, set
        abstract until_date: float option with get, set
        abstract can_be_edited: bool option with get, set
        abstract can_post_messages: bool option with get, set
        abstract can_edit_messages: bool option with get, set
        abstract can_delete_messages: bool option with get, set
        abstract can_restrict_members: bool option with get, set
        abstract can_promote_members: bool option with get, set
        abstract can_change_info: bool option with get, set
        abstract can_invite_users: bool option with get, set
        abstract can_pin_messages: bool option with get, set
        abstract is_member: bool option with get, set
        abstract can_send_messages: bool option with get, set
        abstract can_send_media_messages: bool option with get, set
        abstract can_send_polls: bool with get, set
        abstract can_send_other_messages: bool option with get, set
        abstract can_add_web_page_previews: bool option with get, set

    type [<AllowNullLiteral>] ChatPermissions =
        abstract can_send_messages: bool option with get, set
        abstract can_send_media_messages: bool option with get, set
        abstract can_send_polls: bool option with get, set
        abstract can_send_other_messages: bool option with get, set
        abstract can_add_web_page_previews: bool option with get, set
        abstract can_change_info: bool option with get, set
        abstract can_invite_users: bool option with get, set
        abstract can_pin_messages: bool option with get, set

    type [<AllowNullLiteral>] Sticker =
        abstract file_id: string with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract thumb: PhotoSize option with get, set
        abstract emoji: string option with get, set
        abstract set_name: string option with get, set
        abstract mask_position: MaskPosition option with get, set
        abstract file_size: float option with get, set

    type [<AllowNullLiteral>] StickerSet =
        abstract name: string with get, set
        abstract title: string with get, set
        abstract contains_masks: bool with get, set
        abstract stickers: ResizeArray<Sticker> with get, set

    type [<AllowNullLiteral>] MaskPosition =
        abstract point: string with get, set
        abstract x_shift: float with get, set
        abstract y_shift: float with get, set
        abstract scale: float with get, set

    type [<AllowNullLiteral>] InlineQuery =
        abstract id: string with get, set
        abstract from: User with get, set
        abstract location: Location option with get, set
        abstract query: string with get, set
        abstract offset: string with get, set

    type [<AllowNullLiteral>] InlineQueryResultBase =
        abstract id: string with get, set
        abstract reply_markup: InlineKeyboardMarkup option with get, set

    type [<AllowNullLiteral>] InlineQueryResultArticle =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract title: string with get, set
        abstract input_message_content: InputMessageContent with get, set
        abstract url: string option with get, set
        abstract hide_url: bool option with get, set
        abstract description: string option with get, set
        abstract thumb_url: string option with get, set
        abstract thumb_width: float option with get, set
        abstract thumb_height: float option with get, set

    type [<AllowNullLiteral>] InlineQueryResultPhoto =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract photo_url: string with get, set
        abstract thumb_url: string with get, set
        abstract photo_width: float option with get, set
        abstract photo_height: float option with get, set
        abstract title: string option with get, set
        abstract description: string option with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultGif =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract gif_url: string with get, set
        abstract gif_width: float option with get, set
        abstract gif_height: float option with get, set
        abstract gif_duration: float option with get, set
        abstract thumb_url: string option with get, set
        abstract title: string option with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultMpeg4Gif =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract mpeg4_url: string with get, set
        abstract mpeg4_width: float option with get, set
        abstract mpeg4_height: float option with get, set
        abstract mpeg4_duration: float option with get, set
        abstract thumb_url: string option with get, set
        abstract title: string option with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultVideo =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract video_url: string with get, set
        abstract mime_type: string with get, set
        abstract thumb_url: string with get, set
        abstract title: string with get, set
        abstract caption: string option with get, set
        abstract video_width: float option with get, set
        abstract video_height: float option with get, set
        abstract video_duration: float option with get, set
        abstract description: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultAudio =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract audio_url: string with get, set
        abstract title: string with get, set
        abstract caption: string option with get, set
        abstract performer: string option with get, set
        abstract audio_duration: float option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultVoice =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract voice_url: string with get, set
        abstract title: string with get, set
        abstract caption: string option with get, set
        abstract voice_duration: float option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultDocument =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract title: string with get, set
        abstract caption: string option with get, set
        abstract document_url: string with get, set
        abstract mime_type: string with get, set
        abstract description: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set
        abstract thumb_url: string option with get, set
        abstract thumb_width: float option with get, set
        abstract thumb_height: float option with get, set

    type [<AllowNullLiteral>] InlineQueryResultLocationBase =
        inherit InlineQueryResultBase
        abstract latitude: float with get, set
        abstract longitude: float with get, set
        abstract title: string with get, set
        abstract input_message_content: InputMessageContent option with get, set
        abstract thumb_url: string option with get, set
        abstract thumb_width: float option with get, set
        abstract thumb_height: float option with get, set

    type [<AllowNullLiteral>] InlineQueryResultLocation =
        inherit InlineQueryResultLocationBase
        abstract ``type``: string with get, set

    type [<AllowNullLiteral>] InlineQueryResultVenue =
        inherit InlineQueryResultLocationBase
        abstract ``type``: string with get, set
        abstract address: string with get, set
        abstract foursquare_id: string option with get, set

    type [<AllowNullLiteral>] InlineQueryResultContact =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract phone_number: string with get, set
        abstract first_name: string with get, set
        abstract last_name: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set
        abstract thumb_url: string option with get, set
        abstract thumb_width: float option with get, set
        abstract thumb_height: float option with get, set

    type [<AllowNullLiteral>] InlineQueryResultGame =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract game_short_name: string with get, set

    type [<AllowNullLiteral>] InlineQueryResultCachedPhoto =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract photo_file_id: string with get, set
        abstract title: string option with get, set
        abstract description: string option with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultCachedGif =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract gif_file_id: string with get, set
        abstract title: string option with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultCachedMpeg4Gif =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract mpeg4_file_id: string with get, set
        abstract title: string option with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultCachedSticker =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract sticker_file_id: string with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultCachedDocument =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract title: string with get, set
        abstract document_file_id: string with get, set
        abstract description: string option with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultCachedVideo =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract video_file_id: string with get, set
        abstract title: string with get, set
        abstract description: string option with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultCachedVoice =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract voice_file_id: string with get, set
        abstract title: string with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type [<AllowNullLiteral>] InlineQueryResultCachedAudio =
        inherit InlineQueryResultBase
        abstract ``type``: string with get, set
        abstract audio_file_id: string with get, set
        abstract caption: string option with get, set
        abstract input_message_content: InputMessageContent option with get, set

    type InlineQueryResult =
        obj

    type InputMessageContent =
        obj

    type [<AllowNullLiteral>] InputTextMessageContent =
        inherit InputMessageContent
        abstract message_text: string with get, set
        abstract parse_mode: ParseMode option with get, set
        abstract disable_web_page_preview: bool option with get, set

    type [<AllowNullLiteral>] InputLocationMessageContent =
        inherit InputMessageContent
        abstract latitude: float with get, set
        abstract longitude: float with get, set

    type [<AllowNullLiteral>] InputVenueMessageContent =
        inherit InputLocationMessageContent
        abstract title: string with get, set
        abstract address: string with get, set
        abstract foursquare_id: string option with get, set

    type [<AllowNullLiteral>] InputContactMessageContent =
        inherit InputMessageContent
        abstract phone_number: string with get, set
        abstract first_name: string with get, set
        abstract last_name: string option with get, set

    type [<AllowNullLiteral>] ChosenInlineResult =
        abstract result_id: string with get, set
        abstract from: User with get, set
        abstract location: Location option with get, set
        abstract inline_message_id: string option with get, set
        abstract query: string with get, set

    type [<AllowNullLiteral>] ResponseParameters =
        abstract migrate_to_chat_id: float option with get, set
        abstract retry_after: float option with get, set

    type [<AllowNullLiteral>] LabeledPrice =
        abstract label: string with get, set
        abstract amount: float with get, set

    type [<AllowNullLiteral>] Invoice =
        abstract title: string with get, set
        abstract description: string with get, set
        abstract start_parameter: string with get, set
        abstract currency: string with get, set
        abstract total_amount: float with get, set

    type [<AllowNullLiteral>] ShippingAddress =
        abstract country_code: string with get, set
        abstract state: string with get, set
        abstract city: string with get, set
        abstract street_line1: string with get, set
        abstract street_line2: string with get, set
        abstract post_code: string with get, set

    type [<AllowNullLiteral>] OrderInfo =
        abstract name: string option with get, set
        abstract phone_number: string option with get, set
        abstract email: string option with get, set
        abstract shipping_address: ShippingAddress option with get, set

    type [<AllowNullLiteral>] ShippingOption =
        abstract id: string with get, set
        abstract title: string with get, set
        abstract prices: ResizeArray<LabeledPrice> with get, set

    type [<AllowNullLiteral>] SuccessfulPayment =
        abstract currency: string with get, set
        abstract total_amount: float with get, set
        abstract invoice_payload: string with get, set
        abstract shipping_option_id: string option with get, set
        abstract order_info: OrderInfo option with get, set
        abstract telegram_payment_charge_id: string with get, set
        abstract provider_payment_charge_id: string with get, set

    type [<AllowNullLiteral>] ShippingQuery =
        abstract id: string with get, set
        abstract from: User with get, set
        abstract invoice_payload: string with get, set
        abstract shipping_address: ShippingAddress with get, set

    type [<AllowNullLiteral>] PreCheckoutQuery =
        abstract id: string with get, set
        abstract from: User with get, set
        abstract currency: string with get, set
        abstract total_amount: float with get, set
        abstract invoice_payload: string with get, set
        abstract shipping_option_id: string option with get, set
        abstract order_info: OrderInfo option with get, set

    type [<AllowNullLiteral>] Game =
        abstract title: string with get, set
        abstract description: string with get, set
        abstract photo: ResizeArray<PhotoSize> with get, set
        abstract text: string option with get, set
        abstract text_entities: ResizeArray<MessageEntity> option with get, set
        abstract animation: Animation option with get, set

    type [<AllowNullLiteral>] Animation =
        inherit FileBase
        abstract width: float with get, set
        abstract height: float with get, set
        abstract duration: float with get, set
        abstract thumb: PhotoSize option with get, set
        abstract file_name: string option with get, set
        abstract mime_type: string option with get, set

    type CallbackGame =
        obj

    type [<AllowNullLiteral>] GameHighScore =
        abstract position: float with get, set
        abstract user: User with get, set
        abstract score: float with get, set

    type [<AllowNullLiteral>] Metadata =
        abstract ``type``: MessageType option with get, set

type [<AllowNullLiteral>] TelegramBot =
    inherit EventEmitter
    abstract startPolling: ?options: TelegramBot.StartPollingOptions -> Promise<obj option>
    abstract stopPolling: ?options: TelegramBot.StopPollingOptions -> Promise<obj option>
    abstract isPolling: unit -> bool
    abstract openWebHook: unit -> Promise<obj option>
    abstract closeWebHook: unit -> Promise<obj option>
    abstract hasOpenWebHook: unit -> bool
    abstract getMe: unit -> Promise<TelegramBot.User>
    abstract setWebHook: url: string * ?options: TelegramBot.SetWebHookOptions -> Promise<obj option>
    abstract deleteWebHook: unit -> Promise<bool>
    abstract getWebHookInfo: unit -> Promise<TelegramBot.WebhookInfo>
    abstract getUpdates: ?options: TelegramBot.Update -> Promise<ResizeArray<TelegramBot.Update>>
    abstract processUpdate: update: obj -> unit
    abstract sendMessage: chatId: U2<float, string> * text: string * ?options: TelegramBot.SendMessageOptions -> Promise<TelegramBot.Message>
    abstract answerInlineQuery: inlineQueryId: string * results: ResizeArray<TelegramBot.InlineQueryResult> * ?options: TelegramBot.AnswerInlineQueryOptions -> Promise<bool>
    abstract forwardMessage: chatId: U2<float, string> * fromChatId: U2<float, string> * messageId: U2<float, string> * ?options: TelegramBot.ForwardMessageOptions -> Promise<TelegramBot.Message>
    abstract sendPhoto: chatId: U2<float, string> * photo: U3<string, Stream, Buffer> * ?options: TelegramBot.SendPhotoOptions -> Promise<TelegramBot.Message>
    abstract sendAudio: chatId: U2<float, string> * audio: U3<string, Stream, Buffer> * ?options: TelegramBot.SendAudioOptions -> Promise<TelegramBot.Message>
    abstract sendDocument: chatId: U2<float, string> * doc: U3<string, Stream, Buffer> * ?options: TelegramBot.SendDocumentOptions * ?fileOpts: obj -> Promise<TelegramBot.Message>
    abstract sendMediaGroup: chatId: U2<float, string> * media: ResizeArray<TelegramBot.InputMedia> * ?options: TelegramBot.SendMediaGroupOptions -> Promise<TelegramBot.Message>
    abstract sendSticker: chatId: U2<float, string> * sticker: U3<string, Stream, Buffer> * ?options: TelegramBot.SendStickerOptions -> Promise<TelegramBot.Message>
    abstract sendVideo: chatId: U2<float, string> * video: U3<string, Stream, Buffer> * ?options: TelegramBot.SendVideoOptions -> Promise<TelegramBot.Message>
    abstract sendVideoNote: chatId: U2<float, string> * videoNote: U3<string, Stream, Buffer> * ?options: TelegramBot.SendVideoNoteOptions -> Promise<TelegramBot.Message>
    abstract sendVoice: chatId: U2<float, string> * voice: U3<string, Stream, Buffer> * ?options: TelegramBot.SendVoiceOptions -> Promise<TelegramBot.Message>
    abstract sendChatAction: chatId: U2<float, string> * action: TelegramBot.ChatAction -> Promise<bool>
    abstract kickChatMember: chatId: U2<float, string> * userId: string -> Promise<bool>
    abstract unbanChatMember: chatId: U2<float, string> * userId: string -> Promise<bool>
    abstract restrictChatMember: chatId: U2<float, string> * userId: string * ?options: TelegramBot.RestrictChatMemberOptions -> Promise<bool>
    abstract promoteChatMember: chatId: U2<float, string> * userId: string * ?options: TelegramBot.PromoteChatMemberOptions -> Promise<bool>
    abstract exportChatInviteLink: chatId: U2<float, string> -> Promise<string>
    abstract setChatPhoto: chatId: U2<float, string> * photo: U3<string, Stream, Buffer> -> Promise<bool>
    abstract deleteChatPhoto: chatId: U2<float, string> -> Promise<bool>
    abstract setChatTitle: chatId: U2<float, string> * title: string -> Promise<bool>
    abstract setChatDescription: chatId: U2<float, string> * description: string -> Promise<bool>
    abstract pinChatMessage: chatId: U2<float, string> * messageId: string -> Promise<bool>
    abstract unpinChatMessage: chatId: U2<float, string> -> Promise<bool>
    abstract answerCallbackQuery: callbackQueryId: string * ?options: obj -> Promise<bool>
    abstract answerCallbackQuery: ?options: TelegramBot.AnswerCallbackQueryOptions -> Promise<bool>
    abstract editMessageText: text: string * ?options: TelegramBot.EditMessageTextOptions -> Promise<U2<TelegramBot.Message, bool>>
    abstract editMessageCaption: caption: string * ?options: TelegramBot.EditMessageCaptionOptions -> Promise<U2<TelegramBot.Message, bool>>
    abstract editMessageReplyMarkup: replyMarkup: TelegramBot.InlineKeyboardMarkup * ?options: TelegramBot.EditMessageReplyMarkupOptions -> Promise<U2<TelegramBot.Message, bool>>
    abstract getUserProfilePhotos: userId: U2<float, string> * ?options: TelegramBot.GetUserProfilePhotosOptions -> Promise<TelegramBot.UserProfilePhotos>
    abstract sendLocation: chatId: U2<float, string> * latitude: float * longitude: float * ?options: TelegramBot.SendLocationOptions -> Promise<TelegramBot.Message>
    abstract editMessageLiveLocation: latitude: float * longitude: float * ?options: TelegramBot.EditMessageLiveLocationOptions -> Promise<U2<TelegramBot.Message, bool>>
    abstract stopMessageLiveLocation: ?options: TelegramBot.StopMessageLiveLocationOptions -> Promise<U2<TelegramBot.Message, bool>>
    abstract sendVenue: chatId: U2<float, string> * latitude: float * longitude: float * title: string * address: string * ?options: TelegramBot.SendVenueOptions -> Promise<TelegramBot.Message>
    abstract sendContact: chatId: U2<float, string> * phoneNumber: string * firstName: string * ?options: TelegramBot.SendContactOptions -> Promise<TelegramBot.Message>
    abstract getFile: fileId: string -> Promise<TelegramBot.File>
    abstract getFileLink: fileId: string -> Promise<string>
    abstract getFileStream: fileId: string -> Readable
    abstract downloadFile: fileId: string * downloadDir: string -> Promise<string>
    abstract onText: regexp: RegExp * callback: (TelegramBot.Message -> RegExpExecArray option -> unit) -> unit
    abstract removeTextListener: regexp: RegExp -> TelegramBot.TextListener option
    abstract onReplyToMessage: chatId: U2<float, string> * messageId: U2<float, string> * callback: (TelegramBot.Message -> unit) -> float
    abstract removeReplyListener: replyListenerId: float -> TelegramBot.ReplyListener
    abstract getChat: chatId: U2<float, string> -> Promise<TelegramBot.Chat>
    abstract getChatAdministrators: chatId: U2<float, string> -> Promise<ResizeArray<TelegramBot.ChatMember>>
    abstract getChatMembersCount: chatId: U2<float, string> -> Promise<float>
    abstract getChatMember: chatId: U2<float, string> * userId: string -> Promise<TelegramBot.ChatMember>
    abstract leaveChat: chatId: U2<float, string> -> Promise<bool>
    abstract setChatStickerSet: chatId: U2<float, string> * stickerSetName: string -> Promise<bool>
    abstract deleteChatStickerSet: chatId: U2<float, string> -> Promise<bool>
    abstract sendGame: chatId: U2<float, string> * gameShortName: string * ?options: TelegramBot.SendGameOptions -> Promise<TelegramBot.Message>
    abstract setGameScore: userId: string * score: float * ?options: TelegramBot.SetGameScoreOptions -> Promise<U2<TelegramBot.Message, bool>>
    abstract getGameHighScores: userId: string * ?options: TelegramBot.GetGameHighScoresOptions -> Promise<ResizeArray<TelegramBot.GameHighScore>>
    abstract deleteMessage: chatId: U2<float, string> * messageId: string * ?options: obj -> Promise<bool>
    abstract sendInvoice: chatId: U2<float, string> * title: string * description: string * payload: string * providerToken: string * startParameter: string * currency: string * prices: ResizeArray<TelegramBot.LabeledPrice> * ?options: TelegramBot.SendInvoiceOptions -> Promise<TelegramBot.Message>
    abstract answerShippingQuery: shippingQueryId: string * ok: bool * ?options: TelegramBot.AnswerShippingQueryOptions -> Promise<bool>
    abstract answerPreCheckoutQuery: preCheckoutQueryId: string * ok: bool * ?options: TelegramBot.AnswerPreCheckoutQueryOptions -> Promise<bool>
    abstract addListener: ``event``: U2<TelegramBot.MessageType, string> * listener: (TelegramBot.Message -> TelegramBot.Metadata -> unit) -> TelegramBot
    [<Emit "$0.addListener('callback_query',$1)">] abstract addListener_callback_query: listener: (TelegramBot.CallbackQuery -> unit) -> TelegramBot
    [<Emit "$0.addListener('inline_query',$1)">] abstract addListener_inline_query: listener: (TelegramBot.InlineQuery -> unit) -> TelegramBot
    [<Emit "$0.addListener('chosen_inline_result',$1)">] abstract addListener_chosen_inline_result: listener: (TelegramBot.ChosenInlineResult -> unit) -> TelegramBot
    abstract addListener: ``event``: U7<string, string, string, string, string, string, string> * listener: (TelegramBot.Message -> unit) -> TelegramBot
    [<Emit "$0.addListener('shipping_query',$1)">] abstract addListener_shipping_query: listener: (TelegramBot.ShippingQuery -> unit) -> TelegramBot
    [<Emit "$0.addListener('pre_checkout_query',$1)">] abstract addListener_pre_checkout_query: listener: (TelegramBot.PreCheckoutQuery -> unit) -> TelegramBot
    abstract addListener: ``event``: U3<string, string, string> * listener: (Error -> unit) -> TelegramBot
    abstract on: ``event``: U2<TelegramBot.MessageType, string> * listener: (TelegramBot.Message -> TelegramBot.Metadata -> unit) -> TelegramBot
    [<Emit "$0.on('callback_query',$1)">] abstract on_callback_query: listener: (TelegramBot.CallbackQuery -> unit) -> TelegramBot
    [<Emit "$0.on('inline_query',$1)">] abstract on_inline_query: listener: (TelegramBot.InlineQuery -> unit) -> TelegramBot
    [<Emit "$0.on('chosen_inline_result',$1)">] abstract on_chosen_inline_result: listener: (TelegramBot.ChosenInlineResult -> unit) -> TelegramBot
    abstract on: ``event``: U7<string, string, string, string, string, string, string> * listener: (TelegramBot.Message -> unit) -> TelegramBot
    [<Emit "$0.on('shipping_query',$1)">] abstract on_shipping_query: listener: (TelegramBot.ShippingQuery -> unit) -> TelegramBot
    [<Emit "$0.on('pre_checkout_query',$1)">] abstract on_pre_checkout_query: listener: (TelegramBot.PreCheckoutQuery -> unit) -> TelegramBot
    abstract on: ``event``: U3<string, string, string> * listener: (Error -> unit) -> TelegramBot
    abstract once: ``event``: U2<TelegramBot.MessageType, string> * listener: (TelegramBot.Message -> TelegramBot.Metadata -> unit) -> TelegramBot
    [<Emit "$0.once('callback_query',$1)">] abstract once_callback_query: listener: (TelegramBot.CallbackQuery -> unit) -> TelegramBot
    [<Emit "$0.once('inline_query',$1)">] abstract once_inline_query: listener: (TelegramBot.InlineQuery -> unit) -> TelegramBot
    [<Emit "$0.once('chosen_inline_result',$1)">] abstract once_chosen_inline_result: listener: (TelegramBot.ChosenInlineResult -> unit) -> TelegramBot
    abstract once: ``event``: U7<string, string, string, string, string, string, string> * listener: (TelegramBot.Message -> unit) -> TelegramBot
    [<Emit "$0.once('shipping_query',$1)">] abstract once_shipping_query: listener: (TelegramBot.ShippingQuery -> unit) -> TelegramBot
    [<Emit "$0.once('pre_checkout_query',$1)">] abstract once_pre_checkout_query: listener: (TelegramBot.PreCheckoutQuery -> unit) -> TelegramBot
    abstract once: ``event``: U3<string, string, string> * listener: (Error -> unit) -> TelegramBot
    abstract prependListener: ``event``: U2<TelegramBot.MessageType, string> * listener: (TelegramBot.Message -> TelegramBot.Metadata -> unit) -> TelegramBot
    [<Emit "$0.prependListener('callback_query',$1)">] abstract prependListener_callback_query: listener: (TelegramBot.CallbackQuery -> unit) -> TelegramBot
    [<Emit "$0.prependListener('inline_query',$1)">] abstract prependListener_inline_query: listener: (TelegramBot.InlineQuery -> unit) -> TelegramBot
    [<Emit "$0.prependListener('chosen_inline_result',$1)">] abstract prependListener_chosen_inline_result: listener: (TelegramBot.ChosenInlineResult -> unit) -> TelegramBot
    abstract prependListener: ``event``: U7<string, string, string, string, string, string, string> * listener: (TelegramBot.Message -> unit) -> TelegramBot
    [<Emit "$0.prependListener('shipping_query',$1)">] abstract prependListener_shipping_query: listener: (TelegramBot.ShippingQuery -> unit) -> TelegramBot
    [<Emit "$0.prependListener('pre_checkout_query',$1)">] abstract prependListener_pre_checkout_query: listener: (TelegramBot.PreCheckoutQuery -> unit) -> TelegramBot
    abstract prependListener: ``event``: U3<string, string, string> * listener: (Error -> unit) -> TelegramBot
    abstract prependOnceListener: ``event``: U2<TelegramBot.MessageType, string> * listener: (TelegramBot.Message -> TelegramBot.Metadata -> unit) -> TelegramBot
    [<Emit "$0.prependOnceListener('callback_query',$1)">] abstract prependOnceListener_callback_query: listener: (TelegramBot.CallbackQuery -> unit) -> TelegramBot
    [<Emit "$0.prependOnceListener('inline_query',$1)">] abstract prependOnceListener_inline_query: listener: (TelegramBot.InlineQuery -> unit) -> TelegramBot
    [<Emit "$0.prependOnceListener('chosen_inline_result',$1)">] abstract prependOnceListener_chosen_inline_result: listener: (TelegramBot.ChosenInlineResult -> unit) -> TelegramBot
    abstract prependOnceListener: ``event``: U7<string, string, string, string, string, string, string> * listener: (TelegramBot.Message -> unit) -> TelegramBot
    [<Emit "$0.prependOnceListener('shipping_query',$1)">] abstract prependOnceListener_shipping_query: listener: (TelegramBot.ShippingQuery -> unit) -> TelegramBot
    [<Emit "$0.prependOnceListener('pre_checkout_query',$1)">] abstract prependOnceListener_pre_checkout_query: listener: (TelegramBot.PreCheckoutQuery -> unit) -> TelegramBot
    abstract prependOnceListener: ``event``: U3<string, string, string> * listener: (Error -> unit) -> TelegramBot
    abstract removeListener: ``event``: U2<TelegramBot.MessageType, string> * listener: (TelegramBot.Message -> TelegramBot.Metadata -> unit) -> TelegramBot
    [<Emit "$0.removeListener('callback_query',$1)">] abstract removeListener_callback_query: listener: (TelegramBot.CallbackQuery -> unit) -> TelegramBot
    [<Emit "$0.removeListener('inline_query',$1)">] abstract removeListener_inline_query: listener: (TelegramBot.InlineQuery -> unit) -> TelegramBot
    [<Emit "$0.removeListener('chosen_inline_result',$1)">] abstract removeListener_chosen_inline_result: listener: (TelegramBot.ChosenInlineResult -> unit) -> TelegramBot
    abstract removeListener: ``event``: U7<string, string, string, string, string, string, string> * listener: (TelegramBot.Message -> unit) -> TelegramBot
    [<Emit "$0.removeListener('shipping_query',$1)">] abstract removeListener_shipping_query: listener: (TelegramBot.ShippingQuery -> unit) -> TelegramBot
    [<Emit "$0.removeListener('pre_checkout_query',$1)">] abstract removeListener_pre_checkout_query: listener: (TelegramBot.PreCheckoutQuery -> unit) -> TelegramBot
    abstract removeListener: ``event``: U3<string, string, string> * listener: (Error -> unit) -> TelegramBot
    abstract off: ``event``: U2<TelegramBot.MessageType, string> * listener: (TelegramBot.Message -> TelegramBot.Metadata -> unit) -> TelegramBot
    [<Emit "$0.off('callback_query',$1)">] abstract off_callback_query: listener: (TelegramBot.CallbackQuery -> unit) -> TelegramBot
    [<Emit "$0.off('inline_query',$1)">] abstract off_inline_query: listener: (TelegramBot.InlineQuery -> unit) -> TelegramBot
    [<Emit "$0.off('chosen_inline_result',$1)">] abstract off_chosen_inline_result: listener: (TelegramBot.ChosenInlineResult -> unit) -> TelegramBot
    abstract off: ``event``: U7<string, string, string, string, string, string, string> * listener: (TelegramBot.Message -> unit) -> TelegramBot
    [<Emit "$0.off('shipping_query',$1)">] abstract off_shipping_query: listener: (TelegramBot.ShippingQuery -> unit) -> TelegramBot
    [<Emit "$0.off('pre_checkout_query',$1)">] abstract off_pre_checkout_query: listener: (TelegramBot.PreCheckoutQuery -> unit) -> TelegramBot
    abstract off: ``event``: U3<string, string, string> * listener: (Error -> unit) -> TelegramBot
    abstract removeAllListeners: ``event``: obj -> TelegramBot
    abstract listeners: ``event``: obj -> Array<(obj option -> TelegramBot.Metadata -> unit)>
    abstract rawListeners: ``event``: obj -> Array<(obj option -> TelegramBot.Metadata -> unit)>
    abstract eventNames: unit -> Array<obj>
    abstract listenerCount: ``event``: obj -> float

type [<AllowNullLiteral>] TelegramBotStatic =
    [<Emit "new $0($1...)">] abstract Create: token: string * ?options: TelegramBot.ConstructorOptions -> TelegramBot