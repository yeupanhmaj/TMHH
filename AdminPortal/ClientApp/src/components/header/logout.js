import * as React from 'react';
import { connect } from 'react-redux';
import * as AuthServices from '../../services/authServices';
import { ApplicationState } from '../../stores';
import * as Global from '../../stores/global';
import { ButtonDropdown, DropdownToggle, DropdownMenu, DropdownItem, Button, Modal, ModalBody } from 'reactstrap';
import SimpleReactValidator from 'simple-react-validator';
import * as Actions from '../../libs/actions';
import * as UserService from '../../services/usersService';



class Logout extends React.PureComponent {
    
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }
        });
        this.state = {
            dropdownOpen: false,
            changePassDialog: false,
            isShow: false,
            txtNewPass: '',
            txtNewPassConfirm: '',
        }
    }

   

    handleLogout() {
        AuthServices.logout();
    }
    getUserName() {
        let userName = ''
        if (this.props.globalVariable && this.props.globalVariable.name) {
            userName = this.props.globalVariable.name;
        }
        return (
            <span>{userName}</span>
        )
    }
    toggle = () => {
        this.setState({ dropdownOpen: !this.state.dropdownOpen })
    }

    openChangePassDialog = () => {
        this.setState({ changePassDialog: true })
    }

    changePass = () => {
        if ( this.validator.allValid() ){
            if(this.state.txtNewPass !==this.state.txtNewPassConfirm){
                Actions.openMessageDialog("lỗi" , "mật khẩu và phần nhập lại kiểm tra không giống nhau");
            }else{
                UserService.changePass(this.state.txtNewPass).then((objRespone) => {
                    if (objRespone.isSuccess ===true) {
                        this.setState({ changePassDialog: false })
                    } else {
                      Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                    }
                  }).catch(err => {
           
                  })
            }
         
        }else{
            this.validator.showMessages();
        }      
    }

    handleNewPassInput(event) {
        let value = event.target.value;
        this.setState({ txtNewPass: value });
    }


    handleNewPassConfirmInput(event) {
        let value = event.target.value;
        this.setState({ txtNewPassConfirm: value });
    }

    render() {
        const { dropdownOpen , isShow} = this.state;


        return (
            <React.Fragment>
                <Modal isOpen={this.state.changePassDialog}>
                    <ModalBody style={{
                        position: 'relative',
                        flex: '1 1 auto',
                        padding: '1rem',
                        // minWidth: '300px',
                        width: '96%',
                        minHeight: '300px',
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        fontSize: '12px',
                        marginTop: 20,

                        flexDirection: 'column'
                    }}>
                        <div key={"newpass"}>
                            <label>{"Mật khẩu mới"} </label>
                            <label style={{ color: 'red', fontWeight: 'bold', marginLeft: '5px' }}>{`( * ) `}</label>

                            <div style={{ display: 'flex' }}>
                                <input autoComplete="on" style={{
                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                    width: '175px'
                                }}
                                    type={`${this.state.isShow ===true ? 'text' : 'password'}`} className="form-control" value={this.state.txtNewPass}
                                    onChange={
                                        this.handleNewPassInput.bind(this)
                                    } />

                                
                        <i style={{ color:isShow==true ? 'orange' : 'black'   , padding: '8px 10px', marginLeft: 5, cursor: 'pointer',
                         border:isShow==true ? '1px solid orange' : '1px solid #ccc' }} className="fa fa-eye" onClick={() => { this.setState({ isShow: !this.state.isShow }) }}></i>
                            </div>
                            {this.validator.message("new pass", this.state.txtNewPass, 'required')}
                        </div>
                        <div key={"newpassConfirm"} style={{marginTop:20}}>
                            <label>{"Nhập lại"} </label>
                            <label style={{ color: 'red', fontWeight: 'bold', marginLeft: '5px' }}>{`( * ) `}</label>

                            <div style={{ display: 'flex' }}>
                                <input autoComplete="on" style={{
                                    fontSize: 12, fontFamily: 'roboto', fontWeight: 400,
                                    boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                                    width: '175px'
                                }}
                                    type={`${this.state.isShow ===true ? 'text' : 'password'}`} className="form-control" value={this.state.txtNewPassConfirm}
                                    onChange={
                                        this.handleNewPassConfirmInput.bind(this)
                                    } />

                             <span style={{width:34}}></span>   
                            </div>
                            {this.validator.message("new pass", this.state.txtNewPassConfirm, 'required')}

                        </div>
                    </ModalBody>
                    <div style={{
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        fontSize: '12px',
                        paddingBottom: '50px',
                        marginTop: 35
                    }}>
                        <Button className="btn-danger" style={{ width: '100px', marginLeft: '-30' }} onClick={() => {
                             this.changePass()
                        }}>Lưu</Button>{' '}
                        <Button className="btn-default" style={{ width: '100px', marginLeft: 30 }} onClick={() => { this.setState({ changePassDialog: false }) }}>Tắt</Button>
                    </div>
                </Modal>

                <ButtonDropdown isOpen={dropdownOpen} toggle={this.toggle.bind(this)}>
                    <DropdownToggle right="true" color="info" caret >

                        {this.getUserName()}

                    </DropdownToggle>
                    <DropdownMenu right>
                        <DropdownItem divider />
                        <DropdownItem color="primary" onClick={this.openChangePassDialog.bind(this)}>Đổi password</DropdownItem>
                        <DropdownItem divider />
                        <DropdownItem color="primary" onClick={this.handleLogout.bind(this)}>Đăng xuất</DropdownItem>
                    </DropdownMenu>
                </ButtonDropdown>
            </React.Fragment>
        )
    }
}

function mapStateToProps(state) {
    return {
        globalVariable: state.global
    }
}

export default connect(mapStateToProps)(Logout);