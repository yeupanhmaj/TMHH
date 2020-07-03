import { AuditLocationArr, DeliveryReceiptTypeArr } from '../commons/propertiesType';
import { getListEmployee } from '../services/deliveryReceiptService';
import * as ProposalService from '../services/proposalService';

export const DeliveryReceiptDetails = [
      {
            propName: "proposalCode"
            , title: "Mã đề xuất"
      },
      {
            propName: "deliveryReceiptCode"
            , title: "Mã biên bản giao nhận"
      },
      {
            propName: "deliveryReceiptTypeLabel"
            , title: "Loại"
      },
      {
            propName: "deliveryReceiptDate"
            , title: "Thời gian"
      },

      {
            propName: "deliveryReceiptPlaceLabel"
            , title: "Địa điểm"
      },
    
      {
            propName: "departmentName"
            , title: "Khoa phòng đề xuất"
      },
      {
            propName: "curDepartmentName"
            , title: "Khoa phòng phụ trách"
      },
      {
            propName: "userU"
            , title: "Người cập nhật"
      },
      {
            propName: "updateTime"
            , title: "Ngày cập nhật"
      },
]

export const DeliveryReceiptColums = [
      {
            columnId: "proposalCode"
            , columnName: "Mã đề xuất"
      },
      {
            columnId: "deliveryReceiptCode"
            , columnName: "Mã biên bản giao nhận"
      },
      {
            columnId: "deliveryReceiptTypeLabel"
            , columnName: "Loại"
      },
      {
            columnId: "deliveryReceiptDate"
            , columnName: "Thời gian"
      },

      {
            columnId: "departmentName"
            , columnName: "Khoa phòng đề xuất"
      },
      {
            columnId: "curDepartmentName"
            , columnName: "Khoa phòng phụ trách"
      },
  
      {
            columnId: "inTime"
            , columnName: "Ngày Tạo"
      },
]
export const DeliveryReceiptDefine = 
{
      auto: true,
      props: [
            {
                  header: "deliveryReceiptID",
                  name: "deliveryReceiptID",
                  type: "input",
                  hidden: true,
            },
            {
                  header: "Mã giao nhận",
                  name: "deliveryReceiptCode",
                  type: "input",
                  allowNull:true,
            },
            {
                  header: "Loại biên bản",
                  name: "deliveryReceiptType",
                  type: "select",
                  values: DeliveryReceiptTypeArr,
               
            },
            {
                  header: "Mã đề xuất",
                  name: "proposalCode",
                  type: "autoComplete",        
                  labelCol: "proposalCode",
                  valueCol: "proposalID",
                  getDataFunc: ProposalService.getListProsalCanCreateDR,
            },
            {
                  header: "Mã hợp đồng",
                  name: "contractCode",
                  type: "input",                  
                  isDisable: true,
                  allowNull:true,
            },
            {
                  header: "Mã báo giá",
                  name: "quoteCode",
                  type: "input",                  
                  isDisable: true,
                  allowNull:true,
            },
            {
                  header: "Khoa/phòng đề xuất",
                  name: "departmentName",
                  type: "input",
                  isDisable: true,
                  allowNull:true,
            },
            {
                  header: "Khoa/phòng phụ trách",
                  name: "curDepartmentName",
                  type: "input",
                  isDisable: true,
                  allowNull:true,
            },
            {
                  header: "Ngày giao nhận",
                  name: "deliveryReceiptDate",
                  type: "datePicker",
            },
            
            {
                  header: "Địa chỉ",
                  name: "deliveryReceiptPlace",
                  type: "select",
                  values: AuditLocationArr,
                  width: 380

            },
            // {
            //       header: "Thành phần",
            //       name: "employees",
            //       type: "listEmployeeDeliveryReceipt",
            //       getDataFunc: getListEmployee,
            //       IsFull: true,
            // },
            // {
            //       header: "Danh mục sản phẩm",
            //       name: "items",
            //       type: "listItemDeliveryReceipt",
            //       IsFull: true,
            // },
            
      ]
}