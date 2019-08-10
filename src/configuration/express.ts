import * as TelegramBot from 'node-telegram-bot-api';
import { createPromise } from '../common';
import { config } from './config';

import bodyParser = require('body-parser');
import express = require('express');

export const app = express();
export const getUrl = '/xtbot/get/';

export function setupExpress(logInfo: (message: string) => void,
                             isProduction: () => boolean,
                             cBot: TelegramBot,
                             url: string) {
    app.use(bodyParser.json());

    app.post(`/bot${config.bot.token}`, (req, res) => {
        logInfo('request started');

        cBot.processUpdate(req.body);

        res.sendStatus(200);
    });

    app.get('/healthcheck', (req, res) => {
        res.sendStatus(200);
    });

    return createPromise(app.listen.bind(app), config.server.port)
        .then(() => {
            if (isProduction()) {
                logInfo(`Express server is listening on ${url}:${config.server.port} `);
            } else {
                logInfo(`Express server is listening on ${url}/healthcheck`);
            }
        });
}
