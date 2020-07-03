using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;
using AdminPortal.Helpers;
using Microsoft.SqlServer.Management.Smo;
using System.Globalization;

namespace AdminPortal.DataLayer
{
    public class ProposalDataLayer : BaseLayerData<ProposalDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<ProposalInfo></returns>
        /// 
        public List<ProposalInfo> GetAllProposal(SqlConnection connection, string _userID)
        {
            var result = new List<ProposalInfo>();
            using (var command = new SqlCommand("Select P.*, D.DepartmentCode,D.DepartmentName, D1.DepartmentCode as CurDepartmentCode, D1.DepartmentName as CurDepartmentName, PT.TypeName " +
                " from tbl_Proposal P left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID  where 1 = 1 ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if(!string.IsNullOrEmpty(_userID) && _userID != "admin")
                    {
                        command.CommandText += " and (P.UserI = @UserID or P.UserU = @UserID or P.UserAssign =@UserID )";
                        AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                    }
                    command.CommandText += " order by P.UpdateTime Desc ";
                    while (reader.Read())
                    {
                        var info = new ProposalInfo();
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.InCode = GetDbReaderValue<string>(reader["InCode"]);
                        info.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        info.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.UserAssigned = GetDbReaderValue<string>(reader["UserAssigned"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.Status = GetDbReaderValue<int>(reader["Status"]);
                        info.CurDepartmentID = GetDbReaderValue<int>(reader["CurDepartmentID"]);
                        info.CurDepartmentCode = GetDbReaderValue<string>(reader["CurDepartmentCode"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        info.FollowComment = GetDbReaderValue<string>(reader["FollowComment"]);
                        info.Opinion = GetDbReaderValue<string>(reader["Opinion"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
            }
            return result;
        }
        public List<ProposalInfo> getProposalByID(SqlConnection connection, ProposalSeachCriteria _criteria, string _userID)
        {
            var result = new List<ProposalInfo>();
            using (var command = new SqlCommand("Select P.*, D.DepartmentCode, D.DepartmentName,D1.DepartmentCode as CurDepartmentCode,D1.DepartmentName as CurDepartmentName, PT.TypeName " +
                " from tbl_Proposal P left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID where 1 = 1 ", connection))
            {
                if (!string.IsNullOrEmpty(_criteria.ProposalID))
                {
                    command.CommandText += " and P.ProposalID = @ProposalID";
                    AddSqlParameter(command, "@ProposalID", _criteria.ProposalID, System.Data.SqlDbType.Int);
                }
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (P.UserI = @UserID or P.UserU = @UserID or P.UserAssign =@UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                command.CommandText += " order by UpdateTime  ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalInfo();
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.InCode = GetDbReaderValue<string>(reader["InCode"]);
                        info.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        info.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.UserAssigned = GetDbReaderValue<string>(reader["UserAssigned"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.Status = GetDbReaderValue<int>(reader["Status"]);
                        info.CurDepartmentID = GetDbReaderValue<int>(reader["CurDepartmentID"]);
                        info.CurDepartmentCode = GetDbReaderValue<string>(reader["CurDepartmentCode"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        info.FollowComment = GetDbReaderValue<string>(reader["FollowComment"]);
                        info.Opinion = GetDbReaderValue<string>(reader["Opinion"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
            }
            return result;
        }

        public List<ItemPropsalInfo> GetPropsalItemsByIds(SqlConnection connection, string ids)
        {
            List<ItemPropsalInfo> ret = new List<ItemPropsalInfo>();
            using (var command = new SqlCommand(@"select AutoID, tpi.ItemID, tpi.amount, tpi.note, tpi.IsExceedReserve, tpi.NumExceedReserve,tpi.IsReservered, i.ItemName from
                (select * from tbl_Proposal_Item   where ProposalID in (" + ids + @") )tpi
                inner join  tbl_items as i on tpi.ItemID = i.ItemID
                ", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new ItemPropsalInfo();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemId"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.Note = GetDbReaderValue<string>(reader["Note"]);
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        item.IsExceedReserve = GetDbReaderValue<bool>(reader["IsExceedReserve"]);
                        item.IsReservered = GetDbReaderValue<bool>(reader["IsReservered"]);
                        item.NumExceedReserve = Convert.ToDouble(GetDbReaderValue<object>(reader["NumExceedReserve"]));
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }


        public ProposalDetailInfo getProposalDetail(SqlConnection connection, int ID, string _userID)
        {
            ProposalDetailInfo result = null;
            using (var command = new SqlCommand(" select P.*,D.DepartmentCode,  D.DepartmentName, D1.DepartmentCode as CurDepartmentCode, D1.DepartmentName as CurDepartmentName, PT.TypeName from (select * from tbl_Proposal  where ProposalID = @ProposalID) as P  " +
                " left join tbl_Department D on P.DepartmentID = D.DepartmentID  " +
                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID  " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID  where 1 = 1 ", connection))
            {
                AddSqlParameter(command, "@ProposalID", ID, System.Data.SqlDbType.Int);
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (P.UserI = @UserID or P.UserU = @UserID or P.UserAssign =@UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                command.CommandText += " order by P.UpdateTime  ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        result = new ProposalDetailInfo();
                        result.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        result.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        result.InCode = GetDbReaderValue<string>(reader["InCode"]);
                        result.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        result.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        result.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        result.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        result.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        result.UserAssigned = GetDbReaderValue<string>(reader["UserAssigned"]);
                        result.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        result.Status = GetDbReaderValue<int>(reader["Status"]);
                        result.CurDepartmentID = GetDbReaderValue<int>(reader["CurDepartmentID"]);
                        result.CurDepartmentCode = GetDbReaderValue<string>(reader["CurDepartmentCode"]);
                        result.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        result.FollowComment = GetDbReaderValue<string>(reader["FollowComment"]);
                        result.Opinion = GetDbReaderValue<string>(reader["Opinion"]);
                        result.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
            }
            return result;
        }

        public ProposalDetailBase getProposalDetailByCode(SqlConnection connection, string code, string _userID)
        {
            ProposalDetailBase result = null;
            using (var command = new SqlCommand(" select P.*,D.DepartmentCode,  D.DepartmentName,D1.DepartmentCode as CurDepartmentCode,  D1.DepartmentName as CurDepartmentName, PT.TypeName from (select * from tbl_Proposal  where ProposalCode = @ProposalCode) as P  " +
                " left join tbl_Department D on P.DepartmentID = D.DepartmentID  " +
                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID  " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID  ", connection))
            {
                AddSqlParameter(command, "@ProposalCode", code, System.Data.SqlDbType.VarChar);
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (P.UserI = @UserID or P.UserU = @UserID or P.UserAssign =@UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                command.CommandText += " order by P.UpdateTime  ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        result = new ProposalDetailBase();
                        result.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        result.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        result.InCode = GetDbReaderValue<string>(reader["InCode"]);
                        result.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        result.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        result.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        result.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        result.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        result.UserAssigned = GetDbReaderValue<string>(reader["UserAssigned"]);
                        result.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        result.Status = GetDbReaderValue<int>(reader["Status"]);
                        result.CurDepartmentID = GetDbReaderValue<int>(reader["CurDepartmentID"]);
                        result.CurDepartmentCode = GetDbReaderValue<string>(reader["CurDepartmentCode"]);
                        result.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        result.FollowComment = GetDbReaderValue<string>(reader["FollowComment"]);
                        result.Opinion = GetDbReaderValue<string>(reader["Opinion"]);
                        result.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
            }
            return result;
        }
        public ProposalDetailInfo getProposalDetail2(SqlConnection connection, string proposalCode)
        {
            ProposalDetailInfo result = null;
            using (var command = new SqlCommand(" select P.*, D.DepartmentCode,D.DepartmentName,D1.DepartmentCode as CurDepartmentCode, D1.DepartmentName as CurDepartmentName, PT.TypeName from (select * from tbl_Proposal  where ProposalCode = @ProposalCode) as P  " +
                " left join tbl_Department D on P.DepartmentID = D.DepartmentID  " +
                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID  " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID  ", connection))
            {
                AddSqlParameter(command, "@ProposalCode", proposalCode, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        result = new ProposalDetailInfo();
                        result.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        result.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        result.InCode = GetDbReaderValue<string>(reader["InCode"]);
                        result.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        result.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        result.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        result.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        result.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        result.UserAssigned = GetDbReaderValue<string>(reader["UserAssigned"]);
                        result.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        result.Status = GetDbReaderValue<int>(reader["Status"]);
                        result.CurDepartmentID = GetDbReaderValue<int>(reader["CurDepartmentID"]);
                        result.CurDepartmentCode = GetDbReaderValue<string>(reader["CurDepartmentCode"]);
                        result.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        result.FollowComment = GetDbReaderValue<string>(reader["FollowComment"]);
                        result.Opinion = GetDbReaderValue<string>(reader["Opinion"]);
                        result.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
            }
            return result;
        }
        public int getTotalRecords(SqlConnection connection, ProposalSeachCriteria _criteria, string _userID)
        {
            if (_criteria != null)
            {
                using (var command = new SqlCommand("Select count(*)  as TotalRecords  from tbl_Proposal P where 1 = 1 ", connection))
                {
                    if (_criteria.FromDate != null && _criteria.ToDate != null)
                    {
                        command.CommandText += " and P.DateIn between @FromDate and @ToDate ";
                        AddSqlParameter(command, "@FromDate", _criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                        AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
                    }
                    if (!string.IsNullOrEmpty(_criteria.ProposalCode))
                    {
                        command.CommandText += " and P.ProposalCode like '%" + _criteria.ProposalCode + "%' ";

                    }
                    if (!string.IsNullOrEmpty(_criteria.ProposalType))
                    {
                        command.CommandText += " and P.ProposalType = @ProposalType";
                        AddSqlParameter(command, "@ProposalType", _criteria.ProposalType, System.Data.SqlDbType.Int);
                    }
                    if (!string.IsNullOrEmpty(_criteria.Status))
                    {
                        command.CommandText += " and P.Status = @Status";
                        AddSqlParameter(command, "@Status", _criteria.Status, System.Data.SqlDbType.Int);
                    }
                    if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                    {
                        command.CommandText += " and (P.UserI = @UserID or P.UserU = @UserID or P.UserAssign =@UserID )";
                        AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
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
                using (var command = new SqlCommand("Select count(*) as TotalRecords  from tbl_Proposal where 1 = 1 ", connection))
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
        public List<ProposalInfo> getProposal(SqlConnection connection, ProposalSeachCriteria _criteria, string _userID)
        {
            var result = new List<ProposalInfo>();
            using (var command = new SqlCommand("SELECT [ProposalID],[ProposalCode]," +
                "[InCode],[ProposalType],[DepartmentID],[UserAssigned],[ContractCode]," +
                "[Status],[CurDepartmentID]," +
                "cast([FollowComment] as nvarchar(max)) as [FollowComment]," +
                "cast([Comment] as nvarchar(max)) as [Comment]," +
                "[DueDate],[UserI],[DateIn],[UserU],[UserAssign],[Intime],[UpdateTime],[rowguid]," +
                "Cast([Opinion] as nvarchar(max)) as [Opinion]," +
                "[IsHasQuote],DepartmentCode,DepartmentName,CurDepartmentCode,CurDepartmentName,TypeName," +
                " ItemName =STUFF((SELECT DISTINCT ', ' + ItemName " +
                " FROM (Select P.*, D.DepartmentCode,D.DepartmentName, " +
                "D1.DepartmentCode as CurDepartmentCode, " +
                "D1.DepartmentName as CurDepartmentName,PT.TypeName, I.ItemName " +
                " from tbl_Proposal P" +
                " left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
                " left join tbl_Department D1 on P.CurDepartmentID = D1.DepartmentID " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID " +
                " left join tbl_Proposal_Item PI on P.ProposalID = PI.ProposalID " +
                " left join tbl_Items I on PI.ItemID = I.ItemID where 1 = 1 ) b  " +
                " WHERE b.ProposalID = a.ProposalID FOR XML PATH('')), 1, 2, '') " +
                " FROM (Select P.*, D.DepartmentCode,D.DepartmentName,D1.DepartmentCode as CurDepartmentCode, " +
                " D1.DepartmentName as CurDepartmentName,PT.TypeName, I.ItemName from tbl_Proposal P " +
                " left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
                " left join tbl_Department D1 on P.CurDepartmentID = D1.DepartmentID " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID " +
                " left join tbl_Proposal_Item PI on P.ProposalID = PI.ProposalID " +
                " left join tbl_Items I on PI.ItemID = I.ItemID where 1 = 1 ) a " +
                " where 1=1 and DateIn between @FromDate and @ToDate" +
                "  ", connection))
            {
                AddSqlParameter(command, "@FromDate", _criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
                if (!string.IsNullOrEmpty(_criteria.ProposalID))
                {
                    command.CommandText += " and ProposalID = @ProposalID";
                    AddSqlParameter(command, "@ProposalID", _criteria.ProposalID, System.Data.SqlDbType.Int);
                }
                if (!string.IsNullOrEmpty(_criteria.ProposalCode))
                {
                    command.CommandText += " and ProposalCode like '%" + _criteria.ProposalCode + "%' ";

                }

                if (!string.IsNullOrEmpty(_criteria.ProposalType))
                {
                    command.CommandText += " and ProposalType  = @ProposalType";
                    AddSqlParameter(command, "@ProposalType", _criteria.ProposalType, System.Data.SqlDbType.Int);
                }
                if (_criteria.DepartmentID != 0)
                {
                    command.CommandText += " and ( DepartmentID = @DepartmentID )  ";
                    //Nguyen Minh Hoang
                    //command.CommandText += " --or CurDepartmentID = @CurDepartmentID )";
                    AddSqlParameter(command, "@DepartmentID", _criteria.DepartmentID, System.Data.SqlDbType.Int);
                    //AddSqlParameter(command, "@CurDepartmentID", _criteria.DepartmentID, System.Data.SqlDbType.Int);
                }
                if (!string.IsNullOrEmpty(_criteria.Status))
                {
                    command.CommandText += " and Status = @Status";
                    AddSqlParameter(command, "@Status", _criteria.Status, System.Data.SqlDbType.Int);
                }
                if (!string.IsNullOrEmpty(_criteria.Item))
                {
                    command.CommandText += " and ItemName like N'%"+ _criteria.Item + "%'";
                }
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (UserI = @UserID or UserU = @UserID or UserAssign =@UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                command.CommandText += " GROUP BY [ProposalID],[ProposalCode],[InCode],[ProposalType] ," +
                    "[DepartmentID],[UserAssigned],[ContractCode],[Status] ,[CurDepartmentID]," +
                    "cast([FollowComment] as nvarchar(max)) ,cast([Comment] as nvarchar(max)),[DueDate],[UserI]," +
                    "[DateIn] ,[Intime],[UserU],[UpdateTime],[rowguid],Cast([Opinion] as nvarchar(max)) ,[IsHasQuote]," +
                    "DepartmentCode,DepartmentName,CurDepartmentCode,CurDepartmentName,TypeName,[UserAssign] ";
                command.CommandText += " order by UpdateTime Desc  ";

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
                        var info = new ProposalInfo();
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.InCode = GetDbReaderValue<string>(reader["InCode"]);
                        info.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        info.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.UserAssigned = GetDbReaderValue<string>(reader["UserAssigned"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.Status = GetDbReaderValue<int>(reader["Status"]);
                        info.CurDepartmentID = GetDbReaderValue<int>(reader["CurDepartmentID"]);
                        info.CurDepartmentCode = GetDbReaderValue<string>(reader["CurDepartmentCode"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        info.FollowComment = GetDbReaderValue<string>(reader["FollowComment"]);
                        info.Opinion = GetDbReaderValue<string>(reader["Opinion"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.ItemsName = GetDbReaderValue<string>(reader["ItemName"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }


        public List<ProposalInfo> getProposalByCode(SqlConnection connection, string code)
        {
            var result = new List<ProposalInfo>();
            using (var command = new SqlCommand("Select P.*, D.DepartmentCode,D.DepartmentName,D1.DepartmentCode as CurDepartmentCode, D1.DepartmentName as CurDepartmentName, PT.TypeName " +
                " from tbl_Proposal P left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID where 1 = 1 and P.DateIn between @FromDate and @ToDate ", connection))
            {
                command.CommandText += " and P.ProposalType like '%" + code + "%' ";
                command.CommandText += " order by P.UpdateTime Desc  ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalInfo();
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.InCode = GetDbReaderValue<string>(reader["InCode"]);
                        info.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        info.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.UserAssigned = GetDbReaderValue<string>(reader["UserAssigned"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.Status = GetDbReaderValue<int>(reader["Status"]);
                        info.CurDepartmentID = GetDbReaderValue<int>(reader["CurDepartmentID"]);
                        info.CurDepartmentCode = GetDbReaderValue<string>(reader["CurDepartmentCode"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        info.FollowComment = GetDbReaderValue<string>(reader["FollowComment"]);
                        info.Opinion = GetDbReaderValue<string>(reader["Opinion"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<string> getListProposalCode(SqlConnection connection, string proposalCode, string _userID)
        {
            var result = new List<string>();
            using (var command = new SqlCommand(" Select TOP 10 proposalCode from tbl_Proposal where ProposalCode like '%" + proposalCode + "%'", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (UserI = @UserID or UserU = @UserID or UserAssign =@UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _proposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        result.Add(_proposalCode);
                    }
                }
                return result;
            }
        }

        public void InsertProposalItem(SqlConnection connection, int _ProposalId, ItemPropsalInfo item)
        {
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Proposal_Item] ([ProposalID], [ItemID],[amount], [note],[IsExceedReserve],[NumExceedReserve], [IsReservered] )" +
                   "VALUES(@ProposalID,@ItemID,@amount,@note,@IsExceedReserve,@NumExceedReserve,@IsReservered )", connection))
            {
                AddSqlParameter(command, "@ProposalID", _ProposalId, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ItemID", item.ItemID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@amount", item.Amount, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@note", item.Note, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@IsExceedReserve", item.IsExceedReserve, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@NumExceedReserve", item.NumExceedReserve, System.Data.SqlDbType.Decimal);
                AddSqlParameter(command, "@IsReservered", item.IsReservered, System.Data.SqlDbType.Bit);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


        public void DeleteProposalItems(SqlConnection connection, string _ProposalItemID)
        {
            using (var command = new SqlCommand(" delete tbl_Proposal_Item where AutoID in (" + _ProposalItemID + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public int InsertProposal(SqlConnection connection, ProposalInfo _Proposal, string _userI)
        {
            int lastestInserted = 0;
            var currenttime = DateTime.Now.Date;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Proposal] (ProposalCode,InCode, ProposalType, DepartmentID, UserAssigned, ContractCode, Status, CurDepartmentID, FollowComment, Comment, UserI, DateIn, Opinion, UserAssigned)" +
                    "VALUES(@ProposalCode,@InCode, @ProposalType, @DepartmentID, @UserAssigned, @ContractCode, @Status, @CurDepartmentID, @FollowComment, @Comment, @UserI, @DateIn , @Opinion, @UserAssigned)  " +
                    " select IDENT_CURRENT('dbo.tbl_Proposal') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@ProposalCode", Utils.ChuyenTVKhongDau(_Proposal.ProposalCode), System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@InCode", _Proposal.InCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ProposalType", _Proposal.ProposalType, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DepartmentID", _Proposal.DepartmentID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserAssigned", _Proposal.UserAssigned, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ContractCode", _Proposal.ContractCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Status", 1, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CurDepartmentID", _Proposal.CurDepartmentID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@FollowComment", _Proposal.FollowComment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Opinion", _Proposal.Opinion, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Comment", _Proposal.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserAssigned", _Proposal.UserAssigned, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                //AddSqlParameter(command, "@DateIn", DateTime.Now.Date, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@DateIn", _Proposal.DateIn.Date, System.Data.SqlDbType.DateTime);
                WriteLogExecutingCommand(command);
                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }

            if (lastestInserted != 0)
            {
                using (var command = new SqlCommand("insert into tbl_Proposal_Process " +
                    "(ProposalID, ProposalCode, ProposalTime, CurrentFeature) " +
                    "values (@ProposalID, @ProposalCode, @ProposalTime, @CurrentFeature) ", connection))
                {
                    AddSqlParameter(command, "@ProposalID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@ProposalCode", Utils.ChuyenTVKhongDau(_Proposal.ProposalCode), System.Data.SqlDbType.VarChar);
                    AddSqlParameter(command, "@ProposalTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "Proposal", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }
            return lastestInserted;
        }

        public void UpdateProposal(SqlConnection connection, int _id, ProposalInfo _Proposal, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Proposal " +
                            " SET  DateIn=@DateIn , InCode = @InCode, ProposalType = @ProposalType, DepartmentID = @DepartmentID,UserAssigned = @UserAssigned,ContractCode = @ContractCode, Status=@Status,CurDepartmentID=@CurDepartmentID " +
                            " , FollowComment=@FollowComment ,Comment=@Comment,Opinion=@Opinion, UserU=@UserU ,UpdateTime=getdate(), UserAssigned=@UserAssigned " +
                            " WHERE ProposalID = @ProposalID", connection))
            {
                AddSqlParameter(command, "@DateIn", _Proposal.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ProposalID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ProposalType", _Proposal.ProposalType, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@InCode", _Proposal.InCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@DepartmentID", _Proposal.DepartmentID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserAssigned", _Proposal.UserAssigned, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ContractCode", _Proposal.ContractCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Status", _Proposal.Status, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CurDepartmentID", _Proposal.CurDepartmentID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@FollowComment", _Proposal.FollowComment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Opinion", _Proposal.Opinion, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Comment", _Proposal.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@UserAssigned", _Proposal.UserAssigned, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void DeleteProposal(SqlConnection connection, int _ProposalID)
        {
            using (var command = new SqlCommand(" insert into tbl_Proposal_Log (ProposalID, ProposalCode, InCode, ProposalType, DepartmentID, UserAssigned, ContractCode, Status, CurDepartmentID, FollowComment, Comment, UserI, DateIn,Intime, UserU, UpdateTime, UserAssigned) " +
                " (select ProposalID, ProposalCode, Incode, ProposalType, DepartmentID, UserAssigned, ContractCode, Status, CurDepartmentID, FollowComment, Comment, UserI, DateIn,Intime, UserU, UpdateTime, UserAssigned from tbl_Proposal where ProposalID=@ProposalID ) " +
                " delete tbl_Proposal where ProposalID=@ProposalID ;  delete tbl_Proposal_Process where ProposalID=@ProposalID", connection))
            {
                AddSqlParameter(command, "@ProposalID", _ProposalID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public void DeleteProposals(SqlConnection connection, string _ProposalIDs)
        {
            using (var command = new SqlCommand(" insert into tbl_Proposal_Log (ProposalID, ProposalCode, InCode, ProposalType, DepartmentID, UserAssigned, ContractCode, Status, CurDepartmentID, FollowComment, Comment, UserI, DateIn, Intime, UserU, UpdateTime, UserAssigned) " +
                " (select ProposalID, ProposalCode, Incode, ProposalType, DepartmentID, UserAssigned, ContractCode, Status, CurDepartmentID, FollowComment, Comment, UserI, DateIn, Intime, UserU, UpdateTime, UserAssigned from tbl_Proposal where ProposalID in (" + _ProposalIDs + ")) " +
                " delete tbl_Proposal where ProposalID in (" + _ProposalIDs + ") ; delete tbl_Proposal_Process where ProposalID in (" + _ProposalIDs + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public int GetMaxPropCode(SqlConnection connection, int year)
        {
            string strlSQLGetMaxNumber = "select isnull(max(p.count), 0) " +
                "from tbl_ProposalAutoCode p " +
                "where year = @year ";
            using (var command = new SqlCommand(strlSQLGetMaxNumber))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqlParameter("@year", year));
                int maxCode = (int)db.ExcuteScalar(connection, command);
                maxCode++;
                return maxCode;
            }
        }

        public void UpdateMaxCode(SqlConnection connection, int maxCode, int year)
        {
            string strAddNewCode = "insert into [dbo].[tbl_ProposalAutoCode] ([year], [count]) " +
                    "VALUES(@year,@count) ";
            using (var command = new SqlCommand(strAddNewCode))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqlParameter("@year", year));
                command.Parameters.Add(new SqlParameter("@count", maxCode));
                db.ExcuteScalar(connection, command);
            }
        }

        public int InsertProposalDocument(SqlConnection connection, string preferID, string lenght, string fileName, string type, string _userI)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Document] ([TableName], [PreferId],[Link], [Length], [Type], [UserI])" +
                    "VALUES(@TableName,@PreferId,@Link, @Length,@Type, @UserI) " +
                    " select IDENT_CURRENT('dbo.tbl_Document') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@TableName", "Proposal", System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@PreferId", preferID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Link", fileName, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Length", lenght, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Type", type, System.Data.SqlDbType.VarChar);
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
        public void DeleteDocuments(SqlConnection connection, string preferID)
        {
            if (!string.IsNullOrEmpty(preferID))
            {
                using (var command = new SqlCommand(" delete tbl_Document where TableName = @TableName and PreferID = @PreferId ", connection))
                {
                    AddSqlParameter(command, "@TableName", "Proposal", System.Data.SqlDbType.VarChar);
                    AddSqlParameter(command, "@PreferId", preferID, System.Data.SqlDbType.Int);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }
        }
        public List<ItemPropsalInfo> GetPropsalItems(SqlConnection connection, int proposalId)
        {
            List<ItemPropsalInfo> ret = new List<ItemPropsalInfo>();
            using (var command = new SqlCommand(@"select AutoID, tpi.ItemID, tpi.amount, tpi.note, tpi.IsExceedReserve, tpi.IsReservered,tpi.NumExceedReserve, i.ItemName ,  i.Unit, i.ItemCode ,
                ISNULL(tpi.AcceptanceNote,'') as AcceptanceNote , ISNULL(tpi.AcceptanceResult,'false') as AcceptanceResult,  ISNULL(tpi.ExplanationNote,'') as ExplanationNote,  ISNULL(tpi.SurveyNote,'') as SurveyNote  from
                (select * from tbl_Proposal_Item   where ProposalID = @ProposalID) tpi
                inner join  tbl_items as i on tpi.ItemID = i.ItemID
                ", connection))
            {
                AddSqlParameter(command, "@ProposalID", proposalId, System.Data.SqlDbType.Int);

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new ItemPropsalInfo();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.Note = GetDbReaderValue<string>(reader["Note"]);
                        item.IsExceedReserve = GetDbReaderValue<bool>(reader["IsExceedReserve"]);
                        item.NumExceedReserve = Convert.ToDouble(GetDbReaderValue<object>(reader["NumExceedReserve"]));
                        item.IsReservered = GetDbReaderValue<bool>(reader["IsReservered"]);
                        item.ItemUnit = GetDbReaderValue<string>(reader["Unit"]);
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        item.AcceptanceNote = GetDbReaderValue<string>(reader["AcceptanceNote"]);
                        item.AcceptanceResult = GetDbReaderValue<bool>(reader["AcceptanceResult"]);
                        item.ExplanationNote = GetDbReaderValue<string>(reader["ExplanationNote"]);
                        item.SurveyNote = GetDbReaderValue<string>(reader["SurveyNote"]);
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }

        public List<ItemSurveyInfo> GetSurveyItem(SqlConnection connection, int surveyId)
        {
            List<ItemSurveyInfo> ret = new List<ItemSurveyInfo>();
            using (var command = new SqlCommand(@"select * from tbl_Survey_Item where SurveyId=@SurveyId "
                , connection))
            {
                AddSqlParameter(command, "@SurveyId", surveyId, System.Data.SqlDbType.Int);

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new ItemSurveyInfo();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.ItemUnit = GetDbReaderValue<string>(reader["ItemUnit"]);
                        item.ItemAmount = GetDbReaderValue<double>(reader["ItemAmount"]);
                        item.Note = GetDbReaderValue<string>(reader["Note"]);

                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);

                        ret.Add(item);
                    }
                }
            }
            return ret;
        }



        public void UpdateItemAcceptance(SqlConnection connection, DeliveryReceiptItemInfoNew itemObj, string userID)
        {
            string strAddNewCode = "UPDATE tbl_DeliveryReceipt_items " +
                           " SET  AcceptanceResult = @AcceptanceResult" +
                           " WHERE AutoID = @AutoID";
            using (var command = new SqlCommand(strAddNewCode))
            {
                AddSqlParameter(command, "@AcceptanceResult", itemObj.AcceptanceResult, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@AutoID", itemObj.AutoID, System.Data.SqlDbType.Int);
                db.ExcuteScalar(connection, command);
            }
        }
        public void DeleteSurveyItem(SqlConnection connection, int SurveyID, string userID)
        {
            string strAddNewCode = "delete from tbl_Survey_Item WHERE SurveyID = @SurveyID";

            using (var command = new SqlCommand(strAddNewCode))
            {
                AddSqlParameter(command, "@SurveyID", SurveyID, System.Data.SqlDbType.Int);

                db.ExcuteScalar(connection, command);
            }
        }


        public void InsertSurveyItem(SqlConnection connection, ItemSurveyInfo itemObj, int SurveyID, string userID)
        {
            using (var command = new SqlCommand("Insert into tbl_Survey_Item (SurveyID, ItemName, ItemAmount, Note, ItemUnit)" +
                   "VALUES(@SurveyID, @ItemName, @ItemAmount, @Note, @ItemUnit )", connection))
            {

                AddSqlParameter(command, "@SurveyID", SurveyID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ItemName", itemObj.ItemName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ItemAmount", itemObj.ItemAmount, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@Note", itemObj.Note, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ItemUnit", itemObj.ItemUnit, System.Data.SqlDbType.NVarChar);

                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


        public void UpdateItemExplanation(SqlConnection connection, ItemPropsalInfo itemObj, string userID)
        {
            string strAddNewCode = "UPDATE tbl_Proposal_Item " +
                           " SET   ExplanationNote = @ExplanationNote , UserU = @UserU " +
                           " WHERE AutoID = @AutoID";

            using (var command = new SqlCommand(strAddNewCode))
            {
                AddSqlParameter(command, "@ExplanationNote", itemObj.ExplanationNote, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserU", userID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@AutoID", itemObj.AutoID, System.Data.SqlDbType.Int);
                db.ExcuteScalar(connection, command);
            }
        }

        public List<ProposalSelectItem> GetListProsalHaveQuote(SqlConnection connection, string code)
        {
            var result = new List<ProposalSelectItem>();
            using (var command = new SqlCommand(" Select TOP 10  tblP.ProposalID , tblP.ProposalCode from  tbl_Quote tblQ " +
                " inner join  tbl_Quote_Proposal tblQP on tblQ.QuoteID = tblQP.QuoteID " +
                " inner join  tbl_Proposal tblP on tblP.ProposalID = tblQP.ProposalID " +
                " where tblP.ProposalCode like '%" + code + "%'", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProposalSelectItem record = new ProposalSelectItem();
                        record.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        record.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        result.Add(record);
                    }
                }
                return result;
            }
        }


        public List<ProposalSelectItem> GetListProsalCanCreateAcceptance(SqlConnection connection, string code)
        {
            var result = new List<ProposalSelectItem>();

            string query = " Select TOP 10  tblProcess.ProposalID, tblP.ProposalCode from( select DISTINCT PP.ProposalID , PP.QuoteID, sum(QI.ItemPrice) as TotalPrice FROM tbl_Proposal_Process PP join tbl_Quote_Item QI on QI.QUoteID = PP.QUoteID  " +
             " where  PP.QuoteID is not null and (BidPlanID is not Null or TotalPrice < 20000000) and NegotiationID is not null and  DecisionID is not null" + //and AuditID is not null
             " and ContractID is not null and DeliveryReceiptID is  not null and AcceptanceID is null" +
             "    group by PP.QuoteID ,PP.ProposalID) as tblProcess " +
             " inner join tbl_Proposal tblP on tblProcess.ProposalID =  tblP.ProposalID where tblP.ProposalCode like '%" + code + "%' ";
            using (var command = new SqlCommand(query, connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProposalSelectItem record = new ProposalSelectItem();
                        record.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        record.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        result.Add(record);
                    }
                }
                return result;
            }
        }



        public int getQuoteIDWithHaveFinalContact(SqlConnection connection, int proposalID)
        {
            int ret = 0;
            using (var command = new SqlCommand(" select tblQ.QuoteID " +
                " from ( select * from tbl_Contract where ProposalID = @ProposalID) tblC " +
                " inner join tbl_Decision tblD " +
                " on tblC.DecisionID = tbld.DecisionID " +
                " inner join tbl_Negotiation tblN " +
                " on tblN.NegotiationID = tblD.NegotiationID " +
                " inner join tbl_BidPlan tblB " +
                " on tblN.BidPlanID = tblB.BidPlanID " +
                " inner join tbl_Audit tblA " +
                " on tblA.AuditID = tblB.AuditID " +
                " inner join tbl_Quote tblQ " +
                " on tblQ.QuoteID = tblA.QuoteID ", connection))


            {

                AddSqlParameter(command, "@ProposalID", proposalID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                object lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    ret = Convert.ToInt32(lastInsertedRaw);
                }
            }

            return ret;
        }
        public ProposalRelatedData GetRelateData(SqlConnection connection, int id)
        {
            ProposalRelatedData ret = new ProposalRelatedData();

            using (var command = new SqlCommand(@"select * from(
                        SELECT  ProposalID as ID
                              , ProposalCode as Code
                              , 'Proposal' as  TableType
                        FROM tbl_Proposal
                        where  ProposalID = @ProposalID 
                        union 
                        SELECT  ExplanationID as ID
                              , ExplanationCode as Code
                              , 'Explanation' as  TableType
                        FROM tbl_Explanation
                        where  ProposalID = @ProposalID 
                        union
                        SELECT  SurveyID as ID
                              , SurveyCode as Code
                              , 'Survey' as  TableType
                        FROM tbl_Survey
                        where  ProposalID = @ProposalID 
                        ) temp
                        order by temp.TableType
                        ", connection))
            {
                AddSqlParameter(command, "@ProposalID", id, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        SingleDetailsData item = new SingleDetailsData();
                        string tableType = GetDbReaderValue<string>(reader["TableType"]);
                        if (tableType == "Proposal")
                        {
                            ret.ProposalID = GetDbReaderValue<int>(reader["ID"]);
                            ret.ProposalCode = GetDbReaderValue<string>(reader["Code"]);
                        }
                        else
                        {
                            item.id = GetDbReaderValue<int>(reader["ID"]);
                            item.code = GetDbReaderValue<string>(reader["Code"]);

                        }
                        switch (tableType)
                        {
                            case "Explanation":
                                ret.lstExplanation.Add(item);
                                break;
                            case "Survey":
                                ret.lstSurvey.Add(item);
                                break;
                            case "Quote":
                                ret.lstQuote.Add(item);
                                break;
                            case "Audit":
                                ret.lstAudit.Add(item);
                                break;
                            case "BidPlan":
                                ret.lstBidPlan.Add(item);
                                break;
                            case "Negotiation":
                                ret.lstNegotiation.Add(item);
                                break;
                            case "Decision":
                                ret.lstDecision.Add(item);
                                break;
                            case "Contract":
                                ret.lstContract.Add(item);
                                break;
                            case "DeliveryReceipt":
                                ret.lstDeliveryReceipt.Add(item);
                                break;
                            case "Acceptance":
                                ret.lstAcceptance.Add(item);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return ret;
        }

        public ReserveCodeInfo GetReserve(SqlConnection connection, DateTime? dateIn)
        {
            ReserveCodeInfo result = null;
            using (var command = new SqlCommand("SELECT rc.[ReserveID], rc.[ReserveCode], rc.[DateReserve] \n" +
                " FROM [dbo].[tbl_ReserveCode] rc " +
                " where rc.[DateReserve] between @MonthStart and @MonthEnd ", connection))
            {
                AddSqlParameter(command, "@MonthStart", dateIn.Value.ToString("yyyy-01-01 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@MonthEnd", dateIn.Value.ToString("yyyy-12-31 23:59:59"), System.Data.SqlDbType.DateTime);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new ReserveCodeInfo();
                        result.ReserveID = GetDbReaderValue<int>(reader["ReserveID"]);
                        result.ReserveCode = GetDbReaderValue<string>(reader["ReserveCode"]);
                        result.ReserveDate = GetDbReaderValue<DateTime>(reader["DateReserve"]);
                    }
                }
            }

            return result;
        }

        public ItemReserve CheckReserve(SqlConnection connection, int itemID, double quantity, int reserveID, int departmentID)
        {
            var result = new List<ItemReserve>();
            var ReserCode = GetReserveCode(connection, reserveID);
            using (var command = new SqlCommand(" select (Case when NumExceedReserve > 0 then NumExceedReserve else 0 end) as NumExceedReserve " +
                ", (Case when NumExceedReserve > 0 then cast(1 as bit) else cast(0 as bit) end) as IsExceedReserve, IsReservered  " +
                " from (select  isnull(TotalOutput, 0) + @Quantity - isnull(R.ReserveUnit, 0)   as NumExceedReserve,  (case when R.ReserveUnit > 0 then cast(1 as bit) else cast(0 as bit) end) as IsReservered  from (select * from tbl_reserve  where itemID = @ItemID and reserveID = @ReserveID and DepartmentID = @DepartmentID ) R " +
                " left join (" +
                " SELECT        inpd.ItemID, inpd.StoreID, (CASE WHEN inpd.Cost > 0 THEN inpd.Cost ELSE 0 END) AS Cost, SUM(inpd.Quantity) AS TotalInput, 0.0 AS TotalOutput  " +
                               "FROM            dbo.tbl_InputDetail AS inpd INNER JOIN " +
                            " dbo.tbl_Input AS inp ON inp.InID = inpd.InID " +
                               "WHERE       year(inp.DateIn) = @ReserCode " +
                              " GROUP BY inpd.ItemID, inpd.StoreID, inpd.Lot, inpd.Cost, inpd.ExpireDate  " +
                              " UNION " +
                              " SELECT        outpd.ItemID, outpd.StoreID, outpd.Price AS Cost, 0.0 AS TotalInput, SUM(outpd.Quantity) AS TotalOutput " +
                              " FROM            dbo.tbl_OutputDetail AS outpd INNER JOIN " +
                            " dbo.tbl_Output AS outp ON outp.OutID = outpd.OutID " +
                            "   WHERE         year(outp.DateIn) = @ReserCode " +
                              " GROUP BY outpd.ItemID, outpd.StoreID, outpd.Lot, outpd.Price, outpd.ExpireDate " +
                              " UNION " +
                             "  SELECT        prod.ItemID, prod.StoreID, 0.0 AS Cost, 0.0 AS TotalInput, SUM(prod.Amount) AS TotalOutput " +
                              " FROM            dbo.tbl_ProposalDetail AS prod INNER JOIN " +
                              "  dbo.tbl_Proposal AS pro ON pro.ProposalID = prod.ProposalID " +
                             "  WHERE         year(pro.DateIn) = @ReserCode " +
                              " GROUP BY prod.ItemID, prod.StoreID " +
                    ")  IES on IES.ItemID = R.itemID  " +
                ") A ", connection))
            {
                AddSqlParameter(command, "@Quantity", quantity, System.Data.SqlDbType.Decimal);
                AddSqlParameter(command, "@ItemID", itemID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ReserveID", reserveID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ReserCode", ReserCode, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DepartmentID", departmentID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ItemReserve();
                        info.IsExceedReserve = GetDbReaderValue<bool>(reader["IsExceedReserve"]);
                        info.NumExceedReserve = GetDbReaderValue<double>(reader["NumExceedReserve"]);
                        info.IsReservered = GetDbReaderValue<bool>(reader["IsReservered"]);
                        result.Add(info);
                    }
                }
                if (result.Count == 1)
                {
                    return result[0];
                }
                else return null;
            }
        }

        public bool CheckIsHosReserve(SqlConnection connection, int itemID, int reserveID)
        {
            bool result = false;
            using (var command = new SqlCommand(" Select * from tbl_Reserve where ItemID = @ItemID and ReserveID = @ReserveID ", connection))
            {
                AddSqlParameter(command, "@ItemID", itemID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ReserveID", reserveID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public string GetReserveCode(SqlConnection connection, int reserveID)
        {
            using (var command = new SqlCommand("SELECT [ReserveCode]  \n" +
                " FROM [dbo].[tbl_ReserveCode]  where  [ReserveID] = @ReserveID  ", connection))
            {
                WriteLogExecutingCommand(command);
                AddSqlParameter(command, "@ReserveID", reserveID, System.Data.SqlDbType.Int);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return GetDbReaderValue<string>(reader["ReserveCode"]);
                    }
                }
            }

            return "";
        }

        //report 

        public List<StatusCountReport> CountByStatus(SqlConnection connection)
        {
            var result = new List<StatusCountReport>();
            using (var command = new SqlCommand(
                    " select count(tblTemp.Stat) as cnt, tblTemp.Stat from " +
                    " (" +
                    " select  CASE" +
                    " WHEN  CurrentFeature = 'Proposal' THEN 0 " +
                    " WHEN  CurrentFeature = 'Acceptance' THEN 2 " +
                    " ELSE 1 " +
                    " END as Stat from tbl_Proposal_Process " +
                     ") tblTemp group by tblTemp.Stat ", connection))
            {
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new StatusCountReport();
                        info.label = (GetDbReaderValue<int>(reader["Stat"])).ToString();
                        info.value = (GetDbReaderValue<int>(reader["cnt"])).ToString();
                        result.Add(info);
                    }
                }
            }

            return result;
        }
        public List<ProposalWithItems> getProposalCanCreateQuote(SqlConnection connection, string proposalCode, string itemName)
        {
            var result = new List<ProposalWithItems>();

            var sql = "select P.ProposalID, P.ProposalCode," +
            " PIWI.ItemCode, PIWI.ItemID, PIWI.ItemName , PIWI.Unit , PIWI.Amount from " +
            "(select ProposalID , ProposalCode from  tbl_Proposal_Process " +
            "where tbl_Proposal_Process.QuoteID is null " + //and SurveyID is not null  -- không cần khảo sát
            ") as P " +
            "inner join " +
            "(select PI.ProposalID, PI.ItemID, I.ItemCode, I.ItemName , I.Unit , PI.Amount " +
            "from " +
            "(select * from[tbl_Proposal_Item])  PI " +
            "inner join tbl_Items as I " +
            "on I.itemID = PI.ItemID ";
            if (itemName != null && itemName.Trim() != "")
            {
                sql += "where FREETEXT(ItemName,@itemName)  ";
            }
            sql += ") as PIWI " +
            " on P.ProposalID = PIWI.ProposalID " +
            " inner join tbl_Proposal Propo on Propo.ProposalID = P.ProposalID where 1 = 1 and Propo.ProposalType in (1,2) "; // mua, sửa chữa
            if (!string.IsNullOrEmpty(proposalCode))
            {
                sql += " and  p.ProposalCode like '%" + proposalCode + "%'";
            }
            sql += "order by P.ProposalID";
            using (var command = new SqlCommand(
                sql
                , connection))
            {
                //AddSqlParameter(command, "@proposalCode", "%" +  proposalCode + "%", System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@itemName", "%" + itemName + "%", System.Data.SqlDbType.NVarChar);

                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalWithItems();
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        info.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        info.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        info.ItemUnit = GetDbReaderValue<string>(reader["Unit"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.ItemAmount = GetDbReaderValue<double>(reader["Amount"]);
                        result.Add(info);
                    }
                }
            }

            return result;
        }

        public DRFillDetailInfo getDetailsForDR(SqlConnection connection, int id)
        {
            DRFillDetailInfo ret = new DRFillDetailInfo();
            using (var command = new SqlCommand(
             "  select tblP.ProposalCode, tblP.ProposalID, tbld.DepartmentName, tblCD.DepartmentName as curDepartmentName , tblP.ProposalType" +
             "     from tbl_Proposal tblP" +
             "     inner join tbl_Department tblD " +
             "     on tblp.DepartmentID = tblD.DepartmentID " +
             "     inner join tbl_Department tblCD " +
             "     on tblP.CurDepartmentID = tblCD.DepartmentID " +

             " where tblP.ProposalID = " + id, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.CurDepartmentName = GetDbReaderValue<string>(reader["curDepartmentName"]);
                        ret.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        ret.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                    }
                }
                WriteLogExecutingCommand(command);
            }
            ret.ProposalID = id;
            using (var command = new SqlCommand(
            " select tblCDE.DecisionCode, tblCDE.CapitalID, tblC.ContractCode, tblQ.QuoteID , tblC.ContractID,  tblQ.QuoteCode , " +
            "  tblQ.VATNumber, tblQ.IsVAT from tbl_Quote_Proposal tblQP " +
            " left join tbl_Contract tblC " +
            " on tblQP.QuoteID = tblc.QuoteID " +
            " inner join tbl_Quote tblQ " +
            " on tblQP.QuoteID = tblQ.QuoteID " +
            " left join tbl_Decision tblCDE " +
            " on tblCDE.QuoteID = tblc.QuoteID " +
            " where tblQP.ProposalID = " + id, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        ret.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        ret.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        ret.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        ret.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        ret.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        ret.DecisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                        ret.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                    }
                }
                WriteLogExecutingCommand(command);
            }

            return ret;
        }

        public QuoteRelation getQuoteRelation(SqlConnection connection, int quoteID)
        {
            var result = new QuoteRelation();
            using (var command = new SqlCommand(
              "  select DISTINCT  tbl_Quote.QuoteID, tbl_Quote.QuoteCode, tbl_Quote.DateIn as QuoteTime, " +
              "    tbl_Audit.AuditID, tbl_Audit.AuditCode, tbl_Audit.Intime as AuditTime, " +
              "    tbl_BidPlan.BidPlanID, tbl_BidPlan.BidPlanCode, tbl_BidPlan.DateIn as BidPlanTime," +
              "    tbl_Negotiation.NegotiationID, tbl_Negotiation.NegotiationCode, tbl_Negotiation.DateIn as NegotiationTime," +
              "    tbl_Decision.DecisionID, tbl_Decision.DecisionCode, tbl_Negotiation.DateIn as DecisionTime," +
              "    tbl_Contract.ContractID, tbl_Contract.ContractCode, tbl_Negotiation.DateIn as ContractTime" +
              "    from tbl_Quote" +
              "    inner join tbl_Audit_Quote  on tbl_Quote.QuoteID = tbl_Audit_Quote.QuoteID" +
              "    inner join tbl_Audit  on tbl_Audit_Quote.AuditID = tbl_Audit.AuditID" +
              "    left join tbl_BidPlan  on tbl_Quote.QuoteID = tbl_BidPlan.QuoteID" +
              "    left join tbl_Negotiation  on tbl_Quote.QuoteID = tbl_Audit_Quote.QuoteID" +
              "    left join tbl_Decision  on tbl_Quote.QuoteID = tbl_Decision.QuoteID" +
              "    left join tbl_Contract  on tbl_Quote.QuoteID = tbl_Contract.QuoteID" +
              "    where tbl_Quote.QuoteID = " + quoteID
                , connection))
            {
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        result.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        result.QuoteTime = GetDbReaderValue<DateTime>(reader["QuoteTime"]);

                        result.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        result.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                        result.AuditTime = GetDbReaderValue<DateTime>(reader["AuditTime"]);

                        result.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        result.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        result.BidPlanTime = GetDbReaderValue<DateTime>(reader["BidPlanTime"]);

                        result.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        result.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        result.NegotiationTime = GetDbReaderValue<DateTime>(reader["NegotiationTime"]);


                        result.DecisionID = GetDbReaderValue<int>(reader["DecisionID"]);
                        result.DecisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                        result.DecisionTime = GetDbReaderValue<DateTime>(reader["DecisionTime"]);

                        result.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        result.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        result.ContractTime = GetDbReaderValue<DateTime>(reader["ContractTime"]);
                    }
                }
            }
            return result;
        }

        public List<ProposalSelectItem> getListProsalCanCreateDR(SqlConnection connection, string code)
        {
            var result = new List<ProposalSelectItem>();

            string query = " Select TOP 10  tblProcess.ProposalID, ProposalCode from( select DISTINCT PP.ProposalID, sum(QI.ItemPrice) as TotalPrice FROM tbl_Proposal_Process PP join tbl_Quote_Item QI on QI.QUoteID = PP.QUoteID " +
                " where  PP.QuoteID is not null  and (BidPlanID is not Null or TotalPrice < 20000000) and NegotiationID is not null and  DecisionID is not null" + // and AuditID is not null
                " and ContractID is not null and DeliveryReceiptID is null " +
                "   group by PP.ProposalID ) as tblProcess " +
                " inner join tbl_Proposal on tblProcess.ProposalID = tbl_Proposal.ProposalID where ProposalCode like '%" + code + "%' ";
            using (var command = new SqlCommand(query, connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProposalSelectItem record = new ProposalSelectItem();
                        record.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        record.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        result.Add(record);
                    }
                }
                return result;
            }
        }
        //Nguyen Minh Hoang
        //11-6-2020
        public List<ProposalInfo> GetAllOutdateProposal(SqlConnection connection, string userI)
        {
            var result = new List<ProposalInfo>();
            using (var command = new SqlCommand("Select P.ProposalID,P.ProposalCode, PT.TypeName," +
                " D.DepartmentName,D1.DepartmentName as CurDepartmentName," +
                " Format(P.DateIn,'dd/MM/yyyy') as DateIn," +
                " Format(P.DueDate,'dd/MM/yyyy') as DueDate," +
                " DATEDIFF(day, P.DueDate, GETDATE()) AS DateDiff " +
                " from tbl_Proposal P left join tbl_Department D on P.DepartmentID = D.DepartmentID  " +
                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID where 1 = 1 and DueDate < GETDATE() ", connection))
            {
                if(!string.IsNullOrEmpty(userI) && userI != "admin")
                {
                    command.CommandText += " and P.UserI = @UserI ";
                    AddSqlParameter(command, "@UserI", userI, System.Data.SqlDbType.VarChar);
                }
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalInfo();
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        string dateIn = GetDbReaderValue<string>(reader["DateIn"]);
                        string dueDate = GetDbReaderValue<string>(reader["DueDate"]);
                        info.DateIn = DateTime.ParseExact(dateIn, "d/M/yyyy", new CultureInfo("fr-FR"));
                        info.DueDate = DateTime.ParseExact(dueDate, "d/M/yyyy", new CultureInfo("fr-FR"));
                        info.DateDiff = GetDbReaderValue<int>(reader["DateDiff"]);
                        result.Add(info);
                    }
                }
            }
            return result;
        }
        public List<ProposalsByDepartment> CountByDepartment(SqlConnection connection)
        {

            var result = new List<ProposalsByDepartment>();
            using (var command = new SqlCommand("Select count(P.ProposalCode) Number," +
                "D.DepartmentName from tbl_Proposal P left join tbl_Department D on P.DepartmentID = D.DepartmentID  " +
                "left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID " +
                "left join tbl_ProposalType PT on P.ProposalType = PT.TypeID " +
                "where 1 = 1 " +
                "group by D.DepartmentName " +
                "order by Number", connection))
            {
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalsByDepartment();
                        info.label = (GetDbReaderValue<string>(reader["DepartmentName"])).ToString();
                        info.value = (GetDbReaderValue<int>(reader["Number"])).ToString();
                        result.Add(info);
                    }
                }
            }

            return result;
        }
        public List<ProposalInfo> GetAllExceedReserveProposal(SqlConnection connection)
        {
            var result = new List<ProposalInfo>();
            using (var command = new SqlCommand("Select P.ProposalID,P.ProposalCode, PT.TypeName," +
                " D.DepartmentName,D1.DepartmentName as CurDepartmentName," +
                " Format(P.DateIn,'dd/MM/yyyy') as DateIn," +
                " Format(P.DueDate,'dd/MM/yyyy') as DueDate," +
                " IsExceedReserve " +
                " from tbl_Proposal P left join tbl_Department D on P.DepartmentID = D.DepartmentID  " +
                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID " +
                " left join tbl_ProposalType PT on P.ProposalType = PT.TypeID " +
                "where 1 = 1 and IsExceedReserve = 1 " +
                "and DueDate > GETDATE() ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalInfo();
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        string dateIn = GetDbReaderValue<string>(reader["DateIn"]);
                        string dueDate = GetDbReaderValue<string>(reader["DueDate"]);
                        info.DateIn = DateTime.ParseExact(dateIn, "d/M/yyyy", new CultureInfo("fr-FR"));
                        info.DueDate = DateTime.ParseExact(dueDate, "d/M/yyyy", new CultureInfo("fr-FR"));
                        info.IsExceedReserve = GetDbReaderValue<bool>(reader["IsExceedReserve"]) == true ? "Dự trù" : "Chưa dự trù";
                        result.Add(info);
                    }
                }
            }
            return result;
        }
        public List<ProposalRelatedData> GetProposalProccess(SqlConnection connection, int proposalID)
        {
            var result = new List<ProposalRelatedData>();
            using (var command = new SqlCommand("SELECT TOP 2 [ID]," +
                "[ProposalID],[ProposalCode],[ProposalTime]," +
                "[SurveyID],[SurveyCode],[SurveyTime]," +
                "[ExplanationID],[ExplanationCode],[ExplanationTime],[IsHasExplanation]," +
                "[QuoteID],[QuoteCode],[QuoteTime],[QuoteTotalCost]," +
                "[BidPlanID],[BidPlanCode],[BidPlanTime],[IsHasBidPlan]," +
                "[NegotiationID],[NegotiationCode],[NegotiationTime]," +
                "[DecisionID],[DecisionCode],[DecisionTime]," +
                "[ContractID],[ContractCode],[ContractTime]," +
                "[DeliveryReceiptID],[DeliveryReceiptCode],[DeliveryReceiptTime]," +
                "[AcceptanceID],[AcceptanceCode],[AcceptanceTime],[CurrentFeature]," +
                "[AuditID],[AuditTime]FROM [tbl_Proposal_Process] " +
                "where ProposalID = " + proposalID, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalRelatedData();
                        SingleDetailsData item = new SingleDetailsData();
                        List<SingleDetailsData> lstItem = new List<SingleDetailsData>();

                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);

                        item.id = GetDbReaderValue<int>(reader["SurveyID"]);
                        item.code = GetDbReaderValue<string>(reader["SurveyCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["SurveyTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["ExplanationID"]);
                        item.code = GetDbReaderValue<string>(reader["ExplanationCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["ExplanationTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["QuoteID"]);
                        item.code = GetDbReaderValue<string>(reader["QuoteCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["QuoteTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["BidPlanID"]);
                        item.code = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["BidPlanTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["NegotiationID"]);
                        item.code = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["NegotiationTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["DecisionID"]);
                        item.code = GetDbReaderValue<string>(reader["DecisionCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["DecisionTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["ContractID"]);
                        item.code = GetDbReaderValue<string>(reader["ContractCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["ContractTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        item.code = GetDbReaderValue<string>(reader["DeliveryReceiptCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["DeliveryReceiptTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["AcceptanceID"]);
                        item.code = GetDbReaderValue<string>(reader["AcceptanceCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["AcceptanceTime"]);
                        info.lstProccess.Add(item);
                        //
                        item = new SingleDetailsData();
                        item.id = GetDbReaderValue<int>(reader["AuditID"]);
                        //item.code = GetDbReaderValue<string>(reader["DecisionCode"]);
                        item.date = GetDbReaderValue<DateTime>(reader["AuditTime"]);
                        info.lstProccess.Add(item);
                        //

                        result.Add(info);
                    }
                }
            }
            return result;
        }


        public ProcessInfo GetProposalProccessID(SqlConnection connection, int proposalID)
        {
            var result = new ProcessInfo();
            using (var command = new SqlCommand("select * from tbl_Proposal_Process " +
                "where ProposalID = " + proposalID, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        result.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        result.SurveyID = GetDbReaderValue<int>(reader["SurveyID"]);
                        result.ExplanationID = GetDbReaderValue<int>(reader["ExplanationID"]);
                        result.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        result.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        result.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        result.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        result.DecisionID = GetDbReaderValue<int>(reader["DecisionID"]);
                        result.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        result.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        result.AcceptanceID = GetDbReaderValue<int>(reader["AcceptanceID"]);
                    }
                }
            }
            return result;
        }
    }
}
