import * as React from 'react';
import { Tab, TabList, TabPanel, Tabs } from 'react-tabs';
import "react-tabs/style/react-tabs.css";
import { Modal } from 'reactstrap';
import ConfirmDialog from '../../components/confirmDialog';
import Loading from '../../components/loading';
import * as Actions from '../../libs/actions';
import { AcceptanceDetails } from '../../models/acceptance';
import { AuditDetails } from '../../models/audit';
import { BidPlanDetails } from '../../models/bidPlan';
import { ContractDetails } from '../../models/contract';
import { DecisionDetails } from '../../models/decision';
import { DeliveryReceiptDetails } from '../../models/deliveryReceipt';
import { ExplanationDetails } from '../../models/explanation';
import { NegotiationtDetails } from '../../models/negotiation';
import { ProposalDetails } from '../../models/proposal';
import { QuoteDetails } from '../../models/quote';
import { SurveyDetails } from '../../models/survey';
import * as FileUploaderServices from '../../services/fileUploaderServices';
import * as ProposalService from '../../services/proposalService';
import ListItemAcceptance from '../editCreateItemModal/addEditListItems/listItemAcceptance';
import ListItemDeliveryReceipt from '../editCreateItemModal/addEditListItems/listItemDeliveryReceipt';
import ListItemExplanation from '../editCreateItemModal/addEditListItems/listItemExplanation';
import ListItemQuote from '../editCreateItemModal/addEditListItems/listItemQuote';
import ListItemSurvey from '../editCreateItemModal/addEditListItems/listItemSurvey';
import * as FUNC from './Funcs';
import ItemContent from './itemContent';
import ListComment from './listComment';
import ListDocument from './listDocument';
import ListEmployees from './listEmployees';
import ListItems from './listItems';

import {stringToSlug} from '../../libs/util'
import './main.css';
import ModalCreateEdit from './modify/modalCreateEdit';
var Barcode = require('react-barcode');

export default class ItemDetails extends React.Component {
    _getDataFunc;
    _deleteDataFunc;
    _ItemsDefine;
    _subTab;
    _hiddenLink;
    _editModal;
    constructor(props) {
        super(props);
        this.state = {
            listRelateData: undefined ,
            listItems: undefined ,
            itemContent: undefined ,
            listDocument: undefined ,
            listComment: undefined ,
            listEmployees: undefined ,
            currentItem: undefined ,
            warningText: undefined ,
            tabIndex: 0,
            loading: false,
            feature: undefined ,
    
            code: '',
            currentID: 0 ,
    
            editModal: false,
            proposalCode: '',
            proposalID: 0,
    
            confirmModal: false,
            confirmContent: '',
    
            editItem: undefined ,
            isOutPlan: false,
            outplantext: '',
    
            printStatus: false,
        }
    }

  


    componentDidUpdate(prevProps, prevState) {
        if(prevProps.isOpen!==this.props.isOpen && prevProps.isOpen==false){
            this.setState({isOpen: false, itemContent: undefined, listItems: undefined, listDocument: undefined, listComment: undefined, listEmployees: undefined })       
        }
        if (this.props.idItem && this.props.isOpen === true && JSON.stringify(this.props) !== JSON.stringify(prevProps)) {
            this.checkTabIndex();
            this.getFuncData(this.props.feature);         
            this.getData(this.props.idItem, true);
            this.setState({feature: this.props.feature})
        }
    }
    checkTabIndex() {
        let tabIndex = 0;
        switch (this.props.feature) {
            case 'Proposal':
                tabIndex = 0;
                break;
            case 'Explanation':
                tabIndex = 1;
                break;
            case 'Survey':
                tabIndex = 2;
                break;
            case 'Quote':
                tabIndex = 3;
                break;
            case 'Audit':
                tabIndex = 4;
                break;
            case 'BidPlan':
                tabIndex = 5;
                break;
            case 'Negotiation':
                tabIndex = 6;
                break;
            case 'Decision':
                tabIndex = 7;
                break;
            case 'Contract':
                tabIndex = 8;
                break;
            case 'DeliveryReceipt':
                tabIndex = 9;
                break;
            case 'Acceptance':
                tabIndex = 10;
                break;
            default:
                tabIndex = 0;
                break;
        }

        this.setState({ tabIndex });
    }
    componentWillMount() {
        this.checkTabIndex();
    }

