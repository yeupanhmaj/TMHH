import moment from 'moment';
import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
export function GetAllExplanationWithCondition(
    proposaCode,
    departmentID,
    fromDate,
    toDate,
    pageSize, pageIndex) {
        if(!departmentID) departmentID = 0;
    let query = ''
  
    query=query + 'departmentID=' + departmentID;
    if(proposaCode !=='')
    query=query + '&proposalCode=' + proposaCode;
    query=query + '&fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query=query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');
    query=query + '&pageSize='+ pageSize;
    query=query + '&pageIndex='+ pageIndex;

    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`explanation/byconditions?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {   
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetAllExplanation(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`explanation?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`explanation?ids=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`explanation/${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function createExplanation(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`explanation`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function createExplanationwithAttFiles(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`explanation/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateExplanation(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`explanation/${model.explanationID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateExplanationwithAttFiles(id ,data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPutFormData(`explanation/${id}/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetExplanationById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`explanation/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function addComment(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`explanation/addcomment`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};