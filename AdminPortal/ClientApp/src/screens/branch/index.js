import * as React from 'react';
import { Table, Input, Modal, notification, Button, Popconfirm, Pagination } from 'antd';
import * as Service from '../../services/branchService';
import SimpleReactValidator from 'simple-react-validator';

const { Search } = Input;

export default class Branch extends React.Component {
    constructor(props) {
        super(props);
        //this.validator = new SimpleReactValidator();
        this.state = {
            txtTitleModal: '',
            txtSearch:'',
            visible: false,
            loading: false,
            txtBranchID: '',
            txtBranchName: '',
            txtBranchAdress: '',
            txtBranchPhone: '',
            dataSource: [],
            selectedRowKeys: [],
            totalRecords:0,

        };
    }
    componentWillMount() {
        this.validator = new SimpleReactValidator({
            messages:{
                required:':attribute chưa được nhập!',
                phone:'Điện thoại không đúng định dạng!'
            }
        });
      }
    showNewModal = () => {
        this.setState({
            txtTitleModal: 'Thêm chi nhánh mới',
            txtBranchID: '',
            txtBranchAdress: '',
            txtBranchName: '',
            txtBranchPhone: '',
            visible: true,
        });
    };

    showEditModal = (e) => {
        this.setState({
            visible: true,
            txtBranchID: e.branchID,
            txtBranchAdress: e.branchAddress,
            txtBranchName: e.branchName,
            txtBranchPhone: e.branchPhone,
            txtTitleModal: 'Chỉnh sửa'
        });
    };

    handleOk = e => {
        if (!this.validator.allValid()) {
            this.validator.showMessages();
            this.forceUpdate();
        }
        else
        {
            this.setState({ loading: true });
            if (this.state.txtTitleModal === 'Chỉnh sửa') {
                Service.editBranch({
                    "BranchID": this.state.txtBranchID,
                    "BranchName": this.state.txtBranchName,
                    "BranchAddress": this.state.txtBranchAdress,
                    "BranchPhone": this.state.txtBranchPhone,
                }).then((response) => {
                    if (response.isSuccess === true) {
                        notification.success({ message: 'Chỉnh sửa thành công' });
                        this.updateDataTable();
                    }
                    else {
                        notification.error({ message: response.err.msgString });
                    }
                    this.setState({
                        visible: false,
                        loading: false,
                    });
                }).catch(() => {
                    this.setState({
                        loading: false,
                    });
                });
            }
            else {
                Service.createBranch({
                    "BranchName": this.state.txtBranchName,
                    "BranchAddress": this.state.txtBranchAdress,
                    "BranchPhone": this.state.txtBranchPhone,
                }).then((response) => {
                    if (response.isSuccess === true) {
                        notification.success({ message: 'Tạo mới thành công' });
                        this.updateDataTable();
                    }
                    else {
                        notification.error({ message: response.err.msgString });
                    }
                    this.setState({
                        visible: false,
                        loading: false,
                    });
                }).catch(() => {
                    this.setState({
                        loading: false,
                    });
                });
            }
        }
    };

    handleCancel = e => {
        this.setState({
            visible: false,
        });
    };

    deleteOneRow(id) {
        Service.deleteBranch(id).then((response) => {
            if (response.isSuccess === true) {
                notification.success({ message: 'Xóa thành công' });
                this.updateDataTable();
            }
            else {
                notification.error({ message: response.err.msgString });
            }
        });
    }

    onSelectChange = selectedRowKeys => {

        this.setState({ selectedRowKeys });
    };

    deleteManyRow(ids) {
        Service.deleteManyBranch(ids.join()).then((response) => {
            if (response.isSuccess === true) {
                notification.success({ message: 'Xóa thành công' });
                this.updateDataTable();
            }
            else {
                notification.error({ message: response.err.msgString });
            }
        })
    }

    componentDidMount() {
        this.updateDataTable();
    };

    updateDataTable(page=1,size=10){
        if(this.state.txtSearch !== '' && this.state.txtSearch !== undefined)
        {
            this.searchBranch(this.state.txtSearch,page,size);
        }
        else
        {
            Service.GetAllBranch(page,size).then((response) => {
                if (response.isSuccess === true) {
                    response.data.map((item) => {
                        item.key = item.branchID;
                        return item;
                    })
                    this.setState({ 
                        dataSource: response.data,
                        totalRecords:response.totalRecords,
                    })
                }
            })
        }
    }

    searchBranch(query,page=1,size=10)
    {
        Service.getSearchBranch(query,page,size).then((response) => {
            if (response.isSuccess === true) {
                response.data.map((item) => {
                    item.key = item.branchID;
                    return item;
                })
                this.setState({ 
                    dataSource: response.data,
                    totalRecords:response.totalRecords,
                })
            }
        })
    }

