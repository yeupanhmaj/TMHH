import moment from 'moment';
import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
export function GetAllNegotiationWithCondition(
    fromDate,
    toDate,
    negotiationCode,
    quoteCode,
    customerID,
    pageSize, pageIndex) {

    //prepare parametter

    let query = ''
    query=query + 'fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query=query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');
    if(quoteCode != undefined && quoteCode.trim() != '')
    query=query + '&quoteCode=' + quoteCode;

    if(customerID != undefined && customerID != 0 )
    query=query + '&customerID=' + customerID;

    if(negotiationCode != undefined && negotiationCode.trim() != '')
    query=query + '&negotiationCode=' + negotiationCode;
    
    query=query + '&pageSize='+ pageSize;
    query=query + '&pageIndex='+ pageIndex;


    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`negotiation/byconditions?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {   
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetAllNegotiation(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`negotiation?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`negotiation?negotiationIDs=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`negotiation/${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function createNegotiation(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`negotiation`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function createNegotiationwithAttFiles(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`negotiation/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateNegotiation(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`negotiation/${model.negotiationID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateNegotiationwithAttFiles(id ,data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPutFormData(`negotiation/${id}/withDFile`, data).then(respone => {
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

export function GetNegotiationById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`negotiation/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function addComment(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`negotiation/addcomment`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getListnegotiationCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`negotiation/getListNegotiationByCode?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getNegotiatioByCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`negotiation/getbycode?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetPrintDetails(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`negotiation/print/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
