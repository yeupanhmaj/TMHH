import React from 'react';
import FileUploader from '../../components/fileUploader';
import moment from 'moment';
import * as Actions from '../../libs/actions';
import { Button, Modal } from 'reactstrap';
import ListDocument from './listDocument'
import * as CommentService from '../../services/commentService';

export default class ListComment extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            isShowFileUploader: false,
            textComment: '',
        }
    }

    

    onPostComment() {
        var files = this._fileUploader.getFiles();
        let itemId = this.props.preferId ;
        var data = new FormData();
        if (files.length > 0) {
            for (let file of files) {
                data.append('files', file);
            }
        }
        
        data.append('feature', this.props.feature);
        data.append('preferId', itemId.toString());
        data.append('comment', this.state.textComment);
        CommentService.addComment(data).then(objRespone => {
            if (objRespone.isSuccess ===true) {
                this.setState({
                    itemModal: false
                })
                this.props.onRefreshAfterPost();
                this.clearData();
            } else {
                Actions.openMessageDialog("Error " + objRespone.err.msgCode, objRespone.err.msgString.toString());
                this.clearData();
            }
        }).catch((err) => {
          this.clearData();
        })
      
       
    }

    clearData(){
        this.setState({  isShowFileUploader: false,
            textComment: '',})
            this._fileUploader.clearFiles()
    }

    handleChangeTextArea(event) {
        let value = event.target.value;
        this.setState({ textComment: value });
    }


    render() {
        let items = this.props.items ;


        return (
            <div>
                <div className="commentInputWrapper">
                    <div style={{ position: 'relative' }}>
                        <div style={{ position: 'absolute', zIndex: 3, left: '0px', right: '0px' }}
                            className={`${this.state.isShowFileUploader ===true ? '' : 'displayNone'}`}>
                            <FileUploader ref={(c) => { this._fileUploader = c }} />
                        </div>

                        <div className={`${this.state.isShowFileUploader ===false ? '' : 'invisible'}`}>
                            <textarea className="comment-input"
                                name="textComment"
                                value={this.state.textComment}
                                onChange={
                                    this.handleChangeTextArea.bind(this)
                                }
                                placeholder="Ghi chú"
                                style={{
                                    padding: '0px 0px 0px 10px',
                                    overflow: "hidden visible",
                                    overflowWrap: "break-word",
                                    width: '100%',
                                    marginTop: '20px',
                                    border: '1px solid #ddd',
                                    height: '139px',
                                }}>
                            </textarea>
                        </div>
                        <div style={{ position: 'absolute', zIndex: 4, backgroundColor: 'white', bottom: '10px', right: '10px', }}
                            onClick={() => { this.setState({ isShowFileUploader: !this.state.isShowFileUploader }) }}>
                            <i className={`fa fa-paperclip ${this.state.isShowFileUploader ===true ? 'attFileIconActive' : 'attFileIconInActive'}`} aria-hidden="true"></i>
                        </div>
                    </div>
                    <Button className="btn-info" style={{ width: '140px', marginTop: '10px' }} onClick={() => {
                        { this.onPostComment() }
                    }}>Gửi ghi chú</Button>{' '}
                </div>

                <div className="commentListWrapper">
                    {items.length > 0 &&
                        < div className="commentHeader">
                            Ghi Chú
                        </div>
                    }
                    {items.map((comment, index) =>
                        <div className="rowComment" key={comment.intime}>
                            <div className="leftRowComment">
                                <div className="thumbnail">
                                    <img className="img-responsive" src={require("../../commons/images/user-avatar-placeholder.png")}/>
                                    <div className="text-center">{comment.userName}</div>
                                </div>
                            </div>
                            <div className="rightRowComment">
                                <div className="panel panel-default arrow-left">
                                    <div className="panel-body">
                                        <header className="text-left">
                                            <div className="comment-user"><i className="fa fa-user"></i>{comment.userName}</div>
                                            <div className="comment-date" ><i className="fa fa-clock-o"></i>
                                                {` ` + moment(new Date(comment.intime)).format('DD-MM-YYYY : hh-mm')}</div>
                                        </header>
                                        <div className="comment-post">
                                            <p>
                                                {comment.comment}
                                            </p>
                                            {comment.listDocument !==undefined &&
                                                <ListDocument items={comment.listDocument} feature={"comment"}></ListDocument>
                                            }
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        )
    }
}
