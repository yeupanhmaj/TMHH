import * as React from 'react';
import { connect } from 'react-redux';
import SimpleReactValidator from 'simple-react-validator';
import * as AuthServices from '../../services/authServices';
import history from '../../stores/history';

class Login extends React.Component {   
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator();
        this.state = {
            username: "",
            password: "",
            submitted: false
        };
    }

 
     handleSubmit() {
        const { username, password } = this.state;
        if (this.validator.allValid()) {
            AuthServices.login(username, password).then((objRespone) =>{
                if (objRespone.isSuccess ===true) {
                    if (this.props.location && this.props.location.state) {
                        const { from } = this.props.location.state;
                        if (from !== '' && from !== undefined) {
                            history.push(from);
                        } else {
                            history.push('/');
                        }
                    } else {
                        history.push('/');
                    }
                }
            })
        } else {
            this.validator.showMessages();
            // rerender to show messages for the first time
            // you can use the autoForceUpdate option to do this automatically`
            this.forceUpdate();
        }

    }
     handleChangeUserName(event) {
        let value = event.target.value;
        this.setState({ username: value });
    }
     handleChangePassword(event) {
        let value = event.target.value;
        this.setState({ password: value });
    }
    
    componentDidUpdate(prevProps){
        this.checkprops(this.props);
    }
   

    componentDidMount() {
        this.checkprops(this.props);
    }

    checkprops(props){
        if (props === undefined || props.global === undefined || props.global.token === undefined) return
        if (props && props.global.token !== '') {
            if (props.location && props.location.state) {
                const { from } = props.location.state;
                if (from !== '' && from !== undefined) {
                    history.push(from);
                } else {
                    history.push('/');
                }
            } else {
                history.push('/');
            }
        }    
    }

    render() {
        //const { loggingIn } = this.props;
        const { username, password, submitted } = this.state;
        return (
            <div style={{
                display: 'flex', width: '600px', borderRadius: "20px", backgroundColor: 'rgba(255,255,255,0.3)',
                justifyContent: 'center',
                borderBottom: '0 none',
                boxShadow: '0 1px 5px rgba(0, 0, 0, 0.46)',
                alignItems: 'center'
            }}>
                <div style={{ display: 'flex', flexDirection: 'column', width: '100%', padding: 20 }}>
                    <h2 style={{
                        fontSize: 25,
                        alignSelf: 'center'
                    }}>ĐĂNG NHẬP</h2>
                    <div className="logo" style={{ alignSelf: 'center', margin: '20px 0px 20px 0px' }}>
                        <img src="/images/logo.png" width={120} />
                    </div>
                    <div style={{ alignSelf: 'center', padding: 10, width: '80%' }} className={'form-group' + (submitted && !username ? ' has-error' : '')}>
                        <label style={{ fontSize: 12, fontWeight: 'bold' }} htmlFor="username">Username</label>
                        <input style={{ boxShadow: '2px 2px 3px rgba(0,0,0,0.7)' }}
                            type="text" className="form-control" name="username" value={username} onChange={this.handleChangeUserName.bind(this)} />
                        {this.validator.message('username', this.state.username, 'required')}
                    </div>
                    <div style={{ alignSelf: 'center', padding: 10, width: '80%' }} className={'form-group' + (submitted && !password ? ' has-error' : '')}>
                        <label style={{ fontSize: 12, fontWeight: 'bold' }} htmlFor="password">Password</label>
                        <input style={{ boxShadow: '2px 2px 3px rgba(0,0,0,0.7)' }}
                            type="password" className="form-control" name="password" value={password} onChange={this.handleChangePassword.bind(this)}
                            onKeyDown={(e) => {
                                if (e.key === 'Enter') {
                                    this.handleSubmit();
                                }
                            }} />
                        {this.validator.message('password', this.state.password, 'required')}
                    </div>
                    <div style={{
                        alignSelf: 'center', width: '80%', padding: 20,
                        display: 'flex',
                        justifyContent: 'center'
                    }} className="form-group" >

                      
                        <button style={{ width: '90px' }} className="btn btn-primary" onClick={this.handleSubmit.bind(this)} > Login</button>
                    </div>
                </div>
            </div>
        );
    }
};


function mapStateToProps(state, props) {
    return {
        global: state.global
    };
  }
  


export default connect(
    mapStateToProps
)(Login)
