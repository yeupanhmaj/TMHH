
import * as React from 'react';
import { connect } from 'react-redux';
import { Route } from 'react-router-dom';
import * as AuthServices from '../../services/authServices';
import history from '../../stores/history';



class Authenticated extends Route {
  render(){
    const Comp = this.props.component ;
    let token = localStorage.getItem('token') || '';
    if(token === ''){
    return (
    <React.Fragment>

    </React.Fragment>);
   }else{
     if(Comp !== undefined){
     return (
      <React.Fragment>
          <Comp/>
      </React.Fragment>);
     }else{
      return (
        <React.Fragment>
    
        </React.Fragment>);
     }
   }
  }
  
 static getDerivedStateFromProps(nextProps, prevState)
 {
    try {
      if (nextProps.global != undefined && (nextProps.global.token == '' || nextProps.global.token == undefined)) {
        history.push('login');
      }
    } catch (ex) {
      history.push('login');
    }
    return null
  }
  componentWillMount() {
    let token = localStorage.getItem('token') || '';
    if (token === '') {
      history.push('login');
    }else{
      AuthServices.checkAuth();
    }
     
  }
  componentDidMount() {

  }

}



//   export default connect(
//     (state: ApplicationState["global"]) => state, // Selects which state properties are merged into the component's props
//     Global.actions // Selects which action creators are merged into the component's props
// )(Authenticated);
const mapStateToProps = (state) => ({
  global: state.global,
});

export default connect(
  mapStateToProps
)(Authenticated);