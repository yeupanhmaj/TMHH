import React from 'react';
import SimpleReactValidator from 'simple-react-validator';
import * as Utils from '../../../libs/util';

export default class ListItemDeliveryReceiptC34 extends React.Component {
    
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
        items[index][name] = value

        this.props.onChange(items);
    }
    formatNumber(n) {
         return Utils.formatNumber(n);
       // return n.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
    }
    getTotalAmount() {
        if (this.props.items ===undefined || this.props.items.length ===0)
            return 0
        let total = 0;
        for (let item of this.props.items) {
            if (item['totalPrice']) {
                total += (+(item['totalPrice'].toString().replace(/\./g, '')))
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
        let items = this.props.items;
        if (items !==undefined) {
            for (let item of items) {
                item['isSub'] = false;
                if (item.totalPrice ===undefined || item.totalPrice ===0) {
                    if (item['amount'] ===undefined) item['amount'] = 0;
                    if (item['itemPrice'] ===undefined) item['itemPrice'] = 0;
                    item.totalPrice = (+item['amount'] * +item['itemPrice'])
                }
            }
            this.props.onChange(items);
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
                                <div style={{ minWidth: 100, flex: 4, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên Sản Phẩm</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Đơn vị</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Bảo hành (tháng)</div>
                                <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Đơn giá (VNĐ)</div>
                                <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Thành tiền (VNĐ)</div>
                                <div style={{ minWidth: 100, flex: 5, padding: '5px 10px' }}>Ghi Chú</div>
                            </div>
                            {items && items.map((item, index) => {
                                return (
                                    <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                        <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{index + 1}</div>
                                        <div style={{ minWidth: 100, flex: 4, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                                        <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemCode}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.amount}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 2, padding: '5px 10px', borderRight: '1px solid #ddd' }}>
                                            {item.itemPrice ? this.formatNumber(item.itemPrice) : ''}
                                        </div>
                                        <div className="noWrap" style={{ minWidth: 100, textAlign: 'center', flex: 2, padding: '5px 10px', borderRight: '1px solid #ddd' }}>
                                            {item['totalPrice'] !==undefined ? item['totalPrice'] : 0}
                                        </div>
                                        {itemDef.isNotEdited ===true ?
                                            <div className="noWrap" style={{ minWidth: 100, flex: 5, padding: '5px 10px', borderRight: '1px solid #ddd' }}>
                                                {item.quoteNote}
                                            </div>
                                            :
                                            <div style={{ minWidth: 200, flex: 5, padding: '5px 10px' }}>
                                                <textarea style={{
                                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                                    height: 70, width: '100%'
                                                }}
                                                    value={items[index]['deliveryNote'] || ''}
                                                    onChange={(event) => {
                                                        this.dataChange('deliveryNote', event.target.value, index)
                                                    }}
                                                />
                                            </div>
                                        }
                                    </div>
                                )
                            })}
                              {this.props.VAT ===true &&
                                < div className={'listItemRow'}  >
                                    <div style={{
                                        flex: 10, borderRight: '1px solid #ddd', padding: '5px 63px'
                                        , textAlign: 'end', fontSize: 14, fontWeight: 'bold'
                                    }}>Tổng tiền (chưa VAT)</div>
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
                                    }}>Thuế VAT {this.props.vatNumber ===undefined ? 0 : this.props.vatNumber}%</div>
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
                                    this.formatNumber(Math.round(this.getTotalAmountValue() * ((100 + +this.props.vatNumber) / 100))) :  this.formatNumber(this.getTotalAmountValue())}</div>
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
