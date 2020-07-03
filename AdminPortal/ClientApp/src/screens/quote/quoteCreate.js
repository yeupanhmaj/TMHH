
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader, Table } from 'reactstrap';
import InputCustom from '../../commons/controls/input';
import CheckBoxCustom from '../../commons/controlsSimple/checkbox';
import FileUploader from '../../components/fileUploader';
import * as CommonService from '../../services/commonService';
import * as ProposalService from '../../services/proposalService';
import * as QuoteService from '../../services/quoteService';
import * as Actions from '../../libs/actions';
import MultiSelect from "@kenshooui/react-multi-select";
import "@kenshooui/react-multi-select/dist/style.css";




export default class QuoteCreateModal extends React.Component {
   
    constructor(props) {
        super(props);
        this.state = {
            //search
            searchPropolsaltxt: '',
            searchProducttxt: '',
    
    
            proposalList: [],
            selectedProposalList: [],
    
            customerID: '' ,
            quoteType: '' ,
    
            customerList: [],
            selectedcustomerList: [],
            isHasQuote : false
        }
    }

   

    componentWillMount() {
        
        CommonService.GetAllCustomer().then(
            result => {
                if (result.isSuccess) {
                    let { customerList } = this.state;
                    for (let record of result.data) {
                        let item = { label: record.customerName, id: record.customerID };
                        customerList.push(item);
                    }
                    this.setState({ customerList });
                }
            }
        )
      this.handelNewSearch() ;
    }

    renderBody() {

        return (
            <div style={{}}>
                <div style={{ width: '100%' }}>
                {this.renderProposalSide()}
                </div>
                <div style={{ width: '100%' }}>
                {this.renderSupplierSide()}
                
                </div>

            </div>
        )
    }


    changeSelectProposalSearch(value) {
        this.setState({ searchPropolsaltxt: value })
    }
    changeProductName(value) {
        this.setState({ searchProducttxt: value })
    }

    handelNewSearch() {
        ProposalService.GetItemWithCondition(
            this.state.searchPropolsaltxt,
            this.state.searchProducttxt,
            this.state.isHasQuote
        )
            .then(objRespone => {
                if (objRespone.isSuccess ===true) {

                    let proposalList = [];
                    if (objRespone.data && objRespone.data.length > 0) {
                       var groupes = {};
                        // groupes = objRespone.data.reduce((h, obj) => Object.assign(h, { [obj.proposalID]: h[obj.proposalID].concat(obj) }), {})
                        for(let item of objRespone.data){
                            if( groupes[item.proposalID] == undefined){
                                groupes[item.proposalID] = []
                            }
                                groupes[item.proposalID].push(item)
                        }
                        for (let group in groupes) {
                            let temp = {} ;
                            temp.id = groupes[group][0].proposalID;
                            temp.label = groupes[group][0].proposalCode + ' : ';
                            for (let item of groupes[group]) {
                                
                                temp.label += item.itemAmount + ' ' + item.itemUnit + ' ' + item.itemName + ', ';
                            }
                            temp.label = temp.label.substring(0, temp.label.length - 1);
                            proposalList.push(temp)
                        }

                        this.setState({
                            proposalList: proposalList,

                        })
                    }else{
                        this.setState({
                            proposalList: []

                        })
                    }

                } else {
                    Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
                }
            }).catch(err => {

            })

    }


