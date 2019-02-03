import { connect, Mongoose } from 'mongoose';
import { MongoConfig } from '../configuration/config';

import mongoose = require('mongoose');
(mongoose as any).Promise = Promise;

export function startUpMongo(mongoConfig: MongoConfig): Promise<Mongoose> {
    return connect(mongoConfig.uri, {useNewUrlParser: true})
        .then((mongoose) => {
            mongoose.set('useFindAndModify', false);
            return Promise.resolve(mongoose);
        });
}
