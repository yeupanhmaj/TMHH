import vi from "date-fns/locale/vi";
import moment from 'moment';
import * as React from 'react';
import DatePicker, { registerLocale } from "react-datepicker";
import Select from 'react-select';
import { Button } from 'reactstrap';
import { CONST_FEATURE } from '../../commons';
import { acceptanceResultArr, getLabelString, IsearchState } from '../../commons/propertiesType';
import ConfirmDialog from '../../components/confirmDialog';
import EditCreateItemModal from '../../components/editCreateItemModal';
import TableCustom from '../../components/tableCustom';
import * as Actions from '../../libs/actions';
import { AcceptanceColums, AcceptanceDefine, AcceptanceModel } from '../../models/acceptance';
import * as AcceptanceService from '../../services/acceptanceService';
import * as ProposalService from '../../services/proposalService';
import AcceptanceDetails from './acceptanceDetails';

import SimpleReactValidator from 'simple-react-validator';


export default class Acceptance extends React.Component {
  _toDateCalendar;
  _fromDateCalendar;
  _tableCustom;

  constructor(props) {
    super(props);
    registerLocale("vi", vi);
    this.state = {
      searchState: {
        proposalCode: '',
        department: '',
        fromDate: new Date(`01-01-${(new Date()).getFullYear()}`),
        toDate: new Date()
      }, 
      lastsearchState: null,
      listContractCodeMaster: [],
      lstData: [],
      totalRecords: 0,
      pageSize: 10,
      itemModal: false,
      confirmModal: false,
      // selectedItem: undefined,
      confirmContent: '',
      deleteIds: [],
      currentPage: 0,
      headerEditModal: '',
      selectedItem: {},
      itemDefines: AcceptanceDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      explanationTypeOptions: [],
      statusOptions: [],
      viewItem : {} 
    };

    this.validator = new SimpleReactValidator({
      messages: {
        required: 'Vui lòng không nhập kí tự đặc biệt'
      }
    });
  }



  componentWillMount() {
    let itemDefines = this.state.itemDefines;
    itemDefines.props[2].cbFunc = this.onLoadItemPropsal.bind(this);
    itemDefines.props[4].cbFunc = this.ChangeResult.bind(this);
    this.setState(itemDefines)
  }

  ChangeResult(value){
    let {selectedItem} = this.state
      if (value == 1) {
        for (let record of selectedItem.items) {
            record.acceptanceResult = true;

        }
    } else {
        if (value == 2) {
            for (let record of selectedItem.items) {
                record.acceptanceResult = false;

            }
        }
    }

    this.setState({selectedItem})
  }

