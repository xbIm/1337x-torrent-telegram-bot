import Bottle = require('bottlejs');
import { partial } from '../common';

export default class MyBottle extends Bottle {

    addObj = (name: string, obj: object) => {
        this.factory(name, () => {
            return obj;
        });
    };

    autoFactory = (name: string, func: (...args: Array<any>) => void) => {
        this.factory(name, () => {

            const args = this.getArgs(func);
            if (args.length > 0) {
                return func(...args);
            } else {
                return func();
            }
        });
    };

    addFunc = (name: string, func: (...args: Array<any>) => void) => {
        this.factory(name, () => {
            return this.partial(func);
        });
    };

    partial = (func: () => void) => {
        const args = this.getArgs(func);
        if (args.length > 0) {
            return partial(func, ...args);
        } else {
            return func;
        }
    };

    private getArgs(func: () => void) {
        const params = $args(func);
        const args = [];

        for (const i in params) {
            if (this.container[params[i]] != null) {
                args.push(this.container[params[i]]);
            }
        }

        return args;
    }
}

function $args(func) {
    return (func + '')
        .replace(/[/][/].*$/mg, '') // strip single-line comments
        .replace(/\s+/g, '') // strip white space
        .replace(/[/][*][^/*]*[*][/]/g, '') // strip multi-line comments
        .split(')', 1)[0].replace(/^[^(]*[(]/, '') // extract the parameters
        .replace(/=[^,]+/g, '') // strip any ES6 defaults
        .split(',').filter(Boolean); // split & filter [""]
}