    render() {
        let { loading } = this.state;
        const columns = [
            {
                title: 'Branch ID',
                dataIndex: 'branchID',
                key: 'branchID',
                defaultSortOrder: 'ascend',
                sorter: (a, b) => a.branchID - b.branchID,
            },
            {
                title: 'Branch Name',
                dataIndex: 'branchName',
                key: 'branchName',
                sorter: (a, b) => a.branchName.length - b.branchName.length,
            },
            {
                title: 'Branch Address',
                dataIndex: 'branchAddress',
                key: 'branchAddress',
                sorter: (a, b) => a.branchAddress.length - b.branchAddress.length,
            },
            {
                title: 'Branch Phone',
                dataIndex: 'branchPhone',
                key: 'branchPhone',
                sorter: (a, b) => a.branchPhone.length - b.branchPhone.length,
            },
            {
                title: 'Action',
                render: (record) => {
                    return (
                        <span>

                            <Button type="primary" danger style={{ marginRight: '5px' }}>
                                <Popconfirm
                                    title="Bạn có chắc muốn xóa chi nhánh này?"
                                    onConfirm={() => this.deleteOneRow(record.branchID)}
                                    okText="Đồng ý" cancelText="Hủy">
                                    Delete
                            </Popconfirm>
                            </Button>

                            <Button type="primary" onClick={(e) => { this.showEditModal(record) }} >Edit</Button>

                        </span>

                    )
                },
            },
        ];
        const { selectedRowKeys } = this.state;
        const rowSelection = {
            selectedRowKeys,
            onChange: this.onSelectChange,
            hideDefaultSelections: true,
            selections: [
                Table.SELECTION_ALL,
                Table.SELECTION_INVERT,
                {
                    key: 'odd',
                    text: 'Select Odd Row',
                    onSelect: changableRowKeys => {
                        let newSelectedRowKeys = [];
                        newSelectedRowKeys = changableRowKeys.filter((key, index) => {
                            if (index % 2 !== 0) {
                                return false;
                            }
                            return true;
                        });
                        this.setState({ selectedRowKeys: newSelectedRowKeys });
                    },
                },
                {
                    key: 'even',
                    text: 'Select Even Row',
                    onSelect: changableRowKeys => {
                        let newSelectedRowKeys = [];
                        newSelectedRowKeys = changableRowKeys.filter((key, index) => {
                            if (index % 2 !== 0) {
                                return true;
                            }
                            return false;
                        });
                        this.setState({ selectedRowKeys: newSelectedRowKeys });
                    },
                },
            ],
        };
        return (
            <React.Fragment>
                <div className="d-flex row no-gutters mb-2">
                    <div className="col-md-4">
                        <Search
                            placeholder="Nhập từ khóa tìm kiếm"
                            enterButton="Tìm kiếm"
                            onChange={value => this.setState({txtSearch:value})}
                            onSearch={value => this.searchBranch(value)}
                            />
                    </div>
                    <div className="col text-right align-self-end">
                        <button onClick={this.showNewModal} className="btn btn-success btn-lg mr-2">Tạo mới</button>
                        <button className="btn btn-danger btn-lg">
                            <Popconfirm
                                title="Bạn có chắc muốn xóa chi nhánh đã chọn?"
                                onConfirm={() => this.deleteManyRow(this.state.selectedRowKeys)}
                                okText="Đồng ý" cancelText="Hủy">
                                Xóa tất cả
                            </Popconfirm></button>
                    </div>
                </div>
                <Table pagination={false} rowSelection={rowSelection} dataSource={this.state.dataSource} columns={columns} />
                <div className="text-center my-2">
                    <Pagination
                        total={this.state.totalRecords}
                        showTotal={total => `Tổng: ${total} chi nhánh`}
                        onChange={(page,size)=>this.updateDataTable(page,size)}
                        onShowSizeChange={(page,size)=>this.updateDataTable(1,size)}
                        showSizeChanger={true}
                        />
                </div>
                {this.state.visible === true &&
                    <Modal
                        title={this.state.txtTitleModal}
                        visible={this.state.visible}
                        onOk={this.handleOk}
                        onCancel={this.handleCancel}
                        footer={[
                            <Button key="back" onClick={this.handleCancel}>
                                Hủy
                        </Button>,
                            <Button key="submit" type="primary" loading={loading} onClick={this.handleOk}>
                                Đồng ý
                        </Button>,
                        ]}
                    >
                        <div className="form-group">
                            <label>Tên chi nhánh:</label>
                            <Input value={this.state.txtBranchName} onChange={(e) => { this.setState({ txtBranchName: e.target.value }) }} placeholder="Nhập tên chi nhánh" />
                            {this.validator.message('Tên chi nhánh', this.state.txtBranchName, 'required')}
                        </div>
                        <div className="form-group">
                            <label>Địa chỉ:</label>
                            <Input value={this.state.txtBranchAdress} onChange={(e) => { this.setState({ txtBranchAdress: e.target.value }) }} placeholder="Nhập địa chỉ chi nhánh" />
                            {this.validator.message('Địa chỉ', this.state.txtBranchAdress, 'required')}
                        </div>
                        <div className="form-group">
                            <label>Số điện thoại:</label>
                            <Input maxLength={11} value={this.state.txtBranchPhone} onChange={(e) => { this.setState({ txtBranchPhone: e.target.value }) }} placeholder="Nhập điện thoại chi nhánh" />
                            {this.validator.message('Điện thoại', this.state.txtBranchPhone, 'required|phone')}
                        </div>
                    </Modal>
                }
            </React.Fragment>
        );
    }
};
