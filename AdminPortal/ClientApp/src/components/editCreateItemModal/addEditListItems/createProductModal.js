import React from 'react';
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import SimpleReactValidator from 'simple-react-validator';
import AutoCompleteCustom from '../../../commons/controls/autoComplete';
import * as Actions from '../../../libs/actions';
import * as CommonService from '../../../services/commonService';



const temp = [
    { label: 'viên', value: 'viên' },
    { label: 'Con', value: 'Con' },
    { label: 'xấp/50c', value: 'xấp/50c' },
    { label: 'vỉ/2 viên', value: 'vỉ/2 viên' },
    { label: 'miếng', value: 'miếng' },
    { label: 'sợi', value: 'sợi' },
    { label: 'cục', value: 'cục' },
    { label: 'gói', value: 'gói' },
    { label: 'xe', value: 'xe' },
    { label: 'người', value: 'người' },
    { label: 'Bóng', value: 'Bóng' },
    { label: 'năm', value: 'năm' },
    { label: 'Ống', value: 'Ống' },
    { label: 'lọ', value: 'lọ' },
    { label: 'Phiếu', value: 'Phiếu' },
    { label: 'cây', value: 'cây' },
    { label: 'can', value: 'can' },
    { label: 'Phần', value: 'Phần' },
    { label: 'Quyển', value: 'Quyển' },
    { label: 'vỉ', value: 'vỉ' },
    { label: 'Vĩ', value: 'Vĩ' },
    { label: 'bịch', value: 'bịch' },
    { label: 'Thẻ', value: 'Thẻ' },
    { label: 'Chiếc', value: 'Chiếc' },
    { label: 'tờ', value: 'tờ' },
    { label: 'đôi', value: 'đôi' },
    { label: 'cuộc', value: 'cuộc' },
    { label: 'bộ', value: 'bộ' },
    { label: 'xấp', value: 'xấp' },
    { label: 'tuýt', value: 'tuýt' },
    { label: 'bình', value: 'bình' },
    { label: 'mét', value: 'mét' },
    { label: 'máy', value: 'máy' },
    { label: 'cái', value: 'cái' },
    { label: 'thùng', value: 'thùng' },
    { label: 'tube', value: 'tube' },
    { label: 'thang', value: 'thang' },
    { label: 'chai ', value: 'chai ' },
    { label: 'cặp', value: 'cặp' },
    { label: 'lít', value: 'lít' },
    { label: 'hộp', value: 'hộp' },
    { label: 'Cía', value: 'Cía' },
    { label: 'kg', value: 'kg' },
    { label: 'viỉ', value: 'viỉ' },
    { label: 'tấm', value: 'tấm' },
    { label: 'cuộn', value: 'cuộn' },
    { label: 'cuốn', value: 'cuốn' },
    { label: 'ht', value: 'ht' },
    { label: 'Ram', value: 'Ram' },
    { label: 'Lon', value: 'Lon' },
    { label: 'Bao', value: 'Bao' }
]
export default class CreateProductModal extends React.Component {
    
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }

        });
        this.state = {
            itemName: '' ,
            itemCode: '' ,
            itemUnit: { label: '', value: '' },
            arrayUnit: [],
        }
    }
  

    componentWillMount() {
        this.setState({ arrayUnit: temp,itemName:this.props.productName })
    }

    handleChangeItemCode(value) {
        this.setState({ itemCode: value.target.value })
    }

    handleChangeItemName(value) {
        this.setState({ itemName: value.target.value })
    }
    onSubmmit() {
        let data = {} ;
        data.itemCode = this.state.itemCode;
        data.itemName = this.state.itemName;
        data.itemUnit = this.state.itemUnit.value;
        CommonService.creatItem(data).then((response) => {
            if (response.isSuccess === true) {
                this.props.onSummit(response.item)            
              } else {
                Actions.openMessageDialog("lay data loi", response.err.msgString.toString());
              }
        })
    
    }

    render() {
        return (
            <React.Fragment>
                {/*  create item modal */}
                {this.props.createItemModal === true &&
                    <Modal isOpen={this.props.createItemModal}>
                        <ModalHeader >
                            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                Tạo mới sản phẩm
                            </div>
                        </ModalHeader>
                        <ModalBody style={{
                            position: 'relative',
                            flex: '1 1 auto',
                            minWidth: '600px',
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            fontSize: '12px',
                            marginTop: '15px'
                        }}>
                            <div style={{
                                boxShadow: '2px 2px 2px 2px rgba(190,66,75,0.4)',
                                padding: '50px 100px 40px 100px'
                            }}>
                                <div key={'itemCode create'}>
                                    <label>Mã sản phẩm</label>
                                    <label style={{ color: 'red', fontWeight: 'bold', marginLeft: '5px' }}>{`( * ) `}</label>
                                    <div style={{ display: 'flex' }}>
                                        <input autoComplete="on" style={{
                                            fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                            boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                            width: '175px'
                                        }}
                                            type={'text'} className="form-control" value={this.state.itemCode}
                                            onChange={
                                                this.handleChangeItemCode.bind(this)
                                            } />
                                    </div>
                                </div>

                                <div key={'itemName create'}>
                                    <label>Tên Sản phẩm</label>
                                    <label style={{ color: 'red', fontWeight: 'bold', marginLeft: '5px' }}>{`( * ) `}</label>
                                    <div style={{ display: 'flex' }}>
                                        <input autoComplete="on" style={{
                                            fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                            boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                            width: '175px'
                                        }}
                                            type={'text'} className="form-control" value={this.state.itemName} onChange={
                                                this.handleChangeItemName.bind(this)
                                            } />
                                    </div>
                                </div>

                                <div key={'itemUnit create'}>
                                    <AutoCompleteCustom
                                        valueCol={"itemID"}
                                        labelCol={"itemName"}
                                        name={"product"}
                                        header={"Đơn vị"}
                                        value={this.state.itemUnit}
                                        values={this.state.arrayUnit}
                                        getData={undefined}
                                        isCreateable={true}
                                        createFuntion={(value) => {
                                            let arrayUnit = this.state.arrayUnit;
                                            arrayUnit.push({ label: value, value: value })
                                            this.setState({ arrayUnit, itemUnit: { label: value, value: value } })
                                        }}
                                        onChange={(value) => {
                                            this.setState({ itemUnit: value })
                                        }} />
                                    {this.validator.message('item', this.state.itemUnit.value, 'required')}
                                </div>

                            </div>
                        </ModalBody>
                        <div style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            fontSize: '12px',
                            marginBottom: '30px',
                            marginTop: '20px'
                        }}>
                            <Button className="btn-danger" style={{ width: '100px', marginLeft: '-30' }} onClick={() => {
                                { this.onSubmmit() }
                            }}>Lưu</Button>{' '}
                            <Button className="btn-default" style={{ width: '100px', marginLeft: 30 }} onClick={() => { this.props.onCancel() }}>Tắt</Button>
                        </div>
                    </Modal>
                }
            </React.Fragment>
        )
    }
}
