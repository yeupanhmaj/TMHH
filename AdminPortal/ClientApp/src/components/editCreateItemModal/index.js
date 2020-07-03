
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import AutoCompleteCustom from '../../commons/controls/autoComplete';
import CheckBoxCustom from '../../commons/controls/checkbox';
import DatePickerCustom from '../../commons/controls/datePicker';
import EmailCustom from '../../commons/controls/email';
import InputCustom from '../../commons/controls/input';
import ListAttachfiles from '../../commons/controls/listAttachfiles';
import PasswordCustom from '../../commons/controls/password';
import RadioCustom from '../../commons/controls/radio';
import SelectCustom from '../../commons/controls/select';
import TextAreaCustom from '../../commons/controls/textArea';
import FileUploader from '../../components/fileUploader';
import ListEmployeeAudit from './addEditListItems/listEmployeeAudit';
import ListEmployeeDeliveryReceipt from './addEditListItems/listEmployeeDeliveryReceipt';
import ListItemAcceptance from './addEditListItems/listItemAcceptance';
import ListItemDeliveryReceipt from './addEditListItems/listItemDeliveryReceipt';
import ListItemExplanation from './addEditListItems/listItemExplanation';
import ListItemProposal from './addEditListItems/listItemProposal';
import ListItemQuote from './addEditListItems/listItemQuote';
import ListItemSurvey from './addEditListItems/listItemSurvey';
import './main.css';

export default class EditCreateItemModal extends React.Component {
 
  constructor(props) {
    super(props);
    this.state = {
      localItem: {},
      isEdit: false,
    }
    this._refItem = {}
  }



  checkValidAll() {
    let ret = true;

    var _self = this;
    Object.keys(this._refItem).forEach(function (prop) {
      if (_self._refItem[prop] && _self._refItem[prop].getValidator) {
        if (_self._refItem[prop].getValidator() === false) {
          if (_self._refItem[prop].showMessages) _self._refItem[prop].showMessages()
          ret = false;
        }
      }
    });

    this.forceUpdate();
    return ret;
  }

  getCurrentItem() {
    return this.state.localItem;

  }

  onSubmmit() {

    if (this.checkValidAll()) {
      if (this.props.onSubmmit)
        this.props.onSubmmit(this.state.localItem);
      //
      if (this.props.onSubmmitWithAttachFile)
        this.props.onSubmmitWithAttachFile(this.state.localItem, this._fileUploader.getFiles());
        this.setState({localItem: {}})
    }
  }

  static getDerivedStateFromProps(nextProps, prevState)
  {

    if (nextProps.item && prevState.localItem == {}  || ( nextProps.item  && nextProps.itemDefines && prevState.itemDefines == undefined )) {
      let isEdit = false;
      if ((nextProps.item )[nextProps.keyColumn] !== undefined) {
        isEdit = true
      }
      return  { localItem: nextProps.item, isEdit };
    }
    return null
  }



  changeBindingData(value, name) {
    let { localItem } = this.state;
    let temp = (localItem )
    temp[name] = value;

    this.setState({ localItem: temp })
  }

