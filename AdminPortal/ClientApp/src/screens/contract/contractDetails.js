
import "@kenshooui/react-multi-select/dist/style.css";
import moment from 'moment';
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import DatePickerCustom from '../../commons/controlsSimple/datePicker';
import InputCustom from '../../commons/controlsSimple/input';
import SelectCustom from '../../commons/controlsSimple/select';
import { AuditLocationArr, NegotiationBankIDArr, NegotiationTermArr } from '../../commons/propertiesType';
import WordDownload from '../../components/wordDownload';
import * as Actions from '../../libs/actions';
import * as contractService from '../../services/contractService';
import './contract.css';
import ListItemQuote from './listItemQuote';
import ListAttachfiles from '../../commons/controls/listAttachfiles';
import FileUploader from '../../components/fileUploader';

export default class contractDetais extends React.Component {

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

    static getDerivedStateFromProps(nextProps, prevState) {
        let item = nextProps.item;
        if(item.negotiation){
            for (let location of AuditLocationArr) {
                if (item.negotiation.location)
                    if (item.negotiation.location.toString() == location.value.toString()) {
                        item.negotiation.location = location;
                    }
            }
            for (let bankID of NegotiationBankIDArr) {
                if (item.negotiation.bankID.toString() == bankID.value.toString()) {
                    item.negotiation.bankID = bankID;
                }
            }

            for (let term of NegotiationTermArr) {
                if (item.negotiation.term.toString() == term.value.toString()) {
                    item.negotiation.term = term;
                }
            }
        }
        return {
            item: item
        };
    }

    updateNew() {
        let itemTemp = this.state.item;

        let item = JSON.parse(JSON.stringify(itemTemp));
        item.dateIn = moment(item.dateIn, 'DD-MM-YYYY HH:mm').format('YYYY-MM-DD HH:mm');
        var data = new FormData();


        Object.keys(item).forEach(key => {
            if (key == "quoteID") {
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
        contractService.updateContractwithAttFiles(item["contractID"], data)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {

                    this.props.onUpdateItem(itemTemp);
                    this.setState({ isEdit: false })
                    Actions.setLoading(false);
                } else {
                     Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
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
                    <WordDownload feature="Contract" id={item.contractID} name="Hợp_đồng.doc" ></WordDownload>

                    <button style={{ marginRight: 10 }} type="button" className="btn btn-success"
                        onClick={() => {
                            Actions.openPrintDialog("Contract", item.contractID);
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
                    <InputCustom
                        disabled={!this.state.isEdit}
                        type={'text'}
                        key={"contractCode"}
                        label={"Mã  Hợp Đồng"}
                        value={item.contractCode}
                        onChange={(value) => {
                            item.contractCode = value
                            this.setState({ item })
                        }} />
                    <DatePickerCustom
                        disabled={!this.state.isEdit}
                        label={"Ngày tạo hợp đồng"}
                        value={item.dateIn}
                        showTimeSelect={true}
                        onChange={(value) => {
                            item.dateIn = value
                            this.setState({ item })
                        }} />

                    <SelectCustom
                        doubleWidth={true}
                        disabled={true}

                        label={"Địa điểm"}
                        options={AuditLocationArr}
                        value={item.negotiation.location}
                        onChange={(value) => {
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}

                        label={"Mã báo giá"}
                        value={item.negotiation.quoteCode}
                        onChange={(value) => {

                        }} />

                    <InputCustom
                        doubleWidth={true}
                        disabled={true}
                        type={'text'}

                        label={"Tên công ty"}
                        value={item.negotiation.customerName}
                        onChange={(value) => {

                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}

                        label={"Số điện thoại"}
                        value={item.negotiation.phone}
                        onChange={(value) => {
                            item.phone = value
                            this.setState({ item })
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}

                        label={"Fax"}
                        value={item.negotiation.fax}
                        onChange={(value) => {
                            item.fax = value
                            this.setState({ item })
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}

                        label={"Mã số thuế"}
                        value={item.negotiation.taxCode}
                        onChange={(value) => {
                            item.taxCode = value
                            this.setState({ item })
                        }} />

                    <SelectCustom
                        doubleWidth={true}
                        disabled={true}

                        label={"Tài khoản ngân hàng"}
                        options={NegotiationBankIDArr}
                        value={item.negotiation.bankID}
                        onChange={(value) => {
                            item.bankID = value
                            this.setState({ item })
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}

                        label={"Đại diện"}
                        value={item.negotiation.represent}
                        onChange={(value) => {
                            item.represent = value
                            this.setState({ item })
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}
                        key={"position"}
                        label={"Chức vụ"}
                        value={item.negotiation.position}
                        onChange={(value) => {
                            item.position = value
                            this.setState({ item })
                        }} />

                    <SelectCustom
                        disabled={true}
                        key={"term"}
                        label={"Thời hạn thanh toán"}
                        options={NegotiationTermArr}
                        value={item.negotiation.term}
                        onChange={(value) => {
                            item.term = value
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
                        VAT={item.negotiation.isVAT}
                        vatNumber={item.negotiation.vatNumber}
                        items={item.negotiation.items}
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
                                    feature={'Contract'}
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