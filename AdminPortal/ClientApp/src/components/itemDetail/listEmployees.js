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
                        Thành Phần:
                        <div className={"addItemWrapper listItemWrapper"}  >
                            <div className={'listItemHeader'}  >
                                <div style={{ flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Tên</div>
                                <div style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>Chức vụ</div>


                            </div>
                            {items.map((item, index) => {
                                return (
                                    <div key={`listItemRow` + index.toString()} className={'listItemRow'}  >
                                        <div className="noWrap" style={{ flex: 5, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.name}</div>
                                        <div className="noWrap" style={{ flex: 3, borderRight: '1px solid #ddd', padding: '5px 10px' }}>{item.roleName}</div>

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
