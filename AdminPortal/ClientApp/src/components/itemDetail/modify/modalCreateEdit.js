import React from 'react';
import { CONST_FEATURE } from '../../../commons';
import EditCreateItemModal from '../../editCreateItemModal';
import * as COMMON from '.';

export default class ModalCreateEdit extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            confirmModal: false,
            selectedItem: undefined,
            confirmContent: '',
            headerEditModal: '',
            localItem: {} ,
            itemDefines: undefined ,
        };
    }


    componentWillMount() {

 
    }

    refreshItemDefines(itemDefines) {
        this.setState({ itemDefines })
    }
    refreshItem(selectedItem) {
        this.setState({ selectedItem })
    }

    getCurrentItem() {
        return this._comp.getCurrentItem()
    }

    componentDidMount() {
        let temp = COMMON.CONST_COMMON ;
        if (temp[this.props.feature].init)
            temp[this.props.feature].init(this.refreshItemDefines.bind(this),
                this.refreshItem.bind(this), this.props.proposalCode, this.props.listCurrentData, this.props.item
                , this.getCurrentItem.bind(this))

    }
    clearItem() {
        this.setState({ selectedItem: undefined });
    }

   

    onModalSubmmitWithAttachFile(item, files) {
        let func = COMMON.CONST_COMMON ;
        func[this.props.feature].func(item, files, this.props.onSuccess)
    }


    render() {
        let feature = this.props.feature
        let temp = CONST_FEATURE 
        return (
            <React.Fragment>
                    < EditCreateItemModal
                        ref={(c) => this._comp = c}
                        item={this.state.selectedItem}
                        Modal={this.props.itemModal}
                        itemDefines={this.state.itemDefines}
                        onSubmmitWithAttachFile={this.onModalSubmmitWithAttachFile.bind(this)}
                        onCancel={() => { this.props.onClose() }}
                        headerName={this.state.headerEditModal}
                        keyColumn={temp[feature].KEY_COLUMN}
                        isHasUploader={true}
                        referFeatureType={feature}
                    ></EditCreateItemModal>
            </React.Fragment>

        );
    }
}
