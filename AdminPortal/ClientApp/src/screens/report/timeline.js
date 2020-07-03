import { DatePicker, Table } from 'antd';
import moment from 'moment';
import React, { Component } from 'react';
import { FcCheckmark } from "react-icons/fc";
import { VerticalTimeline, VerticalTimelineElement } from 'react-vertical-timeline-component';
import 'react-vertical-timeline-component/docs/main.css';
import 'react-vertical-timeline-component/style.min.css';
import * as Service from '../../services/reportService';
import * as ProposalService from '../../services/proposalService';
import * as Actions from '../../libs/actions';
import './timeline.css';
const { RangePicker } = DatePicker;

const columns = [
  {
    title: 'Mã đề xuất',
    dataIndex: 'proposalCode',
    key: 'proposalCode',
  },
  {
    title: 'Khoa phòng đề xuất',
    dataIndex: 'departmentName',
    key: 'proposalCode',
  },
  {
    title: 'Ngày đề xuất',
    dataIndex: 'dateIn',
    key: 'proposalCode',
  },
];


export default class Timeline extends Component {
  constructor(props) {

    super(props);
    this.state = {
      data: [],
      lstProposal: [],
      totalRecords: 0,
      proposalCode: '',
      fromDate: moment().startOf('year').format('YYYY-MM-DD'),
      toDate: moment(new Date()).format('YYYY-MM-DD'),
      pageSize: 10,
      currentPage: 0,
      selectedItem: {},
      proposalCode: ''
    };
  }

  searchWithCondition() {
    let { proposalCode, fromDate, toDate, pageSize, currentPage } = this.state;
    ProposalService.GetAllProposalWithCondition(
      undefined,
      undefined,
      proposalCode,
      undefined,
      fromDate,
      toDate,
      undefined,
      pageSize,
      currentPage)
      .then(objRespone => {
        if (objRespone.isSuccess === true) {
          let temp = objRespone.data;
          for (let record of temp) {
            record.key = record.proposalCode;
          }
          this.setState({
            lstProposal: temp,
            totalRecords: objRespone.totalRecords
          })
          Actions.setLoading(false)
        } else {
          Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
        }
      }).catch(err => {

      })
  }

  componentDidMount() {
    this.searchWithCondition();
  };

  getTimeLine(record) {
    Actions.setLoading(true)
    Service.getProposalProccess(record.proposalID).then((response) => {
      let proposalCode = '';
      if (response.isSuccess == true) {
        let result = [];
        if (response.item.proposalDetailInfo != undefined && response.item.proposalDetailInfo.proposalID != 0) {
          result.push({
            name: `Đề xuất: ${response.item.proposalDetailInfo.proposalCode} `,
            code: response.item.proposalDetailInfo.proposalCode,
            dataDate: response.item.proposalDetailInfo.dateIn,
            createdAt: response.item.proposalDetailInfo.inTime,
            updatedAt: response.item.proposalDetailInfo.updateTime,
            userI: response.item.proposalDetailInfo.userI
          })
          proposalCode = response.item.proposalDetailInfo.proposalCode;
        }
        if (response.item.surveyDetailInfo != undefined && response.item.surveyDetailInfo.surveyID != 0) {
          result.push({

            name: `Khảo sát: ${response.item.surveyDetailInfo.surveyCode} `,
            code: response.item.surveyDetailInfo.surveyCode,
            dataDate: response.item.surveyDetailInfo.dateIn,
            createdAt: response.item.surveyDetailInfo.inTime,
            updatedAt: response.item.surveyDetailInfo.updateTime,
            userI: response.item.surveyDetailInfo.userI
          })
        }
        if (response.item.explanationDetailInfo != undefined && response.item.explanationDetailInfo.explanationID != 0) {
          result.push({
            name: `Giải trình: ${response.item.explanationDetailInfo.explanationCode} `,
            code: response.item.explanationDetailInfo.explanationCode,
            dataDate: response.item.explanationDetailInfo.dateIn,
            createdAt: response.item.explanationDetailInfo.inTime,
            updatedAt: response.item.explanationDetailInfo.updateTime,
            userI: response.item.explanationDetailInfo.userI
          })
        }
        if (response.item.quoteInfo != undefined && response.item.quoteInfo.quoteID != 0) {
          result.push({
            name: `Báo giá: ${response.item.quoteInfo.quoteCode} `,
            code: response.item.quoteInfo.quoteCode,
            dataDate: response.item.quoteInfo.dateIn,
            createdAt: response.item.quoteInfo.inTime,
            updatedAt: response.item.quoteInfo.updateTime,
            userI: response.item.quoteInfo.userI
          })
        }
        if (response.item.auditDetailInfo != undefined && response.item.auditDetailInfo.auditID != 0) {
          result.push({
            name: `Biên bản họp kiểm giá:  ${response.item.auditDetailInfo.auditCode} `,
            code: response.item.auditDetailInfo.auditCode,
            dataDate: response.item.auditDetailInfo.dateIn,
            createdAt: response.item.auditDetailInfo.inTime,
            updatedAt: response.item.auditDetailInfo.updateTime,
            userI: response.item.auditDetailInfo.userI
          })
        }
        if (response.item.bidPlanInfo != undefined && response.item.bidPlanInfo.bidPlanID != 0) {
          result.push({
            name: `Kế hoạch thầu: ${response.item.bidPlanInfo.bidPlanCode} `,
            code: response.item.bidPlanInfo.bidPlanCode,
            dataDate: response.item.bidPlanInfo.dateIn,
            createdAt: response.item.bidPlanInfo.inTime,
            updatedAt: response.item.bidPlanInfo.updateTime,
            userI: response.item.bidPlanInfo.userI
          })
        }
        if (response.item.negotiationInfo != undefined && response.item.negotiationInfo.bidPlanID != 0) {
          result.push({
            name: `Thương thảo hợp đồng: ${response.item.negotiationInfo.negotiationCode} `,
            code: response.item.negotiationInfo.negotiationCode,
            dataDate: response.item.negotiationInfo.dateIn,
            createdAt: response.item.negotiationInfo.inTime,
            updatedAt: response.item.negotiationInfo.updateTime,
            userI: response.item.negotiationInfo.userI
          })
        }


        if (response.item.decisionInfo != undefined && response.item.decisionInfo.decisionID != 0) {
          result.push({
            name: `Quyết định chọn thầu: ${response.item.decisionInfo.decisionCode} `,
            code: response.item.decisionInfo.decisionCode,
            dataDate: response.item.decisionInfo.dateIn,
            createdAt: response.item.decisionInfo.inTime,
            updatedAt: response.item.decisionInfo.updateTime,
            userI: response.item.decisionInfo.userI
          })
        }
        if (response.item.contractInfo != undefined && response.item.contractInfo.contractID != 0) {
          result.push({
            name: `Hợp đồng: ${response.item.contractInfo.contractCode} `,
            code: response.item.contractInfo.contractCode,
            dataDate: response.item.contractInfo.dateIn,
            createdAt: response.item.contractInfo.dateIn,
            updatedAt: response.item.contractInfo.updateTime,
            userI: response.item.contractInfo.userI
          })
        }
        if (response.item.deliveryReceiptInfo != undefined && response.item.deliveryReceiptInfo.deliveryReceiptID != 0) {
          result.push({
            name: `Biên bản giao nhận: ${response.item.deliveryReceiptInfo.deliveryReceiptCode} `,
            code: response.item.deliveryReceiptInfo.deliveryReceiptCode,
            dataDate: response.item.deliveryReceiptInfo.dateIn,
            createdAt: response.item.deliveryReceiptInfo.createTime,
            updatedAt: response.item.deliveryReceiptInfo.updateTime,
            userI: response.item.deliveryReceiptInfo.userI
          })
        }
        if (response.item.acceptanceInfo != undefined && response.item.acceptanceInfo.acceptanceID != 0) {
          result.push({
            name: `Bản nghiệm thu : ${response.item.acceptanceInfo.acceptanceCode} `,
            code: response.item.acceptanceInfo.acceptanceCode,
            dataDate: response.item.acceptanceInfo.dateIn,
            createdAt: response.item.acceptanceInfo.createTime,
            updatedAt: response.item.acceptanceInfo.updateTime,
            userI: response.item.acceptanceInfo.userI
          })
        }
        this.setState({ data: result, proposalCode })
        Actions.setLoading(false)
      }
    })
  }

