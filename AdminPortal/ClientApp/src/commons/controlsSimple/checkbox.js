import React from 'react';
import SimpleReactValidator from 'simple-react-validator';
import './control.css';

export default class CheckBoxCustom extends React.Component {
    
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }
        });
    }

    changeSelect() {
        let value = this.props.value;
        if (value ===undefined) {
            value = true;
        } else {
            value = !value
        }
        this.props.onChange(value)   
    }

    componentWillMount(){
     
    }

    render() {
        let value = this.props.value;
        if (value ===undefined) {
            value = false;
        }
    
        return (
            <div className={"wrapcontrol"} key={this.props.key} style={this.props.style}>
                <div style={{ display: 'flex', flexDirection: 'row' }}>
                    <div  className="container-checkbox check-box-header" style={{ width: 20, margin: '8px 8px' }}
                        onClick={() => {if(this.props.disabled == true) return ;  this.changeSelect() }}>
                        <input disabled={this.props.disabled} type="checkbox" checked={value} onChange={() => { }}
                        />
                        <span className="checkmark" style={{ transform: 'scale(1.2)' }}></span>
                    </div>
                    <label>{this.props.lable} </label>
                </div>
                {/* {this.validator.message('values', value, 'required|alpha')} */}
            </div>
        );
    }
}
