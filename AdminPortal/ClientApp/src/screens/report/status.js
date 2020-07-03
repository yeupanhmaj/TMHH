import React, { Component } from 'react';
import { Pie } from 'react-chartjs-2';
import SimpleReactValidator from 'simple-react-validator';
import * as ReportService from '../../services/reportService'
const          labelsLocal= [
  'Mới tạo',
  'Đang tiến hành',
  'Đã hoàn thành',

]
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
              'Đang tiến hành',
              'Đã hoàn thành',
            
            ],
            datasets: [{
              data: [0, 0, 0],
              backgroundColor: [
                '#0000CC',
                '#00AAAA',
                '#00BB55',         
              ],
              hoverBackgroundColor: [
                '#0000CF',
                '#00CFAF',
                '#00CF5F',
              ]
            }]
          };

          let arrays = [0, 1, 2]
          let index = 0 ;
          for (let item of Response.data) {
            data.datasets[0].data[index ] = item.value
            data.labels[index ] = labelsLocal[index ] 

      
            index++;
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