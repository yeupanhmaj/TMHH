import React from 'react';
import { IColumDetails, IItemDefine } from '../propertiesType';

import SimpleReactValidator from 'simple-react-validator';

export default class InputCustom extends React.Component {
    
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }
        });
    }
    getValidator() {
        return this.validator.allValid();


    }
    showMessages() {
        this.validator.showMessages();
    }
    handleChangeInput(event) {
        let value = event.target.value;
        this.props.onChange(value)
    }
    componentDidMount(){

        if(this.props.defaultText !==undefined){
            this.props.onChange(this.props.defaultText) 
        }
    }
    render() {
        let item = this.props.item;
        let value = this.props.value;
        let name = item.name;

        return (
            <div key={item.name}>
                <label>{item.header} </label>
                {  !(item.allowNull ===true )  &&
                    <label style={{color:'red', fontWeight:'bold' , marginLeft:'5px'}}>{`( * ) `}</label> 
              }
              <div style={{display:'flex'}}>
                <input autoComplete="on" style={{
                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                    width: item.width !==undefined ? item.width : '175px'
                }}
                    type={`${item.isNumber ===true ? 'number' : 'text'}`} className="form-control" disabled={item.isDisable ===true} name={name || ''} value={value || ''} onChange={
                        this.handleChangeInput.bind(this)
                    } />
                      {  item.suffix !==undefined  &&
                    <label style={{color:'black', fontWeight:'bold' , marginLeft:'5px'}}>{item.suffix}</label> 
              }
              </div>
                {item.isDisable ===false && item.allowNull ===true &&
                    this.validator.message(item.header, value, 'required')
                }
                 {item.allowNull ===undefined &&
                        this.validator.message(item.header, value, 'required')
                    }
            </div>
        );
    }
}