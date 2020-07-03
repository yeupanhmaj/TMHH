import moment from 'moment';
import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
const prefix = 'acceptance';
export function GetAllAcceptanceWithCondition(
    proposaCode,
    departmentID,
    fromDate,
    toDate,
    pageSize, pageIndex){
        if(!departmentID) departmentID = 0;
    //prepare parametters
    let query = ''
  
    query=query + 'departmentID=' + departmentID;
    if(proposaCode !=='')
    query=query + '&proposalCode=' + proposaCode;
    query=query + '&fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query=query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');
    query=query + '&pageSize='+ pageSize;
    query=query + '&pageIndex='+ pageIndex;


    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`${prefix}/?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {   
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetAllAcceptance(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`${prefix}?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`${prefix}/?IDs=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`${prefix}/${id.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function createAcceptance(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`${prefix}`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function updateAcceptance(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPutFormData(`${prefix}`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetAcceptanceById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`${prefix}/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function addComment(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`${prefix}/addcomment`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};