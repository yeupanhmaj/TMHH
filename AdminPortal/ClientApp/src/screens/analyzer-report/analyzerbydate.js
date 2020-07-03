import * as React from 'react';
import moment from 'moment';
//import { Container } from 'reactstrap';

import { Table, Tag, Space, DatePicker, Button, Select, Modal, Input } from 'antd';
import * as AnalyzerService from '../../services/analyzerService';
import * as DepartmentService from '../../services/departmentService';
import * as CustomerService from '../../services/customerService';
import { DownOutlined, PlusOutlined, SearchOutlined, FormOutlined, DeleteOutlined } from '@ant-design/icons';
import SimpleReactValidator from 'simple-react-validator';
import viVN from 'antd/es/date-picker/locale/vi_VN';

const dateFormat = 'YYYY/MM/DD';
const monthFormat = 'YYYY/MM';
const { RangePicker } = DatePicker;
const dateFormatList = ['DD/MM/YYYY'];

const { Option } = Select;

export default class AnalyzerByDate extends React.Component {
    constructor(props) {

        super(props);
        this.validatorAna = new SimpleReactValidator({
            messages: {
              required: 'Vui lòng nhập thông tin!'
            }
          });
        this.state = {
            data: [],
            totalRecords: 0,
            pageSize: 10,
            currentPage: 0,
            dataDepartment: [],
            dataCustomer: [],
            searchState: {
                analyzerCode: '',
                dateFrom: moment().startOf('month'),
                dateTo: moment(new Date()),
              },
            departmentID: '',
            customerID: '',
            visibleCreate: false,
            localItem: {},
        };
    }
    componentDidMount() {
        this.reSearch();
    };


    getColumnTable() {
        const columns = [
            {
                title: 'Mã phần mềm',
                dataIndex: 'analyzerCode',
                key: 'analyzerCode',
            },
            {
                title: 'Mã kế toán',
                dataIndex: 'analyzerAccountantCode',
                key: 'analyzerAccountantCode',
            },
            {
                title: 'Tên tài sản',
                dataIndex: 'analyzerName',
                key: 'analyzerName',
            },
            {
                title: 'Phân loại',
                dataIndex: 'analyzerType',
                key: 'analyzerType',
            },
            {
                title: 'Số lượng',
                dataIndex: 'amount',
                key: 'amount',
            },
            {
                title: 'Đơn giá',
                dataIndex: 'itemPrice',
                key: 'itemPrice',
            },
            {
                title: 'Nguyên giá',
                dataIndex: 'totalPrice',
                key: 'totalPrice',
            },
            {
                title: 'Nơi sử dụng',
                dataIndex: 'departmentName',
                key: 'departmentName',
            },
            {
                title: 'Mã hợp đồng',
                dataIndex: 'contractCode',
                key: 'contractCode',
            },
            {
                title: 'Tên công ty',
                dataIndex: 'customerName',
                key: 'customerName',
            },
            {
                title: 'Người thực hiện',
                dataIndex: 'userI',
                key: 'userI',
            },
            {
                title: 'Năm sử dụng',
                dataIndex: 'deliveryReceiptDate',
                key: 'deliveryReceiptDate',

            },
            {
                title: 'Diễn giải',
                dataIndex: 'description',
                key: 'description',
            },
            {
                title: '',
                key: 'action',
                render: (text, record) => (
                  
                  <Space size="middle">
                    <Button type="button" title="Chỉnh sửa" type="primary" icon={<FormOutlined />}>
                    </Button>
                  </Space>
                ),
                width: '4%',
              }
        ];
        
        return columns
    
      }

      getAnalyzerTypeArr() {
        const arr = [
            {
                value: '1',
                label: 'CC-DV',
            },
            {
                value: '2',
                label: 'CC-CQ',
            },
            {
                value: '3',
                label: 'TS-DV',
            },
            {
                value: '4',
                label: 'TS-CQ',
            },
        ];
        
        return arr
    
      }

    getDataSelect() {
        DepartmentService.GetAllDepartment(10, 0).then((response) => {
            if (response.isSuccess == true) {
                let {dataDepartment} = this.state
                dataDepartment = [{ label: "Tất cả", value: '' }];
                for (let record of response.data) {
                    let item = { label: record.departmentName, value: record.departmentID };
                    dataDepartment.push(item);
                }
                this.setState({ dataDepartment});
            }
        });
        CustomerService.GetAllCustomer(10, 0).then((response) => {
            if (response.isSuccess == true) {
                this.setState({ dataCustomer: response.data });
            }
            if (response.isSuccess == true) {
                let {dataCustomer} = this.state
                dataCustomer = [{ label: "Tất cả", value: '' }];
                for (let record of response.data) {
                    let item = { label: record.customerName, value: record.customerID };
                    dataCustomer.push(item);
                }
                this.setState({ dataCustomer});
            }
        });
    }
    componentWillMount() {
        
    };

