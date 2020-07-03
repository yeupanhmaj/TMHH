import { QuoteTypeArr } from '../../commons/propertiesType';
import * as ProposalService from '../../services/proposalService';


export const QuoteDetails = [
      {
            propName: "quoteCode"
            , title: "Mã báo giá"
      },
      {
            propName: "customerName"
            , title: "Công ty"
      },
      {
            propName: "proposalCode"
            , title: "Mã đề xuất"
      },
      {
            propName: "quoteTypeLabel"
            , title: "Loại"
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
            title: "Địa chỉ công ty",
            propName: "address",

      },
      {
            title: "Điện Thoại",
            propName: "phone",

      },
      {
            title: "Email",
            propName: "email",

      },
      {
            title: "Fax",
            propName: "fax",

      },
      {
            title: "Người đại diện",
            propName: "surrogate",

      },
      {
            title: "Chức vụ người đại diện",
            propName: "position",

      },
      {
            title: "Mã số thuế",
            propName: "taxCode",

      },
      {
            title: "Ngân hàng",
            propName: "bankName",

      },
      {
            title: "Tài khoản ngân hàng",
            propName: "bankNumber",

      },
      {
            title: "Thời gian tạo báo giá",
            propName: "dateIn",

      },
      {
            title: "Địa điểm giao hàng",
            propName: "deliveryLocation",

      },
      {
            title: "Thời gian giao hàng (ngày) từ khi ký hợp đồng",
            propName: "deliveryTime",

      },
      {
            title: "Thời gian báo giá hiệu lực(ngày) ",
            propName: "validTime",


      },
      {
             title: "Điều kiện giao hàng",
            propName: "deliveryTermAndCondition",

      },
      {
            title: "Có VAT",
            propName: "isVAT",

      },
      {
            title: "Giá trị VAT",
            propName: "vatNumber",

      },
      {
            title: "Điều kiện bảo hành",
            propName: "warrantyTermAndCondition",
      },
      {
            propName: "comment"
            , title: "Ghi chú"
            , isFull: true
      },
]

export const QuoteColums = [
      {
            columnId: "quoteCode"
            , columnName: "Mã báo giá"
      },
      {
            columnId: "customerName"
            , columnName: "Công ty"
      },
      {
            columnId: "proposalCode"
            , columnName: "Mã đề xuất"
      },
      {
            columnId: "comment"
            , columnName: "Ghi chú"
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

export const QuoteDefine = 
{
      height: 250,
      auto: true,
      props: [
            {
                  header: "ID",
                  name: "quoteID",
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
                  header: "Mã báo giá",
                  name: "quoteCode",
                  type: "input",
                  isDisable: true
            },
            {
                  header: "Công ty báo giá",
                  name: "customerID",
                  type: "select",
                  width: 372,

            },
            {
                  header: "Loại",
                  name: "quoteType",
                  type: "select",
                  values: QuoteTypeArr,
                  isNotEdited:true,
            },
            {
                  header: "Địa chỉ công ty",
                  name: "address",
                  type: "input",
                  width: 372,

            },
            {
                  header: "Điện Thoại",
                  name: "phone",
                  type: "input",

                  allowNull: true,
            },
            {
                  header: "Email",
                  name: "email",
                  type: "input",
                  allowNull: true,
            },
            {
                  header: "Fax",
                  name: "fax",
                  type: "input",
                  allowNull: true,
            },
            {
                  header: "Người đại diện",
                  name: "surrogate",
                  type: "input",
            },
            {
                  header: "Chức vụ người đại diện",
                  name: "position",
                  type: "input",
            },
            {
                  header: "Mã số thuế",
                  name: "taxCode",
                  type: "input",

            },
            {
                  header: "Ngân hàng",
                  name: "bankName",
                  type: "input",
                  width: 372,

            },
            {
                  header: "Tài khoản ngân hàng",
                  name: "bankNumber",
                  type: "input",
                  allowNull: true,

            },
            {
                  header: "Thời gian tạo báo giá",
                  name: "dateIn",
                  type: "datePicker"
            },
            {
                  header: "Địa điểm giao hàng",
                  name: "deliveryLocation",
                  type: "select",
                  width: 372,
                  values: [
                        {
                              label:'118 Hồng Bàng, Phường 12, Quận 5, Tp.HCM',
                              value:'118 Hồng Bàng, Phường 12, Quận 5, Tp.HCM'
                        },{
                              label:'201 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Tp.HCM',
                              value:'201 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Tp.HCM'
                        }
                
                  ],

            },
            {
                  header: "Thời gian giao hàng (ngày) từ khi ký hợp đồng",
                  name: "deliveryTime",
                  type: "input",
                  width: 100,
                  isNumber: true
            },
            {
                  header: "Thời gian báo giá hiệu lực(ngày) ",
                  name: "validTime",
                  type: "input",
                  defaultText: "30",
                  width: 100,
                  isNumber: true

            },
            {
                  header: "Có VAT",
                  name: "isVAT",
                  type: "checkbox",
            },
            {
                  header: "Giá trị VAT",
                  name: "vatNumber",
                  type: "input",            
                  defaultText:10,
                  suffix: '%', 
                  width:90,
                   isNumber:true,
                  allowNull:true,
                  isDisable:true,          
            },
            {
                  header: "Điều kiện giao hàng",
                  name: "deliveryTermAndCondition",
                  type: "textArea",
                  allowNull: true,
                  IsFull:true
            },
            {
                  header: "Điều kiện bảo hành",
                  name: "warrantyTermAndCondition",
                  type: "textArea",
                  allowNull: true,
                  IsFull:true
            },
            {
                  header: "Danh mục đề xuất",
                  name: "items",
                  type: "listItemQuote",
                  IsFull: true,
            }
      ]
}