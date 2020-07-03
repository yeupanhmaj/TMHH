
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader, Table } from 'reactstrap';
import ListItems from '../../components/itemDetail/listItems';
import * as Actions from '../../libs/actions';
import CheckBoxCustom from '../../commons/controlsSimple/checkbox';
import InputCustom from '../../commons/controlsSimple/input';
import ListItemQuote from './listItemQuote';
import "@kenshooui/react-multi-select/dist/style.css";
import './quote.css'

import * as QuoteService from '../../services/quoteService';



export default class QouteDetails extends React.Component {
  
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

     

    componentDidMount() {
        this.loadDataItems();
        this.setState({ item: this.props.item });
    }

    loadDataItems() {
        if (this.props.item == undefined) return;
        let listCustomer = (this.props.item )['quotes'];
       
       
        let currentItem = listCustomer[this.state.activeTab];

        QuoteService.getItems(this.props.item['quoteID'], currentItem['customerID'])
            .then((objRespone) => {
                if (objRespone.isSuccess === true) {
                    this.setState({ listItem: objRespone.data })
                }
            })

    }
    handleChangeCustomer(index) {
        this.setState({ activeTab: index }, () => {
            this.loadDataItems();
        })
    }
    fileChange(event) {

        let file = event.target.files[0];
        if (this.props.item == undefined) return;
        let listCustomer = (this.props.item )['quotes'];
        let currentItem = listCustomer[this.state.activeTab];
        let formData = new FormData();
        formData.append('file', file)
        QuoteService.importData(formData, this.props.item['quoteID'], currentItem['customerID'])
            .then((objRespone) => {
                if (objRespone.isSuccess === true) {
                    this.loadDataItems();
                    this._inputfile.value = ''
                }
            }).catch((ex) => {
                this._inputfile.value = ''
            })
       
    }

    handelActiveQuote(){
        if (this.state.item == undefined) return;
        let item = (this.state.item )
        let listCustomer = item['quotes'];
        if(listCustomer==undefined ||  listCustomer.length == 1){
            return;
        }
        let currentItem = listCustomer[this.state.activeTab];
        for(let tempItem of listCustomer){
            tempItem['isChoosed'] = false;
        }
        currentItem['isChoosed'] = true;
        //todo call API
        QuoteService.selectQuote(item['quoteID'], currentItem['customerID'])
            .then((objRespone) => {
                if (objRespone.isSuccess === true) {
                    this.loadDataItems();
                    this._inputfile.value = ''
                }
            }).catch((ex) => {
                this._inputfile.value = ''
            })
        this.setState({item:item})
    }

    renderTabControl() {
        if (this.state.item == undefined) return;
        let item = (this.state.item )
        let listCustomer = item['quotes'];
        let isVAT = item["isVAT"]
        let vatNumber = item["vatNumber"]
        if(listCustomer==undefined ) return
        return (
            <React.Fragment>
                <div>
                    <div style={{ display: 'flex' }}>
                        {listCustomer.map((item, index) => {
                            return (
                                <div key={`${index + item.customerName}`}
                                    onClick={() => { this.handleChangeCustomer(index) }}
                                    style={{
                                        borderRadius: 30, margin: 20 , lineHeight: '29px'
                                    }}
                                    className={`${this.state.activeTab == index ? 'tabactive' : 'tabnormal'}`}>
                                    {item.customerName}
                                </div>
                            )
                        })}
                    </div>
                    <div>
                        <div key={`${this.state.activeTab + listCustomer[this.state.activeTab].isChoosed + 'zz'}`}>
                            {listCustomer[this.state.activeTab].isChoosed == true?
                              <div className={"btn btn-info"} style={{cursor:'pointer' , marginLeft : 30}} onClick={()=>{this.handelActiveQuote()}}>
                                  Đã chọn
                              </div>
                            :
                            <div className={"btn"} style={{cursor:'pointer' , marginLeft : 30 , border:'1px solid rgb(235,235,235)'}} onClick={()=>{this.handelActiveQuote()}}>
                                  Chọn
                                </div>

                            }    
                        </div>    
                    </div>

                    <div style={{ padding: 5 }}>
                        <ListItemQuote
                            VAT={isVAT}
                            vatNumber={vatNumber}
                            items={this.state.listItem}
                            onChange={(value) => { }}
                        />
                    </div>


                    <div>

                        {/*  */}
                        <div className="leftSideFlieUpload">
                            <input style={{ display: 'none' }}
                                ref={(c) => this._inputfile = c}
                                type="file" name="file" accept=".*" className="upload-file" onChange={this.fileChange.bind(this)} />
                            <div className={"btn-search btn btn-secondary"} onClick={() => { this._inputfile.click() }} >
                                <i className="fa fa-upload" style={{ fontSize: '20px', marginRight: 10 }}></i>
                                Upload Files
                            </div>
                        </div>
                        {/*  */}

                    </div>
                </div>
            </React.Fragment>
        )
    }

