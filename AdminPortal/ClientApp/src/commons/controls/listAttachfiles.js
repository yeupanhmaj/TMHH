import React from 'react';
import SimpleReactValidator from 'simple-react-validator';
import * as FileUploaderServices from '../../services/fileUploaderServices';

export default class ListAttachfiles extends React.Component {
    
    _hiddenLink;
    constructor(props) {
        super(props);
        this.validator = new SimpleReactValidator({
            messages: {
                required: 'vui lòng nhập thông tin'
            }
        });
    }
    removeAttachfile(index) {
        let listDocument = this.props.item  ;     
        listDocument.splice(index, 1)
        this.props.onRemove(listDocument);
      }
    
    
    downloadFile( document) {
        FileUploaderServices.dowload(this.props.feature, document.preferId, document.link, document.type)
          .then((response) => {
            response.blob().then((blob) => {
              let url = window.URL.createObjectURL(blob);
              this._hiddenLink.href = url;
              this._hiddenLink.download = document.fileName;
              this._hiddenLink.click();
            })
          })
      }
    render() {
        let listDocument = this.props.item  ;     
        return (
          <div key={"listattachfile" }>
               <a style={{visibility:"hidden"}} ref={(c) => { this._hiddenLink = c }} />
            {listDocument && listDocument.length > 0 &&
            <div style={{
              display: 'flex', flexDirection: 'row', width: '100%',
              flexWrap: 'wrap'
            }}>
              <div style={{
                width: '100%',
                padding: '5px 0 0px 5px'
              }}>
                Tập tin đính kèm :
        </div>
              {listDocument.map((document, index) =>
                <div key={document.autoID + '' + index} style={{
                  display: 'flex',
                  alignItems: 'center',
                  padding: '5px',
                }}>
                  <div className='linkDownLoad'
                    onClick={() => {
                      if (this.props !==undefined) {
                        this.downloadFile(document)
                      }
                    }}
                  >
                    {document.fileName}
                    <i className="fa fa-download icon-download"></i>
                  </div>
                  {this.props.isShowRemove ===true && ( this.props.isDisable == false || this.props.isDisable == undefined) && 
                  <div style={{
                    padding: '0px 2px 0px 2px',
                    height: '30px',
                    boxShadow: 'rgb(255, 255, 255) 0px 1px inset, rgba(34, 25, 25, 0.4) 0px 1px 3px',
                    margin: '5px 0px 0px 3px',
                    cursor: 'pointer',
                    marginRight: 13,
                    
                  }}>
                    <i className="fa fa-trash icon-trash"
                    
                      onClick={() => { 
                        if(this.props.isDisable == false)
                        this.removeAttachfile(index)
                       }}
                    ></i>
                  </div>
                    }
                </div>
              )}
            </div>
          }
          </div>
        )
    }
}
