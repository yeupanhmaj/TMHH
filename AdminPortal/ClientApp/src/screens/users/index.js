import * as React from 'react';
import { Tab, TabList, TabPanel, Tabs } from 'react-tabs';
import { UserModel } from '../../models/user';
import UserComp from './users'
import GroupComp from './groups'
import PermissionComp from './permission'

export default class User extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      lstData: [],
      totalRecords: 0,
      pageSize: 10,
      itemModal: false,
      confirmModal: false,
      selectedItem: {},
      confirmContent: '',
      deleteIds: [],
      currentPage: 0,
      headerEditModal: '',
    };
  }

  

  onModalConFirmAction() {
    this.deleteRecord(this.state.deleteIds)
  }

    handelEditActionFromDataTable(item, type ) {
    if (type === "delete") {
      let confirmContent = `Bạn có chắc muốn xóa với Ids : ${item.toString()}  ?`
      this.setState({ confirmModal: true, confirmContent, deleteIds: item  })
    } else {
      let headerEditModal = "Chỉnh sửa dữ liệu";
      if (type === "insert") headerEditModal = "thêm dữ liệu mới";
      this.setState({ itemModal: true, selectedItem: item, headerEditModal })
    }
  }



  render() {
    return (
      <React.Fragment>
        <div style={{ marginTop: -30 }}>
          <Tabs >
            <TabList>
              <Tab>Quản lý người dùng</Tab>
              <Tab>Quản lý nhóm</Tab>      
              <Tab>Quản lý quyền sử dụng</Tab>          
            </TabList>
            <TabPanel>
              <UserComp />
            </TabPanel>
            <TabPanel>
              <GroupComp />
            </TabPanel> 
            <TabPanel>
              <PermissionComp />
            </TabPanel>        
          </Tabs>
        </div>
      </React.Fragment>
    );
  }
};
