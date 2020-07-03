import * as React from 'react';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import PrintTemplate from '../../components/printTemplate';
import * as Actions from '../../libs/actions';
import ErrorMessageModal from '../ErrorMessageModal';
import Header from '../header';
import NavLeftSlideBar from '../navLeftSlideBar';
import './main.css';
import Loading from '../loading';

class Layout extends React.PureComponent {
    constructor(props){
        super(props);
        this.state = {
            openNav: false
        }
    }
  
    componentWillMount() {

        if(window.onafterprint == undefined){
            window.onafterprint = this.afterPrint.bind(this);
   
        }
    }

    afterPrint(){
        setTimeout(() => {
            Actions.closePrintDialog();
        }, 1000);
  
    }
    handleToggle() {
        this.setState({ openNav: !this.state.openNav })
    }
    isFullPage() {
        if (this.props.location.pathname.includes('/login')) {
            return true;
        }
        return false;
    }
  
    render() {
        return (
            <React.Fragment>
                <Loading show={this.props.loadingModal}/>
                <ErrorMessageModal/>
                <div className="container-layout notprint">
                <div className="padding-class   notprint">
                    {this.isFullPage() === false ?
                        <div className="wrapper-layout-mita">
                            <div className="left-wrapper-mita" style={{width: `${this.state.openNav == true ? '100%' : 'auto' }`
                            , backgroundColor:`${this.state.openNav == true ?  
                            'rgba(230,230,230,0.6)' : 'rgba(20,20,20,0.0)'}` }} 
                            onClick={()=>{this.setState({openNav :!this.state.openNav})}}>
                               
                                <NavLeftSlideBar
                                    isOpen={this.state.openNav} />
                           
                            </div>
                            <div className="right-wrapper-mita">
                                <Header toggle={this.handleToggle.bind(this)} />
                                <div className="content-mita">
                                    {this.props.children}
                                </div>
                            </div>
                        </div>
                        :
                        <div>
                            <div style={{
                                height: '100vh', width: '100%', display: 'flex', justifyContent: 'center',
                                alignItems: 'center' 
                            }} className="printer-container-page">
                                {this.props.children}
                            </div>
                        </div>

                    }
                     </div>
                </div>

                {this.props.print  && this.props.print.printModal === true && 
                    <div style={{ position: 'absolute', top: 0, left: 0 }}>
                        <PrintTemplate id={this.props.print.currentID} feature={this.props.print.feature}></PrintTemplate>
                    </div>
                }
            </React.Fragment>
        );
    }
}
const mapStateToProps = (state) =>{

    return ({
    print: state.print,
    loadingModal: state.modal.loadingModal
  })};

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
    }, dispatch);
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Layout))