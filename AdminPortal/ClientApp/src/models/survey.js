import { surveyTypes } from '../commons/propertiesType';
import * as ProposalService from '../services/proposalService';


export const SurveyDetails = [
      {
            propName: "surveyCode"
            , title: "Mã khảo sát"
      },
      {
            propName: "surveyDepartmentName"
            , title: "Khoa phòng khảo sát"
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
            propName: "typeName",
            title: "Loại đề xuất",
      },
      // {
      //       propName: "status"
      //       , title: "Trạng thái"
      // },
      {
            propName: "solutionName",
            title: "Hướng giải quyết",
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
            propName: "comment"
            , title: "Tình trạng trước khảo sát"
            , isFull :true

      },
      {
            title: "Hàng mẫu",
            propName: "isSample",
      },
      {
            title: "Đồng ý với đề xuất",
            propName: "valid",
      },
      {
            title: "Hướng xử lý",
            propName: "solutionText",
            isFull :true
      },
]


export const SurveyColums = [

      {
            columnId: "surveyCode"
            , columnName: "Mã khảo sát"
      },
      {
            columnId: "comment"
            , columnName: "Tình trạng"
      },
      {
            columnId: "status"
            , columnName: "Trạng thái"
      },
      {
            columnId: "surveyDepartmentName"
            , columnName: "Khoa phòng khảo sát"
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
            columnId: "userI"
            , columnName: "Người tạo"
      },
      {
            columnId: "dateIn"
            , columnName: "Ngày tạo"
      },
]
export const SurveyDefine = 
{
      auto: true,
      props: [
            {
                  header: "ID",
                  name: "surveyID",
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
                  header: "Mã khảo sát",
                  name: "surveyCode",
                  type: "input",
                  allowNull: true,
               
                  isDisable: true
            },
            {
                  header: "Khoa phòng khảo sát",
                  name: "surveyDepartmentID",
                  type: "select",
           
               
            },
            {
                  header: "Ngày tạo",
                  name: "dateIn",
                  type: "datePicker"
               
            },
            {
                  header: "Tên sản phẩm",
                  name: "productsName",
                  type: "input",
                  IsFull:true
            },
            {
                  header: "1. Tình trạng trước khảo sát",
                  name: "comment",
                  type: "textArea",
        
                  minlength:30,
                  height: 120,
                  IsFull:true
            },
           
            {
                  header: "2.Hướng giải quyết",
                  name: "solution",
                  type: "radio",
                  defaultText: 1,
                  values: surveyTypes,
                  IsFull:true,
            },        
            {
                  header: "",
                  name: "solutionText",
                  type: "textArea",
                  allowNull: true,            
                  IsFull:true
            },
            {
                  header: "3.	Các vật tư, linh kiện đề nghị thay thế (nếu sửa chữa)/Tên tài sản, mã hiệu, hãng sản xuất ....(nếu có)",
                  name: "surveyItems",
                  type: "ListItemSurvey",
                  IsFull: true,
            },
            {
                  header: "Hàng mẫu",
                  name: "isSample",
                  type: "checkbox",
                  defaultText: false,                  
                  wrapwidth:'100%',
            },
            {
                  header: "4.	Ý kiến của Khoa phòng",
                  name: "valid",
                  type:'radio',
                  defaultText: true,
                  values: [
                        {
                              value: true,
                              label: "Đồng ý",
                        },
                        {
                              value: false,
                              label: "Không đồng ý ",
                        },
                  ],
                  wrapwidth:'100%',
            },
           
            {
                  header: "5. Ý Kiến khác",
                  name: "departmentComment",
                  type: "textArea",
                  allowNull: true,
                 
                  IsFull:true
            },
      ]
}