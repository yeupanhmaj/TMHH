import * as ApiCaller from '../libs/httpRequests';

const prefix = 'Customer';

export function GetAllCustomer(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};