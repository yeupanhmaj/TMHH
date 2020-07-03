import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';


export function upload(file) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPostFormData(`fakeApiUsers/upload?preferId=50`, file).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};


export function dowload(feature, preferId, fileName, filetype) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGetDownload(`Files/download?feature=${feature}&preferId=${preferId}&fileName=${fileName}`,filetype).then((respone ) => {
            return resolve(respone)
        }).catch((err) => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};



export function dowloadTemplate(feature) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGetDownload(`Files/DownloadTemplate?feature=${feature}`, "").then((respone ) => {
            return resolve(respone)
        }).catch((err) => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};

export function deleteAttachfiles(ids)  {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(`document/?documentIDs=${ids.toString()}`).then(respone =>{
            return resolve(respone) 
        }).catch(err =>{
            return reject(err) 
        })
    });
};


export function dowloadDoc(feature,id) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGetDownload(`Files/DownloadDocFile?feature=${feature}&id=${id}`, "").then((respone ) => {
            return resolve(respone)
        }).catch((err) => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};