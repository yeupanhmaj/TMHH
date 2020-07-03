using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class DecisionDataLayer : BaseLayerData<DecisionDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// </summary>
        /// <returns>Return List<DecisionInfo></returns>
        /// 
        public List<DecisionInfo> GetAllDecision(SqlConnection connection, string _userID)
        {
            var result = new List<DecisionInfo>();
            using (var command = new SqlCommand("Select D.* , BP.BidPlanID, BP.BidPlanCode,  Q.QuoteCode," +
                "  Q.IsVAT,  Q.VATNumber" + //, P.ProposalCode 
                " from tbl_Decision D " +
                  " LEFT JOIN tbl_Quote Q on D.QuoteID  = Q.QuoteID " +
                //" LEFT JOIN tbl_Negotiation N on N.NegotiationID  = D.NegotiationID " +
                " LEFT JOIN tbl_BidPlan BP on BP.QuoteID  = Q.QuoteID " +
                " where  1 = 1 order by D.UpdateTime Desc ", connection))

            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (D.UserI = @UserID or D.UserU = @UserID or D.UserAssign =@UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new DecisionInfo();
                        info.DecisionID = GetDbReaderValue<int>(reader["DecisionID"]);
                        info.DecisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                       
                        info.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        info.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]);
                        info.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                     
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                       
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
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
        public List<DecisionInfo> getDecision(SqlConnection connection, DecisionSeachCriteria _criteria,string _userID)
        {
            var result = new List<DecisionInfo>();
            using (var command = new SqlCommand(" Select D.* , BP.BidPlanID, BP.BidPlanCode,  Q.QuoteCode,  Q.IsVAT,  Q.VATNumber from (Select D.* " +

                " from tbl_Decision D where  1 = 1 and D.DateIn between @FromDate and @ToDate ", connection))
            {
                AddSqlParameter(command, "@FromDate", _criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
               
                if (!string.IsNullOrEmpty(_criteria.DecisionCode))
                {
                    command.CommandText += " and D.DecisionCode = @DecisionCode";
                    AddSqlParameter(command, "@DecisionCode", _criteria.DecisionCode, System.Data.SqlDbType.NVarChar);
                }
               
                command.CommandText += "  ) as D " +
                " LEFT JOIN tbl_Quote Q on D.QuoteID  = Q.QuoteID " +
                " LEFT JOIN tbl_BidPlan BP on BP.QuoteID  = Q.QuoteID " +        
                " Left join tbl_Quote_Customer QC on Q.QuoteID = QC.QuoteID  and QC.IsChoosed = 1 "+
                " LEFT join tbl_Customer C on C.CustomerID = QC.CustomerID " +
               
                " where  1 = 1 ";

                if (!string.IsNullOrEmpty(_criteria.QuoteCode))
                {
                    command.CommandText += " and Q.QuoteCode like '%" + _criteria.QuoteCode + "%'";

                }
                if (!string.IsNullOrEmpty(_criteria.DecisionCode))
                {
                    command.CommandText += " and D.DecisionCode like '%" + _criteria.DecisionCode + "%'";

                }
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (D.UserI = @UserID or D.UserU = @UserID or D.UserAssign =@UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                if (_criteria.CustomerID !=0)
                {
                    command.CommandText += " and C.CustomerID = @CustomerID";
                    AddSqlParameter(command, "@CustomerID", _criteria.CustomerID, System.Data.SqlDbType.Int);
                }
                command.CommandText += " order by D.UpdateTime Desc ";


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
                        var info = new DecisionInfo();
                        info.DecisionID = GetDbReaderValue<int>(reader["DecisionID"]);
                        info.DecisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                        //info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        //info.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        info.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        info.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]);
                        info.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        //info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        //info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        //info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        //info.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        //info.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
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

        public List<string> GetListDecisionByCode(SqlConnection connection, string code,string _userID)
        {
            var result = new List<string>();
            using (var command = new SqlCommand(" Select TOP 10 DecisionCode from " +
                " tbl_Decision D where D.DecisionCode like '%" + code + "%'", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (D.UserI = @UserID or D.UserU = @UserID or D.UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string codeTemp = GetDbReaderValue<string>(reader["DecisionCode"]);
                        result.Add(codeTemp);
                    }
                }
                return result;
            }
        }

        public DecisionInfo getDecision(SqlConnection connection, int _ID,string _userID)
        {
            DecisionInfo info = null;
            using (var command = new SqlCommand(" Select D.* , tblCap.CapitalName , tblN.BidType , tblN.BidExpirated " +
                ",tblN.BidExpiratedUnit ,BP.BidPlanID, BP.BidPlanCode, Q.QuoteCode, Q.IsVAT,  Q.VATNumber ," +
            " Q.TotalCost as QuoteTotalCost, Q.DateIn as QuoteTime,  BP.DateIn as BidPlanTime, tblA.AuditCode " +
            ", tblA.InTime as AuditTime ,  tblN.NegotiationCode , tblN.DateIn as NegotiationTime , " +
            " C.CustomerName, C.Address from (Select D.* " +
            " from tbl_Decision D where  D.DecisionID = @DecisionID) as D " +
            " LEFT JOIN tbl_Quote Q on D.QuoteID  = Q.QuoteID " +
            " LEFT JOIN tbl_BidPlan BP on BP.QuoteID  = Q.QuoteID " +
            " Left join tbl_Quote_Customer QC on Q.QuoteID = QC.QuoteID  and QC.IsChoosed = 1 " +
            " LEFT join tbl_Customer C on C.CustomerID = QC.CustomerID " +
            " inner join tbl_Audit_Quote tblAQ on tblAQ.QuoteID = Q.QuoteID " +
            " inner join tbl_Audit tblA on tblA.AuditID = tblAQ.AuditID " +
            " inner JOIN tbl_Negotiation tblN on tblN.QuoteID = D.QuoteID " +
            " left join tbl_Capital tblCap on tblCap.CapitalID = D.CapitalID " +
            " where  1 = 1", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (D.UserI = @UserID or D.UserU = @UserID or D.UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@DecisionID", _ID, SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new DecisionInfo();
                        info.DecisionID = GetDbReaderValue<int>(reader["DecisionID"]);
                        info.DecisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                        //info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);

                        info.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        info.NegotiationTime = GetDbReaderValue<DateTime>(reader["NegotiationTime"]);


                        info.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        info.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        info.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                        info.CapitalName = GetDbReaderValue<string>(reader["CapitalName"]);
                        //info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        //info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);

                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        //info.AuditID = GetDbReaderValue<int>(reader["AuditID"]);

                        info.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                        info.AuditTime = GetDbReaderValue<DateTime>(reader["AuditTime"]);

                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);

                        info.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        info.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]);
                        info.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        info.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);

                        info.BidPlanTime = GetDbReaderValue<DateTime>(reader["BidPlanTime"]);
                     
                        info.QuoteTime = GetDbReaderValue<DateTime>(reader["QuoteTime"]);
                      
                     
                        info.Address = GetDbReaderValue<string>(reader["Address"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                    }
                }
                return info;
            }
        }

        public DecisionInfo GetDecisionByCode(SqlConnection connection, string _code, string _userID)
        {
            DecisionInfo info = null;

            //using (var command = new SqlCommand(" Select D.* , N.NegotiationCode, BP.BidPlanID, BP.BidPlanCode, A.AuditID, A.AuditCode, Q.QuoteID, Q.QuoteCode, Q.IsVAT,  Q.VATNumber ," +
            //        "Q.TotalCost as QuoteTotalCost, Q.Intime as QuoteTime,  BP.InTime as BidPlanTime, N.InTime as NegotiationTime, C.CustomerName, C.Address , A.InTime as  AuditTime  , P.ProposalCode, DP.DepartmentName  from (Select D.* " +
            using (var command = new SqlCommand(" Select D.* , tblCap.CapitalName, BP.BidPlanID, BP.BidPlanCode, Q.QuoteCode, Q.IsVAT,  Q.VATNumber ," +
                    "Q.TotalCost as QuoteTotalCost, Q.DateIn as QuoteTime,  BP.DateIn as BidPlanTime,  C.CustomerName, C.Address  from (Select D.* " + //,  P.ProposalCode
                " from tbl_Decision D where  D.DecisionCode = @DecisionCode) as D " +
               " LEFT JOIN tbl_Quote Q on D.QuoteID  = Q.QuoteID " +
                " LEFT JOIN tbl_BidPlan BP on BP.QuoteID  = Q.QuoteID " +
                  " Left join tbl_Quote_Customer QC on Q.QuoteID = QC.QuoteID  and QC.IsChoosed = 1 " +
               " LEFT join tbl_Customer C on C.CustomerID = QC.CustomerID " +
                "left join tbl_Capital tblCap on tblCap.CapitalID = D.CapitalID " + 
                "where  1 = 1", connection))

            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (D.UserI = @UserID or D.UserU = @UserID or D.UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@DecisionCode", _code, SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new DecisionInfo();
                        info.DecisionID = GetDbReaderValue<int>(reader["DecisionID"]);
                        info.DecisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                        info.CapitalID = GetDbReaderValue<int>(reader["CapitalID"]);
                        info.CapitalName = GetDbReaderValue<string>(reader["CapitalName"]);
                        info.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        info.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        //info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        //info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        //info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]);
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        //info.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        //info.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);

                        info.BidPlanTime = GetDbReaderValue<DateTime>(reader["BidPlanTime"]);
                       // info.NegotiationTime = GetDbReaderValue<DateTime>(reader["NegotiationTime"]);
                        info.QuoteTime = GetDbReaderValue<DateTime>(reader["QuoteTime"]);
                       // info.AuditTime = GetDbReaderValue<DateTime>(reader["AuditTime"]);
                      
                        info.Address = GetDbReaderValue<string>(reader["Address"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                    }
                }
                
            }
            return info;
        }

        public int getTotalRecords(SqlConnection connection, DecisionSeachCriteria _criteria, string _userID)
        {

                using (var command = new SqlCommand("select  count(D.DecisionCode)  as TotalRecords  from (Select D.* " +

             " from tbl_Decision D where  1 = 1 and D.DateIn between @FromDate and @ToDate ", connection))
                {
                    AddSqlParameter(command, "@FromDate", _criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);

                    if (!string.IsNullOrEmpty(_criteria.DecisionCode))
                    {
                        command.CommandText += " and D.DecisionCode = @DecisionCode";
                        AddSqlParameter(command, "@DecisionCode", _criteria.DecisionCode, System.Data.SqlDbType.NVarChar);
                    }

                    command.CommandText += "  ) as D " +
                     " LEFT JOIN tbl_Quote Q on D.QuoteID  = Q.QuoteID " +
                    " LEFT JOIN tbl_BidPlan BP on BP.QuoteID  = Q.QuoteID " +

                   " Left join tbl_Quote_Customer QC on Q.QuoteID = QC.QuoteID  and QC.IsChoosed = 1 " +
                   " LEFT join tbl_Customer C on C.CustomerID = QC.CustomerID " +

                    " where  1 = 1 ";

                    if (!string.IsNullOrEmpty(_criteria.QuoteCode))
                    {
                        command.CommandText += " and Q.QuoteCode like '%" + _criteria.QuoteCode + "%'";

                    }
                    if (!string.IsNullOrEmpty(_criteria.DecisionCode))
                    {
                        command.CommandText += " and D.DecisionCode like '%" + _criteria.DecisionCode + "%'";

                    }
                    if (_criteria.CustomerID !=0)
                    {
                        command.CommandText += " and C.CustomerID = @CustomerID";
                        AddSqlParameter(command, "@CustomerID", _criteria.CustomerID, System.Data.SqlDbType.Int);
                    }
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (D.UserI = @UserID or D.UserU = @UserID or D.UserAssign = @UserID )";
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
            
         
            return 0;
        }

        public int InsertDecision(SqlConnection connection, DecisionInfo _Decision, string _userI)
        {
            var currenttime = DateTime.Now.Date;
            int lastestInserted = 0;
            if (_Decision.DecisionCode == null || _Decision.DecisionCode == "") _Decision.DecisionCode = DateTime.Now.ToString("yyMMddHHmmssfff");
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Decision] (DecisionCode, QuoteID,Comment,   CapitalID  ,  UserI, DateIn, BidMethod)" +
                    "VALUES(@DecisionCode, @QuoteID, @Comment, @CapitalID,  @UserI, @DateIn, @BidMethod) " +
                    "select IDENT_CURRENT('dbo.tbl_Decision') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@DecisionCode", _Decision.DecisionCode/*"QD-"+ _Decision.ProposalCode*/, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@QuoteID", _Decision.QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidMethod", _Decision.BidMethod, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CapitalID", _Decision.CapitalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Comment", _Decision.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DateIn", _Decision.DateIn, System.Data.SqlDbType.DateTime);
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
                    "set DecisionID=@DecisionID  , DecisionTime=@DecisionTime ,  CurrentFeature=@CurrentFeature where QuoteID=@QuoteID", connection))
                {
                    AddSqlParameter(command, "@QuoteID", _Decision.QuoteID, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@DecisionID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@DecisionTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "Decision", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }

            return lastestInserted;
        }

        public void UpdateDecision(SqlConnection connection, int _id, DecisionInfo _Decision, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Decision " +
                            " SET  DecisionCode = @DecisionCode , Comment = @Comment , CapitalID=@CapitalID,BidMethod =@BidMethod  " +
                            ", UserU=@UserU,UpdateTime=getdate(), DateIn = @DateIn  " +
                            " WHERE (DecisionID = @DecisionID) ", connection))
               // " Insert into tbl_Decision_Log ([DecisionID],[DecisionName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime])  (select [DecisionID],[DecisionName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime] from tbl_Decision where DecisionID=@DecisionID ) "
            {
                AddSqlParameter(command, "@DecisionID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DecisionCode", _Decision.DecisionCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@QuoteID", _Decision.QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidMethod", _Decision.BidMethod, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CapitalID", _Decision.CapitalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Comment", _Decision.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DateIn", _Decision.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void Delete (SqlConnection connection,int _DecisionID)
        {
            using (var command = new SqlCommand(" Delete tbl_Decision where DecisionID=@DecisionID ", connection))
            {
                AddSqlParameter(command, "@DecisionID", _DecisionID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set DecisionID = Null where DecisionID = @DecisionID  ", connection))
            {
                AddSqlParameter(command, "@DecisionID", _DecisionID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string _DecisionIDs)
        {
            using (var command = new SqlCommand(" Delete tbl_Decision where DecisionID in (" + _DecisionIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set DecisionID = Null where DecisionID in (" + _DecisionIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public List<ItemInfo> GetDecisionItems(SqlConnection connection)
        {
            var result = new List<ItemInfo>();
            using (var command = new SqlCommand(" ", connection))
            {
                //using (var reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        var info = new ItemInfo();

                //        result.Add(info);
                //    }
                //}
                return result;
            }
        }

        public List<string> getListDecisionCode(SqlConnection connection, string decisionCode,string _userID)
        {
            var result = new List<string>();
            using (var command = new SqlCommand(" Select TOP 10 DecisionCode from " +
                "tbl_Decision D where D.DecisionCode like '%" + decisionCode + "%'", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (D.UserI = @UserID or D.UserU = @UserID or D.UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _decisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                        result.Add(_decisionCode);
                    }
                }
                return result;
            }
        }

        public List<string> GetDecisionByQuoteIds(SqlConnection connection, string quoteids)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select DecisionID as ID from tbl_Proposal_Process  where QuoteID in (" + quoteids + ")", connection))
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

        public List<string> GetDecisionByNegotiationIds(SqlConnection connection, string NegotiationIds)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select DecisionID as ID from tbl_Proposal_Process  where NegotiationID in (" + NegotiationIds + ")", connection))
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

        public List<string> GetQuoteByDecisionIds(SqlConnection connection, string decisionIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select QuoteID as ID from tbl_Proposal_Process  where DecisionID in (" + decisionIDs + ")", connection))
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
