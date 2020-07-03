import { acceptanceResultArr } from '../commons/propertiesType';
import * as ProposalService from '../services/proposalService';


export const AcceptanceDetails = [
      {
            propName: "proposalCode"
            , title: "Mã đề xuất"
      },

      {
            propName: "acceptanceCode"
            , title: "Mã nghiệm thu"
      },
      {
            propName: "departmentName"
            , title: "Khoa phòng đề xuất"
      },

      {
            propName: "acceptanceResultLabel"
            , title: "Kết quả nghiệm thu",
      },

      {
            propName: "userU"
            , title: "Người cập nhật"
      },
      {
            propName: "updateTime"
            , title: "Ngày cập nhật"
      },
      {
            propName: "acceptanceNote"
            , title: "Ghi Chú",
            isFull: true
      },
]
export const AcceptanceColums = [
      {
            columnId: "proposalCode"
            , columnName: "Mã đề xuất"
      },
      {
            columnId: "acceptanceCode"
            , columnName: "Mã nghiệm thu"
      },
      {
            columnId: "acceptanceNote"
            , columnName: "Ghi Chú"
      },
      {
            columnId: "acceptanceResultLabel"
            , columnName: "kết quả nghiệm thu"
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
            columnId: "userU"
            , columnName: "Người cập nhật"
      },
      {
            columnId: "updateTime"
            , columnName: "Ngày cập nhật"
      },
]
export const AcceptanceDefine =
{
      auto: true,
      props: [
            {
                  header: "acceptanceID",
                  name: "acceptanceID",
                  type: "input",
                  hidden: true,
                  isNotEdited: true,
            },
            {
                  header: "Mã nghiệm thu",
                  name: "acceptanceCode",
                  type: "input",
                  allowNull: true,
            },
            {
                  header: "Mã đề xuất",
                  name: "proposalCode",
                  type: "autoComplete",
                  isNotEdited: true,
                  getDataFunc: ProposalService.getListProsalHaveAcceptance,
                  labelCol: "proposalCode",
                  valueCol: "proposalID",
            },
            {
                  header: "Khoa/phòng",
                  name: "departmentName",
                  type: "input",
                  isDisable: true,
            },
            {
                  header: "Kết quả nghiệm thu",
                  name: "acceptanceResult",
                  type: "select",
                  values: acceptanceResultArr,
                  width: 250
            },
            {
                  header: "Loại nghiệm thu",
                  name: "acceptanceType",
                  type: "select",
                  width: 250,
                  values: [
                        {
                              value: 1, label: "Mua Sắm",
                        },
                        {
                              value: 2, label: "Sửa chữa",
                        },
                  ],
            },
            {
                  header: "Ý kiến của Khoa/ Phòng sử dụng: ",
                  name: "acceptanceNote",
                  type: "textArea",
                  allowNull: true,
                  IsFull: true
            },
            {
                  header: "Danh mục sản phẩm",
                  name: "items",
                  type: "listItemAcceptance",
                  IsFull: true,
            },

      ]
}