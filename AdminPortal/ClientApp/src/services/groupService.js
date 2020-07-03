import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';

const prefix = 'groups';
export function GetAllUserWithCondition(
    proposaCode,
    departmentID,
    pageSize, pageIndex) {
        if(!departmentID) departmentID = 0;
    //prepare parametters
    let query = ''
  
    query=query + 'departmentID=' + departmentID;
    if(proposaCode !='')
    query=query + '&proposalCode=' + proposaCode;
    query=query + '&pageSize='+ pageSize;
    query=query + '&pageIndex='+ pageIndex;


    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`/byconditions?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {   
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetAllUsers(pageSize,pageIndex)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};
export function deleteUsers(ids)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+`?ids=${ids.toString()}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function deleteUser(id)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+`/${id.toString()}`).then(respone =>{
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
        return ApiCaller.httpPut(prefix+`/${model.userID}`, model).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};

export function GetUserById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+`/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};