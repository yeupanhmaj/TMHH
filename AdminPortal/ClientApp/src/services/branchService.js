import * as ApiCaller from '../libs/httpRequests';

const prefix = 'Branch';


export function GetAllBranch(page,size)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`?page=${page}&size=${size}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function createBranch(model)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix,model).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function editBranch(model)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix,model).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function deleteBranch(id)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+'/'+id).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function deleteManyBranch(ids)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+`?ids=${ids}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function getSearchBranch(query,page,size)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`/search?query=${query}&page=${page}&size=${size}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};