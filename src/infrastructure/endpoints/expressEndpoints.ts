import { Request, Response } from 'express';
import { getUrl } from '../../configuration/express';
import { ExpressEndpoint } from '../express/ExpressEndpoint';
import { setTrace } from '../scopeMiddleware';

export const expressEndpoints: Array<ExpressEndpoint> = [
    {
        method: 'get',
        path: getUrl + `:userId/:getId`,
        exec: (getMagneticLink: (id: number, userId: number) => Promise<string>,
               req: Request,
               res: Response) => {
            setTrace(() => {
                const getId = parseInt(req.params.getId, 10);
                const userId = parseInt(req.params.userId, 10);
                if (!getId || !userId) {
                    return res.sendStatus(400);
                }

                getMagneticLink(getId, userId)
                    .then((value) => {
                        if (!value) {
                            return res.sendStatus(400);
                        }
                        res.redirect(value);
                    })
                    .catch((error) => {
                        res.sendStatus(400);
                    });
            });
        },
    },
];
