import React, { Component } from 'react';
import LineChart from './lineChart'
import BubbleChart from './bubbleChart'
import Pie from './pie'
import BarChar from './barChart'

export default class Home extends Component {
  render() {
    return (
      <React.Fragment>
        <div style={{   width: '100%', height: '100%', padding:'20px'}}>
        {/* <div style={{ justifyContent: 'space-around' , flexWrap:'wrap', display:'flex' ,width: '100%', margin:'auto'}}>
          <div style={{minWidth:400,minHeight:200, padding:'5px 0px 5px 5px', width:'40%',	boxShadow:' 0 1px 5px rgba(0, 0, 0, 0.15)', margin:10 }}>
            <LineChart />
          </div>
          <div style={{minWidth:400,minHeight:200, padding:'5px 0px 5px 5px', width:'40%',	boxShadow:' 0 1px 5px rgba(0, 0, 0, 0.15)', margin:10 }}>
            <BubbleChart />
          </div>
        
        </div> */}
        <div style={{ justifyContent: 'space-around' , flexWrap:'wrap', display:'flex' ,width: '100%', margin:'auto'}}>
        <div style={{ minWidth:400,minHeight:200,padding:'5px 0px 5px 5px', width:'40%',	boxShadow:' 0 1px 5px rgba(0, 0, 0, 0.15)' , margin:10}}>
            <Pie />
          </div>
          {/* <div style={{ minWidth:400,minHeight:200, padding:'5px 0px 5px 5px', width:'40%',	boxShadow:' 0 1px 5px rgba(0, 0, 0, 0.15)', margin:10 }}>
            <BarChar />
          </div> */}
        </div>
        </div>
      </React.Fragment>
    );
  }

  componentDidMount() {

  }
}