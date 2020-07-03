import * as ProposalService from '../services/proposalService';

export const ProposalDetails = [
      {
            propName: "proposalCode"
            , title: "Mã đề xuất"
      },
      {
            propName: "proposalTypeName"
            , title: "Loại đề xuất"
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
            propName: "labelStatus"
            , title: "Trạng thái"
      },
      {
            propName: "dateIn"
            , title: "Ngày đề xuất"
      },
      {
            propName: "opinion"
            , title: "Ý kiến khoa / phòng đã khảo sát",
            isFull: true
      },
      {
            propName: "followComment"
            , title: "Ghi chú",
            isFull: true
      }
]



export const ProposalColums = [
      {
            columnId: "proposalCode"
            , columnName: "Mã đề xuất"
      },

      {
            columnId: "proposalTypeName"
            , columnName: "Loại đề xuất"
      },
      {
            columnId: "itemsName"
            , columnName: "Vật Phẩm"
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
            columnId: "labelStatus"
            , columnName: "Trạng thái"
      },
      {
            columnId: "dateIn"
            , columnName: "Ngày lập phiếu"
      },
      {
            columnId: "followComment"
            , columnName: "Ghi chú"
      }]
export const ProposalDefine =
{
      height: 250,
      auto: true,
      sortByOrder: true,
      props: [
            {
                  header: "ID",
                  name: "proposalID",
                  type: "input",
                  hidden: true
            },
            {
                  header: "ID",
                  name: "proposalID",
                  type: "input",
                  hidden: true
            },
            {
                  header: "Loại đề xuất",
                  name: "proposalType",
                  type: "select",
                  order: 2
            },
            {
                  header: "Khoa phòng đề xuất",
                  name: "departmentID",
                  type: "select",
                  values: [
                  ],
                  order: 3,

            },
            {
                  header: "Khoa phòng phụ trách",
                  name: "curDepartmentID",
                  type: "select",
                  order: 4
            },
            // {
            //       header: "Trạng thái",
            //       name: "status",
            //       type: "select",
            //       values: statusArr,
            //       valueDefault:statusArr[3],
            //       order: 5,
            // },

            {
                  header: "Ngày lập",
                  name: "dateIn",
                  type: "datePicker",
                  order: 6,
            },
            {
                  header: "Ý kiến khoa/ phòng đã khảo sát ",
                  name: "opinion",
                  type: "textArea",
                  order: 7,
                  allowNull: true,
                  IsFull: true,
            },
            {
                  header: "Nội dung",
                  name: "followComment",
                  type: "textArea",
                  order: 9,
                  allowNull: true,
                  IsFull: true,
            },
            {
                  header: "Danh mục đề xuất",
                  name: "items",
                  type: "listItems",
                  order: 8,
                  getDataFunc: ProposalService.getListItem,
                  IsFull: true,
            }
      ]
}