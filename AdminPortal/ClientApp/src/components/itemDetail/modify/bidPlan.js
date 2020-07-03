


import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as QuoteService from '../../../services/quoteService';
import * as BidPlanService from '../../../services/bidPlanService';

export function init(updateItemDefine, updateItem, proposalCode, listData, item, getcurrent) {

    let itemDefines = CONST_FEATURE.BidPlan.itemDef;
    if (item) updateItem(item)
    itemDefines.props[2].cbFunc = (code, item) => {
        QuoteService.getQuoteByCode(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
              let selectedItem = item
            //   selectedItem.proposalID = objRespone.item.proposalID;
            //   selectedItem.proposalCode = objRespone.item.proposalCode;
            //   selectedItem.departmentName = objRespone.item.departmentName;
              selectedItem.items = objRespone.item.items;
              selectedItem.isVAT = objRespone.item.isVAT;
              selectedItem.vatNumber = objRespone.item.vatNumber;
              selectedItem.bidPlanCode =  "";//"KH-" + objRespone.item.QuoteID;
              updateItem(item)
            } else {
              Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
          }).catch(err => {
   
          })
    }

    itemDefines.props[3].cbFunc = (value, item) => {
        let selectedItem = item
        let month = value.getMonth() + 1;
        let quater = (Math.ceil(month / 3));
        switch(quater) {
          case 1:
            selectedItem.bidTime = "Quý I năm " + value.getFullYear();
            break;
          case 2:
            selectedItem.bidTime = "Quý II năm " + value.getFullYear();
            break;
          case 3:
            selectedItem.bidTime = "Quý III năm " + value.getFullYear();
            break;
          case 4:
            selectedItem.bidTime = "Quý IV năm " + value.getFullYear();
            break;
          default:
            selectedItem.bidTime = " ";
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


export function BidPlanModify(item, files, callBack) {
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
    if (item[CONST_FEATURE.BidPlan.KEY_COLUMN] === undefined) {

        BidPlanService.createBidPlanwithAttFiles(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {

            })
    } else {
    
        BidPlanService.updateBidPlanwithAttFiles(item["bidPlanID"], data)
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