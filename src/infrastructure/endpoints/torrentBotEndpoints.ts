import * as TelegramBot from 'node-telegram-bot-api';
import { getId } from '../bot/bot';
import { BotEndpoint } from '../bot/BotEndpoint';
import { logInfo, startProfiler } from '../logging';
import { setTrace } from '../scopeMiddleware';
import { onGet, onSearchTorrent } from '../torrent/Search';
import { onSet, onShow, onShowCategory, onShowOrderBy } from '../torrent/SearchArgs';

export default function torrentBotApi(bot: TelegramBot,
                                      onPrevNext: (bot: TelegramBot, message: any, data: string) => any) {

    bot.on('callback_query', (query: TelegramBot.CallbackQuery) => {
        let profiler = null;

        Promise.resolve().then(() => {
            return new Promise((resolve) => {
                setTrace(() => {
                    logInfo(`%s has called with chatId:%d and name:${query.message.from.username} and text:${(query.message).text}`,
                        {funcName: 'onPrevNext', chatId: getId(query.message)});
                    profiler = startProfiler();
                    resolve(onPrevNext(bot, query.message, query.data));
                });
            });
        })
            .then(() => {
                profiler.done({message: 'answered'});
            });
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
