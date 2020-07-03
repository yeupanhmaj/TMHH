import React from 'react';
import SimpleReactValidator from 'simple-react-validator';
import CreateProductModal from './createProductModal';

export default class ListItemSurvey extends React.Component {

    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }
        });
        this.state = {
            note: '',
            itemAmount: 0,
            itemName: '',
            itemNote: '',
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
        let itemAmount = this.state.itemAmount;
        let itemName = this.state.itemName;
        let itemUnit = this.state.itemUnit;
        let note = this.state.note;

        let valid = true;

        if (itemAmount === undefined || itemAmount < 1) {
            this.validator.showMessageFor('itemAmount');
            valid = false;
        }

        if (itemUnit === undefined) {
            this.validator.showMessageFor('itemUnit');
            valid = false;
        }
        if (valid) {
            let item = {
                note,
                itemAmount,
                itemName,
                itemUnit,
            };
            items.push(item);
            this.setState({ note, itemAmount, itemName, itemUnit })
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

    valueChange(value, name) {

        this.setState({ [name]: value })
    }

    createNewItemFunc(name) {
        this.setState({ createItemModal: true, productName: name })
    }

    render() {
        let item = this.props.itemDefine;
        let name = item.name;
        let listItem = this.props.items;

        return (
            <div>
                <label>{item.header}</label>
                <div key={"listItems" + name} >
                    < div className="childDetailWarpper" style={{ marginTop: '20px' }}>
                        {this.props.isDisabled != true &&
                            <div className="addItemWrapperInner">
                                <div className={"childListItems"}>
                                    <label>Tên sản Phẩm</label>
                                    <input style={{
                                        fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                        boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)', width: 400
                                    }}
                                        value={this.state.itemName || ''}
                                        onChange={(event) => {
                                            this.valueChange(event.target.value, 'itemName');
                                        }}
                                        className="form-control" />
                                    {this.validator.message("itemName", this.state.itemName, 'required|min:1')}
                                </div>
                                <div className={"childListItems"}>
                                    <label>Đơn vị</label>
                                    <input style={{
                                        fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                        boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)', width: 120
                                    }}
                                        value={this.state.itemUnit ? this.state.itemUnit : ''}
                                        onChange={(event) => {
                                            this.valueChange(event.target.value, 'itemUnit');
                                        }}
                                        className="form-control"
                                    />
                                    {this.validator.message("itemUnit", this.state.itemUnit, 'required|min:1')}
                                </div>
                                <div className={"childListItems"}>
                                    <label >Số lượng </label>
                                    <input style={{
                                        fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                        boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)', width: 120
                                    }}
                                        value={this.state.itemAmount || ''}
                                        onChange={(event) => {
                                            this.valueChange(event.target.value, 'itemAmount');
                                        }}
                                        type="number" className="form-control" />
                                    {this.validator.message("itemAmount", this.state.itemAmount, 'required|min:1')}
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
                                            this.valueChange(event.target.value, 'note');
                                        }}
                                    />
                                </div>
                                <button type="button" style={{ marginTop: 40, height: 40, width: 75 }} className="btn btn-success"
                                    onClick={() => {
                                        this.addItem();
                                    }}
                                > Thêm </button>
                            </div>
                        }
                        <div className={"addItemWrapper listItemWrapper"}  >
                            {!(item.allowNull === true) &&
                                <label style={{ color: 'red', fontWeight: 'bold' }}>{` ( * ) `}</label>
                            }
                            <div style={{ display: 'table' }}>
                                <div className={'listItemHeader'}  >

                                    <div style={{ minWidth: 100, flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên Sản Phẩm</div>
                                    <div style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Đơn vị tính</div>
                                    <div style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                                    <div style={{ minWidth: 200, flex: 8, padding: '5px 10px', }}>Ghi chú</div>
                                    <div style={{ minWidth: 50, flex: 1, padding: 5 }}></div>
                                </div>
                                {listItem && listItem.map((item, index) => {
                                    return (
                                        <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                            <div className="noWrap" style={{ minWidth: 100, flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                                            <div className="noWrap" style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                                            <div className="noWrap" style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemAmount}</div>
                                            <div className="noWrap" style={{ minWidth: 200, flex: 8, padding: '5px 10px' }}>{item.note}</div>
                                            <div className="noWrap" style={{ minWidth: 50, flex: 1, padding: 5 }}>
                                                {this.props.isDisabled != true &&
                                                 <button type="button" title="Xóa" className="btn btn-danger btnAction"
                                                onClick={(e) => {
                                                    this.removeGridViewItem(name, index)
                                                }}>
                                                <i className="fa fa-trash">
                                                </i></button>}
                                            </div>
                                        </div>
                                    )
                                })}

                            </div>
                        </div>
                    </div>
                    {this.validator.message("listitem", this.props.items, 'required')}
                </div>
            </div>
        )
    }
}
