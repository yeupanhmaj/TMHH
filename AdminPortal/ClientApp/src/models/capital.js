
export const CapitalColums = [
   {
         columnId: "capitalID"
         , columnName: "ID"
   },
   {
      columnId: "capitalName"
      , columnName: "Nguồn vốn"
},
]
export const CapitalDefine = 
{
   height: 250,
   auto: true,
   props: [
         {
               header: "ID",
               name: "capitalID",
               type: "input",
               hidden: true,
         },
         {
               header: "Nguồn vốn",
               name: "capitalName",
               type: "input",
         },
         
   ]
}