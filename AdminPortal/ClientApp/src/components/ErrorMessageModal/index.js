import React from 'react';
import { connect } from 'react-redux';
import { Button, Modal, ModalBody, ModalHeader } from 'reactstrap';
import { ApplicationState } from '../../stores';
import * as ModalStore from '../../stores/modals';


class ErrorMessageModal extends React.Component {
 
    onCancel() {
        (this.props )['closeError']();
    }
    render() {
        let errs = [];
        if (this.props.modal && this.props.modal.errContent) {
            errs = this.props.modal.errContent.replace(/"/g, '').split('@@@@@')
        }
        return (
            <Modal isOpen={this.props.modal ? this.props.modal.errModal : false}>
                <ModalHeader style={{ backgroundColor: 'rgba(190,58,65,0.7)', color: 'white' }}>
                    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                        {this.props.modal ? this.props.modal.errHeader : ''}
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
                        {errs.map((line, index) => {
                            return (
                                <div key={'error' + index}>
                                    {line} <br />
                                </div>
                            )
                        })}
                    </div>
                </ModalBody>
                <div style={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    fontSize: '12px',
                    marginBottom: '20px',
                    marginTop: '20px',
                }}>
                    <Button className="btn-primary" style={{ width: '100px', margin: '0 0 0 0' }}
                        onClick={() => { this.onCancel() }}>Tắt</Button>
                </div>
            </Modal>

        );
    }
}
const mapStateToProps = (state) => ({
    modal: state.modal,
});
export default connect(
    mapStateToProps,
    ModalStore.actions
)(ErrorMessageModal );