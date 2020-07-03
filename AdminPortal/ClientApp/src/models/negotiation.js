import { AuditLocationArr, NegotiationBankIDArr, NegotiationTermArr } from '../commons/propertiesType';
import * as QuoteService from '../services/quoteService';
import * as CapitalService from '../services/capitalService';

export const NegotiationtDetails = [
      {
            propName: "negotiationCode"
            , title: "Mã thương thảo"
      },

      {
            propName: "dateIn"
            , title: "Thời gian"
      },
      
      {
            title: "Địa chỉ",
            propName: "location",
      },
      {
            title: "Điện thoại",
            propName: "phone",
      },
      {
            title: "Fax",
            propName: "fax",
      },
      {
            title: "Mã số thuế",
            propName: "taxCode",
      },
      {
            title: "Số tài khoản",
            propName: "bankIDLabel",
      },

      {
            title: "Đại diện",
            propName: "represent",
      },
      {
            title: "Chức vụ",
            propName: "position",
      },
     
      {
            title: "Hình thức hợp đồng",
            propName: "contractType",
      },
      {
            title: "Thời gian thực hiện hợp đồng",
            propName: "contractTypeExpired",
      },
      {
            title: "Đơn vị thời gian thực hiện hợp đồng",
            propName: "contractTypeExpiredUnit",
      },
]

export const NegotiationColums = [
      {
            columnId: "negotiationCode"
            , columnName: "Mã thương thảo"
      },
      {
            columnId: "quoteCode"
            , columnName: "Mã Báo giá"
      },
      {
            columnId: "customerName"
            , columnName: "Nhà cung cấp"
      },
      {
            columnId: "dateIn"
            , columnName: "Thời gian"
      },
]


export const NegotiationDefine = 
{
      height: 250,
      auto: true,
      props: [
            {
                  header: "ID",
                  name: "negotiationID",
                  type: "input",
                  hidden: true,
            },
            {
                  header: "Mã thương thảo",
                  name: "negotiationCode",
                  type: "input",            
                  allowNull:true
            },
            {
                  header: "Báo giá",
                  name: "quoteCode",
                  type: "autoComplete",
                  getDataFunc: QuoteService.getListquoteCodeCanCreateNegotiation,
            },
            {
                  header: "Nhà cung cấp",
                  name: "customerName",
                  type: "input",
                  isDisable: true,
            },
            {
                  header: "Thời gian",
                  name: "dateIn",
                  type: "datePicker",
                  format:"dd-MM-yyyy HH:mm"
            },
          
            {
                  header: "Địa chỉ",
                  name: "location",
                  allowNull: true,
                  type: "select",
                  values: AuditLocationArr,
                  width: 380
            },
            {
                  header: "Điện thoại",
                  name: "phone",
                  type: "input",
                  allowNull: true,
                  defaultText: "(028) 3957 1342"
            },
            {
                  header: "Fax",
                  name: "fax",
                  type: "input",
                  allowNull: true,
                  defaultText: "(028) 3855 2978"
            },
            {
                  header: "Mã số thuế",
                  name: "taxCode",
                  type: "input",
                  allowNull: true,
                  defaultText: "0304251703"
            },
            {
                  header: "Số tài khoản",
                  name: "bankID",
                  type: "select",
                  values: NegotiationBankIDArr,
                  isNotEdited:true,

            },
            
            {
                  header: "Đại diện",
                  name: "represent",
                  type: "input",
                  allowNull: true,
                  defaultText: "BS.CKII. Phù Chí Dũng"
            },
            {
                  header: "Chức vụ",
                  name: "position",
                  type: "input",
                  allowNull: true,
                  defaultText: "Giám đốc"
            },
            
            {
                  header: "Thời hạn thanh toán",
                  name: "term",
                  type: "select",
                  values: NegotiationTermArr,
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
                  //defaultText: "12",
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
                  IsFull: true,
                  isNotEdited: true
            },
          
      ]
}