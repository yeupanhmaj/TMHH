using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using AdminPortal.Commons;


namespace AdminPortal.DataLayer
{
    public class DocumentDataLayer : BaseLayerData<DocumentDataLayer>
    {
        DataProvider db = new DataProvider();
       
        public List<DocumentInfo> getDocument(SqlConnection connection, DocumentSeachCriteria _criteria)
        {
            var result = new List<DocumentInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Document where  1 = 1 ", connection))
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
                        var info = new DocumentInfo();
                        info.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        info.TableName = GetDbReaderValue<string>(reader["TableName"]);
                        info.PreferId = GetDbReaderValue<int>(reader["PreferId"]).ToString();
                        info.Link = GetDbReaderValue<string>(reader["Link"]);
                        info.FileName = GetDbReaderValue<string>(reader["FileName"]);
                        info.Length = GetDbReaderValue<string>(reader["Length"]);
                        info.Type = GetDbReaderValue<string>(reader["Type"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public void DeleteDocumentByFeatureAndID(SqlConnection connection, string feature, int id)
        {
            using (var command = new SqlCommand("delete tbl_Document where PreferId=@PreferId and TableName=@TableName", connection))
            {
                AddSqlParameter(command, "@PreferId", id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@TableName", feature, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        
        public int InsertDocument(SqlConnection connection, DocumentInfo documentInfo, string _userI)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Document] ([TableName], [PreferId],[Link], [FileName], [Length], [Type], [UserI])" +
                    "VALUES(@TableName,@PreferId,@Link,@FileName, @Length,@Type, @UserI) " +
                    " select IDENT_CURRENT('dbo.tbl_Document') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@TableName", documentInfo.TableName, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@PreferId", documentInfo.PreferId, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Link", documentInfo.Link, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@FileName", documentInfo.FileName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Length", documentInfo.Length, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Type", documentInfo.Type, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                object lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }

            return lastestInserted;
        }

        public List<DocumentInfo> GetDocumentsByIds(SqlConnection connection, string _DocumentIDs)
        {
           var result = new List<DocumentInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Document where AutoID in (" + _DocumentIDs + ")", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new DocumentInfo();
                        info.AutoID = GetDbReaderValue<int >(reader["AutoID"]);
                        info.TableName = GetDbReaderValue<string>(reader["TableName"]);
                        info.PreferId = GetDbReaderValue<int>(reader["PreferId"]).ToString();
                        info.Link = GetDbReaderValue<string>(reader["Link"]);
                        info.FileName = GetDbReaderValue<string>(reader["FileName"]);
                        info.Length = GetDbReaderValue<string>(reader["Length"]);
                        info.Type = GetDbReaderValue<string>(reader["Type"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public DocumentInfo GetDocumentById(SqlConnection connection, int _DocumentID)
        {
            var result = new DocumentInfo();
            using (var command = new SqlCommand("Select * " +
               " from tbl_Document where AutoID = @AutoID", connection))
            {          
                AddSqlParameter(command, "@AutoID", _DocumentID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        result.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        result.TableName = GetDbReaderValue<string>(reader["TableName"]);
                        result.PreferId = GetDbReaderValue<int>(reader["PreferId"]).ToString();
                        result.Link = GetDbReaderValue<string>(reader["Link"]);
                        result.FileName = GetDbReaderValue<string>(reader["FileName"]);
                        result.Length = GetDbReaderValue<string>(reader["Length"]);
                        result.Type = GetDbReaderValue<string>(reader["Type"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                    }
                }
            }
            return result;
        }

        public void DeleteDocument(SqlConnection connection, int _DocumentID)
        {
            using (var command = new SqlCommand("delete tbl_Document where AutoID=@DocumentID", connection))
            {
                AddSqlParameter(command, "@DocumentID", _DocumentID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public void DeleteDocuments(SqlConnection connection, string _DocumentIDs)
        {
            using (var command = new SqlCommand(" delete tbl_Document where AutoID in (" + _DocumentIDs + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public List<DocumentInfo> GetAllDocumentsByProposalID(SqlConnection connection, int proposalID)
        {
            var result = new List<DocumentInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_Document where  TableName = 'Proposal' ", connection))
            {
                command.CommandText += " and PreferId = @PreferId";
                AddSqlParameter(command, "@PreferId", proposalID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new DocumentInfo();
                        info.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        info.TableName = GetDbReaderValue<string>(reader["TableName"]);
                        info.PreferId = GetDbReaderValue<int>(reader["PreferId"]).ToString();
                        info.Link = GetDbReaderValue<string>(reader["Link"]);
                        info.FileName = GetDbReaderValue<string>(reader["FileName"]);
                        info.Length = GetDbReaderValue<string>(reader["Length"]);
                        info.Type = GetDbReaderValue<string>(reader["Type"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.Add(info);
                    }
                }          
            }
            return result;
        }
    }
}
