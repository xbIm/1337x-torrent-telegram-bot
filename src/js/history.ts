import {Document, Model, model, Schema} from 'mongoose';
import {SearchArgs} from "./Entities";

interface UserInput {
    text: string;
    date: Date;
}

interface History {
    userId: number;
    records: UserInput[]
}

interface HistoryModel extends History, Document {
}

const UserInputSchema: Schema = new Schema({
    text: String,
    date: Date,
});

const HistorySchema: Schema = new Schema({
    userId: Number,
    records: [UserInputSchema]
});

const HistoryModel: Model<HistoryModel> = model<HistoryModel>('history', HistorySchema);


export function getHistory(userId: number): Promise<History> {
    return HistoryModel
        .findOne({userId})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null)
}

export function addRecord(userId: number, text: string): Promise<History> {
    return getHistory(userId).then(h => {
        if (!h) {
            h = {userId: userId, records: []}
        }
        h.records.push({text, date: new Date()});
        if (h.records.length > 10) {
            h.records.splice(0, 1)
        }
        return HistoryModel.findOneAndUpdate({userId: userId}, h, {upsert: true, new: true})
            .exec();
    })
        .then((result) => result ? result.toObject({getters: true}) : null);
}
