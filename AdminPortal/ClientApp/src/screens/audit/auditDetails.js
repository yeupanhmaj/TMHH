
import "@kenshooui/react-multi-select/dist/style.css";
import moment from 'moment';
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import DatePickerCustom from '../../commons/controlsSimple/datePicker';

import InputCustom from '../../commons/controlsSimple/input';
import SelectCustom from '../../commons/controlsSimple/select';
import { AuditLocationArr, secretaryArr } from '../../commons/propertiesType';
import ListEmployeeAudit from '../../components/editCreateItemModal/addEditListItems/listEmployeeAudit';
import * as Actions from '../../libs/actions';
import * as AuditService from '../../services/auditService';
import { getListEmployee } from '../../services/auditService';
import './audit.css';
import ListItemQuote from './listItemQuote';
import WordDownload from '../../components/wordDownload'
import ListAttachfiles from '../../commons/controls/listAttachfiles';
import FileUploader from '../../components/fileUploader';

export default class auditDetails extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            activeTab: 0,
            files: [],
            listItem: [],
            item: {} ,
            isEdit: false 
        }
    
    }

   

    componentWillMount() {
        // let item = this.props.item ;     
        // item.preside = {
        //     label: 'Nguyễn Phương Liên',
        //     value: 13
        // }
        // if(item.location)
        // for (let location of AuditLocationArr) {
        //     if (item.location.toString() == location.value.toString()) {
        //         item.location = location;
        //     }
        // }
        // for (let secretary of secretaryArr) {
        //     if (item.secretary.toString() == secretary.value.toString()) {
        //         item.secretary = secretary;
        //     }
        // }

        // this.setState({item})
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        let item = nextProps.item ;     
        item.preside = {
            label: 'Nguyễn Phương Liên',
            value: 13
        }
        if(item.location)
        for (let location of AuditLocationArr) {
            if (item.location.toString() == location.value.toString()) {
                item.location = location;
            }
        }
        for (let secretary of secretaryArr) {
            if (item.secretary.toString() == secretary.value.toString()) {
                item.secretary = secretary;
            }
        }


         return {
            item : item
        };
      }


    updateNew() {
        let itemTemp = this.state.item ;

        let item = JSON.parse(JSON.stringify(itemTemp));
        item.customerID = +item.customerID;
   
        item.inTime = moment(item.inTime , 'DD-MM-YYYY').format('YYYY-MM-DD');

        item.startTime = moment(item.startTime , 'DD-MM-YYYY HH:mm').format('YYYY-MM-DD HH:mm');
        item.endTime = moment(item.endTime , 'DD-MM-YYYY HH:mm').format('YYYY-MM-DD HH:mm');
        if (item.preside) {
          item.preside = item.preside.value
        }
        if (item.secretary) {
          item.secretary = item.secretary.value
        }
        if (item.location) {
            item.location = item.location.value
          }
        var data = new FormData();
        
        let files = this._fileUploader.getFiles(); 
        if (files.length > 0) {
            for (let file of files) {
                data.append('files', file);
            }
        }
       

        Object.keys(item).forEach(key => {
          if (key !== 'employees' && key!=='listDocument') {
            data.append(key, item[key]);
          }
    
        });
        if (item.listDocument)
        for (let i = 0; i < item.listDocument.length; i++) {
          for (let key in item.listDocument[i]) {
            data.append("listDocument[" + i + "]." + key, item.listDocument[i][key]);
          }
        }
        if (item.employees)
          for (let i = 0; i < item.employees.length; i++) {
            for (let key in item.employees[i]) {
              data.append("employees[" + i + "]." + key, item.employees[i][key]);
            }
          }
    
          Actions.setLoading(true);
        
          AuditService.updateAuditwithAttFiles(item["auditID"], data)
            .then(objRespone => {
              if (objRespone.isSuccess === true) {
            
                this.props.onUpdateItem(item);
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
        let item = (this.state.item )
        return (

            <React.Fragment>

                <div style={{
                    position: 'absolute',
                    top: 10,
                    right: 20
                }}>
                <WordDownload feature="AuditWithItemPrice" id={item.auditID} name="Biên_bảng_hợp_kiểm_giá(DonGia).doc" buttonName="Down file (có đơn giá)" ></WordDownload>              
                <WordDownload feature="Audit" id={item.auditID} name="Biên_bảng_hợp_kiểm_giá.doc" ></WordDownload>
                     <button style={{ marginRight: 10 }} type="button" className="btn btn-success"
                                        onClick={() => {
                                            Actions.openPrintDialog("AuditWithItemPrice", item.auditID);
                                        }}>   <i style={{ marginRight: '5px' }} className="fa fa-print" aria-hidden="true"></i>In Biên bản (có đơn giá)</button>
                        <button style={{ marginRight: 10 }} type="button" className="btn btn-success"
                        onClick={() => {
                            Actions.openPrintDialog("Audit", item.auditID);
                        }}>   <i style={{ marginRight: '5px' }} className="fa fa-print" aria-hidden="true"></i>In Biên bản </button>
                        
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
                        key={"auditCode"}
                        label={"Mã biên bản"}
                        value={item.auditCode}
                        onChange={(value) => {
                            item.auditCode = value
                            this.setState({item})
                        }} />
                    {/* <SelectCustom
                        disabled={!this.state.isEdit}
                        key={"location"}
                        label={"Địa điểm"}
                        options={AuditLocationArr}
                        value={item.location}
                        onChange={(value) => {
                            item.location = value
                            this.setState({item})
                        }} /> */}
                    <DatePickerCustom
                        disabled={!this.state.isEdit}
                        label={"Ngày tạo biên bản"}
                        value={item.inTime}
                        onChange={(value) => {
                            item.inTime = value
                            this.setState({item})
                        }} />
                    <DatePickerCustom
                        disabled={!this.state.isEdit}
                        label={"Thời gian bắt đầu"}
                        value={item.startTime}
                        showTimeSelect={true}
                        onChange={(value) => {
                            item.startTime = value
                            this.setState({item})
                        }} />
                    <DatePickerCustom
                        disabled={!this.state.isEdit}
                        label={"Thời gian Kết thúc"}
                        value={item.endTime}
                        showTimeSelect={true}
                        onChange={(value) => {
                            item.endTime = value
               
                            this.setState({item})
                        }} />

                    <SelectCustom
                        disabled={!this.state.isEdit}

                        label={"Chủ tọa"}
                        options={[{
                            label: 'Nguyễn Phương Liên',
                            value: 13
                        
                        }]}
                        value={item.preside}
                        onChange={(value) => {
                            item.location = value
                            this.setState({item})
                        }} />

                    <SelectCustom
                        disabled={!this.state.isEdit}

                        label={"Thư ký"}
                        options={secretaryArr}
                        value={item.secretary}
                        onChange={(value) => {
                            item.secretary = value
            
                            this.setState({item})
                        }} />
                </div>
                <div style={{ margin: 20 }}>
                    <ListEmployeeAudit
                        disabled={!this.state.isEdit}
                        itemDefine={{
                            header: "Thành phần",
                            name: "employees",
                            type: "listEmployeeAudit",
                            getDataFunc: getListEmployee,
                            IsFull: true,
                        }}
                        items={item.employees}
                        onChange={(value) => {
                            item.employees = value

                            this.setState(item)
                        }}>
                    </ListEmployeeAudit>
                </div>
            </React.Fragment>
        )
    }

    renerQuoteDetails() {
        let item = this.props.item 

        if (item != null) {
            let quotesList = item.quotes ;
            if (quotesList)
                return (
                    <React.Fragment>
                        {quotesList.map((item, index) => {
                            return (
                                <div key={`${index}listquoteITEM`} style={{ flex:1, display: 'flex', border: '1px solid rgb(233,233,233)' }}>
                                    {/*  item */}
                                    <div style={{ flex:2,borderRight: '1px solid rgb (233,233,233)'}}>
                                        <div style={{marginRight: 10 , marginLeft : 10}}>
                                        <ListItemQuote
                                            VAT={item.isVAT}
                                            vatNumber={item.vatNumber}
                                            items={item.items}
                                            onChange={(value) => { }}
                                        />
                                            </div>              
                                    </div>
                                    {/* details quote*/}
                                    <div style={{  flex:1, padding: 10 }}>
                                            <div>
                                                Mã báo giá : {item.quoteCode}
                                            </div>
                                            <div>
                                                Công ty : {item.customerName}
                                            </div>
                                            <div>
                                                Đề xuất : {item.proposalCodes.toString()}
                                            </div>
                                            {item.isVAT == true &&
                                               <div>
                                                  VAT : {item.vatNumber} 
                                               </div>  
                                            }
                                    </div>
                                </div>
                            )
                        })
                        }
                    </React.Fragment>
                )
        }

    }

    render() {
        let {item} = this.state
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
                                
                                let curItem = this.state.item ;
                                curItem['listDocument'] = value;
                               
                                this.setState({ item : curItem})
                                }}
                                feature={'Audit'}
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