import * as cheerio from 'cheerio';
import { Response } from '../../domain/CommonEntities';
import { ParseResult, SearchResult, Session, TorrentTableEntity } from '../../domain/torrent/Entities';
import { TorrentConfig } from '../../domain/torrent/TorrentConfig';

export function parseTorrentTable(logDebug: (message: string) => void,
                                  logWarn: (message: string) => void,
                                  response: Response): Promise<ParseResult> {
    try {
        return Promise.resolve(parse(logDebug, logWarn, response));
    } catch (er) {
        return Promise.reject(er);
    }
}

export function formatTorrentTable(session: Session, config: TorrentConfig): Promise<SearchResult> {
    if (session.torrents == null || session.torrents.length === 0) {
        return Promise.resolve({
            text: 'No results were returned. Please refine your search.',
            currentPosition: session.currentPosition,
        });
    }

    const result: SearchResult = {
        text: 'The result of your search \n',
        currentPosition: session.currentPosition,
    };

    const collection = session.torrents.slice(session.currentPosition, session.currentPosition + config.amountPerPage).reverse();

    let i = collection.length - 1;
    for (const elem of collection) {
        result.text += `${(i + 1).toString()}. Name : ${elem.title}\nTime: ${elem.time}\nAuthor : ${elem.author} \n` +
            `S/L: ${elem.seaders.toString()}/${elem.leachers.toString()}  Size: ${elem.size}\n`
            + `click for link: /get${elem.id}\n\n`;

        i--;
    }
    result.prev = (session.currentPosition !== 0) ? 'prev' : null;
    result.next = session.next || (session.torrents.length > session.currentPosition + config.amountPerPage ? 'next' : null);

    return Promise.resolve(result);
}

export function parseMagnetLink(logTrace: (message: string) => void,
                                response: Response): Promise<string> {
    try {
        const $ = cheerio.load(response.html);

        const result = $('.btn-wrap-list').find('a').first().prop('href');
        logTrace('magnet link:' + result);
        if (result) {
            return Promise.resolve(result);
        } else {
            return Promise.reject(Error('magnetik li is empty'));
        }
    } catch (er) {
        return Promise.reject(er);
    }
}

function parse(logDebug: (message: string) => void,
               logWarn: (message: string) => void,
               response): ParseResult {

    const $ = cheerio.load(response.html);

    const torrents: Array<TorrentTableEntity> = [];
    let id = 0;
    $('.table-list tbody')
        .find('tr')
        .each((index, item) => {

            let td = $(item).find('td:nth-child(1)');
            const anchor = td.find('a:nth-child(2)');
            td = td.next();
            const seaders = parseInt(td.text(), 10);
            td = td.next();
            const leachers = parseInt(td.text(), 10);
            td = td.next();
            const time = td.text();
            td = td.next();
            const size = td.contents().get(0).nodeValue;
            td = td.next();
            id++;
            torrents.push({
                id,
                url: anchor.prop('href'),
                title: anchor.text(),
                size,
                seaders,
                leachers,
                time,
                author: td.find('a').text(),
            });
        });

    const result: ParseResult = {
        torrents,
    };

    logDebug('search result length: ' + result.torrents.length);
    const activeLi = $('.pagination .active');

    return (activeLi.length > 0) ?
        {
            ...result, ...{
                prev: activeLi.prev().length > 0 ? activeLi.prev().children('a').prop('href') : null,
                next: activeLi.next().length > 0 ? activeLi.next().children('a').prop('href') : null,
            },
        } : {
            ...result, ...{
                prev: null,
                next: null,
            },
        };
}
