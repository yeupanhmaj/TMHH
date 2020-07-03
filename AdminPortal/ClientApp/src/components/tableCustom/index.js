import * as React from 'react';
import { PaginationItem, PaginationLink } from 'reactstrap';
import { ColumnHeader } from '../../commons/propertiesType';
import './main.css';


export default class TableCustom extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isModifyAll: false,
      currentPage: 1,
      pager: {
        totalPages: 0,
        startPage: 0,
        endPage: 0,
        pages: [],
      },
      selectedRecords: [],
      selectedAllRecord: false,
      pageSize: 10
    }
  }


  componentWillReceiveProps(props) {
    if (props.dataSource && props.dataSource.length > 0) {
      this.setState({ pager: this.getPager(this.state.currentPage, this.state.pageSize, props.totalRecords) });
    } else {
      this.setState({ pager: this.getPager(0, 10, 0) });
    }
    if (JSON.stringify(props.dataSource) !== JSON.stringify(this.props.dataSource)) {
      this.setState({ selectedRecords: new Array(props.dataSource.length).fill(false) });
    }
  }

  componentDidMount() {
    let isModifyAll = this.props.isModifyAll || false;
    this.setState({ isModifyAll });

  }

  isDisable() {
    if (this.props.dataSource === undefined) return true;
    if (this.props.dataSource.length === 0) {
      return true;
    }
    return false;
  }

  isDisableDeleteAll() {
    for (let item of this.state.selectedRecords) {
      if (item) return false;
    }

    return true;
  }
  resetPageIndex() {
    this.setState({ currentPage: 1 })
  }
  HandleSeletedAll() {
    let value = !this.state.selectedAllRecord;
    let selectedRecords = [];
    if (value === true) {
      selectedRecords = new Array(this.props.dataSource.length).fill(true)
    } else {
      selectedRecords = new Array(this.props.dataSource.length).fill(false)
    }
    this.setState({ selectedAllRecord: value, selectedRecords });
  }
  handelSelectRow(index) {
    let selectedRecords = this.state.selectedRecords;
    selectedRecords[index] = !this.state.selectedRecords[index]
    this.setState({ selectedRecords });
  }
  changePageSize(event) {
    // No longer need to cast to any - hooray for react!
    var safeSearchTypeValue = +event.currentTarget.value;
    if (this.props.onChangePage) {
      this.props.onChangePage(1, safeSearchTypeValue);
    }
    this.setState({ pageSize: safeSearchTypeValue, currentPage: 1 })
  }

  renderTable() {
    const { selectedRecords } = this.state;
    if (this.props === null || this.props.columns === null) {
      return (<div>
      </div>
      );
    }
    return (
      <div className="wrap-table100">
        <div style={{
          display: 'flex', justifyContent: 'flex-end',
          height: 35,
          marginBottom: 10
        }}>

          <div style={{
            display: 'flex', justifyContent: 'flex-end',
          }}>
            <button style={{ marginRight: 20 }} type="button" className="btn btn-info pull-left" onClick={() => {
              if (this.props.onEditAction) this.props.onEditAction({}, "insert");
            }}>
              Tạo mới
            </button>

            <button type="button" className="btn btn-danger pull-left" disabled={this.isDisableDeleteAll()} onClick={() => {
              if (this.props.onEditAction) {
                let para = [];

                for (let i = 0; i < this.state.selectedRecords.length; i++) {
                  if (this.state.selectedRecords) {
                    let item = this.props.dataSource[i] 
                    let id = (item[this.props.keyColumn])
                    if (this.state.selectedRecords[i]  === true)
                      para.push(id);
                  }
                }
                this.props.onEditAction(para, "deleteRecords");
              }
            }}>
              Xóa tất cả
        </button>
          </div>
        </div>
        <div className="warrperScrollTable" style={{ overflow: 'auto' }}>
          <table className='table  table-bordered' cellPadding="0" cellSpacing="0" aria-labelledby="tabelLabel">
            <thead>

              <tr className="row header" key={this.props.keyColumn + 'header'}>
                {this.state.isModifyAll &&
                  <th className="nowrap checkboxcustom" style={{ width: 20 }} onClick={this.HandleSeletedAll.bind(this)}>
                    <div className="container-checkbox check-box-header" style={{ width: 20 }}>
                      <input type="checkbox" disabled={this.isDisable()} checked={this.state.selectedAllRecord} onChange={() => { }}
                      />
                      <span className="checkmark"></span>
                    </div>
                  </th>
                }
                {this.props.columns.map((columHeader) => 
                {
                   if(columHeader.hidden != true) 
                   {return(
                  <th className="nowrap"   key={columHeader.columnId} >{columHeader.columnName}</th>
                   )
                   }else{
                    return(
                      <th className="nowrap" style={{display:'none'}}  key={columHeader.columnId} >{columHeader.columnName}</th>
                       )
                   }
                }
                  )}

                {this.state.isModifyAll &&
                  <th className="nowrap" style={{ width: "85px", minWidth: "85px" }} key={"TableActions"}>Action</th>
                }
              </tr>
            </thead>
            <tbody>
              {this.props.dataSource.map((record, index) => {
              
               
                return (
                <tr key={record[this.props.keyColumn] + index.toString() + "itemrow"} className="rowItem" 
                //style={{ cursor: 'pointer' }}
                //  onClick={() => {
                //   if (this.props.onViewAction) {
                   
                //   this.props.onViewAction(record[this.props.keyColumn])
                //   }
                // }}
                >
                  {this.state.isModifyAll &&
                    <td className="nowrap checkboxcustom" style={{ width: 20 }} onChange={() => { }} onClick={(e) => {
                      this.handelSelectRow(index)
                      if (!e) {
                        if (e.stopPropagation) e.stopPropagation();
                      } else {
                        var e2 = window.event;
                        if (e2) {
                          e2.cancelBubble = true;
                          if (e2.stopPropagation !== undefined) e.stopPropagation();
                        }
                      }
                    }} key={index + "itemrowCheckBox"}>
                      <div className="container-checkbox" style={{ width: 20, pointerEvents: 'none' }}>
                        <input type="checkbox" checked={selectedRecords[index]} onChange={() => { }} />
                        <span className="checkmark"></span>
                      </div>
                    </td>
                  }
                  {this.props.columns.map((columHeader, index) =>{
                     if(this.props.columns[index].hidden !=true ) {
                       return (
                    <td style={{width:`${columHeader.width ? columHeader.width : 'auto'}`}} className="nowrap" key={record[this.props.keyColumn] + record[columHeader.columnId] + index.toString()}>
                      {record[columHeader.columnId] === "null" ? '' : record[columHeader.columnId]}</td>
                     )
                    }else{
                      return (
                        <td style={{display:'none'}} className="nowrap" key={record[this.props.keyColumn] + record[columHeader.columnId] + index.toString()}>
                          {record[columHeader.columnId] === "null" ? '' : record[columHeader.columnId]}</td>
                         )
                     }
                  })}

                  {this.state.isModifyAll &&
                    <td className="nowrap" style={{ maxWidth: "120px" }} key={index + "itemrowACtion"}>
                      {this.props.onViewAction &&

                        <button type="button" title="Chi tiết" className="btn btn-search btnAction" onClick={() => {
                          if (this.props.onViewAction) this.props.onViewAction(record[this.props.keyColumn])
                        }}>

                          <i className="fa fa-eye">
                          </i></button>
                      }

                      {this.props.onEditAction &&

                        <button type="button" title="Chỉnh sửa" className="btn btn-info btnAction" onClick={(e ) => {
                        if (this.props.onEditAction) this.props.onEditAction(JSON.parse(JSON.stringify(record)), "edit");
                        if (!e) {
                          if (e.stopPropagation) e.stopPropagation();
                        } else {
                          var e2 = window.event;
                          if (e2) {
                            e2.cancelBubble = true;
                            if (e2.stopPropagation !== undefined) e.stopPropagation();
                          }
                        }
                      }}>
                        
                        <i className="fa fa-edit">
                        </i></button>
                      }
                      {this.props.onEditAction &&
                        <button type="button" title="Xóa" className="btn btn-danger btnAction" onClick={(e) => {
                          if (this.props.onEditAction) {
                            let para = record[this.props.keyColumn];
                            this.props.onEditAction(para, "delete");
                          }
                          if (!e) {
                            if (e.stopPropagation) e.stopPropagation();
                          } else {
                            var e2 = window.event;
                            if (e2) {
                              e2.cancelBubble = true;
                              if (e2.stopPropagation !== undefined) e.stopPropagation();
                            }
                          }
                        }}>
                          <i className="fa fa-trash">
                          </i></button>
                        }
                    </td>
                  }
                </tr>
              )
            }
              )}

            </tbody>
          </table>
        </div>
        {this.props.dataSource.length === 0 &&
          <div className="noDataDiv">No data</div>
        }
        {this.renderPagination()}
      </div>
    );
  }

  handleClick(e, i) {
    if (this.props.onChangePage) {
      this.props.onChangePage(i, this.state.pageSize)
    }
  }

  getPager(currentPage, pageSize, totalRecords) {
    // default to first page
    currentPage = currentPage || 1;
    let totalPages = 0;
    // calculate total pages

    if (pageSize && totalRecords) {
      if (totalRecords % pageSize === 0) {
        totalPages = (totalRecords / (pageSize))
      } else {
        totalPages = (totalRecords / (pageSize)) + 1
      }
    }
    totalPages = Math.trunc(totalPages);
    var startPage, endPage;
    if (totalPages <= 10) {
      // less than 10 total pages so show all
      startPage = 1;
      endPage = totalPages;
    } else {
      // more than 10 total pages so calculate start and end pages
      if (currentPage <= 6) {
        startPage = 1;
        endPage = 10;
      } else if (currentPage + 4 >= totalPages) {
        startPage = totalPages - 9;
        endPage = totalPages;
      } else {
        startPage = currentPage - 5;
        endPage = currentPage + 4;
      }
    }

    var pages = [];

    for (var i = startPage; i <= endPage; i++) {
      pages.push(i)
    }

    return {
      currentPage: currentPage,
      startPage: startPage,
      endPage: endPage,
      pages: pages,
      totalPages: totalPages
    };
  }

  setPage(page) {
    var pager = this.state.pager;
    // update state
    this.setState({ currentPage: page, pager: this.getPager(page, this.state.pageSize, this.props.totalRecords) });

    // call change page function in parent component
    if (this.props.onChangePage)
      this.props.onChangePage(page, this.state.pageSize)
  }

  renderPagination() {
    let { currentPage } = this.state
    let { totalPages, pages } = this.state.pager
    return (
      <div className="pagination-wrapper" style={{ alignSelf: 'flex-end', marginTop: '10px' }}>
        <ul className="pagination">
          {totalPages > 5 &&
            <PaginationItem className={currentPage === 1 ? 'disabled' : ''}>
              <PaginationLink onClick={() => this.setPage(1)}> <i className="fa fa-caret-left" /> <i className="fa fa-caret-left" /></PaginationLink>
            </PaginationItem>
          }
          {totalPages > 5 &&
            <PaginationItem className={currentPage === 1 ? 'disabled' : ''}>
              <PaginationLink onClick={() => this.setPage(currentPage - 1)}><i className="fa fa-caret-left" /> </PaginationLink>
            </PaginationItem>
          }
          {pages.map((page, index) =>
            <PaginationItem key={index} className={currentPage === page ? 'active' : ''}>
              <PaginationLink onClick={() => this.setPage(page)}>{page}</PaginationLink>
            </PaginationItem>
          )}
          {totalPages > 5 &&
            <PaginationItem className={currentPage === totalPages ? 'disabled' : ''}>
              <PaginationLink onClick={() => this.setPage(currentPage + 1)}><i className="fa fa-caret-right" /></PaginationLink>
            </PaginationItem>
          }
          {totalPages > 5 &&
            <PaginationItem className={currentPage === totalPages ? 'disabled' : ''}>
              <PaginationLink onClick={() => this.setPage((totalPages ? totalPages : 0))}><i className="fa fa-caret-right" /><i className="fa fa-caret-right" /></PaginationLink>
            </PaginationItem>
          }
        </ul>

        <div style={{ marginLeft: 10 }}  >
          <span>Số lượng : </span>
          <select style={{
            boxShadow: 'rgba(0, 0, 0, 0.4) 1px 1px 2px'
            , paddingTop: 3
            , paddingBottom: 3
          }} onChange={e => this.changePageSize(e)} value={this.state.pageSize}>
            <option value="10">10</option>
            <option value="25">25</option>
            <option value="50">50</option>
            <option value="100">100</option>
          </select>
          {/* <span> rows </span> */}
        </div>
      </div>
    );
  }

  render() {
    return (
      <React.Fragment>
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
          {this.renderTable()}
        </div>
      </React.Fragment>
    );
  }
}

