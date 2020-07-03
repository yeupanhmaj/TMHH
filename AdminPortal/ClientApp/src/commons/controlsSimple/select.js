import React from 'react';
import Select from 'react-select';
import SimpleReactValidator from 'simple-react-validator';
import './control.css';

export default class SelectCustom extends React.Component {
    
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng chọn thông tin'
            }
        });
        this.state = {
            label: '',
        }
    }
    getValidator() {
        return this.validator.allValid();
    }
   
    showMessages() {
        this.validator.showMessages();
    }
    customStyles = function (disable, width) {
        return {
            placeholder: () => ({
                margin: 0,
                color: '#aaa',
                backgroundColor: disable ? "#e9ecef" : "white"
            }),
            indicatorSeparator: () => ({
                color: '#fff'
            }),

            option: (provided, state) => ({
                ...provided,
                fontSize: 12,
                lineHeight: '12px',
                fontFamily: 'roboto',
                marginTop: 4
            }),
            control: (state) => ({
                display: 'flex',
                width: width ? 350 : 175,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                height: 32,
                bordeRadius: 3,
                paddingLeft: 5,
                fontSize: 12,
                lineHeight: '20px',
                fontFamily: 'roboto',
                backgroundColor: disable ? "#e9ecef" : "white"
            }),
            singleValue: (provided, state) => {
                const backgroundColor = disable ? "#e9ecef" : "white"
                return { ...provided, backgroundColor };
            }
        }
    }

    componentDidMount() {
    }

    handleChangeSlect(item) {
        this.props.onChange(item)
    }
    render() {
     
     let {label ,options , value , disabled} = this.props;
        return (
            <div className={"wrapcontrol"} >
                <label>{label} </label>
                <Select
                    styles={this.customStyles(disabled, this.props.doubleWidth)}
                    value={value}
                    onChange={(value) => { 
                     this.handleChangeSlect(value) }}
                    options={options}
                    isDisabled={disabled == true}
                />           
            </div>
        )
    }
}
