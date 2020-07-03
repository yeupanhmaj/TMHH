import React from 'react';
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
  componentDidMount() {
    if (this.props.defaultText !== undefined) {
      this.props.onChange(this.props.defaultText)
    }
  }
  render() {
    let value = this.props.value;
    return (
      <div key={"input "} className={"wrapcontrol"} style={{width:'90%' , margin:'auto' }}>
        <label>{this.props.label} </label>
        <div style={{ display: 'flex' }}>
          <textarea disabled={this.props.disabled} autoComplete="on" style={{
            fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
            boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
            width:'90%',
            height:100
          }}
            type={this.props.type} className="form-control" value={value || ''} onChange={
              this.handleChangeInput.bind(this)
            } />
        </div>
      </div>
    );
  }
}

