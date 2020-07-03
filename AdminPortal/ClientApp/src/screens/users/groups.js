import * as React from 'react';
import ConfirmDialog from '../../components/confirmDialog';
import EditCreateItemModal from '../../components/editCreateItemModal';
import TableCustom from '../../components/tableCustom';
import * as Actions from '../../libs/actions';
import { UserGroupDefine, UserGroupColums } from '../../models/userGroup';
import * as UserGroupService from '../../services/userGroupService';
import { Button } from 'reactstrap';

export default class UserComp extends React.Component {
  _tableCustom;

  constructor(props) {
    super(props);
    this.state = {
      searchState: {
        groupName: '',
      } ,
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
      itemDefines: UserGroupDefine,
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
    this.setState(itemDefines)
  }

  getmasterData() {

  }
  componentDidMount() {
    this.getmasterData()
    this.getData(this.state.currentPage, this.state.pageSize);
  }

  getData(pageNumber, pageSize) {
    UserGroupService.GetAllUserGroups(pageSize, pageNumber)
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
    UserGroupService.deleteUserGroups(ids)
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
    UserGroupService.deleteUserGroup(id.toString())
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

  handleViewActionFromDataTable(id) {
    this.setState({ detailsItemModal: true, itemDetailsId: id })
  }

  onSubmmit(item) {
    if (this.state.confirmType === "insert") {
      UserGroupService.createUserGroup(item)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({
              itemModal: false , selectedItem:{}
            })

            this.reSearch();
          } else {
            Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
          }
        }).catch(err => {

        })
    } else {
      UserGroupService.updateUserGroup(item)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            this.setState({
              itemModal: false
            })

            this.reSearch();
          } else {
            Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
          }
        }).catch(err => {

        })
    }
  }

   handelEditActionFromDataTable(item, type )  {

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
          this.setState({ itemModal: true, selectedItem: item, confirmType: type, headerEditModal })
        } else {
          UserGroupService.GetUserGroupById(item['groupID']).then(
            result => {
              if (result.isSuccess === true) {

                this.setState({ itemModal: true, selectedItem: result.item, confirmType: type, headerEditModal })
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
    searchState[name] = value
    this.setState({ searchState })
  }

  handelNewSearch() {
    this._tableCustom.resetPageIndex();
    this.setState({ currentPage: 0 }, () => {
      this.searchWithCondition(this.state.searchState);
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
    this.searchWithCondition(this.state.searchState)
  }

  searchWithCondition(stateSearch) {
    UserGroupService.GetAllUserWithCondition(
      stateSearch.groupName || '',
      this.state.pageSize, this.state.currentPage)
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

  render() {
    return (
      <React.Fragment>
        <div className="searchContainer" >
          <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px', marginTop: 10 }}>
            <div className="childSearchWrap" >
              <div style={{ lineHeight: '5px' }}>
                <label> Tên Nhóm </label>
              </div>
              <input style={{
                fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                border: 'none'
              }}
                type="text" className="form-control" name={'groupName'} value={this.state.searchState.groupName} onChange={
                  this.handleSearchInCodeChangeInput.bind(this)
                } />
            </div>
            <div className="childSearchWrap">
              <Button className="btn-search" style={{
                width: '110px',
                marginTop: '10px',
                height: '35px'
              }} onClick={() => {
                this.reSearch()
              }}> Tìm kiếm
             </Button>
            </div>
          </div>

        </div>
        <TableCustom
          ref={(c) => { this._tableCustom = c }}
          keyColumn={"groupID"}
          columns={UserGroupColums}
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
          onSubmmit={this.onSubmmit.bind(this)}
          onCancel={() => { this.setState({ itemModal: false }) }}
          headerName={this.state.headerEditModal}
          keyColumn={"groupID"}

        ></EditCreateItemModal>
        <ConfirmDialog
          onConfirm={this.onModalConFirmAction.bind(this)}
          Modal={this.state.confirmModal}
          onCancel={() => { this.setState({ confirmModal: false }) }}
          content={this.state.confirmContent}
        >
        </ConfirmDialog>
      </React.Fragment>
    );
  }
};
