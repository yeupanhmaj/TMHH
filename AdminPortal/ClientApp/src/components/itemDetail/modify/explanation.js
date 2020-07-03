


import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as CommonService from '../../../services/commonService';
import * as ProposalService from '../../../services/proposalService';
import * as ExplanationService from '../../../services/explanationService';


export function init(updateItemDefine, updateItem, proposalCode, listData, item, getcurrent) {
    let itemDefines = CONST_FEATURE.Explanation.itemDef;
    itemDefines.props[1].isDisable = true;
    itemDefines.props[1].type = 'input';
    updateItemDefine(itemDefines)
    if(item)updateItem(item)
    setTimeout(() => {
        ProposalService.GetProposalByCode(proposalCode).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                let selectedItem = getcurrent();
                selectedItem.proposalID = objRespone.item.proposalID;
                selectedItem.proposalCode = objRespone.item.proposalCode;
                selectedItem.departmentName = objRespone.item.departmentName;
                selectedItem.items = objRespone.item.items;
                selectedItem.explanationCode = "GT-" + objRespone.item.proposalCode;
  
                if(selectedItem.productsName === undefined){
                    let productsName = '';
               
                        if (selectedItem.items && selectedItem.items.length > 0) {
                            for (let record of selectedItem.items) {
                                productsName += record.amount +  ' ' +  record.itemUnit + ' ' +  record.itemName + ', ';
                            }
                            productsName = productsName.substring(0, productsName.length - 2);
                            productsName = productsName + '.'
                        }
                 
                    selectedItem.productsName = productsName
                }
                updateItem(selectedItem)
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
        })
    }, 500);
    

}

export function ExplanationModify(item, files, callBack) {
    item = item ;
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

    // item.status = +item.status;
    if (item[CONST_FEATURE.Explanation.KEY_COLUMN] === undefined) {
        ExplanationService.createExplanationwithAttFiles(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {

            })
    } else {
        data.delete("inTime");
        ExplanationService.updateExplanationwithAttFiles(item["explanationID"], data)
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