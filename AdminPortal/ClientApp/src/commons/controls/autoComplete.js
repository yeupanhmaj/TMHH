import React from 'react';
import Select from 'react-select';
import Creatable from 'react-select/creatable';
import SimpleReactValidator from 'simple-react-validator';

export default class AutoCompleteCustom extends React.Component {
    
  
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập và chọn thông tin'
            }
        });
        this.state = {
            options: [],
        }
    }
   
    getValidator() {
        return this.validator.allValid();
    }
    showMessages() {
        this.validator.showMessages();
    }
    customStyles = function (width) {
        return {
            placeholder: () => ({
                margin: 0,
                color: '#aaa'
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
            control: () => ({
                display: 'flex',
                width: width ? width : 175,
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                height: 32,
                lineHeight: '20px',
                bordeRadius: 3,
                paddingLeft: 5,
                fontSize: 12,

                fontFamily: 'roboto',
                backgroundColor: 'white',

            }),
            singleValue: (provided, state) => {

                const opacity = state.isDisabled ? 0.5 : 1;
                // const backgroundColor = state.isDisabled ? "#e9ecef" : "white"
                const transition = 'opacity 300ms';
                return { ...provided, opacity, transition };
            }
        }
    }

    onSelectData(item) {
        this.props.onChange(item)
    };

    componentWillMount() {

        if (this.props.defaultValue !==null) {
            let options = this.state.options
            if (options ===undefined) options = [];
            options.push(this.props.defaultValue);
            this.setState({ options });

            if (this.props.value ===undefined)
                this.props.onChange(this.props.defaultValue)
        }
    }

    handleInputChange(newValue, force) {
        if (this.props.getData !==undefined) {
            if (newValue.length >= 1 || force ===true) {
                let rightFunc = this.props.getData
                let _self = this;
                if (this.task) clearTimeout(this.task);
                if (typeof rightFunc === "function") {
                    this.task = setTimeout(() => {
                        rightFunc(newValue).then((result) => {
      
                            if (result['isSuccess'] ==true) {
                                let options = [];
                                for (let item of result['data']) {
                                    if(this.props.labelCol != undefined && this.props.valueCol !=undefined  ){
                                       options.push({ label: item[this.props.labelCol], value: item[this.props.valueCol], item: item })
                                    }else{
                                        options.push({ label: item['label'], value: item['value'], item: item })
                                    }
                                }
    
                                _self.setState({ options })
                            }
                        }
                        )
                    }, 300);
                }
            }
        }
    };
    
    componentDidMount() {
        if (this.props.values && this.props.values.length > 0) {
            this.setState({ options: this.props.values })
        } else {
            this.handleInputChange('', true);
        }
    }
    render() {
        let header = this.props.header;
        let value = this.props.value;
        let name = this.props.name;
        let options = this.state.options
        let selectedValue = {} ;

        if (value && options) {
            for (let opt of options) {
                if(opt)
                if (value ===opt.value || value ===opt.label) {
                    selectedValue = opt;
                    break;
                }
            }
        }

        if (selectedValue.value ===undefined) {
            if (options ===undefined) {
                options = []
                if (value) {
                    options.push({ label: value, value: value })
                    selectedValue = { label: value, value: value }
                } else {
                    selectedValue = { label: '', value: '' }
                }
            } else {
                selectedValue = { label: value, value: value }
            }
            if (this.props.labelCol && this.props.valueCol) {
                selectedValue = value;
            }

        }

        return (
            <div key={"autoComplete" + name}>
                <label>{header} </label>
                {(this.props.allowNull ===undefined || this.props.allowNull ===false) &&
                    <label style={{ color: 'red', fontWeight: 'bold', marginLeft: '5px' }}>{`( * ) `}</label>
                }
                {this.props.isCreateable ===true ?
                    <Creatable
                        key={"autoComplete select" + name}
                        styles={this.customStyles(this.props.width)}
                        value={selectedValue}
                        onInputChange={(value) => { this.handleInputChange(value, false) }}
                        onChange={(value) => { this.onSelectData(value) }}
                        options={options || []}
                        onCreateOption={(inputValue) => {
                            if (this.props.createFuntion) this.props.createFuntion(inputValue)
                        }}
                        isDisabled={this.props.isDisable ===true || false}
                    />
                    :
                    <Select
                        key={"autoComplete select" + name}
                        styles={this.customStyles(this.props.width)}
                        value={selectedValue}
                        onInputChange={(value) => { this.handleInputChange(value, false) }}
                        onChange={(value) => { this.onSelectData(value) }}
                        options={options || []}
                        onCreateOption={(inputValue) => {
                        }}
                        isDisabled={this.props.isDisable ===true || false}
                    />
                }
                {(this.props.allowNull ===undefined || this.props.allowNull ===false) &&
                    this.validator.message(header, selectedValue, 'required')
                }
            </div>
        )
    }
}
