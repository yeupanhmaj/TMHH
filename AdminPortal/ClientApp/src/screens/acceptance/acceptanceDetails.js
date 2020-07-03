
import "@kenshooui/react-multi-select/dist/style.css";
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import InputCustom from '../../commons/controlsSimple/input';

import SelectCustom from '../../commons/controlsSimple/select';
import { acceptanceResultArr } from '../../commons/propertiesType';
import * as Actions from '../../libs/actions';
import * as AcceptanceService from '../../services/acceptanceService';
import './acceptance.css';

import ListItemAcceptance from './listItemAcceptance';
import ListAttachfiles from '../../commons/controls/listAttachfiles';
import FileUploader from '../../components/fileUploader';
import TextArea from '../../commons/controlsSimple/textArea';
import WordDownload from "../../components/wordDownload";


const AcceptanceTypeArr = [
    {
        value: 1, label: "Mua Sắm",
    },
    {
        value: 2, label: "Sửa chữa",
    },
]
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
 
        for (let location of acceptanceResultArr) {
            if (item.acceptanceResult)
                if (item.acceptanceResult.toString() == location.value.toString()) {
                    item.acceptanceResult = location;
                    break;
                }
        }


        for (let acceptanceType of AcceptanceTypeArr) {
            if (item.acceptanceType)
                if (item.acceptanceType.toString() == acceptanceType.value.toString()) {
                    item.acceptanceType = acceptanceType;
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

        var data = new FormData();
        item.acceptanceResult = item.acceptanceResult.value
        item.acceptanceType = item.acceptanceType.value
        Object.keys(item).forEach(key => {
            if (key !== 'items' && key !== 'employees' && key!== 'listDocument' ) {
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
        if (item.listDocument)
            for (let i = 0; i < item.listDocument.length; i++) {
                for (let key in item.listDocument[i]) {
                    data.append("listDocument[" + i + "]." + key, item.listDocument[i][key]);
                }
            }

        if (item.items) {
            for (let i = 0; i < item.items.length; i++) {
                for (let key in item.items[i]) {
                    data.append("items[" + i + "]." + key, item.items[i][key]);
                }
            }
        }
        Actions.setLoading(true);
        AcceptanceService.updateAcceptance(data)
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
                    <WordDownload feature="Acceptance" id={item.acceptanceID} name="Nghiem_Thu.doc" ></WordDownload>
                    <button style={{ marginRight: 10 }} type="button" className="btn btn-success"
                        onClick={() => {
                            Actions.openPrintDialog("Acceptance", item.acceptanceID);
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
                        key={"acceptanceCode"}
                        label={"Mã biên bản nghiệm thu"}
                        value={item.acceptanceCode}
                        onChange={(value) => {
                            item.acceptanceCode = value

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

                        label={"Kết quả nghiệm thu"}
                        options={acceptanceResultArr}
                        value={item.acceptanceResult}
                        onChange={(value) => {
                            item.acceptanceResult = value
                            if (value.value == 1) {
                                for (let record of item.items) {
                                    record.acceptanceResult = true;
                                }
                            } else {
                                if (value.value == 2) {
                                    for (let record of item.items) {
                                        record.acceptanceResult = false;
                                    }
                                }
                            }
                            this.setState({ item })
                        }} />

                        <SelectCustom
                        disabled={!this.state.isEdit}

                        label={"Loại Nghiệm thu"}
                        options={AcceptanceTypeArr}
                        value={item.acceptanceType}
                        onChange={(value) => {
                            item.acceptanceType = value                      
                            this.setState({ item })
                        }} />

                    
                </div>
                <TextArea
                       disabled={!this.state.isEdit}
                        type={'text'}
                        label={"Ý kiến của Khoa/ Phòng sử dụng: "}
                        value={item.acceptanceNote}
                        onChange={(value) => {
                            item.acceptanceNote = value
                            this.setState({ item })
                        }} />
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
                                    <ListItemAcceptance
                                        isNotEdited={!this.state.isEdit}
                                        items={item.items}
                                        onChange={(value) => {
                                            item.items = value;
                                            this.setState({ item })
                                        }}
                                    />
                                </div>
                            </div>
                        </div>
                    </React.Fragment>
                )
        }

    }

    render() {
        let {item} = this.state;
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
                                    feature={'Acceptance'}
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