    componentDidMount() {
        this.getDataSelect();
        this.reSearch();
    };

    handelNewSearch() {

        this.setState({ currentPage: 0 }, () => {
          this.reSearch();
        });
      }

      getLabelString = function (status, array) {
        for (let item of array) {
           if(item != undefined && item.value!= undefined  && status != undefined)
           if (item.value.toString() == status.toString()) return item.label
        }
        return ''
     }


    reSearch() {
        let { searchState, departmentID, customerID, pageSize, currentPage } = this.state;
        AnalyzerService.GetAllAnalyzerWithCondition(
            searchState.analyzerCode,
            departmentID,
            customerID,
            searchState.dateFrom.format('YYYY/MM/DD'),
            searchState.dateTo.format('YYYY/MM/DD'),
            pageSize,
            currentPage
        ).then((response) => {
            if (response.isSuccess == true) {
                for (let item of response.data) {
                    item.deliveryReceiptDate = moment(new Date(item.dateIn)).format('YYYY');
                    let analyzerTypeArr = this.getAnalyzerTypeArr();
                    item.analyzerTypeName  = this.getLabelString(item.analyzerType, analyzerTypeArr);
                }
                this.setState({ data: response.data, totalRecords: response.totalRecords})
                
            }
        })
    }
    onchangeSearchBinding(data, txt) {
        this.setState({ fromDate: moment(txt[0]), toDate: moment(txt[1]) });
    }
    onChangeDepartment = (value) => {
        this.setState(
            {
                departmentID: value
            }
        )
       
    }
    onChangeCustomer = (value) => {
        this.setState(
            {
                customerID: value
            }
        )
    }

    onChangeDateFrom(value) {

        let { searchState } = this.state
        searchState.dateFrom = moment(new Date(value)).format('YYYY-MM-DD');
        this.setState({ searchState });
      }
    
      onChangeDateTo(value) {
        let { searchState } = this.state
        searchState.dateTo = moment(new Date(value)).format('YYYY-MM-DD');
        this.setState({ searchState });
      }
    

    handelChangePage(page) {
        this.setState({ currentPage: page.current - 1 , pageSize : page.pageSize}, () => {
          this.reSearch();
        })
      }

      onChangeSearchCode(value) {
          let {searchState} = this.state;
          searchState.analyzerCode = value
        this.setState({ 
            searchState
        })
      }

      //View iteam , update

      handleOkCreate = e => {
        if (this.validatorAna.allValid()) {
          let { localItem } = this.state;
          localItem.analyzerGroupID = +localItem.analyzerGroupID;
          AnalyzerService.updateAnalyzer(localItem).then((objRespone) => {
            if (objRespone.isSuccess) {
                this.handelNewSearch();
                this.setState({
                    visibleCreate: false,
                  });
            }
          })
         
        }
        else {
          this.validatorAna.showMessages();
          this.forceUpdate();
        }
    
      };

      handleCancelCreate = e => {
        let {  localItem } = this.state;
        localItem.analyzerCode = '';
        localItem.analyzerAccountantCode = '';
        localItem.analyzerName = '';
        localItem.description = '';
        this.setState({
          visibleCreate: false,
          localItem,
        });
      };

      onNewAnaChange = (event, prop) => {

        let { localItem } = this.state;
        localItem[prop] = event.target.value
        this.setState({
          localItem
        });
      };
    
      onNewAnaSelectChange = (event, prop) => {
        let { localItem } = this.state;
        localItem[prop] = event
        this.setState({
          localItem
        });
      };

      showModalCreate = (value) => {
        AnalyzerService.GetAnalyzerById(
            +value
        ).then((response) => {
            if (response.isSuccess == true) {
                let {localItem} = this.state;
                localItem = response.item;
                console.log(localItem)
                this.setState({
                    visibleCreate: true,
                    localItem
                  });
            }
        })
       
      };

