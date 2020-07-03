
import moment from 'moment';
import * as React from 'react';
import { CONST_FEATURE } from '../../commons';
import * as FUNC from '../itemDetail/Funcs';
import './main.css';
import * as PrepareData from './prepareData';
//import SUNEDITOR from 'suneditor';
var Barcode = require('react-barcode');


export default class PrintTemplate extends React.Component {
    _getDataFunc;
    _preDataFunc;
   
    state = {
       /// editorHtml: '',
     ///   theme: 'snow',
        itemContent: {} ,
        listItems: {} ,
        currentID: 0,
        loading: false,
     //   Editor: {} ,
        code: {} ,
        currentDate: '',
        barCode: '',

        printFlag : false,
        printcontent: '',


        item:null
    }

    getUrlParameter = (name) => {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        let regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        let results = regex.exec(window.location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };


    componentDidMount() {
        // this._editer = SUNEDITOR.create('editor', {
        //     font: [
        //         'Time News Roman',
        //         'Arial',
        //         'Courier New,Courier'
        //     ],
        //     fontSize: [
        //         8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 20, 22, 24, 26, 30, 32, 36, 40
        //     ],
        //     fontSizeUnit: 'pt',

        //     showPathLabel: false,
        //     charCounter: true,
        //     maxCharCount: 13000,
        //     width: '210mm',
        //     maxWidth: '210mm',
        //     height: 'auto',
        //     minHeight: '78vh',
        //     maxHeight: '78vh',
        //     buttonList: [
        //         ['undo', 'redo', 'font', 'fontSize', 'formatBlock'],
        //         ['bold', 'underline', 'italic', 'strike', 'subscript', 'superscript', 'removeFormat', 'print'],
        //         '/',// Line break
        //         ['fontColor', 'hiliteColor', 'outdent', 'indent', 'align', 'horizontalRule', 'list', 'table'],

        //     ],
        //     callBackSave: function (contents) {

        //     }
        //});

        let feature = this.props.feature;
        let id = this.props.id;
        if (feature) {
            switch (feature) {
                case CONST_FEATURE.Proposal.feature:
                    this._getDataFunc = FUNC.getProposalItem;
                    this._preDataFunc = PrepareData.prepareProposal;
                    break;
                case CONST_FEATURE.Explanation.feature:
                    this._getDataFunc = FUNC.getExplanationItem;
                    this._preDataFunc = PrepareData.prepareExplanation;
                    break;
                case CONST_FEATURE.Survey.feature:
                    this._getDataFunc = FUNC.getSurveyItem;
                    this._preDataFunc = PrepareData.prepareSurvey;
                    break;
                case CONST_FEATURE.Quote.feature:
                    this._getDataFunc = FUNC.getQuoteItem;
                    break;
                case CONST_FEATURE.Audit.feature:
                    this._getDataFunc = FUNC.getAuditItem;
                    this._preDataFunc = PrepareData.prepareAudit;
                    break
                case "AuditWithItemPrice":
                    this._getDataFunc = FUNC.getAuditItem;
                    this._preDataFunc = PrepareData.prepareAuditWithItemPrice;
                    break
                    
                case CONST_FEATURE.BidPlan.feature:
                    this._getDataFunc = FUNC.getBidPlanItem;
                    this._preDataFunc = PrepareData.prepareBidPlan;
                    break;
                case CONST_FEATURE.Negotiation.feature:
                    this._getDataFunc = FUNC.getNegotiationItem;
                    this._preDataFunc = PrepareData.prepareNegotiation;
                    break;
                case CONST_FEATURE.Decision.feature:
                    this._getDataFunc = FUNC.getDecisionItem;
                    this._preDataFunc = PrepareData.prepareDecision;
                    break;
                case CONST_FEATURE.Contract.feature:
                    this._getDataFunc = FUNC.getContractItem;
                    this._preDataFunc = PrepareData.prepareContract;
                    break;
                case CONST_FEATURE.DeliveryReceipt.feature:
                    this._getDataFunc = FUNC.getDeliveryReceiptItem;
                    // this._preDataFunc = PrepareData.prepareDeliveryReceipt;
                    break;
                case CONST_FEATURE.Acceptance.feature:
                    this._getDataFunc = FUNC.getAcceptanceItem;
                  //  this._preDataFunc = PrepareData.prepareAcceptance;
                    break;
                default:
                    this._getDataFunc = undefined
                    break;
            }
        }
 
        if (this._getDataFunc && id !== 0) {
            this.setState({ loading: true })
            this._getDataFunc(id).then(
                (item) => {                
                    if (CONST_FEATURE.DeliveryReceipt.feature === feature) {
                        switch (item.itemContent.deliveryReceiptType){
                            case 1 :
                                this._preDataFunc = PrepareData.prepareDeliveryReceiptC34;
                                break;
                            case 2 :
                                this._preDataFunc = PrepareData.prepareDeliveryReceiptC34;
                                break;
                            case 3 : 
                                this._preDataFunc = PrepareData.prepareDeliveryReceiptInternal;
                                break;
                            default : 
                            this._preDataFunc = null;
                        }
                      
                    }
            
                    if (CONST_FEATURE.Acceptance.feature === feature) {
                        if(item.itemContent.acceptanceType == 1){
                            this._preDataFunc = PrepareData.prepareAcceptance;
                        }else{
                            this._preDataFunc = PrepareData.prepareAcceptancePrepair;
                        }
                    }

                    if (CONST_FEATURE.Proposal.feature === feature) {
                        this.setState({ barCode: item.itemContent.proposalCode })
                    }
                    if (CONST_FEATURE.Explanation.feature === feature) {
                        this.setState({ barCode: item.itemContent.explanationCode })
                    }
                    if (CONST_FEATURE.Survey.feature === feature) {
                        this.setState({ barCode: item.itemContent.surveyCode })
                    }
                    if (this._preDataFunc) {
                        this.setState({item})
                       
                    }
                  
                 //   this._editer.setContents(raw);
                 this.setState({})
                    window.print();
                }).catch((er) => {
                    console.log(er)
                    this.setState({ loading: false })
                })
        }
    }

    // handleChange(html) {
    //     this.setState({ editorHtml: html });
    // }


    downloadWord() {
        var header = `<html xmlns:v="urn:schemas-microsoft-com:vml"
        xmlns:o="urn:schemas-microsoft-com:office:office"
        xmlns:w="urn:schemas-microsoft-com:office:word"
        xmlns:m="http://schemas.microsoft.com/office/2004/12/omml"
        xmlns="http://www.w3.org/TR/REC-html40">
        <head><meta http-equiv=Content-Type content="text/html; charset=utf-8"><title></title>
        <style>
        v\:* {behavior:url(#default#VML);}
        o\:* {behavior:url(#default#VML);}
        w\:* {behavior:url(#default#VML);}
        .shape {behavior:url(#default#VML);}
        </style>
        <style>
        @page
        {
            mso-page-orientation: landscape;
            size:29.7cm 21cm;    margin:1cm 1cm 1cm 1cm;
        }
        @page Section1 {
            mso-header-margin:.5in;
            mso-footer-margin:.5in;
            mso-header: h1;
            mso-footer: f1;
            }
        div.Section1 { page:Section1; }
        table#hrdftrtbl
        {
            margin:0in 0in 0in 900in;
            width:1px;
            height:1px;
            overflow:hidden;
        }
        p.MsoFooter, li.MsoFooter, div.MsoFooter
        {
            margin:0in;
            margin-bottom:.0001pt;
            mso-pagination:widow-orphan;
            tab-stops:center 3.0in right 6.0in;
            font-size:12.0pt;
        }
        </style>
        <xml>
        <w:WordDocument>
        <w:View>Print</w:View>
        <w:Zoom>100</w:Zoom>
        <w:DoNotOptimizeForBrowser/>
        </w:WordDocument>
        </xml>
        </head>`;
        var content = "<body>" + this._editer.getContents(true) + "</body></html>";
        var sourceHTML = header + content

        var source = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(sourceHTML);
        var fileDownload = document.createElement("a");
        document.body.appendChild(fileDownload);
        fileDownload.href = source;
        fileDownload.download = 'document.doc';
        fileDownload.click();
        document.body.removeChild(fileDownload);
    }


    ShowBarCode() {
        if (this.state.barCode && this.state.barCode !=='') {
            let top = 130;
            if(this.props.feature === CONST_FEATURE.Survey.feature){
                top = 220;
            }
            return (
                <React.Fragment>
                     <div className={"barcoreProposal notshowwhenprint"} style={{ position: 'absolute', top: top, right: "-65px", zIndex: 10000 }}>
                     <Barcode value={this.state.barCode} width={1} height={50} fontSize={11} textMargin={1} />
                    </div>              
                </React.Fragment>
            )
        }
    }

    render() {
            let {item} =this.state
        return (
            <React.Fragment>
                {this.ShowBarCode()}
                <div style={{ display: 'flex', flexDirection: 'row' }} className={"notshowwhenprint"}>
                    <div style={{ width: '220mm', margin: 20, border: '1px solid #ddd', backgroundColor: '#bbb' }}>
                        {/* <textarea id="editor"></textarea> */}
                        <div className={"printarea"}>
                            {item && 
                             this._preDataFunc(item, moment().format(`DD/MM/YYYY`))}
                        </div>
                    </div>
                </div>
            </React.Fragment>
        );
    }
}
