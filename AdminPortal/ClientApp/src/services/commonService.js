import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';

export function GetAllDepartment() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`department?pageSize=${1000}&pageIndex=${0}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetCustomerById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`customer/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetAllCustomer() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`customer?pageSize=${1000}&pageIndex=${0}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function GetCustomerByBidPlanId(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`customer/bybidplancode?bidplancode=${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetListItemByCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`items/getListItem?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function GetListCategory(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`Catogery`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function creatItem(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`items`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
