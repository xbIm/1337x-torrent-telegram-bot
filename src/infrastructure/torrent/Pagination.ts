import * as TelegramBot from 'node-telegram-bot-api';
import { Direction, PaginationRequest, SearchResult } from '../../domain/torrent/Entities';

export function onPrevNext(
    logInfo: (message: string) => void,
    logTrace: (message: string) => void,
    logError: (error: Error) => void,
    getOtherPage: (data: PaginationRequest, userId: number, messageId: number) => Promise<SearchResult>,
    editMessageText: (text: string, options?: TelegramBot.EditMessageTextOptions) => Promise<TelegramBot.Message | boolean | Error>,
    bbot: TelegramBot,
    message: any,
    data: string): Promise<void> {

    return Promise.resolve()
        .then(() => {

            logInfo(`onPrevNext has called with chatId:${message.from.id} and name:${message.from.username} and data=${message.text}`);

            const dataParsed = PaginationRequest.fromData(data);
            return getOtherPage(dataParsed, message.chat.id, message.message_id)
                .then((searchResult) => {

                    const prevNext = [];
                    if (searchResult.prev) {
                        prevNext.push([{
                            text: `Previous`,
                            callback_data: new PaginationRequest(Direction.Prev, searchResult.currentPosition).toData(),
                        }]);
                    }
                    if (searchResult.next) {
                        prevNext.push([{
                            text: `Next`,
                            callback_data: new PaginationRequest(Direction.Next, searchResult.currentPosition).toData(),
                        }]);
                    }

                    const editOptions = {
                        chat_id: message.chat.id,
                        message_id: message.message_id,
                        reply_markup: {
                            inline_keyboard: prevNext,
                        },
                    };
                    logTrace(`value.text.length: ${searchResult.text.length}`);
                    return editMessageText(searchResult.text, editOptions);
                })
                .then(() => {
                    logInfo(`onPrevNext finish with success`);
                    return Promise.resolve(true);
                })
                .catch((error: Error) => {
                    if (error.name === 'UserError') {
                        logInfo('UserError=' + error.message);
                    } else {
                        logError(error);
                    }

                    editMessageText('error', {
                        chat_id: message.chat.id,
                        message_id: message.message_id,
                    });
                    return Promise.resolve(null);
                });
        });
}
