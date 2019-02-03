import { expressEndpoints } from '../infrastructure/endpoints/expressEndpoints';
import { torrentBotEndpoints } from '../infrastructure/endpoints/torrentBotEndpoints';
import { commonBotApis, unknownAnswer } from '../infrastructure/endpoints/сommonBotEndpoints';
import { buildDI } from './di';

buildDI()
    .addBotApi(commonBotApis)
    .addBotApi(torrentBotEndpoints)
    .addBotApi(unknownAnswer)
    .addExpressApi(expressEndpoints)
    .serverStart();
