import * as ProposalService from '../services/proposalService';

export const ExplanationDetails = [
      {
            propName: "explanationCode"
            , title: "Mã giải trình"
      },
      {
            propName: "proposalCode"
            , title: "Mã đề xuất"
      },
      {
            propName: "departmentName"
            , title: "Khoa phòng đề xuất"
      },
      // {
      //       propName: "status"
      //       , title: "Trạng thái"
      // },
      {
            propName: "necess"
            , title: "Sự cần thiết"
      },
      {
            propName: "suitable"
            , title: "Phù hợp với quy hoạch phát triển"
      },
      {
            title: "Số NB",
            propName: "nbNum"
      },
      {
            title: "Số XN",
            propName: "xnNum",
      },
      {
            title: "Máy đã có",
            propName: "isAvailable",
      },
      {
            title: "Số máy đã có",
            propName: "available",
      },
      {
            title: "Tính năng cơ bản",
            propName: "tncb",
      },
      {
            title: "Dự báo lỗi thời công nghệ",
            propName: "dbltcn",
      },
      {
            title: "Người vận hành",
            propName: "nvhttb",
      },
      {
            title: "Đào tạo nhân lực",
            propName: "dtnl",
      },
      {
            title: "Quản lý",
            propName: "nql",
      },
      {
            title: "Hiểu quả Kinh tế, Xã hội",
            propName: "hqktxh",
      },
      {
            propName: "userI"
            , title: "Người tạo"
      },
      {
            propName: "inTime"
            , title: "Ngày tạo"
      },
      {
            propName: "comment"
            , title: "Ghi chú"
            , isFull: true
      },
]

export const ExplanationColums = [

      {
            columnId: "explanationCode"
            , columnName: "Mã giải trình"
      },
      {
            columnId: "comment"
            , columnName: "Ghi chú"
      },
      {
            columnId: "proposalCode"
            , columnName: "Mã đề xuất"
      },
      {
            columnId: "departmentName"
            , columnName: "Khoa phòng đề xuất"
      },
      {
            columnId: "status"
            , columnName: "Trạng thái"
      },
      {
            columnId: "userI"
            , columnName: "Người tạo"
      },
      {
            columnId: "inTime"
            , columnName: "Ngày tạo"
      },
]
export const ExplanationDefine = 
{
      height: 500,
      auto: true,
      props: [
            {
                  header: "ID",
                  name: "explanationID",
                  type: "input",
                  hidden: true,
            },
            {
                  header: "Mã đề xuất",
                  name: "proposalCode",
                  type: "autoComplete",
               
                  getDataFunc: ProposalService.getListprosalCode,
            },
            {
                  header: "Mã giải trình",
                  name: "explanationCode",
                  type: "input",
                  isDisable: true,
                
            },

            // {
            //       header: "Trạng thái",
            //       name: "status",
            //       type: "select",
            //       values: statusArrStr,
            //       valueDefault:statusArrStr[0],
            //       order: 5,
            // },

            {
                  header: "Tên Hàng Hóa",
                  name: "productsName",
                  type: "input",
                  IsFull:true,
                  width:'100%',
            },
            {
                  header: "Sự cần thiết",
                  name: "necess",
                  type: "radio",
                 
                  width:200,              
                  defaultText: true,
                  values: [
                        {
                              value: true,
                              label: "Có",
                        },
                        {
                              value: false,
                              label: "Không",
                        },
                  ]
            },
            {
                  header: "Phù hợp với quy hoạch phát triển",
                  name: "suitable",
                  type: "radio",
                   
                  width:250,                           
                  defaultText: true,
                  values: [
                        {
                              value: true,
                              label: "Có",
                        },
                        {
                              value: false,
                              label: "Không",
                        },
                  ],

            },
            {
                  header: "Số NB",
                  name: "nbNum",
                  type: "input",
                  allowNull: true,
                  width:200,       
            },

            {
                  header: "Số XN",
                  name: "xnNum",
                  type: "input",
                  allowNull: true,
                  width:200,       
            },
            {
                  header: "Số máy đã có",
                  name: "available",
                  type: "input",
                  width:200,       
            },
            {
                  header: "Máy đã có",
                  name: "isAvailable",
                  type: "radio",
                  width:200,       
                  values: [
                        {
                              value: true,
                              label: "Có",
                        },
                        {
                              value: false,
                              label: "Không",
                        },
                  ]
            },
            {
                  header: "Giải thích",
                  name: "comment",
                  type: "textArea",
                  IsFull:true,
                  
            },
           
          
            {
                  header: "Tính năng cơ bản",
                  name: "tncb",
                  type: "textArea",
                  wrapwidth:'50%',       
            },
            {
                  header: "Dự báo lỗi thời công nghệ",
                  name: "dbltcn",
                  type: "textArea",
                  allowNull: true,
                  wrapwidth:'49%',
            },
            {
                  header: "Người vận hành trang thiết bị",
                  name: "nvhttb",
                  type: "input",
                  allowNull: true,
                  wrapwidth:'50%',   
                  width:'70%'
            },
            {
                  header: "Đào tạo nhân lực",
                  name: "dtnl",
                  type: "input",
                  allowNull: true,
                  wrapwidth:'50%', 
                  width:'70%'
            },
            {
                  header: "Người quản lý",
                  name: "nql",
                  type: "input",
                  allowNull: true,
                  wrapwidth:'50%', 
                  width:'70%'  
            },
            {
                  header: "Hiểu quả Kinh tế, Xã hội",
                  name: "hqktxh",
                  type: "textArea",
                  allowNull: true,
                  IsFull:true,
                 
            },
            {
                  header: "Danh mục đề xuất",
                  name: "items",
                  type: "listItemExplanation",
                  IsFull: true,
            }
      ]
}