export interface ExpressEndpoint {
    method: string;
    path: string;
    exec: (...args: Array<any>) => void;
}