  componentDidMount() {

    this.handelNewSearch();
  }

 
  deleteRecords(ids) {
    AcceptanceService.deleteRecords(ids)
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
    AcceptanceService.deleteRecord(id)
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
    AcceptanceService.GetAcceptanceById(id).then(
      result => {
        if (result.isSuccess == true) {     
          result.item.inTime = moment(new Date(result.item.inTime)).format('DD-MM-YYYY')   
          result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')   
          this.setState({ detailsItemModal: true, viewItem: result.item })
        } else {        
          Actions.openMessageDialog("lay data loi", result.err.msgString);       
        }
        Actions.setLoading(false);
      }).catch(ex => {
        Actions.openMessageDialog("lay data loi", ex.toString());
      })
  }
  
  
  onModalSubmmitWithAttachFile(item, files) {
    item = item

    var data = new FormData();
     if (files.length > 0) {
            for (let file of files) {
                data.append('files', file);
            }
        }

    Object.keys(item).forEach(key => {
      if (key !== 'items'){
        if(key == "proposalCode"){
          data.append("proposalCode", item[key].label);
          data.append("proposalID", item[key].value);
          data.append("proposalType", item[key].value);
        }else{
         
          data.append(key, item[key]);
        }
    
      }
    });

    for (let i = 0; i < item.items.length; i++) {
      for (let key in item.items[i]) {       
          data.append("items[" + i + "]." + key, item.items[i][key]);            
      }
    }

    if (item["acceptanceID"] === undefined) {
    //  Actions.load
    Actions.setLoading(true);
      AcceptanceService.createAcceptance(data)
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
      AcceptanceService.updateAcceptance(data)
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

    handelEditActionFromDataTable(item, type ) {
    if (type === "delete") {
      let confirmContent = `Bạn có chắc muốn xóa với Id : ${(item ).toString()}  ?`
      this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteId: +(item ) })
    } else {
      if (type === "deleteRecords") {
        let confirmContent = `Bạn có chắc muốn xóa với Ids : ${(item).toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item  })
      } else {
        let headerEditModal = "Chỉnh sửa dữ liệu";
        if (type === "insert") {
          headerEditModal = "thêm dữ liệu mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
        } 
      }
    }
  }

  onLoadItemPropsal(code, item) {
    if(code != undefined && item != undefined){
      ProposalService.getDetailsAcceptanceByProposalID(code.value).then((objRespone) => {
        if (objRespone.isSuccess === true) {
          let selectedItem = item
        //  selectedItem.proposalID = objRespone.item.proposalID;
        //  selectedItem.proposalCode = objRespone.item.proposalCode;
          selectedItem.departmentName = objRespone.item.departmentName;
          selectedItem.items = objRespone.item.deliveryReceiptItems;
          selectedItem.acceptanceType = objRespone.item.proposalType
          this.setState({
            selectedItem
          })
        } else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      })
    }
  }

  onModalConFirmAction() {
    if (this.state.confirmType === "deleteRecords") {
      this.deleteRecords(this.state.deleteIds)
    } else {
      this.deleteRecord(this.state.deleteId)
    }
  }


  //event change input 

  handleSearchInCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState )[name] = value
    this.setState({ searchState })
  }
  handelNewSearch() {
    this._tableCustom.resetPageIndex();
    this.setState({ currentPage: 0, searchState: this.state.searchState }, () => {
      this.searchWithCondition(this.state.searchState);
    });
  }
  handleSearchChangeDateChange(name, date) {
    let searchState = this.state.searchState;
    (searchState )[name] = date;
    this.setState({ searchState });
  }

  handelChangePage(pageNum, pageSize) {

    this.setState({ currentPage: pageNum - 1, pageSize }, () => {
      this.reSearch();
    })
  }
  handleChangDepartment(value) {
    let searchState = this.state.searchState 
    searchState['department'] = value;
    this.setState(
      { searchState }
    );
  };
  handleChangeStatusType(value) {
    let searchState = this.state.searchState 
    searchState['status'] = value;
    this.setState(
      { searchState }
    );
  };

  reSearch() {
    if (this.state.lastsearchState === null) {
      this.searchWithCondition(this.state.searchState)
    } else {
      this.searchWithCondition(this.state.lastsearchState)
    }
  }
  searchWithCondition(stateSearch) {
    AcceptanceService.GetAllAcceptanceWithCondition(
      stateSearch.proposalCode || '',
      stateSearch.department.value,
      stateSearch.fromDate,
      stateSearch.toDate,
      this.state.pageSize, this.state.currentPage)
      .then(objRespone => {

        if (objRespone.isSuccess === true) {

          for (let item of objRespone.data) {
            item.updateTime = moment(new Date(item.updateTime)).format('DD-MM-YYYY');
          }
          for (let item of objRespone.data) {
            item.acceptanceResultLabel = getLabelString(item.acceptanceResult, acceptanceResultArr).toString();
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
    let lstDepartments = [];
    lstDepartments = [{ label: "Tất cả", value: 0 }];
    if (this.state.itemDefines !== undefined && this.state.itemDefines.props !== undefined) {
      lstDepartments = lstDepartments.concat(this.state.itemDefines.props[3].values)
    }

    let ItemDeatilsHeader = '';
    let viewItem = this.state.viewItem ;
    if(viewItem != undefined){
      ItemDeatilsHeader = "Bản nghiệm thu : "  + viewItem.acceptanceCode;
    }


    return (
      <React.Fragment>
        {/* search */}
        <div className="searchContainer" >
          <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px' }}>
            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label> Mã đề xuất </label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'proposalCode'} value={this.state.searchState.proposalCode} onChange={
                  this.handleSearchInCodeChangeInput.bind(this)
                } />
                  {this.validator.message('Mã đề xuất', this.state.searchState.proposalCode, 'alpha_num_dash_space')}
            </div>
           
            <div className="childSearchWrap">
              <div style={{ lineHeight: '5px' }}>
                <label>Từ ngày</label>
              </div>
              <div style={{
                fontSize: 12, fontFamily: 'roboto', fontWeight: 400, width: '120px',
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
                fontSize: 12, fontFamily: 'roboto', fontWeight: 400, width: '120px',
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
          keyColumn="acceptanceID"
          columns={AcceptanceColums}
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
          keyColumn={CONST_FEATURE.Acceptance.KEY_COLUMN}
          isHasUploader={true}
          referFeatureType={CONST_FEATURE.Acceptance.feature}
        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        
      {this.state.detailsItemModal == true && 
        <AcceptanceDetails  
           header={ ItemDeatilsHeader }
           item={this.state.viewItem}
          Modal={this.state.detailsItemModal}
          onCancel={() => { this.setState({ detailsItemModal: false })   }}
          onSubmitSuccess={()=>{ this.setState({ detailsItemModal: false });  this.handelNewSearch(); }}
          onUpdateItem={(data)=>{
            this.handleViewActionFromDataTable(data.acceptanceID);
          }}         
        ></AcceptanceDetails>
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
    fontSize: 12,
    lineHeight: '12px',
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
    fontSize: 12,
    lineHeight: '12px',
    fontFamily: 'roboto',
  }),
  singleValue: (provided, state) => {
    const opacity = state.isDisabled ? 0.5 : 1;
    const transition = 'opacity 300ms';

    return { ...provided, opacity, transition };
  }
}