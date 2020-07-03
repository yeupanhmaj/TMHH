import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { withRouter } from 'react-router-dom';
import { listData, MenuRecord } from '../../commons/menuData';
import Logout from './logout';
import './main.css';




const PAGE_NOT_FOUND = 'Page Not Found'
class Header extends React.PureComponent {
    constructor(props){
        super(props);
        this.state = {
            tittle: PAGE_NOT_FOUND,
        }
    }
  
    getNamePage(props) {
        let result = PAGE_NOT_FOUND
        if(props.location.pathname == `/`) return "Report"; 
        for (let node of listData) {
            result = this.getNamePageChild(node, props.location.pathname);
            if (result !== PAGE_NOT_FOUND) {
                return result;
            }
        }
        return result;
    }

    getNamePageChild(node, pathname) {
        let result = PAGE_NOT_FOUND
        if(pathname == `/`) return "Report"; 
        if (node.url === pathname) {
            result = node.label;
            return result;
        }
        if (result === PAGE_NOT_FOUND && node.nodes && node.nodes.length > 0) {
            for (let item of node.nodes){
                result = this.getNamePageChild(item, pathname);
                if(result !== PAGE_NOT_FOUND) return result;
            }
           
        }
        return result;
    }

   
    componentDidMount() {
       
    }



     popup() {
      return 'I see you are leaving the site';
    }


    render() {
        return (
            <div className="header-mita">
                <div className="hoverWheat" style={{
                    fontSize: "18px", cursor: "pointer", display: "flex",
                    marginLeft: 10,
                    color: 'rgb(30,168,247)',
                    flex: 1
                }} onClick={() => this.props.toggle()} >
                    <i className="fa fa-bars fa-lg"></i>
                </div>
                <div className="Header-title-containner">
                 {this.getNamePage(this.props)}
                </div>
                <div className={"logOutIcon"} style={{
                    cursor: 'pointer', display: 'flex', justifyContent: 'flex-end', alignItems: 'center', color: 'white',
                    flex: 1,
                    marginRight: 10,
                }}
                >

                    <Logout />

                </div>
            </div>
        );
    }
}


export default withRouter(Header);
