
import "@kenshooui/react-multi-select/dist/style.css";
import moment from 'moment';
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import ListAttachfiles from '../../commons/controls/listAttachfiles';
import DatePickerCustom from '../../commons/controlsSimple/datePicker';
import InputCustom from '../../commons/controlsSimple/input';
import SelectCustom from '../../commons/controlsSimple/select';
import { AuditLocationArr, NegotiationBankIDArr, NegotiationTermArr, BidTypeArr } from '../../commons/propertiesType';
import FileUploader from '../../components/fileUploader';
import WordDownload from '../../components/wordDownload';
import * as Actions from '../../libs/actions';
import * as NegotiationService from '../../services/negotiationService';
import ListItemQuote from './listItemQuote';
import './negotiation.css';



export default class negotiationDetais extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            activeTab: 0,
            files: [],
            listItem: [],
            item: {},
            isEdit: false
        }
    }




    componentWillMount() {




    }

    static getDerivedStateFromProps(nextProps, prevState) {
        let item = nextProps.item;
        for (let location of AuditLocationArr) {
            if (item.location)
                if (item.location.toString() == location.value.toString()) {
                    item.location = location;
                }
        }
        for (let bankID of NegotiationBankIDArr) {
            if (item.bankID.toString() == bankID.value.toString()) {
                item.bankID = bankID;
            }
        }

        for (let term of NegotiationTermArr) {
            if (item.term.toString() == term.value.toString()) {
                item.term = term;
            }
        }
        let temp = {};
        if (item.bidType && item.bidType.value == undefined) {
            if (item.bidType == "Hợp đồng trọn gói") {
                temp.value = 1;
                temp.label = "Hợp đồng trọn gói";
            } else {
                temp.value = 2;
                temp.label = 'Đơn giá cố định';
            }
            item.bidType = temp;
        }
        temp = {};
        if (item.bidExpiratedUnit && item.bidExpiratedUnit.value == undefined) {
            if (item.bidExpiratedUnit == "Ngày") {
                temp.value = 1;
                temp.label = "Ngày";
            } else {
                temp.value = 2;
                temp.label = 'Tháng';
            }
            item.bidExpiratedUnit = temp;
        }


        return {
            item: item
        };
    }

    updateNew() {
        let itemTemp = this.state.item;

        let item = JSON.parse(JSON.stringify(itemTemp));
        item.inTime = moment(item.inTime, 'DD-MM-YYYY').format('YYYY-MM-DD');
        item.dateIn = moment(item.dateIn, 'DD-MM-YYYY HH:mm').format('YYYY-MM-DD HH:mm');

        var data = new FormData();
        item.bidType = item.bidType.label;

        item.bidExpiratedUnit = item.bidExpiratedUnit.label;
        item.term = item.term.value;
        item.bankID = item.bankID.value;
        item.location = item.location.value;

        Object.keys(item).forEach(key => {
            if (key == "quoteCode") {
                data.append("quoteID", item["quoteCode"].value);

            } else {
                if (key !== 'listDocument') {
                    data.append(key, item[key]);
                }
            }
        });
        
        let files = this._fileUploader.getFiles();
        if (files.length > 0) {
            for (let file of files) {
                data.append('files', file);
            }
        }
        if (item.listDocument)
            for (let i = 0; i < item.listDocument.length; i++) {
                for (let key in item.listDocument[i]) {
                    data.append("listDocument[" + i + "]." + key, item.listDocument[i][key]);
                }
            }
            Actions.setLoading(true);
        NegotiationService.updateNegotiationwithAttFiles(item["negotiationID"], data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    this.props.onUpdateItem(itemTemp);
                    this.setState({ isEdit: false })
                    Actions.setLoading(false);
                } else {
                    //  Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }).catch(err => {
            })
    }



    renderGeneral() {
        if (this.state.item == undefined) return;
        let item = (this.state.item)
        return (
            <React.Fragment>

                <div style={{
                    position: 'absolute',
                    top: 10,
                    right: 20
                }}>
                    <WordDownload feature="Negotiation" id={item.negotiationID} name="Thương_thảo_hợp_đồng.doc" ></WordDownload>

                    <button style={{ marginRight: 10 }} type="button" className="btn btn-success"
                        onClick={() => {
                            Actions.openPrintDialog("Negotiation", item.negotiationID);
                        }}>   <i style={{ marginRight: '5px' }} className="fa fa-print" aria-hidden="true"></i>In biểu mẫu</button>
                    {this.state.isEdit == false ?
                        <Button className={"btn btn-danger"} onClick={() => {
                            this.setState({ isEdit: true })
                        }} > Sửa</Button>
                        :
                        <Button className={"btn btn-info"}
                            onClick={() => {

                                this.updateNew();


                            }}
                        > Lưu</Button>
                    }
                </div>

                <div style={{
                    display: 'flex', margin: "50px 20px 20px 20px", justifyContent: 'space-between',
                    flexWrap: 'wrap'
                }}>
                    <SelectCustom
                        doubleWidth={true}
                        disabled={!this.state.isEdit}
                        key={"location"}
                        label={"Địa điểm"}
                        options={AuditLocationArr}
                        value={item.location}
                        onChange={(value) => {
                            item.location = value
                            this.setState({ item })
                        }} />
                    <InputCustom
                        disabled={!this.state.isEdit}
                        type={'text'}
                        key={"negotiationCode"}
                        label={"Mã Thương Thảo Hợp Đồng"}
                        value={item.negotiationCode}
                        onChange={(value) => {
                            item.negotiationCode = value
                            this.setState({ item })
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}
                        key={"quoteCode"}
                        label={"Mã báo giá"}
                        value={item.quoteCode}
                        onChange={(value) => {

                        }} />
                    <DatePickerCustom
                        disabled={!this.state.isEdit}
                        label={"Ngày tạo biên bản"}
                        value={item.dateIn}
                        showTimeSelect={true}
                        onChange={(value) => {
                            item.dateIn = value
                            this.setState({ item })
                        }} />



                    <InputCustom
                        doubleWidth={true}
                        disabled={true}
                        type={'text'}
                        key={"customerName"}
                        label={"Tên công ty"}
                        value={item.customerName}
                        onChange={(value) => {

                        }} />
                    <InputCustom
                        disabled={!this.state.isEdit}
                        type={'text'}
                        key={"phone"}
                        label={"Số điện thoại"}
                        value={item.phone}
                        onChange={(value) => {
                            item.phone = value
                            this.setState({ item })
                        }} />
                    <InputCustom
                        disabled={!this.state.isEdit}
                        type={'text'}
                        key={"Fax"}
                        label={"Fax"}
                        value={item.fax}
                        onChange={(value) => {
                            item.fax = value
                            this.setState({ item })
                        }} />
                    <InputCustom
                        disabled={!this.state.isEdit}
                        type={'text'}
                        key={"taxCode"}
                        label={"Mã số thuế"}
                        value={item.taxCode}
                        onChange={(value) => {
                            item.taxCode = value
                            this.setState({ item })
                        }} />
                    <SelectCustom
                        doubleWidth={true}
                        disabled={!this.state.isEdit}
                        key={"bankID"}
                        label={"Tài khoản ngân hàng"}
                        options={NegotiationBankIDArr}
                        value={item.bankID}
                        onChange={(value) => {
                            item.bankID = value
                            this.setState({ item })
                        }} />
                   

                    <InputCustom
                        disabled={!this.state.isEdit}
                        type={'text'}
                        key={"represent"}
                        label={"Đại diện"}
                        value={item.represent}
                        onChange={(value) => {
                            item.represent = value
                            this.setState({ item })
                        }} />
                    <InputCustom

                        disabled={!this.state.isEdit}
                        type={'text'}
                        key={"position"}
                        label={"Chức vụ"}
                        value={item.position}
                        onChange={(value) => {
                            item.position = value
                            this.setState({ item })
                        }} />


                    
                    <SelectCustom

                        disabled={!this.state.isEdit}
                        key={"term"}
                        label={"Thời hạn thanh toán"}
                        options={NegotiationTermArr}
                        value={item.term}
                        onChange={(value) => {
                            item.term = value
                            this.setState({ item })
                        }} />
                        <SelectCustom

                        disabled={!this.state.isEdit}
                        key={"bidType"}
                        label={"Loại hợp đồng"}
                        options={BidTypeArr}
                        value={item.bidType}
                        onChange={(value) => {
               
                            item.bidType = value
                            this.setState({ item })
                        }} />

                        <InputCustom

                        disabled={!this.state.isEdit}
                        type={'text'}
                        key={"bidExpirated"}
                        label={"Thời gian thực hiện"}
                        value={item.bidExpirated}
                        onChange={(value) => {
                            item.bidExpirated = value
                            this.setState({ item })
                        }} />

                        <SelectCustom

                        disabled={!this.state.isEdit}
                        key={"bidExpiratedUnit"}
                        label={"Tháng/Ngày"}
                        options={[
                            {
                                  label:'Ngày',
                                  value:'Ngày'
                            },{
                                  label:'Tháng',
                                  value:'Tháng'
                            }
                    
                        ]}
                        value={item.bidExpiratedUnit}
                        onChange={(value) => {
                            item.bidExpiratedUnit = value
                            this.setState({ item })
                        }} />
                </div>

            </React.Fragment>
        )
    }

    renerQuoteDetails() {
        let item = this.props.item

        if (item != null) {
            return (

                <div style={{ padding: 20 }}>
                    <ListItemQuote
                        VAT={item.isVAT}
                        vatNumber={item.vatNumber}
                        items={item.items}
                        onChange={(value) => { }}
                    />
                </div>



            )
        }

    }

    render() {
        let { item } = this.state
        return (
            <div>
                {this.props.item &&
                    <Modal isOpen={this.props.Modal}>
                        {this.props.Modal &&
                            <div onClick={() => { this.props.onCancel() }}
                                className="closeIcon">
                                <i className="fa fa-window-close" aria-hidden="true"></i>
                            </div>
                        }
                        <ModalHeader>
                            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                {this.props.header}
                            </div>
                        </ModalHeader>
                        <ModalBody style={{
                            position: 'relative',
                            flex: '1 1 auto',
                            padding: '20px',
                            width: '98%',
                            minHeight: '300px',
                            justifyContent: 'center',
                            alignItems: 'center',
                            fontSize: '12px',
                            flexDirection: 'row',
                        }}>
                            <div>
                                {this.renderGeneral()}
                                {this.renerQuoteDetails()}
                                <ListAttachfiles
                                    isShowRemove={true}
                                    item={item['listDocument']}
                                    isDisable={!this.state.isEdit}
                                    onRemove={(value) => {

                                        let curItem = this.state.item;
                                        curItem['listDocument'] = value;

                                        this.setState({ item: curItem })
                                    }}
                                    feature={'Negotiation'}
                                />
                            </div>
                            {this.state.isEdit &&
                                <div style={{ marginBottom: 20, marginTop: 15 }}>
                                    <FileUploader ref={(c) => { this._fileUploader = c }} />
                                </div>
                            }
                        </ModalBody>
                        <div style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            fontSize: '12px',
                            marginBottom: '30px',
                            marginTop: '20px'
                        }}>

                            <Button className="btn-default" style={{ width: '100px', marginLeft: 30 }} onClick={() => { this.props.onCancel() }}>Tắt</Button>
                        </div>
                    </Modal>
                }
            </div>
        );
    }
}