import * as TelegramBot from 'node-telegram-bot-api';
import { KeyboardButton } from 'node-telegram-bot-api';
import { getUrl } from '../../configuration/express';
import { Direction, PaginationRequest, SearchResult, Session } from '../../domain/torrent/Entities';
import { getId } from '../bot/bot';

export function onGet(getSession: (userId: number) => Promise<Session>,
                      reply: (message: TelegramBot.Message, text: string, options?: TelegramBot.SendMessageOptions) => Promise<any>,
                      replyError: (message: TelegramBot.Message, error: Error) => Promise<any>,
                      message: TelegramBot.Message,
                      match: Array<string>) {
    const param = match[0].replace(/^\D+/g, '');

    const userId = getId(message);
    const torrentId = parseInt(param, 10);

    return getSession(userId)
        .then((session) => {
            const torrent = session.torrents.find((e) => e.id === torrentId);
            if (!torrent) {
                return reply(message, 'torrent id not found');
            }
            return reply(message, `${torrent.title} \nlink: ${getLink(userId, torrentId)}`, {parse_mode: 'HTML'});
        })
        .catch((error) => replyError(message, error));
}

export function onSearchTorrent(
    replyMessageWithInlineKeyboardLong: (
        message: TelegramBot.Message,
        text: string,
        array: Array<Array<KeyboardButton>>) => Promise<any>,
    searchTorrents: (userId: number,
                     messageId: number,
                     searchText: string) => Promise<SearchResult>,
    reply: (message: TelegramBot.Message,
            text: string) => Promise<void>,
    replyError: (message: TelegramBot.Message, error: Error) => Promise<any>,
    logTrace: (message: string) => void,
    message: TelegramBot.Message,
    match: Array<string>): Promise<any> {

    const text = match[1];

    if (!/^[A-Za-z0-9_ ]*$/.test(text)) {
        return reply(message, 'Only latin sign is allowed');
    }
    if (text.length > 100) {
        return reply(message, 'Text should be less than 100 symbols');
    }

    const userId = getId(message);
    const messageId = message.message_id;

    return searchTorrents(userId, messageId, text)
        .then((value) => {

            const prevNext = [];
            if (value.prev) {
                prevNext.push([{
                    text: `Previous`,
                    callback_data: new PaginationRequest(Direction.Prev, value.currentPosition).toData(),
                }]);
            }
            if (value.next) {
                prevNext.push([{
                    text: `Next`,
                    callback_data: new PaginationRequest(Direction.Next, value.currentPosition).toData(),
                }]);
            }
            logTrace(`value.text.length: ${value.text.length}`);

            return replyMessageWithInlineKeyboardLong(message, value.text, prevNext);
        })
        .catch((error) => replyError(message, error));
}

function getLink(chatId: number, i: number) {
    return `<a href='${process.env.HOST}${getUrl}${chatId}/${i}'>download</a>`;
}
