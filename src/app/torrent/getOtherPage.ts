import { format } from '../../common';
import { Response } from '../../domain/CommonEntities';
import { ELEM_PATH } from '../../domain/torrent/Consts';
import {
    Direction,
    PaginationRequest,
    ParseResult,
    RequestArgs,
    SearchResult,
    Session,
} from '../../domain/torrent/Entities';
import { TorrentConfig } from '../../domain/torrent/TorrentConfig';
import { formatTorrentTable } from './parse';

export default function getOtherPage(
    getSession: (userId: number) => Promise<Session>,
    searchFullByUrl: (requestArgs: RequestArgs, userId: number) => Promise<SearchResult>,
    saveSession: (session: Session) => Promise<Session>,
    parseTorrentTable: (response: Response, startIndex: number) => Promise<ParseResult>,
    request: (url: string) => Promise<Response>,
    logDebug: (message: string) => void,
    torrentConfig: TorrentConfig,
    data: PaginationRequest,
    userId: number,
    messageId: number,
): Promise<SearchResult> {
    return getSession(userId)
        .then((session) => {
            return validateMessageId(session, messageId);
        })
        .then((session) => pagination(session, data, torrentConfig, request, saveSession, parseTorrentTable));
}

function validateMessageId(session: Session, messageId: number) {
    if (session.messageId > messageId) {
        return Promise.reject(Error('session expired'));
    }
    return Promise.resolve(session);
}

function pagination(session: Session, data: PaginationRequest, torrentConfig: TorrentConfig,
                    request: (url: string) => Promise<Response>,
                    saveSession: (session: Session) => Promise<Session>,
                    parseTorrentTable: (response: Response, startIndex: number) => Promise<ParseResult>): Promise<SearchResult> {
    if (data.direction === Direction.Next) {
        const currentPosition = data.currentPosition + torrentConfig.amountPerPage;
        if (session.torrents.length < currentPosition + torrentConfig.amountPerPage) {
            return formatTorrentTable({
                ...session,
                currentPosition,
            }, torrentConfig);
        } else {
            return request(format(ELEM_PATH, [session.next]))
                .then((response) => parseTorrentTable(response, session.torrents.length))
                .then((parseResult) => {
                    return Promise.resolve({
                        ...session,
                        torrents: session.torrents.concat(parseResult.torrents),
                        prev: parseResult.prev,
                        next: parseResult.next,
                    });
                })
                .then(saveSession)
                .then((newSession) => {
                    return formatTorrentTable({
                        ...newSession,
                        currentPosition,
                    }, torrentConfig);
                });
        }
    } else {
        const currentPosition = data.currentPosition - torrentConfig.amountPerPage;
        return formatTorrentTable({
            ...session,
            currentPosition,
        }, torrentConfig);
    }
    return Promise.reject('failed');
}
