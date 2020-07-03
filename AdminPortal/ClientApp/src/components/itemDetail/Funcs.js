
import moment from 'moment';
import * as AcceptanceService from '../../services/acceptanceService';
import * as AuditService from '../../services/auditService';
import * as BidPlanService from '../../services/bidPlanService';
import * as ContractService from '../../services/contractService';
import * as DecisionService from '../../services/decisionService';
import * as DeliveryReceiptService from '../../services/deliveryReceiptService';
import * as ExplanationService from '../../services/explanationService';
import * as NegotiationService from '../../services/negotiationService';
import * as ProposalService from '../../services/proposalService';
import * as QuoteService from '../../services/quoteService';
import * as SurveyService from '../../services/surveyService';
import { getLabelString, statusArr, DeliveryReceiptTypeArr, acceptanceResultArr, AuditLocationArr, BidMethodArr, NegotiationTermArr, NegotiationBankIDArr, QuoteTypeArr, surveyTypes } from '../../commons/propertiesType';

export function getAcceptanceItem(id) {
    return new Promise((resolve, reject) => {
        AcceptanceService.GetAcceptanceById(id).then(result => {
            if (result.isSuccess ===true) {
                result.item.updateTime = moment(new Date(result.item.updateTime)).format('DD-MM-YYYY')
                result.item.acceptanceResultLabel = getLabelString(result.item.acceptanceResult, acceptanceResultArr).toString()
                let itemContent = JSON.parse(JSON.stringify(result.item));
                let item = {} ;
                if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                delete itemContent.items;
                delete itemContent.listDocument;
                delete itemContent.listComment;
                item.itemContent = itemContent
                return resolve(item);
            } else {
                return reject(result.err)
            }
        }).catch((ex) => {
            return reject(ex);
        })
    })
}

export function getAuditItem(id) {
    return new Promise((resolve, reject) => {
  
        AuditService.GetAuditById(id).then(
            result => {
                if (result.isSuccess ===true) {
                    result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                    result.item.startTime = moment(new Date(result.item.startTime)).format('DD-MM-YYYY HH:mm')
                    result.item.endTime = moment(new Date(result.item.endTime)).format('DD-MM-YYYY HH:mm');
                    result.item.location = getLabelString(result.item.location, AuditLocationArr).toString();
                    let itemContent = JSON.parse(JSON.stringify(result.item));
                    let item = {} ;
                    if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                    if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                    if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                    if (itemContent.employees) item.listEmployees = JSON.parse(JSON.stringify(itemContent.employees));
                    item.item = JSON.parse(JSON.stringify(result.item))
                    delete itemContent.items;
                    delete itemContent.listDocument;
                    delete itemContent.listComment;
                    delete itemContent.listEmployees;
                    item.itemContent = itemContent
                    return resolve(item);
                } else {
                    return reject(result.err)
                }
            }).catch((ex) => {
                return reject(ex);
            })
    })
}

export function getBidPlanItem(id) {
    return new Promise((resolve, reject) => {
        BidPlanService.GetBidPlanById(id).then(
            result => {
                if (result.isSuccess ===true) {

                    result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                    result.item.bidMethodLabel = getLabelString(result.item.bidMethod, BidMethodArr).toString();
                    result.item.bidLocationLabel = getLabelString(result.item.bidLocation, AuditLocationArr).toString();
                    let itemContent = JSON.parse(JSON.stringify(result.item));
                    let item = {} ;
                    if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                    if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                    if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                    item.item = JSON.parse(JSON.stringify(result.item))

                    delete itemContent.items;
                    delete itemContent.listDocument;
                    delete itemContent.listComment;
                    item.itemContent = itemContent
                    return resolve(item);
                } else {
                    return reject(result.err)
                }
            }).catch((ex) => {
                return reject(ex);
            })
    })
}

export function getContractItem(id) {
    return new Promise((resolve, reject) => {
        ContractService.GetPrintDetails(id).then(
            result => {
                if (result.isSuccess ===true) {
                   
                    result.item.aLocation = getLabelString(result.item.aLocation, AuditLocationArr).toString()
                    result.item.termLabel = getLabelString(result.item.term, NegotiationTermArr).toString()
                    result.item.aBankIDLabel = getLabelString(result.item.aBankID, NegotiationBankIDArr).toString()
             
                    let itemContent = JSON.parse(JSON.stringify(result.item));
                    let item = {} ;
                    if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                    if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                    if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                    item.item = JSON.parse(JSON.stringify(result.item))
                    delete itemContent.items;
                    delete itemContent.listDocument;
                    delete itemContent.listComment;
                    item.itemContent = itemContent
                    return resolve(item);
                } else {
                    return reject(result.err)
                }
            }).catch((ex) => {
                return reject(ex);
            })
    })
}

