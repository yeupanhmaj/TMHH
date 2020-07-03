using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class SurveyDataLayer : BaseLayerData<SurveyDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<SurveyInfo></returns>
        /// 
        public List<SurveyInfo> GetAllSurvey(SqlConnection connection,string _userID)
        {
            var result = new List<SurveyInfo>();
            using (var command = new SqlCommand("Select S.*, SD.DepartmentName, P.ProposalCode, P.ProposalType, PT.TypeName, P.DepartmentID as ProDepartmentID, D.DepartmentName as ProDepartmentName  " +
            " from tbl_Survey S   " +
            " left join tbl_Department SD on S.SurveyDepartmentID = SD.DepartmentID " +
            " left join tbl_Proposal P on P.ProposalID = S.ProposalID" +
            " left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
            " left join tbl_ProposalType PT on PT.TypeID = P.ProposalType" +
            " where 1=1 "
            , connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( S.UserAssign = " + _userID + " ) or ( S.UserI = " + _userID + " )";

                }
                command.CommandText += " order by S.UpdateTime Desc ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new SurveyInfo();
                        info.SurveyID = GetDbReaderValue<int>(reader["SurveyID"]);
                        info.SurveyCode = GetDbReaderValue<string>(reader["SurveyCode"]);
                        info.SurveyName = GetDbReaderValue<string>(reader["SurveyName"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        info.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.TypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["ProDepartmentID"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["ProDepartmentName"]);
                        info.SurveyDepartmentID = GetDbReaderValue<int>(reader["SurveyDepartmentID"]);
                        info.SurveyDepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.Solution = GetDbReaderValue<int>(reader["Solution"]);
                        info.SolutionText = GetDbReaderValue<string>(reader["SolutionText"]);
                        info.IsSample = GetDbReaderValue<bool>(reader["IsSample"]);
                        info.Valid = GetDbReaderValue<bool>(reader["Valid"]);
                        info.ValidText = GetDbReaderValue<string>(reader["ValidText"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.DateIn = GetDbReaderValue<DateTime?>(reader["DateIn"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<SurveyInfo> getSurvey(SqlConnection connection, SurveySeachCriteria criteria,string _userID)
        {
            var result = new List<SurveyInfo>();
            using (var command = new SqlCommand("Select S.*, P.ProposalCode, P.DepartmentID,   D.DepartmentName, P.DepartmentID as ProDepartmentID , P.ProposalType, PT.TypeName, D1.DepartmentName as CurDepartmentName, D.DepartmentName as ProDepartmentName " + 
                " from tbl_Survey  S" +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = S.ProposalID " +
                " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
                " left join tbl_Department D1 on P.CurDepartmentID = D1.DepartmentID " +
                " left join tbl_ProposalType PT on PT.TypeID = P.ProposalType " + 
            " where   S.ProposalID <> 0 ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( S.UserAssign = " + _userID + " ) or ( S.UserI = " + _userID + " )";

                }
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
                    command.CommandText += " and S.Intime between @FromDate and @ToDate ";
                    AddSqlParameter(command, "@FromDate", criteria.fromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@ToDate", criteria.toDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
                }

                if (criteria.pageSize == 0) criteria.pageSize = 10;
                var offSet = criteria.pageIndex * criteria.pageSize;
                command.CommandText += " order by S.UpdateTime Desc  ";
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", criteria.pageSize, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new SurveyInfo();
                        info.SurveyID = GetDbReaderValue<int>(reader["SurveyID"]);
                        info.SurveyCode = GetDbReaderValue<string>(reader["SurveyCode"]);
                        info.SurveyName = GetDbReaderValue<string>(reader["SurveyName"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                        info.TypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["ProDepartmentID"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["ProDepartmentName"]);
                        info.SurveyDepartmentID = GetDbReaderValue<int>(reader["SurveyDepartmentID"]);
                        info.SurveyDepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.Solution = GetDbReaderValue<int>(reader["Solution"]);
                        info.SolutionText = GetDbReaderValue<string>(reader["SolutionText"]);
                        info.IsSample = GetDbReaderValue<bool>(reader["IsSample"]);
                        info.Valid = GetDbReaderValue<bool>(reader["Valid"]);
                        info.ValidText = GetDbReaderValue<string>(reader["ValidText"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.DateIn = GetDbReaderValue<DateTime?>(reader["DateIn"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
                return result;
                }
            }
        

        public SurveyDetailInfo getSurveyDetail(SqlConnection connection, int ID,string _userID)
        {
            SurveyDetailInfo result = null;
            using (var command = new SqlCommand("Select S.*, SD.DepartmentName, P.ProposalCode, P.ProposalType, PT.TypeName, P.DepartmentID as ProDepartmentID, " +
                "D.DepartmentName as ProDepartmentName, P.DateIn as ProposalDate " +
            "from (Select * from tbl_Survey  where 1= 1  ", connection))
            {
                {
                    if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                    {
                        command.CommandText += " and ( UserAssign = " + _userID + " ) or ( UserI = " + _userID + " )";

                    }
                    command.CommandText += " and SurveyID = @SurveyID";
                    AddSqlParameter(command, "@SurveyID", ID, System.Data.SqlDbType.Int);
                    command.CommandText += "  ) as S  " +
                    " left join tbl_Department SD on S.SurveyDepartmentID = SD.DepartmentID " +
                    " left join tbl_Proposal P on P.ProposalID = S.ProposalID" +
                    " left join tbl_Department D on P.DepartmentID = D.DepartmentID " +
                    " left join tbl_ProposalType PT on PT.TypeID = P.ProposalType ";
                    WriteLogExecutingCommand(command);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new SurveyDetailInfo();
                            result.SurveyID = GetDbReaderValue<int>(reader["SurveyID"]);
                            result.SurveyCode = GetDbReaderValue<string>(reader["SurveyCode"]);
                            result.SurveyName = GetDbReaderValue<string>(reader["SurveyName"]);
                            result.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                            result.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                            result.ProposalType = GetDbReaderValue<int>(reader["ProposalType"]);
                            result.ProposalTypeName = GetDbReaderValue<string>(reader["TypeName"]);
                            result.TypeName = GetDbReaderValue<string>(reader["TypeName"]);
                            result.DepartmentID = GetDbReaderValue<int>(reader["ProDepartmentID"]);
                            result.DepartmentName = GetDbReaderValue<string>(reader["ProDepartmentName"]);
                            result.SurveyDepartmentID = GetDbReaderValue<int>(reader["SurveyDepartmentID"]);
                            result.SurveyDepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                            result.Comment = GetDbReaderValue<string>(reader["Comment"]);
                             result.Status = GetDbReaderValue<string>(reader["Status"]);
                            result.Solution = GetDbReaderValue<int>(reader["Solution"]);
                            result.SolutionText = GetDbReaderValue<string>(reader["SolutionText"]);
                            result.IsSample = GetDbReaderValue<bool>(reader["IsSample"]);
                            result.Valid = GetDbReaderValue<bool>(reader["Valid"]);
                            result.ValidText = GetDbReaderValue<string>(reader["ValidText"]);
                            result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                            result.ProposalDate = GetDbReaderValue<DateTime>(reader["ProposalDate"]);
                            result.ProductsName = GetDbReaderValue<string>(reader["ProductsName"]);
                            result.DepartmentComment = GetDbReaderValue<string>(reader["DepartmentComment"]);
                            result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                            result.DateIn = GetDbReaderValue<DateTime?>(reader["DateIn"]);
                            result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                            result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        }
                    }
                    return result;
                }
            }
        }
        public int getTotalRecords(SqlConnection connection, SurveySeachCriteria criteria,string _userID)
        {
            if (criteria != null)
            {
                using (var command = new SqlCommand("Select count(temp.SurveyID) as TotalRecords from (Select S.*, P.ProposalCode, D.DepartmentName " +
               " from tbl_Survey S  " +
               " LEFT JOIN tbl_Proposal P on P.ProposalID  = S.ProposalID " +
               " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
               " where S.SurveyID <> 0   ", connection))
                {
                    if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                    {
                        command.CommandText += " and ( S.UserAssign = " + _userID + " ) or ( S.UserI = " + _userID + " )";
                    }
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
                        AddSqlParameter(command, "@FromDate", criteria.fromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                        AddSqlParameter(command, "@ToDate", criteria.toDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
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
        public int InsertSurvey(SqlConnection connection, SurveyInfo _Survey, string _userI)
        {
            int lastestInserted = 0;
            var currenttime = DateTime.Now.Date;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Survey] (SurveyCode, ProposalID,SurveyDepartmentID,  Comment, Solution, SolutionText, IsSample, Valid, ValidText, ProductsName, DepartmentComment, Status,  UserI, DateIn)" +
                    "VALUES(@SurveyCode, @ProposalID,@SurveyDepartmentID,  @Comment, @Solution, @SolutionText, @IsSample, @Valid, @ValidText,@ProductsName, @DepartmentComment, @Status,  @UserI, @DateIn) " +
                    " select IDENT_CURRENT('dbo.tbl_Survey') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@SurveyCode", "KS-" + _Survey.ProposalCode, System.Data.SqlDbType.NVarChar);
                // AddSqlParameter(command, "@SurveyCode", _Survey.SurveyCode, System.Data.SqlDbType.NVarChar);
                // AddSqlParameter(command, "@SurveyName", _Survey.SurveyName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ProposalID", _Survey.ProposalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@SurveyDepartmentID", _Survey.SurveyDepartmentID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Comment", _Survey.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Solution", _Survey.Solution, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@SolutionText", _Survey.SolutionText, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@IsSample", _Survey.IsSample, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@Valid", _Survey.Valid, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@ValidText", _Survey.ValidText, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ProductsName", _Survey.ProductsName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DepartmentComment", _Survey.DepartmentComment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Status", _Survey.Status, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DateIn", _Survey.DateIn, System.Data.SqlDbType.DateTime);
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
                    "set SurveyID=@SurveyID  , SurveyCode=@SurveyCode, SurveyTime=@SurveyTime, CurrentFeature=@CurrentFeature where ProposalID=@ProposalID", connection))
                {
                    AddSqlParameter(command, "@SurveyID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@ProposalID", _Survey.ProposalID, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@SurveyCode", "KS-" + _Survey.ProposalCode, System.Data.SqlDbType.VarChar);
                    AddSqlParameter(command, "@SurveyTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "Survey", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }
            return lastestInserted;
        }

        public void UpdateSurvey(SqlConnection connection, int _id, SurveyInfo _Survey, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Survey \n" +
                            " SET ProposalID = @ProposalID , SurveyDepartmentID = @SurveyDepartmentID, ProductsName = @ProductsName , DepartmentComment = @DepartmentComment , " +
                            " Comment = @Comment, Status = @Status , Solution=@Solution, SolutionText=@SolutionText, IsSample=@IsSample, Valid=@Valid, ValidText=@ValidText, UserU=@UserU,UpdateTime=getdate(), DateIn = @DateIn \n" +
                            " WHERE (SurveyID = @SurveyID) ", connection))
               // " Insert into tbl_Survey_Log ([SurveyID],[SurveyName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime])  (select [SurveyID],[SurveyName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime] from tbl_Survey where SurveyID=@SurveyID ) "
            {
                AddSqlParameter(command, "@SurveyID", _id, System.Data.SqlDbType.Int);
             //   AddSqlParameter(command, "@SurveyCode", _Survey.SurveyCode, System.Data.SqlDbType.NVarChar);
            //   AddSqlParameter(command, "@SurveyName", _Survey.SurveyName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ProposalID", _Survey.ProposalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@SurveyDepartmentID", _Survey.SurveyDepartmentID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Comment", _Survey.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Solution", _Survey.Solution, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@SolutionText", _Survey.SolutionText, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@IsSample", _Survey.IsSample, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@Valid", _Survey.Valid, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@ValidText", _Survey.ValidText, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Status", _Survey.Status, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DateIn", _Survey.DateIn, System.Data.SqlDbType.DateTime);

                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ProductsName", _Survey.ProductsName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DepartmentComment", _Survey.DepartmentComment, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void Delete(SqlConnection connection,int _SurveyID)
        {
            using (var command = new SqlCommand(" Delete tbl_Survey where SurveyID=@SurveyID ", connection))
            {
                AddSqlParameter(command, "@SurveyID", _SurveyID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string ids)
        {
            using (var command = new SqlCommand("Delete tbl_Survey where SurveyID in (" + ids + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public List<string> GetSurveyByProposalId(SqlConnection connection, string proposalIds)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select SurveyID as ID from tbl_Survey  where ProposalID in (" + proposalIds + ")", connection))
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
