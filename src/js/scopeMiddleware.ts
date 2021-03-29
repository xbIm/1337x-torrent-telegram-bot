import * as crypto from 'crypto';
import * as TelegramBot from 'node-telegram-bot-api';

const TRACE = 'trace_id';
import cls = require('cls-hooked');
import {ILogger} from "./logging";

const getNamespace = cls.getNamespace;
const nsName = 'namespace';
cls.createNamespace(nsName);

export function wrapTrace(logger: ILogger, funcName: string, message: TelegramBot.Message, func: () => Promise<TelegramBot.Message>) {

    setTrace(() => {
        if ((message).text.length < 100) {
            logger.LogInfo(`${funcName} has called with chatId:${message.chat.id} and name:${message.from.username} and text:${(message).text}`,
                {funcName, chatId: message.chat.id,text: message.text});
        }
        logger.LogInfoDuration("answered", {funcName}, func, null);
    })
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