export function getDecisionItem(id) {
    return new Promise((resolve, reject) => {
        DecisionService.GetDecisionById(id).then(
            result => {
                if (result.isSuccess ===true) {
                    result.item.dateInOri = result.item.dateIn;
                    result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                    let itemContent = JSON.parse(JSON.stringify(result.item));
                    let item = {} ;
                    if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                    if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                    if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                    item.item = JSON.parse(JSON.stringify(result.item))
                    delete itemContent.items;
                    delete itemContent.listDocument;
                    delete itemContent.listComment;
                    item.itemContent = itemContent
                    return resolve(item);
                } else {
                    return reject(result.err)
                }
            }).catch((ex) => {
                return reject(ex);
            })
    })
}

export function getDeliveryReceiptItem(id) {
    return new Promise((resolve, reject) => {
        DeliveryReceiptService.GetDeliveryReceiptById(id).then(result => {
            if (result.isSuccess ===true) {
                result.item.deliveryReceiptDate = moment(new Date(result.item.deliveryReceiptDate)).format('DD-MM-YYYY')
                result.item.updateTime = moment(new Date(result.item.updateTime)).format('DD-MM-YYYY')
                result.item.deliveryReceiptPlaceLabel = getLabelString(result.item.deliveryReceiptPlace, AuditLocationArr).toString();
                result.item.deliveryReceiptTypeLabel = getLabelString(result.item.deliveryReceiptType, DeliveryReceiptTypeArr).toString()
                let itemContent = JSON.parse(JSON.stringify(result.item));
                let item = {} ;
                if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                if (itemContent.employees) item.listEmployees = JSON.parse(JSON.stringify(itemContent.employees));
                item.item = JSON.parse(JSON.stringify(result.item))
                delete itemContent.items;
                delete itemContent.listDocument;
                delete itemContent.listComment;
                item.itemContent = itemContent
                return resolve(item);
            } else {
                return reject(result.err)
            }
        }).catch((ex) => {
            return reject(ex);
        })
    })
}

export function getExplanationItem(id) {
    return new Promise((resolve, reject) => {
        ExplanationService.GetExplanationById(id).then(
            result => {
                if (result.isSuccess ===true) {
                    result.item.inTime = moment(new Date(result.item.inTime)).format('DD-MM-YYYY')
                    let itemContent = JSON.parse(JSON.stringify(result.item));
                    let item = {} ;
                    if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                    if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                    if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                    item.item = JSON.parse(JSON.stringify(result.item))
                    let productsName = '';
                    if (itemContent.itemContent ===undefined) {
                        if (itemContent.items && itemContent.items.length > 0) {
                            for (let record of itemContent.items) {
                                productsName += record.itemName + ', ';
                            }
                            productsName = productsName.substring(0, productsName.length - 2);
                            productsName = productsName + '.'
                        }
                    }
                    itemContent.productsName = productsName;
                    delete itemContent.items;
                    delete itemContent.listDocument;
                    delete itemContent.listComment;
                    item.itemContent = itemContent
                    return resolve(item);
                } else {
                    return reject(result.err)
                }
            }).catch((ex) => {
                return reject(ex);
            })
    })
}

