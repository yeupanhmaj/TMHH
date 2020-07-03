import * as React from 'react';
import { Tabs, Input } from 'antd';
import DataTable from './outDateProposals';
import Status from './status';
import ProposalsByDepartment from './proposalsByDepartment';
import EntendedProposals from './entendedProposals';
import Timeline from './timeline';


export default class Report extends React.Component {
    constructor(props) {
        super(props);
        this.state = {

        };
    }
    callback(key) {

    }
    render() {
        const { TabPane } = Tabs;
        return (
            <React.Fragment>
                <Tabs defaultActiveKey="1" onChange={this.callback}>
                <TabPane tab="Timeline" key="1">
                        <Timeline />
                    </TabPane>
                    <TabPane tab="Đề xuất quá hạn" key="2">
                        <DataTable />
                    </TabPane>
                    <TabPane tab="Đề xuất dự trù" key="3">
                        <EntendedProposals />
                    </TabPane>
                    <TabPane tab="Tình trạng" key="4">
                        <Status />
                    </TabPane>
                    <TabPane tab="Số lượng đề xuất theo khoa phòng" key="5">
                        <ProposalsByDepartment />
                    </TabPane>
                   
                </Tabs>
            </React.Fragment>
        );
    }
};
