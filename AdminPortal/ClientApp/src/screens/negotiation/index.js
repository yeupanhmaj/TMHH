import moment from 'moment';
import * as React from 'react';
import DatePicker from "react-datepicker";
import Select from 'react-select';
import { Button } from 'reactstrap';
import { CONST_FEATURE } from '../../commons';
import ConfirmDialog from '../../components/confirmDialog';
import EditCreateItemModal from '../../components/editCreateItemModal';
import TableCustom from '../../components/tableCustom';
import * as Actions from '../../libs/actions';
import { NegotiationColums, NegotiationDefine } from '../../models/negotiation';
import * as NegotiationService from '../../services/negotiationService';
import * as QuoteService from '../../services/quoteService';
import NegotiationDetails from './negotiationDetails';

import SimpleReactValidator from 'simple-react-validator';

export default class Negotiation extends React.Component {
  _toDateCalendar;
  _fromDateCalendar;
  _tableCustom;

  constructor(props) {
    super(props);
    this.state = {
      searchState: {
        fromDate: new Date(`01-01-${new Date().getFullYear()}`),
        toDate: new Date(),
        quoteCode: '',
        customerID: '',
        negotiationCode: '',
      } ,
      lastSearchState: null,
      listContractCodeMaster: [],
      lstData: [],
      totalRecords: 0,
      pageSize: 10,
      itemModal: false,
      confirmModal: false,
      selectedItem: undefined,
      confirmContent: '',
      deleteIds: [],
      currentPage: 0,
      headerEditModal: '',
      localItem: {},
      itemDefines: NegotiationDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      negotiationTypeOptions: [],
      statusOptions: [],
      lstDepartments: [],
      lstCustomers: [],
      viewItem : {} ,
    };
    this.validator = new SimpleReactValidator({
      messages: {
        required: 'Vui lòng không nhập kí tự đặc biệt'
      }
    });
  }

 

  componentWillMount() {
    let itemDefines = this.state.itemDefines;
    itemDefines.props[2].cbFunc = this.onLoadQuote.bind(this);
    this.setState(itemDefines)
  }


