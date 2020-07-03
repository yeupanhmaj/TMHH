import * as React from 'react';
import './main.css';


const Loading = (props ) => {
    if(props.show === true){
        if(props.inside ===  true){
            return (
                <div className="lds-wrapper-fit-parent">
                   {/* <div className="lds-heart">
                    

                         </div> */}
                         <div  className="imageLoading" >
                          <img style={{padding:'15px'}} alt={""} src="./images/logo.png"></img>
                        </div>
                        </div>
            )
        }else{
            return (
                <div className="lds-wrapper">
                    {/* <div className="lds-heart"></div> */}
                    <div  className="imageLoading" >
                          <img style={{padding:'15px'}} alt={""} src="./images/logo.png"></img>
                        </div>
                </div>
            )
        }
    }else{
        return (<div style={{display:'none'}}></div>);
    }
}

export default (Loading);
