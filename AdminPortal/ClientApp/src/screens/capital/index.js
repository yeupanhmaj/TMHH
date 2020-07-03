import moment from 'moment';
import * as React from 'react';
import CommonSearch from '../../commons/controls/commonSearch';
import ConfirmDialog from '../../components/confirmDialog';
import EditCreateItemModal from '../../components/editCreateItemModal';
import ItemDetails from '../../components/itemDetail';
import TableCustom from '../../components/tableCustom';
import * as Actions from '../../libs/actions';
import { CapitalColums, CapitalDefine, CapitalModel} from '../../models/capital';
import * as CapitalService from '../../services/capitalService';
import { CONST_FEATURE } from '../../commons';

import {GenericArr, getLabelString  } from '../../commons/propertiesType'




export default class Proposal extends React.Component {
  _search;
  _tableCustom;

  constructor(props) {
    super(props);
    this.state = {
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
      itemDefines: {},
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      capitalTypeOptions: [],
      statusOptions: [],
    };
  }

 

  componentWillMount() {
    let itemDefines = this.state.itemDefines;
    this.setState(itemDefines)
  }

  componentDidMount() {
    this.getData();
  }

  getData() {
    CapitalService.GetAllCapital()
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          this.setState({
            lstData: objRespone.data,
            totalRecords: objRespone.totalRecords
          })
        } else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      }).catch(err => {
       
      })
  }

  deleteRecords(ids) {
    CapitalService.deleteRecords(ids)
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
    CapitalService.deleteRecord(id)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          this.setState({
            confirmModal: false
          })
          this.reSearch();
        } else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      }).catch(err => {
       
      })
  }
  //handle event from custom table
  // handleViewActionFromDataTable(id) {
  //   this.setState({ detailsItemModal: true, itemDetailsId: id })
  // }

  onModalSubmmitWithAttachFile(item) {
 
    var data = new FormData();

    if (item["capitalID"] === undefined) {
      CapitalService.createCapital(item)
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
      CapitalService.updateCapital( item)
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
        let confirmContent = `Bạn có chắc muốn xóa với Ids : ${item.toString()}  ?`
        this.setState({ confirmModal: true, confirmContent, confirmType: type, deleteIds: item  })
      } else {
        let headerEditModal = "Chỉnh sửa nhân viên";
        if (type === "insert") {
          headerEditModal = "thêm nhân viên mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
        } else {
          CapitalService.GetCapitalById(item[CONST_FEATURE.Capital.KEY_COLUMN]).then(
            result => {
              if (result.isSuccess === true) {
            
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
    let searchState = this._search.getSearchParameter() ;
    (searchState )[name] = value
    this.setState({ searchState })
  }
  handelNewSearch() {
    this._tableCustom.resetPageIndex();
    this.setState({ currentPage: 0 }, () => {
      this.searchWithCondition(this._search.getSearchParameter());
    });
  }


  handelChangePage(pageNum, pageSize) {
    // this.getData(pageNum - 1, pageSize);
    this.setState({ currentPage: pageNum - 1, pageSize }, () => {
      this.reSearch();
    })
  }


  //search
  reSearch() {
    this.searchWithCondition(this._search.getSearchParameter())
  }
  searchWithCondition(stateSearch) {
    CapitalService.GetAllCapital()
      .then(objRespone => {

        if (objRespone.isSuccess === true) {
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
    return (
      <React.Fragment>
        <TableCustom
          ref={(c) => { this._tableCustom = c }}
          keyColumn={CONST_FEATURE.Capital.KEY_COLUMN}
          columns={CapitalColums}
          dataSource={this.state.lstData}
          totalRecords={this.state.totalRecords ? this.state.totalRecords : 0}
          onChangePage={this.handelChangePage.bind(this)}
          isModifyAll={true}
          onEditAction={this.handelEditActionFromDataTable.bind(this)}
          //onViewAction={this.handleViewActionFromDataTable.bind(this)} 
        />
        <EditCreateItemModal
          item={this.state.selectedItem}
          Modal={this.state.itemModal}
          itemDefines={this.state.itemDefines}
          onSubmmit={this.onModalSubmmitWithAttachFile.bind(this)}
          onCancel={() => { this.setState({ itemModal: false }) }}
          headerName={this.state.headerEditModal}
          keyColumn={CONST_FEATURE.Capital.KEY_COLUMN}
          isHasUploader={false}
          referFeatureType={CONST_FEATURE.Capital.feature}

        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        <ItemDetails   feature={CONST_FEATURE.Capital.feature}
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