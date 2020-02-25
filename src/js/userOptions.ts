import {Document, Model, model, Schema} from 'mongoose';
import {UserOptions} from './Entities';


interface UserOptionsModel extends UserOptions, Document {
}

const UserOptionsSchema = new Schema({
    userId: Number,
    showMagnetic: String,
});

const UserOptions: Model<UserOptionsModel> = model<UserOptionsModel>('userOptions', UserOptionsSchema);

export function saveUserOptions(userOptions: UserOptions): Promise<UserOptions> {
    //const profiler = startProfiler();
    return UserOptions
        .findOneAndUpdate({userId: userOptions.userId}, userOptions, {upsert: true, new: true})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null)
        .then((result) => {
            //profiler.done({message: 'mongo:saveUserOptions'});
            return result;
        });
}

export function getUserOptions(userId: number): Promise<UserOptions> {
    return UserOptions
        .findOne({userId})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null);
}

export function updateUserOptions(userId: number, fieldName: string, value: string): Promise<UserOptions> {

    return UserOptions
        .findOneAndUpdate({userId},
            {[fieldName]: value, userId}
            , {upsert: true, new: true})
        .exec()
        .then((result) => result ? result.toObject({getters: true}) : null);
}
