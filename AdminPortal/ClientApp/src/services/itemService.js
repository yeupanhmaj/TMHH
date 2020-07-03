import * as ApiCaller from '../libs/httpRequests';

const prefix = 'Items';


export function GetAllItems() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + '/GetListItem').then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};