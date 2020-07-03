import moment from 'moment';
import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
export function GetAllContractWithCondition(
    fromDate,
    toDate,
    quoteCode,
    customerID,
    contractCode,
    pageSize, pageIndex) {
    //prepare parametter
    let query = ''
    query=query + 'fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query=query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');

    if(quoteCode != undefined && quoteCode.trim() != '')
    query=query + '&quoteCode=' + quoteCode;

    if(customerID != undefined && customerID != 0 )
    query=query + '&customerID=' + customerID;

    if(contractCode != undefined && contractCode.trim() != '')
    query=query + '&contractCode=' + contractCode;
    query=query + '&pageSize='+ pageSize;
    query=query + '&pageIndex='+ pageIndex;

    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`contract/byconditions?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {   
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetAllContract(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`contract?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`contract?contractIDs=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`contract/${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function createContract(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`contract`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function createContractwithAttFiles(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`contract/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateContract(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`contract/${model.contractID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateContractwithAttFiles(id ,data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPutFormData(`contract/${id}/withDFile`, data).then(respone => {
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

export function GetContractById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`contract/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function addComment(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`contract/addcomment`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getListcontractCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`contract/getListcontractCode?contractCode=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getContractByCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`contract/getbycode?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};



export function GetPrintDetails(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`contract/print/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getSelectContract(code ) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`contract/getItems?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


