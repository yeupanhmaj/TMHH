using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class ContractDataLayer : BaseLayerData<ContractDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// </summary>
        /// <returns>Return List<ContractInfo></returns>
        /// 
      
        public List<ContractInfo> getContract(SqlConnection connection, ContractSeachCriteria _criteria,string _userID)
        {
            var result = new List<ContractInfo>();

            string query = "select tblC.DateIn, tblC.ContractID , tblC.ContractCode, tblQ.QuoteCode , tbl_Customer.CustomerName from tbl_Contract tblC  "  +
                 " inner join tbl_Quote tblQ " +
                 " on tblC.QuoteID = tblQ.QuoteID "  +
                 " inner join tbl_Quote_Customer tblQC " +
                 " on tblQC.QuoteID = tblQ.QuoteID  and tblQC.IsChoosed = 1 " +
                "  inner join tbl_Customer " +
                "  on tblQC.CustomerID = tbl_Customer.CustomerID " +
                " where tblC.DateIn between @FromDate and @ToDate ";
            if(_criteria.ContractCode !=null && _criteria.ContractCode != "")
            {
                query += " and tblC.ContractCode like '%" + _criteria.ContractCode + "%'";
            }
            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                query += " and tblQ.QuoteCode like '%" + _criteria.QuoteCode + "%'";
            }
            if (_criteria.CustomerID != 0)
            {
                query += " and tbl_Customer.CustomerID =" + _criteria.CustomerID + " ";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                query += " and (tblC.UserI = " + _userID + "or tblC.UserAssign = " + _userID + " )";
            }

            query += " order by tblC.UpdateTime Desc ";
            using (var command = new SqlCommand(query, connection))
            {
                AddSqlParameter(command, "@FromDate", _criteria.FromDate, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate, System.Data.SqlDbType.DateTime);
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
                        var info = new ContractInfo();
                        info.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);                                 
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public ContractInfo getContract(SqlConnection connection, int _ID,string _userID)
        {
            ContractInfo info = null;
            using (var command = new SqlCommand(" Select C.* ," +
                " D.DecisionCode, N.NegotiationID, N.NegotiationCode," +
                " BP.BidPlanID, BP.BidPlanCode," +
                " A.AuditID, A.AuditCode," +
                " Q.QuoteID, Q.QuoteCode ," +
                " Q.IsVAT ,  Q.VATNumber ," +
                " N.BSide , N.BLocation , N.BPhone , N.BFax , N.BBankID , N.BTaxCode , N.BSide ,   N.BRepresent , N.BPosition , " +
                " N.ASide , N.ALocation , N.APhone , N.AFax , N.ABankID , N.ATaxCode , N.ASide ,   N.ARepresent , N.APosition ," +
                " N.InTime as NegotiationTime , Q.TotalCost as  QuoteTotalCost, D.InTime as DecisionTime , " +
                " P.ProposalCode, DP.DepartmentName  " +
                "from (Select C.* " +
                " from tbl_Contract C where  C.ContractID = @ContractID) as C " +
                " LEFT JOIN tbl_Decision D on C.DecisionID  = D.DecisionID " +
                " LEFT JOIN tbl_Negotiation N on N.NegotiationID  = D.NegotiationID " +
                " LEFT JOIN tbl_BidPlan BP on BP.BidPlanID  = N.BidPlanID " +
                " LEFT JOIN tbl_Audit A on A.AuditID  = BP.AuditID " +
                " LEFT JOIN tbl_Quote Q on A.QuoteID  = Q.QuoteID " +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = D.ProposalID " +
                " LEFT JOIN tbl_Department DP on DP.DepartmentID  = P.DepartmentID  where  1 = 1", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (C.UserI = " + _userID + "or C.UserAssign = " + _userID + " )";
                }
                AddSqlParameter(command, "@ContractID", _ID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new ContractInfo();
                        info.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.Code = GetDbReaderValue<string>(reader["Code"]);
                        info.DecisionID = GetDbReaderValue<int>(reader["DecisionID"]);
                        info.DecisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                        info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        info.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        info.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        info.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        info.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        info.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.DateIn = GetDbReaderValue<DateTime?>(reader["DateIn"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);



                        info.ASide = GetDbReaderValue<string>(reader["ASide"]);
                        info.ALocation = GetDbReaderValue<string>(reader["ALocation"]);
                        info.APhone = GetDbReaderValue<string>(reader["APhone"]);
                        info.AFax = GetDbReaderValue<string>(reader["AFax"]);
                        info.ABankID = GetDbReaderValue<string>(reader["ABankID"]);
                        info.ATaxCode = GetDbReaderValue<string>(reader["ATaxCode"]);
                        info.ARepresent = GetDbReaderValue<string>(reader["ARepresent"]);
                        info.APosition = GetDbReaderValue<string>(reader["APosition"]);
                        info.BSide = GetDbReaderValue<string>(reader["BSide"]);
                        info.BLocation = GetDbReaderValue<string>(reader["BLocation"]);
                        info.BPhone = GetDbReaderValue<string>(reader["BPhone"]);
                        info.BFax = GetDbReaderValue<string>(reader["BFax"]);
                        info.BBankID = GetDbReaderValue<string>(reader["BBankID"]);
                        info.BTaxCode = GetDbReaderValue<string>(reader["BTaxCode"]);
                        info.BRepresent = GetDbReaderValue<string>(reader["BRepresent"]);
                        info.BPosition = GetDbReaderValue<string>(reader["BPosition"]);
                        info.NegotiationTime = GetDbReaderValue<DateTime>(reader["NegotiationTime"]);
                        info.DecisionTime = GetDbReaderValue<DateTime>(reader["DecisionTime"]);
                        info.QuoteTotalCost = GetDbReaderValue<double>(reader["QuoteTotalCost"]);
                    }
                }
                return info;
            }
        }

        public NewContractInfo getContractNew(SqlConnection connection, int _ID)
        {
            NewContractInfo info = null;
            using (var command = new SqlCommand(" Select * from tbl_Contract " +
                " where ContractID = @ContractID "  , connection))
            {
                AddSqlParameter(command, "@ContractID", _ID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new NewContractInfo();
                        info.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                    }
                }
                return info;
            }
        }
        public int getTotalRecords(SqlConnection connection, ContractSeachCriteria _criteria,string _userID)
        {
            string query = "select count(*) as  TotalRecords from tbl_Contract tblC  " +
                 " inner join tbl_Quote tblQ " +
                 " on tblC.QuoteID = tblQ.QuoteID " +
                 " inner join tbl_Quote_Customer tblQC " +
                 " on tblQC.QuoteID = tblQ.QuoteID  and tblQC.IsChoosed = 1 " +
                "  inner join tbl_Customer " +
                "  on tblQC.CustomerID = tbl_Customer.CustomerID " +
                " where tblC.DateIn between @FromDate and @ToDate ";
            if (_criteria.ContractCode != null && _criteria.ContractCode != "")
            {
                query += " and tblC.ContractCode like '%" + _criteria.ContractCode + "%'";
            }
            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                query += " and tblQ.QuoteCode like '%" + _criteria.QuoteCode + "%'";
            }
            if (_criteria.CustomerID != null && _criteria.CustomerID != 0)
            {
                query += " and  tbl_Customer.CustomerID = " + _criteria.CustomerID + " ";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                query += " and (tblC.UserI = " + _userID + "or tblC.UserAssign = " + _userID + " )";
            }

            using (var command = new SqlCommand(query, connection))
            {
                AddSqlParameter(command, "@FromDate", _criteria.FromDate, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate, System.Data.SqlDbType.DateTime);

              
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

        public int InsertContract(SqlConnection connection, ContractInfo _Contract, string _userI)
        {
            int lastestInserted = 0;
            var currenttime = DateTime.Now.Date;
            if (_Contract.ContractCode == null) _Contract.ContractCode = "HD-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            using (var command = new SqlCommand("Insert into [dbo].[tbl_Contract] ( DecisionID, NegotiationID,  ContractCode, QuoteID ,Comment, UserI, DateIn)" +
                    "VALUES(@DecisionID,@NegotiationID, @ContractCode, @QuoteID, @Comment, @UserI, @DateIn) " +
                    "select IDENT_CURRENT('dbo.tbl_Contract') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@DecisionID", _Contract.DecisionID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@NegotiationID", _Contract.NegotiationID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ContractCode", _Contract.ContractCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@QuoteID", _Contract.QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Comment", _Contract.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DateIn", _Contract.DateIn, System.Data.SqlDbType.DateTime);
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
                    "set ContractID=@ContractID  ,ContractTime=@ContractTime ,  CurrentFeature=@CurrentFeature where QuoteID=@QuoteID", connection))
                {
                    AddSqlParameter(command, "@QuoteID", _Contract.QuoteID, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@ContractID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@ContractTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "Contract", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }

            return lastestInserted;
        }

        public void UpdateContract(SqlConnection connection, int _id, ContractInfo _Contract, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Contract " +
                            " SET ContractCode = @ContractCode , Comment = @Comment "+
                            ", UserU=@UserU,UpdateTime=getdate(), DateIn = @DateIn " +
                            " WHERE (ContractID = @ContractID) ", connection))
               // " Insert into tbl_Contract_Log ([ContractID],[ContractName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime])  (select [ContractID],[ContractName],[ProposalID],[Comment],[UserI],[Intime],[UserU],[UpdateTime] from tbl_Contract where ContractID=@ContractID ) "
            {

                AddSqlParameter(command, "@ContractID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ContractCode", _Contract.ContractCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Comment", _Contract.Comment, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DateIn", _Contract.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void Delete(SqlConnection connection,int _ContractID)
        {
            using (var command = new SqlCommand(" Delete tbl_Contract where ContractID=@ContractID ", connection))
            {
                AddSqlParameter(command, "@ContractID", _ContractID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set ContractID = Null where ContractID = @ContractID  ", connection))
            {
                AddSqlParameter(command, "@ContractID", _ContractID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string _ContractIDs)
        {
            using (var command = new SqlCommand(" Delete tbl_Contract where ContractID in (" + _ContractIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set ContractID = Null where ContractID in (" + _ContractIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

      

        public List<string> getListContractCode(SqlConnection connection, string contractCode)
        {
            var result = new List<string>();
            using (var command = new SqlCommand(" Select TOP 10 ContractCode from tbl_Contract where ContractCode like '%" + contractCode + "%'", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _contractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        result.Add(_contractCode);
                    }
                }
                return result;
            }
        }
        public ContractInfo GeContractByCode(SqlConnection connection, string _code, string _userID)
        {
            ContractInfo info = null;
            using (var command = new SqlCommand(" Select C.* , D.DecisionCode, N.NegotiationID, N.NegotiationCode, " +
                " BP.BidPlanID, BP.BidPlanCode, A.AuditID, A.AuditCode, Q.QuoteID, Q.QuoteCode,  Q.IsVAT ,  " +
                " Q.VATNumber,  P.ProposalCode, DP.DepartmentName  from (Select C.* " +
                " from tbl_Contract C where  C.ContractCode = @ContractCode) as C " +
                " LEFT JOIN tbl_Decision D on C.DecisionID  = D.DecisionID " +
                " LEFT JOIN tbl_Negotiation N on N.NegotiationID  = D.NegotiationID " +
                " LEFT JOIN tbl_BidPlan BP on BP.BidPlanID  = N.BidPlanID " +
                " LEFT JOIN tbl_Audit A on A.AuditID  = BP.AuditID " +
                " LEFT JOIN tbl_Quote Q on A.QuoteID  = Q.QuoteID " +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = D.ProposalID " +
                " LEFT JOIN tbl_Department DP on DP.DepartmentID  = P.DepartmentID  where  1 = 1", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (C.UserI = @UserID or C.UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@ContractCode", _code, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new ContractInfo();
                        info.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.DecisionID = GetDbReaderValue<int>(reader["DecisionID"]);
                        info.DecisionCode = GetDbReaderValue<string>(reader["DecisionCode"]);
                        info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        info.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        info.BidPlanID = GetDbReaderValue<int>(reader["BidPlanID"]);
                        info.BidPlanCode = GetDbReaderValue<string>(reader["BidPlanCode"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        info.AuditID = GetDbReaderValue<int>(reader["AuditID"]);
                        info.AuditCode = GetDbReaderValue<string>(reader["AuditCode"]);
                        info.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        info.DateIn = GetDbReaderValue<DateTime?>(reader["DateIn"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return info;
            }
        }

        public List<string> GetContractByDecisionids(SqlConnection connection, string decisionids)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select ContractID as ID from tbl_Proposal_Process where DecisionID in (" + decisionids + ")", connection))
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
        public List<ContractSelectItem> GetContractSelectItem(SqlConnection connection, string code,string _userID)
        {
            List<ContractSelectItem> ret   = new List<ContractSelectItem>();
            using (var command = new SqlCommand(" Select * from tbl_Contract " +
                " where ContractCode like  '%'"+ code + "%' ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (UserI = @UserID or UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ContractSelectItem info = new ContractSelectItem();
                        info.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        ret.Add(info);
                    }
                }
                return ret;
            }
        }

        public List<string> GetContractByQuoteIds(SqlConnection connection, string quoteids,string _userID)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select ContractID as ID from tbl_Proposal_Process  where QuoteID in (" + quoteids + ")", connection))
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
        public List<string> GetQuoteByContractIds(SqlConnection connection, string contractIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select QuoteID as ID from tbl_Proposal_Process  where ContractID in (" + contractIDs + ")", connection))
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
