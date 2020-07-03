import React from "react";
import { withRouter } from 'react-router-dom';
import { listData } from '../../../commons/menuData';
import './main.css';

var role = '';
var name = '';

class NewTree extends React.Component  {
    constructor(props){
        super(props);
        this.state = {
            openNodes: {},
            location: '',
        }
    }
   

    componentWillMount() {
        role = localStorage.getItem('role') || '';
        name = localStorage.getItem('name') || '';
    }

    componentDidMount()
    {
        this.setState({ location: this.props.location.pathname })
    }

    isOpennode(key) {
        let openNodes = this.state.openNodes 
        if (openNodes[key] && (openNodes[key] === true)) {
            return true;
        }
        return false
    }

    getClassOpen(node) {
        if (node.nodes === undefined || node.nodes.length < 1) {
            return (<div></div>)
        }
        let openNodes = this.state.openNodes 
        if (openNodes[node.key] !== undefined && (openNodes[node.key] === true)) {
            return (
                <i key={`fa fa-caret-down` + node.key} style={{ marginLeft: 6, fontSize: 12 }} className={`fa fa-caret-down`}></i>
            )
        } else {
            return (
                <i key={`fa fa-caret-right` + node.key} style={{ marginLeft: 6, fontSize: 12 }} className={`fa fa-caret-right`}></i>
            )
        }
    }

    handleToogle(node) {
        if (node.nodes !== undefined && node.nodes.length > 0) {
            let toogleValue = true;
            let openNodes = this.state.openNodes 
            if (openNodes[node.key] && (openNodes[node.key] === true)) {
                toogleValue = false;
            }
            openNodes[node.key] = toogleValue;
            this.setState({ openNodes });
        } else {
            if (node.url)
            { 
                this.setState({location : node.url})
                this.props.history.push(node.url)
            }
        }
    }

    checkActive(path1, path2) {

        if(path1=='/' && path2=='/report') return true;
        if (path2 === undefined) return false;
        return path1 === path2
    }

    renderNode(node, lvl, index) {
        return (
            <div key={node.key + this.isOpennode(node.key) + this.checkActive(this.state.location, node.url)}>
                <div style={{
                    marginLeft: `${(lvl * 10)}px`,
                    borderLeft: `1px solid rgba(242,242,242,0.8)`
                }}>

                    <div onClick={() => { this.handleToogle(node) }} style={{ paddingLeft: '5px' }}
                        className={`navbtn ${this.checkActive(this.state.location, node.url) ? 'navbtnActive' : ''}`} >
                        {node.icon &&
                            <i style={{ marginRight: 10 }} className={node.icon}></i>
                        }
                        {node.label}
                        <span>
                            {this.getClassOpen(node)}
                        </span>

                    </div>
                </div>
                {this.isOpennode(node.key) === true &&
                    <div key={node.key + 'sub'}>
                        {node.nodes !== undefined && node.nodes.map((data, index) => {
                            if (name === "Administrator") {
                                return this.renderNode(data, lvl + 1, index)
                            } else {
                                if (data.role !== undefined) {
                                    if (role.includes(data.role))
                                        return this.renderNode(data, (lvl + 1), index)
                                } else {
                                    return this.renderNode(data, (lvl + 1), index)
                                }
                            }
                        }
                        )}
                    </div>
                }

            </div>
        )
    }
    
    render() {
        return (
            <div style={{ cursor: 'pointer', margin: '10px 1spx 5px 5px' }}>
                {listData.map((data, index) => {
                    if (name === "Administrator") {
                        return this.renderNode(data, 0, index)
                    } else {
                        if (data.role !== undefined) {
                            if (role.includes(data.role))
                                return this.renderNode(data, 0, index)
                        } else {
                            return this.renderNode(data, 0, index)
                        }
                    }
                }
                )}
            </div>
        );
    }

}
export default withRouter(NewTree);