import test from 'ava';
import * as fs from 'fs';
import { promisify } from 'util';
import { parseTorrentTable } from '../../../../../src/app/torrent/parse';
import { fakeLog } from './../../../../fakes/fakes';

const readFileAsync = promisify(fs.readFile);

test.cb('parseTorrentTable_good_result', (t) => {
    readFileAsync(__dirname + '/table1.html').then((html) => {
        parseTorrentTable(fakeLog, fakeLog, {html: html.toString()})
            .then((result) => {
                t.deepEqual(result.torrents.length, 20);
                t.end();
            });
    });
});
