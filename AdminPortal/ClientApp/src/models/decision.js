import * as CapitalService from '../services/capitalService';
import * as QuoteService from '../services/quoteService';
import { BidMethodArr } from '../commons/propertiesType';


export const DecisionDetails = [
      {
            propName: "decisionCode"
            ,title : "Mã quyết định"
      },
      {
            propName: "quoteCode"
            , title: "Mã báo giá"
      },
      // {
      //       propName: "proposalID"
      //       , title: "Mã đề xuất"
      // },
      {
            propName: "userI"
            , title: "Người tạo"
      },
      {
            propName: "dateIn"
            , title: "Ngày tạo"
      },
]

export const DecisionColums = [
      {
            columnId: "decisionCode"
            , columnName: "Mã quyết định"
      },
      {
            columnId: "quoteCode"
            , columnName: "Mã báo giá"
      },
      // {
      //       columnId: "proposalID"
      //       , columnName: "Mã đề xuất"
      // },
      {
            columnId: "userI"
            , columnName: "Người tạo"
      },
      {
            columnId: "dateIn"
            , columnName: "Ngày tạo"
      },
]
export const DecisionDefine = 
{
      height: 250,
      auto: true,
      props: [
            {
                  header: "ID",
                  name: "decisionID",
                  type: "input",
                  hidden: true,
            },
            {
                  header: "Mã quyết định",
                  name: "decisionCode",
                  type: "input",
                  allowNull:true
            },
            {
                  header: "Mã báo giá",
                  name: "quoteCode",
                  type: "autoComplete",
                  getDataFunc: QuoteService.getListquoteCodeCanCreateDecision,
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
                  header: "Ngày tạo",
                  name: "dateIn",
                  type: "datePicker",
            },
            {
                  header: "Hình thức và phương thức lựa chọn",
                  name: "bidMethod",
                  type: "select",
                  values: BidMethodArr,
                  isNotEdited:true,
            },
            {
                   header: "Danh mục sản phẩm",
                        name: "items",
                        type: "listItemQuote",
                        IsFull:true,
                        isNotEdited:true
            },
            
      ]
}