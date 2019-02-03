import test from 'ava';
import * as fs from 'fs';
import { promisify } from 'util';
import { parseMagnetLink } from '../../../../../src/app/torrent/parse';
import { fakeLog } from '../../../../fakes/fakes';

const readFileAsync = promisify(fs.readFile);

test.cb('parseTorrentTable_good_result', (t) => {
    readFileAsync(__dirname + '/magnetic_link.html')
        .then((html) => {
            parseMagnetLink(fakeLog, {html: html.toString()})
                .then((result) => {
                    t.deepEqual(result.length, 1014);
                    t.end();
                });
        });
});

test.cb('parseTorrentTable_empty_result', (t) => {
    readFileAsync(__dirname + '/magnetic_link_empty.html')
        .then((html) => {
                parseMagnetLink(fakeLog, {html: html.toString()})
                    .catch((empty: Error) => {
                        t.true(!!empty.message);
                        t.end();
                    });
            },
        );
});
