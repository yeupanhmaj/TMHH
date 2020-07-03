using System;


namespace AdminPortal.Entities
{
    public class CustomerInfo
    {
        public int CustomerID { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
        public string TaxCode { get; set; }

        public string BankNumber { get; set; }
        public string BankName { get; set; }
        public string Surrogate { get; set; }
        public string Position { get; set; }
        public string Fax { get; set; }
        public string UserI { get; set; }

        public DateTime? InTime { get; set; }

        public string UserU { get; set; }

        public DateTime UpdateTime { get; set; }

        public int SourceID { get; set; }
    }

    public class CustomerSeachCriteria
    {
        public string CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
    }
}
