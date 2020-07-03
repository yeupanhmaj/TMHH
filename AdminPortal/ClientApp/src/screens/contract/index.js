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
import { ContractColums, ContractDefine } from '../../models/contract';
import * as ContractService from '../../services/contractService';
import * as QuoteService from '../../services/quoteService';
import ContractDetails from './contractDetails';
import SimpleReactValidator from 'simple-react-validator';
export default class Contract extends React.Component {
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
        contractCode: '',
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
      itemDefines: ContractDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      contractTypeOptions: [],
      statusOptions: [],
      lstDepartments: [],
      lstCustomers: [],
      viewItem: {},
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
    if (record && record.value)
      QuoteService.getQuoteInfo(record.value).then((objRespone) => {
        if (objRespone.isSuccess === true) {
          let selectedItem = item
          selectedItem.customerID = objRespone.item.customerID;
          selectedItem.customerName = objRespone.item.customerName;
          selectedItem.isVAT = objRespone.item.isVAT;
          selectedItem.vatNumber = objRespone.item.vatNumber;
          selectedItem.items = objRespone.item.items;
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
    ContractService.getMasterData().then(
      result => {
        let lstCustomers = result[0]
        let itemDefines = this.state.itemDefines;
        if (itemDefines && itemDefines.props && itemDefines.props.length > 0) {
          if (lstCustomers.isSuccess) {
            this.state.lstCustomers = [];
            for (let record of lstCustomers.data) {
              let item = { label: record.customerName, value: record.customerID };
              (this.state.lstCustomers).push(item);
            }
          }

        }
        this.setState({ itemDefines });
      }
    )
  }

  componentDidMount() {
    this.getmasterData()
    this.reSearch();
  }


  deleteRecords(ids) {
    ContractService.deleteRecords(ids)
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
    ContractService.deleteRecord(id)
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
    ContractService.GetContractById(id).then(
      result => {
        if (result.isSuccess === true) {

          result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
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
    item.inTime = moment(new Date()).format('YYYY-MM-DD HH:mm');
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD HH:mm');
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
      if (key != "quoteCode") {
        data.append(key, item[key]);
      } else {
        data.append("quoteID", item["quoteCode"].value);
      }
    });

    if (item["contractID"] === undefined) {
      Actions.setLoading(true);
      ContractService.createContractwithAttFiles(data)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({
              itemModal: false, selectedItem: {}
            })
            // if have conditions please keep
            this.reSearch();
            Actions.setLoading(false);
          } else {
            Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
          }
        }).catch(err => {

        })
    } 
  }
  onModalSubmitAction() {

  }

  handelEditActionFromDataTable(item, type) {
    if (type === "delete") {

      let confirmContent = `Bạn có chắc muốn xóa quyết định hợp đồng  ?`
      this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteId: +(item) })
    } else {
      if (type === "deleteRecords") {
        let contractCodes = [];
        let contracts = item.map((id) => {
          return this.state.lstData.find((obj) =>
            obj.contractID === id
          )
        })
        if (contracts) {
          for (let record of (contracts)) {
            contractCodes.push('  ' + (record).contractCode);
          }
        }
        let confirmContent = `Bạn có chắc muốn xóa các hợp đồng : ${contractCodes.toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item })
      } else {
        let headerEditModal = "Chỉnh sửa hợp đồng";
        if (type === "insert") {
          headerEditModal = "thêm hợp đồng mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
        } else {
          ContractService.GetContractById(item[CONST_FEATURE.Contract.KEY_COLUMN]).then(
            result => {
              if (result.isSuccess === true) {
                result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                this.setState({ itemModal: true, selectedItem: result.item, headerEditModal })
              } else {
                Actions.openMessageDialog("lay data loi", result.err.msgString);
              }
            }).catch(ex => {
              Actions.openMessageDialog("lay data loi", ex.toString());
            })
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
  }


  //event change input 

  handleSearchContractCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState)[name] = value
    this.setState({ searchState })
  }

  handleSearchProposalCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState)[name] = value
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
    (searchState)[name] = date;
    this.setState({ searchState });
  }

  handelChangePage(pageNum, pageSize) {
    this.setState({ currentPage: pageNum - 1, pageSize }, () => {
      this.reSearch();
    })
  }

  handleChangeDepartmentID(value) {
    let searchState = this.state.searchState
    searchState['departmentID'] = value;
    this.setState(
      { searchState }
    );
  };
  handleChangeCustomerID(value) {
    let searchState = this.state.searchState
    searchState['customerID'] = value;
    this.setState(
      { searchState }
    );
  };


  //search
  reSearch() {
    if (this.state.lastsearchState === null) {
      this.searchWithCondition(this.state.searchState)
    } else {
      this.searchWithCondition(this.state.lastsearchState)
    }
  }
  handleSearchQuoteCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState)[name] = value
    this.setState({ searchState })
  }
  searchWithCondition(searchState) {

    ContractService.GetAllContractWithCondition(
      searchState.fromDate,
      searchState.toDate,
      searchState.quoteCode,
      searchState.customerID.value || '',
      searchState.contractCode,
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
    let viewItem = this.state.viewItem;
    if (viewItem != undefined) {
      ItemDeatilsHeader = "Hợp đồng mã : " + viewItem.contractCode;
    }

    return (
      <React.Fragment>
        {/* search */}
        <div className="searchContainer" >
          <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px' }}>
            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label>Mã hợp đồng</label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'contractCode'} value={this.state.searchState.contractCode} onChange={
                  this.handleSearchContractCodeChangeInput.bind(this)
                } />
                {this.validator.message('Mã hợp đồng', this.state.searchState.contractCode, 'alpha_num_dash_space')}
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
                  selected={this.state.searchState.fromDate}
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
                  selected={this.state.searchState.toDate}
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
          keyColumn={CONST_FEATURE.Contract.KEY_COLUMN}
          columns={ContractColums}
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
          keyColumn={CONST_FEATURE.Contract.KEY_COLUMN}
          isHasUploader={true}
          referFeatureType={CONST_FEATURE.Contract.feature}
        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        {this.state.detailsItemModal == true &&
          <ContractDetails
            header={ItemDeatilsHeader}
            item={this.state.viewItem}
            Modal={this.state.detailsItemModal}
            onCancel={() => { this.setState({ detailsItemModal: false }) }}
            onSubmitSuccess={() => { this.setState({ detailsItemModal: false }); this.handelNewSearch(); }}
            onUpdateItem={(data) => {
              this.handleViewActionFromDataTable(data.contractID)
            }}
          ></ContractDetails>
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