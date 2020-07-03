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
    public class EmployeeRoleDataLayer : BaseLayerData<EmployeeRoleDataLayer>
    {
        DataProvider db = new DataProvider();
        
        public List<EmployeeRoleInfo> GetAllEmployeeRole(SqlConnection connection)
        {
            var result = new List<EmployeeRoleInfo>();
            using (var command = new SqlCommand("Select * from tbl_EmployeeRoles  where  1 = 1 order by RoleID ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new EmployeeRoleInfo();
                        info.RoleID = GetDbReaderValue<int>(reader["RoleID"]);
                        info.RoleName = GetDbReaderValue<string>(reader["RoleName"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
       
        public int InsertEmployeeRole(SqlConnection connection, EmployeeRoleInfo _EmployeeRole)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_EmployeeRoles] ( RoleName)" +
                    "VALUES( @RoleName)  ", connection))
            {
                //AddSqlParameter(command, "@RoleID", _EmployeeRole.RoleID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@RoleName", _EmployeeRole.RoleName, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            return lastestInserted;
        }

        public void UpdateEmployeeRole(SqlConnection connection, EmployeeRoleInfo _EmployeeRole)
        {
            using (var command = new SqlCommand("UPDATE tbl_EmployeeRoles \n" +
                            "SET RoleName = @RoleName \n" +
                            "WHERE (RoleID = @RoleID)", connection))
            {
                AddSqlParameter(command, "@RoleID", _EmployeeRole.RoleID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@RoleName", _EmployeeRole.RoleName, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void DeleteEmployeeRole(SqlConnection connection, int _roleID)
        {
            using (var command = new SqlCommand("delete tbl_EmployeeRoles where RoleID=@RoleID", connection))
            {
                AddSqlParameter(command, "@RoleID", _roleID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
    }
}
