import moment from 'moment';
import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
export function GetAllProposalWithCondition(
    proposaType,
    departmentID,
    proposalCode,
    status,
    fromDate,
    toDate,
    item,
    pageSize, pageIndex) {

    //prepare parametter
    if (!departmentID) departmentID = 0;
    let query = ''
    query = query + 'departmentID=' + departmentID;
    if (proposaType)
        query = query + '&proposalType=' + proposaType;
    if (status)
        query = query + '&status=' + status;
    if (proposalCode)
        query = query + '&proposalCode=' + proposalCode;
    if (item)
        query = query + '&item=' + item;
    query = query + '&fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query = query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');
    query = query + '&pageSize=' + pageSize;
    query = query + '&pageIndex=' + pageIndex;


    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/byconditions?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetAllProposal(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`proposal?proposalIDs=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`proposal/${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function createProposal(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`proposal`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function createProposalwithAttFiles(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`proposal/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {

            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateProposal(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`proposal/${model.proposalID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {

            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateProposalwithAttFiles(id, data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPutFormData(`proposal/${id}/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function getMasterData() {
    let GetProposalType = new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposaltype?pageSize=${1000}&pageIndex=${0}`).then(respone => {
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
    let excute = [GetProposalType, GetDepartment];

    return Promise.all(excute).then(
        (respone) => {
            return respone
        });
};

export function GetProposalById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetProposalByCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getbycode?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getListprosalCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getListProposalCode?proposalCode=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function getListProposalCanCreateContract(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getListProposalCanCreateContract?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getListProsalHaveAcceptance(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getListProsalHaveAcceptance?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getListItem(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`items/getListItem?name=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getProposalWithContactItemsByCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getProposalWithContactItemsByCode?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getRelateData(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getRelateData?id=${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};



export function GetItemWithCondition(proposalCode, itemName, isHasQuote) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getProposalCanCreateQuote?proposalCode=${proposalCode}&itemName=${itemName} 
        &isHasQuote=${isHasQuote}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getDetailsForDR(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getDetailsForDR?id=${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getDetailsAcceptanceByProposalID(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getDetailsAcceptanceByProposalID?id=${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getListProsalCanCreateDR(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/getListProsalCanCreateDR?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};