  onLoadQuote(record, item) {
   if(record && record.value)
    QuoteService.getQuoteInfo(record.value).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        let selectedItem = item 
        selectedItem.customerID = objRespone.item.customerID;
        selectedItem.customerName = objRespone.item.customerName;
        selectedItem.isVAT = objRespone.item.isVAT;
        selectedItem.vatNumber = objRespone.item.vatNumber;   
        selectedItem.bidType   = objRespone.item.bidType;  
        selectedItem.bidExpirated   = objRespone.item.bidExpirated;  
        selectedItem.bidExpiratedUnit   = objRespone.item.bidExpiratedUnit;  
        selectedItem.items   = objRespone.item.items;  

        this.setState({
          selectedItem
        })
      } else {
        Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
      }
    }).catch(err => {

    })
  }

  getmasterData() {
    NegotiationService.getMasterData().then(
      result => {
        let lstCustomers = result[0]
        let lstDepartments = result[1]
        let itemDefines = this.state.itemDefines;
        if (itemDefines && itemDefines.props && itemDefines.props.length > 0) {
          if (lstCustomers.isSuccess) {
            this.state.lstCustomers = [];
            for (let record of lstCustomers.data) {
              let item = { label: record.customerName, value: record.customerID };
              (this.state.lstCustomers).push(item);
            }
          }
          if (lstDepartments.isSuccess) {
            this.state.lstDepartments = [];
            for (let record of lstDepartments.data) {
              let item = { label: record.departmentName, value: record.departmentID };
              (this.state.lstDepartments).push(item);
            }
          }
        }
        this.setState({ itemDefines });
      }
    )
  }
  componentDidMount() {
    this.getmasterData();
   
    this.handelNewSearch();
  }

 

  deleteRecords(ids) {
    NegotiationService.deleteRecords(ids)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          this.setState({
            confirmModal: false
          })
          // if have conditions please keep
          this.reSearch();
        } else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      }).catch(err => {

      })
  }

  deleteRecord(id) {
    NegotiationService.deleteRecord(id)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          this.setState({
            confirmModal: false
          })
          // if have conditions please keep
          this.reSearch();
        } else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      }).catch(err => {

      })
  }
  //handle event from custom table
  handleViewActionFromDataTable(id) {
    Actions.setLoading(true);
    NegotiationService.GetNegotiationById(id).then(
      result => {
        if (result.isSuccess === true) {
          result.item.inTime = moment(new Date(result.item.inTime)).format('DD-MM-YYYY')
          result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY HH:mm')
          this.setState({ detailsItemModal: true, viewItem: result.item })
          Actions.setLoading(false);
        } else {
          Actions.openMessageDialog("lay data loi", result.err.msgString);
        }
      }).catch(ex => {
        Actions.openMessageDialog("lay data loi", ex.toString());
      })   
  }


  onModalSubmmitWithAttachFile(item, files) {
    item = item ;
    item.inTime = moment(new Date()).format('YYYY-MM-DD HH:mm');
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD HH:mm');
    var data = new FormData();
    if (files.length > 0) {
      for (let file of files) {
          data.append('files', file);
      }
  }

    Object.keys(item).forEach(key => {
      if(key!="quoteCode"){
      data.append(key, item[key]);
      }else{
        data.append("quoteID", item["quoteCode"].value);
      }
    });

    // item.status = +item.status;
    if (item["negotiationID"] === undefined) {
      Actions.setLoading(true);
      NegotiationService.createNegotiationwithAttFiles(data)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({
              itemModal: false , selectedItem:{}
            })
            // if have conditions please keep
            this.reSearch();
            Actions.setLoading(false);
          } else {
            Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
          }
        }).catch(err => {

        })
    } else {
      NegotiationService.updateNegotiationwithAttFiles(item["negotiationID"], data)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({
              itemModal: false
            })
            // if have conditions please keep
            this.reSearch();
          } else {
            Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
          }
        }).catch(err => {

        })
    }
  }
  
  onModalSubmitAction() {

  }

    handelEditActionFromDataTable(item, type ) {
    if (type === "delete") {
      let confirmContent = `Bạn có chắc muốn xóa thương thảo hợp đồng ?`
      this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteId: +(item ) })
    } else {
      if (type === "deleteRecords") {
        let negotiationCodes = [];
        let negotiations =   item.map((id) => {
          return this.state.lstData.find((obj) =>
            obj.negotiationID === id
          )
        })
        if (negotiations) {
          for (let record of (negotiations )) {
            negotiationCodes.push('  ' + (record ).negotiationCode);
          }
        }
        let confirmContent = `Bạn có chắc muốn xóa các thương thảo hợp đồng : ${negotiationCodes.toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item  })
      } else {
        let headerEditModal = "Chỉnh sửa thương thảo hợp đồng";
        if (type === "insert") {
          headerEditModal = "thêm thương thảo hợp đồng  mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
        } else {
          
        }

      }
    }
  }

  onModalConFirmAction() {
    if (this.state.confirmType === "deleteRecords") {
      this.deleteRecords(this.state.deleteIds)
    } else {
      this.deleteRecord(this.state.deleteId)
    }
    this.handelNewSearch();

  }


  //event change input 
  handleSearchNegotiationCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState )[name] = value
    this.setState({ searchState })
  }

  handleSearchQuoteCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState )[name] = value
    this.setState({ searchState })
  }
  handelNewSearch() {
    this._tableCustom.resetPageIndex();
    this.setState({ currentPage: 0, lastSearchState: this.state.searchState }, () => {
      this.searchWithCondition(this.state.searchState);
    });
  }
  handleSearchChangeDateChange(name, date) {
    let searchState = this.state.searchState;
    (searchState )[name] = date;
    this.setState({ searchState });
  }

  handelChangePage(pageNum, pageSize) {
    // this.getData(pageNum - 1, pageSize);
    this.setState({ currentPage: pageNum - 1, pageSize }, () => {
      this.reSearch();
    })
  }


  handleChangeCustomerID(value) {
    let {searchState} = this.state 
    searchState['customerID'] = value;
    this.setState(
      { searchState }
    );
  };


  //search
  reSearch() {
    if (this.state.lastSearchState === null) {
      this.searchWithCondition(this.state.searchState);
    } else {
      this.searchWithCondition(this.state.lastSearchState)
    }
  }
  
  searchWithCondition(searchState) {
    NegotiationService.GetAllNegotiationWithCondition(
      searchState.fromDate,
      searchState.toDate,
      searchState.negotiationCode,
      searchState.quoteCode,
      searchState.customerID,
      this.state.pageSize,
      this.state.currentPage)
      .then(objRespone => {

        if (objRespone.isSuccess === true) {
          for (let item of objRespone.data) {
            item.dateIn = moment(new Date(item.dateIn)).format('DD-MM-YYYY');
          }
          this.setState({

            lstData: objRespone.data,
            totalRecords: objRespone.totalRecords
            // currentPage:pageNumber
          })
        } else {
          Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
        }
      }).catch(err => {

      })
  }


  //search
  render() {
    let lstCustomers = [];
    lstCustomers = [{ label: "Tất cả", value: '' }];
    let lstDepartments = [];
    lstDepartments = [{ label: "Tất cả", value: '' }];
    lstCustomers = lstCustomers.concat(this.state.lstCustomers);
    lstDepartments = lstDepartments.concat(this.state.lstDepartments);


    let ItemDeatilsHeader = '';
    let viewItem = this.state.viewItem ;
    if(viewItem != undefined){
      ItemDeatilsHeader = "Bản Thương thảo hợp đồng mã : "  + viewItem.negotiationCode;
    }

    return (
      <React.Fragment>
        {/* search */}
        <div className="searchContainer" >
          <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px' }}>
          
        
            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label>Mã thương thảo</label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'negotiationCode'} value={this.state.searchState.negotiationCode} onChange={
                  this.handleSearchNegotiationCodeChangeInput.bind(this)
                } />
                     {this.validator.message('Mã thương thảo', this.state.searchState.negotiationCode, 'alpha_num_dash_space')}
            </div>

            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label>Mã báo giá</label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'quoteCode'} value={this.state.searchState.quoteCode} onChange={
                  this.handleSearchQuoteCodeChangeInput.bind(this)
                } />
                 {this.validator.message('Mã báo giá', this.state.searchState.quoteCode, 'alpha_num_dash_space')}
            </div>
         
            <div className="childSearchWrap">
              <div style={{ lineHeight: '5px' }}>
                <label>Công ty</label>
              </div>
              {lstCustomers !== undefined &&
                <Select
                  placeholder="Công ty"
                  styles={customStyles}
                  value={this.state.searchState.customerID}
                  onChange={this.handleChangeCustomerID.bind(this)}
                  options={lstCustomers}
                />
              }
            </div>

            <div
              className="childSearchWrap"
            >
              <div style={{ lineHeight: '5px' }}>
                <label>Từ ngày</label>
              </div>
              <div style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400, width: '120px',
                height: '32px',
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none',
                display: 'flex',
                flexDirection: 'row',
                cursor: 'pointer'
              }}>
                <DatePicker
                  className={"datePickerCustom"}
                  dateFormat="dd-MM-yyyy"
                  showMonthDropdown
                  showYearDropdown
                  todayButton="Hôm nay"
                  locale="vi"
                  onChange={this.handleSearchChangeDateChange.bind(this, 'fromDate')}
                  selected={this.state.searchState.fromDate }
                  ref={(c) => this._fromDateCalendar = c}
                />
                <div style={{ cursor: 'pointer', lineHeight: '34px' }} onClick={() => {
                  this._fromDateCalendar.setOpen(true)
                }}>
                  <i className="fa fa-calendar" ></i>
                </div>
              </div>
            </div>
            <div className="childSearchWrap">
              <div style={{ lineHeight: '5px' }}>
                <label>Đến ngày</label>
              </div>
              <div style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400, width: '120px',
                height: '32px',
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none',
                display: 'flex',
                flexDirection: 'row'
              }}>
                <DatePicker
                  className={"datePickerCustom"}
                  dateFormat="dd-MM-yyyy"
                  showMonthDropdown
                  showYearDropdown
                  todayButton="Hôm nay"
                  locale="vi"
                  onChange={this.handleSearchChangeDateChange.bind(this, 'toDate')}
                  selected={this.state.searchState.toDate }
                  ref={(c) => this._toDateCalendar = c}
                />
                <div style={{ cursor: 'pointer', lineHeight: '34px' }} onClick={() => {
                  this._toDateCalendar.setOpen(true)
                }}>
                  <i className="fa fa-calendar" ></i>
                </div>
              </div>
            </div>
            <div className="childSearchWrap">
              <Button className="btn-search" style={{
                width: '110px',
                marginTop: '10px',
                height: '35px'
              }} onClick={() => {
                if (this.validator.allValid()) {
                  this.handelNewSearch()
                } else {
                  this.validator.showMessages();         
                  this.forceUpdate();
                }
              }}> Tìm kiếm
              </Button>
            </div>
          </div>
        </div>
        {/* search */}
        <TableCustom
          ref={(c) => { this._tableCustom = c }}
          keyColumn={CONST_FEATURE.Negotiation.KEY_COLUMN}
          columns={NegotiationColums}
          dataSource={this.state.lstData}
          totalRecords={this.state.totalRecords ? this.state.totalRecords : 0}
          onChangePage={this.handelChangePage.bind(this)}
          isModifyAll={true}
          onEditAction={this.handelEditActionFromDataTable.bind(this)}
          onViewAction={this.handleViewActionFromDataTable.bind(this)}
        />
        <EditCreateItemModal
          item={this.state.selectedItem}
          Modal={this.state.itemModal}
          itemDefines={this.state.itemDefines}
          // onSubmmit={this.onModalSubmitAction.bind(this)}
          onSubmmitWithAttachFile={this.onModalSubmmitWithAttachFile.bind(this)}
          onCancel={() => { this.setState({ itemModal: false }) }}
          headerName={this.state.headerEditModal}
          keyColumn={CONST_FEATURE.Negotiation.KEY_COLUMN}
          isHasUploader={true}
          referFeatureType={CONST_FEATURE.Negotiation.feature}
        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>

        {this.state.detailsItemModal == true && 
        <NegotiationDetails  
          header={ ItemDeatilsHeader }
          item={this.state.viewItem}
          Modal={this.state.detailsItemModal}
          onCancel={() => { this.setState({ detailsItemModal: false })   }}
          onSubmitSuccess={()=>{
             this.setState({ detailsItemModal: false });  this.handelNewSearch(); }}
          onUpdateItem={(data)=>{
           this.handleViewActionFromDataTable(data.negotiationID) }}         
        ></NegotiationDetails>
        }
      </React.Fragment>
    );
  }
};
const customStyles = {
  placeholder: () => ({
    margin: 0,
    color: '#aaa'
  }),
  indicatorSeparator: () => ({
    color: '#fff'
  }),

  option: (provided, state) => ({
    ...provided,
    fontSize: 14,
    lineHeight: '14px',
    fontFamily: 'roboto',
    marginTop: 4
  }),
  control: () => ({
    display: 'flex',
    width: 175,
    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
    height: 32,
    bordeRadius: 3,
    paddingLeft: 5,
    fontSize: 14,
    lineHeight: '14px',
    fontFamily: 'roboto',
  }),
  singleValue: (provided, state) => {
    const opacity = state.isDisabled ? 0.5 : 1;
    const transition = 'opacity 300ms';

    return { ...provided, opacity, transition };
  }
}