export function getNegotiationItem(id) {
    return new Promise((resolve, reject) => {
        NegotiationService.GetPrintDetails(id).then(
            result => {

                if (result.isSuccess ===true) {

                    result.item.aLocation = getLabelString(result.item.aLocation, AuditLocationArr).toString()
                    result.item.termLabel = getLabelString(result.item.term, NegotiationTermArr).toString()
                    result.item.aBankIDLabel = getLabelString(result.item.aBankID, NegotiationBankIDArr).toString()
             
                    let itemContent = JSON.parse(JSON.stringify(result.item));
                    let item = {} ;
                    if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                    if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                    if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                    item.item = JSON.parse(JSON.stringify(result.item))
                    delete itemContent.items;
                    delete itemContent.listDocument;
                    delete itemContent.listComment;
                    item.itemContent = itemContent
                    return resolve(item);
                } else {
                    return reject(result.err)
                }
            }).catch((ex) => {
                return reject(ex);
            })
    })
}
export function getProposalItem(id) {
    return new Promise((resolve, reject) => {
        ProposalService.GetProposalById(id).then(result => {
            if (result.isSuccess ===true) {
                result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                result.item.labelStatus = getLabelString(result.item.status, statusArr).toString();
                let itemContent = JSON.parse(JSON.stringify(result.item));
                let item = {} ;
                if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                item.item = JSON.parse(JSON.stringify(result.item))
                delete itemContent.items;
                delete itemContent.listDocument;
                delete itemContent.listComment;
                item.itemContent = itemContent
                return resolve(item);
            } else {
                return reject(result.err)
            }


        }).catch((ex) => {
            return reject(ex);
        })
    })
}

export function getQuoteItem(id) {
    return new Promise((resolve, reject) => {
        QuoteService.GetQuoteById(id).then(
            result => {
                if (result.isSuccess ===true) {
                    result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                    result.item.quoteTypeLabel = getLabelString(result.item.quoteType, QuoteTypeArr).toString();
                    let itemContent = JSON.parse(JSON.stringify(result.item));
                    let item = {} ;
                    if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                    if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                    if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                    item.item = JSON.parse(JSON.stringify(result.item))
                    delete itemContent.items;
                    delete itemContent.listDocument;
                    delete itemContent.listComment;
                    item.itemContent = itemContent
                    return resolve(item);
                } else {
                    return reject(result.err)
                }
            }).catch((ex) => {
                return reject(ex);
            })
    })
}

export function getSurveyItem(id) {
    return new Promise((resolve, reject) => {
        SurveyService.GetSurveyById(id).then(
            result => {
                if (result.isSuccess ===true) {
                    result.item.inTime = moment(new Date(result.item.inTime)).format('DD-MM-YYYY')
                    result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                    result.item.solutionName = getLabelString(result.item.solution, surveyTypes);
                    let itemContent = JSON.parse(JSON.stringify(result.item));
                    let item = {} ;
                    if (itemContent.items) item.listItems = JSON.parse(JSON.stringify(itemContent.items));
                    if (itemContent.sureyItems) item.sureyItems = JSON.parse(JSON.stringify(itemContent.sureyItems));
                    if (itemContent.listDocument) item.listDocument = JSON.parse(JSON.stringify(itemContent.listDocument));
                    if (itemContent.listComment) item.listComment = JSON.parse(JSON.stringify(itemContent.listComment));
                    item.item = JSON.parse(JSON.stringify(result.item))
                    if (itemContent.itemContent ===undefined) {
                        let productsName = '';
                        if (itemContent.items && itemContent.items.length > 0) {
                            for (let record of itemContent.items) {
                                productsName += record.itemName + ', ';
                            }
                            productsName = productsName.substring(0, productsName.length - 2);
                            productsName = productsName + '.'
                        }
                        itemContent.productsName = productsName;
                    }
                    delete itemContent.items;
                    delete itemContent.listDocument;
                    delete itemContent.listComment;
                    item.itemContent = itemContent
                   
                    return resolve(item);
                } else {
                    return reject(result.err)
                }
            }).catch((ex) => {
                return reject(ex);
            })
    })
}

export function deleteRecord(id, feature) {
    return new Promise((resolve, reject) => {
        let service = undefined ;
        switch (feature) {
            case "Proposal":
                service = ProposalService;
                break;
            case "Explanation":
                service = ExplanationService;
                break;
            case "Survey":
                service = SurveyService;
                break;
            case "Quote":
                service = QuoteService;
                break;
            case "Audit":
                service = AuditService;
                break;
            case "BidPlan":
                service = BidPlanService;
                break;
            case "Negotiation":
                service = NegotiationService;
                break;
            case "Decision":
                service = DecisionService;
                break;
            case "Contract":
                service = ContractService;
                break;
            case "DeliveryReceipt":
                service = DeliveryReceiptService;
                break;
            case "Acceptance":
                service = AcceptanceService;
                break;
            default:
                break;
        }
        if (service ===undefined) {
            return reject("lỗi không tìm được service")
        }
        service.deleteRecord(id).then(
            (result) => {

                return resolve(result);

            }).catch((ex) => {
                return reject(ex);
            })
    })
}
