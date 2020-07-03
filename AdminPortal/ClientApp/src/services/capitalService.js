import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';

export function GetAllCapital() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`capital`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`capital?IDs=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`capital/${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function createCapital(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`capital`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function updateCapital(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`capital/${model.capitalID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetCapitalById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`capital/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function GetAllCapitalRole() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`capitalRole`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function GetCapitalByName(name) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`capital/getbyname?name=${name}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
