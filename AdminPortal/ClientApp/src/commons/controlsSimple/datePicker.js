import moment from 'moment';
import React from 'react';
import DatePicker from "react-datepicker";
import './control.css';

export default class DatePickerCustom extends React.Component {

    datepickInstance;
    constructor(props) {
        super(props);
       
    }

    handleChangeDateChange(value) {
        let {showTimeSelect } = this.props;
        if(value)
        if(showTimeSelect) 
        {
            
            this.props.onChange(moment(value).format('DD-MM-YYYY HH:mm') )
        }else{
  
            this.props.onChange(moment(value).format('DD-MM-YYYY') )
        }
    }
  
    componentDidMount() {
        
    }
    render() {
        let format = 'dd-MM-yyyy';
        let formatMM = "DD-MM-YYYY";
        let {value , label , showTimeSelect , disabled} = this.props;
        if(showTimeSelect) 
        {
             format = 'dd-MM-yyyy HH:mm';
            formatMM = 'DD-MM-YYYY HH:mm'
        }
        var date = moment(value , formatMM).toDate();   
  
        return (
            <div className={"wrapcontrol"}  >  <label>{label} </label>
               
                <div
                    key={"datePicker"}
                    style={{
                        fontSize: 12, fontFamily: 'roboto', fontWeight: 400, width: '175px',
                        height: '33px',
                        boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                        paddingLeft: '5px',
                        border: 'none', 
                        cursor: 'pointer',
                        backgroundColor:  disabled == true ? 'rgb(235, 235, 238)': 'white'
                    }}>

                    <div style={{
                        display: 'flex',
                        flexDirection: 'row'
                    }}>
                        <DatePicker
                            disabled={disabled}
                            className={"datePickerCustomInsideItemModal"}
                            dateFormat={format}
                            showMonthDropdown
                            showYearDropdown
                            showTimeSelect={showTimeSelect ==  true}
                            timeIntervals={5}
                            todayButton="Hôm nay"
                            locale="vi"
                            ref={(c) => { this.datepickInstance = c }}
                            onChange={this.handleChangeDateChange.bind(this)}
                            selected={date}
                        />
                        <div style={{ cursor: 'pointer', lineHeight: '34px' }} onClick={() => {
                            this.datepickInstance.setOpen(true)
                        }}>
                            <i className="fa fa-calendar" ></i>
                        </div>
                    </div>
                  
                </div>
            </div>
        );
    }
}
