import React from 'react';
import { IColumDetails, IItemDefine } from '../propertiesType';

import SimpleReactValidator from 'simple-react-validator';

export default class RadioCustom extends React.Component {
    
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
    handleOptionChange(value) {
        this.props.onChange(value)
    }
    componentWillMount() {
        if (this.props.value ===undefined && this.props.item.defaultText) {
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
                 <div style={{ width:item.width ? item.width : 'auto' }}>
                <label >{item.header} </label>         {  !(item.allowNull ===true )  &&
                    <label style={{color:'red', fontWeight:'bold' , marginLeft:'5px'}}>{`( * ) `}</label> 
              }
                <div style={{display:'flex'}}>
                    {this.props.options && this.props.options.map((item, index) => {
                        return (
                            <div style={{ display: 'flex', flexDirection: 'row' , marginLeft:10 ,}} key={"childkey" + index + item.header}>
                                <div className="container-checkbox check-box-header" style={{ width: 20, margin: '8px 8px' }}
                                    onClick={() => { this.handleOptionChange(item.value) }}>
                                    <input type="checkbox" checked={value ===item.value} onChange={() => { }}
                                    />
                                    <span className="checkmark" style={{ transform: 'scale(1.2)' }}></span>
                                </div>
                                <label>{item.label} </label>
                            </div>
                        )
                    })
                    }
                    </div>
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
