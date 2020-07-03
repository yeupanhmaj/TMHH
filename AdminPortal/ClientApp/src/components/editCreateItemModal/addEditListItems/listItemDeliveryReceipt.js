import React from 'react';
import ListItemDeliveryReceiptC34 from './listItemDeliveryReceiptC34';
import ListItemDeliveryReceiptC50 from './listItemDeliveryReceiptC50';


export default class ListItemDeliveryReceipt extends React.Component {
    this_component;

    getValidator(){
        if(this.this_component)
        return this.this_component.getValidator();
    }
    showMessages(){
         this.this_component.showMessages();
    }
    getBody() {
        if (this.props.type ===undefined) {
            return <div></div>
        } else {
            if (this.props.type ===1) {
                return (<ListItemDeliveryReceiptC34
                    ref={(c) => this.this_component = c}
                    itemDefine={this.props.itemDefine}
                    items={this.props.items}
                    VAT={this.props.VAT}
                    vatNumber={this.props.vatNumber}
                    onChange={this.props.onChange.bind(this)}
                />)
            } else {
                return (<ListItemDeliveryReceiptC50
                    ref={(c) => this.this_component = c}
                    itemDefine={this.props.itemDefine}
                    items={this.props.items}
                    VAT={this.props.VAT}
                    vatNumber={this.props.vatNumber}
                    onChange={this.props.onChange.bind(this)}
                />)
            }

        }
    }
    render() {
        return this.getBody();
    }
}
