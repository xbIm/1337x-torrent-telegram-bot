export interface BotEndpoint {
    match: RegExp,
    name: string,
    exec: (...args: Array<any>) => void,
}
