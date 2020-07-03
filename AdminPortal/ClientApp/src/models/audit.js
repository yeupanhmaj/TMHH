import { AuditLocationArr } from '../commons/propertiesType';
import { getListEmployee } from '../services/auditService';

export const AuditDetails = [
      {
            propName: "proposalCode"
            , title: "Mã đề xuất"
      },
      {
            propName: "auditCode"
            , title: "Mã biên bản"
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
            title: "Địa điểm",
            propName: "location",
      },
      {
            title: "Ngày tạo biên bản",
            propName: "dateIn",

      },
      {
            title: "Bắt đầu",
            propName: "startTime",

      },
      {
            title: "Kết thúc",
            propName: "endTime",

      },
      {
            title: "Chủ tọa",
            propName: "presideName",

      },
      {
            title: "Thư ký",
            propName: "secretaryName",

      },
      {
            propName: "comment"
            , title: "Ghi chú"
            , isFull: true
      },
]
export const AuditColums = [

      {
            columnId: "auditCode"
            , columnName: "Mã biên bản"
      },
      {
            columnId: "quoteCodes"
            , columnName: "mã báo giá"
      },
      {
            columnId: "proposalCodes"
            , columnName: "mã  đề xuất"
      },
      {
            columnId: "inTime"
            , columnName: "Ngày tạo"
      }
]
export const AuditDefine = 
{
      height: 250,
      auto: true,
      props: [
            {
                  header: "ID",
                  name: "auditID",
                  type: "input",
                  hidden: true,
            },
            {
                  header: "Mã biên bản Họp giá",
                  name: "auditCode",
                  type: "input",     
                  allowNull: true,
            },          
            {
                  header: "Địa điểm",
                  name: "location",
                  type: "select",
                  values: AuditLocationArr,
                  valueDefault:   {
                        label: 'Cơ sở 1: 118 Hồng Bàng, Q.5, Tp. Hồ Chí Minh',
                        value: 1
                  },
                  isNotEdited: true,
                  hidden: true,
            },
            {
                  header: "Ngày tạo biên bản",
                  name: "inTime",
                  type: "datePicker",
            },
            {
                  header: "Bắt đầu",
                  name: "startTime",
                  type: "datePicker",
                  format:"dd-MM-yyyy HH:mm"
            },
            {
                  header: "Kết thúc",
                  name: "endTime",
                  type: "datePicker",
                  format:"dd-MM-yyyy HH:mm"
            },
            {
                  header: "Chủ tọa",
                  name: "preside",
                  type: "autoComplete",
                  getDataFunc: getListEmployee,   
                  valueDefault:{
                        label:'Nguyễn Phương Liên',
                        value:13,
                  },
                  values: [
                        {
                              label:'Nguyễn Phương Liên',
                              value:13
                        }
                
                  ],
                  labelCol: 'name',
                  valueCol: 'id',

            },
            {
                  header: "Thư ký",
                  name: "secretary",
                  type: "autoComplete",
                  getDataFunc: getListEmployee,             
                  values: [
                        {
                              label:'Trần Thị Thiên Kim',
                              value:3
                        },
                        {
                              label:'Trần Thị Thu Hồng',
                              value:2
                        }
                
                  ],
                  labelCol: 'name',
                  valueCol: 'id',
            },         
            {
                  header: "Nội dung",
                  name: "comment",
                  type: "textArea",
                  allowNull: true,
                  IsFull:true
            },
            {
                  header: "Thành phần",
                  name: "employees",
                  type: "listEmployeeAudit",
                  getDataFunc: getListEmployee,
                  IsFull: true,
            }
            // {
            //       header: "Danh mục sản phẩm",
            //       name: "items",
            //       type: "listItemQuote",
            //       IsFull: true,
            //       isNotEdited: true
            // }
      ]
}