import { format } from '../../common';
import { Response } from '../../domain/CommonEntities';
import {
    All,
    BASE_PATH,
    BASE_PATH_CATEGORY,
    BASE_PATH_CATEGORY_ORDER_BY,
    BASE_PATH_ORDER_BY,
    ELEM_PATH,
} from '../../domain/torrent/Consts';
import { ParseResult, RequestArgs, SearchArgs, SearchResult, Session, TorrentTableEntity } from '../../domain/torrent/Entities';
import { TorrentConfig } from '../../domain/torrent/TorrentConfig';
import { getOrderBy } from './getOrderBy';
import { formatTorrentTable } from './parse';

export function searchTorrents(
    logTrace: (message: string) => void,
    logDebug: (message: string) => void,
    traceObject: (message: string, obj: any) => void,
    getSearchArgs: (userId: number) => Promise<SearchArgs>,
    searchFullByUrl: (requestArgs: RequestArgs, userId: number, messageId: number) => Promise<SearchResult>,
    userId: number,
    messageId: number,
    searchText: string,
): Promise<SearchResult> {
    return getSearchArgs(userId)
        .then((searchArgs) => {
            traceObject('current searchArgs:', searchArgs);

            const category = getCategory(logTrace, searchArgs);
            const orderBy = getOrderBy(logTrace, searchArgs);
            const requestArgs = formatUrl(searchText, category, orderBy);

            return searchFullByUrl(requestArgs, userId, messageId);
        });
}

export function searchFullByUrl(
    request: (url: string) => Promise<Response>,
    parseTorrentTable: (response: Response) => Promise<ParseResult>,
    saveSession: (session: Session) => Promise<Session>,
    torrentConfig: TorrentConfig,
    requestArgs: RequestArgs,
    userId: number,
    messageId: number): Promise<SearchResult> {

    return request(requestArgs.url)
        .then(parseTorrentTable)
        .then((parseResult) => {
            return Promise.resolve(
                {
                    ...parseResult,
                    ...{
                        userId,
                        messageId,
                        currentPosition: 0,
                    },
                },
            );
        })
        .then(saveSession)
        .then((session) => formatTorrentTable(session, torrentConfig));
}

export function getMagneticLink(
    request: (url: string) => Promise<Response>,
    getTorrentsForUser: (id: number, userId: number) => Promise<TorrentTableEntity>,
    parseMagnetLink: (response: Response) => Promise<string>,
    id: number,
    userId: number): Promise<string> {

    return getTorrentsForUser(id, userId)
        .then((torrentTableEntity) => {
            return Promise.resolve(format(ELEM_PATH, [torrentTableEntity.url]));
        })
        .then(request)
        .then(parseMagnetLink);
}

function getCategory(logTrace: (message: string) => void,
                     searchArg: SearchArgs): string {
    if (searchArg == null || searchArg.category === All) {
        return null;
    }

    logTrace('searchArg.category:' + searchArg.category);
    return searchArg.category;
}

function formatUrl(searchText: string, category: string, sortBy: string): RequestArgs {

    if (sortBy && category) {
        return {
            url: format(BASE_PATH_CATEGORY_ORDER_BY, [searchText.replace(' ', ' +'), category, sortBy]),
            elemNumbers: 40,
        };
    }

    if (category) {
        return {
            url: format(BASE_PATH_CATEGORY, [searchText.replace(' ', ' +'), category]),
            elemNumbers: 40,
        };
    }

    if (sortBy == null) {
        return {url: format(BASE_PATH, [searchText.replace(' ', ' +')]), elemNumbers: 20};
    }

    return {url: format(BASE_PATH_ORDER_BY, [searchText.replace(' ', '+'), sortBy]), elemNumbers: 20};
}
