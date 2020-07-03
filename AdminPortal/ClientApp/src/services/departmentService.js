import * as ApiCaller from '../libs/httpRequests';

const prefix = 'Department';

export function GetAllDepartment(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};