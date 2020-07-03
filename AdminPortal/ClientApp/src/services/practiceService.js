import * as ApiCaller from '../libs/httpRequests';

const prefix = 'Practice';


export function GetAlllPractice()  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};
export function GetPractice(id)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`?UserID=${id}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};
export function createUser(model)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix,model).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};
export function updateUser(model)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix,model).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function deleteUser(id)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+`/${id}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};
export function deleteAllUser(ids)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+`?ids=${ids}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};