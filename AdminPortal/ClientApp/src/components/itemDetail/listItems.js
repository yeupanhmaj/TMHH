import React from 'react';

export default class ListItems extends React.Component {


    constructor(props) {
        super(props);
    }
    render() {
        let items = this.props.items;

        return (
            <div>
                {items !==undefined &&
                    < div className="childDetailWarpper" style={{ marginTop: '20px' }}>
                        <div className={"addItemWrapper listItemWrapper"}  >
                            <div className={'listItemHeader'}  >
                            <div style={{ flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Mã Sản Phẩm</div>
                                <div style={{ flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên Sản Phẩm</div>
                             
                                <div style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Số lượng</div>
                                <div style={{ flex: 8, padding: '5px 10px' }}>Ghi chú</div>

                            </div>
                            {items.map((item, index) => {
                                return (
                                    <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                        <div className="noWrap" style={{ flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemCode}</div>
                                        <div className="noWrap" style={{ flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.itemName}</div>
                                        
                                        <div className="noWrap" style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.amount}</div>
                                        <div className="noWrap" style={{ flex: 8, padding: '5px 10px' }}>{item.description}</div>
                                    </div>
                                )
                            })}
                        </div>
                    </div>
                }
            </div>
        )
    }
}