    componentDidMount() {

    }

    onConfirmDelete() {
        let listHeader = this.getListHeader();
        let confirmContent = "Bạn muốn xóa " + listHeader[this.state.tabIndex].toLocaleLowerCase();
        confirmContent += ' :  ' + this.state.code;

        this.setState({ confirmModal: true, confirmContent })
    }

    onDeleteData() {
        this.setState({ loading: true })
        FUNC.deleteRecord(this.state.currentID, this.state.feature)
            .then((objRespone) => {

                if (objRespone.isSuccess === true) {
                    if (this._subTab && this._subTab.length > 1) {
                        if (this.state.currentID === this._subTab[0].id) {
                            this.getData(this._subTab[1].id, true);
                        } else {
                            this.getData(this._subTab[0].id, true);
                        }
                    } else {
                        this.getData(0, true);
                    }
                } else {
                    this.setState({ loading: false })
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            })
        this.setState({
            confirmModal: false,
            loading: false
        })
    }

    constGetListItem(items) {
        let ret = (<div />)
        let details = {
            header: "Danh mục sản phẩm",
            name: "items",
            type: "listItems",
            IsFull: true,
            isNotEdited: true,
        }
        
        if (items) {
            switch (this.state.feature) {
                case 'Proposal':
                    ret = (<ListItems items={items}></ListItems>)
                    break;
                case 'Explanation':
                    ret = (<ListItemExplanation
                        itemDefine={details}
                        items={items}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'Survey':
                    ret = (<ListItemSurvey
                        itemDefine={details}
                        isDisabled={true}
                        items={this.state.surveyItems}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'Quote':
                    ret = (<ListItemQuote
                        itemDefine={details}
                        items={items}
                        VAT={this.state.itemContent['isVAT']}
                        vatNumber={this.state.itemContent['vatNumber']}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'Audit':
                    ret = (<ListItemQuote
                        itemDefine={details}
                        items={items}
                        VAT={this.state.itemContent['isVAT']}
                        vatNumber={this.state.itemContent['vatNumber']}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'BidPlan':
                    ret = (<ListItemQuote
                        itemDefine={details}
                        items={items}
                        VAT={this.state.itemContent['isVAT']}
                        vatNumber={this.state.itemContent['vatNumber']}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'Negotiation':
                    ret = (<ListItemQuote
                        itemDefine={details}
                        items={items}
                        VAT={this.state.itemContent['isVAT']}
                        vatNumber={this.state.itemContent['vatNumber']}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'Decision':
                    ret = (<ListItemQuote
                        itemDefine={details}
                        items={items}
                        VAT={this.state.itemContent['isVAT']}
                        vatNumber={this.state.itemContent['vatNumber']}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'Contract':
                    ret = (<ListItemQuote
                        itemDefine={details}
                        items={items}
                        VAT={this.state.itemContent['isVAT']}
                        vatNumber={this.state.itemContent['vatNumber']}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'DeliveryReceipt':
                    ret = (<ListItemDeliveryReceipt
                        itemDefine={details}
                        type={this.state.itemContent["deliveryReceiptType"]}
                        items={items}
                        VAT={this.state.itemContent['isVAT']}
                        vatNumber={this.state.itemContent['vatNumber']}
                        onChange={(value) => { }}
                    />)
                    break;
                case 'Acceptance':
                    ret = (<ListItemAcceptance
                        itemDefine={details}
                        items={items}
                        onChange={(value) => { }}
                    />)
                    break;
                default:
                    ret = (<div />)
                    break;
            }
        }
        return ret;
    }

    getRelatedData() {
        let data = this.state.listRelateData;
        if (data) {
            return (
                <div className="DetailsItemFullWidth" style={{ display: 'flex', flexDirection: 'column', fontSize: 12 }}>
                    <div style={{ fontSize: 15, textAlign: 'center', marginBottom: '5px' }}>
                        <b> Danh sách các tập tin liên quan </b>
                    </div>
                    <div>
                        Đề xuất : <b>{data.proposalCode}</b>
                    </div>
                    {this.childRelate(data.lstExplanation, "Giải Trình")}
                    {this.childRelate(data.lstSurvey, "Khảo sát")}
                    {this.childRelate(data.lstQuote, "Báo giá")}
                    {this.childRelate(data.lstAudit, "Biển bản họp kiểm giá")}
                    {this.childRelate(data.lstBidPlan, "Kế hoạch chọn thầu")}
                    {this.childRelate(data.lstNegotiation, "Biên bản thương thảo HĐ")}
                    {this.childRelate(data.lstDecision, "Hồ sơ quyết định chọn thầu")}
                    {this.childRelate(data.lstContract, "Hợp Đồng")}
                    {this.childRelate(data.lstDeliveryReceipt, "Biên bản giao nhận")}
                    {this.childRelate(data.lstAcceptance, "Biên bản nghiệm thu")}
                </div>
            )
        }
    }

    childRelate(list, name) {
        let text = '';
        for (let item of list) {
            text += ' ' + item.code + ','
        }
        text = text.substring(0, text.length - 1);
        return (
            <div>
                {name} : <b>{text}</b>
            </div>
        );

    }

    changeTab(index) {
        let id = 0;
        let code = '';
        let feature = '';
        let data = this.state.listRelateData;
        let array = undefined 
        switch (index) {
            case 0:
                feature = "Proposal"
                id = data.proposalID;
                break;
            case 1:
                feature = "Explanation"
                array = data.lstExplanation
                break;
            case 2:
                feature = "Survey"
                array = data.lstSurvey
                break;
            case 3:
                feature = "Quote"
                array = data.lstQuote
                break;
            case 4:
                feature = "Audit"
                array = data.lstAudit
                break;
            case 5:
                feature = "BidPlan"
                array = data.lstBidPlan
                break;
            case 6:
                feature = "Negotiation"
                array = data.lstNegotiation
                break;
            case 7:
                feature = "Decision"
                array = data.lstDecision
                break;
            case 8:
                feature = "Contract"
                array = data.lstContract
                break;
            case 9:
                feature = "DeliveryReceipt"
                array = data.lstDeliveryReceipt
                break;
            case 10:
                feature = "Acceptance"
                array = data.lstAcceptance
                break;
            default:
                break;
        }
        if (array !== undefined && array.length > 0) {
            id = array[0].id
            code = array[0].code
        }

        this.getFuncData(feature);

        this.setState({ tabIndex: index, feature, code }, () => {
            this.getData(id, false);
        });
    }
    getFuncData(feature) {
        this._getDataFunc = undefined ;
        this._subTab = undefined 
        let lst = this.state.listRelateData;
        if (feature) {
            switch (feature) {
                case "Proposal":
                    this._getDataFunc = FUNC.getProposalItem;
                    this._ItemsDefine = ProposalDetails;
                    this._subTab = undefined;
                    break;
                case "Explanation":
                    this._getDataFunc = FUNC.getExplanationItem;
                    this._ItemsDefine = ExplanationDetails;
                    if (lst)
                        this._subTab = lst.lstExplanation;
                    break;
                case "Survey":
                    this._getDataFunc = FUNC.getSurveyItem;
                    this._ItemsDefine = SurveyDetails;
                    if (lst)
                        this._subTab = lst.lstSurvey;
                    break;
                case "Quote":
                    this._getDataFunc = FUNC.getQuoteItem;
                    this._ItemsDefine = QuoteDetails;
                    if (lst)
                        this._subTab = lst.lstQuote;
                    break;
                case "Audit":
                    this._getDataFunc = FUNC.getAuditItem;
                    this._ItemsDefine = AuditDetails;
                    if (lst)
                        this._subTab = lst.lstAudit;
                    break;
                case "BidPlan":
                    this._getDataFunc = FUNC.getBidPlanItem;
                    this._ItemsDefine = BidPlanDetails;
                    if (lst)
                        this._subTab = lst.lstBidPlan;
                    break;
                case "Negotiation":
                    this._getDataFunc = FUNC.getNegotiationItem;
                    this._ItemsDefine = NegotiationtDetails;
                    if (lst)
                        this._subTab = lst.lstNegotiation;
                    break;
                case "Decision":
                    this._getDataFunc = FUNC.getDecisionItem;
                    this._ItemsDefine = DecisionDetails;
                    if (lst)
                        this._subTab = lst.lstDecision;
                    break;
                case "Contract":
                    this._getDataFunc = FUNC.getContractItem;
                    this._ItemsDefine = ContractDetails;
                    if (lst)
                        this._subTab = lst.lstContract;
                    break;
                case "DeliveryReceipt":
                    this._getDataFunc = FUNC.getDeliveryReceiptItem;
                    this._ItemsDefine = DeliveryReceiptDetails;
                    if (lst)
                        this._subTab = lst.lstDeliveryReceipt;
                    break;
                case "Acceptance":
                    this._getDataFunc = FUNC.getAcceptanceItem;
                    this._ItemsDefine = AcceptanceDetails;
                    if (lst)
                        this._subTab = lst.lstAcceptance;
                    break;
                default:
                    this._getDataFunc = undefined
                    break;
            }
        }
    }


    getSubTabData(feature) {
        this._subTab = undefined 
        let lst = this.state.listRelateData;
        if (feature) {
            switch (feature) {
                case "Proposal":
                    this._subTab = undefined;
                    break;
                case "Explanation":

                    if (lst)
                        this._subTab = lst.lstExplanation;
                    break;
                case "Survey":

                    if (lst)
                        this._subTab = lst.lstSurvey;
                    break;
                case "Quote":

                    if (lst)
                        this._subTab = lst.lstQuote;
                    break;
                case "Audit":

                    if (lst)
                        this._subTab = lst.lstAudit;
                    break;
                case "BidPlan":

                    if (lst)
                        this._subTab = lst.lstBidPlan;
                    break;
                case "Negotiation":

                    if (lst)
                        this._subTab = lst.lstNegotiation;
                    break;
                case "Decision":

                    if (lst)
                        this._subTab = lst.lstDecision;
                    break;
                case "Contract":

                    if (lst)
                        this._subTab = lst.lstContract;
                    break;
                case "DeliveryReceipt":

                    if (lst)
                        this._subTab = lst.lstDeliveryReceipt;
                    break;
                case "Acceptance":
                    if (lst)
                        this._subTab = lst.lstAcceptance;
                    break;
                default:
                    this._getDataFunc = undefined
                    break;
            }
        }
    }

    isAllowAddNew() {
        let result = true;
        if (this.state.itemContent !== undefined) result = false
        if (this.state.feature === "Quote") result = true
        return result
    }

    getData(id, getRelate) {
        if (this._getDataFunc && id !== 0) {
            this.setState({ loading: true })
            this._getDataFunc(id).then(
                (item) => {
                    let itemContent = item.itemContent;

                    let listDocument = item.listDocument;
                    let listComment = item.listComment;
                    let listItems = item.listItems;
                    let surveyItems = item.item.surveyItems;
         
                    let listEmployees = item.listEmployees;
                    if (getRelate) {
                        this.setState({ surveyItems, isOpen: true, itemContent, listItems, listDocument, listComment, listEmployees, currentID: id, currentItem: item.item })
                        ProposalService.getRelateData(itemContent['proposalID']).then(
                            (response) => {

                                if (response.isSuccess === true) {
                                    let isOutPlan = false;
                                    let outplantext = '';
                                    if (this.props.feature === "Proposal") {
                                        if (this.state.listItems && response.item && response.item.lstExplanation.length === 0)

                                            for (let item of this.state.listItems) {
                                                if (item.isExceedReserve === true) {
                                                    isOutPlan = true
                                                    outplantext = outplantext + item.itemName + " vượt quá dự trù " +
                                                        item.numExceedReserve + " " + item.itemUnit + ","
                                                } else {
                                                    // if (item.isReservered === true) {
                                                    //     isOutPlan = true
                                                    //     outplantext = outplantext + item.itemName + " không có dự trù,"
                                                    // }
                                                }
                                            }
                                        if (isOutPlan === true) {
                                            outplantext = outplantext.substring(0, outplantext.length - 1);
                                            outplantext += "."
                                        }
                                    }
                                    this.setState({
                                        isOutPlan,
                                        outplantext,
                                        loading: false, listRelateData: response.item, proposalCode: response.item.proposalCode
                                        , proposalID: response.item.proposalID
                                    })
                                } else {
                                    Actions.openMessageDialog("Error " + response.err.msgCode, response.err.msgString.toString());
                                    this.setState({ loading: false })
                                }

                            }).catch((er) => {
                                this.setState({ loading: false })
                            })
                    } else {
                        this.setState({surveyItems,  isOpen: true, itemContent, listItems, listDocument, listComment, listEmployees, loading: false, currentID: id, currentItem: item.item })

                    }
                }).catch((er) => {
                    this.setState({ loading: false })
                })
        } else {
            this.setState({
                listItems: undefined ,
                itemContent: undefined ,
                listDocument: undefined ,
                listComment: undefined ,
                listEmployees: undefined ,
                currentID: 0 ,
            })
            if (getRelate) {

                ProposalService.getRelateData(this.state.proposalID).then(
                    (response) => {
                        if (response.isSuccess === true) {
                            this.setState({
                                loading: false, listRelateData: response.item
                            })
                        } else {
                            Actions.openMessageDialog("Error " + response.err.msgCode, response.err.msgString.toString());
                            this.setState({ loading: false })
                        }

                    }).catch((er) => {
                        this.setState({ loading: false })
                    })
            }
        }
    }

    renderSubTabs() {
        this.getSubTabData(this.state.feature)
        if (this._subTab && this._subTab.length > 1) {
            return (
                <div>
                    {this._subTab.map((item, index) => {
                        return (
                            <span key={`subtab${index + ' ' + item.id}`} className={"CustomTabsTCK " + `${item.id === this.state.currentID ? 'CustomTabsActive' : ''}`} onClick={() => {
                                if (item.id !== this.state.currentID) {
                                    this.setState({ code: item.code });
                                    this.getData(item.id, false)
                                }
                            }}>{item.code}</span>
                        )
                    })
                    }
                </div>
            )
        }
    }

    checkRenderBarCode() {
        if (this.state.feature === "Proposal" && this.state.proposalCode !== undefined && this.state.proposalCode !== '' && this.state.isOutPlan === false) {
            return (
                <div style={{ width: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                    <Barcode value={stringToSlug(this.state.proposalCode)} width={1} height={50} />
                </div>
            )
        }
    }
    renderBody() {

        let itemContent = this.state.itemContent;
        let listItems = this.state.listItems;      
        let listDocument = this.state.listDocument;
        let listComment = this.state.listComment;
        let listEmployees = this.state.listEmployees;

        return (
            <React.Fragment>

                <div>
                    <div className="DetailsProposal">
                        <div className="wrapperControl">
                            <div style={{ flex: 1 }}>
                                {this.renderSubTabs()}
                            </div>
                            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'flex-end' }}>
                                {itemContent !== undefined  &&
                                    <button style={{ marginRight: 10 }} type="button" className="btn btn-warning pull-left" onClick={() => {
                                        let currentItem = this.state.currentItem;
                                        currentItem.items = JSON.parse(JSON.stringify(listItems));
                                        this.setState({ editItem: currentItem, editModal: true })
                                    }}>
                                        <i className="fa fa-edit">
                        </i>   Sửa
                             </button>
                                }
                                {itemContent !== undefined &&
                                    <button style={{ marginRight: 10 }} type="button" className="btn btn-danger pull-left" onClick={() => {
                                        this.onConfirmDelete();
                                    }}>
                                        <i className="fa fa-trash">
                        </i>   xóa
                             </button>
                                }
                                {this.state.feature !== "Proposal" && this.isAllowAddNew() &&
                                    <button style={{ marginRight: 10 }} type="button" className="btn btn-info pull-left" onClick={() => {

                                        this.setState({ editItem: {}, editModal: true })
                                    }}>
                                        Tạo mới
                             </button>
                                }

                                {this.state.feature !== "Quote" && itemContent && this.state.isOutPlan === false &&
                                    <button style={{ marginRight: 10 }} type="button" className="btn btn-success"
                                        onClick={() => {
                                            Actions.openPrintDialog(this.state.feature, this.state.currentID);
                                        }}>   <i style={{ marginRight: '5px' }} className="fa fa-print" aria-hidden="true"></i>In biểu mẫu</button>
                                }
                                {this.state.feature !== "Quote" &&
                                    <button style={{ marginRight: 10 }} type="button" className="btn btn-info pull-left" onClick={() => {
                                        //change to downloadfa-download
                                        this.downloadFile();
                                    }}>
                                      <i style={{ marginRight: '5px' }} className="fa fa-download" aria-hidden="true"></i>Tải File mẫu</button>   
                            
                                }

                                {/* {itemContent &&
                                    <button style={{ marginRight: 10 }} type="button" className="btn btn-info pull-left" onClick={() => {
                                        //change to download
                                        this.downloadDoc();
                                    }}>
                                        Tải file Word
                             </button>
                                } */}

                            </div>
                        </div>
                        {this.state.isOutPlan &&
                            <div style={{ marginTop: 10, width: '100%', textAlign: 'center', color: 'red' }} > <h4>Vui Lòng Tạo Giải Trình: {` ${this.state.outplantext}`} </h4> </div>
                        }

                        {/* {this.checkRenderBarCode()} */}


                        {itemContent &&
                            <ItemContent ItemsDefine={this._ItemsDefine} item={itemContent} ></ItemContent>
                        }
                        {listEmployees && listEmployees.length > 0 &&
                            <ListEmployees items={listEmployees}></ListEmployees>
                        }
                        {listItems &&
                            this.constGetListItem(listItems)
                        }
                        {listDocument &&
                            <ListDocument items={listDocument} feature={this.state.feature}></ListDocument>
                        }


                        {itemContent === undefined && listItems === undefined && listDocument === undefined &&
                            <div style={{ height: '400px', width: '100%', fontSize: 15, fontWeight: 'bold', display: 'flex', justifyContent: 'center', alignItems: 'center' }} >
                                <label >Không có dữ liệu</label>
                            </div>
                        }
                    </div>
                    <div>
                        {this.getRelatedData()}
                    </div>
                    {listComment &&
                        <ListComment onRefreshAfterPost={() => { this.getData(this.state.currentID, false) }} items={listComment} preferId={this.state.currentID} feature={this.state.feature}></ListComment>
                    }

                </div>

            </React.Fragment>
        )
    }

    getListHeader() {
        var Headers = [
            "Đề xuất",
            "Giải trình",
            "Khảo sát",
            "Báo Giá",
            "Họp giá",
            "Kế Hoạch Thầu",
            "BB Thương Thảo HĐ",
            "QĐ Chọn Thầu",
            "Hợp Đồng",
            "BB Giao Nhận",
            "BB Nghiệm Thu"
        ]
        return Headers
    }


    renderTabs() {
        let lst = this.state.listRelateData;
        let index = 3;
        if (lst) {
            if (lst.lstQuote && lst.lstQuote.length > 0) {
                index++
            }
            if (lst.lstAudit && lst.lstAudit.length > 0) {
                index++
            }
            if (lst.lstBidPlan && lst.lstBidPlan.length > 0) {
                index++
            }
            if (lst.lstNegotiation && lst.lstNegotiation.length > 0) {
                index++
            }
            if (lst.lstDecision && lst.lstDecision.length > 0) {
                index++
            }
            if (lst.lstContract && lst.lstContract.length > 0) {
                index++
            }
            if (lst.lstDeliveryReceipt && lst.lstDeliveryReceipt.length > 0) {
                index++
            }

            if (index === 4) {
                if (this.state.isOutPlan === true) {
                    index = 3;
                }
            }
            let arrays = this.getListHeader().slice(0, index);
            return (
                <Tabs onSelect={tabIndex => this.changeTab(tabIndex)} defaultIndex={this.state.tabIndex} key={"number of tab " + index}>
                    <TabList>
                        {arrays.map((item, index) => {
                            return (
                                <Tab key={`tab${index}`}>{item}</Tab>
                            )
                        })}

                    </TabList>
                    {arrays.map((item, index) => {
                        return (
                            <TabPanel key={`TabPanel${index}`}>
                                {this.state.tabIndex === index &&
                                    this.renderBody()
                                }
                            </TabPanel>
                        )
                    })}
                </Tabs>
            )
        }
    }

    onAddEditSuccess(data, type) {
        if (type === 'edit') {
            this.getData(this.state.currentID, false)
        } else {
            if (data && data.id) {
                this.getData(data.id, true)
            }
        }
        this._editModal.clearItem();
        this.setState({ editModal: false });

    }

    downloadDoc() {
        let feature = this.state.feature;
        FileUploaderServices.dowloadDoc(feature, this.state.currentID)
            .then((response) => {
                response.blob().then((blob) => {
                    let url = window.URL.createObjectURL(blob);
                    this._hiddenLink.href = url;
                    this._hiddenLink.download = 'test.doc';
                    this._hiddenLink.click();
                })
            })
    }


    downloadFile() {
        let feature = this.state.feature;
        if (this.state.feature === "DeliveryReceipt") {
            if (this.state.itemContent && this.state.itemContent["deliveryReceiptType"] === 2) {
                feature = "DeliveryReceiptC50";
            } else {
                feature = "DeliveryReceiptC34";
            }
        }
        let fileName = "";
        switch (feature) {
            case "Proposal":
                fileName = "DeXuat.docx";
                break;
            case "Explanation":
                fileName = "GiaiTrinh.docx";
                break;
            case "Survey":
                fileName = "KhaoSat.docx";
                break;
            case "Audit":
                fileName = "HopGia.docx";
                break;
            case "BidPlan":
                fileName = "ChonThau.docx";
                break;
            case "Negotiation":
                fileName = "ThuongThao.docx";
                break;
            case "Decision":
                fileName = "QuyetDinhChonThau.docx";
                break;
            case "Contract":
                fileName = "HopDong.docx";
                break;
            case "DeliveryReceiptC34":
                fileName = "GiaoNhan34.docx";
                break;
            case "DeliveryReceiptC50":
                fileName = "GiaoNhanC50.docx";
                break;
            case "Acceptance":
                fileName = "NghiemThu.docx";
                break;
            default:
                break;
        }
        FileUploaderServices.dowloadTemplate(feature)
            .then((response) => {
                response.blob().then((blob) => {
                    let url = window.URL.createObjectURL(blob);
                    this._hiddenLink.href = url;
                    this._hiddenLink.download = fileName;
                    this._hiddenLink.click();
                })
            })
    }

    render() {
        return (
            <React.Fragment>

                {this.props.isOpen &&
                    <React.Fragment>
                        <a style={{ visibility: "hidden" }} ref={(c) => { this._hiddenLink = c }} />
                        <Loading show={this.state.loading} />
                        {/* {this.state.tabIndex === 0 &&
                    <PrintProposalTemplate onclose={() => { this.setState({ printModal: false }) }} isOpen={this.state.printModal} item={this.state.itemDetail}></PrintProposalTemplate>
                } */}
                        {/* {this.state.tabIndex === 1 &&
                    <PrintExplanationTemplate onclose={() => { this.setState({ printModal: false }) }} isOpen={this.state.printModal} item={this.state.explantionDetail}></PrintExplanationTemplate>
                } */}


                        {this.props.isOpen === true && this.state.editModal === false && this.state.confirmModal === false &&
                            <div onClick={() => { this.props.onclose() }}
                                className="closeIcon">
                                <i className="fa fa-window-close" aria-hidden="true"></i>
                            </div>
                        }

                        <Modal isOpen={this.props.isOpen}>
                            <div style={{ width: '94%', margin: 'auto', minHeight: '96vh', position: 'relative' }}>
                                {this.renderTabs()}
                            </div>
                            {this.state.editModal === true &&
                                <ModalCreateEdit
                                    ref={(c) => this._editModal = c}
                                    item={this.state.editItem}
                                    listCurrentData={this.state.listRelateData}
                                    proposalID={this.state.proposalID}
                                    proposalCode={this.state.proposalCode}
                                    feature={this.state.feature}
                                    itemModal={this.state.editModal}
                                    onSuccess={this.onAddEditSuccess.bind(this)}
                                    onClose={() => { this.setState({ editModal: false }) }}
                                ></ModalCreateEdit>
                            }
                        </Modal>
                        {this.state.confirmModal === true &&
                            <ConfirmDialog
                                onConfirm={this.onDeleteData.bind(this)}
                                Modal={this.state.confirmModal}
                                onCancel={() => { this.setState({ confirmModal: false }) }}
                                content={this.state.confirmContent}
                            />
                        }
                    </React.Fragment>
                }
            </React.Fragment>
        );
    }
};
