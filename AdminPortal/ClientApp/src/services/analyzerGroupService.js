import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';
import moment from 'moment';

const prefix = 'analyzerGroup';
export function GetAllAnalyzerGroupWithCondition(
    analyzerGroupCode,
    quoteCode,
    contractCode,
    fromDate,
    toDate,
    pageSize, pageIndex) {
    let query = ''
    query=query + 'pageSize='+ pageSize;
    query=query + '&pageIndex='+ pageIndex;
    query=query + '&fromDate=' + moment(new Date(fromDate)).format('YYYY-MM-DD, 00:00:00');
    query=query + '&toDate=' + moment(new Date(toDate)).format('YYYY-MM-DD, 23:59:59');
    if(analyzerGroupCode && analyzerGroupCode !== '')
    query=query + '&analyzerGroupCode=' + analyzerGroupCode;
    if(quoteCode && quoteCode !== '')
    query=query + '&quoteCode='+ quoteCode;
    if(contractCode && contractCode !== '')
    query=query + '&contractCode='+ contractCode;



    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {   
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetAllAnalyzerGroup(pageSize,pageIndex)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};
export function deleteAnalyzerGroups(ids)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+`?ids=${ids.toString()}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function deleteAnalyzerGroup(id)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+`/${id.toString()}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};


export function createAnalyzerGroup(model)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix,model).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};
export function updateAnalyzerGroup(model)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix+`/${model.AnalyzerGroupID}`, model).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function GetAnalyzerGroupById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

