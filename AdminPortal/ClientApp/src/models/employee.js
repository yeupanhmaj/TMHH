import { GenericArr } from '../commons/propertiesType';


export const EmployeeColums = [
   {
         columnId: "name"
         , columnName: "Tên nhân viên"
   },
   {
      columnId: "title"
      , columnName: "Chức danh"
},
   {
         columnId: "roleName"
         , columnName: "Chức vụ"
   },

]
export const EmployeeDefine  = 
{
   height: 250,
   auto: true,
   props: [
         {
               header: "ID",
               name: "employeeID",
               type: "input",
               hidden: true,
         },
         {
               header: "Tên",
               name: "name",
               type: "input",
         },
         {
            header: "Chức danh",
            name: "title",
            type: "input",
      },
         {
               header: "Chức vụ",
               name: "roleName",
               type: "input",
         },
         {
            header: "Giới tính",
            name: "generic",
            type: "select",
            values: GenericArr,
            isNotEdited:true,
      },
   ]
}