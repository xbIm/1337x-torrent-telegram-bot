import { KeyboardButton } from 'node-telegram-bot-api';

export function fakeLog(message: string) {
    return 0;
}

export function fakeErrorLog(error: Error) {
    return 0;
}

export function replyMessageWithKeyboard(message: string,
                                         array: Array<Array<KeyboardButton>>): Promise<any> {

    return Promise.resolve(true);
}

export function fakeReplyError(error: Error): Promise<any> {

    return Promise.resolve(true);
}

export class ArraysEqualMatcher {

    constructor(private expectedValue) {

    }

    matches = (anotherArray) => {
        return this._matches(this.expectedValue, anotherArray);
    }

    describeTo = () => {
        return 0;
    }

    private _matches = (array, anotherArray) => {
        if (array instanceof Array || anotherArray instanceof Array) {
            if (array.length !== anotherArray.length) {
                return false;
            }

            for (let i = 0; i < array.length; i++) {
                const a = array[i];
                const b = anotherArray[i];

                if (a instanceof Array || b instanceof Array) {
                    if (!this._matches(a, b)) {
                        return false;
                    }
                } else if (a !== b) {
                    return false;
                }
            }
            return true;
        } else {
            return array === anotherArray;
        }
    }
}

export class ObjectEqualMatcher {

    constructor(private expectedValue) {

    }

    matches = (anotherObject) => {
        return this._matches(this.expectedValue, anotherObject);
    }

    describeTo = () => {
        return 0;
    }

    private _matches = (expected, another) => {
        return JSON.stringify(expected) === JSON.stringify(another);
    }
}
