import * as TelegramBot from 'node-telegram-bot-api';
import { KeyboardButton } from 'node-telegram-bot-api';
import { logError } from '../logging';

export function replyMessageWithKeyboardLong(reply: (message: TelegramBot.Message,
                                                     text: string,
                                                     options?: TelegramBot.SendMessageOptions) => Promise<TelegramBot.Message | Error>,
                                             message: TelegramBot.Message,
                                             text: string,
                                             array: Array<Array<KeyboardButton>>) {
    return reply(message, text, {
        reply_markup: {
            keyboard: array,
            one_time_keyboard: true,
        },
    });
}

export function editMessageText(bot: TelegramBot, text: string, options?: TelegramBot.EditMessageTextOptions):
    Promise<TelegramBot.Message | boolean | Error> {
    return bot.editMessageText(text, options);
}

export function replyMessageWithInlineKeyboardLong(reply: (message: TelegramBot.Message,
                                                           text: string,
                                                           options?: TelegramBot.SendMessageOptions) =>
                                                       Promise<TelegramBot.Message | Error>,
                                                   message: TelegramBot.Message,
                                                   text: string,
                                                   array: Array<Array<KeyboardButton>>) {

    return reply(message, text, {
        reply_markup: {
            inline_keyboard: array,
        },
        parse_mode: 'HTML',
    });
}

export function getId(msg: TelegramBot.Message): number {
    if (!msg.chat) {
        return null;
    }
    return msg.chat.id;
}

export function sendMessage(bot: TelegramBot,
                            chatId: number | string,
                            text: string,
                            options?: TelegramBot.SendMessageOptions)
    : Promise<TelegramBot.Message | Error> {
    return bot.sendMessage(chatId, text, options);
}

export function reply(bot: TelegramBot,
                      logDebug: (message: string) => void,
                      msg: TelegramBot.Message,
                      text: string,
                      options?: TelegramBot.SendMessageOptions) {

    const chatId = getId(msg);

    logDebug(`replying to ${chatId}`);

    return bot.sendMessage(chatId, text, options);
}

export function replyError(
    reply: (message: TelegramBot.Message,
            text: string,
            options?: TelegramBot.SendMessageOptions) => Promise<TelegramBot.Message | Error>,
    message: TelegramBot.Message,
    error: Error,
    options?: TelegramBot.SendMessageOptions): Promise<any> {
    {
        logError(error);

        return reply(message, 'Something went wrong', options);
    }
}

export function replyMessageWithKeyboard(reply: (message: TelegramBot.Message,
                                                 text: string, option?: TelegramBot.SendMessageOptions)
                                             => Promise<TelegramBot.Message | Error>,
                                         message: TelegramBot.Message,
                                         text: string,
                                         array: Array<Array<KeyboardButton>>) {
    return reply(message, text, {
        reply_markup: {
            keyboard: array,
            one_time_keyboard: true,
        },
    });
}
