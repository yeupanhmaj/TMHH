


import moment from 'moment';
import { CONST_FEATURE } from '../../../commons';
import * as Actions from '../../../libs/actions';
import * as CommonService from '../../../services/commonService';
import * as ProposalService from '../../../services/proposalService';
import * as QuoteService from '../../../services/quoteService';

export function getItemDefines(itemDefines, callback) {
    CommonService.GetAllCustomer().then(
        result => {
            if (result.isSuccess) {
                itemDefines.props[3].values = [];
                for (let record of result.data) {
                    let item = { label: record.customerName, value: record.customerID };
                    (itemDefines.props[3].values ).push(item);
                }
                itemDefines.props[1].isDisable = true;
                callback(itemDefines);
            }
        }
    )
}


export function init(updateItemDefine, updateItem, proposalCode, listData, item) {
    let itemDefines = CONST_FEATURE.Quote.itemDef;
    itemDefines.props[1].type = "input"
    if(item === undefined) {
        itemDefines.props[19].isDisable = true;
    }else{
        if (item[itemDefines.props[18].name] === true){
            itemDefines.props[19].isDisable = false;
        }else{
            itemDefines.props[19].isDisable = true;
        }
    }
    itemDefines.props[3].cbFunc = (code, item) => {
        CommonService.GetCustomerById(code).then((objRespone) => {
            if (objRespone.isSuccess === true) {
                let selectedItem = item
                selectedItem.customerID = objRespone.item.customerID;
                selectedItem.customerName = objRespone.item.customerName;
                selectedItem.address = objRespone.item.address;
                selectedItem.phone = objRespone.item.phone;
                selectedItem.email = objRespone.item.email;
                selectedItem.taxCode = objRespone.item.taxCode;
                selectedItem.bankNumber = objRespone.item.bankNumber;
                selectedItem.bankName = objRespone.item.bankName;
                selectedItem.surrogate = objRespone.item.surrogate;
                selectedItem.position = objRespone.item.position;
                updateItem(selectedItem);
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
        })
    }

    
   

    getItemDefines(itemDefines, updateItemDefine);

        ProposalService.GetProposalByCode(proposalCode).then((objRespone) => {
            if (objRespone.isSuccess === true) {    
                let selectedItem = {} 

                if(item) {
                    selectedItem = item
                }else{
                    itemDefines.props.forEach(element => {
                        if(element.defaultText)
                        selectedItem[element.name] = element.defaultText
                    });
                } 
                selectedItem.proposalID = objRespone.item.proposalID;
                selectedItem.proposalCode = objRespone.item.proposalCode;
                selectedItem.departmentName = objRespone.item.departmentName;

                if(item  && item.items === undefined) // error refresh list
                selectedItem.items = objRespone.item.items;
              //  selectedItem.quoteCode = "BG-" + objRespone.item.proposalCode;
                selectedItem.dateIn = new Date();
                updateItem(selectedItem)
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
            }
        })
  
 
    itemDefines.props[18].cbFunc = (value, item) => {
        if(value === true){
            itemDefines.props[19].isDisable = false;
        }else{
            itemDefines.props[19].isDisable = true;
        }
        updateItemDefine(itemDefines);
    }
}


export function QuoteModify(item, files, callBack) {
    item.customerID = +item.customerID;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');

    //check totalcost
    let total = 0
    for (let record of item.items) {
        if (record['itemPrice']) {
            total += (+(record['itemPrice'].toString().replace(/\./g, '')) * record['amount'])
        }
    }

    if(item.isVAT === true){
        item.totalCost = Math.round(total * 1.1)
    }else{
        item.totalCost = total
    }
    //check totalcost

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
    if (item[CONST_FEATURE.Quote.KEY_COLUMN] === undefined) {
        QuoteService.createQuotewithAttFiles(data)
            .then(objRespone => {
                callBack(objRespone, "insert");
            })
    } else {
        QuoteService.updateQuotewithAttFiles(item["quoteID"], data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    callBack(objRespone, "edit");
                } else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
    }
}



