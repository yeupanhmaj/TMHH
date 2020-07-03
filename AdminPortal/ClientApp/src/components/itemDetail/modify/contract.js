



import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as ContractService from '../../../services/contractService';
import * as DecisionService from '../../../services/decisionService';

export function init(updateItemDefine, updateItem, proposalCode, listData,item) {
    let itemDefines = CONST_FEATURE.Contract.itemDef;
    if (item) updateItem(item)
    itemDefines.props[2].cbFunc = (code, item) => {
        DecisionService.getDecisionByCode(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                let selectedItem = item
                selectedItem.proposalID = objRespone.item.proposalID;
                selectedItem.proposalCode = objRespone.item.proposalCode;
                selectedItem.departmentName = objRespone.item.departmentName;
                selectedItem.items = objRespone.item.items;
                selectedItem.isVAT = objRespone.item.isVAT;
                selectedItem.vatNumber = objRespone.item.vatNumber;
                selectedItem.contractCode = "HD-" + objRespone.item.proposalCode;
                updateItem(selectedItem)
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
        })
    }
    itemDefines.props[2].values = []
    for (let record of listData.lstDecision) {
        let item = { label: record.code, value: record.code };
        itemDefines.props[2].values.push(item);
    }
    itemDefines.props[2].type = "select";
    updateItemDefine(itemDefines);
}

export function ContractModify(item, files, callBack) {
    item = item ;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');
    var data = new FormData();
    if (files.length > 0) {
        for (let file of files) {
            data.append('files', file);
        }
    }

    Object.keys(item).forEach(key => {
        data.append(key, item[key]);
    });

    // item.status = +item.status;
    if (item["contractID"] ===undefined) {
        ContractService.createContractwithAttFiles(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    } else {
        ContractService.updateContractwithAttFiles(item["contractID"], data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone,'edit');
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    }
}