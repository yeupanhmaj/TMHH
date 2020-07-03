import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as AcceptanceService from '../../../services/acceptanceService';
import * as DeliveryReceiptService from '../../../services/deliveryReceiptService';

export function init(updateItemDefine, updateItem, proposalCode, listData, item) {
    let itemDefines = CONST_FEATURE.Acceptance.itemDef;
    itemDefines.props[3].cbFunc = (code, item) => {
        DeliveryReceiptService.GetDeliveryReceiptById(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                let selectedItem = {} 
                if(item)selectedItem = item

                selectedItem.proposalID = objRespone.item.proposalID;
                selectedItem.proposalCode = objRespone.item.proposalCode;
                selectedItem.departmentName = objRespone.item.departmentName;
                selectedItem.items = objRespone.item.items;
                selectedItem.acceptanceCode = 'NT-' + objRespone.item.proposalCode;
                updateItem(selectedItem)
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
        })
    }
    itemDefines.props[3].values = []
    itemDefines.props[3].type = "select";
    itemDefines.props[3].isDisable = false;

    itemDefines.props[2].type = "input";
    itemDefines.props[2].isDisable = true;

    for (let record of listData.lstDeliveryReceipt) {
        let item = { label: record.code, value: record.id };
        itemDefines.props[3].values.push(item);
    }

    updateItemDefine(itemDefines);
}

export function AcceptanceModify(item, files, callBack) {
    item = item ;

    var data = new FormData();
    if (files.length > 0) {
        for (let file of files) {
            data.append('files', file);
        }
    }

    Object.keys(item).forEach(key => {
        if (key !== 'items')
            data.append(key, item[key]);
    });
    for (let i = 0; i < item.items.length; i++) {
        for (let key in item.items[i]) {
            data.append("items[" + i + "]." + key, item.items[i][key]);
        }
    }

    if (item["acceptanceID"] === undefined) {
        AcceptanceService.createAcceptance(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    } else {
        AcceptanceService.updateAcceptance(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone,'edit');
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    }
} 