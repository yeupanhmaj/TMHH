using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class DepartmentDataLayer : BaseLayerData<DepartmentDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<DepartmentInfo></returns>
        /// 



        public string GetDepartmentNamesFromQuote(SqlConnection connection , int quote)
        {
            string ret = "";
            using (var command = new SqlCommand(" select  tblD.DepartmentName from " +
                   "   tbl_Quote_Proposal tblQP" +
                   "   inner join tbl_Proposal tblP on tblP.ProposalID = tblQp.ProposalID" +
                   "  inner join tbl_Department tblD on tblD.DepartmentID = tblP.DepartmentID" +
                   "   where tblQP.QuoteID =" + quote, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret += GetDbReaderValue<string>(reader["DepartmentName"]) + ",";
                    }
                }
            }
            if (ret.Length < 1) return ret;
            return ret.Remove(ret.Length - 1);
          
        }

        public List<DepartmentInfo> GetAllDepartment(SqlConnection connection)
        {
            var result = new List<DepartmentInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Department where  1 = 1 order by DepartmentID ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new DepartmentInfo();
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.Address = GetDbReaderValue<string>(reader["Address"]);
                        info.Phone = GetDbReaderValue<string>(reader["Phone"]);
                        info.Email = GetDbReaderValue<string>(reader["Email"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.SourceID = GetDbReaderValue<int>(reader["SourceID"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<DepartmentInfo> getDepartment(SqlConnection connection, DepartmentSeachCriteria _criteria)
        {
            var result = new List<DepartmentInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Department where  1 = 1 ", connection))
            {
                if (!string.IsNullOrEmpty(_criteria.DepartmentID))
                {
                    command.CommandText += " and DepartmentID = @DepartmentID";
                    AddSqlParameter(command, "@DepartmentID", _criteria.DepartmentID, System.Data.SqlDbType.Int);
                }
                if (!string.IsNullOrEmpty(_criteria.DepartmentCode))
                {
                    command.CommandText += " and DepartmentCode = @DepartmentCode";
                    AddSqlParameter(command, "@DepartmentCode", _criteria.DepartmentCode, System.Data.SqlDbType.NVarChar);
                }
                if (!string.IsNullOrEmpty(_criteria.DepartmentName))
                {
                    command.CommandText += " and DepartmentName like N'%" + _criteria.DepartmentName + "%' ";
                }
                command.CommandText += " order by DepartmentID  ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new DepartmentInfo();
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.Address = GetDbReaderValue<string>(reader["Address"]);
                        info.Phone = GetDbReaderValue<string>(reader["Phone"]);
                        info.Email = GetDbReaderValue<string>(reader["Email"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.SourceID = GetDbReaderValue<int>(reader["SourceID"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public int InsertDepartment(SqlConnection connection, DepartmentInfo _department, string _userI)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Department] (DepartmentCode, DepartmentName, Address,Phone,Email,UserI,SourceID)"  +
                    "VALUES(@DepartmentCode, @DepartmentName, @Address, @Phone, @Email, @UserI, @SourceID)  ", connection))
            {
                AddSqlParameter(command, "@DepartmentCode", _department.DepartmentCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@DepartmentName", _department.DepartmentName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Address", _department.Address, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Phone", _department.Phone, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Email", _department.Email, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@SourceID", _department.SourceID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            return lastestInserted;
        }

        public void UpdateDepartment(SqlConnection connection, DepartmentInfo _department, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Department \n" +
                            "SET DepartmentCode = @DepartmentCode, DepartmentName = @DepartmentName, Address = @Address,Phone = @Phone,Email = @Email, UserU=@UserU,UpdateTime=getdate() \n" +
                            "WHERE (DepartmentID = @DepartmentID)", connection))
            {
                AddSqlParameter(command, "@DepartmentID", _department.DepartmentID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DepartmentCode", _department.DepartmentCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@DepartmentName", _department.DepartmentName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Address", _department.Address, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Phone", _department.Phone, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Email", _department.Email, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@SourceID", _department.SourceID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void DeleteDepartment(SqlConnection connection,int _departmentID)
        {
            using (var command = new SqlCommand("delete tbl_Department where DepartmentID=@DepartmentID", connection))
            {
                AddSqlParameter(command, "@DepartmentID", _departmentID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public string GetDepartmentCodeById(SqlConnection connection, int _departmentID)
        {
            string strlSQLGetDepartmentCode = "select DepartmentCode from tbl_Department  " +
             "where DepartmentID = @departmentId ";

            using (var command = new SqlCommand(strlSQLGetDepartmentCode))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqlParameter("@DepartmentID", _departmentID));
                string deparmentcode = (string)db.ExcuteScalar(connection, command);
                return deparmentcode;
            }
          
        }
    }
}
