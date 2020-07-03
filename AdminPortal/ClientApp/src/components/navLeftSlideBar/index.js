import React from "react";
import './main.css';
// import TreeView from './treeView'
import NewTree from './newTree';

export default class NavLeftSlideBar extends React.Component {

    render() {
        return (
      
            <div 
            onClick={(e)=>{  e.stopPropagation();}}
            id="slidebarCustom" className={(this.props.isOpen ? 'slideIn' : 'slideOut')}>
                      <div style={{backgroundColor:'white' ,height:'100%'}}>
                <div className={"nav-header " + (this.props.isOpen ? 'slideIn' : 'slideOut')}>
                    <div style={{marginTop:'5px'}} className="logo" >
                        <img src="/images/logo.png" width={60}  alt={""}
                        className={(this.props.isOpen ? 'slideInLogo' : 'slideOutLogo')} />
                    </div>

                </div>
                <div className={"nav-content " + (this.props.isOpen ? 'slideIn' : 'slideOut')}>
                    {this.props.isOpen === true &&
                        <NewTree  />
                    }
                </div>
                </div>
            </div>
            
        );
    }
}
