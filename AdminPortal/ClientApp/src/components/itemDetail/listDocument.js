import React from 'react';
import * as FileUploaderServices from '../../services/fileUploaderServices';
import * as Actions from '../../libs/actions';

export default class ListDocument extends React.Component {

    downloadFile(referFeatureType, document) {
        FileUploaderServices.dowload(referFeatureType, document.preferId, document.link, document.type)
            .then((response) => {
                response.blob().then((blob) => {
                    let url = window.URL.createObjectURL(blob);
                    this._hiddenLink.href = url;
                    this._hiddenLink.download = document.fileName;
                    this._hiddenLink.click();
                })
            }).catch((err) => {
                Actions.openMessageDialog("lay data loi", err.toString());
            })
    }
    constructor(props) {
        super(props);
    }
    render() {
        let items = this.props.items ;
        return (
            < div className="childDetailWarpper" style={{ marginTop: '15px' }}>
                 <a style={{ display: "none" }} ref={(c) => { this._hiddenLink = c }} />
                {items !==undefined && items.length > 0 &&
                    <div style={{
                        display: 'flex', flexDirection: 'row', width: '100%',
                        flexWrap: 'wrap'
                    }}>
                        <div style={{
                            width: '100%',
                            marginBottom: '-5px',
                            padding: '5px 0 0px 5px'
                        }}>
                            Tập tin đính kèm :
                </div>
                        {items.map((document, index) =>
                            <div key={document.autoID + '' + index} style={{
                                display: 'flex',
                                alignItems: 'center',
                                padding: '5px',
                            }}>
                                <div className='linkDownLoad'
                                    onClick={() => {
                                        if (this.props !==undefined) {
                                            this.downloadFile(this.props.feature, document)
                                        }
                                    }}
                                >
                                    {document.fileName}
                                    <i className="fa fa-download icon-download"></i>
                                </div>
                            </div>
                        )}
                    </div>

                }

            </div>
        )
    }
}
