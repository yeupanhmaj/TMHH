import React from 'react';
import { ItemDetails } from '../../commons/propertiesType';

export default class ItemContent extends React.Component {

    getValue(data ){
        if(data === true) return "Có"
        if(data === false) return "Không"
        if(data === "null") return ""
        return data;
    }
    render() {
        let item = this.props.item ;
        return (
            <div className="childDetailWarpper" style={{ marginTop: '15px' }}>
                {this.props.ItemsDefine.map((itemDef, index) =>
                    itemDef.isFull === true ? (
                        <div key={`details${index}`} style={{ width: 100, flexBasis: '100%' }}>
                            <label>{itemDef.title + ' : '} </label>
                            <div className="DetailsItemFullWidth">
                                <div>{this.getValue(item[itemDef.propName])}</div>
                            </div>
                        </div>
                    )
                        :
                        (
                            <div key={`details${index}`} className="DetailslItemHaftSize">
                                <label>{itemDef.title + ' : '} </label> <b> {this.getValue(item[itemDef.propName])}</b>
                            </div>
                        )

                )}
            </div>
        )
    }
}
