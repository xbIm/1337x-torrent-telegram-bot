//todo:remove this file

export interface ParseResult {
    torrents: Array<TorrentTableEntity>;
    next?: string;
    prev?: string;
}

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



const All = 'All';
const ORDER_BY = 'orderby';
const CATEGORY = 'category';

export interface SearchArgs {
    userId: number;
    [CATEGORY]?: string;
    [ORDER_BY]?: string;
}

export interface SearchResult {
    text: string;
    currentPosition: number;
    next?: string;
    prev?: string;
}

export interface SearchArg {
    name: string;
    urlPart: string;
}

export interface RequestArgs {
    url: string;
    elemNumbers: number;
}

const categoryArray: Array<SearchArg> = [
    {name: 'All', urlPart: All},
    {name: 'Movies', urlPart: 'Movies'},
    {name: 'TV', urlPart: 'TV'},
    {name: 'Games', urlPart: 'Games'},
    {name: 'Music', urlPart: 'Music'},
    {name: 'Applications', urlPart: 'Apps'},
    {name: 'Documentaries', urlPart: 'Documentaries'},
    {name: 'Anime', urlPart: 'Anime'},
    {name: 'Other', urlPart: 'Other'},
    {name: 'XXX', urlPart: 'XXX'},
];

const orderByArray: Array<SearchArg> = [
    {name: 'Default', urlPart: All},
    {name: 'Time', urlPart: 'time'},
    {name: 'Size', urlPart: 'size'},
    {name: 'Seeders', urlPart: 'seeders'},
    {name: 'Leechers', urlPart: 'leechers'},
];

export const searchArgsValues: { [fieldName: string]: Array<SearchArg>; } = {
    [CATEGORY]: categoryArray,
    [ORDER_BY]: orderByArray,
};

export const userOptionsArgsValues: { [fieldName: string]: Array<SearchArg>; } = {
    showMagnetic: [{name: 'on', urlPart: ''}, {name: 'off', urlPart: ''}],
};

export interface UserOptions {
    userId: number;
    showMagnetic: boolean;
}
