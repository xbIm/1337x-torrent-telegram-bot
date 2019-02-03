import { createPromise } from '../common';
import { config } from './config';

export function createLocalTunnel(): Promise<{}> {

    const localtunnel = require('localtunnel');

    return createPromise(localtunnel, config.server.port);
}
