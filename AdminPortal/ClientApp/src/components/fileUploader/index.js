import * as React from 'react';
import * as Actions from '../../libs/actions';
import * as FileUploaderServices from '../../services/fileUploaderServices';
import "./main.css";
export default class FileUploader extends React.Component {

  constructor(props) {
    super(props);
    this.uploadFile = this.uploadFile.bind(this);
    this.fileChange = this.fileChange.bind(this);
    this.state = {
      files: []
    }
  }
  

  setFile(files){
    this.setState({ files })
  }

  fileChange(event) {
    let files = this.state.files;
    for (let item of event.target.files) {
      if (item.size > 5242880) {
        Actions.openMessageDialog("Error", 'file ' + item.name + ' có dung lượng lớn hơn 5MB');
      } else {
        files.push(item);
      }
    }
    this.setState({ files })
    event.preventDefault();
  }

  getSizeFile(size) {
    if (size < 1024) return `${size} bytes`
    size = size / 1024
    if (size < 1024) return `${Math.round(size)} KB`
    size = size / 1024
    if (size < 1024) return `${Math.round(size)} MB`
    size = size / 1024
    return `${Math.round(size)} GB`
  }
  clearFiles(){
    this._inputfile.value = ''
    this.setState({files:[]})
  }
  removeFile(index) {
    let files = this.state.files;
     files.splice(index,1);
    this.setState({files})
  }

  render() {
    return (
      <div>
        <form onSubmit={this.uploadFile} className="fileUploaderWrapper" encType="multipart/form-data" >
          <div className="leftSideFlieUpload">
            <input style={{ display: 'none' }}
              ref={(c) => this._inputfile = c}
              multiple={true} type="file" name="file" accept=".*" className="upload-file" onChange={this.fileChange} />
            <div className={"controlUploader"} onClick={() => { this._inputfile.click() }} >
              <i className="fa fa-upload" style={{ fontSize: '20px' }}></i>
              Upload Files
           </div>
          </div>
          <div className="rightSideFlieUpload">
            {this.state.files !== undefined && this.state.files.map((file, index) => {
              return (
                <div key={file + index + file.size.toString() + "filesIndex"} className="fileWrapForm">
                  <div style={{ flex: 4 }}>
                    {file.name}
                  </div>
                  <div style={{ flex: 1 }} >
                    {this.getSizeFile(file.size)}
                  </div>
                  <div onClick={() => {
                    this.removeFile(index);
                  }} style={{ flex: 1 }}>
                    <i style={{ flex: 1, maxWidth: '23px', fontSize: '15px', cursor: 'pointer', boxShadow: '0 1px 5px rgba(0, 0, 0, 0.2)' }} className="fa fa-eraser"></i>
                  </div>

                </div>
              )
            })}
          </div>
        </form>
      </div>
    );
  }
  uploadFile() {
    let that = this;
    for (let file of this.state.files) {
      let formData = new FormData();
      formData.append('file', file)

      FileUploaderServices.upload(formData)
        .then(objRespone => {
          if (objRespone.isSuccess === true) {
            Actions.openMessageDialog("Success", 'da up');
          } else {
            Actions.openMessageDialog("Error", objRespone.err.msgString.toString());
            // pop up error
          }
        }).catch(err => {
         
        })
    }
  }

  getFiles() {
      return this.state.files;
  }



};