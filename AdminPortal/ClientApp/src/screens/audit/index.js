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
import { AuditColums, AuditDefine } from '../../models/audit';
import * as AuditService from '../../services/auditService';
import AuditCreate from './auditCreate';
import AuditDetails from './auditDetails';





export default class Audit extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      searchState: {
        fromDate: new Date(`01-01-${new Date().getFullYear()}`),
        toDate: new Date(),
        proposalCode: '',
        departmentID: '',
        customerID: '',
        auditCode: '',
      },
      lastsearchState: null,
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
      itemDefines: AuditDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      auditTypeOptions: [],
      statusOptions: [],
      lstDepartments: [],
      lstCustomers: [],
  
  
      viewItem: undefined,
  
  
      //hot fix 
      tempItem : {} 
    };
  }

 


  componentWillMount() {
    let itemDefines = this.state.itemDefines;
    setTimeout(() => {
      AuditService.getListDefaultEmployee().then((result) => {
        let tempItem = (this.state.tempItem || {}) 
         if (result.isSuccess) {
           tempItem.employees = result.data;
           
            this.setState({tempItem , selectedItem:tempItem})
         }

     })
    }, 10);
   
    


    itemDefines.props[7].cbFunc = (employee, item) => {
      let selectedItem = (this.state.selectedItem || {}) 
        if (item.employees && employee) {
            let update = false;
            for (let i = 0; i < item.employees.length; i++) {
                if (item.employees[i].employeeID === employee.value) {
                    item.employees.splice(i,1)
                    update = true;
                    break;
                }
            }
            if(update){
              this.setState({selectedItem})
            }
        }
    }
    this.setState(itemDefines)

  }

  



  getmasterData() {
    AuditService.getMasterData().then(
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
    this.getmasterData()

    this.handelNewSearch();
  }

  deleteRecords(ids) {
    AuditService.deleteRecords(ids)
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
    AuditService.deleteRecord(id)
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
    AuditService.GetAuditById(id).then(
      result => {
        if (result.isSuccess === true) {
          result.item.inTime = moment(new Date(result.item.inTime)).format('DD-MM-YYYY')
          result.item.startTime = moment(new Date(result.item.startTime)).format('DD-MM-YYYY HH:mm')
          result.item.endTime = moment(new Date(result.item.endTime)).format('DD-MM-YYYY HH:mm');
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
    item.customerID = +item.customerID;
    item.inTime = moment(new Date(item.inTime)).format('YYYY-MM-DD');

    item.startTime = moment(new Date(item.startTime)).format('YYYY-MM-DD HH:mm');
    item.endTime = moment(new Date(item.endTime)).format('YYYY-MM-DD HH:mm');
    if (item.preside) {
      item.preside = item.preside.value
    }
    if(item.location == null) item.location = 1;
    if (item.secretary) {
      item.secretary = item.secretary.value
    }
    var data = new FormData();
    if (files.length > 0) {
      for (let file of files) {
        data.append('files', file);
      }
    }
    if(item.listDocument)
    {
      for (let i = 0; i < item.listDocument.length; i++) {
        for (let key in item.listDocument[i]) {
          data.append("documents[" + i + "]." + key, item.listDocument[i][key]);
        }
      }
    }
    Object.keys(item).forEach(key => {
      if (key !== 'employees') {
        data.append(key, item[key]);
      }

    });

    if (item.employees)
      for (let i = 0; i < item.employees.length; i++) {
        for (let key in item.employees[i]) {
          data.append("employees[" + i + "]." + key, item.employees[i][key]);
        }
      }

    let listQuote = this._inputQuote.getListQuote();
    for (let i = 0; i < listQuote.length; i++) {  
        data.append("QuoteIDs[" + i + "]", listQuote[i]);    
    }

    if (item["auditID"] === undefined) {
      Actions.setLoading(true);
      AuditService.createAuditwithAttFiles(data)
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
      AuditService.updateAuditwithAttFiles(item["auditID"], data)
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
  
      let confirmContent = `Bạn có chắc muốn xóa biên bản Họp giá   ?`
      this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteId: +(item ) })
    } else {
      if (type === "deleteRecords") {
        let auditCodes = [];
        let audits =   item.map((id) => {
          return this.state.lstData.find((obj) =>
            obj.auditID === id
          )
        })
        if (audits) {
          for (let record of (audits )) {
            auditCodes.push('  ' + (record ).auditCode);
          }
        }
        let confirmContent = `Bạn có chắc muốn xóa các biên bản Họp giá  : ${auditCodes.toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item  })
      } else {
        let headerEditModal = "Chỉnh sửa biên bản Họp giá ";
        if (type === "insert") {
          headerEditModal = "thêm biên bản Họp giá  mới";
          this.setState({ itemModal: true, selectedItem: this.state.tempItem, headerEditModal })
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

  handleSearchAuditCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState )[name] = value
    this.setState({ searchState })
  }

  handleSearchProposalCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState )[name] = value
    this.setState({ searchState })
  }

  handelNewSearch() {
    this._tableCustom.resetPageIndex();
    this.setState({ currentPage: 0, lastsearchState: this.state.searchState }, () => {
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
    let searchState = this.state.searchState 
    searchState['customerID'] = value;
    this.setState(
      { searchState }
    );
  };;

  //search
  reSearch() {
    if (this.state.lastsearchState === null) {
      this.searchWithCondition(this.state.lastsearchState)
    }
  }
  searchWithCondition(stateSearch) {
    AuditService.GetAllAuditWithCondition(
      stateSearch.fromDate,
      stateSearch.toDate,
      stateSearch.customerID,
      stateSearch.auditCode,
      stateSearch.quoteCode,
      this.state.pageSize,
      this.state.currentPage)
      .then(objRespone => {
  
        if (objRespone.isSuccess == true) {
      
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
  getChild(){
    return( 
    
    <AuditCreate
      ref={(c) => {this._inputQuote = c}}
      item={this.state.selectedItem}
      Modal={this.state.itemModal}
      onCancel={() => { this.setState({ itemModal: false }) }}
      onSubmitSuccess={()=>{ this.setState({ itemModal: false }) ;
       this.handelNewSearch();
    }}
    ></AuditCreate>
    )
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
      ItemDeatilsHeader = "Bản Kiểm giá : "  + viewItem.auditCode;
    }

    return (
      <React.Fragment>
        {/* search */}
        <div className="searchContainer" >
          <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px' }}>
            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label>Mã biên bản kiểm giá</label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'auditCode'} value={this.state.searchState.auditCode} onChange={
                  this.handleSearchAuditCodeChangeInput.bind(this)
                } />
            </div>

            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label>Mã báo giá</label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'quoteCode'} value={this.state.searchState.quoteCode} onChange={
                  this.handleSearchProposalCodeChangeInput.bind(this)
                } />
            </div>
            
            {/* <div className="childSearchWrap">
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
            </div> */}

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
                this.handelNewSearch()
              }}> Tìm kiếm
              </Button>
            </div>
          </div>
        </div>
        {/* search */}
        <TableCustom
          ref={(c) => { this._tableCustom = c }}
          keyColumn={CONST_FEATURE.Audit.KEY_COLUMN}
          columns={AuditColums}
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
          keyColumn={CONST_FEATURE.Audit.KEY_COLUMN}
          isHasUploader={true}
          referFeatureType={CONST_FEATURE.Audit.feature}
          OthersChild={this.getChild()}
        ></EditCreateItemModal>

        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        {this.state.detailsItemModal == true && 
        <AuditDetails  
           header={ ItemDeatilsHeader }
           item={this.state.viewItem}
          Modal={this.state.detailsItemModal}
          onCancel={() => { this.setState({ detailsItemModal: false })   }}
          onSubmitSuccess={()=>{ this.setState({ detailsItemModal: false });  this.handelNewSearch(); }}
          onUpdateItem={(data)=>{
            this.handleViewActionFromDataTable(data.auditID) }}         
        ></AuditDetails>
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