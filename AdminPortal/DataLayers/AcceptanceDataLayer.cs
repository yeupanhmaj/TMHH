using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class AcceptanceDataLayer : BaseLayerData<AcceptanceDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<AuditInfo></returns>
        /// 
        public List<AcceptanceInfo> Getlist(SqlConnection connection, AcceptanceCriteria _criteria, string _userID)
        {
            var result = new List<AcceptanceInfo>();
            using (var command = new SqlCommand("Select A.*, P.ProposalCode, D.DepartmentName , D1.DepartmentName as CurDepartmentName" +
                " from tbl_Acceptance A  " +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = A.ProposalID " +
                " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
                " left join tbl_Department D1 on P.CurDepartmentID = D1.DepartmentID " +
                " where   A.ProposalID <> 0 ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (A.UserAssign =@UserID)";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                if (_criteria.proposalCode != "" && _criteria.proposalCode!= null)
                {
                    command.CommandText += " and P.ProposalCode like  '%" + _criteria.proposalCode + "%' ";
                }
                if (_criteria.departmentID !=0)
                {
                    command.CommandText += " and ( P.departmentID = @departmentID ";
                    command.CommandText += " or  P.CurDepartmentID = @departmentID ) ";
                    AddSqlParameter(command, "@departmentID", _criteria.departmentID, System.Data.SqlDbType.Int);
                }
                if (_criteria.fromDate != null  && _criteria.toDate != null)
                {
                    command.CommandText += " and P.DateIn between @FromDate and @ToDate ";
                    AddSqlParameter(command, "@FromDate", _criteria.fromDate, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@ToDate", _criteria.toDate, System.Data.SqlDbType.DateTime);
                }

                if (_criteria.pageSize == 0) _criteria.pageSize = 10;
                var offSet = _criteria.pageIndex * _criteria.pageSize;
                command.CommandText += " order by A.UpdateTime ";
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", _criteria.pageSize, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new AcceptanceInfo();
                        info.AcceptanceID = GetDbReaderValue<int>(reader["AcceptanceID"]);
                        info.AcceptanceNote = GetDbReaderValue<string>(reader["AcceptanceNote"]);
                        info.AcceptanceCode = GetDbReaderValue<string>(reader["AcceptanceCode"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        info.AcceptanceResult = GetDbReaderValue<int>(reader["AcceptanceResult"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.CreateTime = GetDbReaderValue<DateTime>(reader["CreateTime"]);                   
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int GetTotalRecords(SqlConnection connection, AcceptanceCriteria _criteria,string _userID)
        {
           
                using (var command = new SqlCommand("Select count(temp.AcceptanceID) as TotalRecords from (Select A.*, P.ProposalCode, D.DepartmentName " +
               " from tbl_Acceptance A  " +
               " LEFT JOIN tbl_Proposal P on P.ProposalID  = A.ProposalID " +
               " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
               " where  A.ProposalID <> 0   ", connection))
                {
                    if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                    {
                        command.CommandText += " and (A.UserAssign =@UserID)";
                        AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                    }
                if (_criteria.proposalCode != "" && _criteria.proposalCode != null)
                    {
                        command.CommandText += " and P.ProposalCode like  '%" + _criteria.proposalCode + "%' ";
                    }
                    if (_criteria.departmentID != 0)
                    {
                        command.CommandText += " and ( P.departmentID = @departmentID ";
                        command.CommandText += " or  P.CurDepartmentID = @departmentID ) ";
                        AddSqlParameter(command, "@departmentID", _criteria.departmentID, System.Data.SqlDbType.Int);
                    }
                    if (_criteria.fromDate != null && _criteria.toDate != null)
                    {
                        command.CommandText += " and P.DateIn between @FromDate and @ToDate ";
                        AddSqlParameter(command, "@FromDate", _criteria.fromDate, System.Data.SqlDbType.DateTime);
                        AddSqlParameter(command, "@ToDate", _criteria.toDate, System.Data.SqlDbType.DateTime);
                    }
                    command.CommandText += " ) as temp";
                    WriteLogExecutingCommand(command);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return GetDbReaderValue<int>(reader["TotalRecords"]);
                        }
                    }
                }           
            return 0;
        }

        public AcceptanceInfo GetDetail(SqlConnection connection, int id,string _userID)
        {
            AcceptanceInfo info = new AcceptanceInfo();
            using (var command = new SqlCommand("Select A.*, P.ProposalCode, D1.DepartmentName as CurDepartmentName, D.DepartmentName , DR.DeliveryReceiptID  , DR.DeliveryReceiptCode " +
                " from tbl_Acceptance A  " +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = A.ProposalID " +
                " LEFT JOIN tbl_DeliveryReceipt DR  on DR.ProposalID  = P.ProposalID " +
                " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
                " LEFT JOIN tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID " +
                " where  A.AcceptanceID = @AcceptanceID order by A.UpdateTime ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (A.UserAssign =@UserID)";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@AcceptanceID", id, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info.AcceptanceID = GetDbReaderValue<int>(reader["AcceptanceID"]);
                        info.AcceptanceNote = GetDbReaderValue<string>(reader["AcceptanceNote"]);
                        info.AcceptanceCode = GetDbReaderValue<string>(reader["AcceptanceCode"]);
                        info.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        info.DeliveryReceiptCode = GetDbReaderValue<string>(reader["DeliveryReceiptCode"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        info.AcceptanceResult = GetDbReaderValue<int>(reader["AcceptanceResult"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.CreateTime = GetDbReaderValue<DateTime>(reader["CreateTime"]);
                        info.AcceptanceType = GetDbReaderValue<int>(reader["AcceptanceType"]);
                    }
                }
            }
            return info;
        }

        public int Create(SqlConnection connection, AcceptanceInfo obj, string userID)
        {
            var currenttime = DateTime.Now.Date;
            int lastestInserted = 0;
            DateTime localDate = DateTime.Now;
            using (var command = new SqlCommand("Insert into tbl_Acceptance " +
                " (AcceptanceNote,AcceptanceCode, ProposalID, AcceptanceResult, UserU, CreateTime, AcceptanceType, UserI )" +
                    "VALUES (@AcceptanceNote,@AcceptanceCode, @ProposalID, @AcceptanceResult, @UserU, @CreateTime, @AcceptanceType, @UserI )" +
                    " select IDENT_CURRENT('dbo.tbl_Acceptance') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@AcceptanceNote", obj.AcceptanceNote, System.Data.SqlDbType.NVarChar);

                AddSqlParameter(command, "@AcceptanceCode", "NT-" + obj.ProposalCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ProposalID", obj.ProposalID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@AcceptanceResult", obj.AcceptanceResult, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserU", userID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@UserI", userID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@CreateTime", localDate, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@AcceptanceType", obj.AcceptanceType, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);

                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }


            if (lastestInserted > 0)
            {
                using (var command = new SqlCommand("update  tbl_Proposal_Process " +
                    "set AcceptanceID=@AcceptanceID  , AcceptanceTime=@AcceptanceTime ,  CurrentFeature=@CurrentFeature where ProposalID=@ProposalID", connection))
                {
                    AddSqlParameter(command, "@ProposalID", obj.ProposalID, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@AcceptanceID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@AcceptanceTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "Acceptance", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }
            return lastestInserted;
        }
        
        public int Update(SqlConnection connection, AcceptanceInfo obj, string userID)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Update tbl_Acceptance " +
                " SET AcceptanceNote = @AcceptanceNote, AcceptanceResult = @AcceptanceResult , UserU = @UserU  , AcceptanceType = @AcceptanceType" +
                    " where AcceptanceID = @AcceptanceID ", connection))
            {
                AddSqlParameter(command, "@AcceptanceNote", obj.AcceptanceNote, System.Data.SqlDbType.NVarChar);

                AddSqlParameter(command, "@AcceptanceType", obj.AcceptanceType, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@AcceptanceResult", obj.AcceptanceResult, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserU", userID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@AcceptanceID", obj.AcceptanceID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);

                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }
            return lastestInserted;
        }

      


        public void Delete(SqlConnection connection, int _AcceptanceID)
        {
            using (var command = new SqlCommand("delete tbl_Acceptance where AcceptanceID = @AcceptanceID ", connection))
            {
                AddSqlParameter(command, "@AcceptanceID", _AcceptanceID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            using (var command = new SqlCommand("update  tbl_Proposal_Process " +
                 "set AcceptanceID=null  , AcceptanceTime=null ,  CurrentFeature=@CurrentFeature where AcceptanceID=@AcceptanceID", connection))
            {             
                AddSqlParameter(command, "@AcceptanceID", _AcceptanceID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CurrentFeature", "Decision", System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public void DeleteMuti(SqlConnection connection, string _AcceptanceIDs)
        {
            using (var command = new SqlCommand("delete tbl_Acceptance where AcceptanceID in (" + _AcceptanceIDs + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand("update  tbl_Proposal_Process " +
              "set AcceptanceID=null  , AcceptanceTime=null ,  CurrentFeature=@CurrentFeature where AcceptanceID in (" + _AcceptanceIDs + ")", connection))
            {
                AddSqlParameter(command, "@CurrentFeature", "DeliveryReceipt", System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

        }

        public List<string> GetAcceptanceByQuoteIds(SqlConnection connection, string quoteids)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select AcceptanceID as ID from tbl_Proposal_Process  where QuoteID in (" + quoteids + ")", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(GetDbReaderValue<int>(reader["ID"]).ToString());
                    }
                }
            }
            return result;
        }
    }
}
