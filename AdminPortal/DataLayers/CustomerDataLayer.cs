using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class CustomerDataLayer : BaseLayerData<CustomerDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<CustomerInfo></returns>
        /// 
        public List<CustomerInfo> GetAllCustomer(SqlConnection connection)
        {
            var result = new List<CustomerInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Customer where  1 = 1 order by CustomerID ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new CustomerInfo();
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        info.CustomerCode = GetDbReaderValue<string>(reader["CustomerCode"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.Address = GetDbReaderValue<string>(reader["Address"]);
                        info.Phone = GetDbReaderValue<string>(reader["Phone"]);
                        info.Email = GetDbReaderValue<string>(reader["Email"]);
                        info.TaxCode = GetDbReaderValue<string>(reader["TaxCode"]);
                        info.BankNumber = GetDbReaderValue<string>(reader["BankNumber"]);
                        info.BankName = GetDbReaderValue<string>(reader["BankName"]);
                        info.Surrogate = GetDbReaderValue<string>(reader["Surrogate"]);
                        info.Position = GetDbReaderValue<string>(reader["Position"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<CustomerInfo> getCustomer(SqlConnection connection, CustomerSeachCriteria _criteria)
        {
            var result = new List<CustomerInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Customer where  1 = 1 ", connection))
            {
                if (!string.IsNullOrEmpty(_criteria.CustomerID))
                {
                    command.CommandText += " and CustomerID = @CustomerID";
                    AddSqlParameter(command, "@CustomerID", _criteria.CustomerID, System.Data.SqlDbType.Int);
                }
                if (!string.IsNullOrEmpty(_criteria.CustomerCode))
                {
                    command.CommandText += " and CustomerCode = @CustomerCode";
                    AddSqlParameter(command, "@CustomerCode", _criteria.CustomerCode, System.Data.SqlDbType.NVarChar);
                }
                if (!string.IsNullOrEmpty(_criteria.CustomerName))
                {
                    command.CommandText += " and CustomerName like N'%" + _criteria.CustomerName + "%' ";
                }
                command.CommandText += " order by CustomerID  ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new CustomerInfo();
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        info.CustomerCode = GetDbReaderValue<string>(reader["CustomerCode"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.Address = GetDbReaderValue<string>(reader["Address"]);
                        info.Phone = GetDbReaderValue<string>(reader["Phone"]);
                        info.Email = GetDbReaderValue<string>(reader["Email"]);
                        info.Fax = GetDbReaderValue<string>(reader["Fax"]);
                        info.TaxCode = GetDbReaderValue<string>(reader["TaxCode"]);
                        info.BankNumber = GetDbReaderValue<string>(reader["BankNumber"]);
                        info.BankName = GetDbReaderValue<string>(reader["BankName"]);
                        info.Surrogate = GetDbReaderValue<string>(reader["Surrogate"]);
                        info.Position = GetDbReaderValue<string>(reader["Position"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public CustomerInfo getCustomer(SqlConnection connection, int _ID)
        {
            CustomerInfo info = null;
            using (var command = new SqlCommand("Select * " +
                " from tbl_Customer where  CustomerID = @CustomerID ", connection))
            {

                AddSqlParameter(command, "@CustomerID", _ID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new CustomerInfo();
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        info.CustomerCode = GetDbReaderValue<string>(reader["CustomerCode"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.Address = GetDbReaderValue<string>(reader["Address"]);
                        info.Phone = GetDbReaderValue<string>(reader["Phone"]);
                        info.Fax = GetDbReaderValue<string>(reader["Fax"]);
                        info.Email = GetDbReaderValue<string>(reader["Email"]);
                        info.TaxCode = GetDbReaderValue<string>(reader["TaxCode"]);
                        info.BankNumber = GetDbReaderValue<string>(reader["BankNumber"]);
                        info.BankName = GetDbReaderValue<string>(reader["BankName"]);
                        info.Surrogate = GetDbReaderValue<string>(reader["Surrogate"]);
                        info.Position = GetDbReaderValue<string>(reader["Position"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return info;
            }
        }

        public CustomerInfo getCustomerByBidPlanCode(SqlConnection connection, string bidPlanCode)
        {
            CustomerInfo info = null;
            using (var command = new SqlCommand("Select C.* from " +
                " (select BP.AuditID from tbl_BidPlan BP where BP.BidPlanCode = @BidPlanCode) BP " +
                " left join tbl_Audit A on A.AuditID = BP.AuditID" +
                " left join tbl_Quote Q on Q.QuoteID = A.QuoteID" +
                " left join tbl_Customer C on C.CustomerID = Q.CustomerID ", connection))
            {

                AddSqlParameter(command, "@BidPlanCode", bidPlanCode, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new CustomerInfo();
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        info.CustomerCode = GetDbReaderValue<string>(reader["CustomerCode"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.Address = GetDbReaderValue<string>(reader["Address"]);
                        info.Phone = GetDbReaderValue<string>(reader["Phone"]);
                        info.Fax = GetDbReaderValue<string>(reader["Fax"]);
                        info.Email = GetDbReaderValue<string>(reader["Email"]);
                        info.TaxCode = GetDbReaderValue<string>(reader["TaxCode"]);
                        info.BankNumber = GetDbReaderValue<string>(reader["BankNumber"]);
                        info.BankName = GetDbReaderValue<string>(reader["BankName"]);
                        info.Surrogate = GetDbReaderValue<string>(reader["Surrogate"]);
                        info.Position = GetDbReaderValue<string>(reader["Position"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return info;
            }
        }

        public int InsertCustomer(SqlConnection connection, CustomerInfo _customer, string _userI)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Customer] (CustomerCode, CustomerName, Address,Phone,Email,UserI,SourceID)"  +
                    "VALUES(@CustomerCode, @CustomerName, @Address, @Phone, @Email, @UserI, @SourceID)  ", connection))
            {
                AddSqlParameter(command, "@CustomerCode", _customer.CustomerCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@CustomerName", _customer.CustomerName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Address", _customer.Address, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Phone", _customer.Phone, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Email", _customer.Email, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            return lastestInserted;
        }

        public void UpdateCustomer(SqlConnection connection, CustomerInfo _customer, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Customer \n" +
                            "SET CustomerCode = @CustomerCode, CustomerName = @CustomerName, Address = @Address,Phone = @Phone,Email = @Email, UserU=@UserU,UpdateTime=getdate() \n" +
                            "WHERE (CustomerID = @CustomerID)", connection))
            {
                AddSqlParameter(command, "@CustomerID", _customer.CustomerID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerCode", _customer.CustomerCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@CustomerName", _customer.CustomerName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Address", _customer.Address, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Phone", _customer.Phone, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Email", _customer.Email, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void DeleteCustomer(SqlConnection connection,int _customerID)
        {
            using (var command = new SqlCommand("delete tbl_Customer where CustomerID=@CustomerID", connection))
            {
                AddSqlParameter(command, "@CustomerID", _customerID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
    }
}
