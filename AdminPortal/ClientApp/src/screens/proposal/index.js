import moment from 'moment';
import * as React from 'react';
import DatePicker from "react-datepicker";
import Select from 'react-select';
import { Button } from 'reactstrap';
import { CONST_FEATURE } from '../../commons';
import { getLabelString, statusArr } from '../../commons/propertiesType';
import ConfirmDialog from '../../components/confirmDialog';
import EditCreateItemModal from '../../components/editCreateItemModal';
import ItemDetails from '../../components/itemDetail';
import TableCustom from '../../components/tableCustom';
import * as Actions from '../../libs/actions';
import { ProposalColums, ProposalDefine } from '../../models/proposal';
import * as ProposalService from '../../services/proposalService';






export default class Proposal extends React.Component {
  _toDateCalendar;
  _fromDateCalendar;
  _tableCustom;

  constructor(props) {
    super(props);
    this.state = {
      searchState: {
        department: '',
        proposalType: '',
        status: '',
        proposalCode: '',
        fromDate: new Date(`01-01-${new Date().getFullYear()}`),
        toDate: new Date()
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
      itemDefines: ProposalDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      proposalTypeOptions: [],
      statusOptions: [],
    };

  }





  getmasterData() {
    ProposalService.getMasterData().then(
      result => {
        let lstProposalTypes = result[0]
        let lstDepartments = result[1]
        let itemDefines = this.state.itemDefines;
        if (itemDefines && itemDefines.props && itemDefines.props.length > 0) {
          if (lstProposalTypes.isSuccess) {
            itemDefines.props[2].values = [];
            for (let record of lstProposalTypes.data) {
              let item = { label: record.typeName, value: record.typeID };
              (itemDefines.props[2].values).push(item);
            }
          }
          if (lstDepartments.isSuccess) {
            itemDefines.props[3].values = [];
            itemDefines.props[4].values = [];
            for (let record of lstDepartments.data) {
              let item = { label: record.departmentName, value: record.departmentID };
              (itemDefines.props[3].values).push(item);
              if ("Hành chánh quản trị" === item.label || "Tài chính kế toán" === item.label) {
                (itemDefines.props[4].values).push(item);
                if ("Hành chánh quản trị" === item.label) {
                  itemDefines.props[4].valueDefault = item;
                }
              }
            }
          }
        }
        this.setState({ itemDefines });
      }
    )
  }
  componentDidMount() {
    this.getmasterData()
    this.getData(this.state.currentPage, this.state.pageSize);
  }

  getData(pageNumber, pageSize) {
    ProposalService.GetAllProposal(pageSize, pageNumber)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
     
          for (let item of objRespone.data) {
            item.labelStatus = getLabelString(item.status, statusArr).toString();
          }
          for (let item of objRespone.data) {
            item.dateIn = moment(new Date(item.dateIn)).format('DD-MM-YYYY');
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
    ProposalService.deleteRecords(ids)
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
    ProposalService.deleteRecord(id)
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
    this.setState({ detailsItemModal: true, itemDetailsId: id })
  }

  onModalSubmmitWithAttachFile(item, files) {
    item.departmentID = +item.departmentID;
    item.proposalType = +item.proposalType;
    item.curDepartmentID = +item.curDepartmentID;

    item.status = null;
    delete item.status;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');

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

    if (item["proposalID"] === undefined) {
      ProposalService.createProposalwithAttFiles(data)
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
      if (item.listDocument) {
        for (let i = 0; i < item.listDocument.length; i++) {
          for (let key in item.listDocument[i]) {
            data.append("documents[" + i + "]." + key, item.listDocument[i][key]);
          }
        }
      }
      ProposalService.updateProposalwithAttFiles(item["proposalID"], data)
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
    this.setState({ proposalCode: item.ProposalCode })
  }
  onModalSubmitAction() {

  }

  handelEditActionFromDataTable(item, type) {
    if (type === "delete") {
      let proposalCode = '';
      let proposal = this.state.lstData.find((obj) => obj.proposalID.toString() === item.toString())
      if (item) {
        proposalCode = (proposal).proposalCode;
      }
      let confirmContent = `Bạn có chắc muốn xóa đề xuất : ${proposalCode}  ?`
      this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteId: +(item) })
    } else {
      if (type === "deleteRecords") {
        let proposalCodes = [];
        let proposals = item.map((id) => {
          return this.state.lstData.find((obj) =>
            obj.proposalID === id
          )
        })
        if (proposals) {
          for (let record of (proposals)) {
            proposalCodes.push('  ' + (record).proposalCode);
          }
        }
        let confirmContent = `Bạn có chắc muốn xóa các đề xuất : ${proposalCodes.toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item })
      } else {
        let headerEditModal = "Chỉnh sửa đề xuất";
        if (type === "insert") {

          headerEditModal = "thêm đề xuất mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
        } else {
          ProposalService.GetProposalById(item[CONST_FEATURE.Proposal.KEY_COLUMN]).then(
            result => {
              if (result.isSuccess === true) {
                result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                result.item.labelStatus = getLabelString(result.item.status, statusArr).toString();
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

  handleSearchInCodeChangeInput(event) {
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

  handleChangeProposalType(value) {
    let searchState = this.state.searchState
    searchState['proposalType'] = value;
    this.setState(
      { searchState }
    );
  };

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


  //search
  reSearch() {
    if (this.state.lastsearchState === null) {
      this.getData(this.state.currentPage, this.state.pageSize);
    } else {
      this.searchWithCondition(this.state.lastsearchState)
    }
  }

  searchWithCondition(stateSearch) {
    ProposalService.GetAllProposalWithCondition(
      stateSearch.proposalType.value || '',
      stateSearch.department.value || '',
      stateSearch.proposalCode,
      stateSearch.status.value || '',
      stateSearch.fromDate,
      stateSearch.toDate,
      this.state.pageSize, this.state.currentPage)
      .then(objRespone => {
  
        if (objRespone.isSuccess === true) {
          for (let item of objRespone.data) {
            item.labelStatus = getLabelString(item.status, statusArr).toString();
          }
          for (let item of objRespone.data) {
            item.dateIn = moment(new Date(item.dateIn)).format('DD-MM-YYYY');
          }
          this.setState({
            lstData: objRespone.data,
            totalRecords: objRespone.totalRecords,

          })
        } else {
          Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
        }
      }).catch(err => {

      })
  }

  scanBarCode() {
    if (this.state.searchState.proposalCode !== "") {
      ProposalService.GetProposalByCode(this.state.searchState.proposalCode)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({ detailsItemModal: true, idItem: objRespone.item.proposalID })
          } else {
            Actions.openMessageDialog("Lỗi barcode", objRespone.err.msgString.toString());
          }
        })
    }
  }

  //search
  render() {
    let lstProposalTypes = [];
    let lstItemStatus = [];
    let lstDepartments = [];
    lstProposalTypes = [{ label: "Tất cả", value: '' }];
    if (this.state.itemDefines !== undefined && this.state.itemDefines.props !== undefined
      && this.state.itemDefines.props[2].values !== undefined) {
      lstProposalTypes = lstProposalTypes.concat(this.state.itemDefines.props[2].values)
    }
    lstItemStatus = [{ label: "Tất cả", value: '' }];
    if (this.state.itemDefines !== undefined && this.state.itemDefines.props !== undefined) {
      lstItemStatus = lstItemStatus.concat(this.state.itemDefines.props[5].values)
    }
    lstDepartments = [{ label: "Tất cả", value: '' }];
    if (this.state.itemDefines !== undefined && this.state.itemDefines.props !== undefined) {
      lstDepartments = lstDepartments.concat(this.state.itemDefines.props[3].values)
    }
    return (
      <React.Fragment>
        {/* search */}
        <div className="searchContainer" >
          <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px' }}>
            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label>Mã đề xuất</label>
              </div>
              <input style={{
                fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'proposalCode'} value={this.state.searchState.proposalCode} onChange={
                  this.handleSearchInCodeChangeInput.bind(this)
                }
                onFocus={(e) => {
                  let searchState = this.state.searchState;
                  (searchState)['proposalCode'] = ''
                  this.setState({ searchState })
                }}
                onKeyDown={(e) => {
                  if (e.key === 'Enter') {
                    this.scanBarCode();
                  }
                }}
              />
            </div>

            <div className="childSearchWrap">
              <div style={{ lineHeight: '5px' }}>
                <label>Loại đề xuất</label>
              </div>
              {lstProposalTypes !== undefined &&
                <Select
                  placeholder="Loại đề xuất"
                  styles={customStyles}
                  value={this.state.searchState.proposalType}
                  onChange={this.handleChangeProposalType.bind(this)}
                  options={lstProposalTypes}
                />
              }
            </div>

            <div className="childSearchWrap">
              <div style={{ lineHeight: '5px' }}>
                <label>Khoa/phòng</label>
              </div>
              {lstDepartments !== undefined &&
                <Select
                  placeholder="Khoa/phòng"
                  styles={customStyles}
                  value={this.state.searchState.department}
                  onChange={this.handleChangDepartment.bind(this)}
                  options={lstDepartments}
                />
              }
            </div>

            <div className="childSearchWrap">
              <div style={{ lineHeight: '5px' }}>
                <label>Trạng thái</label>
              </div>
              {lstItemStatus !== undefined &&
                <Select
                  placeholder="Trạng thái"
                  styles={customStyles}
                  value={this.state.searchState.status}
                  onChange={this.handleChangeStatusType.bind(this)}
                  options={lstItemStatus}
                />
              }
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
                this.handelNewSearch()
              }}> Tìm kiếm
              </Button>
            </div>
          </div>
        </div>
        {/* search */}
        <TableCustom
          ref={(c) => { this._tableCustom = c }}
          keyColumn={CONST_FEATURE.Proposal.KEY_COLUMN}
          columns={ProposalColums}
          dataSource={this.state.lstData}
          totalRecords={this.state.totalRecords ? this.state.totalRecords : 0}
          onChangePage={this.handelChangePage.bind(this)}
          isModifyAll={true}
          onEditAction={this.handelEditActionFromDataTable.bind(this)}
          onViewAction={this.handleViewActionFromDataTable.bind(this)} />
        <EditCreateItemModal
          item={this.state.selectedItem}
          Modal={this.state.itemModal}
          itemDefines={this.state.itemDefines}
          onSubmmitWithAttachFile={this.onModalSubmmitWithAttachFile.bind(this)}
          onCancel={() => { this.setState({ itemModal: false }) }}
          headerName={this.state.headerEditModal}
          keyColumn={CONST_FEATURE.Proposal.KEY_COLUMN}
          isHasUploader={true}
          referFeatureType={CONST_FEATURE.Proposal.feature}
        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        <ItemDetails feature={CONST_FEATURE.Proposal.feature}
          onclose={() => { this.setState({ detailsItemModal: false }) }} isOpen={this.state.detailsItemModal} idItem={this.state.itemDetailsId}></ItemDetails>

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