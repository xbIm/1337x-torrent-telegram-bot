import {connect, Mongoose} from 'mongoose';
import mongoose = require('mongoose');

(mongoose as any).Promise = Promise;

export function startUpMongo(uri: string): Promise<Mongoose> {
    return connect(uri, {useNewUrlParser: true})
        .then((mongoose) => {
            mongoose.set('useFindAndModify', false);
            return Promise.resolve(mongoose);
        });
}
