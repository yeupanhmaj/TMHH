using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class ExplanationDataLayer : BaseLayerData<ExplanationDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<ExplanationInfo></returns>
        /// 
        public List<ExplanationInfo> GetAllExplanation(SqlConnection connection,string _userID)
        {
            var result = new List<ExplanationInfo>();
            using (var command = new SqlCommand("Select E.*, P.ProposalCode, P.DepartmentID, " +
                " D.DepartmentName, P.ProposalType, PT.TypeName " +
            " from tbl_Explanation E " +
            " left join tbl_Proposal P on P.ProposalID = E.ProposalID" +
            " left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
            " left join tbl_ProposalType PT on PT.TypeID = P.ProposalType "+
            " where  1 = 1 order by E.UpdateTime Desc ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( E.UserAssign = @UserID ) or ( E.UserI = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ExplanationInfo();
                        info.ExplanationID = GetDbReaderValue<int>(reader["ExplanationID"]);
                        info.ExplanationCode = GetDbReaderValue<string>(reader["ExplanationCode"]);
                        info.ExplanationName = GetDbReaderValue<string>(reader["ExplanationName"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        info.TypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.Necess = GetDbReaderValue<bool>(reader["Necess"]);
                        info.Suitable = GetDbReaderValue<bool>(reader["Suitable"]);
                        info.NBNum = GetDbReaderValue<string>(reader["NBNum"]);
                        info.XNNum = GetDbReaderValue<string>(reader["XNNum"]);
                        info.Available = GetDbReaderValue<string>(reader["Available"]);
                        info.IsAvailable = GetDbReaderValue<bool>(reader["IsAvailable"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.TNCB = GetDbReaderValue<string>(reader["TNCB"]);
                        info.Status = GetDbReaderValue<string>(reader["Status"]);
                        info.DBLTCN = GetDbReaderValue<string>(reader["DBLTCN"]);
                        info.NVHTTB = GetDbReaderValue<string>(reader["NVHTTB"]);
                        info.DTNL = GetDbReaderValue<string>(reader["DTNL"]);
                        info.NQL = GetDbReaderValue<string>(reader["NQL"]);
                        info.HQKTXH = GetDbReaderValue<string>(reader["HQKTXH"]);
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

        public ExplanationDetailInfo getExplanationDetail(SqlConnection connection, int ID,string _userID)
        {
            ExplanationDetailInfo result = null;
            using (var command = new SqlCommand("Select E.*, P.ProposalCode, P.DepartmentID,  D.DepartmentName, " +
                " P.ProposalType, PT.TypeName from (Select * " +
                " from tbl_Explanation where  1 = 1 ", connection))
            {
                command.CommandText += " and ExplanationID = @ExplanationID";
                AddSqlParameter(command, "@ExplanationID", ID, SqlDbType.Int);
                command.CommandText += "  ) as E " +
                    " left join tbl_Proposal P on P.ProposalID = E.ProposalID" +
                    " left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
                    " left join tbl_ProposalType PT on PT.TypeID = P.ProposalType  where 1 = 1";
                command.CommandText += " order by E.UpdateTime Desc ";
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( E.UserAssign = @UserID ) or ( E.UserI = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new ExplanationDetailInfo();
                        result.ExplanationID = GetDbReaderValue<int>(reader["ExplanationID"]);
                        result.ExplanationCode = GetDbReaderValue<string>(reader["ExplanationCode"]);
                        result.ExplanationName = GetDbReaderValue<string>(reader["ExplanationName"]);
                        result.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        result.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        result.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        result.TypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        result.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        result.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        result.Necess = GetDbReaderValue<bool>(reader["Necess"]);
                        result.Suitable = GetDbReaderValue<bool>(reader["Suitable"]);
                        result.NBNum = GetDbReaderValue<string>(reader["NBNum"]);
                        result.XNNum = GetDbReaderValue<string>(reader["XNNum"]);
                        result.Available = GetDbReaderValue<string>(reader["Available"]);
                        result.IsAvailable = GetDbReaderValue<bool>(reader["IsAvailable"]);
                        result.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        result.TNCB = GetDbReaderValue<string>(reader["TNCB"]);
                        result.Status = GetDbReaderValue<string>(reader["Status"]);
                        result.DBLTCN = GetDbReaderValue<string>(reader["DBLTCN"]);
                        result.NVHTTB = GetDbReaderValue<string>(reader["NVHTTB"]);
                        result.DTNL = GetDbReaderValue<string>(reader["DTNL"]);
                        result.NQL = GetDbReaderValue<string>(reader["NQL"]);
                        result.HQKTXH = GetDbReaderValue<string>(reader["HQKTXH"]);
                        result.ProductsName = GetDbReaderValue<string>(reader["ProductsName"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return result;
            }
        }

        public List<ExplanationInfo> getExplanation(SqlConnection connection, ExplanationSeachCriteria criteria,string _userID)
        {
            var result = new List<ExplanationInfo>();
            using (var command = new SqlCommand("Select E.*, P.ProposalCode, P.DepartmentID,  D.DepartmentName, P.ProposalType, PT.TypeName, D1.DepartmentName as CurDepartmentName" +
                " from tbl_Explanation  E" +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = E.ProposalID " +
                " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
                " LEFT JOIN tbl_Department D1 on P.CurDepartmentID = D1.DepartmentID " +
                " LEFT JOIN tbl_ProposalType PT on PT.TypeID  = P.ProposalType " +
                " where   E.ProposalID <> 0 ", connection))
            {
                if (criteria.proposalCode != "" && criteria.proposalCode != null)
                {
                    command.CommandText += " and P.ProposalCode like  '%" + criteria.proposalCode + "%' ";
                }
                if (criteria.departmentID != 0)
                {
                    command.CommandText += " and ( P.departmentID = @departmentID ";
                    command.CommandText += " or  P.CurDepartmentID = @departmentID ) ";
                    AddSqlParameter(command, "@departmentID", criteria.departmentID, System.Data.SqlDbType.Int);
                }
                if (criteria.fromDate != null && criteria.toDate != null)
                {
                    command.CommandText += " and P.DateIn between @FromDate and @ToDate ";
                    AddSqlParameter(command, "@FromDate", criteria.fromDate, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@ToDate", criteria.toDate, System.Data.SqlDbType.DateTime);
                }
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( E.UserAssign = @UserID ) or ( E.UserI = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                if (criteria.pageSize == 0) criteria.pageSize = 10;
                var offSet = criteria.pageIndex * criteria.pageSize;
                command.CommandText += " order by E.UpdateTime Desc";
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", criteria.pageSize, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ExplanationInfo();
                        info.ExplanationID = GetDbReaderValue<int>(reader["ExplanationID"]);
                        info.ExplanationCode = GetDbReaderValue<string>(reader["ExplanationCode"]);
                        info.ExplanationName = GetDbReaderValue<string>(reader["ExplanationName"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        info.TypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.Necess = GetDbReaderValue<bool>(reader["Necess"]);
                        info.Suitable = GetDbReaderValue<bool>(reader["Suitable"]);
                        info.NBNum = GetDbReaderValue<string>(reader["NBNum"]);
                        info.XNNum = GetDbReaderValue<string>(reader["XNNum"]);
                        info.Available = GetDbReaderValue<string>(reader["Available"]);
                        info.IsAvailable = GetDbReaderValue<bool>(reader["IsAvailable"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.TNCB = GetDbReaderValue<string>(reader["TNCB"]);
                        info.Status = GetDbReaderValue<string>(reader["Status"]);
                        info.DBLTCN = GetDbReaderValue<string>(reader["DBLTCN"]);
                        info.NVHTTB = GetDbReaderValue<string>(reader["NVHTTB"]);
                        info.DTNL = GetDbReaderValue<string>(reader["DTNL"]);
                        info.NQL = GetDbReaderValue<string>(reader["NQL"]);
                        info.HQKTXH = GetDbReaderValue<string>(reader["HQKTXH"]);
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
        public int getTotalRecords(SqlConnection connection, ExplanationSeachCriteria criteria,string _userID)
        {
            if (criteria != null)
            {
                using (var command = new SqlCommand("Select count(temp.ExplanationID) as TotalRecords from (Select E.ExplanationID " +
               " from tbl_Explanation E  " +
               " LEFT JOIN tbl_Proposal P on P.ProposalID  = E.ProposalID " +
               " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
               " where E.ProposalID <> 0   ", connection))
                {


                    if (criteria.proposalCode != "" && criteria.proposalCode != null)
                    {
                        command.CommandText += " and P.ProposalCode like  '%" + criteria.proposalCode + "%' ";
                    }
                    if (criteria.departmentID != 0)
                    {
                        command.CommandText += " and ( P.departmentID = @departmentID ";
                        command.CommandText += " or  P.CurDepartmentID = @departmentID ) ";
                        AddSqlParameter(command, "@departmentID", criteria.departmentID, System.Data.SqlDbType.Int);
                    }
                    if (criteria.fromDate != null && criteria.toDate != null)
                    {
                        command.CommandText += " and P.DateIn between @FromDate and @ToDate ";
                        AddSqlParameter(command, "@FromDate", criteria.fromDate, System.Data.SqlDbType.DateTime);
                        AddSqlParameter(command, "@ToDate", criteria.toDate, System.Data.SqlDbType.DateTime);
                    }
                    if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                    {
                        command.CommandText += " and ( E.UserAssign = @UserID ) or ( E.UserI = @UserID )";
                        AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
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

            }
            else
            {
                using (var command = new SqlCommand("Select count(*) as TotalRecords  from tbl_Explanation where 1 = 1 ", connection))
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
        public int InsertExplanation(SqlConnection connection, ExplanationInfo _Explanation, string _userI)
        {
            int lastestInserted = 0;
            var currenttime = DateTime.Now.Date;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Explanation] (ExplanationCode,ExplanationName, ProposalID, Necess, Suitable, NBNum, XNNum, Available, IsAvailable, Comment, TNCB, DBLTCN, NVHTTB, DTNL, NQL, HQKTXH, ProductsName, Status,UserI)" +
                    "VALUES(@ExplanationCode,@ExplanationName, @ProposalID, @Necess, @Suitable, @NBNum, @XNNum, @Available, @IsAvailable, @Comment, @TNCB, @DBLTCN, @NVHTTB, @DTNL, @NQL, @HQKTXH , @ProductsName , @Status, @UserI)  " +
                    " select IDENT_CURRENT('dbo.tbl_Explanation') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@ExplanationName", _Explanation.ExplanationName, System.Data.SqlDbType.NVarChar);
                //  AddSqlParameter(command, "@ExplanationCode",  _Explanation.ExplanationCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ExplanationCode", "GTMS-" + _Explanation.ProposalCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ProposalID", _Explanation.ProposalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Necess",  _Explanation.Necess, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@Suitable",_Explanation.Suitable, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@NBNum", _Explanation.NBNum, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@XNNum", _Explanation.XNNum, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Available", _Explanation.Available, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@IsAvailable", _Explanation.IsAvailable, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@Comment", _Explanation.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@TNCB", _Explanation.TNCB, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DBLTCN", _Explanation.DBLTCN, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Status", _Explanation.Status, System.Data.SqlDbType.NVarChar);

                AddSqlParameter(command, "@NVHTTB", _Explanation.NVHTTB, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DTNL", _Explanation.DTNL, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@NQL", _Explanation.NQL, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@HQKTXH", _Explanation.HQKTXH, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ProductsName", _Explanation.ProductsName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }

            if (lastestInserted != 0)
            {
                using (var command = new SqlCommand("update  tbl_Proposal_Process " +
                    "set ExplanationID=@ExplanationID  , ExplanationCode=@ExplanationCode, ExplanationTime=@ExplanationTime, CurrentFeature=@CurrentFeature where ProposalID=@ProposalID"
                    , connection))
                {
                    AddSqlParameter(command, "@ExplanationID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@ProposalID", _Explanation.ProposalID, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@ExplanationCode", "GTMS-" + _Explanation.ProposalCode, System.Data.SqlDbType.VarChar);
                    AddSqlParameter(command, "@ExplanationTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "Explanation", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }
            return lastestInserted;
        }

        public void UpdateExplanation(SqlConnection connection, int _id, ExplanationInfo _Explanation, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Explanation \n" +
                            " SET ProposalID = @ProposalID , " +
                            " Necess = @Necess, Status=@Status , Suitable = @Suitable, NBNum = @NBNum, XNNum = @XNNum,Available = @Available,  IsAvailable = @IsAvailable, ProductsName=@ProductsName ,  " +
                            " Comment = @Comment,TNCB = @TNCB,DBLTCN = @DBLTCN,NVHTTB = @NVHTTB,DTNL = @DTNL,NQL = @NQL,HQKTXH = @HQKTXH, UserU=@UserU,UpdateTime=getdate() \n" +
                            " WHERE (ExplanationID = @ExplanationID) ", connection))
               // " Insert into tbl_Explanation_Log ([ExplanationID],[ExplanationName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime])  (select [ExplanationID],[ExplanationName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime] from tbl_Explanation where ExplanationID=@ExplanationID ) "
            {
                AddSqlParameter(command, "@ExplanationID", _id, System.Data.SqlDbType.Int);
             //   AddSqlParameter(command, "@ExplanationName", _Explanation.ExplanationName, System.Data.SqlDbType.NVarChar);
             //   AddSqlParameter(command, "@ExplanationCode", _Explanation.ExplanationCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ProposalID", _Explanation.ProposalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Necess", _Explanation.Necess, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@Suitable", _Explanation.Suitable, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@NBNum", _Explanation.NBNum, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@XNNum", _Explanation.XNNum, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Available", _Explanation.Available, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@IsAvailable", _Explanation.IsAvailable, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@Comment", _Explanation.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Status", _Explanation.Status, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@TNCB", _Explanation.TNCB, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DBLTCN", _Explanation.DBLTCN, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@NVHTTB", _Explanation.NVHTTB, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DTNL", _Explanation.DTNL, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@NQL", _Explanation.NQL, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@HQKTXH", _Explanation.HQKTXH, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ProductsName", _Explanation.ProductsName, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void Delete(SqlConnection connection,int _ExplanationID)
        {
            using (var command = new SqlCommand("Delete tbl_Explanation where ExplanationID=@ExplanationID ", connection))
            {
                AddSqlParameter(command, "@ExplanationID", _ExplanationID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string ids)
        {
            using (var command = new SqlCommand("Delete tbl_Explanation where ExplanationID in (" + ids + ")" , connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


        public List<string> GetExplanationsByProposalId(SqlConnection connection, string proposalIds,string _userID)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select ExplanationID as ID from tbl_Explanation E  where ProposalID in (" + proposalIds + ")", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( E.UserAssign = @UserID ) or ( E.UserI = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
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
