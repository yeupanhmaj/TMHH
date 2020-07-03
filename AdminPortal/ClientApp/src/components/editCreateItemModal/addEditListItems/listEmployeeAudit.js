import React from 'react';
import SimpleReactValidator from 'simple-react-validator';
import AutoCompleteCustom from '../../../commons/controls/autoComplete';
import * as Actions from '../../../libs/actions';
import * as EmployeeService from '../../../services/employeeService';


export default class ListEmployeeAudit extends React.Component {
    
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }
        });
        this.state = {
            ItemDetails: {} ,
            selectedItem: {} ,
            options: [],
            comment: '' ,
        }
    }
    getValidator() {
        return this.validator.fieldValid("listitem");
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
                bordeRadius: 3,
                paddingLeft: 5,
                fontSize: 12,
                lineHeight: '12px',
                fontFamily: 'roboto',
                backgroundColor: 'white'
            }),
            singleValue: (provided, state) => {
                const opacity = state.isDisabled ? 0.5 : 1;
                const transition = 'opacity 300ms';
                return { ...provided, opacity, transition };
            }
        }
    }
   

    addItem() {
        let items = this.props.items;
        if (items ===undefined) items = [];
        let comment = this.state.comment;
        let employeeID = this.state.selectedItem.value;
        let name = this.state.selectedItem.label;

        let valid = true;
        
        if (employeeID ===undefined || employeeID < 1) {
            this.validator.showMessageFor('employeeID');
            valid = false;
        }



        if (valid) {
            ///
            EmployeeService.GetEmployeeById(employeeID).then(Response => {
                if (Response.isSuccess) {
                    let roleName = Response.item.roleName
                    let item = {
                        employeeID,
                        name,
                        comment,
                        roleName
                    };
                    items.push(item);
                    this.setState({ employeeID: '', name: '', comment: '', selectedItem: {} })
                    this.props.onChange(items)
                }else{
                    Actions.openMessageDialog("Error " + Response.err.msgCode, Response.err.msgString.toString());
                }
            })

        } else {
            this.forceUpdate();
        }
    }
    removeGridViewItem(name, index) {
        let lstData = this.props.items
        if (lstData ===undefined) lstData = [];
        lstData.splice(index, 1);
        this.props.onChange(lstData)
    }

    moveupGridViewItem(name, index) {
        let lstData = this.props.items
        //if (lstData ===undefined) lstData = [];
        let tempData = lstData[index];
        lstData[index] = lstData[index - 1];
        lstData[index - 1] = tempData;
        this.props.onChange(lstData)
    }

    commentChange(value) {
        this.setState({ comment: value })
    }


    render() {
        let item = this.props.itemDefine;
        let name = item.name;
        let listItem = this.props.items ;
        return (
            <div key={"listItems" + name} >

                < div className="childDetailWarpper" >
                    <div className="addEmployeeItemWrapperInner">
                        <div className="childListEmployees" >
                            <AutoCompleteCustom
                                valueCol={"employeeID"}
                                labelCol={"name"}
                                name={"product"}
                                header={"Nhân viên"}
                                value={this.state.selectedItem}
                                getData={item.getDataFunc}
                                isDisable={this.props.disabled == true}
                                onChange={(value) => {
                                    this.setState({ selectedItem: value });
                                }} />
                            {this.validator.message('employeeID', this.state.selectedItem.value, 'required')}
                        </div>

                        <button type="button" disabled={this.props.disabled == true} style={{ marginTop: 25, height: 30, width: 75 }} className="btn btn-success"
                            onClick={() => {
                                this.addItem();
                            }}
                        > Thêm </button>
                    </div>
                    <div className={"addItemWrapper listItemWrapper"}  >
                        {!(item.allowNull ===true) &&
                            <label style={{ color: 'red', fontWeight: 'bold' }}>{` ( * ) `}</label>
                        }
                        <div style={{ display: 'table' }}>
                            <div className={'listItemHeader'}  >
                                <div style={{ minWidth: 100, flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên</div>
                                <div style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Chức vụ</div>

                                <div style={{ minWidth: 50, flex: 1, padding: 5 }}></div>
                                <div style={{ minWidth: 50, flex: 1, padding: 5 }}></div>
                            </div>
                            {listItem && listItem.map((item, index) => {
                                return (
                                    <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                        <div className="noWrap" style={{ minWidth: 100, flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.name}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.roleName}</div>

                                        {listItem[0] ===item && <div style={{ minWidth: 50, flex: 1, padding: 5 }}></div>}
                                        {listItem[0] !==item && <div className="noWrap" style={{ minWidth: 50, flex: 1, padding: 5 }}> <button
                                        disabled={this.props.disabled == true}
                                        type="button" title="Lên" className="btn btn-primary btnAction"
                                            onClick={(e) => {
                                                this.moveupGridViewItem(name, index)
                                            }}>
                                            <i className="fa fa-sort-up">
                                            </i></button>
                                        </div>}
                                        <div className="noWrap" style={{ minWidth: 50, flex: 1, padding: 5 }}> <button
                                        disabled={this.props.disabled == true}
                                        type="button" title="Xóa" className="btn btn-danger btnAction"
                                            onClick={(e) => {
                                                this.removeGridViewItem(name, index)
                                            }}>
                                            <i className="fa fa-trash">
                                            </i></button>
                                        </div>
                                    </div>
                                )
                            })}

                        </div>
                    </div>
                </div>
                {this.validator.message("listitem", this.props.items, 'required')}
            </div>
        )
    }
}
