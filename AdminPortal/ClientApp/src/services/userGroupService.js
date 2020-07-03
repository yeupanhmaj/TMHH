import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';

const prefix = 'usersGroup';
export function GetAllUserWithCondition(
    groupName,
    pageSize, pageIndex) {
    let query = ''
    query = query + 'pageSize=' + pageSize;
    query = query + '&pageIndex=' + pageIndex;
    if (groupName)
        query = query + '&groupName=' + groupName;
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `?${query}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function GetAllUserGroups(pageSize, pageIndex) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `?pageSize=${pageSize}&pageIndex=${pageIndex}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};
export function deleteUserGroups(ids) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix + `?ids=${ids.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

export function deleteUserGroup(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix + `/${id.toString()}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};


export function createUserGroup(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};
export function updateUserGroup(model) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix + `/${model.groupID}`, model).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

export function GetUserGroupById(id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/${id}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};



export function getlstGroupPermission(groupID) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`GroupPermission?groupID=${groupID}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};


export function updateGroupPermission(groupID, data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(`GroupPermission?groupID=${groupID}`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

export function getListGroupOfUser(userID) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + `/listgroup?userID=${userID}`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

export function updatelstGroupOfUser(userID, data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix + `/updatelistgroup?userID=${userID}`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};