import promiseRetry = require('promise-retry');
import _request = require('request');

const domain = process.env["1337_domain"] || "https://1337x.to";

export function request(url): Promise<Response> {
    //const profiler = startProfiler();
    return promiseRetry((retry, num) => {
        return new Promise((resolve, reject) => {
            _request(domain + url, (err, response, html) => {
                if (err) {
                    reject(err);
                    return;
                }
                //profiler.done({message: 'server respond', url, statusCode: response.statusCode, try: num});
                resolve({html: html, statusCode: response.statusCode, try: num});
            });

        })
            .catch(retry);
    });
}
