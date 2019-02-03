import * as TelegramBot from 'node-telegram-bot-api';
import { KeyboardButton } from 'node-telegram-bot-api';
import { getOrderBy } from '../../app/torrent/getOrderBy';
import { compareStr } from '../../common';
import { CATEGORY, ORDER_BY } from '../../domain/torrent/Consts';
import { SearchArg, SearchArgs, searchArgsValues } from '../../domain/torrent/Entities';
import { getId } from '../bot/bot';

export function onShowCategory(replyMessageWithKeyboard: (message: TelegramBot.Message,
                                                          text: string,
                                                          array: Array<Array<KeyboardButton>>) => Promise<any>,
                               message: TelegramBot.Message) {

    const categories = [];
    for (const elem of searchArgsValues[CATEGORY]) {
        categories.push(
            [{text: `/set ${CATEGORY} ${elem.name}`}],
        );
    }

    categories.push(['Cancel']);

    return replyMessageWithKeyboard(message, 'Please choose category for your searches', categories);
}

export function onShow(replyMessageWithKeyboard: (message: TelegramBot.Message,
                                                  text: string,
                                                  array: Array<Array<KeyboardButton>>) => Promise<any>,
                       message: TelegramBot.Message) {

    return replyMessageWithKeyboard(
        message,
        'Please choose more options',
        [[{text: `/show ${ORDER_BY}`}], [{text: `/show ${CATEGORY}`}]]);
}

export function onShowOrderBy(getSearchArgs: (userId: number) => Promise<SearchArgs>,
                              logTrace: (message: string) => void,
                              logDebug: (message: string) => void,
                              replyMessageWithKeyboard: (message: TelegramBot.Message, text: string, array: Array<Array<KeyboardButton>>)
                                  => Promise<any>,
                              replyError: (message: TelegramBot.Message, error: Error) => Promise<any>,
                              message: TelegramBot.Message) {

    const chatId = getId(message);

    return getSearchArgs(chatId)
        .then((searchArgs) => {

            const current = getOrderBy(logTrace, searchArgs);
            const response = current == null ?
                'OrderBy is not set. Please choose on which order to search.'
                : `Current OrderBy is set:${current}. You can change it.`;

            const orderBy = [];
            for (const elem of searchArgsValues[ORDER_BY]) {
                orderBy.push(
                    [{text: '/set orderby ' + elem.name}],
                );
            }
            orderBy.push(['Cancel']);

            return replyMessageWithKeyboard(message, response, orderBy);
        })
        .catch((error) => replyError(message, error));
}

export function onSet(reply: (msg: TelegramBot.Message, text: string, options?: TelegramBot.SendMessageOptions) => Promise<any>,
                      replyError: (msg: TelegramBot.Message, error: Error, options?: TelegramBot.SendMessageOptions) => Promise<any>,
                      logDebug: (message: string) => void,
                      updateSearchArgs: (userId: number, fieldName: string, value: string) => Promise<SearchArgs>,
                      message: TelegramBot.Message,
                      match: Array<string>) {
    const chatId = getId(message);

    const type = match[1];
    const elem = match[2];

    logDebug(`type:${type} elem:${elem}`);

    return validateSearchArgs(type, elem)
        .then((searchArg) => updateSearchArgs(chatId, type, searchArg.urlPart))
        .then((result) => {
            return reply(message, `${type}:${elem} has been set`, {
                reply_markup: {
                    remove_keyboard: true,
                },
            });
        })
        .catch((error) => replyError(message, error, {
            reply_markup: {
                remove_keyboard: true,
            },
        }));
}

function validateSearchArgs(fieldNameReq: string, valueReq: string): Promise<SearchArg> {
    const fieldName = [ORDER_BY, CATEGORY].find((item) => compareStr(fieldNameReq, item));
    if (!fieldName) {
        return Promise.reject(Error(`Field name ${fieldNameReq} doesn't exists`));
    }

    const value = searchArgsValues[fieldName].find((item) => compareStr(item.name, valueReq));

    if (!value) {
        return Promise.reject(Error(`${fieldNameReq} ${valueReq} doesn't exists`));
    }
    return Promise.resolve(value);
}
