import moment from 'moment';
import React from 'react';
import DatePicker from "react-datepicker";
import SimpleReactValidator from 'simple-react-validator';
import { IColumDetails } from '../propertiesType';

export default class InputCustom extends React.Component {
    
    datepickInstance;
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng chọn ngày tháng'
            }
        });
    }
    getValidator() {
        return this.validator.allValid();
    }
    handleChangeDateChange(value) {

        this.props.onChange(value);
    }
    showMessages() {
        this.validator.showMessages();
    }

    componentDidMount() {
        let format = 'DD-MM-YYYY';
        if (this.props.format) format = this.props.format
        let value = this.props.value;
        if (this.props.isEdit ===undefined) {         
            if (value = undefined) {
                this.props.onChange(new Date());
                return;
            }
            if (!(value instanceof Date)) {
                var date = moment(value, format);

                if (date.isValid()) {
                } else {
                    this.props.onChange(new Date());
                }
            } else {
                this.props.onChange(new Date());
            }
        }else{
            if (this.props.format){
                format ='DD-MM-YYYY HH:mm'
            } 
            this.props.onChange(moment(value, format).toDate());
        }
    }
    render() {
        let format = 'DD-MM-YYYY';
        if (this.props.format){
            format ='DD-MM-YYYY HH:mm'
        } 
        let item = this.props.item;
        let value = this.props.value;
        let name = item.name;
        if (!(value instanceof Date)) {
            var date = moment(value, format);
            if (date.isValid()) {
                value = date.toDate();
            } else {
                value = new Date()
            }
        }
        

        return (
            <div>  <label>{item.header} </label>
                {!(item.allowNull ===true) &&
                    <label style={{ color: 'red', fontWeight: 'bold', marginLeft: '5px' }}>{`( * ) `}</label>
                }
                <div
                    key={"datePicker" + name}
                    style={{
                        fontSize: 12, fontFamily: 'roboto', fontWeight: 400, width: '175px',
                        height: '33px',
                        boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                        paddingLeft: '5px',
                        border: 'none',
                        cursor: 'pointer',
                        backgroundColor: 'white'
                    }}>

                    <div style={{
                        display: 'flex',
                        flexDirection: 'row'
                    }}>
                        <DatePicker
                            className={"datePickerCustomInsideItemModal"}
                            dateFormat={this.props.format ===undefined ? "dd-MM-yyyy" : this.props.format}
                            showMonthDropdown
                            showYearDropdown
                            showTimeSelect={this.props.format ===undefined ? false : true}
                            timeIntervals={5}
                            todayButton="Hôm nay"
                            locale="vi"
                            ref={(c) => { this.datepickInstance = c }}
                            onChange={this.handleChangeDateChange.bind(this)}
                            selected={value}
                        />
                        <div style={{ cursor: 'pointer', lineHeight: '32px' }} onClick={() => {
                            this.datepickInstance.setOpen(true)
                        }}>
                            <i className="fa fa-calendar" ></i>
                        </div>
                    </div>
                    {item.allowNull ===false &&
                        this.validator.message(item.header, value, 'required')
                    }
                </div>
            </div>
        );
    }
}
