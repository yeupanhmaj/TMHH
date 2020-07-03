using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class BidPlanDataLayer : BaseLayerData<BidPlanDataLayer>
    {

        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<BidPlanInfo></returns>
        /// 
        public List<BidPlanInfo> GetAllBidPlan(SqlConnection connection,string _userID)
        {
            var result = new List<BidPlanInfo>();
            using (var command = new SqlCommand("Select BP.*, Q.QuoteCode " + //, A.AuditCode, P.ProposalCode, D.DepartmentName
                " from tbl_BidPlan BP  " +
              
                " LEFT JOIN tbl_Quote Q on Q.QuoteID  = BP.QuoteID " +
                //" LEFT JOIN tbl_Proposal P on P.ProposalID  = BP.ProposalID " +
               // " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
                " where  1 = 1 order by BP.UpdateTime Desc ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new BidPlanInfo();
                        info.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        info.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                       
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.Bid = GetDbReaderValue<string>(reader["Bid"]);
                        info.BidName = GetDbReaderValue<string>(reader["BidName"]);
                      
                       
                        info.BidTime = GetDbReaderValue<string>(reader["BidTime"]); 
                        info.BidLocation = GetDbReaderValue<string>(reader["BidLocation"]);
                        info.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]); 
                        info.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        info.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        info.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
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
        public List<BidPlanInfo> getBidPlan(SqlConnection connection, BidPlanSeachCriteria _criteria,string _userID)
        {
            var result = new List<BidPlanInfo>();
            string query = " Select BP.* ," +
                " Q.QuoteID, Q.IsVAT ,  Q.VATNumber ," +
                " Q.QuoteCode from (Select " +
                "BP.* " +
                " from tbl_BidPlan BP where  1 = 1 and BP.DateIn between @FromDate and @ToDate";
             query += " ) as BP " +
            " LEFT JOIN tbl_Quote Q on BP.QuoteID  = Q.QuoteID " +
            " LEFT JOIN tbl_Quote_Customer QC on QC.QuoteID  = Q.QuoteID  and QC.[IsChoosed] = 1 " +
            " LEFT JOIN tbl_Customer C on C.CustomerID = QC.CustomerID " +
            " where  1 = 1 ";
            if (_criteria.BidPlanCode != null && _criteria.BidPlanCode != "")
            {
                query += " and BP.BidPlanCode like '%" + _criteria.BidPlanCode + "%'";
            }
            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                query += " and Q.QuoteCode  like '%" + _criteria.QuoteCode + "%'";
            }
            if (_criteria.CustomerID != 0)
            {
                query += " and C.CustomerID =" + _criteria.CustomerID + " ";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                query += " and (BP.UserI = @UserID  or BP.UserAssign =@UserID )";              
            }
            using (var command = new SqlCommand(query, connection)) //, A.AuditID  , A.AuditCode, D.DepartmentName, P.ProposalCode
            {
                AddSqlParameter(command, "@FromDate",_criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
                command.CommandText += " order by BP.UpdateTime Desc ";           
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
                        var info = new BidPlanInfo();
                        info.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        info.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                      
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                      
                        info.Bid = GetDbReaderValue<string>(reader["Bid"]);
                        info.BidName = GetDbReaderValue<string>(reader["BidName"]);
                       
                        info.BidTime = GetDbReaderValue<string>(reader["BidTime"]);
                        info.BidLocation = GetDbReaderValue<string>(reader["BidLocation"]);
                        info.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]);
                        info.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        info.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        info.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
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

        public BidPlanInfo getBidPlan(SqlConnection connection, int _ID,string _userID)
        {
            BidPlanInfo result = null;
            using (var command = new SqlCommand(
                "Select BP.* , tblCap.CapitalName, Q.InTime as QuoteCreateTime, Q.TotalCost, Q.QuoteID, Q.QuoteCode, Q.IsVAT, tblA.AuditCode, tblA.Intime as AuditTime, " +
                "Q.VATNumber from(Select BP.* from tbl_BidPlan BP where BP.BidPlanID = @BidPlanID) " +
                "as BP   LEFT JOIN tbl_Quote Q on BP.QuoteID = Q.QuoteID " +
                "left join tbl_Audit_Quote tblAQ on tblAQ.QuoteID = Q.QuoteID " +
                "left join tbl_Audit tblA on tblA.AuditID = tblAQ.AuditID " +
                "left join tbl_Capital tblCap on tblCap.CapitalID = BP.CapitalID "
            , connection))
            {
                AddSqlParameter(command, "@BidPlanID", _ID, System.Data.SqlDbType.Int);
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (BP.UserI = @UserID or BP.UserAssign = @UserID )";
                }
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new BidPlanInfo();
                        result.QuoteCreateTime = GetDbReaderValue<DateTime>(reader["QuoteCreateTime"]);
                      
                        result.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        result.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                      
                        result.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        result.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        result.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        result.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        result.AuditTime = GetDbReaderValue<DateTime>(reader["AuditTime"]);
                        result.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                        result.Bid = GetDbReaderValue<string>(reader["Bid"]);
                        result.BidName = GetDbReaderValue<string>(reader["BidName"]);
                        result.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                        result.CapitalName = GetDbReaderValue<string>(reader["CapitalName"]);
                        result.BidTime = GetDbReaderValue<string>(reader["BidTime"]);
                        result.BidLocation = GetDbReaderValue<string>(reader["BidLocation"]);
                        result.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]);
                        result.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        result.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        result.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);
                        result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return result;
            }
        }

        public BidPlanInfo GetBidPlanByCode(SqlConnection connection, string _code)
        {
            BidPlanInfo result = null;
            using (var command = new SqlCommand("Select BP.*  ,Q.InTime as QuoteCreateTime, Q.TotalCost , Q.QuoteID, Q.IsVAT,  Q.VATNumber, Q.QuoteCode from (Select BP.* " + 
                " from tbl_BidPlan BP where  BP.BidPlanCode = @BidPlanCode) as BP  " +

                " LEFT JOIN tbl_Quote Q on BP.QuoteID  = Q.QuoteID "              
                , connection))
            {
                AddSqlParameter(command, "@BidPlanCode", _code, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new BidPlanInfo();
                        result.QuoteCreateTime = GetDbReaderValue<DateTime>(reader["QuoteCreateTime"]);
                        
                        result.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        result.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                       
                        result.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        result.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        result.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        result.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        
                        result.Bid = GetDbReaderValue<string>(reader["Bid"]);
                        result.BidName = GetDbReaderValue<string>(reader["BidName"]);
                        result.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                        result.BidTime = GetDbReaderValue<string>(reader["BidTime"]);
                        result.BidLocation = GetDbReaderValue<string>(reader["BidLocation"]);
                        result.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]);
                        result.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        result.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        result.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);
                        result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return result;
            }
        }


        public BidPlanPrint getBidPlanInfoByQuote(SqlConnection connection, int QuoteInfo)
        {
            BidPlanPrint ret = new BidPlanPrint();
            using (var command = new SqlCommand(
                " select tblBP.* ,tblC.CapitalName  from tbl_BidPlan tblBP inner join tbl_Capital tblC on tblC.CapitalID =  tblBP.CapitalID  " +
                "  where tblBP.QuoteID = " + QuoteInfo,
                connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        ret.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);
                        ret.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        ret.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        ret.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                        ret.CapitalName = GetDbReaderValue<string>(reader["CapitalName"]);
                    }
                }
            }
            return ret;
        }


        public int getTotalRecords(SqlConnection connection, BidPlanSeachCriteria _criteria,string _userID)
        {

            string query = " Select count (BP.BidPlanID)  as TotalRecords   from (Select " +
               "BP.* " +
               " from tbl_BidPlan BP where  1 = 1 and BP.DateIn between @FromDate and @ToDate";
            query += " ) as BP " +
           " LEFT JOIN tbl_Quote Q on BP.QuoteID  = Q.QuoteID " +
           " LEFT JOIN tbl_Quote_Customer QC on QC.QuoteID  = Q.QuoteID  and QC.[IsChoosed] = 1 " +
           " LEFT JOIN tbl_Customer C on C.CustomerID = QC.CustomerID " +
           " where  1 = 1 ";
            if (_criteria.BidPlanCode != null && _criteria.BidPlanCode != "")
            {
                query += " and BP.BidPlanCode like '%" + _criteria.BidPlanCode + "%'";
            }
            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                query += " and Q.QuoteCode  like '%" + _criteria.QuoteCode + "%'";
            }
            if (_criteria.CustomerID != 0)
            {
                query += " and C.CustomerID =" + _criteria.CustomerID + " ";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                query += " and (BP.UserI = "+ _userID + "or BP.UserAssign = " + _userID + " )";
            }
            using (var command = new SqlCommand(query, connection))
                {
                    AddSqlParameter(command, "@FromDate",_criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
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
        public int InsertBidPlan(SqlConnection connection, BidPlanInfo _BidPlan, string _userI)
        {
            int lastestInserted = 0;
            var currenttime = DateTime.Now.Date;
            if (_BidPlan.BidPlanCode == null || _BidPlan.BidPlanCode == "") _BidPlan.BidPlanCode = DateTime.Now.ToString("yyMMddHHmmssfff");
            using (var command = new SqlCommand("Insert into [dbo].[tbl_BidPlan] (BidPlanCode, QuoteID, Bid, BidName, BidTime, BidLocation, BidMethod, BidType, BidExpirated, BidExpiratedUnit, UserI, DateIn , CapitalID)" +
                    "VALUES(@BidPlanCode, @QuoteID, @Bid , @BidName, @BidTime,@BidLocation, @BidMethod, @BidType, @BidExpirated, @BidExpiratedUnit, @UserI, @DateIn ,@CapitalID) " +
                    "select IDENT_CURRENT('dbo.tbl_BidPlan') as LastInserted ", connection))
            {
                //    AddSqlParameter(command, "@BidPlanCode", _BidPlan.BidPlanCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidPlanCode", _BidPlan.BidPlanCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@QuoteID", _BidPlan.QuoteID, System.Data.SqlDbType.Int);

                AddSqlParameter(command, "@CapitalID", _BidPlan.CapitalID, System.Data.SqlDbType.Int);

                
                AddSqlParameter(command, "@Bid", _BidPlan.Bid, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidName", _BidPlan.BidName, System.Data.SqlDbType.NVarChar);
              
                AddSqlParameter(command, "@BidTime", _BidPlan.BidTime, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidLocation", _BidPlan.BidLocation, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidMethod", _BidPlan.BidMethod, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidType", _BidPlan.BidType, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidExpirated", _BidPlan.BidExpirated, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidExpiratedUnit", _BidPlan.BidExpiratedUnit, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DateIn", _BidPlan.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
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
                    "set BidPlanID=@BidPlanID  , BidPlanTime=@BidPlanTime ,  CurrentFeature=@CurrentFeature where QuoteID=@QuoteID", connection))
                {
                    AddSqlParameter(command, "@QuoteID", _BidPlan.QuoteID, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@BidPlanID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@BidPlanTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "BidPlan", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }


            return lastestInserted;
        }

        public void UpdateBidPlan(SqlConnection connection, int _id, BidPlanInfo _BidPlan, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_BidPlan \n" +
                            " SET  QuoteID = @QuoteID , Bid = @Bid ,BidName = @BidName, CapitalID=@CapitalID  " +
                            ",BidTime = @BidTime , BidLocation = @BidLocation , BidMethod = @BidMethod , BidType = @BidType , BidExpirated=@BidExpirated, BidExpiratedUnit=@BidExpiratedUnit , UserU=@UserU,UpdateTime=getdate(), DateIn = @DateIn \n" +
                            " WHERE (BidPlanID = @BidPlanID) ", connection))
               // " Insert into tbl_BidPlan_Log ([BidPlanID],[BidPlanName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime])  (select [BidPlanID],[BidPlanName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime] from tbl_BidPlan where BidPlanID=@BidPlanID ) "
            {
               AddSqlParameter(command, "@BidPlanID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidPlanCode", _BidPlan.BidPlanCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@QuoteID", _BidPlan.QuoteID, System.Data.SqlDbType.Int);
               // AddSqlParameter(command, "@ProposalID", _BidPlan.ProposalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Bid", _BidPlan.Bid, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidName", _BidPlan.BidName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@CapitalID", _BidPlan.CapitalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidTime", _BidPlan.BidTime, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidLocation", _BidPlan.BidLocation, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidMethod", _BidPlan.BidMethod, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidType", _BidPlan.BidType, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidExpirated", _BidPlan.BidExpirated, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidExpiratedUnit", _BidPlan.BidExpiratedUnit, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DateIn", _BidPlan.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void Delete(SqlConnection connection,int _BidPlanID)
        {
            using (var command = new SqlCommand(" Delete tbl_BidPlan where BidPlanID=@BidPlanID ", connection))
            {
                AddSqlParameter(command, "@BidPlanID", _BidPlanID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set BidPlanID = Null where BidPlanID = @BidPlanID  ", connection))
            {
                AddSqlParameter(command, "@BidPlanID", _BidPlanID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string _BidPlanIDs)
        {
            using (var command = new SqlCommand(" Delete tbl_BidPlan where BidPlanID in (" + _BidPlanIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set BidPlanID = Null where BidPlanID in (" + _BidPlanIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public List<string> getListBidPlanCode(SqlConnection connection, string bidPlanCode)
        {
            var result = new List<string>();
            using (var command = new SqlCommand(" Select TOP 10 BidPlanCode from tbl_BidPlan where BidPlanCode like '%" + bidPlanCode + "%'", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _bidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        result.Add(_bidPlanCode);
                    }
                }
                return result;
            }
        }

        public List<string> GetBidPlanByQuoteIds(SqlConnection connection, string quoteIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select BidPlanID as ID from tbl_Proposal_Process  where QuoteID in (" + quoteIDs + ")", connection))
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

        public List<string> GetQuoteByBidPlanIds(SqlConnection connection, string bidplanIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select QuoteID as ID from tbl_Proposal_Process  where BidPlanID in (" + bidplanIDs + ")", connection))
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
