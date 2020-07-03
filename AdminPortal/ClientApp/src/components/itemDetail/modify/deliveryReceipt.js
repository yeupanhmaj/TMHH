



import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as DeliveryReceiptService from '../../../services/deliveryReceiptService';
import * as ContractService from '../../../services/contractService';
import { DeliveryReceiptTypeArr, getLabelString, getValueFrom} from '../../../commons/propertiesType';
export function init(updateItemDefine, updateItem, proposalCode, listData,item) {
    let itemDefines = CONST_FEATURE.DeliveryReceipt.itemDef;
    if (item) {
        item.deliveryReceiptType = getLabelString(item.deliveryReceiptType, DeliveryReceiptTypeArr).toString();
        updateItem(item)
    }
    itemDefines.props[4].cbFunc = (code, item) => {
        ContractService.GetContractById(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                let selectedItem = item
                selectedItem.deliveryReceiptCode = 'GN-' + objRespone.item.proposalCode;
                selectedItem.proposalID = objRespone.item.proposalID;
                selectedItem.proposalCode = objRespone.item.proposalCode;
                selectedItem.departmentName = objRespone.item.departmentName;
                selectedItem.items = objRespone.item.items;
                selectedItem.isVAT = objRespone.item.isVAT;
                selectedItem.vatNumber = objRespone.item.vatNumber;
                selectedItem.deliveryReceiptType = getLabelString(objRespone.item.deliveryReceiptType, DeliveryReceiptTypeArr).toString();
                updateItem(selectedItem)


            }
        })
    }
    
    itemDefines.props[3].isDisable = true
    itemDefines.props[1].type = "input";
    itemDefines.props[3].type = "input";
    itemDefines.props[1].isDisable = true
    itemDefines.props[2].isDisable = true
    itemDefines.props[2].type = "input";
    itemDefines.props[4].isDisable = false
    itemDefines.props[4].values = []
    for (let record of listData.lstContract) {
        let item = { label: record.code, value: record.id };
        itemDefines.props[4].values.push(item);
    }
    itemDefines.props[4].type = "select";
    updateItemDefine(itemDefines);
}

export function DeliveryReceiptModify(item, files, callBack) {
    item = item ;
    item.deliveryReceiptType = getValueFrom(item.deliveryReceiptType, DeliveryReceiptTypeArr).toString();

    item.deliveryReceiptDate = moment(item.deliveryReceiptDate).format('YYYY-MM-DD');
    var data = new FormData();
     if (files.length > 0) {
            for (let file of files) {
                data.append('files', file);
            }
        }
    Object.keys(item).forEach(key => {
        if (key !=='items' && key !=='employees')
            data.append(key, item[key]);
    });
    if (item.items) {
        for (let i = 0; i < item.items.length; i++) {
            for (let key in item.items[i]) {
                data.append("items[" + i + "]." + key, item.items[i][key]);
            }
        }
    }

    if (item.employees) {
        for (let i = 0; i < item.employees.length; i++) {
            for (let key in item.employees[i]) {
                data.append("employees[" + i + "]." + key, item.employees[i][key]);
            }
        }
    }

    if (item["deliveryReceiptID"] === undefined) {
        DeliveryReceiptService.createDeliveryReceipt(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    } else {
        DeliveryReceiptService.updateDeliveryReceipt(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone,'edit');
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    }
}