import * as TelegramBot from 'node-telegram-bot-api';
import { BotEndpoint } from '../bot/BotEndpoint';

export const commonBotApis: Array<BotEndpoint> = [{
    match: /\/start/i,
    name: 'onStart',
    exec: (reply: (message: TelegramBot.Message, text: string, options?: TelegramBot.SendMessageOptions) => Promise<{}>,
           message: TelegramBot.Message): Promise<{}> => {
        return reply(message, 'Hi, I\'m searching for torrents on 1337x.to, To start searching write me what torrent '
            + 'link are you looking for or press /help for more information.');
    },
}, {
    match: /\/help/i,
    name: 'onHelp',
    exec: (reply: (message: TelegramBot.Message, text: string, options?: TelegramBot.SendMessageOptions) => Promise<{}>,
           message: TelegramBot.Message): Promise<{}> => {

        const answer = '<b>What can I do?</b>\nTell me what do you need to find and I ll do it\n\nBrief list of commands:\n' +
            '/start - the first command \n' +
            '/help - brief information, which you are reading now\n' +
            '/search - torrents search\n' +
            '/show orderby - show order by options for torrents search\n' +
            '/set orderby - set order by for torrents search\nP.S. \n' +
            '- This is not official bot of 1337x.to\n' +
            '- For question please contact @xbimz ';
        return reply(message, answer, {parse_mode: 'HTML'});
    },
},
    {
        match: /cancel/i,
        name: 'onCancel',
        exec: (reply: (message: TelegramBot.Message, text: string, options?: TelegramBot.SendMessageOptions) => Promise<{}>,
               message: TelegramBot.Message): Promise<{}> => {
            return reply(message, 'Cancel. Let\' start over', {
                reply_markup: {
                    remove_keyboard: true,
                },
            });
        },
    },
];

export const unknownAnswer: Array<BotEndpoint> = [{
    match: /^\/.+/gi,
    name: 'onUnkownCall',
    exec: (reply: (message: TelegramBot.Message, text: string, options?: TelegramBot.SendMessageOptions) => Promise<{}>,
           message: TelegramBot.Message): Promise<{}> => {
        return reply(message, 'Command not found, try /help');
    },
}];
