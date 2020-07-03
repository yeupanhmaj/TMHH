import React, {Component} from 'react';
import {Bar} from 'react-chartjs-2';

const data = {
  labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
  datasets: [
    {
      label: 'My First dataset',
      backgroundColor: 'rgba(255,99,132,0.2)',
      borderColor: 'rgba(255,99,132,1)',
      borderWidth: 1,
      hoverBackgroundColor: 'rgba(255,99,132,0.4)',
      hoverBorderColor: 'rgba(255,99,132,1)',
      data: [65, 59, 80, 81, 56, 55, 40]
    }
  ]
};
export default class BarChart extends Component {
  _instance ;
    constructor(props) {
        super(props);
      }
    
  render() {
    return (
      <React.Fragment>
        <h2>Bar Example</h2>
        <Bar ref={(c) => { this._instance = c }} data={data}  redraw={true} options={{
          responsive: true,
          maintainAspectRatio: true,
        }}/>
        </React.Fragment>
    );
  }

  componentDidMount() {
  
  }
}