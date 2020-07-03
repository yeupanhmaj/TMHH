


import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as ProposalService from '../../../services/proposalService';

import moment from 'moment';
import * as SurveyService from '../../../services/surveyService';


export function getItemDefines(itemDefines, callback) {
    SurveyService.getMasterData().then(
        result => {

            let lstSurveyDepartments = result[0]
            // let lstProposalTypes = result[1]

            if (itemDefines && itemDefines.props && itemDefines.props.length > 0) {
                if (lstSurveyDepartments.isSuccess) {
                    itemDefines.props[3].values = [];
                    for (let record of lstSurveyDepartments.data) {
                        let item = { label: record.departmentName, value: record.departmentID };
                        if ("Hành chánh quản trị" == item.label || "Tài chính kế toán" == item.label || 39 == item.value) {
                            (itemDefines.props[3].values ).push(item);
                           
                        }
                   
                    }
                }
            
            }

            callback(itemDefines);
        }
    )
}

export function init(updateItemDefine, updateItem, proposalCode, listData, item) {
    let itemDefines = CONST_FEATURE.Survey.itemDef;
    itemDefines.props[1].isDisable = true;
    itemDefines.props[1].type = "input";
    getItemDefines(itemDefines, updateItemDefine);
    if(item)updateItem(item);
    ProposalService.GetProposalByCode(proposalCode).then((objRespone) => {
        if (objRespone.isSuccess === true) {
            let selectedItem = {} 
            if(item)selectedItem = item;
            selectedItem.proposalID = objRespone.item.proposalID;
            selectedItem.proposalType = objRespone.item.proposalType;
            selectedItem.proposalCode = objRespone.item.proposalCode;
            selectedItem.departmentName = objRespone.item.departmentName;
            selectedItem.items = objRespone.item.items;
            selectedItem.surveyCode = "KS-" + objRespone.item.proposalCode;
        
            if(selectedItem.productsName === undefined){
                let productsName = '';
                    if (selectedItem.items && selectedItem.items.length > 0) {
                        for (let record of selectedItem.items) {
                            productsName += record.amount +  ' ' +  ( record.itemUnit ? record.itemUnit : '') + ' ' +  record.itemName + ', ';
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

}

export function SurveyModify(item, files, callBack) {
    item = item ;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');
  
    var data = new FormData();
     if (files.length > 0) {
            for (let file of files) {
                data.append('files', file);
            }
        }
        Object.keys(item).forEach(key => {
            if (key !== 'items' && key !== 'surveyItems')
              data.append(key, item[key]);
          });
    
          for (let i = 0; i < item.items.length; i++) {
            for (let key in item.items[i]) {
              data.append("items[" + i + "]." + key, item.items[i][key]);
            }
          }
        
          for (let i = 0; i < item.surveyItems.length; i++) {
            for (let key in item.surveyItems[i]) {
              data.append("surveyItems[" + i + "]." + key, item.surveyItems[i][key]);
            }
          }
    for (let i = 0; i < item.items.length; i++) {
        for (let key in item.items[i]) {
            data.append("items[" + i + "]." + key, item.items[i][key]);
        }
    }

    if (item[CONST_FEATURE.Survey.KEY_COLUMN] === undefined) {
        SurveyService.createSurveywithAttFiles(data)
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
        SurveyService.updateSurveywithAttFiles(item["surveyID"], data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone,"edit");
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {

            })
    }
}