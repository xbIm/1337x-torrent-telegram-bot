import {Document, Model, model, Schema} from 'mongoose';

export class TorrentTableEntity {
    public id: number;
    public url: string;
    public title: string;
    public size: string;
    public seaders: number;
    public leachers: number;
    public time: string;
    public author: string;
}

export interface Session {
    userId: number;
    messageId: number;
    currentPosition: number;
    torrents: Array<TorrentTableEntity>;
    next?: string;
    prev?: string;
}

interface SessionModel extends Session, Document {
}

const TorrentSchema = new Schema({
    id: Number,
    url: String,
    title: String,
    size: String,
    seaders: Number,
    leachers: Number,
    time: String,
    author: String,
});

const SessionSchema: Schema = new Schema({
    userId: Number,
    messageId: Number,
    currentPosition: Number,
    torrents: [TorrentSchema],
    next: String,
    prev: String,
});

const SessionModel: Model<SessionModel> = model<SessionModel>('session', SessionSchema);

export function saveSession(session: Session): Promise<Session> {
    // const profiler = startProfiler();
    return SessionModel
        .findOneAndUpdate({userId: session.userId}, session, {upsert: true, new: true})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null)
        .then((result) => {
            //profiler.done({message: 'mongo:saveSession'});
            return result;
        });
}

export function getSession(userId: number): Promise<Session> {
    //const profiler = startProfiler();
    return SessionModel
        .findOne({userId})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null)
        .then((result) => {
            //profiler.done({message: 'mongo:getSession'});
            return result;
        });
}
