import { MongoConfig } from '../configuration/config';
import { startUpMongo } from './mongo';

export default function connectToPersistence(mongoConfig: MongoConfig): Promise<{}> {
    return startUpMongo(mongoConfig);
}
