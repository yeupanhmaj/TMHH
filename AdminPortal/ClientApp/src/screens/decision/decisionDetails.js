
import moment from 'moment';
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import DatePickerCustom from '../../commons/controlsSimple/datePicker';
import InputCustom from '../../commons/controlsSimple/input';
import SelectCustom from '../../commons/controlsSimple/select';

import WordDownload from '../../components/wordDownload';
import * as Actions from '../../libs/actions';
import * as DecisionService from '../../services/decisionService';
import './decision.css';
import ListItemQuote from './listItemQuote';

import ListAttachfiles from '../../commons/controls/listAttachfiles';
import FileUploader from '../../components/fileUploader';


import AutoCompleteCustom from '../../commons/controls/autoComplete';
import * as CapitalService from '../../services/capitalService';
import { BidMethodArr } from '../../commons/propertiesType';

const cappitalDefine = {
    header: "Nguồn vốn",
    name: "capitalID",
    type: "autoComplete",
    getDataFunc: CapitalService.GetCapitalByName,
    labelCol: 'capitalName',
    valueCol: 'capitalID',
}


export default class decisionDetails extends React.Component {

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
        for (let bidMethod of BidMethodArr) {
            if (item.bidMethod.toString() == bidMethod.value.toString()) {
                item.bidMethod = bidMethod;
            }
        }
        return {
            item: item
        };
    }

    updateNew() {
        let itemTemp = this.state.item;
        let item = JSON.parse(JSON.stringify(itemTemp));

        item.bidMethod = item.bidMethod.value;
        item.dateIn = moment(item.dateIn, 'DD-MM-YYYY').format('YYYY-MM-DD');


        var data = new FormData();


        Object.keys(item).forEach(key => {
            if (key !== 'listDocument') {
                data.append(key, item[key]);
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
        DecisionService.updateDecisionwithAttFiles(item["decisionID"], data)
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
                    <WordDownload feature="Decision" id={item.decisionID} name="Quyết_định_Chọn_nhà_cung_cấp.doc" ></WordDownload>

                    <button style={{ marginRight: 10 }} type="button" className="btn btn-success"
                        onClick={() => {
                            Actions.openPrintDialog("Decision", item.decisionID);
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
                        key={"decisionCode"}
                        label={"Mã hồ sơ quyết định chọn thầu"}
                        value={item.decisionCode}
                        onChange={(value) => {
                            item.decisionCode = value
                            this.setState({ item })
                        }} />

                    <InputCustom
                        disabled={true}
                        type={'text'}
                        label={"Mã báo giá"}
                        value={item.quoteCode}
                        onChange={(value) => {

                        }} />

                        <div style={{ marginTop: 15 }}>
                        <AutoCompleteCustom
                            values={cappitalDefine.values}
                            width={250}
                            allowNull={cappitalDefine.allowNull}
                            labelCol={cappitalDefine.labelCol ? cappitalDefine.labelCol : 'label'}
                            valueCol={cappitalDefine.valueCol ? cappitalDefine.valueCol : 'value'}
                            key={"autoComplete" + cappitalDefine.name}
                            name={cappitalDefine.name}
                            header={cappitalDefine.header}
                            value={(item)[cappitalDefine.name]}
                            getData={cappitalDefine.getDataFunc ? cappitalDefine.getDataFunc.bind(this) : null}
                            isDisable={!this.state.isEdit}
                            defaultValue={cappitalDefine.valueDefault || undefined}
                            onChange={(value) => {
                                item[cappitalDefine.name] = value.value
                                this.setState({ item })
                            }
                            } />
                    </div>
                    <DatePickerCustom
                        disabled={!this.state.isEdit}
                        label={"Ngày tạo quyết Định chọn thầu"}
                        value={item.dateIn}
                        onChange={(value) => {
                            item.dateIn = value
                            this.setState({ item })
                        }} />
                         <SelectCustom
                        disabled={!this.state.isEdit}
                        options={BidMethodArr}
                        label={"Hình thức và phương thức lựa chọn"}
                        value={item.bidMethod}
                        onChange={(value) => {
                            item.bidMethod = value
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
        let { item } = this.state;
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
                                    feature={'Decision'}
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