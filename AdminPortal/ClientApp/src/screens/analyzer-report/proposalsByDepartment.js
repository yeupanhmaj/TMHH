import React, { Component } from 'react';
import { Bar } from 'react-chartjs-2';
import * as Service from '../../services/reportService';


export default class ProposalsByDepartment extends Component {
    _instance;
    constructor(props) {
        super(props);
        this.state = {
            data: {
                labels: [],
                datasets: [
                    {
                        label: 'Số đề xuất theo phòng',
                        backgroundColor: 'rgba(255,99,132,0.2)',
                        borderColor: 'rgba(255,99,132,1)',
                        borderWidth: 1,
                        hoverBackgroundColor: 'rgba(255,99,132,0.4)',
                        hoverBorderColor: 'rgba(255,99,132,1)',
                        data: []
                    },
                ]
            }
        }
    }

    render() {
        let { data } = this.state;
        return (
            <React.Fragment>
                <h2 style={{ textAlign: 'center' }}>Thống kê đề xuất theo từng phòng</h2>
                {data.labels.length > 0 &&
                    <Bar ref={(c) => { this._instance = c }} data={data} redraw={true} options={{
                        responsive: true,
                        maintainAspectRatio: true,
                    }} height={80} />
                }
            </React.Fragment>
        );
    }

    componentDidMount() {
        Service.getProposalByDepartment().then((response) => {
            if (response.isSuccess == true) {
                let { data } = this.state;
                for (let item of response.data) {
                    data.labels.push(item.label)
                    data.datasets[0].data.push(item.value)
                }
                this.setState({ data })
            }
        })
    }
}