import React from 'react';
import Select from 'react-select';
import SimpleReactValidator from 'simple-react-validator';
import { AcceptanceStatusArr, getLabelString } from '../../../commons/propertiesType';

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
        return this.validator;
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


    resultChange(item, index) {

        let items = this.props.items;
        items[index]['acceptanceResult'] = item.value
        this.props.onChange(items);
    }

    getSelectedValue(index) {
        let selectedValue = { value: '', label: '' };
        let items = this.props.items;
        let value = items[index]['acceptanceResult']
       
        let label = getLabelString(value, AcceptanceStatusArr);

        if (value !==undefined) {
            selectedValue.value = value;
            selectedValue.label = label;
        }
        return selectedValue;
    }

    noteChange(value, index) {
        let items = this.props.items;
        items[index]['acceptanceNote'] = value
        this.props.onChange(items);
    }


    render() {
        let itemDef = this.props.itemDefine;
        let name = itemDef.name;
        let items = this.props.items ;

        return (
            <div key={"listItems" + name} >
                < div className="childDetailWarpper" style={{ marginTop: '20px' }}>
                    <div className={"addItemWrapper listItemWrapper"}  style={{overflow:'visible'}} >
                        <div style={{ display: 'table' }}>
                            <div className={"addItemWrapper"}   >

                                <div className={'listItemHeader'}  >
                                    <div style={{ minWidth: 100, flex: 8, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên Sản Phẩm</div>
                                    <div style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                                    <div style={{ minWidth: 100, flex: 3, padding: '5px 10px', borderRight: '1px solid #ddd' }}>Kết quả</div>
                                    
                                </div>
                                {items && items.map((item, index) => {
                                    return (
                                        <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                            <div style={{ minWidth: 100, flex: 8, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                                            <div className="noWrap" style={{ minWidth: 100, flex: 1, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.amount}</div>
                                            {itemDef.isNotEdited ===true ?
                                                <div className="noWrap" style={{ minWidth: 200, flex: 3, padding: '5px 10px', borderRight: '1px solid #ddd' }}>
                                                    {this.getSelectedValue(index).label }
                                                </div>
                                                :

                                                <div className="noWrap" style={{ minWidth: 100, flex: 3, padding: '5px 10px', borderRight: '1px solid #ddd' }}>
                                                    <Select
                                                        placeholder={"kết quả"}
                                                        styles={this.customStyles("100%")}
                                                        value={this.getSelectedValue(index)}
                                                        onChange={(value) => { this.resultChange(value, index) }}
                                                        options={(AcceptanceStatusArr )}
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
