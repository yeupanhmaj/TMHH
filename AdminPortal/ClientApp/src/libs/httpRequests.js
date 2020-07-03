import * as Configure from '../configure/urlConfig';
import * as Action from './actions';

export function httpGet(path, useToken = true) {
    return callApi(path, 'GET', null, useToken, false);
}

export function httpGetDownload(path, filetype, useToken = true) {
    return Download(path);
}

export function httpPost(path, body, useToken = true) {
    return callApi(path, 'POST', body, useToken, false);
}

export function httpPut(path, body, useToken = true) {
    return callApi(path, 'PUT', body, useToken, false);
}

export function httpDelete(path, useToken = true) {
    return callApi(path, 'DELETE', null, useToken, false);
}
export function httpPostFormData(path, body, useToken = true) {
    return callApi(path, 'POST', body, useToken, false, true);
}
export function httpPutFormData(path, body, useToken = true) {
    return callApi(path, 'PUT', body, useToken, false, true);
}

function callApi(path, method, body = null, useToken = true, https = false, isFormData = false) {
    let url = Configure.BASE_URL + path;

    const requestHeaders= new Headers();
    requestHeaders.set('Content-Type', 'application/json');

    if (useToken) {
        let token = '';
        if (useToken) {
            token = localStorage.getItem('token') || '';
        }
        requestHeaders.set('Authorization', 'bearer ' + token);
    }

    let requestInfo;

    switch (method) {
        case 'POST':
        case 'PUT':
            if (isFormData ===false) {
                requestInfo = { method: method, headers: requestHeaders, body: JSON.stringify(body) };
            } else {
                requestHeaders.delete('Content-Type');
                requestInfo = { method: method, headers: requestHeaders, body: body };
            }
            break;
        case 'DELETE':
        case 'GET':
            requestInfo = { method: method, headers: requestHeaders };
            break;
        default:
            break;
    }

    return new Promise((resolve, reject) => {
        fetch(url, requestInfo)
            .then(response => {

                if (response.status !==200) {
                    if (response.status ===401) {
                     //   Action.openMessageDialog("Lỗi phân quyền", "Vui lòng đăng nhập account đúng phân quyền")
                        localStorage.setItem('token', '');
                        localStorage.setItem('name', '');
                        Action.logout();
                        return reject("Vui lòng đăng nhập account đúng phân quyền" )
                    }
                    return response.json().then((obj) => {
                        return reject(getError(obj));
                    })
                }
                return resolve(response.json());
            })
            .catch(ex => {
                if (ex.toString() ===`TypeError: Failed to fetch`) {
                    Action.openMessageDialog("Lỗi kết nối mang", "Hiện tại không thể kết nối đến server vui lòng liên hệ IT")
                    return;
                }
                return reject(ex);
            })
    });
}
function getError(obj) {
    let ret = ""
    for (let record in obj.errors) {
        ret += obj.errors[record] + '@@@@@'
    }

    return ret
}


function Download(path) {
    let url = Configure.BASE_URL + path;
    const requestHeaders= new Headers();
    requestHeaders.set('Content-Type', "text/plain");
    requestHeaders.set('Content-Disposition', 'attachment');
    let token = '';
    token = localStorage.getItem('token') || '';
    requestHeaders.set('Authorization', 'bearer ' + token);

    let requestInfo;

    requestInfo = { method: "GET", headers: requestHeaders };

    return new Promise((resolve, reject) => {
        fetch(url, requestInfo)
            .then(response => {

                if (response.status !==200) {
                    if (response.status ===401) {
                        Action.openMessageDialog("Lỗi phân quyền", "Vui lòng đăng nhập tài đúng phân quyền")
                        localStorage.setItem('token', '');
                        localStorage.setItem('name', '');
                        Action.logout();
                        return reject("Vui lòng đăng nhập tài đúng phân quyền")
                    } else {
                        return response.json().then((obj) => {
                            return reject(getError(obj));
                        })
                    }
                }
                return resolve(response);
            })
            .catch(ex => {
                if (ex.toString() ===`TypeError: Failed to fetch`) {
                    Action.openMessageDialog("Lỗi kết nối mang", "Hiện tại không thể kết nối đến server vui lòng liên hệ IT")
                    return;
                }
                return reject(ex);
            })
    });
}
