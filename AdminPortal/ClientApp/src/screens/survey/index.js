import moment from 'moment';
import * as React from 'react';
import { CONST_FEATURE } from '../../commons';
import CommonSearch from '../../commons/controls/commonSearch';
import ConfirmDialog from '../../components/confirmDialog';
import EditCreateItemModal from '../../components/editCreateItemModal';
import ItemDetails from '../../components/itemDetail';
import TableCustom from '../../components/tableCustom';
import * as Actions from '../../libs/actions';
import { SurveyColums, SurveyDefine } from '../../models/survey'; //, ProposalTypeModel
import * as ProposalService from '../../services/proposalService';
import * as SurveyService from '../../services/surveyService';





export default class Proposal extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
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
      itemDefines: SurveyDefine,
      confirmType: '',
      deleteId: 0,
      detailsItemModal: false,
      itemDetailsId: -1,
      surveyTypeOptions: [],
      statusOptions: [],
    };
  }

  

  componentWillMount() {
    let itemDefines = this.state.itemDefines;
    itemDefines.props[1].cbFunc = this.onLoadItemPropsal.bind(this);
    this.setState(itemDefines)
  }

  getmasterData() {
    SurveyService.getMasterData().then(
      result => {

        let lstSurveyDepartments = result[0]
        let lstProposalTypes = result[1]
        let itemDefines = this.state.itemDefines;

        if (itemDefines && itemDefines.props && itemDefines.props.length > 0) {
          if (lstSurveyDepartments.isSuccess) {
            itemDefines.props[4].values = [];
            for (let record of lstSurveyDepartments.data) {
              let item = { label: record.departmentName, value: record.departmentID };
              (itemDefines.props[4].values ).push(item);
            }
          }
          if (lstProposalTypes.isSuccess) {
            itemDefines.props[2].values = [];
            for (let record of lstProposalTypes.data) {
              let item = { label: record.typeName, value: record.typeID };
              (itemDefines.props[2].values ).push(item);
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
    SurveyService.GetAllSurvey(pageSize, pageNumber)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {

          // for (let item of objRespone.data) {
          //   item.labelStatus = getLabelString(item.status).toString();
          // }
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
    SurveyService.deleteRecords(ids)
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
    SurveyService.deleteRecord(id)
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
  handleViewActionFromDataTable(id) {
    this.setState({ detailsItemModal: true, itemDetailsId: id })
  }

  onModalSubmmitWithAttachFile(item, files) {
    item = item;
    item.dateIn = moment(new Date(item.dateIn)).format('YYYY-MM-DD');
    var data = new FormData();
    if (files.length > 0) {
      for (let file of files) {
          data.append('files', file);
      }
  }

    Object.keys(item).forEach(key => {
      if (key !== 'items' && key !== 'surveyItems')
        data.append(key, item[key]);
    });

    for (let i = 0; i < item.items.length; i++) {
      for (let key in item.items[i]) {
        data.append("items[" + i + "]." + key, item.items[i][key]);
      }
    }
  
    for (let i = 0; i < item.surveyItems.length; i++) {
      for (let key in item.surveyItems[i]) {
        data.append("surveyItems[" + i + "]." + key, item.surveyItems[i][key]);
      }
    }

    if (item["surveyID"] === undefined) {
      SurveyService.createSurveywithAttFiles(data)
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
    } else {
      SurveyService.updateSurveywithAttFiles(item["surveyID"], data)
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
        let headerEditModal = "Chỉnh sửa khảo sát";
        if (type === "insert") {
          headerEditModal = "thêm dữ khảo sát mới";
          this.setState({ itemModal: true, selectedItem: item, headerEditModal })
        } else {
          SurveyService.GetSurveyById(item[CONST_FEATURE.Survey.KEY_COLUMN]).then(
            result => {
              if (result.isSuccess === true) {
                result.item.dateIn = moment(new Date(result.item.dateIn)).format('DD-MM-YYYY')
                //     result.item.labelStatus = getLabelString(result.item.status).toString();
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

  onLoadItemPropsal(code, item) {

    ProposalService.GetProposalByCode(code).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        let selectedItem = item
        selectedItem.proposalID = objRespone.item.proposalID;
        selectedItem.proposalType = objRespone.item.proposalType;
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
  reSearch() {
    this.searchWithCondition(this._search.getSearchParameter())
  }
  searchWithCondition(stateSearch) {
    SurveyService.GetAllSurveyWithCondition(
      stateSearch.proposalCode || '',
      stateSearch.department.value,
      stateSearch.fromDate,
      stateSearch.toDate,
      this.state.pageSize, this.state.currentPage)
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
    return (
      <React.Fragment>
        {/* search */}
        <CommonSearch
          handelNewSearch={this.handelNewSearch.bind(this)}
          ref={(c) => { this._search = c }}
        ></CommonSearch>
        {/* search */}
        <TableCustom
          ref={(c) => { this._tableCustom = c }}
          keyColumn={CONST_FEATURE.Survey.KEY_COLUMN}
          columns={SurveyColums}
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
          onSubmmitWithAttachFile={this.onModalSubmmitWithAttachFile.bind(this)}
          onCancel={() => { this.setState({ itemModal: false }) }}
          headerName={this.state.headerEditModal}
          keyColumn={CONST_FEATURE.Survey.KEY_COLUMN}
          isHasUploader={true}
          referFeatureType={CONST_FEATURE.Survey.feature}

        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
        <ItemDetails   feature={CONST_FEATURE.Survey.feature}
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