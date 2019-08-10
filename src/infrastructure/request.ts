import { Response } from '../domain/CommonEntities';
import { startProfiler } from './logging';

import promiseRetry = require('promise-retry');
import _request = require('request');

export function request(url): Promise<Response> {
    const profiler = startProfiler();
    return promiseRetry((retry, num) => {
        return new Promise((resolve, reject) => {
            _request(url, (err, response, html) => {
                if (err) {
                    reject(err);
                    return;
                }
                profiler.done({message: 'server respond', url, statusCode: response.statusCode, try: num});
                resolve({obj: response, html});
            });
        })
            .catch(retry);
    });
}
