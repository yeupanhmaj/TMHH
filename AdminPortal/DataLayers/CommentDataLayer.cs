using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class CommentDataLayer : BaseLayerData<CommentDataLayer>
    {
        DataProvider db = new DataProvider();
       
        public List<CommentInfo> getComment(SqlConnection connection, CommentSeachCriteria _criteria)
        {
            var result = new List<CommentInfo>();
            using (var command = new SqlCommand("Select C.*, U.UserName  " +
                " from tbl_Comment C left join tbl_User U on C.UserI = U.UserID where 1 = 1 ", connection))
            {
                if (!string.IsNullOrEmpty(_criteria.TableName))
                {
                    command.CommandText += " and TableName = @TableName";
                    AddSqlParameter(command, "@TableName", _criteria.TableName, System.Data.SqlDbType.VarChar);
                }
                if (!string.IsNullOrEmpty(_criteria.PreferId))
                {
                    command.CommandText += " and PreferId = @PreferId";
                    AddSqlParameter(command, "@PreferId", _criteria.PreferId, System.Data.SqlDbType.Int);
                }
                command.CommandText += " order by InTime  ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new CommentInfo();
                        info.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        info.TableName = GetDbReaderValue<string>(reader["TableName"]);
                        info.PreferId = GetDbReaderValue<int>(reader["PreferId"]).ToString();
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.UserName = GetDbReaderValue<string>(reader["UserName"]);
                        info.Intime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int insertComment(SqlConnection connection, CommentInfo _insertInfo, string _UserI)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Comment] (TableName, PreferId, Comment, UserI)" +
                    "VALUES(@TableName,@PreferId, @Comment, @UserI)  " +
                    " select IDENT_CURRENT('dbo.tbl_Comment') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@TableName", _insertInfo.TableName, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@PreferId", _insertInfo.PreferId, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Comment", _insertInfo.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserI", _UserI, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }

            return lastestInserted;
        }

        public void DeleteComment(SqlConnection connection, CommentSeachCriteria _criteria)
        {
            using (var command = new SqlCommand(" delete tbl_Comment where TableName=@TableName and PreferId = @PreferId ", connection))
            {
                AddSqlParameter(command, "@TableName", _criteria.TableName, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@PreferId", _criteria.PreferId, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteComment(SqlConnection connection, int _CommentID)
        {
            using (var command = new SqlCommand("delete tbl_Document where PreferId = @CommentID and TableName = 'Comment' " +
                " delete tbl_Comment where AutoID=@CommentID", connection))
            {
                AddSqlParameter(command, "@CommentID", _CommentID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public void DeleteComments(SqlConnection connection, string _CommentIDs)
        {
            using (var command = new SqlCommand("delete tbl_Document where PreferId in (" + _CommentIDs + ") and TableName = 'Comment' " +
                " delete tbl_Comment where AutoID in (" + _CommentIDs + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
    }
}
