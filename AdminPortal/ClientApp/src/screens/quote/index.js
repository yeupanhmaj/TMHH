import moment from 'moment';
import * as React from 'react';
import DatePicker from "react-datepicker";
import Select from 'react-select';
import { Button } from 'reactstrap';
import { CONST_FEATURE } from '../../commons';
import ConfirmDialog from '../../components/confirmDialog';
import TableCustom from '../../components/tableCustom';
import * as Actions from '../../libs/actions';
import { QuoteColums, QuoteDefine } from '../../models/quote';
import * as CommonService from '../../services/commonService';
import * as ProposalService from '../../services/proposalService';
import * as QuoteService from '../../services/quoteService';
import QuoteCreate from './quoteCreate';
import QuoteDetails from './quoteDetails';
import SimpleReactValidator from 'simple-react-validator';

const KEY_COLUMN = "quoteID"
export default class Quote extends React.Component {
  _toDateCalendar;
  _fromDateCalendar;
  _tableCustom;

  constructor(props) {
    super(props);
    this.state = {
      searchState: {
        fromDate: new Date(`01-01-${new Date().getFullYear()}`),
        toDate: new Date(),
        proposalCode: '',
        customerID: '',
        quoteCode: '',
      },
      lastsearchState: null,
      listContractCodeMaster: [],
      lstData: [],
      totalRecords: 0,
      pageSize: 10,
      itemModal: false,
      confirmModal: false,
      selectedItem: undefined,
      viewItem: undefined,
      confirmContent: '',
      deleteIds: [],
      currentPage: 0,
      headerEditModal: '',
      localItem: {},
      itemDefines: QuoteDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      quoteTypeOptions: [],
      statusOptions: [],
      lstDepartments: [],
    };
    this.validator = new SimpleReactValidator({
      messages: {
        required: 'Vui lòng không nhập kí tự đặc biệt'
      }
    });
  }



  componentWillMount() {
    let itemDefines = this.state.itemDefines;
    itemDefines.props[1].cbFunc = this.onLoadItemPropsal.bind(this);
    itemDefines.props[3].cbFunc = this.onLoadCustomer.bind(this);
  }

