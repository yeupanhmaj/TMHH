



import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as DecisionService from '../../../services/decisionService';
import * as NegotiationService from '../../../services/negotiationService';

export function init(updateItemDefine, updateItem, proposalCode, listData,item) {
    let itemDefines = CONST_FEATURE.Decision.itemDef;
    if (item) updateItem(item)
    itemDefines.props[2].cbFunc = (code, item) => {
        NegotiationService.getNegotiatioByCode(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                let selectedItem = item
                selectedItem.proposalID = objRespone.item.proposalID;
                selectedItem.proposalCode = objRespone.item.proposalCode;
                selectedItem.departmentName = objRespone.item.departmentName;
                selectedItem.items = objRespone.item.items;
                selectedItem.isVAT = objRespone.item.isVAT;
                selectedItem.vatNumber = objRespone.item.vatNumber;
                selectedItem.bidMethod = +objRespone.item.bidMethod;
                selectedItem.decisionCode = "QD-" + objRespone.item.proposalCode;
                updateItem(selectedItem)
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
        })
    }
    itemDefines.props[2].values = []
    for (let record of listData.lstNegotiation) {
        let item = { label: record.code, value: record.code };
        itemDefines.props[2].values.push(item);
    }
    itemDefines.props[2].type = "select";
    updateItemDefine(itemDefines);
}


export function DecisionModify(item, files, callBack) {

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
    if (item["decisionID"] === undefined) {
        DecisionService.createDecisionwithAttFiles(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    } else {
        DecisionService.updateDecisionwithAttFiles(item["decisionID"], data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone,'edit');
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    }
}