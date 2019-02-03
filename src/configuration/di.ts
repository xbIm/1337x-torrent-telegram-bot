import * as Bottle from 'bottlejs';
import * as TelegramBot from 'node-telegram-bot-api';
import getOtherPage from '../app/torrent/getOtherPage';
import { getTorrentsForUser } from '../app/torrent/getTorrentForUser';
import { parseMagnetLink, parseTorrentTable } from '../app/torrent/parse';
import { getMagneticLink, searchFullByUrl, searchTorrents } from '../app/torrent/searchOnSite';
import {
    editMessageText,
    reply,
    replyError,
    replyMessageWithInlineKeyboardLong,
    replyMessageWithKeyboard,
    sendMessage,
} from '../infrastructure/bot/bot';
import { BotEndpoint } from '../infrastructure/bot/BotEndpoint';
import torrentBotApi from '../infrastructure/endpoints/torrentBotEndpoints';
import { ExpressEndpoint } from '../infrastructure/express/ExpressEndpoint';
import { logDebug, logError, logInfo, logTrace, logWarn, startProfiler, traceObject } from '../infrastructure/logging';
import { request } from '../infrastructure/request';
import { setTrace, wrapTrace } from '../infrastructure/scopeMiddleware';
import { onPrevNext } from '../infrastructure/torrent/Pagination';
import connectToPersistence from '../persistence/connectToPersistance';
import { getSearchArgs, saveSearchArgs, updateSearchArgs } from '../persistence/torrent/mongo/searchArgs';
import { getSession, saveSession } from '../persistence/torrent/mongo/session';
import { config, isProduction } from './config';
import { app, setupExpress } from './express';
import { createLocalTunnel } from './localtunnel';
import MyBottle from './MyBottle';
import { serverStart } from './startup';

export function buildDI(): Bottle.IContainer {
    const bottle = new MyBottle();

    // logging
    bottle.addFunc('wrapTrace', wrapTrace);
    bottle.addFunc('setTrace', setTrace);
    bottle.addFunc('logInfo', logInfo);
    bottle.addFunc('logError', logError);
    bottle.addFunc('logWarn', logWarn);
    bottle.addFunc('logTrace', logTrace);
    bottle.addFunc('logDebug', logDebug);
    bottle.addFunc('startProfiler', startProfiler);
    bottle.addFunc('traceObject', traceObject);

    // tunnel
    bottle.addFunc('createLocalTunnel', createLocalTunnel);

    // config
    bottle.addFunc('isProduction', isProduction);
    bottle.addObj('mongoConfig', config.mongo);
    bottle.addObj('botConfig', config.bot);
    bottle.addObj('config', config);
    bottle.addObj('torrentConfig', config.torrentConfig);

    bottle.addFunc('request', request);
    // bot
    bottle.autoFactory('bot', (botConfig) => {
        return new TelegramBot(botConfig.token, botConfig.options);
    });
    bottle.addObj('app', app);

    bottle.addFunc('sendMessage', sendMessage);
    bottle.addFunc('replyMessageWithInlineKeyboardLong', replyMessageWithInlineKeyboardLong);
    bottle.addFunc('reply', reply);
    bottle.addFunc('replyMessageWithKeyboard', replyMessageWithKeyboard);
    bottle.addFunc('replyError', replyError);
    bottle.addFunc('editMessageText', editMessageText);
    // express
    bottle.addFunc('setupExpress', setupExpress);

    // mongo
    bottle.addFunc('connectToPersistence', connectToPersistence);
    bottle.addFunc('getSearchArgs', getSearchArgs);
    bottle.addFunc('saveSearchArgs', saveSearchArgs);
    bottle.addFunc('saveSession', saveSession);
    bottle.addFunc('getSession', getSession);
    bottle.addFunc('updateSearchArgs', updateSearchArgs);

    // search
    bottle.addFunc('searchTorrents', searchTorrents);
    bottle.addFunc('searchFullByUrl', searchFullByUrl);
    bottle.addFunc('getOtherPage', getOtherPage);

    // tasks
    bottle.addFunc('torrentBotApi', torrentBotApi);

    // logic
    bottle.addFunc('onPrevNext', onPrevNext);
    bottle.addFunc('getMagneticLink', getMagneticLink);
    bottle.addFunc('parseMagnetLink', parseMagnetLink);
    bottle.addFunc('parseTorrentTable', parseTorrentTable);
    bottle.addFunc('getTorrentsForUser', getTorrentsForUser);

    bottle.addFunc('serverStart', serverStart);
    bottle.addFunc('addBotApi', addBotApi);
    bottle.addFunc('addExpressApi', addExpressApi);
    bottle.addObj('bottle', bottle);
    return bottle.container;
}

function addBotApi(bottle: MyBottle, bot: TelegramBot, wrapTrace:
                       (func: ((match: RegExpExecArray | null) => Promise<any>))
                           => ((msg: TelegramBot.Message, match: RegExpExecArray | null) => void),
                   methods: Array<BotEndpoint>) {

    for (const endpoint of methods) {
        bot.onText(endpoint.match,
            wrapTrace(Object.defineProperty(bottle.partial(endpoint.exec), 'name', {value: endpoint.name})));
    }

    return bottle.container;
}

function addExpressApi(bottle: MyBottle, app: any, wrapTrace: (...args: Array<any>) => void, methods: Array<ExpressEndpoint>) {

    for (const endpoint of methods) {
        app[endpoint.method](endpoint.path, bottle.partial(endpoint.exec));
    }

    return bottle.container;
}

export function buildTestDi(dict: { [id: string]: (...args: Array<any>) => any; }): Bottle.IContainer {
    const localBottle = new MyBottle();

    Object.keys(dict).map((i) => {
        localBottle.addFunc(i, dict[i]);
    });

    return localBottle.container;
}