    render() {
        let { localItem } = this.state;
        const listItemsDepartment = this.state.dataDepartment.map((item, index) =>
            <Option value={item.value}>
                {item.label}
            </Option>
        );

        const listItemsCustomer = this.state.dataCustomer.map((item, index) =>
        <Option value={item.value}>
            {item.label}
        </Option>
        );
        
        return (
            <React.Fragment>
                <div style={{ display: 'flex', flexDirection: 'row', height: '50px', width: '1500px'  }}>
                    <div style={{ flex: 1 }}>
                    <DatePicker style={{ marginLeft: '5px', marginRight: '5px' }}
                        defaultValue={this.state.searchState.dateFrom}
                        format={dateFormatList}
                        locale={viVN}
                        onchange={(value) => { this.onChangeDateFrom(value) }}
                    />
                    </div>
                    <div style={{ flex: 1 }}>
                    <DatePicker style={{  marginLeft: '5px', marginRight: '5px' }}
                        defaultValue={this.state.searchState.dateTo}
                        format={dateFormatList}
                        locale={viVN}
                        onchange={(value) => { this.onChangeDateTo(value) }}
                    />
                    </div>
                    <div style={{ flex: 2 }}>
                    <Select showSearch
                        style={{ width: 325, marginRight: '20px' }}
                        placeholder="Chọn phòng ban sử dụng"
                        optionFilterProp="children"
                        onChange={(value) => {this.onChangeDepartment(value)}}
                        onFocus={this.onFocus}
                        onBlur={this.onBlur}
                        onSearch={this.onSearch}
                        filterOption={(input, option) =>
                            option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0}>
                        {listItemsDepartment}
                    </Select>
                    </div>
                    <div style={{ flex: 2 }}>
                    <Select showSearch
                        style={{ width: 325, marginRight: '20px' }}
                        placeholder="Chọn công ty"
                        optionFilterProp="children"
                        onChange={(value) => {this.onChangeCustomer(value)}}
                        onFocus={this.onFocus}
                        onBlur={this.onBlur}
                        onSearch={this.onSearch}
                        filterOption={(input, option) =>
                            option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0}>
                        {listItemsCustomer}
                    </Select>
                    </div>
                    <div style={{ flex: 2 }}>
                    <Input 
                        style={{width: 250, marginLeft: '5px', marginRight: '5px', border: '1px solid blue !important' }} 
                        onChange={(e) => { this.onChangeSearchCode(e.target.value ) }} 
                        placeholder="Mã tài sản" />
                    </div>
                    <div style={{ flex: 1 }}>
                        <Button type="primary" style={{ width: 100, marginRight: '20px' }} onClick={this.handelNewSearch.bind(this)}>Lọc</Button>
                    </div>

                </div>
                <Modal
                width='1000px'
                title="Chỉnh sửa tài sản"
                visible={this.state.visibleCreate}
                onOk={this.handleOkCreate}
                onCancel={this.handleCancelCreate}
              >
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', height: '60px' }}>
                  <div style={{ flex: 1, marginRight: '10px' }}>
                    <p style={{ marginBottom: '2px' }}>
                        <Input style={{ border: '1px solid blue !important', }} 
                        placeholder="Mã tài sản" 
                        value={localItem.analyzerCode}
                        disabled = {true} 
                        onChange={(event) => { this.onNewAnaChange(event, 'analyzerCode') }} /></p>
                    {this.validatorAna.message('Mã tài sản', localItem.analyzerCode, 'required')}
                  </div>
                  <div style={{ flex: 1, marginRight: '10px' }}>
                    <p style={{ marginBottom: '2px' }}>
                        <Input style={{ border: '1px solid blue !important', }} 
                        placeholder="Mã kế toán" 
                        value={localItem.AnalyzerAccountantCode} 
                        onChange={(event) => { this.onNewAnaChange(event, 'AnalyzerAccountantCode') }} /></p>
                    {this.validatorAna.message('Mã tài sản', localItem.AnalyzerAccountantCode, 'required')}
                  </div>
                  <div style={{ flex: 3, marginRight: '10px' }}>
                    <p style={{ marginBottom: '2px' }}>
                        <Input style={{ border: '1px solid blue !important' }} 
                        placeholder="Tên tài sản" 
                        value={localItem.analyzerName} 
                        onChange={(event) => { this.onNewAnaChange(event, 'analyzerName') }} /></p>
                    {this.validatorAna.message('Tên tài sản', localItem.analyzerName, 'required')}
                  </div>
                </div>

              
                <p><Input style={{ border: '1px solid blue !important', height: '50px' }} placeholder="Mô tả" value={localItem.description} onChange={(event) => { this.onNewAnaChange(event, 'description') }} /></p>
                
              

              </Modal>
                <Table 
                    dataSource={this.state.data} 
                    columns={this.getColumnTable()}
                    bordered={true}
                    pagination={{
                        total: this.state.totalRecords, defaultPageSize: this.state.pageSize, showSizeChanger: true,
                        pageSizeOptions: ['10', '20', '50', '100']
                      }}
                      onChange={(page) => { this.handelChangePage(page) }}
                 />
            </React.Fragment>

        );
    };
};