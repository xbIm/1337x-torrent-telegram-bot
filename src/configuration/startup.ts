import * as TelegramBot from 'node-telegram-bot-api';

export function serverStart(logInfo: (message: string, ...meta: Array<any>) => void,
                            logError: (error: Error) => void,
                            isProduction: () => boolean,
                            config: any,
                            bot: TelegramBot,
                            connectToPersistence: () => Promise<any>,
                            setupExpress: (bot: TelegramBot, url: string) => Promise<any>,
                            createLocalTunnel: () => Promise<any>,
                            torrentBotApi: () => void,
) {
    connectToPersistence()
        .then(() => {
            if (isProduction()) {
                return Promise.resolve({url: config.server.host});
            } else {
                return createLocalTunnel()
                    .then((tunnel) => {
                        return Promise.resolve({url: tunnel.url});
                    });
            }
        })
        .then((res) => {
            bot.setWebHook(`${res.url}/bot${config.bot.token}`);

            torrentBotApi();
            return setupExpress(bot, res.url);
        })
        .then(() => {
            logInfo('started');
        })
        .catch((error) => {
            logError(error);
        });
}
