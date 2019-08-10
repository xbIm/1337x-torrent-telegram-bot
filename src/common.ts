export function partial(fn, ...restOfName: Array<any>) {
    // A reference to the Array#slice method.
    const slice = Array.prototype.slice;
    // Convert arguments object to an array, removing the first argument.
    const args = slice.call(arguments, 1);

    return function() {
        // Invoke the originally-specified function, passing in all originally-
        // specified arguments, followed by any just-specified arguments.
        return fn.apply(this, args.concat(slice.call(arguments, 0)));
    };
}

export function createPromise<T>(func: (arg?: any, callback?: ((error: Error, data: T) => void)) => void, arg?: any): Promise<T> {
    return new Promise((resolve, reject) => {
        function callback(err, data): void {
            if (err) {
                reject(err);
                return;
            }
            resolve(data);
        }

        if (arg) {
            func(arg, callback);
        } else {
            func(callback);
        }

    });
}

export function compareStr(str1: string, str2: string): boolean {
    return str1 != null && str2 != null
        && typeof str1 === 'string' && typeof str2 === 'string'
        && str1.toLocaleUpperCase() === str2.toLocaleUpperCase();
}

export function format(format: string, args: Array<string>) {
    return format.replace(/{(\d+)}/g, (match, num) => {
        return typeof args[num] !== 'undefined'
            ? args[num]
            : match;
    });
}
