using AdminPortal.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.DataLayers
{
    public class PracticeDataLayer:BaseLayerData<PracticeDataLayer>
    {
        public PracticeInfo GetUser(SqlConnection connection, string strUser)
        {

            var info = new PracticeInfo();
            using (var command = new SqlCommand("Select id, name" +
                " from dbo.test  where id=@id", connection))
            {
                AddSqlParameter(command, "@id", strUser, System.Data.SqlDbType.VarChar);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    
                    info.UserName = GetDbReaderValue<string>(reader["Name"]);
                    info.UserID = GetDbReaderValue<string>(reader["ID"]);
                    
                }
                return info;
            }
        }
        public List<PracticeInfo> GetUser(SqlConnection connection, string id, PracticeCriteria criteria)
        {
            var result = new List<PracticeInfo>();
            using (var command = new SqlCommand("Select ltrim(rtrim(ID)) as ID, Name  " +
                " from dbo.test where id=@id" +
                " where   1=1", connection))
            {
                AddSqlParameter(command, "@id", id, System.Data.SqlDbType.VarChar);
                if (criteria.pageSize == 0) criteria.pageSize = 10;
                var offSet = criteria.pageIndex * criteria.pageSize;
                command.CommandText += " order by U.Name ";
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", criteria.pageSize, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new PracticeInfo();
                        info.UserName = GetDbReaderValue<string>(reader["Name"]);
                        info.UserID = GetDbReaderValue<string>(reader["ID"]);
                        //      info.Disable = GetDbReaderValue<byte>(reader["GroupID"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<PracticeInfo> Getlist(SqlConnection connection, PracticeCriteria criteria)
        {
            var result = new List<PracticeInfo>();
            using (var command = new SqlCommand("Select ltrim(rtrim(ID)) as ID, Name  " +
                " from dbo.test " +
                " where   1=1 ", connection))
            {
                if (criteria.UserName != "" && criteria.UserName != null)
                {
                    command.CommandText += " and Name like  '%" + criteria.UserName + "%' ";
                }
                if (criteria.UserID != "" && criteria.UserID != null)
                {
                    command.CommandText += " and ID = @ID ";
                    AddSqlParameter(command, "@ID", criteria.UserID, System.Data.SqlDbType.VarChar);
                }

                if (criteria.pageSize == 0) criteria.pageSize = 50;
                var offSet = criteria.pageIndex * criteria.pageSize;
                command.CommandText += " order by Name ";
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";             
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", criteria.pageSize, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new PracticeInfo();
                        info.UserName = GetDbReaderValue<string>(reader["Name"]);
                        info.UserID = GetDbReaderValue<string>(reader["ID"]);
                        //      info.Disable = GetDbReaderValue<byte>(reader["GroupID"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public int GetTotalRecords(SqlConnection connection, PracticeCriteria criteria)
        {
            if (criteria != null)
            {
                var result = new List<PracticeInfo>();
                using (var command = new SqlCommand("Select count(*)  as TotalRecords  " +
                    " from dbo.test U " +
                    " where   1=1 ", connection))
                {

                    if (criteria.UserName != "" && criteria.UserName != null)
                    {
                        command.CommandText += " and U.Name like  '%" + criteria.UserName + "%' ";
                    }



                    if (criteria.UserID != "" && criteria.UserID != null)
                    {
                        command.CommandText += " and ID = @ID ";
                        AddSqlParameter(command, "@ID", criteria.UserID, System.Data.SqlDbType.VarChar);
                    }

                    WriteLogExecutingCommand(command);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return GetDbReaderValue<int>(reader["TotalRecords"]);
                        }
                    }
                }

            }
            else
            {
                using (var command = new SqlCommand("Select count(*) as TotalRecords  from dbo.test where 1 = 1 ", connection))
                {
                    WriteLogExecutingCommand(command);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return GetDbReaderValue<int>(reader["TotalRecords"]);
                        }
                    }
                }
            }
            return 0;
        }
        public void DeleteUser(SqlConnection connection, string id)
        {
            using (var command = new SqlCommand("delete from dbo.test " +
                 " where  ID=@UserID "
                 , connection))
            {
                AddSqlParameter(command, "@UserID", id, System.Data.SqlDbType.NVarChar);

                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public void DeleteAllUser(SqlConnection connection, string ids)
        {
            using (var command = new SqlCommand("delete from dbo.test " +
                 " where  ID in ("+ids+") "
                 , connection))
            {
                AddSqlParameter(command, "@ids", ids, System.Data.SqlDbType.NVarChar);

                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public void CreateUser(SqlConnection connection, PracticeInfo user)
        {
            using (var command = new SqlCommand("Insert into dbo.test " +
                 " (ID ,Name)" +
                     "VALUES " +
                     "(@ID,@Name) ", connection))
            {
                AddSqlParameter(command, "@ID", user.UserID, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Name", user.UserName, System.Data.SqlDbType.NVarChar);                                
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

        }
        public void UpdateUser(SqlConnection connection,PracticeInfo user)
        {
            using (var command = new SqlCommand("update  dbo.test " +
                 " SET  Name=@UserName " +
                 "where ID=@UserId "
                 , connection))
            {
                AddSqlParameter(command, "@UserID", user.UserID, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserName", user.UserName, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void CreateConfirm(SqlConnection connection, ConfirmInfo value)
        {
            using (var command = new SqlCommand(" INSERT INTO [dbo].[tbl_Confirm] " +
                " (AnalyzerCode,AnalyzerName,EmployeeID,Name,DepartmentID,DepartmentName) " +
                " VALUES(@AnalyzerCode,(select AnalyzerName from tbl_Analyzer where AnalyzerCode=@AnalyzerCode), " +
                " @EmployeeID,@Name,@DepartmentID,(select DepartmentName from tbl_Department where DepartmentID=@DepartmentID)) ", connection))
            {
                AddSqlParameter(command, "@AnalyzerCode", value.AnalyzerCode, System.Data.SqlDbType.NVarChar);
                //AddSqlParameter(command, "@AnalyzerName", value.AnalyzerName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@EmployeeID", value.EmployeeID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Name", value.Name, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DepartmentID", value.DepartmentID, System.Data.SqlDbType.Int);
                //AddSqlParameter(command, "@DepartmentName", value.DepartmentName, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

        }
    }
}
