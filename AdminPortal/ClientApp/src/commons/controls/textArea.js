import React from 'react';
import { IColumDetails, IItemDefine } from '../propertiesType';

import SimpleReactValidator from 'simple-react-validator';

export default class TextAreaCustom extends React.Component {
  
  constructor(props) {
    super(props);
    this.validator = new SimpleReactValidator({
      messages: {
        required: 'vui lòng nhập thông tin'
      }
    });
  }
  handleChangeTextArea(event) {
    let value = event.target.value;
    this.props.onChange(value)
  }
  getValidator() {
    return this.validator.allValid();
  }
  showMessages() {
    this.validator.showMessages();
  }
  render() {
    let item = this.props.item;
    let value = this.props.value;
    let name = item.name;
    let width = item.width || '100%';
    let height = item.height || 130;

    return (
      <div key={"textArea" + name } style={{width:width}}>
        <label>{item.header} </label>
        {  !(item.allowNull ===true )  &&
                    <label style={{color:'red', fontWeight:'bold' , marginLeft:'5px'}}>{`( * ) `}</label> 
              }
        <textarea style={{
          fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
          boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
          height: height, width: '100%',
        }}
          className="form-control" disabled={item.isDisable ===true} name={name || ''} value={value || ''} onChange={
            this.handleChangeTextArea.bind(this)
          } />
        {item.allowNull ===undefined &&
          this.validator.message(item.header, value, `required${item.minlength?'|min:'+ item.minlength : ''}`)
        }
      </div>
    )
  }
}
