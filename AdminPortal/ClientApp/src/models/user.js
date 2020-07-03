

export const UserColums = [
   {
      columnId: "userID"
      , columnName: "Tài khoản"
   },
   {
      columnId: "userName"
      , columnName: "Tên người dùng"
   },
   {
      columnId: "email"
      , columnName: "Email"
   },
   {
      columnId: "disableLable"
      , columnName: "Ngừng kích hoạt"
   },
]

export const UserDefine = 
{
   props: [
      {
         header: "Tài khoản",
         name: "userID",
         type: "input"
      },
     
      {
         header: "Mật khẩu",
         name: "password",
         type: "password"
      },
       {
         header: "Tên người dùng",
         name: "userName",
         type: "input"
      },
      {
         header: "Email",
         name: "email",
         type: "email"
      },
      {
         header: "Trạng thái",
         name: "disable",
         type: "radio",
         width:300,              
         defaultText: false,
         values: [
               {
                     value: true,
                     label: "Khóa",
               },
               {
                     value: false,
                     label: "Bình thường",
               },
         ]
      },
   ]
}