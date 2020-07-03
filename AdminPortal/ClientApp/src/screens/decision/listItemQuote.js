import React from 'react';
import SimpleReactValidator from 'simple-react-validator';
import * as Utils from '../../libs/util';


export default class ListItemQuote extends React.Component {
    
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
    }

    getTotalAmount() {
        if (this.props.items ===undefined || this.props.items.length ===0)
            return 0
        let total = 0;
        for (let item of this.props.items) {
            if (item['itemPrice']) {
                total += (+(item['itemPrice'].toString().replace(/\./g, '')) * item['amount'])
            }
        }
        total = total;
        return this.formatNumber(total);
    }

    getTotalAmountValue() {
        let total = this.getTotalAmount();
        if (total ===undefined) return 0
        return +(total.toString().replace(/\./g, ''));
    }

    render() {
        let items = this.props.items ;
        let checkWarranty = false;
        if (items !==undefined) {
            for (let item of items) {
                if (item['warrantyYears'] > 0) {
                    checkWarranty = true;
                    break;
                }
            }
        }

        return (
            <div key={"listItems item quote" } >
                < div className="childDetailWarpper" style={{ marginTop: '20px' }}>
                    <div className={"addItemWrapper listItemWrapper"}  >
                        <div style={{ display: 'table' }}>
                            <div className={'listItemHeader'}  >
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>STT</div>
                                <div style={{ minWidth: 100, flex: 4, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên Sản Phẩm</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Đơn vị</div>
                                <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                                {(checkWarranty ) && <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Bảo hành (tháng)</div>}
                                <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Đơn giá (VNĐ)</div>
                                <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Thành tiền (VNĐ)</div>
                  
                            </div>
                            {items && items.map((item, index) => {
                                return (
                                    <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                        <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{index + 1}</div>
                                        <div style={{ minWidth: 100, flex: 4, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemUnit}</div>
                                        <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.amount}</div>
                                            <div>
                                                {checkWarranty && <div className="noWrap" style={{ minWidth: 100, flex: 1, padding: '5px 10px', borderRight: '1px solid #ddd' }}>
                                                    {item.warrantyYears}
                                                </div>}
                                            </div>
              
                                            <div className="noWrap" style={{ minWidth: 100, flex: 2, padding: '5px 10px', borderRight: '1px solid #ddd' }}>
                                                {item.itemPrice ? this.formatNumber(item.itemPrice) : ''}
                                            </div>
                                                                                   
                                        <div className="noWrap" style={{ minWidth: 100, textAlign: 'center', flex: 2, padding: '5px 10px', borderRight: '1px solid #ddd' }}>
                                            {(items[index] !==undefined && items[index]['itemPrice'] !==undefined) ? 
                                            this.formatNumber((items[index]['itemPrice'].toString().replace(/\./g, ''))
                                             *
                                             items[index]['amount']) : 0}
                                        </div>                                                     
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
                            {this.props.VAT ===true && this.props.vatNumber !=undefined &&
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
                                }}>{(this.props.VAT ===true && this.props.vatNumber !==undefined) 
                                    ? this.formatNumber(Math.round(this.getTotalAmountValue() * ((100 + +this.props.vatNumber) / 100))) : this.formatNumber(this.getTotalAmountValue())}</div>
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
