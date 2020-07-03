import moment from 'moment';
import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
export function GetAllAuditWithCondition(
    fromDate,
    toDate,
    customerID,
    auditCode,
    quoteCode,
    pageSize, pageIndex) {

    let query = ''
    query=query + 'fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query=query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');
   
    if(quoteCode != undefined && quoteCode.trim() != '')
    query=query + '&quoteCode=' + quoteCode;

    if(customerID != undefined && customerID != 0 )
    query=query + '&customerID=' + customerID;

    if(auditCode != undefined && auditCode.trim() != '')
    query=query + '&auditCode=' + auditCode;



    query=query + '&pageSize='+ pageSize;
    query=query + '&pageIndex='+ pageIndex;

    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`audit/byconditions?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {   
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetAllAudit(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`audit?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`audit?auditIDs=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`audit/${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function createAudit(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`audit`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function createAuditwithAttFiles(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`audit/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateAudit(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`audit/${model.auditID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateAuditwithAttFiles(id ,data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPutFormData(`audit/${id}/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function getMasterData() {
    let GetCustomer = new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`customer?pageSize=${1000}&pageIndex=${0}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
    let GetDepartment = new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`department?pageSize=${1000}&pageIndex=${0}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
    let excute = [GetCustomer, GetDepartment];
    return Promise.all(excute).then(
        (respone) => {
            return respone
        });
};

export function GetAuditById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`audit/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function addComment(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`audit/addcomment`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getListauditCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`audit/getListAuditCode?auditCode=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getAuditByCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`audit/getbycode?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getListEmployee(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`employee/getListEmployee?name=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getListDefaultEmployee() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`audit/GetDefaultMember`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};