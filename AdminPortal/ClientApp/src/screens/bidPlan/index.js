import moment from 'moment';
import * as React from 'react';
import DatePicker from "react-datepicker";
import Select from 'react-select';
import { Button } from 'reactstrap';
import { CONST_FEATURE } from '../../commons';
import { BidMethodArr, getLabelString } from '../../commons/propertiesType';
import ConfirmDialog from '../../components/confirmDialog';
import EditCreateItemModal from '../../components/editCreateItemModal';
import TableCustom from '../../components/tableCustom';
import * as Actions from '../../libs/actions';
import { BidPlanColums, BidPlanDefine } from '../../models/bidPlan';
import * as BidPlanService from '../../services/bidPlanService';
import * as QuoteService from '../../services/quoteService';
import BidplanDetails from './bidplanDetails';

import SimpleReactValidator from 'simple-react-validator';


export default class BidPlan extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      searchState: {
        fromDate: new Date(`01-01-${new Date().getFullYear()}`),
        toDate: new Date(),
        quoteCode: '',
        customerID: '',
        bidPlanCode: '',
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
      itemDefines: BidPlanDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      bidPlanTypeOptions: [],
      statusOptions: [],
      lstDepartments: [],
      lstCustomers: [],

      viewItem: {}
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
    itemDefines.props[3].cbFunc = this.loadQuater.bind(this);
    this.setState(itemDefines)
  }

  onLoadQuote(code, item) {

    if (code == undefined || item == undefined) return
    QuoteService.getQuoteInfo(code.value).then((objRespone) => {

      if (objRespone.isSuccess === true) {
        let selectedItem = item

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


  loadQuater(value, item) {
    let selectedItem = item
    let month = value.getMonth() + 1;
    let quater = (Math.ceil(month / 3));
    switch (quater) {
      case 1:
        selectedItem.bidTime = "Quý I năm " + value.getFullYear();
        break;
      case 2:
        selectedItem.bidTime = "Quý II năm " + value.getFullYear();
        break;
      case 3:
        selectedItem.bidTime = "Quý III năm " + value.getFullYear();
        break;
      case 4:
        selectedItem.bidTime = "Quý IV năm " + value.getFullYear();
        break;
      default:
        selectedItem.bidTime = " ";
    }
    this.setState({
      selectedItem
    })
  }

  getmasterData() {
    BidPlanService.getMasterData().then(
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

  getData(pageNumber, pageSize) {
    BidPlanService.GetAllBidPlan(pageSize, pageNumber)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          for (let item of objRespone.data) {
            item.dateIn = moment(new Date(item.dateIn)).format('DD-MM-YYYY');
            item.bidMethodLabel = getLabelString(item.bidMethod, BidMethodArr).toString();
          }
          this.setState({
            lstData: objRespone.data,
            totalRecords: objRespone.totalRecords
            // currentPage:pageNumber
          })
        } else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      }).catch(err => {

      })
  }

  deleteRecords(ids) {
    BidPlanService.deleteRecords(ids)
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
    BidPlanService.deleteRecord(id)
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
    BidPlanService.GetBidPlanById(id).then(
      result => {
        if (result.isSuccess === true) {
          result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
          this.setState({ detailsItemModal: true, viewItem: result.item })
          Actions.setLoading(false);
        }
      }).catch(ex => {
        Actions.openMessageDialog("lay data loi", ex.toString());
      })
  }





  onModalSubmmitWithAttachFile(item, files) {
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');

    var data = new FormData();
    if (files.length > 0) {
      for (let file of files) {
        data.append('files', file);
      }
    }

    Object.keys(item).forEach(key => {
      if (key != "quoteCode") {
        data.append(key, item[key]);
      } else {
        data.append("quoteID", item["quoteCode"].value);
      }
    });

    // item.status = +item.status;
    if (item["bidPlanID"] === undefined) {
      Actions.setLoading(true);
      BidPlanService.createBidPlanwithAttFiles(data)
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

      let confirmContent = `Bạn có chắc muốn xóa kế hoạch chọn thầu ?`
      this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteId: +(item) })
    } else {
      if (type === "deleteRecords") {
        let bidPlanCodes = [];
        let bidPlans = item.map((id) => {
          return this.state.lstData.find((obj) =>
            obj.bidPlanID === id
          )
        })
        if (bidPlans) {
          for (let record of (bidPlans)) {
            bidPlanCodes.push('  ' + (record).bidPlanCode);
          }
        }
        let confirmContent = `Bạn có chắc muốn xóa các kế hoạch chọn thầu : ${bidPlanCodes.toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item })
      } else {
        let headerEditModal = "Chỉnh sửa kế hoạch chọn thầu ";
        if (type === "insert") {
          headerEditModal = "Thêm kế hoạch chọn thầu  mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
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

  handleSearchBidPlanCodeChangeInput(event) {
    let value = event.target.value;
    let name = event.target.name;
    let searchState = this.state.searchState;
    (searchState)[name] = value
    this.setState({ searchState })
  }

  handleSearchQuoteCodeChangeInput(event) {
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
    // this.getData(pageNum - 1, pageSize);
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
      this.getData(this.state.currentPage, this.state.pageSize);
    } else {
      this.searchWithCondition(this.state.lastsearchState)
    }
  }
  searchWithCondition(stateSearch) {
    BidPlanService.GetAllBidPlanWithCondition(
      stateSearch.fromDate,
      stateSearch.toDate,
      stateSearch.quoteCode,
      stateSearch.customerID.value || '',
      stateSearch.bidPlanCode,
      this.state.pageSize,
      this.state.currentPage)
      .then(objRespone => {

        if (objRespone.isSuccess === true) {
          for (let item of objRespone.data) {
            item.dateIn = moment(new Date(item.dateIn)).format('DD-MM-YYYY');
            item.bidMethodLabel = getLabelString(item.bidMethod, BidMethodArr).toString();
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
    let viewItem = this.state.viewItem;
    if (viewItem != undefined) {
      ItemDeatilsHeader = "Kế hoạch chọn thầu : " + viewItem.bidPlanCode;
    }


    return (
      <React.Fragment>
        {/* search */}
        <div className="searchContainer" >
          <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px' }}>
            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label>Mã kế hoạch chọn thầu</label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'bidPlanCode'} value={this.state.searchState.bidPlanCode} onChange={
                  this.handleSearchBidPlanCodeChangeInput.bind(this)
                } />
                  {this.validator.message('Mã kế hoạch chọn thầu', this.state.searchState.bidPlanCode, 'alpha_num_dash_space')}
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
                  this.handleSearchQuoteCodeChangeInput.bind(this)
                } />
                  {this.validator.message('Mã báo giá', this.state.searchState.quoteCode, 'alpha_num_dash_space')}
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
          keyColumn={CONST_FEATURE.BidPlan.KEY_COLUMN}
          columns={BidPlanColums}
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
          keyColumn={CONST_FEATURE.BidPlan.KEY_COLUMN}
          isHasUploader={true}
          referFeatureType={CONST_FEATURE.BidPlan.feature}
        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        {this.state.detailsItemModal == true &&
          <BidplanDetails
            header={ItemDeatilsHeader}
            item={this.state.viewItem}
            Modal={this.state.detailsItemModal}
            onCancel={() => { this.setState({ detailsItemModal: false }) }}
            onSubmitSuccess={() => { this.setState({ detailsItemModal: false }); this.handelNewSearch(); }}
            onUpdateItem={(data) => {
              this.handleViewActionFromDataTable(data.bidPlanID)
            }}
          ></BidplanDetails>
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
    fontSize: 11,
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