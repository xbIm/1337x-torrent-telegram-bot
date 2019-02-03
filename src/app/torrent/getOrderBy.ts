import { All, ORDER_BY } from '../../domain/torrent/Consts';
import { SearchArgs } from '../../domain/torrent/Entities';

export function getOrderBy(logTrace: (message: string) => void,
                           searchArg: SearchArgs): string {
    if (searchArg == null || searchArg[ORDER_BY] === All) { return null; }

    logTrace('searchArg.sortBy:' + searchArg[ORDER_BY]);
    return searchArg[ORDER_BY];
}
