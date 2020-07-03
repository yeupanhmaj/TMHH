import * as ApiCaller from '../libs/httpRequests';

const prefix = 'Confirm';

export function createConfirm(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};