using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class AnalyzerGroupDataLayer : BaseLayerData<AnalyzerGroupDataLayer>
    {

        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả nhóm tài sản
        /// </summary>
        /// <param name="connection"> </param>
        /// <returns>Return List<AnalyzerGroupInfo></returns>
        /// 
        public List<AnalyzerGroupInfo> GetAllAnalyzerGroup(SqlConnection connection, AnalyzerGroupSeachCriteria _criteria)
        {
            var result = new List<AnalyzerGroupInfo>();
            using (var command = new SqlCommand("Select ANG.* " +
                " from tbl_AnalyzerGroup ANG  " +
                " where  1 = 1 order by ANG.AnalyzerGroupName Desc "
                , connection))
            {
                if (_criteria.pageSize == 0) _criteria.pageSize = 10;
                var offSet = _criteria.pageIndex * _criteria.pageSize;
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", _criteria.pageSize, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new AnalyzerGroupInfo();
                        info.AnalyzerGroupID = GetDbReaderValue<int>(reader["AnalyzerGroupID"]);
                        info.AnalyzerGroupCode = GetDbReaderValue<string>(reader["AnalyzerGroupCode"]);
                        info.AnalyzerGroupName = GetDbReaderValue<string>(reader["AnalyzerGroupName"]);
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

        /// <summary>
        /// Hàm lấy nhóm tài sản theo ID
        /// </summary>
        /// <param ID="_ID"></param>
        /// <returns>Return List<AnalyzerGroupInfo></returns>
        /// 
        public AnalyzerGroupInfo getAnalyzerGroup(SqlConnection connection, int _ID)
        {
            AnalyzerGroupInfo result = null;
            using (var command = new SqlCommand(
                " Select ANG.* " +
                "  from tbl_AnalyzerGroup ANG where ANG.AnalyzerGroupID = @AnalyzerGroupID "
            , connection))
            {
                AddSqlParameter(command, "@AnalyzerGroupID", _ID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new AnalyzerGroupInfo();
                        result.AnalyzerGroupID = GetDbReaderValue<int>(reader["AnalyzerGroupID"]);
                        result.AnalyzerGroupCode = GetDbReaderValue<string>(reader["AnalyzerGroupCode"]);
                        result.AnalyzerGroupName = GetDbReaderValue<string>(reader["AnalyzerGroupName"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Hàm lấy nhóm tài sản theo Code
        /// </summary>
        /// <param AnalyzerGroupCode="_code"></param>
        /// <returns>Return List<AnalyzerGroupInfo></returns>
        /// 
        public AnalyzerGroupInfo GetAnalyzerGroupByCode(SqlConnection connection, string _code)
        {
            AnalyzerGroupInfo result = null;
            using (var command = new SqlCommand(
               " Select  ANG.* " +
               " from tbl_AnalyzerGroup AN where AN.AnalyzerGroupCode = @AnalyzerGroupCode" 
           , connection))
            {
                AddSqlParameter(command, "@AnalyzerGroupCode", _code, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new AnalyzerGroupInfo();
                        result.AnalyzerGroupID = GetDbReaderValue<int>(reader["AnalyzerGroupID"]);
                        result.AnalyzerGroupCode = GetDbReaderValue<string>(reader["AnalyzerGroupCode"]);
                        result.AnalyzerGroupName = GetDbReaderValue<string>(reader["AnalyzerGroupName"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Hàm lấy số record theo điều kiện
        /// </summary>
        /// <param Criteria="_criteria"></param>
        /// <returns>Return List<AnalyzerGroupInfo></returns>
        /// 
        public int getTotalRecords(SqlConnection connection, AnalyzerGroupSeachCriteria _criteria)
        {
            if (_criteria != null)
            {
                using (var command = new SqlCommand("Select count(ANG.AnalyzerGroupID) as TotalRecords  " +
                " from tbl_AnalyzerGroup ANG where  1 = 1 ", connection))
                {
                 
                    if (!string.IsNullOrEmpty(_criteria.AnalyzerGroupCode))
                    {
                        command.CommandText += " and ANG.AnalyzerGroupCode = @AnalyzerGroupCode";
                        AddSqlParameter(command, "@AnalyzerGroupCode", _criteria.AnalyzerGroupCode, System.Data.SqlDbType.NVarChar);
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
                using (var command = new SqlCommand("Select count(*) as TotalRecords  from tbl_AnalyzerGroup where 1 = 1 ", connection))
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

        /// <summary>
        /// Hàm Insert Tài sản
        /// </summary>
        /// <param AnalyzerGroupInfo="_AnalyzerGroup"></param>
        /// <param userInput="_userI"></param>
        /// <returns>Return List<AnalyzerGroupInfo></returns>
        /// 
        public int InsertAnalyzerGroup(SqlConnection connection, AnalyzerGroupInfo _AnalyzerGroup, string _userI)
        {
            int lastestInserted = 0;
            var currenttime = DateTime.Now.Date;
            if (_AnalyzerGroup.AnalyzerGroupCode == null || _AnalyzerGroup.AnalyzerGroupCode == "") _AnalyzerGroup.AnalyzerGroupCode = DateTime.Now.ToString("yyMMddHHmmssfff");
            using (var command = new SqlCommand("Insert into [dbo].[tbl_AnalyzerGroup] (AnalyzerGroupCode,AnalyzerGroupName, UserI, UserU, UpdateTime)" +
                    "VALUES(@AnalyzerGroupCode, @AnalyzerGroupName , @UserI, @UserI, Getdate()) " +
                    "select IDENT_CURRENT('dbo.tbl_AnalyzerGroup') as LastInserted ", connection))
            {
               
                AddSqlParameter(command, "@AnalyzerGroupCode", _AnalyzerGroup.AnalyzerGroupCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@AnalyzerGroupName", _AnalyzerGroup.AnalyzerGroupName, System.Data.SqlDbType.NVarChar);
              
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }

            return lastestInserted;
        }

        /// <summary>
        /// Hàm Update Tài sản
        /// </summary>
        /// <param AnalyzerGroupInfo="_AnalyzerGroup"></param>
        /// <param userInput="_userI"></param>
        /// <returns>Return List<AnalyzerGroupInfo></returns>
        /// 
        public void UpdateAnalyzerGroup(SqlConnection connection, int _id, AnalyzerGroupInfo _AnalyzerGroup, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_AnalyzerGroup \n" +
                            " SET  AnalyzerGroupCode = @AnalyzerGroupCode , AnalyzerGroupName = @AnalyzerGroupName  " +
                            ", UserU=@UserU,UpdateTime=getdate() \n" +
                            " WHERE (AnalyzerGroupID = @AnalyzerGroupID) ", connection))
            
            {
                AddSqlParameter(command, "@AnalyzerGroupID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@AnalyzerGroupCode", _AnalyzerGroup.AnalyzerGroupCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@AnalyzerGroupName", _AnalyzerGroup.AnalyzerGroupName, System.Data.SqlDbType.NVarChar);
           

                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void Delete(SqlConnection connection,int _AnalyzerGroupID)
        {
            using (var command = new SqlCommand(" Delete tbl_AnalyzerGroup where AnalyzerGroupID=@AnalyzerGroupID ", connection))
            {
                AddSqlParameter(command, "@AnalyzerGroupID", _AnalyzerGroupID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string _AnalyzerGroupIDs)
        {
            using (var command = new SqlCommand(" Delete tbl_AnalyzerGroup where AnalyzerGroupID in (" + _AnalyzerGroupIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public List<string> getListAnalyzerGroupCode(SqlConnection connection, string AnalyzerGroupCode)
        {
            var result = new List<string>();
            using (var command = new SqlCommand(" Select TOP 10 AnalyzerGroupCode from tbl_AnalyzerGroup where AnalyzerGroupCode like '%" + AnalyzerGroupCode + "%'", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _AnalyzerGroupCode = GetDbReaderValue<string>(reader["AnalyzerGroupCode"]);
                        result.Add(_AnalyzerGroupCode);
                    }
                }
                return result;
            }
        }
    }
}
