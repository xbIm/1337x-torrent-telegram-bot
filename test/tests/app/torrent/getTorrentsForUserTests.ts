import test from 'ava';
import * as JsMock from 'js-mock';
import { getTorrentsForUser } from '../../../../src/app/torrent/getTorrentForUser';
import { buildTestDi } from '../../../../src/configuration/di';
import { TorrentTableEntity } from '../../../../src/domain/torrent/Entities';
import { fakeLog } from '../../../fakes/fakes';

test.beforeEach((t) => {
    const container = buildTestDi({
        getTorrentsForUser,
        logTrace: fakeLog,
        getSession: JsMock.mock('getSession'),
    });

    t.context = {...t.context, ...container};
});

const userId = 500;
const id = 1;

test('getTorrentsForUser_recordExists_goodResult', async (t) => {

    const resultTorrent: TorrentTableEntity = {
        id,
        url: '',
        title: '',
        size: '',
        seaders: 0,
        leachers: 0,
        time: '',
        author: '',
    };

    t.context.getSession.once().with(userId).returns(Promise.resolve({
        torrents: [resultTorrent],
    }));

    const result = await t.context.getTorrentsForUser(id, userId);

    t.context.getSession.verify();
    t.deepEqual(resultTorrent, result);
});

test('getTorrentsForUser_noRecord_notFoundError', async (t) => {

    t.context.getSession.once().with(userId).returns(Promise.resolve(null));

    const result = await t.throws(t.context.getTorrentsForUser(id, userId));

    t.context.getSession.verify();
    t.deepEqual('not found', result.message);
});
