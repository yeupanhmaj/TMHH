using AdminPortal.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.DataLayers
{
    public class BranchDataLayer:BaseLayerData<BranchDataLayer>
    {

        public List<BranchInfo> Search(SqlConnection connection, string query,int position, int size)
        {
            var result = new List<BranchInfo>();
            using (var command = new SqlCommand("select * from [dbo].[tbl_Branch] " +
                " where BranchID = @BranchID " +
                " or BranchName like concat('%',@BranchName,'%') " +
                " or BranchAddress like concat('%',@BranchAddress,'%') " +
                " or BranchPhone like concat('%',@BranchPhone,'%') " +
                " order by BranchID "+
                " OFFSET @position ROWS FETCH NEXT @size ROWS ONLY ", connection))
            {
                if(int.TryParse(query,out int tmp))
                {
                    AddSqlParameter(command, "@BranchID", query, System.Data.SqlDbType.Int);
                }
                else
                {
                    AddSqlParameter(command, "@BranchID", 0, System.Data.SqlDbType.Int);
                }
                AddSqlParameter(command, "@BranchName", query, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BranchAddress", query, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BranchPhone", query, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@position", position, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@size", size, System.Data.SqlDbType.Int);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new BranchInfo();
                        info.BranchID = GetDbReaderValue<int>(reader["BranchID"]);
                        info.BranchName = GetDbReaderValue<string>(reader["BranchName"]);
                        info.BranchAddress = GetDbReaderValue<string>(reader["BranchAddress"]);
                        info.BranchPhone = GetDbReaderValue<string>(reader["BranchPhone"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public int totalRecordSearch(SqlConnection connection, string query)
        {
            int total = 0;
            using (var command = new SqlCommand("select count(*) from [dbo].[tbl_Branch] " +
                " where BranchID = @BranchID " +
                " or BranchName like concat('%',@BranchName,'%') " +
                " or BranchAddress like concat('%',@BranchAddress,'%') " +
                " or BranchPhone like concat('%',@BranchPhone,'%') ", connection))
            {
                if (int.TryParse(query, out int tmp))
                {
                    AddSqlParameter(command, "@BranchID", query, System.Data.SqlDbType.Int);
                }
                else
                {
                    AddSqlParameter(command, "@BranchID", 0, System.Data.SqlDbType.Int);
                }
                AddSqlParameter(command, "@BranchName", query, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BranchAddress", query, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BranchPhone", query, System.Data.SqlDbType.NVarChar);
                total = (int)command.ExecuteScalar();
            }
            return total;
        }
        public BranchInfo GetBranchById(SqlConnection connection, int branchID)
        {
            var result = new List<BranchInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Branch where  BranchID=@BranchID", connection))
            {
                AddSqlParameter(command, "@BranchID", branchID, System.Data.SqlDbType.NVarChar);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new BranchInfo();
                        info.BranchID = GetDbReaderValue<int>(reader["BranchID"]);
                        info.BranchName = GetDbReaderValue<string>(reader["BranchName"]);
                        info.BranchAddress = GetDbReaderValue<string>(reader["BranchAddress"]);
                        info.BranchPhone = GetDbReaderValue<string>(reader["BranchPhone"]);
                        result.Add(info);
                    }
                }
                return result.FirstOrDefault();
            }
        }
        public int GetTatalRecordBranch(SqlConnection connection)
        {
            int total = 0;
            using (var command = new SqlCommand("Select count(*) " +
                " from tbl_Branch where  1 = 1", connection))
            {
                total = (int)command.ExecuteScalar();
                return total;
            }
        }
        public List<BranchInfo> GetAllBranch(SqlConnection connection,int position, int size)
        {
            var result = new List<BranchInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Branch where  1 = 1 order by BranchID " +
                " OFFSET @position ROWS FETCH NEXT @size ROWS ONLY ", connection))
            {
                AddSqlParameter(command, "@position", position, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@size", size, System.Data.SqlDbType.Int);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new BranchInfo();
                        info.BranchID = GetDbReaderValue<int>(reader["BranchID"]);
                        info.BranchName = GetDbReaderValue<string>(reader["BranchName"]);
                        info.BranchAddress = GetDbReaderValue<string>(reader["BranchAddress"]);
                        info.BranchPhone = GetDbReaderValue<string>(reader["BranchPhone"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int InsertBranch(SqlConnection connection, BranchInfo branch)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Branch] (BranchName, BranchAddress, BranchPhone)" +
                    "VALUES(@BranchName, @BranchAddress, @BranchPhone)  ", connection))
            {
                AddSqlParameter(command, "@BranchName", branch.BranchName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BranchAddress", branch.BranchAddress, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BranchPhone", branch.BranchPhone, System.Data.SqlDbType.NChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            return lastestInserted;
        }

        public int UpdateBranch(SqlConnection connection, BranchInfo branch)
        {
            int record = 0;
            using (var command = new SqlCommand("Update [dbo].[tbl_Branch] " +
                    " set BranchName=@BranchName, BranchAddress=@BranchAddress, BranchPhone=@BranchPhone" +
                    " where BranchID=@BranchID  ", connection))
            {
                AddSqlParameter(command, "@BranchID", branch.BranchID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BranchName", branch.BranchName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BranchAddress", branch.BranchAddress, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BranchPhone", branch.BranchPhone, System.Data.SqlDbType.NChar);
                WriteLogExecutingCommand(command);
                record = command.ExecuteNonQuery();
            }

            return record;
        }

        public int DeleteBranch(SqlConnection connection, int branchId)
        {
            int record = 0;
            using (var command = new SqlCommand("Delete from [dbo].[tbl_Branch] " +
                    " where BranchID=@BranchID  ", connection))
            {
                AddSqlParameter(command, "@BranchID", branchId, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                record = command.ExecuteNonQuery();
            }

            return record;
        }

        public int DeleteManyBranch(SqlConnection connection, string ids)
        {
            int record = 0;
            using (var command = new SqlCommand("Delete from [dbo].[tbl_Branch] " +
                    " where BranchID in (" + ids + ")  ", connection))
            {
                AddSqlParameter(command, "@BranchID", ids, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                record = command.ExecuteNonQuery();
            }

            return record;
        }
    }
}
