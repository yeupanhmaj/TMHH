import moment from 'moment';
import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
export function GetAllQuoteWithCondition(
    fromDate,
    toDate,
    proposalCode,
    customerID,
    quoteCode,
    pageSize, pageIndex) {
    let query = ''
    query=query + 'fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query=query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');


    if(proposalCode != undefined && proposalCode != '')
    query=query + '&proposalCode=' + proposalCode;
    if(customerID != undefined && customerID != '')
    query=query + '&customerID=' + customerID;
    if(quoteCode != undefined && quoteCode != '')
    query=query + '&quoteCode=' + quoteCode;
    
    query=query + '&pageSize='+ pageSize;
    query=query + '&pageIndex='+ pageIndex;


    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/byconditions?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {   
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function GetAllQuote(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecords(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`quote?quoteIDs=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function deleteRecord(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`quote/${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function createQuote(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`quote`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function createQuotewithAttFiles(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`quote/withDFile`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateQuote(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`quote/${model.quoteID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function updateQuotewithAttFiles(id ,data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPutFormData(`quote/${id}/withDFile`, data).then(respone => {
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

export function GetQuoteById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function addComment(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`quote/addcomment`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getListquoteCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getListQuoteCode?quoteCode=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getQuoteByCode(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getbycode?code=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function insetQuote(data ) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`quote/createQuoteWithMutilProposal`,data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getExits(data ) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`quote/getExitQuote`,data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};



export function importData(file ,quoteID  , customerID ) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`quote/upload?quoteID=${quoteID}&customerID=${customerID}`, file).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};




export function getItems(quoteID  , customerID ) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getItems?quoteID=${quoteID}&customerID=${customerID}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function updateQuoteNew(quoteID ,  data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`quote/updatenew/${quoteID}`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function selectQuote(quoteID ,  customerID) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(`quote/selectQuote`, {quoteID, customerID }).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};



export function GetItemWithCondition(searchText ,isHasAudit) {
    return new  Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/searchQuoteCanCreateAudit?text=${searchText}&&isHasAudit=${isHasAudit}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetQuoteItemWithCondition(searchText ,isHasAudit) {
    return new  Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/searchQuoteItem?text=${searchText}&&isHasAudit=${isHasAudit}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getQuoteInfo(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getQuoteInfo?QuoteID=${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getListquoteCodeContract(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getListQuoteCode?quoteCode=${code}&isContract=true`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getListquoteCodeCanCreateBiplan(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getListquoteCodeCanCreateBiplan?quoteCode=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function getListquoteCodeCanCreateNegotiation(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getListquoteCodeCanCreateNegotiation?quoteCode=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getListquoteCodeCanCreateDecision(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getListquoteCodeCanCreateDecision?quoteCode=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function getListquoteCodeCanCreateContract(code) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`quote/getListquoteCodeCanCreateContract?quoteCode=${code}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

