import * as Actions from '../libs/actions';
import * as ApiCaller from '../libs/httpRequests';

const prefixOutDate = 'ProposalReport';
const prefixByDepartment = 'GetByDepartment'

export function getReportCountStaus() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(`proposal/countByStatus`).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function getOutDateProposal() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefixOutDate).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function getProposalByDepartment() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefixOutDate + '/byDepartment').then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function getProposalExceedReserve() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefixOutDate + '/isExceedReserve').then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};
export function getProposalProccess(proposalID) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefixOutDate + '/proccess?proposalID=' + proposalID ).then(respone => {
            return resolve(respone)
        }).catch(err => {
            Actions.openMessageDialog("Error", err.toString());
            return reject(err)
        })
    });
};