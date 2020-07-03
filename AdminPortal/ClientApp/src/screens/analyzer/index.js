import * as React from 'react';
import { Button, DatePicker, Input, Table, Modal, Select, Divider, notification, Pagination, Space, Popconfirm } from 'antd';
import moment from 'moment';
import MultiSelect from "@kenshooui/react-multi-select";
import * as AnalyzerService from '../../services/analyzerService';
import * as AnalyzerGroupService from '../../services/analyzerGroupService';
import { DownOutlined, PlusOutlined, SearchOutlined, FormOutlined,DeleteOutlined  } from '@ant-design/icons';
import * as Actions from '../../libs/actions';
import SimpleReactValidator from 'simple-react-validator';
import InputCustom from '../../commons/controls/input';
import * as QuoteService from '../../services/quoteService';
import viVN from 'antd/es/date-picker/locale/vi_VN';
import './main.css';


const dateFormatList = ['DD/MM/YYYY'];



const { Option } = Select;


export default class User extends React.Component {
  constructor(props) {
    super(props);
    this.validatorAnaGroup = new SimpleReactValidator({
      messages: {
        required: 'Vui lòng nhập thông tin!'
      }
    });
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
      visibleCreate: false,
      listAnalyzerGroup: [],
      newAnalyzerGroup: {
        AnalyzerGroupCode: '',
        AnalyzerGroupName: '',
      },
      localItem: {},
      searchText: '',
      quoteList: [],
      selectedQuoteList: [],
      searchState: {
        analyzerCode: '',
        dateFrom: '',
        dateTo: '',
      },
      action: 0, // 1: Create , 2: View , 3: Edit
    };
  }

  //Get data

  getColumnTable() {
    const columns = [
      {
        title: 'Mã tài sản',
        dataIndex: 'analyzerCode',
      },
      {
        title: 'Mã kế toán',
        dataIndex: 'analyzerCode',
      },
      {
        title: 'Tên tài sản',
        dataIndex: 'analyzerName',
      },
      {
        title: 'Tên tài sản',
        dataIndex: 'analyzerName',
      },
      {
        title: 'Tên tài sản',
        dataIndex: 'analyzerName',
      },

      {
        title: 'Mô tả',
        dataIndex: 'description',
      },
      {
        title: '',
        key: 'action',
        render: (text, record) => (
          
          <Space size="middle">
            {/* <Button type="button" title="Chỉnh sửa" type="primary" icon={<FormOutlined onClick={this.showModalCreate()} />}>

            </Button> */}

            <Popconfirm  title={`Bạn muốn xóa tài sản mã: ${record.analyzerCode} ?`} onConfirm={() => this.handleDelete(record.analyzerID, record.analyzerCode)}>
              <Button type="button" title="Xóa" type="danger" icon={<DeleteOutlined />}>
              </Button>
            </Popconfirm>
          </Space>
        ),
        width: '4%',
      }
    ];
    
    return columns

  }

  getData() {
    let { pageSize, currentPage, searchState } = this.state
    AnalyzerService.GetAllAnalyzer(pageSize, currentPage)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          for (let item of objRespone.data) {
            if (item.expirationDate != null) {
              item.expirationDate = moment(new Date(item.expirationDate)).format('DD-MM-YYYY');
            }
            else item.expirationDate = '';
          }
          this.setState({ data: objRespone.data, totalRecords: objRespone.totalRecords });
          this.handelNewSearchItemCreate()
        }
        else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      }
      )
  };

  getAnalzerGroup() {
    AnalyzerGroupService.GetAllAnalyzerGroup(10, 0)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          this.setState({ listAnalyzerGroup: objRespone.data });
        }
        else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      }
      )
  };

  handelChangePage(page) {

    this.setState({ currentPage: page.current - 1 }, () => {
      this.reSearch();
    })
  }

  reSearch() {
    this.getData(this.state.searchState)
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

  // Create new group

  onNewAnaGroupChange = (event, prop) => {
    let { newAnalyzerGroup } = this.state;
    newAnalyzerGroup[prop] = event.target.value
    this.setState({
      newAnalyzerGroup
    });
  };

  addAnaCategoryGroup = () => {
    if (this.validatorAnaGroup.allValid()) {
      AnalyzerGroupService.createAnalyzerGroup(this.state.newAnalyzerGroup).then((objRespone) => {
        if (objRespone.isSuccess == true) {
          this.getAnalzerGroup();
          this.createNewGroupNotification(this.state.newAnalyzerGroup.AnalyzerGroupName, 1)
          let { newAnalyzerGroup } = this.state
          newAnalyzerGroup.AnalyzerGroupCode = '';
          newAnalyzerGroup.AnalyzerGroupName = '';
          this.setState({
            newAnalyzerGroup
          });
        } else {
          Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
        }
      })
    } else {
      this.validatorAnaGroup.showMessages();
      this.forceUpdate();
    }


  };

  createNewGroupNotification = (name, type) => {
    let messageReturn = ""
    switch(type) {
      case 1:
        messageReturn = 'Đã thêm nhóm tài sản: ' + name 
        break;
      case 2:
        messageReturn = 'Đã thêm tài sản: ' + name
        break;
      case 3:
        messageReturn = 'Đã xóa tài sản: ' + name
        break;
      default:
        // code block
    }
    notification.open({
      message: messageReturn,
      description:
        '',
      duration: 2
    });
  };

  // end create anagroup

  //Create new analyzer

  showModalCreate = () => {
    this.setState({
      visibleCreate: true,
      //action: userAction
    });
  };

  handleOkCreate = e => {
    if (this.validatorAna.allValid()) {
      let { localItem } = this.state;
      localItem.analyzerGroupID = +localItem.analyzerGroupID;
      AnalyzerService.createAnalyzer(localItem).then((objRespone) => {
        if (objRespone.isSuccess) {
          this.getData(this.state.currentPage, this.state.pageSize);
          this.createNewGroupNotification(this.state.localItem.analyzerName, 2)
          localItem.analyzerCode = '';
          localItem.analyzerName = '';
          localItem.expirationDate = '';
          localItem.description = '';
          localItem.analyzerGroupID = '';
          localItem.serial = '';
          localItem.analyzerAccountantCode = '';

          localItem.expirationDate = moment(new Date(), dateFormatList);
          this.handelNewSearchItemCreate()
        }
      })
      this.setState({
        localItem,
        visibleCreate: false,
        selectedQuoteList: [],
      });
    }
    else {
      this.validatorAna.showMessages();
      this.forceUpdate();
    }

  };

  handleCancelCreate = e => {
    let { newAnalyzerGroup, localItem } = this.state;
    localItem.analyzerCode = '';
    localItem.analyzerName = '';
    localItem.description = '';
    localItem.analyzerGroupID = '';
    localItem.analyzerAccountantCode = '';
    localItem.expirationDate = moment(new Date(), dateFormatList);

    newAnalyzerGroup.AnalyzerGroupCode = '';
    newAnalyzerGroup.AnalyzerGroupName = '';
    this.setState({
      visibleCreate: false,
      newAnalyzerGroup,
      localItem,
      selectedQuoteList: [],
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

  handleChangeQuote(selectedItems) {
    let { localItem } = this.state;
    if (selectedItems.length > 0) {
      localItem.deliveryReceiptID = selectedItems[0].deliveryReceiptID;
      localItem.quoteItemID = selectedItems[0].quoteItemID;
      localItem.analyzerName = selectedItems[0].itemNames;
      this.setState({ localItem });
    }
    else {
      localItem.quoteItemID = "";
      localItem.deliveryReceiptID = "";
      localItem.analyzerName = "";
      this.setState({ localItem });
    }
  }

  handelNewSearchItemCreate() {
    QuoteService.GetQuoteItemWithCondition(
      this.state.searchText,
      this.state.isHasAudit
    )
      .then(objRespone => {
        if (objRespone.isSuccess === true) {

          let quoteList = [];

          if (objRespone.data && objRespone.data.length > 0) {
            for (let group of objRespone.data) {
              let temp = {};
              temp.id = group.quoteItemID;
              temp.deliveryReceiptID = group.deliveryReceiptID;
              temp.quoteItemID = group.quoteItemID;
              temp.itemNames = group.itemNames;
              temp.label = group.proposalCodes + " : số lượng " + group.avaAmount + " : " + group.itemNames ;


              quoteList.push(temp)
            }
            this.setState({
              quoteList: quoteList,
            })
          } else {
            this.setState({
              quoteList: []
            })
          }
        } else {
          Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
        }
      }).catch(err => {
      })
  }

  changeDateExpired(value) {
    let { localItem } = this.state;
    localItem.expirationDate = moment(new Date(value)).format('YYYY-MM-DD');
    this.setState({ localItem });
  }

  changeSearchText(value) {
    this.setState({ searchText: value });
  }

  // end create ana

  //search

  changeAnalyzerCodeSearch(value) {
    this.setState({ searchState: value });
  }

  handelNewSearch() {

    this.setState({ currentPage: 0 }, () => {
      this.getData();
    });
  }

  //Delete item 

  handleDelete = (key, name) => {
    AnalyzerService.deleteAnalyzer(key).then(
      this.getData(),
      this.createNewGroupNotification(name, 3)
    )
  };

  // end search

  componentWillMount() {
  
  }

  componentDidMount() {
    let { localItem, searchState } = this.state;
    searchState.dateFrom = moment(new Date(moment().startOf('month')), dateFormatList);
    searchState.dateTo = moment(new Date(), dateFormatList);
    localItem.expirationDate = moment(new Date(), dateFormatList);
    this.setState(localItem);
    this.handelNewSearchItemCreate();
    this.getData(this.state.currentPage, this.state.pageSize); 
    this.getAnalzerGroup();
  };



  render() {
    let { newAnalyzerGroup, localItem } = this.state;
    return (
      <React.Fragment>
        <div style={{ display: 'flex', flexDirection: 'column', width: '100%', height: '100%' }}>
          {/* <div style={{ display: 'flex', flexDirection: 'row', height: '150px' }}>
            <div style={{ flex: 1 }}>
              <DatePicker style={{ width: '15%', marginLeft: '5px', marginRight: '5px' }}
                // value = {this.state.searchState.dateFrom !== "" ? moment(this.state.searchState.dateFrom) : moment(new Date(moment().startOf('month')), dateFormatList)}
                defaultValue={moment(new Date(moment().startOf('month')), dateFormatList)}
                format={dateFormatList}
                locale={viVN}
                onchange={(value) => { this.onChangeDateFrom(value) }}
              />
              <DatePicker style={{ width: '15%', marginLeft: '5px', marginRight: '5px' }}
                // value = {this.state.searchState.dateTo !== "" ? moment(this.state.searchState.dateTo) : moment(new Date(moment().startOf('month')), dateFormatList)}
                defaultValue={moment(new Date(), dateFormatList)}
                format={dateFormatList}
                locale={viVN}
                onchange={(value) => { this.onChangeDateTo(value) }}
              />
              <Input style={{ width: '30%', marginLeft: '5px', marginRight: '5px', border: '1px solid blue !important' }} onChange={(e) => { this.setState({ searchText: e.target.value }) }} placeholder="Mã tài sản" />
              <Button style={{ marginLeft: '5px', marginRight: '5px' }} type="primary" icon={<SearchOutlined />} onClick={() => { this.handelNewSearch() }} >Tìm kiếm</Button>
            </div>
              <div style={{ flex: 1, display: 'flex', justifyContent: 'flex-end' }}>
              <Button type="primary" style={{ width: '15%', height: '40px', marginLeft: '5px', marginRight: '5px' }} onClick={this.showModalCreate()} >Thêm</Button>
              <Modal
                width='1000px'
                title="Thêm mới tài sản"
                visible={this.state.visibleCreate}
                onOk={this.handleOkCreate}
                onCancel={this.handleCancelCreate}
              >
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', height: '60px' }}>
                  <div style={{ flex: 2, marginRight: '10px' }}>
                    <p style={{ marginBottom: '2px' }}><Input style={{ border: '1px solid blue !important', }} placeholder="Mã tài sản" value={localItem.analyzerCode} onChange={(event) => { this.onNewAnaChange(event, 'analyzerCode') }} /></p>
                    {this.validatorAna.message('Mã tài sản', localItem.analyzerCode, 'required')}
                  </div>
                  <div style={{ flex: 3, marginRight: '10px' }}>
                    <p style={{ marginBottom: '2px' }}><Input style={{ border: '1px solid blue !important' }} placeholder="Tên tài sản" value={localItem.analyzerName} onChange={(event) => { this.onNewAnaChange(event, 'analyzerName') }} /></p>
                    {this.validatorAna.message('Tên tài sản', localItem.analyzerName, 'required')}
                  </div>

                </div>

                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', height: '60px' }}>
                  <div style={{ flex: 3, marginRight: '10px' }}>
                    <p style={{ marginBottom: '2px' }}><Input style={{ border: '1px solid blue !important' }} placeholder="Mã serial" value={localItem.serial} onChange={(event) => { this.onNewAnaChange(event, 'serial') }} /></p>
                  </div>
                  <div style={{ flex: 3, marginRight: '10px' }}>
                    <p style={{ marginBottom: '2px' }}>
                      <Select
                        style={{ width: '100%' }}
                        showSearch
                        optionFilterProp="children"
                        filterOption={(input, option) =>
                          option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                        }
                        value={localItem.analyzerGroupID}
                        onChange={(event) => { this.onNewAnaSelectChange(event, 'analyzerGroupID') }}
                        placeholder="Chọn nhóm tài sản"
                        dropdownMatchSelectWidth = {21}
                        dropdownRender={menu => (
                          <div>
                            {menu}
                            <Divider style={{ margin: '4px 0' }} />
                            <div style={{ display: 'flex', flexWrap: 'nowrap', padding: 8 }}>
                              <div>
                                <Input placeholder="Mã nhóm" style={{ flex: 'auto' }} value={newAnalyzerGroup.AnalyzerGroupCode} onChange={(event) => { this.onNewAnaGroupChange(event, 'analyzerGroupCode') }} />
                                {newAnalyzerGroup.AnalyzerGroupCode == '' && this.validatorAnaGroup.message('Mã nhóm', newAnalyzerGroup.AnalyzerGroupCode, 'required')}
                              </div>
                              <div>
                                <Input placeholder="Tên nhóm" style={{ flex: 'auto' }} value={newAnalyzerGroup.AnalyzerGroupName} onChange={(event) => { this.onNewAnaGroupChange(event, 'analyzerGroupName') }} />
                                {this.validatorAnaGroup.message('Tên nhóm', newAnalyzerGroup.AnalyzerGroupName, 'required')}
                              </div>
                              <a
                                style={{ flex: 'none', padding: '8px', display: 'block', cursor: 'pointer' }}
                                onClick={() => { this.addAnaCategoryGroup() }}
                              >
                                <PlusOutlined /> Thêm 
                                    </a>
                            </div>
                          </div>
                        )}
                      >
                        {this.state.listAnalyzerGroup.map(item => (
                          <Option key={item.analyzerGroupID}>{item.analyzerGroupName}</Option>
                        ))}
                      </Select></p>
                    {this.validatorAna.message('ID nhóm', localItem.analyzerGroupID, 'required')}
                  </div>
                  <div style={{ flex: 4 }}> 
                    Hạn sử dụng <DatePicker style={{ marginLeft: '5px' }}
                      defaultValue={moment(new Date(), dateFormatList)}
                      value={this.state.localItem.expirationDate !== "" ? moment(this.state.localItem.expirationDate) : moment(new Date(), dateFormatList)}
                      format={dateFormatList}
                      locale={viVN}
                      allowClear={false}
                      onChange={(value) => this.changeDateExpired(value)}
                    />
                    {this.validatorAna.message('Hạn sử dụng', localItem.expirationDate, 'required')}
                  </div>
                </div>
                <p><Input style={{ border: '1px solid blue !important', height: '50px' }} placeholder="Mô tả" value={localItem.description} onChange={(event) => { this.onNewAnaChange(event, 'description') }} /></p>
                <p><Input style={{ border: '1px solid blue !important', height: '50px' }} placeholder="Mã kế toán" value={localItem.analyzerAccountantCode} onChange={(event) => { this.onNewAnaChange(event, 'analyzerAccountantCode') }} /></p>
                <p>
                  <div style={{ marginTop: '15px' }}>
                    <div style={{
                      width: '100%',
                      overflow: 'auto', padding: 10, marginTop: 20, marginBottom: 20, border: '1px solid #ccc', borderRadius: 30
                    }}>

                      <div style={{ display: 'flex' }}>
                        <div style={{
                          marginLeft: '32px',
                        }}>
                          <InputCustom
                            item={{
                              header: "Tìm Kiếm",
                              name: "search",
                              type: "input",
                              width: 200,
                              allowNull: true
                            }}
                            value={this.state.searchText}
                            onChange={(value) => {
                              this.changeSearchText(value);
                            }} />
                        </div>

                        <Button className="btn-search" style={{
                          width: '110px',
                          marginTop: '32px',
                          marginLeft: '32px',
                          height: '35px'
                        }} onClick={() => {
                          this.handelNewSearchItemCreate()
                        }}> Tìm kiếm
                        </Button>
                      </div>
                      <div style={{ marginTop: '15px' }}>
                        <MultiSelect
                          maxSelectedItems={1}
                          showSelectedItems={false}
                          showSelectAll={false}
                          height={350}
                          responsiveHeight={350}
                          items={this.state.quoteList}
                          selectedItems={this.state.selectedQuoteList}
                          onChange={this.handleChangeQuote.bind(this)}
                        />
                        {this.validatorAna.message('Mã tài sản', localItem.quoteItemID, 'required')}
                      </div>
                    </div>
                  </div>
                </p>

              </Modal>
              
            </div>  
          </div>
          <div style={{ flex: 1, padding: '20px' }}>
            <Table columns={this.getColumnTable()} dataSource={this.state.data} bordered={true}
              pagination={{
                total: this.state.totalRecords, defaultPageSize: this.state.pageSize, showSizeChanger: true,
                pageSizeOptions: ['10', '20', '50', '100']
              }}
              onChange={(page) => { this.handelChangePage(page) }}
            />
          </div> */}
        </div>
      </React.Fragment>
    );
  }
};
