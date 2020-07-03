using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;
using AdminPortal.Helpers;

namespace AdminPortal.DataLayer
{
    public class NegotiationDataLayer : BaseLayerData<NegotiationDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<NegotiationInfo></returns>
        /// 
     
        public List<NegotiationInfo> getNegotiation(SqlConnection connection, NegotiationSeachCriteria _criteria,string _userID)
        {
            var result = new List<NegotiationInfo>();
            var query = "SELECT tblN.* , tblQ.QuoteCode , tbl_Customer.CustomerName from  tbl_Negotiation tblN" +
               "  inner join tbl_Quote tblQ " +
               "  on tblN.QuoteID = tblQ.QuoteID" +
                "  inner join tbl_Customer" +
                "  on tblN.CustomerID = tbl_Customer.CustomerID  " +
                " where tblN.DateIn between @FromDate and @ToDate ";
            if (_criteria.NegotiationCode != null && _criteria.NegotiationCode != "")
            {
                query += " and tblN.NegotiationCode like '%" + _criteria.NegotiationCode + "%'";
            }
            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                query += " and tblQ.QuoteCode  like '%" + _criteria.QuoteCode + "%'";
            }
            if (_criteria.CustomerID != 0)
            {
                query += " and tbl_Customer.CustomerID =" + _criteria.CustomerID + " ";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                query += " and ( tblN.UserAssign = " + _userID+ " ) or ( tblN.UserI = " + _userID + " )";              
            }
            query += " order by tblN.UpdateTime Desc ";


