import * as QuoteService from '../services/quoteService';


export const ContractDetails = [
      {
            propName: "contractCode"
            , title: "Mã kế hoạch chọn thầu"
      },   
      {
            propName: "quoteCode"
            , title: "Mã báo giá"
      },
      {
            propName: "customerName"
            , title: "Nhà cung cấp"
      },
      {
            propName: "dateIn"
            , title: "Ngày tạo"
      },
]


export const ContractColums = [
      {
            columnId: "contractCode"
            , columnName: "Mã hợp đồng"
      },
      {
            columnId: "quoteCode"
            , columnName: "Mã báo giá"
      },
      {
            columnId: "customerName"
            , columnName: "Nhà cung cấp"
      },
      {
            columnId: "dateIn"
            , columnName: "Ngày tạo"
      },
]
export const ContractDefine = 
{
      height: 250,
      auto: true,
      props: [
            {
                  header: "ID",
                  name: "contractID",
                  type: "input",
                  hidden: true,
            },         
            {
                  header: "Mã hợp đồng",
                  name: "contractCode",
                  type: "input",
                  allowNull:true,
            },         
            {
                  header: "Báo giá",
                  name: "quoteCode",
                  type: "autoComplete",
                  getDataFunc: QuoteService.getListquoteCodeCanCreateContract,
            },
            {
                  header: "Nhà cung cấp",
                  name: "customerName",
                  type: "input",
                  isDisable: true,
                  width:370
            },
            {
                  header: "Ngày tạo",
                  name: "dateIn",
                  type: "datePicker",
            },
            {
                  header: "Danh mục sản phẩm",
                  name: "items",
                  type: "listItemQuote",
                  IsFull: true,
                  isNotEdited: true
            },          
      ]
}