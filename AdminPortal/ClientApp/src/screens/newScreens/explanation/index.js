import moment from 'moment';
import * as React from 'react';
import { CONST_FEATURE } from '../../../commons';
import CommonSearch from '../../../commons/controls/commonSearch';
import ConfirmDialog from '../../../components/confirmDialog';
import EditCreateItemModal from '../../../components/editCreateItemModal';
import ItemDetails from '../../../components/itemDetail';
import TableCustom from '../../../components/tableCustom';
import * as Actions from '../../../libs/actions';
import { ExplanationColums, ExplanationDefine, ExplanationModel } from '../../../models/explanation'; //, ProposalTypeModel
import * as ExplanationService from '../../../services/explanationService';
import * as ProposalService from '../../../services/proposalService';


export default class Proposal extends React.Component {
  _search;
  _tableCustom;

  constructor(props) {
    super(props);
    this.state = {
      lastsearchParameter: null,
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
      itemDefines: ExplanationDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      explanationTypeOptions: [],
      statusOptions: [],
      lstDepartments: []
    };
  
  }

  
  componentWillMount() {
    let itemDefines = this.state.itemDefines;
    itemDefines.props[1].cbFunc = this.onLoadItemPropsal.bind(this);
    this.setState(itemDefines)
  }


  componentDidMount() {
    this.getData(this.state.currentPage, this.state.pageSize);
  }

  getData(pageNumber, pageSize) {
    ExplanationService.GetAllExplanation(pageSize, pageNumber)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {

          // for (let item of objRespone.data) {
          //   item.labelStatus = getLabelString(item.status).toString();
          // }
          for (let item of objRespone.data) {
            item.inTime = moment(new Date(item.inTime)).format('DD-MM-YYYY');
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
    ExplanationService.deleteRecords(ids)
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
    ExplanationService.deleteRecord(id)
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
    item = item;
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
    if (item["explanationID"] === undefined) {
      ExplanationService.createExplanationwithAttFiles(data)
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
    } else {
      ExplanationService.updateExplanationwithAttFiles(item["explanationID"], data)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({
              itemModal: false , selectedItem:{}
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
        let confirmContent = `Bạn có chắc muốn xóa với Ids : ${item.toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item  })
      } else {
        let headerEditModal = "Chỉnh sửa dữ liệu";
        if (type === "insert") {
          headerEditModal = "thêm dữ liệu mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
        } else {
          ExplanationService.GetExplanationById(item[CONST_FEATURE.Explanation.KEY_COLUMN]).then(
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


  handelNewSearch() {
    let searchParameter = this._search.getSearchParameter() 
    this._tableCustom.resetPageIndex();
    this.setState({ currentPage: 0 }, () => {
      this.searchWithCondition(searchParameter);
    });
  }


  handelChangePage(pageNum, pageSize) {
    // this.getData(pageNum - 1, pageSize);
    this.setState({ currentPage: pageNum - 1, pageSize }, () => {
      this.reSearch();
    })
  }

  handleChangeStatusType(value) {
    let searchParameter = this._search.getSearchParameter() 
    searchParameter['status'] = value;
    this.setState(
      { searchParameter }
    );
  };


  //search
  reSearch() {
    this.searchWithCondition(this._search.getSearchParameter())
  }

  searchWithCondition(stateSearch) {
    ExplanationService.GetAllExplanationWithCondition(
      stateSearch.proposalCode || '',
      stateSearch.department.value,
      stateSearch.fromDate,
      stateSearch.toDate,
      this.state.pageSize, this.state.currentPage)
      .then(objRespone => {

        if (objRespone.isSuccess === true) {

          for (let item of objRespone.data) {
            item.inTime = moment(new Date(item.inTime)).format('DD-MM-YYYY');
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

  onLoadItemPropsal(code, item) {
    ProposalService.GetProposalByCode(code).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        let selectedItem = item
        selectedItem.proposalID = objRespone.item.proposalID;
        selectedItem.proposalCode = objRespone.item.proposalCode;
        selectedItem.departmentName = objRespone.item.departmentName;
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

  //search
  render() {
    let lstDepartments = [];
    lstDepartments = [{ label: "Tất cả", value: 0 }];
    if (this.state.itemDefines !== undefined && this.state.itemDefines.props !== undefined) {
      lstDepartments = lstDepartments.concat(this.state.itemDefines.props[3].values)
    }
    return (
      <React.Fragment>
        <CommonSearch
          handelNewSearch={this.handelNewSearch.bind(this)}
          ref={(c) => { this._search = c }}
        ></CommonSearch>

        <TableCustom
          ref={(c) => { this._tableCustom = c }}
          keyColumn={CONST_FEATURE.Explanation.KEY_COLUMN}
          columns={ExplanationColums}
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
          keyColumn={CONST_FEATURE.Explanation.KEY_COLUMN}
          isHasUploader={true}
          referFeatureType={CONST_FEATURE.Explanation.feature}

        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        <ItemDetails  feature={CONST_FEATURE.Explanation.feature}
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