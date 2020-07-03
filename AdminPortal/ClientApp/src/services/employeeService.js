import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';



export function GetAllEmployee() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`employee`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`employee?IDs=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`employee/${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function createEmployee(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`employee`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function updateEmployee(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`employee/${model.employeeID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetEmployeeById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`employee/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function GetAllEmployeeRole() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`employeeRole`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getMasterData() {
    let GetEmployeeRole = new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`employeeRole`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
    let excute = [GetEmployeeRole];
    return Promise.all(excute).then(
        (respone) => {
            return respone
        });
};

export function SearchEmployee(keySearch) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`employee/GetListEmployee?name=${keySearch}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};