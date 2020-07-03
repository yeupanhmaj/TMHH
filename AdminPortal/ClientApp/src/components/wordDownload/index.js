import * as React from 'react';
import * as FileUploaderServices from '../../services/fileUploaderServices';

export default class WordDownload extends React.Component{
  
  constructor(props) {
    super(props);
  }
  download(){
    FileUploaderServices.dowloadDoc(this.props.feature, this.props.id)
    .then((response) => {
        response.blob().then((blob) => {
            let url = window.URL.createObjectURL(blob);
            this._hiddenLink.href = url;
            this._hiddenLink.download = this.props.name;
            this._hiddenLink.click();
        })
    })                
  }
  render() {
    
    return (
      <React.Fragment>
      <a style={{ visibility: "hidden" }} ref={(c) => { this._hiddenLink = c }} />
           
      <button style={{ marginRight: 10 }} type="button" className="btn btn-info"
                                        onClick={() => {
                                          this.download();              
                                        }}>   <i style={{ marginRight: '5px' }} className="fa fa-download" aria-hidden="true">
                                          </i>{this.props.buttonName !=undefined ? this.props.buttonName : 'Dowload File Word'} </button>
             
      </React.Fragment>
    );
  }
}

