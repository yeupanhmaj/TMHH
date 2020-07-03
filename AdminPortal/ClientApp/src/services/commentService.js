import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';

export function addComment(data) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`comment/addcomment`, data).then(respone => {
            return resolve(respone)
        }).catch(err => {
            
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};