  onLoadCustomer(code, item) {
    CommonService.GetCustomerById(code).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        let selectedItem = item
        selectedItem.customerID = objRespone.item.customerID;
        selectedItem.customerName = objRespone.item.customerName;
        selectedItem.address = objRespone.item.address;
        selectedItem.phone = objRespone.item.phone;
        selectedItem.email = objRespone.item.email;
        selectedItem.taxCode = objRespone.item.taxCode;
        selectedItem.bankNumber = objRespone.item.bankNumber;
        selectedItem.bankName = objRespone.item.bankName;
        selectedItem.surrogate = objRespone.item.surrogate;
        selectedItem.position = objRespone.item.position;
        this.setState({
          selectedItem
        })

      } else {
        Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
      }
    }).catch(err => {

    })
  }

  onLoadItemPropsal(code, item) {
    ProposalService.GetProposalByCode(code).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        let selectedItem = item
        selectedItem.proposalID = objRespone.item.proposalID;
        selectedItem.proposalCode = objRespone.item.proposalCode;
        selectedItem.departmentName = objRespone.item.departmentName;
        selectedItem.items = objRespone.item.items;
        selectedItem.quoteCode = "BG-" + objRespone.item.proposalCode;
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
    QuoteService.getMasterData().then(
      result => {
        let lstCustomers = result[0]
        let lstDepartments = result[1]
        let itemDefines = this.state.itemDefines;
        if (itemDefines && itemDefines.props && itemDefines.props.length > 0) {
          if (lstCustomers.isSuccess) {
            itemDefines.props[3].values = [];
            for (let record of lstCustomers.data) {
              let item = { label: record.customerName, value: record.customerID };
              (itemDefines.props[3].values).push(item);
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
    QuoteService.deleteRecords(ids)
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
    QuoteService.deleteRecord(id)
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
    QuoteService.GetQuoteById(id).then(
      result => {
        if (result.isSuccess === true) {
          result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
          this.setState({ detailsItemModal: true, itemDetailsId: id, viewItem: result.item })

          Actions.setLoading(false);
        } else {
          Actions.openMessageDialog("lay data loi", result.err.msgString);
        }
      }).catch(ex => {
        Actions.openMessageDialog("lay data loi", ex.toString());
      })
  }


  onModalSubmmitWithAttachFile(item, files) {
    item = item
    item.customerID = +item.customerID;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');
    let total = 0
    for (let record of item.items) {
      if (record['itemPrice']) {
        total += (+(record['itemPrice'].toString().replace(/\./g, '')) * record['amount'])
      }
    }
    var data = new FormData();
    if (files.length > 0) {
      for (let file of files) {
        data.append('files', file);
      }
    }

    Object.keys(item).forEach(key => {
      if (key !== 'items')
        data.append(key, item[key]);
    });
    for (let i = 0; i < item.items.length; i++) {
      for (let key in item.items[i]) {
        data.append("items[" + i + "]." + key, item.items[i][key]);
      }
    }

    // item.status = +item.status;
    if (item["quoteID"] === undefined) {
      QuoteService.createQuotewithAttFiles(data)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({
              itemModal: false, selectedItem: {}
            })
            // if have conditions please keep
            this.reSearch();
          } else {
            Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
          }
        }).catch(err => {

        })
    } else {
      QuoteService.updateQuotewithAttFiles(item["quoteID"], data)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({
              itemModal: false, selectedItem: {}
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

  handelEditActionFromDataTable(item, type) {
    if (type === "delete") {
      let quoteCode = '';
      let quote = this.state.lstData.find((obj) => obj.quoteID.toString() === item.toString())
      if (item) {
        quoteCode = (quote).quoteCode;
      }
      let confirmContent = `Bạn có chắc muốn xóa báo giá : ${quoteCode}  ?`
      this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteId: +(item) })
    } else {
      if (type === "deleteRecords") {
        let quoteCodes = [];
        let quotes = item.map((id) => {
          return this.state.lstData.find((obj) =>
            obj.quoteID === id
          )
        })
        if (quotes) {
          for (let record of (quotes)) {
            quoteCodes.push('  ' + (record).quoteCode);
          }
        }
        let confirmContent = `Bạn có chắc muốn xóa các báo giá : ${quoteCodes.toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item })
      } else {
        let headerEditModal = "Chỉnh sửa báo giá";
        if (type === "insert") {
          headerEditModal = "thêm báo giá mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
        } else {
          //edit
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

  handleSearchQuoteCodeChangeInput(event) {
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
    QuoteService.GetAllQuoteWithCondition(
      stateSearch.fromDate,
      stateSearch.toDate,
      stateSearch.proposalCode,
      stateSearch.customerID.value || '',
      stateSearch.quoteCode,
      this.state.pageSize,
      this.state.currentPage)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          for (let item of objRespone.data) {
            item.dateIn = moment(new Date(item.inTime)).format('DD-MM-YYYY');
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
    if (this.state.itemDefines !== undefined && this.state.itemDefines.props !== undefined) {
      lstCustomers = lstCustomers.concat(this.state.itemDefines.props[3].values
      )
    }
    lstDepartments = lstDepartments.concat(this.state.lstDepartments);

    let ItemDeatilsHeader = '';
    let viewItem = this.state.viewItem;
    if (viewItem != undefined) {
      ItemDeatilsHeader = "Báo giá : " + viewItem.quoteCode;
    }

    return (
      <React.Fragment>
        {/* search */}
        <div className="searchContainer" >
          <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px' }}>
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
         
            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label>Mã đề xuất</label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'proposalCode'} value={this.state.searchState.proposalCode} onChange={
                  this.handleSearchProposalCodeChangeInput.bind(this)
                } />
                   {this.validator.message('Mã đề xuất', this.state.searchState.proposalCode, 'alpha_num_dash_space')}
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
          keyColumn={CONST_FEATURE.Quote.KEY_COLUMN}
          columns={QuoteColums}
          dataSource={this.state.lstData}
          totalRecords={this.state.totalRecords ? this.state.totalRecords : 0}
          onChangePage={this.handelChangePage.bind(this)}
          isModifyAll={true}
          onEditAction={this.handelEditActionFromDataTable.bind(this)}
          onViewAction={this.handleViewActionFromDataTable.bind(this)}
        />


        <QuoteCreate
          item={this.state.selectedItem}
          Modal={this.state.itemModal}
          onCancel={() => { this.setState({ itemModal: false }) }}
          onSubmitSuccess={() => {
            this.setState({ itemModal: false });
            this.handelNewSearch();
          }}
        ></QuoteCreate>

        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        {this.state.detailsItemModal == true &&
          <QuoteDetails
            header={ItemDeatilsHeader}
            item={this.state.viewItem}
            Modal={this.state.detailsItemModal}
            onCancel={() => { this.setState({ detailsItemModal: false }) }}
            onSubmitSuccess={() => { this.setState({ detailsItemModal: false }); this.handelNewSearch(); }}
            onUpdateItem={(data) => { this.setState({ viewItem: data }); this.handelNewSearch(); }}

          ></QuoteDetails>
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