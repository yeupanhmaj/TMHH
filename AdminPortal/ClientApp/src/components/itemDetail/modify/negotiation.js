


import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as BidPlanService from '../../../services/bidPlanService';
import * as CommonService from '../../../services/commonService';
import * as NegotiationService from '../../../services/negotiationService';

export function init(updateItemDefine, updateItem, proposalCode, listData, item) {
    let itemDefines = CONST_FEATURE.Negotiation.itemDef;

    itemDefines.props[2].cbFunc = (code, item) => {
        BidPlanService.getBidPlanByCode(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                let selectedItem = item
                selectedItem.proposalID = objRespone.item.proposalID;
                selectedItem.proposalCode = objRespone.item.proposalCode;
                selectedItem.departmentName = objRespone.item.departmentName;
                selectedItem.items = objRespone.item.items;
                selectedItem.isVAT = objRespone.item.isVAT;
                selectedItem.vatNumber = objRespone.item.vatNumber;
                selectedItem.bidType   = objRespone.item.bidType;  
                selectedItem.bidExpirated   = objRespone.item.bidExpirated;  
                selectedItem.bidExpiratedUnit   = objRespone.item.bidExpiratedUnit;  
                selectedItem.negotiationCode = "TT-" + objRespone.item.proposalCode;
                updateItem(selectedItem);
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
        })
        CommonService.GetCustomerByBidPlanId(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
              let selectedItem = item
              selectedItem.customerID = objRespone.item.customerID;
              selectedItem.bSide = objRespone.item.customerName;
              selectedItem.bLocation = objRespone.item.address;
              selectedItem.bPhone = objRespone.item.phone;
              selectedItem.bFax = objRespone.item.fax;
              selectedItem.bTaxCode = objRespone.item.taxCode;
              selectedItem.bBankID = objRespone.item.bankNumber;
              selectedItem.bRepresent = objRespone.item.surrogate;
              selectedItem.bPosition = objRespone.item.position;
              updateItem(selectedItem);
      
            } else {
              Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
          })
    }
    itemDefines.props[2].values = []
    for (let record of listData.lstBidPlan) {
        let item = { label: record.code, value: record.code };
        itemDefines.props[2].values.push(item);
    }
    itemDefines.props[2].type = "select";
    updateItemDefine(itemDefines);
    if(item)updateItem(item);

}


export function NegotiationModify(item, files, callBack) {
    item = item ;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD HH:mm');
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
    if (item[CONST_FEATURE.Negotiation.KEY_COLUMN] === undefined) {
        NegotiationService.createNegotiationwithAttFiles(data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone);
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {

            })
    } else {
        NegotiationService.updateNegotiationwithAttFiles(item[CONST_FEATURE.Negotiation.KEY_COLUMN], data)
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