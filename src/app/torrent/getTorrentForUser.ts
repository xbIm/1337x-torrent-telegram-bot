import { Session, TorrentTableEntity } from '../../domain/torrent/Entities';

export function getTorrentsForUser(
    getSession: (userId: number) => Promise<Session>,
    logTrace: (message: string) => void,
    id: number,
    userId: number): Promise<TorrentTableEntity> {

    return getSession(userId)
        .then((res) => {

            logTrace('getSession');
            if (res == null) {
                return Promise.reject(Error('not found'));
            }

            for (const elem of res.torrents) {
                if (elem.id === id) {
                    return Promise.resolve(elem);
                }
            }
        });
}
