import * as TelegramBot from 'node-telegram-bot-api';
import { BotEndpoint } from '../bot/BotEndpoint';
import { onGet, onSearchTorrent } from '../torrent/Search';
import { onSet, onShow, onShowCategory, onShowOrderBy } from '../torrent/SearchArgs';

export default function torrentBotApi(bot: TelegramBot,
                                      wrapTrace: (func: ((match: RegExpExecArray | null) => Promise<any>))
                                          => ((msg: TelegramBot.Message, match: RegExpExecArray | null) => void),
                                      onPrevNext: (bot: TelegramBot, message: any) => any) {

    bot.once('callback_query', (query: TelegramBot.CallbackQuery) => {
        onPrevNext(bot, query.message);
    });
}

export const torrentBotEndpoints: Array<BotEndpoint> = [
    {
        match: /\/search (.+)/,
        name: 'onSearch',
        exec: onSearchTorrent,
    }, {
        match: /\/get(.+)/,
        name: 'onGet',
        exec: onGet,
    }, {
        match: /\/show category$/,
        name: 'onShowCategory',
        exec: onShowCategory,
    }, {
        match: /\/{1}show$/,
        name: 'onShow',
        exec: onShow,
    },
    {
        match: /\/show orderby$/,
        name: 'onShowOrderBy',
        exec: onShowOrderBy,
    },
    {
        match: /\/set (.+) (.+)/gi,
        name: 'onSet',
        exec: onSet,

    }, {
        match: /(.+)/,
        name: 'onSearchTorrent',
        exec: onSearchTorrent,
    },
];
