import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
import moment from 'moment';

const prefix = 'analyzer';
export function GetAllAnalyzerWithCondition(
    analyzerCode,
    departmentID,
    customerID,
    fromDate,
    toDate,
    pageSize, pageIndex) {
    let query = ''
    query = query + 'pageSize=' + pageSize;
    query = query + '&pageIndex=' + pageIndex;
    query = query + '&fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query = query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');
    if (analyzerCode && analyzerCode !== '')
        query = query + '&analyzerCode=' + analyzerCode;
    if (departmentID && departmentID !== '')
        query = query + '&departmentID=' + departmentID;
    if (customerID && customerID !== '')
        query = query + '&customerID=' + customerID;



    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/byconditions?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetAllAnalyzer(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};
export function deleteAnalyzers(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix + `?ids=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

export function deleteAnalyzer(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix + `/${id.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};


export function createAnalyzer(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};
export function updateAnalyzer(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix + `/${model.analyzerID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

export function GetAnalyzerById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function GetNumberAnalyzerByDate(fromDate, toDate, departmentName, customerName) {
    let query = '';
    query = query + 'fromDate=' + fromDate;
    query = query + '&toDate=' + toDate;
    query = query + '&departmentName=' + departmentName;
    query = query + '&customerName=' + customerName;
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/analyzerByDate?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

