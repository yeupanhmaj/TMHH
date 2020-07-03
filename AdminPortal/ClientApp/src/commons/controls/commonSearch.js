import React from 'react';
import DatePicker from "react-datepicker";
import Select from 'react-select';
import { Button } from 'reactstrap';
import * as CommonService from '../../services/commonService';
import { ISearchParameter } from '../propertiesType';

export default class CommonSearch extends React.Component {
  constructor(props){
      super(props)
      this.state = {
        searchParameter: {
            proposalCode: '',
            department: '',
            fromDate: new Date(`01-01-${(new Date()).getFullYear()}`),
            toDate: new Date()
        },
        lstDepartments: []
    }
  }
   
    

    getSearchParameter() {
        return this.state.searchParameter;
    }

    componentWillMount() {
        CommonService.GetAllDepartment().then(
            result => {
                if (result.isSuccess) {
                    let lstDepartments = []
                    lstDepartments = [{ label: "Tất cả", value: '' }];
                    for (let record of result.data) {
                        let item = { label: record.departmentName, value: record.departmentID };
                        lstDepartments.push(item);
                    }
                    this.setState({ lstDepartments });
                }
            }
        )
    }


    handleSearchInCodeChangeInput(event) {
        let value = event.target.value;
        let name = event.target.name;
        let searchParameter = this.state.searchParameter;
        (searchParameter )[name] = value
        this.setState({ searchParameter })
    }

    handleChangDepartment(value) {
        let searchParameter = this.state.searchParameter 
        searchParameter['department'] = value;
        this.setState(
            { searchParameter }
        );
    };
    handleSearchChangeDateChange(name, date) {
        let searchParameter = this.state.searchParameter;
        (searchParameter )[name] = date;
        this.setState({ searchParameter });
    }
    render() {
        let lstDepartments = this.state.lstDepartments;
        return (

            <div className="searchContainer" >
                <div style={{ width: '100%', height: 'auto', display: 'flex', flexWrap: 'wrap', top: '-10px' }}>
                    <div className="childSearchWrap" >
                        <div style={{ lineHeight: '5px' }}>
                            <label> Mã đề xuất </label>
                        </div>
                        <input style={{
                            fontSize: 14, fontFamily: 'roboto', fontWeight: 400,
                            boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                            border: 'none'
                        }}
                            type="text" className="form-control" name={'proposalCode'} value={this.state.searchParameter.proposalCode} onChange={
                                this.handleSearchInCodeChangeInput.bind(this)
                            } />
                    </div>
                    <div className="childSearchWrap">
                        <div style={{ lineHeight: '5px' }}>
                            <label>Khoa/phòng</label>
                        </div>
                        {lstDepartments !==undefined &&
                            <Select
                                placeholder="Khoa/phòng"
                                styles={customStyles}
                                value={this.state.searchParameter.department}
                                onChange={this.handleChangDepartment.bind(this)}
                                options={lstDepartments}
                            />
                        }
                    </div>

                    <div className="childSearchWrap">
                        <div style={{ lineHeight: '5px' }}>
                            <label>Từ ngày</label>
                        </div>
                        <div style={{
                            fontSize: 12, fontFamily: 'roboto', fontWeight: 400, width: '120px',
                            height: '32px',
                            boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                            border: 'none',
                            display: 'flex',
                            flexDirection: 'row',
                            cursor: 'pointer'
                        }}>
                            <DatePicker
                               className={"datePickerCustom"}
                               dateFormat="dd-MM-yyyy"
                               showMonthDropdown
                               showYearDropdown
                               todayButton="Hôm nay"
                               locale="vi"
                                onChange={this.handleSearchChangeDateChange.bind(this, 'fromDate')}
                                selected={this.state.searchParameter.fromDate }
                                ref={(c) => this._fromDateCalendar = c}
                            />
                            <div style={{ cursor: 'pointer', lineHeight: '34px' }} onClick={() => {
                                this._fromDateCalendar.setOpen(true)
                            }}>
                                <i className="fa fa-calendar" ></i>
                            </div>
                        </div>
                    </div>
                    <div className="childSearchWrap">
                        <div style={{ lineHeight: '5px' }}>
                            <label>Đến ngày</label>
                        </div>
                        <div style={{
                            fontSize: 12, fontFamily: 'roboto', fontWeight: 400, width: '120px',
                            height: '32px',
                            boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
                            border: 'none',
                            display: 'flex',
                            flexDirection: 'row'
                        }}>
                            <DatePicker
                                className={"datePickerCustom"}
                                dateFormat="dd-MM-yyyy"
                                showMonthDropdown
                                showYearDropdown
                                todayButton="Hôm nay"
                                locale="vi"
                                onChange={this.handleSearchChangeDateChange.bind(this, 'toDate')}
                                selected={this.state.searchParameter.toDate }
                                ref={(c) => this._toDateCalendar = c}
                            />
                            <div style={{ cursor: 'pointer', lineHeight: '34px' }} onClick={() => {
                                this._toDateCalendar.setOpen(true)
                            }}>
                                <i className="fa fa-calendar" ></i>
                            </div>
                        </div>
                    </div>
                    <div className="childSearchWrap">
                        <Button className="btn-search" style={{
                            width: '110px',
                            marginTop: '10px',
                            height: '35px'
                        }} onClick={() => {
                            this.props.handelNewSearch()
                        }}> Tìm kiếm
             </Button>
                    </div>
                </div>
            </div>
        );
    }
}
const customStyles = {
    placeholder: () => ({
        margin: 0,
        color: '#aaa'
    }),
    indicatorSeparator: () => ({
        color: '#fff'
    }),

    option: (provided, state) => ({
        ...provided,
        fontSize: 12,
        lineHeight: '12px',
        fontFamily: 'roboto',
        marginTop: 4
    }),
    control: () => ({
        display: 'flex',
        width: 175,
        boxShadow: '0 1px 5px rgba(0, 0, 0, 0.15)',
        height: 32,
        bordeRadius: 3,
        paddingLeft: 5,
        fontSize: 12,
        lineHeight: '12px',
        fontFamily: 'roboto',
    }),
    singleValue: (provided, state) => {
        const opacity = state.isDisabled ? 0.5 : 1;
        const transition = 'opacity 300ms';

        return { ...provided, opacity, transition };
    }
}