    quoteDetails() {
        
        if (this.state.item == undefined) return;
        let item = (this.state.item )

        return (
            <div>
                <div style={{ display: 'flex' }}>
                    <div style={{ margin: 10 }}>
                        <InputCustom
                            disabled={!this.state.isEdit}
                            type={'text'}
                            key={"input ma bao gia"}
                            label={"Mã báo giá"}
                            value={item.quoteCode}
                            onChange={(value) => {
                                item.quoteCode = value
                                this.setState({item})
                            }} />
                    </div>
                    <div style={{ margin: "40px 10px 10px 10px" }}>

                        <CheckBoxCustom
                            disabled={!this.state.isEdit}
                            key={"checkbox ishasquote"}

                            value={item.isVAT}
                            lable={"VAT"}
                            onChange={(value) => {
                                item.isVAT = !item.isVAT;
                                this.setState({item})
                            }} />
                    </div>
                    <div style={{ margin: 10 }}>
                        <InputCustom
                            disabled={!this.state.isEdit}
                            type={'number'}
                            key={"input gia tri vat"}
                            label={"Giá trị VAT"}
                            value={item.vatNumber}
                            onChange={(value) => {
                                item.vatNumber = value;
                                this.setState({item})
                            }} />

                    </div>
                   
                </div>
            </div>
        )
    }

    renderBody() {
        if (this.props.item == undefined) return;
        let lstProposal = (this.props.item )['lstProposal'];
        if (lstProposal.length < 1) return;
        return (
            <div>
                {lstProposal.map((item, index) => {
                    return <div key={item.ProposalCode + index} style={{
                        display: 'flex', padding: '0px 10px 15px 10px ',
                        border: '1px solid rgb(223,223,223)', margin: 7
                    }} >
                        <div style={{ flex: 2 }}>
                            <div style={{ marginTop: 20 }}>
                                Mã đế xuất : {item.proposalCode}
                            </div>
                            <div>
                                Phòng bang : {item.departmentName}
                            </div>
                            <div>
                                Thời gian : {item.inTtime}
                            </div>
                        </div>

                        <div style={{ flex: 6 }}>
                            <ListItems items={item.items}></ListItems>
                        </div>


                    </div>
                })}
            </div>
        )


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
                                {this.quoteDetails()}
                                {this.renderBody()}
                                {this.renderTabControl()}                            </div>


                        </ModalBody>
                        <div style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            fontSize: '12px',
                            marginBottom: '30px',
                            marginTop: '20px'
                        }}>
                        
                        <div style={{ marginRight: "10px" , width: '100px' }}>
                            {this.state.isEdit == false ?
                                <Button style={{width:100}} className={"btn btn-danger"} onClick={()=>{
                                    this.setState({isEdit: true})
                                }} > Sửa</Button>
                                :
                                <Button style={{width:100}} className={"btn btn-info"}
                                onClick={()=>{
                                    Actions.setLoading(true);
                                    QuoteService.updateQuoteNew(item.quoteID, {
                                        quoteCode: item.quoteCode ,
                                        isVAT:  item.isVAT ,
                                        vatNumber: +item.vatNumber 
                                    }).then( (respone ) =>{
                                        this.props.onUpdateItem(item);
                                        this.setState({isEdit: false})
                                        Actions.setLoading(false);
                                    })
                                
                                }}
                                > Lưu</Button>
                            }
                        </div>
                        <div>
                            <Button className="btn-default" style={{ width: '100px', marginLeft: 30 }} onClick={() => { this.props.onCancel() }}>Tắt</Button>
                      
                            </div>
                              </div>
                    </Modal>
                }
            </div>
        );
    }
}