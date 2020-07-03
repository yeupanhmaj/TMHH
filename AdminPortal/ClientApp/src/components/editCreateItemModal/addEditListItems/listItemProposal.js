import React from 'react';
import SimpleReactValidator from 'simple-react-validator';
import AutoCompleteCustom from '../../../commons/controls/autoComplete';
import * as CommonService from '../../../services/commonService';
import CreateProductModal from './createProductModal';

export default class ListItemProposal extends React.Component {

    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }

        });
        this.state = {
            note: '',
            ItemDetails: {},
            amount: 0,
            selectedItemName: {},
            selectedItemCode: {},
            options: [],

            productName: '',
            createItemModal: false
        }
    }

    getValidator() {
        return this.validator.fieldValid("listitem");
    }
    showMessages() {
        this.validator.showMessageFor("listitem");
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
        if (items === undefined) items = [];
        let note = this.state.note;
        let amount = this.state.amount;
        let itemId = this.state.selectedItemName.value;
        let itemName = this.state.selectedItemName.label;
        let itemCode = this.state.selectedItemCode.label;
        let valid = true;
        let itemUnit = this.state.CurrItem.itemUnit;

        if (itemId === undefined || itemId === '') {
            this.validator.showMessageFor('item');
            valid = false;
        }
        if (amount === undefined || amount < 1) {
            this.validator.showMessageFor('amount');
            valid = false;
        }
        if (valid) {
            let item = {
                note,
                amount,
                itemId,
                itemName,
                itemCode, itemUnit
            };
            items.push(item);
            this.setState({ note: '', amount: '', itemId: '', itemName: '', selectedItem: {}, unit: '' })
            this.props.onChange(items)
        } else {
            this.forceUpdate();
        }
    }

    removeGridViewItem(name, index) {
        let lstData = this.props.items
        if (lstData === undefined) lstData = [];
        lstData.splice(index, 1);
        this.props.onChange(lstData)
    }

    amountChange(value) {
        this.setState({ amount: value })
    }
    noteChange(value) {
        this.setState({ note: value })
    }

    createNewItemFunc(name) {
        this.setState({ createItemModal: true, productName: name })
    }

    render() {
        let item = this.props.itemDefine;
        let name = item.name;
        let listItem = this.props.items;

        return (
            <React.Fragment>
                <div key={"listItems" + name} >
                    < div className="childDetailWarpper" style={{ marginTop: '20px' }}>
                        <div className="addItemWrapperInner">
                            {/* Nguyen Minh Hoang*/}
                            <div className={"childListItems"}>
                                <AutoCompleteCustom
                                    valueCol={"categoryID"}
                                    labelCol={"categoryName"}
                                    name={"productCode"}
                                    header={"Loại sản phẩm"}
                                    value={this.state.selectedItemCode}
                                    getData={CommonService.GetListCategory}
                                    onChange={(value) => {
                                        console.log(value)
                                    }} />
                                {this.validator.message('item', this.state.selectedItemCode.value, 'required')}
                            </div>
                            <div className={"childListItems"}>
                                <AutoCompleteCustom
                                    valueCol={"itemID"}
                                    labelCol={"itemCode"}
                                    name={"productCode"}
                                    header={"Mã Sản phẩm"}
                                    value={this.state.selectedItemCode}
                                    getData={CommonService.GetListItemByCode}
                                    onChange={(value) => {
                                        let selectedItemName = { label: value.item.itemName, value: value.item.itemID }

                                        this.setState({ selectedItemCode: value, selectedItemName, CurrItem: value.item });
                                    }} />
                                {this.validator.message('item', this.state.selectedItemCode.value, 'required')}
                            </div>
                            <div className={"childListItems"}>
                                <AutoCompleteCustom
                                    valueCol={"itemID"}
                                    labelCol={"itemName"}
                                    name={"product"}
                                    header={"Sản phẩm"}
                                    value={this.state.selectedItemName}
                                    getData={item.getDataFunc}
                                    isCreateable={true}
                                    createFuntion={this.createNewItemFunc.bind(this)}
                                    onChange={(value) => {
                                        if (value.item) {
                                            let selectedItemCode = { label: value.item.itemCode, value: value.item.itemID }
                                            this.setState({ selectedItemName: value, selectedItemCode, CurrItem: value.item });
                                        }
                                    }} />
                                {this.validator.message('item', this.state.selectedItemName.value, 'required')}
                            </div>

                            <div className={"childListItems"}>
                                <label >Số lượng </label>
                                <input style={{
                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)', width: 120
                                }}
                                    value={this.state.amount || ''}
                                    onChange={(event) => {
                                        this.amountChange(event.target.value)
                                    }}
                                    type="number" className="form-control" />
                                {this.validator.message("amount", this.state.amount, 'required|min:1')}
                            </div>
                            <div className={"childListItems"}>
                                <label>Đơn vị</label>
                                <input style={{
                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)', width: 120
                                }}
                                    value={this.state.CurrItem ? this.state.CurrItem.itemUnit : ''}
                                    onChange={(event) => {

                                    }}
                                    className="form-control"
                                    disabled={true} />
                            </div>
                            <div className={"childListItems"}>
                                <label >Ghi chú</label>
                                <textarea style={{
                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                    height: 70, width: 260
                                }}
                                    value={this.state.note || ''}
                                    onChange={(event) => {
                                        this.noteChange(event.target.value)
                                    }}
                                />
                            </div>
                            <button type="button" style={{ marginTop: 40, height: 40, width: 75 }} className="btn btn-success"
                                onClick={() => {
                                    this.addItem();
                                }}
                            > Thêm </button>
                        </div>

                        <div className={"addItemWrapper listItemWrapper"}  >
                            {!(item.allowNull === true) &&
                                <label style={{ color: 'red', fontWeight: 'bold' }}>{` ( * ) `}</label>
                            }
                            <div style={{ display: 'table' }}>
                                <div className={'listItemHeader'}  >
                                    <div style={{ minWidth: 100, flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Mã Sản Phẩm</div>
                                    <div style={{ minWidth: 100, flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên Sản Phẩm</div>
                                    <div style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                                    <div style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>ĐVT</div>
                                    <div style={{ minWidth: 200, flex: 8, padding: '5px 10px', }}>Ghi chú</div>
                                    <div style={{ minWidth: 50, flex: 1, padding: 5 }}></div>
                                </div>
                                {listItem && listItem.map((item, index) => {
                                    return (
                                        <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >

                                            <div className="noWrap" style={{ minWidth: 100, flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                                            <div className="noWrap" style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.amount}</div>
                                            <div className="noWrap" style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                                            <div className="noWrap" style={{ minWidth: 200, flex: 8, padding: '5px 10px' }}>{item.note}</div>
                                            <div className="noWrap" style={{ minWidth: 50, flex: 1, padding: 5 }}> <button type="button" title="Xóa" className="btn btn-danger btnAction"
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

                {this.state.createItemModal &&
                    <CreateProductModal
                        createItemModal={this.state.createItemModal}
                        productName={this.state.productName}
                        onCancel={() => { this.setState({ createItemModal: false }) }}
                        onSummit={(item) => {

                            let selectedItemName = {};
                            let selectedItemCode = {};
                            selectedItemCode.value = item.itemID;
                            selectedItemCode.label = item.itemCode;
                            selectedItemName.value = item.itemID;
                            selectedItemName.label = item.itemName;
                            this.setState({ selectedItemName, selectedItemCode, createItemModal: false, CurrItem: item })
                        }}
                    >

                    </CreateProductModal>
                }

            </React.Fragment>
        )
    }
}
