


import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as AuditService from '../../../services/auditService';
import * as QuoteService from '../../../services/quoteService';

export function init(updateItemDefine, updateItem, proposalCode, listData, item, getcurrent) {
    let itemDefines = CONST_FEATURE.Audit.itemDef;
    if (item && item.employees) {
        updateItem(item)
    } else {
        setTimeout(() => {
            AuditService.getListDefaultEmployee().then((result) => {
                item = getcurrent();
                if (result.isSuccess) {
                    item.employees = result.data;
                    updateItem(item)
                }

            })
        }, 500);

    }
    itemDefines.props[2].cbFunc = (code, item) => {
        QuoteService.getQuoteByCode(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                let selectedItem = item
                selectedItem.proposalID = objRespone.item.proposalID;
                selectedItem.proposalCode = objRespone.item.proposalCode;
                selectedItem.departmentName = objRespone.item.departmentName;
                selectedItem.isVAT = objRespone.item.isVAT;
                selectedItem.vatNumber = objRespone.item.vatNumber;
                selectedItem.items = objRespone.item.items;
                selectedItem.auditCode = "BKG-" + objRespone.item.proposalCode;
                updateItem(selectedItem)
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
        })
    }


    itemDefines.props[8].cbFunc = (employee, item) => {
        item = getcurrent();
        if (item.employees) {
            let update = false;
            for (let i = 0; i < item.employees.length; i++) {
                if (item.employees[i].employeeID === employee.value) {
                    item.employees.splice(i,1)
                    update = true;
                    break;
                }
            }

            if(update){
                updateItem(item)
            }
        }
    }
    itemDefines.props[2].values = []
    for (let record of listData.lstQuote) {
        let item = { label: record.code, value: record.code };
        itemDefines.props[2].values.push(item);
    }

    itemDefines.props[2].type = "select";
    updateItemDefine(itemDefines);


}


export function AuditModify(item, files, callBack) {
    item = item ;
    item.customerID = +item.customerID;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');
    item.startTime = moment(new Date(item.startTime)).format('YYYY-MM-DD HH:mm');
    item.endTime = moment(new Date(item.endTime)).format('YYYY-MM-DD HH:mm');
    item.preside = item.preside
    if (item.preside.value) {
        item.preside = item.preside.value
    }
    item.secretary = item.secretary
    if (item.secretary.value) {
        item.secretary = item.secretary.value
    }
    var data = new FormData();
    if (files.length > 0) {
        for (let file of files) {
            data.append('files', file);
        }
    }

    Object.keys(item).forEach(key => {
        if (key !=='employees' && key!= 'items' ) {
            data.append(key, item[key]);
        }

    });
    if (item.employees) {
        for (let i = 0; i < item.employees.length; i++) {
            for (let key in item.employees[i]) {
                data.append("employees[" + i + "]." + key, item.employees[i][key]);
            }
        }
    }

    // item.status = +item.status;
    if (item[CONST_FEATURE.Audit.KEY_COLUMN] === undefined) {
        AuditService.createAuditwithAttFiles(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {

            })
    } else {
        AuditService.updateAuditwithAttFiles(item["auditID"], data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone, 'edit');
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {

            })
    }
}
