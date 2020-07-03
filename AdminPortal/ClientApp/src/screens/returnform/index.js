import * as React from 'react';
import { Table, Input, Select, Button } from 'antd';
import * as Actions from '../../libs/actions';
import moment from 'moment';
import * as AnalyzerService from '../../services/analyzerService';
import * as Service from '../../services/departmentService';
import * as ConfirmService from '../../services/confirmService'
import * as EmployeeService from '../../services/employeeService'
import Container from 'reactstrap/lib/Container';

const { Option } = Select;

export default class ReturnForm extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            dataD: [],
            data: [],
            dataE: [],
            selectedRowKeys: [],
            totalRecords: 0,
            pageSize: 10,
            currentPage: 0,
            txtName: '',
            item: {
                AnalyzerCode: '',
                AnalyzerName: '',
                EmployeeID: 0,
                Name: '',
                departmentID: 0,
                DepartmentName: '',
            },
        };
    }
    componentDidMount() {
        Service.GetAllDepartment(10, 3).then((response) => {
            if (response.isSuccess == true) {

                this.setState({ dataD: response.data })
            }
        });
        EmployeeService.GetAllEmployee().then((objRespone) => {
      
                this.setState({ dataE: objRespone.data })
            }
        );
        this.getData(this.state.currentPage, this.state.pageSize);
    }
    getData = () => {
        let { pageSize, currentPage, searchState } = this.state
        AnalyzerService.GetAllAnalyzer(pageSize, currentPage)
            .then(objRespone => {

                objRespone.data.map((item) => {
                    item.key = item.analyzerCode;
                    return item;
                })
                if (objRespone.isSuccess === true) {
                    for (let item of objRespone.data) {
                        if (item.expirationDate != null) {
                            item.expirationDate = moment(new Date(item.expirationDate)).format('DD-MM-YYYY');
                        }
                        else item.expirationDate = '';
                    }
                    this.setState({ data: objRespone.data, totalRecords: objRespone.totalRecords });
          
                }
                else {
                    Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                }
            }
            )
    };
    handleConfirm = () => {
        let { item, data, dataD, selectedRowKeys } = this.state;
        item.Name = this.state.txtName;
        ConfirmService.createConfirm(item)


    }
    handelChangePage = (page) => {
        this.setState({ currentPage: page.current - 1 }, () => {
            this.reSearch();
        })
    }
    reSearch = () => {
        this.getData(this.state.searchState)
    }
    onChangeD = (value) => {

        this.state.item.departmentID = value;

    }
    onChangeE = (value) => {

        this.state.item.EmployeeID = value;

    }
    onBlur=() => {
        console.log('blur');
    }

    onFocus=()=>  {
        console.log('focus');
    }

    onSearch=(val) => {
        console.log('search:', val);
    }
    onSelectChange = (selectedRowKeys, record) => {

        if (selectedRowKeys.length >= 1) {
            this.state.item.AnalyzerCode = selectedRowKeys[0]
        }

        this.setState({ selectedRowKeys });
    };
    render= () => {
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
        const columns = [
            {
                title: 'ID',
                dataIndex: 'analyzerID',
                key: 'analyzerID'
            },
            {
                title: 'Mã tài sản',
                dataIndex: 'analyzerCode',
                key: 'analyzerCode'
            },
            {
                title: 'Tên tài sản',
                dataIndex: 'analyzerName',
                key: 'analyzerName'
            },
            {
                title: 'Nhóm tài sản',
                dataIndex: 'analyzerGroupName',
                key: 'analyzerGroupName'
            },
            {
                title: 'Mã serial',
                dataIndex: 'serial',
                key: 'serial'
            },
            {
                title: 'Ngày hết hạn',
                dataIndex: 'expirationDate',
                key: 'expirationDate'
            },
            {
                title: 'Mô tả',
                dataIndex: 'description',
                key: 'description'
            },
        ];
        const listItems = this.state.dataD.map((item, index) =>
            <Option value={item.departmentID}>
                {item.departmentName}
            </Option>
        );
        const listItemsE = this.state.dataE.map((item, index) =>
            <Option value={item.employeeID}>
                {item.name}
            </Option>
        );
        return (
            <React.Fragment>
                <React.Fragment>
                    <div style={{ display: 'flex', flexDirection: 'column', width: '100%', height: '50px' }}>
                        <div style={{ display: 'flex', flexDirection: 'row', width: '100%', height: '100%', paddingBottom: '5px' }}>
                            <Select showSearch
                                style={{ width: 250, marginRight: '20px' }}
                                placeholder="Chọn phòng ban"
                                optionFilterProp="children"
                                onChange={this.onChangeD}
                                onFocus={this.onFocus}
                                onBlur={this.onBlur}
                                onSearch={this.onSearch}
                                filterOption={(input, option) =>
                                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0}>
                                {listItems}
                            </Select>
                            <Select showSearch
                                style={{ width: 250 }}
                                placeholder="Chọn nhân viên trả"
                                optionFilterProp="children"
                                onChange={this.onChangeE}
                                onFocus={this.onFocus}
                                onBlur={this.onBlur}
                                onSearch={this.onSearch}
                                filterOption={(input, option) =>
                                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0}>
                                {listItemsE}
                            </Select>

                            <div style={{ flex: 1, display: 'flex', justifyContent: 'flex-end' }}>

                                <Button type="primary" onClick={this.handleConfirm}>Xác nhận</Button>

                            </div>
                        </div>
                        <div style={{ display: 'flex', flexDirection: 'column', width: '100%', height: '100%' }}>
                            <Table rowSelection={rowSelection}
                                columns={columns} dataSource={this.state.data} bordered={true}
                                pagination={{
                                    total: this.state.totalRecords, defaultPageSize: this.state.pageSize, showSizeChanger: true,
                                    pageSizeOptions: ['10', '20', '50', '100']
                                }
                                }
                                onChange={(page) => { this.handelChangePage(page) }}
                            />
                        </div>
                    </div>

                </React.Fragment>
            </React.Fragment>
        );
    }
}