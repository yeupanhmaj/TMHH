import { AuditLocationArr, BidMethodArr } from '../commons/propertiesType';
import * as QuoteService from '../services/quoteService';
import * as CapitalService from '../services/capitalService';



export const BidPlanDetails = [
      {
            propName: "bidName"
            , title: "Tên gói thầu"
      },
      {
            propName: "bidPlanCode"
            , title: "Mã kế hoạch chọn thầu"
      },
      {
            propName: "quoteCode"
            , title: "Mã báo giá"
      },
      {
            propName: "proposalCode"
            , title: "Mã đề xuất"
       },
      {
            propName: "departmentName"
            , title: "Khoa phòng đề xuất"
      },
      {
            propName: "userI"
            , title: "Người tạo"
      },
      {
            propName: "dateIn"
            , title: "Ngày tạo"
      },
      {
            title: "Bên mời thầu",
            propName: "bid",
      },
    
      {
            title: "Thời gian tổ chức",
            propName: "bidTime",

      },
      {
            title: "Địa điểm thực hiện",
            propName: "bidLocationLabel",

      },
      {
            title: "Hình thức và phương thức lựa chọn",
            propName: "bidMethodLabel",
      },
      {
            title: "Thời gian thực hiện",
            propName: "bidExpirated",
      },
      {
            title: "Đơn vị thời gian thực hiện",
            propName: "bidExpiratedUnit",
      },
      {
            title: "Loại hợp đồng",
            propName: "bidType",
      },
     
]

export const BidPlanColums = [
      {
            columnId: "bidName"
            , columnName: "Tên gói thầu"
      },
      {
            columnId: "bidPlanCode"
            , columnName: "Mã kế hoạch chọn thầu"
      },
      {
            columnId: "quoteCode"
            , columnName: "Mã báo giá"
      },    
      {
            columnId: "userI"
            , columnName: "Người tạo"
      },
      {
            columnId: "dateIn"
            , columnName: "Ngày tạo"
      },
      
]
export const BidPlanDefine = 
      {     height:250,
            auto: true,
            props: [
                  {
                        header: "ID",
                        name: "bidPlanID",
                        type: "input",
                        hidden:true,
                  }, 
                  {
                        header: "Mã kế hoạch chọn thầu",
                        name: "bidPlanCode",
                        type: "input",
                        allowNull:true
                  },              
                  {
                        header: "Mã Báo giá",
                        name: "quoteCode",
                        type: "autoComplete",
                        getDataFunc: QuoteService.getListquoteCodeCanCreateBiplan,
                       
                  },
                 
                  {
                        header: "Ngày tạo",
                        name: "dateIn",
                        type: "datePicker",
                  },
                  {
                        header: "Bên mời thầu",
                        name: "bid",
                        type: "input",
                        defaultText: "BỆNH VIỆN TRUYỀN MÁU HUYẾT HỌC",
                  },
                  {
                        header: "Tên gói thầu",
                        name: "bidName",
                        type: "input",
                        allowNull:true
                  },
                  {
                        header: "Nguồn vốn",
                        name: "bidCapital",
                        type: "autoComplete",
                        getDataFunc: CapitalService.GetCapitalByName,
                        labelCol: 'capitalName',
                        valueCol: 'capitalID',
                  },
                  {
                        header: "Thời gian tổ chức",
                        name: "bidTime",
                        type: "input",
                        isNotEdited:true,
                  },
                  {
                        header: "Địa điểm thực hiện",
                        name: "bidLocation",
                        type: "select",
                        values: AuditLocationArr,
                        isNotEdited: true,
                  },
                  {
                        header: "Hình thức và phương thức lựa chọn",
                        name: "bidMethod",
                        type: "select",
                        values: BidMethodArr,
                        isNotEdited:true,
                  },
                  {
                        header: "Loại hợp đồng",
                        name: "bidType",
                        type: "select",
                        values: [
                              {
                                    label:'Hợp đồng trọn gói',
                                    value:'Hợp đồng trọn gói'
                              },{
                                    label:'Đơn giá cố định',
                                    value:'Đơn giá cố định'
                              }
                      
                        ],
                  },
                  {
                        header: "Thời gian thực hiện",
                        name: "bidExpirated",
                        type: "input",
                        defaultText: "12",
                        allowNull: true,
                        width:100
                  },
                  {
                        header: "Tháng/Ngày",
                        name: "bidExpiratedUnit",                     
                        type: "select",
                        // valueDefault:{
                        //       label:'Tháng',
                        //       value:'Tháng',

                        // },
                        values: [
                              {
                                    label:'Ngày',
                                    value:'Ngày'
                              },{
                                    label:'Tháng',
                                    value:'Tháng'
                              }
                      
                        ],
                        width:90
                  },
                  {
                        header: "Danh mục sản phẩm",
                        name: "items",
                        type: "listItemQuote",
                        IsFull:true,
                        isNotEdited:true
                  }
            ]
      }
