using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Entities
{
    public class EmployeeInfo
    {
        public int EmployeeID { get; set; }


        public string Title { get; set; }

        public string RoleName { get; set; }

        public string Name { get; set; }

        public int Generic { get; set; }

        public string DepartmentID { get; set; }

        public string UserID { get; set; }

    }

    public class AuditEmployeeInfo
    {
        public int AutoID { get; set; }
        public int AuditID { get; set; }


        public int EmployeeID { get; set; }

        public string Comment { get; set; }



        public string Title { get; set; }

        public string RoleName { get; set; }

        public string Name { get; set; }

        public int Generic { get; set; }

        public string DepartmentID { get; set; }

        public string UserID { get; set; }

    }

    public class DeliveryReceiptEmployeeInfo
    {
        public int AutoID { get; set; }
        public int DeliveryReceiptID { get; set; }

        public int EmployeeID { get; set; }

        public int Role { get; set; }

        public string Title { get; set; }

        public string RoleName { get; set; }

        public string Name { get; set; }

        public int Generic { get; set; }

        public string DepartmentID { get; set; }

        public string UserID { get; set; }

    }

  
}
