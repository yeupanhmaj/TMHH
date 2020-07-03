


import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as ProposalService from '../../../services/proposalService';


export function init(updateItemDefine, updateItem, proposalCode, listData, item, getcurrent) {
    let itemDefines = CONST_FEATURE.Proposal.itemDef;

    updateItemDefine(itemDefines)
    if(item)updateItem(item)
    
}

export function ProposalModify(item, files, callBack) {
    item = item ;
    item.departmentID = +item.departmentID;
    item.proposalType = +item.proposalType;
    item.curDepartmentID = +item.curDepartmentID;
    item.status = +item.status;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');

    var data = new FormData();
    if (files.length > 0) {
        for (let file of files) {
            data.append('files', file);
        }
    }

    Object.keys(item).forEach(key => {
        if (key !=='items')
            data.append(key, item[key]);
    });
    for (let i = 0; i < item.items.length; i++) {
        for (let key in item.items[i]) {
            data.append("items[" + i + "]." + key, item.items[i][key]);
        }
    }

    if (item[CONST_FEATURE.Proposal.KEY_COLUMN] === undefined) {
        ProposalService.createProposalwithAttFiles(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {

                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {

            })
    } else {
        if(item.listDocument)
        {
            for (let i = 0; i < item.listDocument.length; i++) {
                for (let key in item.listDocument[i]) {
                    data.append("documents[" + i + "]." + key, item.listDocument[i][key]);
                }
            }
        }
        ProposalService.updateProposalwithAttFiles(item["proposalID"], data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone,'edit');
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {
            })
    }
}




