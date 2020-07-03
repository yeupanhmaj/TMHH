using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class EmployeeDataLayer : BaseLayerData<EmployeeDataLayer>
    {
        DataProvider db = new DataProvider();
        public List<EmployeeInfo> GetEmployeeByCondition(SqlConnection connection, string name)
        {
            var result = new List<EmployeeInfo>();
            var sqlQuery = @" Select E.* from tbl_employee E  where 1 = 1";
            using (var command = new SqlCommand(sqlQuery, connection))
            {

                if (!string.IsNullOrEmpty(name))
                {
                    command.CommandText += "and FREETEXT(E.Name,@name)"; 
                    AddSqlParameter(command, "@name", '%' + name + '%' , System.Data.SqlDbType.NVarChar);
                }
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new EmployeeInfo();
                        info.EmployeeID = GetDbReaderValue<int>(reader["EmployeeID"]);
          
                        info.Title = GetDbReaderValue<string>(reader["Title"]);
                        info.RoleName = GetDbReaderValue<string>(reader["RoleName"]);
                        info.Name = GetDbReaderValue<string>(reader["Name"]);
                        info.Generic = GetDbReaderValue<int>(reader["Generic"]);
                        info.DepartmentID = GetDbReaderValue<string>(reader["DepartmentID"]);
                        info.UserID = GetDbReaderValue<string>(reader["UserID"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<EmployeeInfo> GetAllEmployee(SqlConnection connection)
        {
            var result = new List<EmployeeInfo>();
            using (var command = new SqlCommand("Select E.* " +
                " from tbl_employee E  where  1 = 1 order by EmployeeID ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new EmployeeInfo();
                        info.EmployeeID = GetDbReaderValue<int>(reader["EmployeeID"]);
        
                        info.Title = GetDbReaderValue<string>(reader["Title"]);
                        info.RoleName = GetDbReaderValue<string>(reader["RoleName"]);
                        info.Name = GetDbReaderValue<string>(reader["Name"]);
                        info.Generic = GetDbReaderValue<int>(reader["Generic"]);
                        info.DepartmentID = GetDbReaderValue<string>(reader["DepartmentID"]);
                        info.UserID = GetDbReaderValue<string>(reader["UserID"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public List<EmployeeInfo> GetAllEmployeebyID(SqlConnection connection, int id)
        {
            var result = new List<EmployeeInfo>();
            using (var command = new SqlCommand("Select E.* " +
                " from tbl_employee E where  EmployeeID = @EmployeeID order by EmployeeID ", connection))
            {
                AddSqlParameter(command, "@EmployeeID", id, System.Data.SqlDbType.Int);
                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        var info = new EmployeeInfo();
                        info.EmployeeID = GetDbReaderValue<int>(reader["EmployeeID"]);
   
                        info.Title = GetDbReaderValue<string>(reader["Title"]);
                        info.RoleName = GetDbReaderValue<string>(reader["RoleName"]);
                        info.Name = GetDbReaderValue<string>(reader["Name"]);
                        info.Generic = GetDbReaderValue<int>(reader["Generic"]);
                        info.DepartmentID = GetDbReaderValue<string>(reader["DepartmentID"]);
                        info.UserID = GetDbReaderValue<string>(reader["UserID"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int InsertEmployee(SqlConnection connection, EmployeeInfo _employee, string _userI)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Employee] (RoleName, Title, Name, Generic,DepartmentID)" +
                    "VALUES(@RoleName, @Title, @Name, @Generic, @DepartmentID)  ", connection))
            {
                AddSqlParameter(command, "@RoleName", _employee.RoleName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Title", _employee.Title, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Name", _employee.Name, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Generic", _employee.Generic, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DepartmentID", _employee.DepartmentID, System.Data.SqlDbType.Int);
                //AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            return lastestInserted;
        }

        public void UpdateEmployee(SqlConnection connection, EmployeeInfo _employee, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Employee \n" +
                            "SET RoleName = @RoleName, Name = @Name, Generic = @Generic,DepartmentID = @DepartmentID, Title = @Title \n" +
                            "WHERE (EmployeeID = @EmployeeID)", connection))
            {
                AddSqlParameter(command, "@EmployeeID", _employee.EmployeeID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Title", _employee.Title, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@RoleName", _employee.RoleName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Name", _employee.Name, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Generic", _employee.Generic, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DepartmentID", _employee.DepartmentID, System.Data.SqlDbType.Int);
                //AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void DeleteEmployee(SqlConnection connection, int _employeeID)
        {
            using (var command = new SqlCommand("delete tbl_Employee where EmployeeID=@DEmployeeID", connection))
            {
                AddSqlParameter(command, "@DEmployeeID", _employeeID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
    }
}
