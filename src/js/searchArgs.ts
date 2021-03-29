import { Document, Model, model, Schema } from 'mongoose';

export interface SearchArgs {
    userId: number;
    category?: string;
    orderby?: string;
}

interface SearchArgsModel extends SearchArgs, Document {
}

const SearchArgsSchema: Schema = new Schema({
    userId: Number,
    category: String,
    orderby: String,
});

const SearchArgsModel: Model<SearchArgsModel> = model<SearchArgsModel>('searchArgs', SearchArgsSchema);

export function saveSearchArgs(session: SearchArgs): Promise<SearchArgs> {
    return SearchArgsModel
        .findOneAndUpdate({userId: session.userId}, session, {upsert: true, new: true})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null);
}

export function updateSearchArgs(userId: number, fieldName: string, value: string): Promise<SearchArgs> {
    return SearchArgsModel
        .findOneAndUpdate({userId},
            {[fieldName]: value, userId}
            , {upsert: true, new: true})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null);
}

export function getSearchArgs(userId: number): Promise<SearchArgs> {
    return SearchArgsModel
        .findOne({userId})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null);
}
