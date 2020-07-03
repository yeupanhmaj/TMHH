import * as React from 'react';
import { Button, DatePicker, Input, Table, Modal, Select } from 'antd';
import * as Service from '../../services/practiceService';

export default class Practice extends React.Component {
  constructor(props) {

    super(props);
    this.state = {
      txtCode: 'Nhập mã cần tìm kiếm',
      vinsible: false,
      txtName: 'name test',
      txtID: 'id test',
      title: '',
      data: [],
      selectedRowKeys: [],
      tempData: []
    };
  }

  showNewModal = () => {
    this.setState({
      visible: true,
      txtID: '',
      txtName: '',
      title: 'Tạo mới'
    });
  };
  showEditModal = (e) => {
   
    this.setState({
      visible: true,
      txtID: e.userID,
      txtName: e.userName,
      title: 'Chỉnh sửa'
    });
  };
  handleOk = e => {
    if (this.state.title == 'Tạo mới') {
      Service.createUser({
        UserID: this.state.txtID,
        UserName: this.state.txtName
      })
        .then((response) => {
          if (response.isSuccess == true) {
            let { data } = this.state;

            let newUser = {
              userID: this.state.txtID,
              userName: this.state.txtName,
              key: this.state.txtID
            };


            data.unshift(newUser);

            this.setState({
              data: JSON.parse(JSON.stringify(data)),
              visible: false
            });

          }
        });
    } else {

      Service.updateUser({
        UserID: this.state.txtID,
        UserName: this.state.txtName
      })
        .then((response) => {
          if (response.isSuccess == true) {
            //window.location.reload();
          }
        });
      this.setState({
        visible: false,
      });
    }

  };

  handleCancel = e => {

    this.setState({
      visible: false,
    });
  };

  handleSearch = e => {
    Service.GetPractice(this.state.txtCode).then((response) => {
      if (response.isSuccess == true) {

        this.setState({ data: response.data })
      }
    })
  };

  componentDidMount() {
    Service.GetAlllPractice().then((response) => {
      if (response.isSuccess == true) {
        response.data.map((item) => {
          item.key = item.userID;
          return item;
        })
        this.setState({ data: response.data })
      }
    })
  };

  deletefunction(id) {
    let confirmDeletion = window.confirm('Xóa bản ghi đã chọn?');
    if (confirmDeletion) {
      Service.deleteUser(id).then((response) => {
        if (response.isSuccess == true) {

          let { data } = this.state;
          data = data.filter((record) => { return record.userID != id })
          this.setState({ data });
        }
      })
    }
  };

  edit() {
    Service.updateUser({
      UserID: this.state.txtID,
      UserName: this.state.txtName
    })
      .then((response) => {
        if (response.isSuccess == true) {

        }
      });
  };

  onSelectChange = selectedRowKeys => {

    this.setState({ selectedRowKeys });
  };

  removeFromArray(original, remove) {
    return original.filter(value => !remove.includes(value));
  }

  handleDeleteAll(ids) {
    let temp = "";
    //add các key cần xóa vào chuỗi
    for (var i = 0; i < ids.length; i++) {
      if (i == ids.length - 1) {
        temp += "'" + ids[i] + "'";
      }
      else {
        temp += "'" + ids[i] + "',";
      }
    }

    let confirmDeletion = window.confirm('Xóa bản ghi đã chọn?');
    if (confirmDeletion) {
      Service.deleteAllUser(temp).then((response) => {
        if (response.isSuccess == true) {
          let { data, selectedRowKeys } = this.state;
          data = data.filter(function (item) {
            return !selectedRowKeys.includes(item.userID);
          })
          this.setState({ data })
        }
      })
    }
  };


  render() {
    const { selectedRowKeys } = this.state;
    const rowSelection = {
      selectedRowKeys,
      onChange: this.onSelectChange,
      hideDefaultSelections: true,
      selections: [
        Table.SELECTION_ALL,
        Table.SELECTION_INVERT,
      ],
    };
    var columns = [
      {
        title: 'ID',
        dataIndex: 'userID',
        key: 'userID',
      },
      {
        title: 'Name',
        dataIndex: 'userName',
        key: 'userName',
      },
      {
        title: 'Action',
        filters: [],
        onFilter: () => { },
        render: (record) => {
          return (
            <span>

              <Button type="primary" danger style={{ marginRight: '5px' }}
                onClick={() => { this.deletefunction(record.userID.toString()) }}>Delete</Button>

              <Button type="primary" onClick={(e) => { this.showEditModal(record) }} >Edit</Button>

            </span>

          )
        },
      },
    ];
    const { Option } = Select;
    return (
      <React.Fragment>
        <div style={{ display: 'flex', flexDirection: 'column', width: '100%', height: '100%' }}>

          <div style={{ display: 'flex', height: '120px', width: '100%', flexDirection: 'row' }}>

            <div style={{ flex: 3, flexDirection: 'row' }}>

              <span style={{ marginRight: '3px' }}>
                Từ Ngày
                        </span>

              <DatePicker style={{ marginRight: '5px' }} />

              <span style={{ marginRight: '3px' }}>
                Đến Ngày
                        </span>

              <DatePicker style={{ marginRight: '5px' }} />

              <p style={{ marginRight: '3px' }}>
                Mã Tài Sản
                        </p>

              <Input value={this.state.txtCode} onChange={(e) => {
                this.setState({ txtCode: e.target.value })
              }} placeholder="Basic usage" style={{ border: '1px', width: '40%' }} />

              <Button onClick={this.handleSearch} type="primary" style={{ marginLeft: '20px' }}>Tìm kiếm</Button>

              <Modal
                title={this.state.title}
                visible={this.state.visible}
                onOk={this.handleOk}
                onCancel={this.handleCancel}>

                <span style={{ marginRight: '3px' }}>
                  ID
                                </span>
                <Input value={this.state.txtID} onChange={(e) => { this.setState({ txtID: e.target.value }) }} style={{ border: '1px', width: '40%' }} />
                <span style={{ marginRight: '3px' }}>
                  Tên
                                </span>
                <Input value={this.state.txtName} onChange={(e) => { this.setState({ txtName: e.target.value }) }} style={{ border: '1px', width: '40%' }} />

                <Select>
                  <Option value="lucy">Lucy</Option>
                  <Option value="john">John</Option>
                  <Option value="michel">Michel</Option>
                </Select>
              </Modal>
            </div>

            <div style={{ flex: 1, display: 'flex', justifyContent: 'flex-end' }}>

              <Button type="primary" onClick={this.showNewModal} style={{ marginRight: '20px' }}>Thêm</Button>

              <Button type="primary" onClick={(e) => this.handleDeleteAll(this.state.selectedRowKeys)}>Xóa</Button>
            </div>

          </div>

          <div style={{ flex: 1 }}>

            <Table rowSelection={rowSelection} dataSource={this.state.data} columns={columns} />

          </div>

        </div>
      </React.Fragment>
    );
  }
};