  getType(details) {
    let ret = {}
    let name = details.name;
    let type = details.type
    let values = details.values || [];
    const { localItem } = this.state
    switch (type) {
      case "autoComplete":
        ret = (
          <AutoCompleteCustom
            values={values}
            ref={(c) => this._refItem["autoComplete" + name] = c}
            allowNull={details.allowNull}
            labelCol={details.labelCol ? details.labelCol : 'label'}
            valueCol={details.valueCol ? details.valueCol : 'value'}
            key={"autoComplete" + name}
            name={details.name}
            header={details.header}
            value={(localItem )[name]}
            getData={details.getDataFunc ? details.getDataFunc.bind(this) : null}
            isDisable={details.isDisable || false}
            defaultValue={details.valueDefault || undefined}
            onChange={(value) => {
              this.changeBindingData(value, name);
              if (details.cbFunc)
                details.cbFunc(value, this.state.localItem);
            }} />
        )
        break
      case "input":
        ret = (
          <InputCustom
            ref={(c) => this._refItem["input" + name] = c}
            key={"input" + name}
            item={details}
            value={(localItem )[name]}
            defaultText={details.defaultText}
            onChange={(value) => {
              this.changeBindingData(value, name);
            }} />
        )
        break;
        case "password":
          ret = (
            <PasswordCustom
              ref={(c) => this._refItem["password" + name] = c}
              key={"password" + name}
              item={details}
              value={(localItem )[name]}
              defaultText={details.defaultText}
              onChange={(value) => {
                this.changeBindingData(value, name);
              }} />
          )
          break;
          case "email":
            ret = (
              <EmailCustom
                ref={(c) => this._refItem["email" + name] = c}
                key={"email" + name}
                item={details}
                value={(localItem )[name]}
                defaultText={details.defaultText}
                onChange={(value) => {
                  this.changeBindingData(value, name);
                }} />
            )
            break;
      case "select":
        ret = (
          <SelectCustom
            ref={(c) => this._refItem["select" + name] = c}
            defaultValue={details.valueDefault || undefined}
            key={"select" + name}
            item={details}
            options={values}
            value={(localItem )[name]}
            onChange={(value) => {
              this.changeBindingData(value, name);
              if (details.cbFunc)
                details.cbFunc(value, this.state.localItem);
            }} />
        )
        break;
      case "checkbox":
        ret = (
          <CheckBoxCustom
            key={"checkbox" + name}
            item={details}
            value={(localItem )[name]}
            marginTop={this.props.itemDefines.auto}
            defaultText={details.defaultText}
            onChange={(value) => {
              this.changeBindingData(value, name);
              if (details.cbFunc) {
                details.cbFunc(value, this.state.localItem);
              }
            }} />
        )
        break;
      case "radio":
        ret = (
          <RadioCustom
            options={values}
            key={"radio" + name}
            item={details}
            value={(localItem )[name]}
            marginTop={this.props.itemDefines.auto}
            defaultText={details.defaultText}
            onChange={(value) => {
              this.changeBindingData(value, name);
              if (details.cbFunc) {
                details.cbFunc(value, this.state.localItem);
              }
            }} />
        )
        break;
      case "datePicker":
        ret = (
          <DatePickerCustom
            ref={(c) => this._refItem["datePicker" + name] = c}
            key={name}
            item={details}
            format={details.format}
            value={(localItem )[name]}
            isEdit={this.state.isEdit ? true : undefined}
            onChange={(value) => {
              this.changeBindingData(value, name);
              if (details.cbFunc)
                details.cbFunc(value, this.state.localItem);
            }} />
        )
        break;
      case "textArea":
        ret = (
          <TextAreaCustom
            ref={(c) => this._refItem["textArea" + name] = c}
            key={"textArea" + name}
            item={details}
            value={(localItem )[name]}
            onChange={(value) => {
              this.changeBindingData(value, name);
            }} />
        )
        break;
      case "listItems":
        ret = (
          <ListItemProposal
            ref={(c) => this._refItem["listItems" + name] = c}
            itemDefine={details}
            items={(localItem )[name]}
            onChange={(value) => {
              this.changeBindingData(value, name);
            }}
          />
        )
        break;
      case "listItemAcceptance":
        ret = (<ListItemAcceptance
          ref={(c) => this._refItem["listItemAcceptance" + name] = c}
          itemDefine={details}
          items={(localItem )[name]}
          onChange={(value) => {
            this.changeBindingData(value, name);
          }}
        />)
        break;
      case "ListItemSurvey":   
        ret = (<ListItemSurvey
          ref={(c) => this._refItem["listItemSurvey" + name] = c}
          itemDefine={details}
          items={(localItem )[name]}
          onChange={(value) => {
            this.changeBindingData(value, name);
          }}
        />)
        break;
      case "listItemQuote":

        ret = (<ListItemQuote
          ref={(c) => this._refItem["listItemQuote" + name] = c}
          itemDefine={details}
          items={(localItem )[name]}
          VAT={(localItem )['isVAT']}
          vatNumber={(localItem )['vatNumber']}
          onChange={(value) => {
            this.changeBindingData(value, name);
          }}
        />)
        break;
      case "listItemExplanation":
        ret = (<ListItemExplanation
          ref={(c) => this._refItem["listItemExplanation" + name] = c}
          itemDefine={details}
          items={(localItem )[name]}
          onChange={(value) => {
            this.changeBindingData(value, name);
          }}
        />)
        break;
      case "listItemDeliveryReceipt":
        ret = (<ListItemDeliveryReceipt
          ref={(c) => this._refItem["listItemDeliveryReceipt" + name] = c}
          itemDefine={details}
          type={(localItem )["deliveryReceiptType"]}
          VAT={(localItem )['isVAT']}
          vatNumber={(localItem )['vatNumber']}
          items={(localItem )[name]}
          onChange={(value) => {
            this.changeBindingData(value, name);
          }}
        />)
        break;
      case "listEmployeeAudit":
        ret = (
          <ListEmployeeAudit
            ref={(c) => this._refItem["listEmployeeAudit" + name] = c}
            itemDefine={details}
            items={(localItem )[name]}
            onChange={(value) => {
              this.changeBindingData(value, name);
            }}
          />
        )
        break;
      case "listEmployeeDeliveryReceipt":
        ret = (
          <ListEmployeeDeliveryReceipt
            ref={(c) => this._refItem["listEmployeeDeliveryReceipt" + name] = c}
            itemDefine={details}
            items={(localItem )[name]}
            onChange={(value) => {
              this.changeBindingData(value, name);
            }}
          />
        )
        break;
      default:
        ret = (<div style={{ display: 'none' }}></div>);
        break
    }
    return ret
  }
  renderBody() {
    let localItem = this.state.localItem ;
    let sortedItemDefine = this.props.itemDefines;
    if (sortedItemDefine)
      if (sortedItemDefine.sortByOrder === true) {
        sortedItemDefine.props.sort((a, b) => {
          if (a.order === undefined) return 1;
          if (b.order === undefined) return 1;
          return a.order - b.order
        })
      }
    if (sortedItemDefine !== undefined && sortedItemDefine.props !== undefined && localItem) {
      return (
        <div style={{ width: '100%' }}>

          <div style={{
            display: 'flex',
            flexWrap: 'wrap',
            justifyContent: 'flex-start'
          }}>
            {sortedItemDefine.props.map((details, index) => {
              if (details.hidden === false || details.hidden === undefined) {
                return (
                  <div key={index + `itemDefinesWrap`}
                    style={{ width: details.wrapwidth ? details.wrapwidth : 'auto', }}
                    className={
                      (details.IsFull === true) ? 'wrapTextArea' : 'wrapOther'}>
                    {(details.hidden === undefined || details.hidden === false) &&
                      <div key={index + `itemDefines`} className={'ItemEditDiv'}>
                        {this.getType(details)}
                      </div>
                    }
                  </div>
                )
              }
            }
            )}
          </div>

          {this.props.OthersChild}

          <ListAttachfiles
            isShowRemove={true}
            item={localItem['listDocument']}
            onRemove={(value) => {
              let curItem = this.state.localItem ;
              curItem['listDocument'] = value;
              this.setState({ localItem: curItem })
            }}
            feature={this.props.referFeatureType || ''}
          />
        </div>
      )
    } else {
      return (
        <div>
        </div>
      )
    }
  }

