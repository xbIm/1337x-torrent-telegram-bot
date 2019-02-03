import * as winston from 'winston';
import { config } from '../configuration/config';
import { getTrace } from './scopeMiddleware';

const {combine, timestamp, label, json, printf, splat} = winston.format;
import { SPLAT } from 'triple-beam';

const filterTraceId = winston.format((info, opts) => {
    return {...info, trace_id: getTrace()};
});

const filterSplat = winston.format((info, opts) => {
    Object.assign(info, info['0']);
    info[SPLAT] = values(info['0']);
    delete info['0'];
    return info;
});

const logger = winston.createLogger({
    level: config.server.logLevelConsole,
    format: combine(
        label({label: 'torrent-bot'}),
        timestamp(),
        filterTraceId(),
        filterSplat(),
        splat(),
        json(),
    ),
    transports: [
        new winston.transports.Console(),
    ],
});

export function traceObject(message: string, obj: any) {
    logTrace(message + ':' + JSON.stringify(obj));
}

export function logTrace(message: string) {
    logger.silly(message);
}

export function logDebug(message: string) {
    logger.debug(message);
}

export function logInfo(message: string, ...meta: Array<any>) {
    logger.info(message, meta);
}

export function logWarn(message: string) {
    logger.warn(message);
}

export function logError(error: Error) {
    logger.error(error.message, {errorStack: error.stack.substr(0, 100)});
}

export function startProfiler(): winston.Profiler {
    return logger.startTimer();
}

function values(object: {}) {
    const vals = [];
    for (const key in object) {
        if (object.hasOwnProperty(key)) {
            vals.push(object[key]);
        }
    }
    return vals;
}
