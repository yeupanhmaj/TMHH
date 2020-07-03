import React from 'react';
import SimpleReactValidator from 'simple-react-validator';

export default class ListItemAcceptance extends React.Component {
    
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

    noteChange(value, index) {
        let items = this.props.items;
        items[index]['explanationNote'] = value
        this.props.onChange(items);
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
                            <div className={"addItemWrapper"}  >
                                <div className={'listItemHeader'}  >
                                    <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>STT</div>
                                    <div style={{ minWidth: 100, flex: 7, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên Sản Phẩm</div>
                                    <div style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                                    <div style={{ minWidth: 200, flex: 5, padding: '5px 10px' }}>Ghi Chú</div>
                                </div>
                                {items && items.map((item, index) => {
                                    return (
                                        <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                            <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{index + 1}</div>
                                            <div style={{ minWidth: 100, flex: 7, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                                            <div className="noWrap" style={{ minWidth: 100, flex: 2, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.amount}</div>
                                            {itemDef.isNotEdited ===true ?
                                                <div className="noWrap" style={{ minWidth: 200, flex: 5, padding: '5px 10px' }}>
                                                    {items[index]['explanationNote'] || ''}
                                                </div>
                                                :
                                                <div style={{ minWidth: 200, flex: 5, padding: '5px 10px' }}>
                                                    <textarea style={{
                                                        fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                                        boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                                        height: 70, width: '100%'
                                                    }}
                                                        value={items[index]['explanationNote'] || ''}
                                                        onChange={(event) => {
                                                            this.noteChange(event.target.value, index)
                                                        }}
                                                    />
                                                </div>
                                            }
                                        </div>
                                    )
                                })}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
