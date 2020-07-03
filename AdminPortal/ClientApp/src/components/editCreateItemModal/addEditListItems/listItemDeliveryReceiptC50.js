import React from 'react';
import SimpleReactValidator from 'simple-react-validator';
import * as Utils from '../../../libs/util';

export default class ListItemDeliveryReceiptC50 extends React.Component {
    
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }
        });
    }
    getValidator(){
        return this.validator.allValid();
    }
    showMessages(){
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

    dataChange(name, value, index) {
        let items = this.props.items;
        items[index][name] = value
        this.props.onChange(items);
    }
    amountChange(name, value, index) {
        if (isNaN(+(value.replace(/\./g, '')))) return;
        let items = this.props.items;
        let item = items[index];
        item[name] = value
        item.totalPrice = this.toNumber(item['shipCost']) + this.toNumber(item['testCost'])
            + (this.toNumber(item['itemPrice']) * this.toNumber(item['amount']))

        this.props.onChange(items);
    }
    toNumber(value) {
        if (value ===undefined) return 0;
        try {
            return +(value.toString().replace(/\./g, ''))
        } catch (ex) {
            return 0;
        }
    }
    formatNumber(n) {
        try {
            return Utils.formatNumber(n);
        }
        catch (ex) {
            return 0
        }
    }


    getTotalAmount() {
        if (this.props.items ===undefined || this.props.items.length ===0)
            return 0
        let total = 0;
        for (let item of this.props.items) {
            if (item['totalPrice']) {
                total += (+(item['totalPrice']));
            }
        }
        return this.formatNumber(total);
    }
    getTotalAmountValue() {
        let total = this.getTotalAmount();
        if(total ===undefined) return 0
        return +(total.toString().replace(/\./g, ''));
    }

    componentDidMount() {
        //calculate before total render
        let items = this.props.items;
        if (items) {
            for (let item of items) {
                if (item.startUseYear ===undefined) item.startUseYear = (new Date()).getFullYear()
                if (item.totalPrice ===undefined) {
                    if (item['shipCost'] ===undefined) item['shipCost'] = 0;
                    if (item['testCost'] ===undefined) item['testCost'] = 0;
                    if (item['amount'] ===undefined) item['amount'] = 0;
                    if (item['itemPrice'] ===undefined) item['itemPrice'] = 0;
                    item.totalPrice = +item['shipCost'] + +item['testCost'] + (+item['amount'] * +item['itemPrice'])
                }
            }
            this.props.onChange(items)
        }
    }



    render() {
        let itemDef = this.props.itemDefine;
        let name = itemDef.name;
        let items = this.props.items ;
        return (
            <div key={"listItems" + name} >
                < div className="childDetailWarpper" style={{ marginTop: '20px' }}>
                    <div className={"addItemWrapper listItemWrapper"}  >
                        <div style={{ display: 'table' }}>
                            <div className={'listItemHeader'}  >
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>STT</div>
                                <div style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên, ký hiệu quy cách (cấp hạng TSCĐ)</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số hiệu TSCĐ</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Nước sản xuất</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Năm sản xuất</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Năm đưa vào sử dụng</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Đvt</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                                <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Giá mua (ZSX)</div>
                                <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Chi phí vận chuyển</div>
                                <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Chi phí chạy thử</div>
                                <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Nguyên giá TSCĐ</div>
                                <div style={{ minWidth: 200, flex: 2, padding: '5px 10px' }}>TL kỹ thuật kèm theo</div>
                            </div>
                            {items && items.map((item, index) => {
                                return (
                                    <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                        <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{index + 1}</div>
                                        <div style={{ minWidth: 100, flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemCode}</div>

                                        {itemDef.isNotEdited ===true ?
                                            <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                {items[index]['itemManufactureCountry'] || ''}
                                            </div>
                                            :
                                            <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                <input style={{
                                                    textAlign: 'center',
                                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                                    width: '100%'
                                                }}

                                                    value={items[index]['itemManufactureCountry'] || ''}
                                                    onChange={(event) => {
                                                        this.dataChange('itemManufactureCountry', event.target.value, index)
                                                    }}
                                                />
                                            </div>

                                        }
                                        {itemDef.isNotEdited ===true ?
                                            <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                {items[index]['manufactureYear'] || ''}
                                            </div>
                                            :
                                            <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                <input style={{
                                                    textAlign: 'center',
                                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                                    width: '100%'
                                                }}

                                                    value={items[index]['manufactureYear'] || ''}
                                                    onChange={(event) => {
                                                        this.dataChange('manufactureYear', event.target.value, index)
                                                    }}
                                                />
                                            </div>
                                        }
                                        {itemDef.isNotEdited ===true ?
                                            <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                {items[index]['startUseYear'] || ''}
                                            </div>
                                            :
                                            <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                <input style={{
                                                    textAlign: 'center',
                                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                                    width: '100%'
                                                }}

                                                    value={items[index]['startUseYear'] || (new Date()).getFullYear()}
                                                    onChange={(event) => {
                                                        this.dataChange('startUseYear', event.target.value, index)
                                                    }}
                                                />
                                            </div>
                                        }
                                        <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.amount ? this.formatNumber(item.amount) : ''}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemPrice ? this.formatNumber(item.itemPrice) : ''}</div>

                                        {itemDef.isNotEdited ===true ?
                                            <div className="noWrap" style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                {item['shipCost'] ? this.formatNumber(item['shipCost']) : ''}
                                            </div>
                                            :
                                            <div className="noWrap" style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                <input style={{
                                                    textAlign: 'center',
                                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                                    width: '100%'
                                                }}

                                                    value={item['shipCost'] ? this.formatNumber(item['shipCost']) : ''}
                                                    onChange={(event) => {
                                                        this.amountChange('shipCost', event.target.value, index)
                                                    }}
                                                />
                                            </div>
                                        }

                                        {itemDef.isNotEdited ===true ?
                                            <div className="noWrap" style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                {item['testCost'] ? this.formatNumber(item['testCost']) : ''}
                                            </div>
                                            :
                                            <div className="noWrap" style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                <input style={{
                                                    textAlign: 'center',
                                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                                    width: '100%',
                                                }}

                                                    value={item['testCost'] ? this.formatNumber(item['testCost']) : ''}
                                                    onChange={(event) => {
                                                        this.amountChange('testCost', event.target.value, index)
                                                    }}
                                                />
                                            </div>
                                        }

                                        <div className="noWrap" style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}> {item.totalPrice ? this.formatNumber(item.totalPrice) : ''}</div>


                                        {itemDef.isNotEdited ===true ?
                                            <div className="noWrap" style={{minWidth: 200, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                {item['itemDocument'] || ''}
                                            </div>
                                            :
                                            <div style={{ minWidth: 200, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>
                                                <textarea style={{
                                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                                    height: 70, width: '100%'
                                                }}
                                                    value={item['itemDocument'] || ''}
                                                    onChange={(event) => {
                                                        this.dataChange('itemDocument', event.target.value, index)
                                                    }}
                                                />
                                            </div>
                                        }
                                    </div>
                                )
                            })}
                              {this.props.VAT ===true && this.props.vatNumber != undefined &&
                                < div className={'listItemRow'}  >
                                    <div style={{
                                        flex: 10, borderRight: '1px solid #ddd', padding: '5px 63px'
                                        , textAlign: 'end', fontSize: 14, fontWeight: 'bold'
                                    }}>Thuế VAT {this.props.vatNumber ===undefined ? 0 : this.props.vatNumber}%</div>
                                    <div className="noWrap" style={{
                                        textAlign: 'center', fontSize: 14, fontWeight: 'bold', flex: 2,
                                        borderRight: '1px solid #ddd', padding: '5px 10px'
                                    }}>{this.formatNumber(Math.round(this.getTotalAmountValue()))}</div>
                                    <div style={{
                                        flex: 5, paddingLeft: '20px'
                                    }}></div>

                                </div>
                            }
                          {this.props.VAT ===true && this.props.vatNumber != undefined &&
                                <div className={'listItemRow'}  >
                                    <div style={{
                                        flex: 10, borderRight: '1px solid #ddd', padding: '5px 63px'
                                        , textAlign: 'end', fontSize: 14, fontWeight: 'bold'
                                    }}>Thuế VAT 10%</div>
                                    <div className="noWrap" style={{
                                        textAlign: 'center', fontSize: 14, fontWeight: 'bold', flex: 2,
                                        borderRight: '1px solid #ddd', padding: '5px 10px'
                                    }}>{this.formatNumber(Math.round(this.getTotalAmountValue() * this.props.vatNumber / 100))}</div>
                                    <div style={{
                                        flex: 5, paddingLeft: '20px'
                                    }}></div>
                                </div>
                            }
                            <div className={'listItemRow'}  >
                                <div style={{
                                    flex: 10, borderRight: '1px solid #ddd', padding: '5px 63px'
                                    , textAlign: 'end', fontSize: 14, fontWeight: 'bold'
                                }}>Tổng tiền</div>
                                <div className="noWrap" style={{
                                    textAlign: 'center', fontSize: 14, fontWeight: 'bold', flex: 2,
                                    borderRight: '1px solid #ddd', padding: '5px 10px'
                                }}>{(this.props.VAT ===true && this.props.vatNumber !==undefined) ?
                                    this.formatNumber(Math.round(this.getTotalAmountValue() * ((100 + +this.props.vatNumber) / 100))) : this.formatNumber(this.getTotalAmountValue())}</div>
                                <div style={{
                                    flex: 5, paddingLeft: '20px'
                                }}></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
