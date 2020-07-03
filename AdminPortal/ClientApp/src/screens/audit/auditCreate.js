
import MultiSelect from "@kenshooui/react-multi-select";
import "@kenshooui/react-multi-select/dist/style.css";
import React from 'react';
import "react-datepicker/dist/react-datepicker.css";
import { Button } from 'reactstrap';
import InputCustom from '../../commons/controls/input';
import CheckBoxCustom from '../../commons/controlsSimple/checkbox';
import * as Actions from '../../libs/actions';
import * as QuoteService from '../../services/quoteService';






export default class QuoteCreateModal extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            //search
            searchText: '',
    
            quoteList: [],
            selectedQuoteList: [],
    
            isHasAudit: false
        }
    }

     

    componentWillMount() {
        this.handelNewSearch();
    }
    getListQuote() {

        let data = [];
        for (let item of this.state.selectedQuoteList) {
            data.push(item.id);
        }
        return data;
    }
    renderBody() {
        return (
            <div style={{}}>
                <div style={{ width: '100%' }}>
                    {this.renderQuoteSide()}
                </div>
                <div style={{ width: '100%' }}>
                </div>

            </div>
        )
    }


    changeSearchText(value) {
        this.setState({ searchText: value })
    }


    handelNewSearch() {
        
        QuoteService.GetItemWithCondition(
            this.state.searchText,
            this.state.isHasAudit
        )
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    let quoteList = [];

                    if (objRespone.data && objRespone.data.length > 0) {

                        for (let group of objRespone.data) {
                            let temp = {} ;
                            temp.id = group.quoteID;
                            temp.label = group.quoteCode + ' : ' + group.proposalCodes + " : " + group.itemNames;


                            quoteList.push(temp)
                        }
                        this.setState({
                            quoteList: quoteList,
                        })
                    } else {
                        this.setState({
                            quoteList: []
                        })
                    }
                } else {
                    Actions.openMessageDialog("lay data loi", objRespone.err.msgString.toString());
                }
            }).catch(err => {
            })
    }

    renderQuoteSide() {
        return (
            <React.Fragment>
                <div style={{
                    width: '100%',
                    overflow: 'auto', padding: 10, marginTop: 20, marginBottom: 20, border: '1px solid #ccc', borderRadius: 30
                }}>

                    <div style={{ display: 'flex' }}>
                        <div style={{
                            marginLeft: '32px',
                        }}>
                            <InputCustom
                                item={{
                                    header: "Tìm Kiếm",
                                    name: "search",
                                    type: "input",
                                    width: 200,
                                    allowNull: true
                                }}
                                value={this.state.searchText}
                                onChange={(value) => {
                                    this.changeSearchText(value);
                                }} />
                        </div>

                        <CheckBoxCustom
                              style={{marginTop:35,marginLeft:35 , display:'none'}}
                            key={"checkbox ishasquote"}
                            value={this.state.isHasAudit}
                            lable={"Đã có biên bản kiểm giá"}
                            onChange={(value) => {
                                this.setState({ isHasAudit: value });
                            }} />

                        <Button className="btn-search" style={{
                            width: '110px',
                            marginTop: '32px',
                            marginLeft: '32px',
                            height: '35px'
                        }} onClick={() => {
                            this.handelNewSearch()
                        }}> Tìm kiếm
                        </Button>
                    </div>
                    <div style={{ marginTop: '15px' }}>
                        <MultiSelect
                            height={350}
                            responsiveHeight={350}
                            items={this.state.quoteList}
                            selectedItems={this.state.selectedQuoteList}
                            onChange={this.handleChangeQuote.bind(this)}
                        />
                    </div>
                </div>
            </React.Fragment>
        )
    }


    handleChangeQuote(selectedItems) {
        this.setState({ selectedQuoteList: selectedItems });
    }

    onSubmmit() {
        let data = {} ;
        data.quoteList = [];
        for (let item of this.state.selectedQuoteList) {
            data.quoteList.push(item.id);
        }
        this.props.onSubmitSuccess();

    }
    render() {
        return (
            <div>
                {this.props.item &&
                    <div>
                        {this.renderBody()}
                    </div>

                }
            </div>
        );
    }
}