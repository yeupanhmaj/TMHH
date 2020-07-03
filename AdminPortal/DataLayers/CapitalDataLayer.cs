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
    public class CapitalDataLayer : BaseLayerData<CapitalDataLayer>
    {
        DataProvider db = new DataProvider();
        
        public List<CapitalInfo> GetAllCapital(SqlConnection connection)
        {
            var result = new List<CapitalInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Capital  where  1 = 1 and isused = 1 order by CapitalID ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new CapitalInfo();
                        info.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                        info.CapitalName = GetDbReaderValue<string>(reader["CapitalName"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<CapitalInfo> GetByName(SqlConnection connection, string name)
        {
            var result = new List<CapitalInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Capital  where  CapitalName like '%" + name + "%'  and isused = 1 order by CapitalID ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new CapitalInfo();
                        info.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                        info.CapitalName = GetDbReaderValue<string>(reader["CapitalName"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<CapitalInfo> GetAllCapitalbyID(SqlConnection connection, int id)
        {
            var result = new List<CapitalInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Capital  where  CapitalID = @CapitalID  AND isused = 1 order by CapitalID ", connection))
            {
                AddSqlParameter(command, "@CapitalID", id, System.Data.SqlDbType.Int);
                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        var info = new CapitalInfo();
                        info.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                        info.CapitalName = GetDbReaderValue<string>(reader["CapitalName"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int InsertCapital(SqlConnection connection, CapitalInfo _capital)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Capital] (CapitalName)" +
                    "VALUES(@CapitalName)  ", connection))
            {
                AddSqlParameter(command, "@CapitalName", _capital.CapitalName, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            return lastestInserted;
        }

        public void UpdateCapital(SqlConnection connection, CapitalInfo _capital)
        {
            using (var command = new SqlCommand("UPDATE tbl_Capital \n" +
                            "SET CapitalName = @CapitalName  \n" +
                            "WHERE (CapitalID = @CapitalID)", connection))
            {
                AddSqlParameter(command, "@CapitalID", _capital.CapitalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CapitalName", _capital.CapitalName, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void DeleteCapital(SqlConnection connection, int _employeeID)
        {
            using (var command = new SqlCommand("UPDATE tbl_Capital set IsUsed = 0 where EmployeeID=@DEmployeeID", connection))
            {
                AddSqlParameter(command, "@DEmployeeID", _employeeID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
    }
}
