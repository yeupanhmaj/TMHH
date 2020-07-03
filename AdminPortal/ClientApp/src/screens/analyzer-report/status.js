import React, { Component } from 'react';
import { Pie } from 'react-chartjs-2';
import SimpleReactValidator from 'simple-react-validator';
import * as ReportService from '../../services/reportService'

export default class Status extends Component {

  constructor(props) {
    super(props);
    this.validator = new SimpleReactValidator();
    this.state = {
      data: undefined,
    }
  }
  componentDidMount() {
    ReportService.getReportCountStaus()
      .then(Response => {

        if (Response.isSuccess) {
          let data = {
            labels: [
              'Mới tạo',
              'Đã được duyệt',
              'Đang tiến hành',
              'Đã hoàn thành',
              'Đã bị từ chối',
            ],
            datasets: [{
              data: [0, 0, 0, 0, 0],
              backgroundColor: [
                '#0000CC',
                '#00AAAA',
                '#00BB55',
                '#00DD00',
                '#CC0000',
              ],
              hoverBackgroundColor: [
                '#0000CF',
                '#00CFAF',
                '#00CF5F',
                '#00CF00',
                '#CF0000',
              ]
            }]
          };

          let arrays = [0, 1, 2, 3, 4]

          for (let item of Response.data) {
            data.datasets[0].data[+item.label - 1] = item.value
            data.labels[+item.label - 1] = data.labels[+item.label - 1] + " : " + item.value

            arrays = arrays.filter(function (value, index, arr) {

              return value !== (+item.label - 1);

            });
          }

          for (let index of arrays) {
            data.labels[index] = data.labels[index] + " : " + 0
          }

          this.setState({ data })

        }
      }
      ).catch((ex) => {

      })

  }

  render() {
    return (
      <React.Fragment>
        <div style={{ textAlign: 'center', marginTop: 10, marginBottom: 10 }}>
          <h2 style={{ textAlign: 'center' }}>Thống kê các loại đề xuất theo trạng thái</h2>
        </div>
        {this.state.data !== undefined &&
          < Pie data={this.state.data} ref={(c) => { this._instance = c }} redraw={true} options={{
            responsive: true,
            maintainAspectRatio: true,
          }} height={80} />
        }
      </React.Fragment>
    );
  }
}