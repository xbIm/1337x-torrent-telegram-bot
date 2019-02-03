import * as TelegramBot from 'node-telegram-bot-api';
import { TorrentConfig } from '../domain/torrent/TorrentConfig';

export const config = {
    env: process.env.NODE_ENV,
    bot: {
        token: process.env.TOKEN,
        options: {onlyFirstMatch: true},
    } as BotConfig,
    server: {
        host: process.env.HOST,
        port: parseInt(process.env.PORT, 10) || 3000 as number,
        logLevelConsole: process.env.LOG_LEVEL_CONSOLE || 'silly',
        logLevelAppInsights: process.env.LOG_LEVEL_APPINSIGHTS || 'info',
        appInsightsKey: process.env.APPINSIGHTS_INSTRUMENTATIONKEY,
    } as ServerConfig,
    mongo: {
        uri: process.env.MONGO_URI || 'mongodb://localhost:27017/admin',
    } as MongoConfig,
    torrentConfig: {
        amountPerPage: parseInt(process.env.PER_PAGE, 10) || 10,
    } as TorrentConfig,

};

export const isProduction = () => process.env.NODE_ENV === 'production';

export interface ServerConfig {
    host: string;
    port: number;
    logLevelConsole: string;
    logLevelAppInsights: string;
    appInsightsKey: string;
}

export interface MongoConfig {
    uri: string;
}

export interface BotConfig {
    token: string,
    options: TelegramBot.ConstructorOptions,
    amountPerPage: number,
}
