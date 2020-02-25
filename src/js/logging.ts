import * as winston from 'winston';
//import { getTrace } from './scopeMiddleware';

const {combine, timestamp, label, json, printf, splat} = winston.format;
import {SPLAT} from 'triple-beam';
import {getTrace} from "./scopeMiddleware";

const filterTraceId = winston.format((info, opts) => {
    return {...info, trace_id: getTrace()};
});

const filterSplat = winston.format((info, opts) => {
    Object.assign(info, info['0']);
    info[SPLAT] = values(info['0']);
    delete info['0'];

    return info;
});

export interface ILogger {
    LogTrace: (message: string) => void;
    LogDebug: (message: string) => void;
    LogInfo: (message: string, meta: any) => void;
    LogError: (error: Error) => void;
    LogInfoDuration: (message: string, meta: any, f: (any) => Promise<any>, arg: any) => Promise<any>;
}

export function createLogger(logLevel: string): ILogger {
    const logger = winston.createLogger({
        level: logLevel,
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

    return {
        LogTrace: (message: string) => logger.silly(message),
        LogDebug: (message: string) => logger.debug(message),
        LogInfo: (message: string, meta: Array<any>) => logger.info(message, meta),
        LogError: (error: Error) => logger.error(error.message, {errorStack: error.stack.substr(0, 100)}),
        LogInfoDuration: (message: string, meta: Array<any>, f: (any) => Promise<any>, arg: any): Promise<any> => {
            const profiler = logger.startTimer();
            return f(arg)
                .then((r) => {
                    const toBeLogged = {};
                    if (arg != null) {
                        for (let i in r) {
                            if (!r.hasOwnProperty(i)) continue;

                            if (typeof r[i] == "string") {
                                if (r[i].length > 40) {
                                    continue;
                                }
                            }
                            toBeLogged[i] = r[i];
                        }
                    }


                    profiler.done({message, ...toBeLogged, ...meta});
                    return r;
                })
        }
    }
}

export function getEnvVal(key) {
    return process.env[key];
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