    renderProposalSide() {
        return (
            <React.Fragment>
                <div style={{
                    width: '100%',
                    overflow: 'auto', padding: 10, marginTop: 20, marginBottom: 20, border: '1px solid #ccc', borderRadius: 30
                }}>

                    <div style={{ display: 'flex' }}>
                        <div style={{

                            marginLeft: '32px',

                        }}>
                            <InputCustom
                                item={{
                                    header: "Đề xuất",
                                    name: "proposal",
                                    type: "input",
                                    width: 200,
                                    allowNull: true
                                }}
                                value={this.state.searchPropolsaltxt}
                                onChange={(value) => {
                                    this.changeSelectProposalSearch(value);
                                }} />
                        </div>
                        <div style={{
                            marginLeft: '32px',
                        }}>
                            <InputCustom
                                item={{
                                    header: "Sản phẩm",
                                    name: "customer",
                                    type: "input",
                                    width: 200,
                                    allowNull: true
                                }}
                                value={this.state.searchProducttxt}
                                onChange={(value) => {
                                    this.changeProductName(value);
                                }} />
                 
                        </div>

                        <CheckBoxCustom
                            style={{marginTop:35,marginLeft:35 , display:'none'}}
                            key={"checkbox ishasquote"  }

                            value={this.state.isHasQuote}
          
                            lable={"Đã có báo giá"}
                            onChange={(value) => {
                                this.setState({isHasQuote: value});
                            }}/>
                        <Button className="btn-search" style={{
                            width: '110px',
                            marginTop: '32px',
                            marginLeft: '32px',
                            height: '35px'
                        }} onClick={() => {
                            this.handelNewSearch()
                        }}> Tìm kiếm
                        </Button>
                    </div>


                    <div style={{ marginTop: '15px' }}>
                        <MultiSelect
                            height={350}
                            responsiveHeight={350}
                            items={this.state.proposalList}
                            selectedItems={this.state.selectedProposalList}
                            onChange={this.handleChooseProposal.bind(this)}
                        // withGrouping={true}
                        />
                    </div>


                </div>
            </React.Fragment>
        )
    }


    handleChooseCompany(selectedItems) {
        this.setState({ selectedcustomerList: selectedItems });
    }
    handleChooseProposal(selectedItems) {

        this.setState({ selectedProposalList: selectedItems });
    }

    onSubmmit() {
        let data = {} ;
        data.customerList = [];
        data.proposalList = [];
        for (let item of this.state.selectedcustomerList) {
            data.customerList.push(item.id);
        }

        for (let item of this.state.selectedProposalList) {
            data.proposalList.push(item.id);
        }
        QuoteService.getExits(data).then((respone) => {
            if (respone.isSuccess) {
                if(respone.data.length == 0){
                    Actions.setLoading(true);
                    QuoteService.insetQuote(data).then((respone) => {
                        if (respone.isSuccess) {
                            this.props.onSubmitSuccess();
                            Actions.setLoading(false);
                        }else{
                            Actions.openMessageDialog("tạo báo giá", respone.err.msgString.toString());
                        }
                    })
                }else{
                    //todo
                    Actions.openMessageDialog("tạo báo giá", "Báo giá đã tồn tại ");
                }
            }
        })
    }

    renderSupplierSide() {
        return (
            <React.Fragment>
                <div style={{
                    width: '100%', overflow: 'auto',
                    marginTop: 20, marginBottom: 20, border: '1px solid #ccc', borderRadius: 30, padding: 20
                }}>

                    <MultiSelect
                        maxSelectedItems={3}
                        height={250}
                        responsiveHeight={250}
                        items={this.state.customerList}
                        selectedItems={this.state.selectedcustomerList}
                        onChange={this.handleChooseCompany.bind(this)}
                    />


                </div>
            </React.Fragment>
        )
    }



    render() {
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
                                Tạo Báo Giá
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
                                {this.renderBody()}
                            </div>

                            {/* <div style={{ marginBottom: 20, marginTop: 15 }}>
                                <FileUploader ref={(c) => { this._fileUploader = c }} />
                            </div> */}

                        </ModalBody>
                        <div style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            fontSize: '12px',
                            marginBottom: '30px',
                            marginTop: '20px'
                        }}>
                            <Button className="btn-danger" style={{ width: '100px', marginLeft: '-30' }} onClick={() => {
                                { this.onSubmmit() }
                            }}>Lưu</Button>
                            <Button className="btn-default" style={{ width: '100px', marginLeft: 30 }} onClick={() => { this.props.onCancel() }}>Tắt</Button>
                        </div>
                    </Modal>
                }
            </div>
        );
    }
}