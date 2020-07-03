using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class AuditDataLayer : BaseLayerData<AuditDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<AuditInfo></returns>
        /// 
     
    
        public List<int> getQuoteOfAudit(SqlConnection connection, int _ID)
        {
            List <int> result = new List<int>();
            using (var command = new SqlCommand(
                " select QuoteID from tbl_Audit_Quote where AuditID = @AuditID "
                , connection))
            {
                AddSqlParameter(command, "@AuditID", _ID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(GetDbReaderValue<int>(reader["QuoteID"]));
              
                    }
                }
                return result;
            }
        }



        public AuditDetailInfo getAudtiGeneralInfo(SqlConnection connection, int _ID,string _userID)
        {
            AuditDetailInfo result = null;
            using (var command = new SqlCommand("  Select A.*, "+
              "   Preside.Name as PresideName, Preside.RoleName as PresideRoleName, Preside.Title as PresideTitle, Secretary.Name as SecretaryName, "+
              "    Secretary.RoleName as SecretaryRoleName, " +
              "    Secretary.Title as SecretaryTitle  from(Select A.* from tbl_Audit A where A.AuditID = @AuditID) as A " +
              "    inner JOIN tbl_Employee Preside  on A.Preside = Preside.EmployeeID " +
              "    inner JOIN tbl_Employee Secretary  on A.Secretary = Secretary.EmployeeID "

                , connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (A.UserAssign = @UserID or A.UserI = @UserID)";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@AuditID", _ID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new AuditDetailInfo();
                        result.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        result.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);                                         
                        result.Location = GetDbReaderValue<string>(reader["Location"]);
                        result.Preside = GetDbReaderValue<int>(reader["Preside"]);
                        result.PresideName = GetDbReaderValue<string>(reader["PresideName"]);
                        result.PresideTitle = GetDbReaderValue<string>(reader["PresideTitle"]);
                        result.PresideRoleName = GetDbReaderValue<string>(reader["PresideRoleName"]);
                        result.Secretary = GetDbReaderValue<int>(reader["Secretary"]);
                        result.SecretaryName = GetDbReaderValue<string>(reader["SecretaryName"]);
                        result.SecretaryTitle = GetDbReaderValue<string>(reader["SecretaryTitle"]);
                        result.SecretaryRoleName = GetDbReaderValue<string>(reader["SecretaryRoleName"]);
                        result.Members = GetDbReaderValue<string>(reader["Members"]);
                        result.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        result.StartTime = GetDbReaderValue<DateTime>(reader["StartTime"]);
                        result.EndTime = GetDbReaderValue<DateTime>(reader["EndTime"]);
                    //    result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        break;
                    }
                }
                return result;
            }
        }
        public AuditDetailInfo GetAuditByCode(SqlConnection connection, string code, string _userID)
        {
            AuditDetailInfo result = null;
            using (var command = new SqlCommand(" Select A.*, Q.QuoteCode, Q.IsVAT,  Q.VATNumber,  P.ProposalCode, D.DepartmentName , D1.DepartmentCode as CurDepartmentCode " +
               " , Preside.Name as PresideName , Preside.RoleName as PresideRoleName  , Preside.Title as PresideTitle  " +
                " , Secretary.Name as SecretaryName , Secretary.RoleName as SecretaryRoleName  , Secretary.Title as SecretaryTitle  " +
                " from (Select A.* " +
                " from tbl_Audit A where  A.AuditCode = @AuditCode) as A " +
                " Join tbl_Audit_Quote AQ on AQ.AuditID = A.AuditID "+
                " LEFT JOIN tbl_Quote Q on AQ.QuoteID  = Q.QuoteID " +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = A.ProposalID " +

                " LEFT JOIN tbl_Employee Preside on A.Preside  = Preside.EmployeeID " +
                " LEFT JOIN tbl_Employee Secretary on A.Secretary  = Secretary.EmployeeID " +

                " left join tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID  " +
                " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (A.UserAssign = @UserID or A.UserI = @UserID)";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@AuditCode", code, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new AuditDetailInfo();
                        result.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        result.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);                     
                        result.Location = GetDbReaderValue<string>(reader["Location"]);
                        result.Preside = GetDbReaderValue<int>(reader["Preside"]);
                        result.PresideName = GetDbReaderValue<string>(reader["PresideName"]);
                        result.PresideTitle = GetDbReaderValue<string>(reader["PresideTitle"]);
                        result.PresideRoleName = GetDbReaderValue<string>(reader["PresideRoleName"]);
                        result.Secretary = GetDbReaderValue<int>(reader["Secretary"]);
                        result.SecretaryName = GetDbReaderValue<string>(reader["SecretaryName"]);
                        result.SecretaryTitle = GetDbReaderValue<string>(reader["SecretaryTitle"]);
                        result.SecretaryRoleName = GetDbReaderValue<string>(reader["SecretaryRoleName"]);
                        result.Members = GetDbReaderValue<string>(reader["Members"]);
                        result.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        result.StartTime = GetDbReaderValue<DateTime>(reader["StartTime"]);
                        result.EndTime = GetDbReaderValue<DateTime>(reader["EndTime"]);
                        result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return result;
            }
        }
        public int getTotalRecords(SqlConnection connection, AuditSeachCriteria _criteria,string _userID)
        {


            string Condition = "";
            if (_criteria.pageSize == 0) _criteria.pageSize = 10;
            var offSet = _criteria.pageIndex * _criteria.pageSize;

            if (_criteria.AuditCode != "" && _criteria.AuditCode != null)
            {
                Condition += " and tblA.AuditCode like  '%" + _criteria.AuditCode + "%' ";
            }
            
            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                Condition += " and tblQ.QuoteCode  like '%" + _criteria.QuoteCode + "%'";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                Condition += " and (tblA.UserAssign = " + _userID+ " or tblA.UserI = " + _userID + ")";
            }
                string query = $"select count(AuditID) as TotalRecords from(" +
                " select DISTINCT  T.AuditID, T.UpdateTime from(" +
                " select tblA.AuditID, tblA.UpdateTime, tblA.AuditCode, tblQ.QuoteCode, tblP.ProposalCode, tblA.Intime  from tbl_Audit tblA" +
                " inner join tbl_Audit_Quote tblAQ on tblA.AuditID = tblAQ.AuditID    " +
                " inner join tbl_Quote tblQ  on tblAQ.QuoteID = tblQ.QuoteID " +
                " inner Join tbl_Quote_Proposal tblQP " +
                " on tblQP.QuoteID = tblQ.QuoteID  inner join tbl_Proposal tblP  on tblP.ProposalID = tblQP.ProposalID  " +
                "  where  tblA.InTime between @FromDate and @ToDate " + Condition + "  ) as T  " +
                "   ) tbltemp    ";

           
            using (var command = new SqlCommand(query, connection))
                {
              
                AddSqlParameter(command, "@FromDate", _criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
                    
                  
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

        public int InsertAudit(SqlConnection connection, AuditInfo _Audit, string _userI)
        {
            int lastestInserted = 0;
            if (_Audit.AuditCode == "" || _Audit.AuditCode == null)
            {
                _Audit.AuditCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Audit] (AuditCode, StartTime, EndTime, Location, Preside, Secretary, Members, Comment, UserI )" +
                    "VALUES(@AuditCode , @StartTime, @EndTime, @Location,@Preside, @Secretary, @Members, @Comment, @UserI) " +
                    "select IDENT_CURRENT('dbo.tbl_Audit') as LastInserted ", connection))
            {

                AddSqlParameter(command, "@AuditCode", _Audit.AuditCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@StartTime", _Audit.StartTime, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@EndTime", _Audit.EndTime, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@Location", _Audit.Location, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Preside", _Audit.Preside, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Secretary", _Audit.Secretary, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Members", _Audit.Members, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Comment", _Audit.Comment, System.Data.SqlDbType.NText);
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

        public void InsertAuditQuote(SqlConnection connection, int AuditID, int QuoteID)
        {
            var currenttime = DateTime.Now.Date;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Audit_Quote] (AuditID, QuoteID)" +
                    "VALUES(@AuditID ,  @QuoteID) " , connection))
            {
                AddSqlParameter(command, "@AuditID", AuditID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

  
            using (var command = new SqlCommand("update  tbl_Proposal_Process " +
                "set AuditID=@AuditID  , AuditTime=@AuditTime ,  CurrentFeature=@CurrentFeature where QuoteID=@QuoteID", connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@AuditID", AuditID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@AuditTime", currenttime, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@CurrentFeature", "Audit", System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            
        }


        public void UpdateAudit(SqlConnection connection, int _id, AuditInfo _Audit, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Audit \n" +
                            " SET  InTime=@InTime, StartTime = @StartTime ,EndTime = @EndTime ,AuditCode=@AuditCode " +
                            ",Location = @Location , Preside = @Preside , Secretary = @Secretary , Members = @Members ,Comment = @Comment, UserU=@UserU,UpdateTime=getdate() \n" +
                            " WHERE (AuditID = @AuditID) ", connection))            
            {
                AddSqlParameter(command, "@AuditID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@AuditCode", _Audit.AuditCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@StartTime", _Audit.StartTime, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@EndTime", _Audit.EndTime, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@Location", _Audit.Location, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Preside", _Audit.Preside, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Secretary", _Audit.Secretary, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Members", _Audit.Members, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Comment", _Audit.Comment, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@InTime", _Audit.InTime, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void Delete(SqlConnection connection,int _AuditID)
        {
            using (var command = new SqlCommand(" Delete tbl_Audit where AuditID=@AuditID  ", connection))
            {
                AddSqlParameter(command, "@AuditID", _AuditID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set AuditID = Null where AuditID = @AuditID  ", connection))
            {
                AddSqlParameter(command, "@AuditID", _AuditID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string _AuditIDs)
        {
            using (var command = new SqlCommand(" Delete tbl_Audit where AuditID in (" + _AuditIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set AuditID = Null where AuditID in (" + _AuditIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        

        public List<string> getListAuditCode(SqlConnection connection, string auditCode)
        {
            var result = new List<string>();
            using (var command = new SqlCommand(" Select TOP 10 AuditCode from tbl_Audit where AuditCode like '%" + auditCode + "%'", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _auditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                        result.Add(_auditCode);
                    }
                }
                return result;
            }
        }


        public List<AuditEmployeeInfo> GetAuditEmployeesById(SqlConnection connection, string id)
        {
            List<AuditEmployeeInfo> ret = new List<AuditEmployeeInfo>();
            using (var command = new SqlCommand(@"select AE.AutoID, AE.AuditID, AE.EmployeeID, AE.Comment,E.Title, E.RoleName, E.Name, E.Generic , E.DepartmentID , E.UserID from
                (select * from tbl_AuditEmployee where AuditID = @AuditID ) AE
                left join  tbl_Employee as E on E.EmployeeID = AE.EmployeeID
                ", connection))
            {
                AddSqlParameter(command, "@AuditID", id, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new AuditEmployeeInfo();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        item.EmployeeID = GetDbReaderValue<int>(reader["EmployeeID"]);
                        item.Comment = GetDbReaderValue<string>(reader["Comment"]);
            
                        item.Title = GetDbReaderValue<string>(reader["Title"]);
                        item.RoleName = GetDbReaderValue<string>(reader["RoleName"]);
                        item.Name = GetDbReaderValue<string>(reader["Name"]);
                        item.Generic = GetDbReaderValue<int>(reader["Generic"]);
                        item.DepartmentID = GetDbReaderValue<string>(reader["DepartmentID"]);
                        item.UserID = GetDbReaderValue<string>(reader["UserID"]);
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }

        public List<AuditEmployeeInfo> GetAuditEmployeesByIds(SqlConnection connection, string ids)
        {
            List<AuditEmployeeInfo> ret = new List<AuditEmployeeInfo>();
            using (var command = new SqlCommand(@"select AE.AutoID, AE.AuditID, AE.EmployeeID, AE.Comment,  E.Title, E.RoleName, E.Name, E.Generic , E.DepartmentID , E.UserID from
                (select * from tbl_AuditEmployee where AuditID in (" + ids + @")) AE
                left join  tbl_Employee as E on E.EmployeeID = AE.EmployeeID
                left join tbl_EmployeeRoles as ER on ER.RoleID = E.Role
                ", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new AuditEmployeeInfo();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        item.EmployeeID = GetDbReaderValue<int>(reader["EmployeeID"]);
                        item.Comment = GetDbReaderValue<string>(reader["Comment"]);

                        item.Title = GetDbReaderValue<string>(reader["Title"]);
                        item.RoleName = GetDbReaderValue<string>(reader["RoleName"]);
                        item.Name = GetDbReaderValue<string>(reader["Name"]);
                        item.Generic = GetDbReaderValue<int>(reader["Generic"]);
                        item.DepartmentID = GetDbReaderValue<string>(reader["DepartmentID"]);
                        item.UserID = GetDbReaderValue<string>(reader["UserID"]);
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }

        public List<AuditEmployeeInfo> GetAuditDefaultMember(SqlConnection connection)
        {
            List<AuditEmployeeInfo> ret = new List<AuditEmployeeInfo>();
            using (var command = new SqlCommand(@"select  * from tbl_Employee where IsAuditMem = 'true' 
            order by OrderAuditMem
                ", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new AuditEmployeeInfo();
                        item.EmployeeID = GetDbReaderValue<int>(reader["EmployeeID"]);
                        item.Title = GetDbReaderValue<string>(reader["Title"]);
                        item.RoleName = GetDbReaderValue<string>(reader["RoleName"]);
                        item.Name = GetDbReaderValue<string>(reader["Name"]);
                        item.Generic = GetDbReaderValue<int>(reader["Generic"]);
                        item.DepartmentID = GetDbReaderValue<string>(reader["DepartmentID"]);
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }

        public void InsertAuditEmployee(SqlConnection connection, int _AuditID, AuditEmployeeInfo item)
        {
            using (var command = new SqlCommand("Insert into [dbo].[tbl_AuditEmployee] ([AuditID], [EmployeeID],[Comment])" +
                   "VALUES(@AuditID,@EmployeeID,@Comment)", connection))
            {

                AddSqlParameter(command, "@AuditID", _AuditID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@EmployeeID", item.EmployeeID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Comment", item.Comment, System.Data.SqlDbType.NVarChar);

                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

    

        public void DeleteAuditEmployees(SqlConnection connection, string _AuditEmployeeID)
        {
            using (var command = new SqlCommand(" delete tbl_AuditEmployee where AutoID in (" + _AuditEmployeeID + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }



        public List<string> GetAuditByQuoteIds(SqlConnection connection, string quoteids)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select AuditID as ID from tbl_Proposal_Process  where QuoteID in (" + quoteids + ")", connection))
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

        /////////////////////////////////////////


        public SearchAuditInfo getAuditInfoByQuote(SqlConnection connection, int QuoteInfo)
        {
            SearchAuditInfo ret = new SearchAuditInfo();
            using (var command = new SqlCommand(
                " select tblA.AuditCode, tblA.AuditID, tbla.Intime from tbl_Audit_Quote tblAQ" +
                " inner join tbl_Audit tblA " +
                " on tblAQ.AuditID = tblA.AuditID " + 
                "  where tblAQ.QuoteID = " + QuoteInfo,
                connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]).ToString();
                        ret.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        ret.InTime = GetDbReaderValue<DateTime>(reader["InTime"]);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="_criteria"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public List<SearchAuditInfo> getAuditNew(SqlConnection connection, AuditSeachCriteria _criteria,string _userID)
        {
            var result = new List<SearchAuditInfo>();

            string Condition = "";
            if (_criteria.pageSize == 0) _criteria.pageSize = 10;
            var offSet = _criteria.pageIndex * _criteria.pageSize;

            if (_criteria.AuditCode != "" && _criteria.AuditCode != null)
            {
                Condition += " and tblA.AuditCode like  '%" + _criteria.AuditCode + "%' ";
            }

            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                Condition += " and tblQ.QuoteCode  like '%" + _criteria.QuoteCode + "%'";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                Condition += " and (tblA.UserAssign = " + _userID + " or tblA.UserI = " + _userID + ")";
            }
            string query = $"select tblA.AuditID , tblA.UpdateTime,  tblA.AuditCode,  tblQ.QuoteCode, tblP.ProposalCode , tblA.Intime "+
                " from(select * from tbl_Audit   where tbl_Audit.AuditID in "+
                " (select AuditID from("+
                " select DISTINCT  T.AuditID, T.UpdateTime from(" +
                " select tblA.AuditID, tblA.UpdateTime, tblA.AuditCode, tblQ.QuoteCode, tblP.ProposalCode, tblA.Intime  from tbl_Audit tblA" +
                " inner join tbl_Audit_Quote tblAQ on tblA.AuditID = tblAQ.AuditID    " +
                " inner join tbl_Quote tblQ  on tblAQ.QuoteID = tblQ.QuoteID " +
                " inner Join tbl_Quote_Proposal tblQP " +
                " on tblQP.QuoteID = tblQ.QuoteID  inner join tbl_Proposal tblP  on tblP.ProposalID = tblQP.ProposalID  " +
                "  where  tblA.InTime between @FromDate and @ToDate  " +  Condition + " ) as T  " +
                " order by T.UpdateTime  " +
                " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY " +
                "  ) tbltemp))tblA " +
                " inner join tbl_Audit_Quote tblAQ on tblA.AuditID = tblAQ.AuditID   inner join tbl_Quote tblQ  on tblAQ.QuoteID = tblQ.QuoteID " +
                " inner Join tbl_Quote_Proposal tblQP " +
                " on tblQP.QuoteID = tblQ.QuoteID  inner join tbl_Proposal tblP  on tblP.ProposalID = tblQP.ProposalID   ";

            using (var command = new SqlCommand(query, connection))
            {

                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", _criteria.pageSize, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@FromDate", _criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);            
                WriteLogExecutingCommand(command);

                int tempAuditID = 0;
                bool isNeedAdd = false;
                var  info = new SearchAuditInfo();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {                
                        if (tempAuditID != GetDbReaderValue<int>(reader["AuditID"])) {
                            if (info.AuditID != 0)
                            {
                                isNeedAdd = false;
                                result.Add(info);
                            }
                            info = new SearchAuditInfo();
                            info.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                            info.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                            info.QuoteCodes = GetDbReaderValue<string>(reader["QuoteCode"]);
                            info.ProposalCodes = GetDbReaderValue<string>(reader["ProposalCode"]);
                            info.InTime = GetDbReaderValue<DateTime>(reader["InTime"]);
                            isNeedAdd = true;
                            tempAuditID = GetDbReaderValue<int>(reader["AuditID"]);                         
                        }
                        else
                        {
                            string proCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                            string QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                            if (info.ProposalCodes.Contains(proCode) == false)
                            {
                                info.ProposalCodes += " , " + proCode;
                            }
                            if (info.QuoteCodes.Contains(QuoteCode) == false)
                            {
                                info.QuoteCodes += " , " + QuoteCode;
                            }
                            tempAuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        }                                           
                    }
                    if (isNeedAdd)
                    {
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<AuditInfo></returns>
        /// 


        public List<string> GetQuoteByAuditIds(SqlConnection connection, string auditIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select QuoteID as ID from tbl_Proposal_Process  where AuditID in (" + auditIDs + ")", connection))
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
