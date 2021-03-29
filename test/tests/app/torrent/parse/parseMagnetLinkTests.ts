import test from 'ava';
import * as fs from 'fs';
import { promisify } from 'util';
import { parseMagnetLink } from '../../../../../src/app/torrent/parse';
import { fakeErrorLog, fakeLog } from '../../../../fakes/fakes';

const readFileAsync = promisify(fs.readFile);

test.cb('parseTorrentTable_good_result', (t) => {
    readFileAsync(__dirname + '/magnetic_link.html')
        .then((html) => {
            parseMagnetLink(fakeLog, fakeErrorLog, {html: html.toString()})
                .then((result) => {
                    t.deepEqual(result.length, 486);
                    t.end();
                });
        });
});