            using (var command = new SqlCommand(query
               , connection))
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
                        var info = new NegotiationInfo();
                        info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        info.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        info.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        info.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        info.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);

                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);

                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.Term = GetDbReaderValue<int>(reader["Term"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int getTotalRecords(SqlConnection connection, NegotiationSeachCriteria _criteria,string _userID)
        {
            int result = 0;
            var query = "SELECT  count (tblN.NegotiationID) as totalRecords from  tbl_Negotiation tblN" +
              "  inner join tbl_Quote tblQ " +
              "  on tblN.QuoteID = tblQ.QuoteID" +
               "  inner join tbl_Customer" +
               "  on tblN.CustomerID = tbl_Customer.CustomerID  " +
               " where tblN.DateIn between @FromDate and @ToDate ";
            if (_criteria.NegotiationCode != null && _criteria.NegotiationCode != "")
            {
                query += " and tblN.NegotiationCode like '%" + _criteria.NegotiationCode + "%'";
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
                query += " and ( tblN.UserAssign = " + _userID + " ) or ( tblN.UserI = " + _userID + " )";
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
                        result = GetDbReaderValue<int>(reader["totalRecords"]);                     
                    }
                }
                return result;
            }
        }  

        public NegotiationInfo getNegotiation(SqlConnection connection, int _ID,string _userID)
        {
            NegotiationInfo info = null;
            using (var command = new SqlCommand("" +
                 "SELECT tblN.* , tbl_Quote.QuoteCode , tbl_Customer.CustomerName,  " +
                 "  tbl_Quote.ISVAT, tbl_Quote.VATNumber  from  tbl_Negotiation tblN" +
               "  inner join tbl_Quote " +
               "  on tblN.QuoteID = tbl_Quote.QuoteID" +
                "  inner join tbl_Customer" +
                "  on tblN.CustomerID = tbl_Customer.CustomerID  " +
                " where tblN.NegotiationID = @NegotiationID ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( tblN.UserAssign = @UserID ) or ( tblN.UserI = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@NegotiationID", _ID, SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new NegotiationInfo();
                        info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        info.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);

                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);

                        info.Location = GetDbReaderValue<string>(reader["Location"]);
                        info.Phone = GetDbReaderValue<string>(reader["Phone"]);
                        info.Fax = GetDbReaderValue<string>(reader["Fax"]);
                        info.BankID = GetDbReaderValue<string>(reader["BankID"]);
                        info.TaxCode = GetDbReaderValue<string>(reader["TaxCode"]);
                        info.Represent = GetDbReaderValue<string>(reader["Represent"]);
                        info.Position = GetDbReaderValue<string>(reader["Position"]);
                        info.contractType = GetDbReaderValue<string>(reader["contractType"]);
                        info.contractTypeExpired = GetDbReaderValue<int>(reader["contractTypeExpired"]);
                        info.contractTypeExpiredUnit = GetDbReaderValue<string>(reader["contractTypeExpiredUnit"]);
                        info.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        info.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        info.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);



                        info.IsVAT = GetDbReaderValue<bool>(reader["ISVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);

                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.Term = GetDbReaderValue<int>(reader["Term"]);
                    }
                }
                return info;
            }
        }

        public NegotiationInfo GetNegotiationByCode(SqlConnection connection, string code)
        {
            NegotiationInfo info = null;
            using (var command = new SqlCommand("" +
                 "SELECT tblN.* , tbl_Quote.QuoteCode , tbl_Customer.CustomerName  from  tbl_Negotiation tblN" +
               "  inner join tbl_Quote " +
               "  on tblN.QuoteID = tbl_Quote.QuoteID" +
                "  inner join tbl_Customer" +
                "  on tblN.CustomerID = tbl_Customer.CustomerID  " +
                " where tblN.NegotiationCode = @NegotiationCode ", connection))
            {
                AddSqlParameter(command, "@NegotiationID", code, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new NegotiationInfo();
                        info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        info.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        info.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        info.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        info.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);

                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);



                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.Term = GetDbReaderValue<int>(reader["Term"]);
                    }
                }
                return info;
            }
        }


        public List<NegotiationLstcb> GetListNegotiationByCode(SqlConnection connection, string code)
        {
            List<NegotiationLstcb> ret =  new List<NegotiationLstcb>() ;
            NegotiationLstcb info = new NegotiationLstcb();
            using (var command = new SqlCommand("" +
                 "SELECT tblN.* "+
              
                " where tblN.NegotiationCode like '%" + code + "%'", connection))
            {
              
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new NegotiationLstcb();
                        info.NegotiationID = GetDbReaderValue<int>(reader["NegotiationID"]);
                        info.NegotiationCode = GetDbReaderValue<string>(reader["NegotiationCode"]);
                        ret.Add(info);
                    }
                }
                return ret;
            }
        }

        public int InsertNegotiation(SqlConnection connection, NegotiationInfo _Negotiation, string _userI)
        {
            int lastestInserted = 0;
            var currenttime = DateTime.Now.Date;
            if (_Negotiation.NegotiationCode == null) _Negotiation.NegotiationCode = "TT-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            using (var command = new SqlCommand("Insert into [dbo].[tbl_Negotiation]" +
                " (NegotiationCode, QuoteID , Location, Phone, Fax, BankID, TaxCode, Represent, Position,UserI, DateIn, Term , CustomerID ,BidExpirated, BidExpiratedUnit, BidType)" +//, CapitalID
                    "VALUES( @NegotiationCode, @QuoteID , @Location, @Phone, @Fax, @BankID, @TaxCode, @Represent, @Position, @UserI, @DateIn, @Term, @CustomerID ,@BidExpirated, @BidExpiratedUnit, @BidType) " + //, @CapitalID
                    "select IDENT_CURRENT('dbo.tbl_Negotiation') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@NegotiationCode", _Negotiation.NegotiationCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@QuoteID", _Negotiation.QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerID", _Negotiation.CustomerID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Location", _Negotiation.Location, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Phone", _Negotiation.Phone, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Fax", _Negotiation.Fax, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@BankID", _Negotiation.BankID, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@TaxCode", _Negotiation.TaxCode, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Represent", _Negotiation.Represent, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Position", _Negotiation.Position, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@DateIn", _Negotiation.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Term", _Negotiation.Term, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidExpirated", _Negotiation.BidExpirated, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidExpiratedUnit", _Negotiation.BidExpiratedUnit, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidType", _Negotiation.BidType, System.Data.SqlDbType.NText);


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
                    "set NegotiationID=@NegotiationID  , NegotiationTime=@NegotiationTime ,  CurrentFeature=@CurrentFeature where QuoteID=@QuoteID", connection))
                {
                    AddSqlParameter(command, "@QuoteID", _Negotiation.QuoteID, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@NegotiationID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@NegotiationTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "Negotiation", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }
            return lastestInserted;
        }

        public void UpdateNegotiation(SqlConnection connection, int _id, NegotiationInfo _Negotiation, string _userI)
        {
            using (var command = new SqlCommand("update  [dbo].[tbl_Negotiation]" +
                "  set " +
                "  NegotiationCode = @NegotiationCode " +
                " , QuoteID = @QuoteID" +
                " , Location=@Location " +
                " , Phone=@Phone " +
                " , Fax=@Fax " +
                " , BankID=@BankID " +
                " , TaxCode=@TaxCode " +
                " , Represent=@Represent " +
                " , Position=@Position " +
                " , UserI=@UserI " +
                " , DateIn =@DateIn " +
                " , Term  =@Term " +
                " , CustomerID = @CustomerID  " +
                " , BidExpirated = @BidExpirated  " +
                " , BidExpiratedUnit = @BidExpiratedUnit  " +
                " , BidType = @BidType  " +
                " where NegotiationID = " + _id.ToString()// " , CapitalID = @CapitalID  " +
                   , connection))
            {
                AddSqlParameter(command, "@NegotiationCode", _Negotiation.NegotiationCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@QuoteID", _Negotiation.QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerID", _Negotiation.CustomerID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Location", _Negotiation.Location, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Phone", _Negotiation.Phone, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Fax", _Negotiation.Fax, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@BankID", _Negotiation.BankID, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@TaxCode", _Negotiation.TaxCode, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Represent", _Negotiation.Represent, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Position", _Negotiation.Position, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@DateIn", _Negotiation.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Term", _Negotiation.Term, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidExpirated", _Negotiation.BidExpirated, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@BidExpiratedUnit", _Negotiation.BidExpiratedUnit, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@BidType", _Negotiation.BidType, System.Data.SqlDbType.NText);
                command.ExecuteScalar();              
            }
   
        }

        public void Delete(SqlConnection connection,int _NegotiationID)
        {
            using (var command = new SqlCommand(" Delete tbl_Negotiation where NegotiationID=@NegotiationID ", connection))
            {
                AddSqlParameter(command, "@NegotiationID", _NegotiationID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set NegotiationID = Null where NegotiationID = @NegotiationID  ", connection))
            {
                AddSqlParameter(command, "@NegotiationID", _NegotiationID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string _NegotiationIDs)
        {
            using (var command = new SqlCommand(" Delete tbl_Negotiation where NegotiationID in (" + _NegotiationIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand(" Update tbl_Proposal_Process set NegotiationID = Null where NegotiationID in (" + _NegotiationIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


        public List<string> GetNegotiationByBidPlanIds(SqlConnection connection, string bidPlanIds)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select NegotiationID as ID from tbl_Proposal_Process  where BidPlanID in (" + bidPlanIds + ")", connection))
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


        public int getNegotiationIDbyQuoteID(SqlConnection connection, int quoteID,string _userID)
        {
            int ret = 0;
  
            using (var command = new SqlCommand("" +
                 "SELECT tblN.NegotiationID from tbl_Negotiation tblN " +
                " where tblN.quoteID  = " + quoteID , connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( tblN.UserAssign = @UserID ) or ( tblN.UserI = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret = GetDbReaderValue<int>(reader["NegotiationID"]);
                    }
                }
                return ret;
            }
        }

        public List<string> GetNegotiationByQuoteIds(SqlConnection connection, string quoteIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select NegotiationID as ID from tbl_Proposal_Process  where QuoteID in (" + quoteIDs + ")", connection))
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

        public List<string> GetQuoteByNegotiationIds(SqlConnection connection, string negotiationIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select QuoteID as ID from tbl_Proposal_Process  where NegotiationID in (" + negotiationIDs + ")", connection))
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
