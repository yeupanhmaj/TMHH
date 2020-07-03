import React from 'react';
import { Button, Modal, ModalBody } from 'reactstrap';





export default class ConfirmDialog extends React.PureComponent {
  onConfirm() {
    this.props.onConfirm();
  }
  render() {
    return (
      <div>
        <Modal isOpen={this.props.Modal}>
          <ModalBody style={{
            position: 'relative',
            flex: '1 1 auto',
            padding: '1rem',
            // minWidth: '300px',
            width:'96%',
            minHeight: '300px',
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            fontSize: '12px',
            marginTop:20
          }}>
            {this.props.content}
          </ModalBody>
          <div style={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            fontSize: '12px',
            paddingBottom: '50px',
            marginTop:35
          }}>
            <Button className="btn-danger" style={{ width: '100px', marginLeft: '-30' }} onClick={() => {
              { this.onConfirm() }
            }}>Lưu</Button>{' '}
            <Button className="btn-default" style={{ width: '100px', marginLeft: 30 }} onClick={() => { this.props.onCancel() }}>Tắt</Button>
          </div>
        </Modal>
      </div>
    );
  }
}
