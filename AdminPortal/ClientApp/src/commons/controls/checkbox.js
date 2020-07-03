import React from 'react';
import { IColumDetails, IItemDefine } from '../propertiesType';

import SimpleReactValidator from 'simple-react-validator';

export default class CheckBoxCustom extends React.Component {
    
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }
        });
    }

    changeSelect(name) {
        let value = this.props.value;
        if (value ===undefined) {
            value = true;
        } else {
            value = !value
        }
        this.props.onChange(value)
      
    }
    componentWillMount(){
        if(this.props.value ===undefined && this.props.item.defaultText){
                this.props.onChange(this.props.item.defaultText)
        }
    }
    render() {
        let item = this.props.item;
        let value = this.props.value;
        if (value ===undefined) {
            value = false;
        }
        let name = item.name;
        return (
            <div key={name}>
                <div style={{ display: 'flex', flexDirection: 'row', marginTop: this.props.marginTop ===true ? 30 : 0 }}>
                    <div className="container-checkbox check-box-header" style={{ width: 20, margin: '8px 8px' }}
                        onClick={() => { this.changeSelect(name) }}>
                        <input type="checkbox" checked={value} onChange={() => { }}
                        />
                        <span className="checkmark" style={{ transform: 'scale(1.2)' }}></span>
                    </div>
                    <label>{item.header} </label>
                </div>
                {/* {this.validator.message('values', value, 'required|alpha')} */}
            </div>
        );
    }
}
