
import "@kenshooui/react-multi-select/dist/style.css";
import moment from 'moment';
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import DatePickerCustom from '../../commons/controlsSimple/datePicker';
import InputCustom from '../../commons/controlsSimple/input';
import SelectCustom from '../../commons/controlsSimple/select';
import { AuditLocationArr  , DeliveryReceiptTypeArr} from '../../commons/propertiesType';
import ListEmployeeDeliveryReceipt from '../../components/editCreateItemModal/addEditListItems/listEmployeeDeliveryReceipt';
import * as Actions from '../../libs/actions';
import * as DeliveryReceiptService from '../../services/deliveryReceiptService';
import './deliveryReceipt.css';
import ListItemQuote from './listItemQuote';
import ListAttachfiles from '../../commons/controls/listAttachfiles';
import FileUploader from '../../components/fileUploader';

import WordDownload from '../../components/wordDownload';

export default class auditDetails extends React.Component {

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
        for (let location of AuditLocationArr) {
            if (item.deliveryReceiptPlace)
                if (item.deliveryReceiptPlace.toString() == location.value.toString()) {
                    item.deliveryReceiptPlace = location;
                    break;
                }
        }
        for (let type of DeliveryReceiptTypeArr) {
            if (item.deliveryReceiptType)
                if (item.deliveryReceiptType.toString() == type.value.toString()) {
                    item.deliveryReceiptType = type;
                    break;
                }
        }
        return {
            item: item
        };
    }


    updateNew() {
        let itemTemp = this.state.item;
        let item = JSON.parse(JSON.stringify(itemTemp));
  


        item.dateIn = moment(item.dateIn, 'DD-MM-YYYY').format('YYYY-MM-DD');
        item.inTime = moment(item.inTime, 'DD-MM-YYYY').format('YYYY-MM-DD');
        item.deliveryReceiptDate = moment(item.deliveryReceiptDate, 'DD-MM-YYYY').format('YYYY-MM-DD');

        var data = new FormData();
        item.deliveryReceiptPlace = item.deliveryReceiptPlace.value
        item.deliveryReceiptType = item.deliveryReceiptType.value
        Object.keys(item).forEach(key => {
            if (key !== 'items' && key !== 'employees' && key!== 'listDocument') {
                if (key == "proposalCode") {
                    data.append("proposalCode", item[key].label);
                    data.append("proposalID", item[key].value);
                } else {
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
        if (item.items) {
            for (let i = 0; i < item.items.length; i++) {
                for (let key in item.items[i]) {
                    data.append("items[" + i + "]." + key, item.items[i][key]);
                }
            }
        }

        if (item.listDocument) {
            for (let i = 0; i < item.listDocument.length; i++) {
                for (let key in item.listDocument[i]) {
                    data.append("listDocument[" + i + "]." + key, item.listDocument[i][key]);
                }
            }
        }

        if (item.employees) {
            for (let i = 0; i < item.employees.length; i++) {
                for (let key in item.employees[i]) {
                    data.append("employees[" + i + "]." + key, item.employees[i][key]);
                }
            }
        }
        Actions.setLoading(true);
        DeliveryReceiptService.updateDeliveryReceipt(data)
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

                    <WordDownload feature="DeliveryReceipt" id={item.deliveryReceiptID} name={`BBGN${[item.deliveryReceiptType.label]}.doc`}></WordDownload>
                    <button style={{ marginRight: 10 }} type="button" className="btn btn-success"
                        onClick={() => {
                            Actions.openPrintDialog("DeliveryReceipt", item.deliveryReceiptID);
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
                        key={"deliveryReceiptCode"}
                        label={"Mã biên bản giao nhận"}
                        value={item.deliveryReceiptCode}
                        onChange={(value) => {
                            item.deliveryReceiptCode = value
                            this.setState({ item })
                        }} />
                    <InputCustom
                        disabled={true}
                        type={'text'}
                        label={"mã đề xuất"}
                        value={item.proposalCode}
                        onChange={(value) => {

                        }} />
                    <InputCustom
                        disabled={true}
                        type={'text'}

                        label={"Mã báo giá"}
                        value={item.quoteCode}
                        onChange={(value) => {
                            item.quoteCode = value
                            this.setState({ item })
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}
                        label={"Khoa/phòng đề xuất"}
                        value={item.departmentName}
                        onChange={(value) => {
                            item.departmentName = value
                            this.setState({ item })
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}
                        label={"Khoa/phòng phụ trách"}
                        value={item.curDepartmentName}
                        onChange={(value) => {
                            item.curDepartmentName = value
                            this.setState({ item })
                        }} />

                    <SelectCustom
                        disabled={!this.state.isEdit}
                        key={"location"}
                        label={"Địa điểm"}
                        options={AuditLocationArr}
                        value={item.deliveryReceiptPlace}
                        onChange={(value) => {
                            item.deliveryReceiptPlace = value
                            this.setState({ item })
                        }} />
                        <SelectCustom
                        disabled={!this.state.isEdit}
                        key={"Loại"}
                        label={"Loại"}
                        options={DeliveryReceiptTypeArr}
                        value={item.deliveryReceiptType}
                        onChange={(value) => {
                            item.deliveryReceiptType = value
                            this.setState({ item })
                        }} />
                    <DatePickerCustom
                        disabled={!this.state.isEdit}
                        label={"Ngày tạo biên bản"}
                        value={item.deliveryReceiptDate}
                        onChange={(value) => {
                            item.deliveryReceiptDate = value
                            this.setState({ item })
                        }} />


                </div>
                {/* <div style={{ margin: 20 }}>

                    <ListEmployeeDeliveryReceipt
                        itemDefine={{
                            header: "Thành phần",
                            name: "employees",
                            type: "listEmployeeDeliveryReceipt",
                            getDataFunc: DeliveryReceiptService.getListEmployee,
                            IsFull: true,

                        }}
                        items={item.employees}
                        onChange={(value) => {
                            item.employees = value
                            this.setState(item)
                        }}>
                    </ListEmployeeDeliveryReceipt>

                </div> */}
            </React.Fragment>
        )
    }

    renerQuoteDetails() {
        let item = this.props.item

        if (item != null) {
            let items = item.items;
            if (items)
                return (
                    <React.Fragment>
                        <div style={{ flex: 1, display: 'flex', border: '1px solid rgb(233,233,233)' }}>
                            {/*  item */}
                            <div style={{ flex: 2, borderRight: '1px solid rgb (233,233,233)' }}>
                                <div style={{ marginRight: 10, marginLeft: 10 }}>
                                    <ListItemQuote
                                        disabled={!this.state.isEdit}
                                        VAT={item.isVAT}
                                        vatNumber={item.vatNumber}
                                        items={item.items}
                                        onChange={(value) => { }}
                                        proposalCode={item.proposalCode}
                                    />
                                </div>
                            </div>
                        </div>
                    </React.Fragment>
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
                                    feature={'DeliveryReceipt'}
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