  render() {
    return (
      <div>

        {this.props.item &&
          <Modal isOpen={this.props.Modal}>
            {this.props.Modal &&
              <div onClick={() => { this.props.onCancel() }}
                className="closeIcon">
                <i className="fa fa-window-close" aria-hidden="true"></i>
              </div>
            }
            <ModalHeader>
              <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                {this.props.headerName}
              </div>
            </ModalHeader>
            <ModalBody style={{
              position: 'relative',
              flex: '1 1 auto',
              padding: '20px',

              width: '98%',
              minHeight: '300px',
              justifyContent: 'center',
              alignItems: 'center',
              fontSize: '12px',
              flexDirection: 'row',
            }}>
              <div>
                {this.renderBody()}
                
              </div>
              {this.props.isHasUploader &&
                <div style={{ marginBottom: 20, marginTop: 15 }}>
                  <FileUploader ref={(c) => { this._fileUploader = c }} />
                </div>
              }
            </ModalBody>
            <div style={{
              display: 'flex',
              justifyContent: 'center',
              alignItems: 'center',
              fontSize: '12px',
              marginBottom: '30px',
              marginTop: '20px'
            }}>
              <Button className="btn-danger" style={{ width: '100px', marginLeft: '-30' }} onClick={() => {
                { this.onSubmmit() }
              }}>Lưu</Button>{' '}
              <Button className="btn-default" style={{ width: '100px', marginLeft: 30 }} onClick={() => { this.props.onCancel() }}>Tắt</Button>
            </div>
          </Modal>
        }
      </div>
    );
  }
}

