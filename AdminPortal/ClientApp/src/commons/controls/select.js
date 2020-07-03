import React from 'react';
import Select from 'react-select';
import SimpleReactValidator from 'simple-react-validator';

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
                width: width ? width : 175,
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
        let item = this.props.item;

        if (this.props.value ===undefined) {
            if (item.valueDefault) {
                this.props.onChange(item.valueDefault)
            }
        }else{
            if(this.props.options){
                for (let item of this.props.options){
                    if(item.label ===this.props.value){
                        if(item.value !==this.props.value){
                            this.props.onChange(item.value);
                            break;
                        }
                    }
                }
            }
        }

    }
    handleChangeSlect(item) {
        let label = item.label;
        this.setState(
            { label }
        );
        this.props.onChange(item.value)
        if(this.props.cbFunc){
            this.props.cbFunc(item.value)
        }
    }
    render() {

        let item = this.props.item;
        let value = this.props.value;
        if (value && typeof value ==="string") value = value.trim();
        let name = item.name;
        let options = this.props.options
        let selectedValue = {} 
        let label = this.state.label
        if (value !==undefined) {
            for (let item of options) {
                if (item.value ===value) {
                    label = item.label
                    break;
                } else {
                    if (item.label ===value) {
                        label = value
                        value = item.value
                        break;
                    }
                }
            }
        }else{
            if(this.props.defaultValue) value = this.props.defaultValue
        }

        selectedValue = { label: label, value: value }

        return (
            <div key={"select" + name}>
                <label>{item.header} </label>
                {!(item.allowNull ===true) &&
                    <label style={{ color: 'red', fontWeight: 'bold', marginLeft: '5px' }}>{`( * ) `}</label>
                }
                <Select
                    placeholder={name}
                    styles={this.customStyles(item.isDisable, item.width)}
                    value={selectedValue}
                    onChange={(value) => { this.handleChangeSlect(value) }}
                    options={options}
                    isDisabled={item.isDisable}
                />
                {(item.allowNull ===undefined || item.allowNull ===false) &&
                    this.validator.message('vaulue', this.props.value, 'required')
                }
            </div>
        )
    }
}
