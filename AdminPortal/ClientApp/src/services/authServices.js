

import * as ApiCaller from '../libs/httpRequests';
import { store } from '../stores';


export function login(username, password) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost('Auth/signin', {
            userName: username,
            password: password,
        }, false)
            .then(objRespone => {
                if (objRespone.isSuccess ===true) {
                    let token = '';
                    let name = '';
                    let userID = '';
                    let role = '';
                    if (objRespone.token) {
                        token = objRespone.token
                        name = objRespone.name || ''
                        userID = objRespone.userID || ''
                        role = objRespone.role || ''
                    }
                    localStorage.setItem('token', token);
                    localStorage.setItem('name', name);
                    localStorage.setItem('userID', userID);
                    localStorage.setItem('role', role);
                    store.dispatch({ type: 'USER_SET_DATA', data: { token: objRespone.token, name: name, role:objRespone.userID } });
                } else {
                    store.dispatch({
                        type: 'MODAL_OPEN_ERROR_MODAL', errHeader: "Đăng nhập thất bại",
                        errContent: objRespone.err.msgString
                    });

                }
                return resolve(objRespone)

            }).catch(err => {
                store.dispatch({
                    type: 'MODAL_OPEN_ERROR_MODAL', errHeader: "Đăng nhập thất bại",
                    errContent: JSON.stringify(err)
                });
                return reject(err)
            });
    });

}
export function logout() {
    localStorage.setItem('token', '');
    localStorage.setItem('name', '');
    localStorage.setItem('role', '');
    store.dispatch({ type: 'USER_SET_DATA', data: { token: '', name: '', role:'' } });
}
export function checkAuth() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet('Auth/check', true)
        .then(objRespone => {
            if (objRespone.isSuccess ===true) {
            } else {
                store.dispatch({ type: 'USER_SET_DATA', data: { token: '', name: '', role:'' } });
            }
            return resolve(objRespone)
        }).catch(err => {        
            store.dispatch({ type: 'USER_SET_DATA', data: { token: '', name: '', role:'' } });
            return reject(err)
        });
    });
}