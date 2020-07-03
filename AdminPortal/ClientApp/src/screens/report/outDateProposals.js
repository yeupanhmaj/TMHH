import * as React from 'react';
import moment from 'moment';
import { Table, Tag, Space  , DatePicker } from 'antd';
import * as Service from '../../services/reportService';

export default class DueProposal extends React.Component {
    constructor(props) {

        super(props);
        this.state = {
            data: []
        };
    }
    componentDidMount() {
      
        Service.getOutDateProposal().then((response) => {
            if (response.isSuccess == true) {
                response.data.map((item) => {
                    item.key = item.proposalID;

                    return item;
                })
                for (let item of response.data) {
                    item.dateIn = moment(new Date(item.dateIn)).format('DD-MM-YYYY');
                    item.dueDate = moment(new Date(item.dueDate)).format('DD-MM-YYYY');
                }
                this.setState({ data: response.data })
            }
        })
    };
    render() {
        const columns = [
            {
                title: 'ID',
                dataIndex: 'proposalID',
                key: 'proposalID',
                render: text => <a>{text}</a>,
            },
            {
                title: 'Mã đề xuất',
                dataIndex: 'proposalCode',
                key: 'proposalCode',
                render: text => <a>{text}</a>,
            },
            {
                title: 'Loại đề xuất',
                dataIndex: 'proposalTypeName',
                key: 'proposalTypeName',
            },
            {
                title: 'Phòng ban yêu cầu',
                dataIndex: 'departmentName',
                key: 'departmentName',
            },
            {
                title: 'Phòng ban phụ trách',
                dataIndex: 'curDepartmentName',
                key: 'curDepartmentName',
            },
            {
                title: 'Ngày lập',
                key: 'dateIn',
                dataIndex: 'dateIn',
            },
            {
                title: 'Ngày hết hạn',
                key: 'dueDate',
                dataIndex: 'dueDate',
            },
            {
                title: 'Ngày quá hạn',
                key: 'dateDiff',
                dataIndex: 'dateDiff',
                render: text => <a style={{ color: 'red' }}>{text}</a>,
            }
        ];
        return (
            <React.Fragment>
            <Table dataSource={this.state.data} columns={columns} />
            </React.Fragment>
        );
    };
};