import * as React from 'react';
import { Tabs, Input } from 'antd';
import AnalyzerByDate from './analyzerbydate'


export default class ReportAnalyzer extends React.Component {
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
                    <TabPane tab="Đề xuất theo thời gian" key="1">
                        <AnalyzerByDate></AnalyzerByDate>
                    </TabPane>
                </Tabs>
            </React.Fragment>
        );
    }
};
