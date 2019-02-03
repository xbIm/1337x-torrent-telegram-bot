import { Response } from '../domain/CommonEntities';
import { startProfiler } from './logging';

import _request = require('request');

export function request(url): Promise<Response> {
    const profiler = startProfiler();
    return new Promise((resolve, reject) => {
        _request(url, (err, response, html) => {
            if (err) {
                reject(err);
                return;
            }
            profiler.done({message: 'server respond', url, statusCode: response.statusCode});
            resolve({obj: response, html});
        });
    });
}
