import * as crypto from 'crypto';
import * as TelegramBot from 'node-telegram-bot-api';
import * as winston from 'winston';

import { getId } from './bot/bot';
import { logError, logInfo } from './logging';

const TRACE = 'trace_id';
import cls = require('continuation-local-storage');

const getNamespace = cls.getNamespace;
const nsName = 'namespace';
cls.createNamespace(nsName);

export function wrapTrace(
    setTrace: (func: () => void) => void,
    startProfiler: () => winston.Profiler,
    func: ((msg: TelegramBot.Message, match: RegExpExecArray | null) => Promise<any>))
    : ((msg: TelegramBot.Message, match: RegExpExecArray | null) => void) {
    return (message: TelegramBot.Message, match: any) => {
        let traceId = null;
        let profiler = null;

        Promise.resolve().then(() => {
            return new Promise((resolve) => {
                setTrace(() => {
                    traceId = getTrace();
                    logInfo(`%s has called with chatId:%d and name:${message.from.username} and text:${(message).text}`,
                        {funcName: func.name, chatId: getId(message)});
                    profiler = startProfiler();
                    resolve(func(message, match));
                });
            });
        })
            .then(() => {
                profiler.done({message: 'answered'});

                if (traceId !== getTrace()) {
                    logError(Error(`different trace id, ${traceId} != ${getTrace()}`));
                }
            })
            .catch((ex) => {
                logError(ex);
                if (traceId !== getTrace()) {
                    logError(Error(`different trace id, ${traceId} != ${getTrace()}`));
                }
            });
    };
}

export function getTrace(): string {
    const ns = getNamespace(nsName);
    if (ns == null) {
        return '';
    }
    return getNamespace(nsName).get(TRACE);
}

export function setTrace(func: () => void): void {

    const ns = getNamespace(nsName);
    ns.run(() => {
        ns.set(TRACE, randomHex(8));
        func();
    });
}

function init(func: () => void) {
    const namespace = cls.getNamespace(nsName);
    namespace.run(() => {
        func();
    });
}

export function getFromScope(key: string): any {
    const namespace = cls.getNamespace(nsName);
    return namespace.get(key);
}

export function setInScope(key: string, value: any): void {
    const namespace = cls.getNamespace(nsName);
    return namespace.set(key, value);
}

export function randomHex(length: number) {
    return crypto.randomBytes(length).toString('hex');
}