  getSelection() {
    return {
      type: 'radio',
      onChange: (selectedRowKeys, selectedRows) => {

      },
      onSelect: (record, selected, selectedRows) => {

        this.getTimeLine(record);
      },

    };
  }
  onchangeSearchBinding(data, txt) {
    this.setState({ fromDate: txt[0], toDate: txt[1] });

    Actions.setLoading(true)
    this.searchWithCondition();
  }
  render() {
    let { data } = this.state;

    const listItems = () => (
      <VerticalTimeline>
        {this.state.proposalCode != '' &&
          <h1 >Đề xuất : {this.state.proposalCode}</h1>
        }
        {data.map((proccess) =>
          <VerticalTimelineElement
            //className="vertical-timeline-element--work"
            contentStyle={{ backgroundColor: 'rgb(230,230,230)' }}
            contentArrowStyle={{ borderRight: '7px solid  rgb(33, 150, 243)' }}
            iconStyle={{ backgroundColor: 'PaleGreen' }}
            icon={<FcCheckmark />}
            date={proccess.name}>
            <div style={{ lineHeight: '30px', display: 'flex', flexDirection: 'column' }}>
              <span className="vertical-timeline-element-subtitle">Ngày tạo: {moment(new Date(proccess.createdAt)).format('DD-MM-YYYY')}</span>
              <span className="vertical-timeline-element-subtitle">Ngày update: {moment(new Date(proccess.updatedAt)).format('DD-MM-YYYY')}</span>
              <span className="vertical-timeline-element-subtitle">Người tạo: {proccess.userI}</span>
            </div>

          </VerticalTimelineElement>
        )}
      </VerticalTimeline>
    );
    return (
      <div style={{}}>
        <div style={{ display: 'flex' }}>
          <div style={{ flex: 2 }}>
            <div style={{ display: 'flex' }}>
              <RangePicker
                defaultValue={[moment().startOf('month'), moment(new Date())]}
                format={'YYYY/MM/DD'}
                onChange={this.onchangeSearchBinding.bind(this)}
              />
            </div>
            <div style={{ display: 'flex', marginTop: 15 }}>


              <Table dataSource={this.state.lstProposal} columns={columns}

                rowSelection={this.getSelection()} />

            </div>
          </div>
          <div style={{ flex: 5 }}>
            <ul>{listItems()}</ul>
          </div>
        </div>

      </div>);
  }
}
