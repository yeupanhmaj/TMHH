using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class QuoteDataLayer : BaseLayerData<QuoteDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<QuoteInfo></returns>
        /// 
       
        public List<QuoteInfo> GetListQuote(SqlConnection connection, QuoteSeachCriteria _criteria,string _userID)
        {
            var result = new List<QuoteInfo>();


            string Condition = "";
            if (_criteria.pageSize == 0) _criteria.pageSize = 10;
            var offSet = _criteria.pageIndex * _criteria.pageSize;

           
            if (_criteria.ProposalCode != "" && _criteria.ProposalCode != null)
            {
                Condition += " and tblP.ProposalCode like  '%" + _criteria.ProposalCode + "%' ";
            }
            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                Condition += " and tblQ.QuoteCode  like '%" + _criteria.QuoteCode + "%'";
            }
            if (_criteria.CustomerID != 0)
            {
                Condition += " and tblQC.CustomerID =" + _criteria.CustomerID + " ";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                Condition += " and ( tblQ.UserAssign = " + _userID + " ) or ( tblQ.UserI = " + _userID + " )";
            }
            string query = $"  select tblQ.Intime, tblQ.QuoteCode , tblQ.QuoteID ,   tblP.ProposalCode  ,tblQ.IsVAT, tblQ.VATNumber , tblQC.CustomerID " +
            " from(select * from tbl_Quote where tbl_Quote.QuoteID in (select QuoteID from(select DISTINCT  tblQ.QuoteID, tblQ.UpdateTime" +
            " from tbl_Quote tblQ inner join tbl_Quote_Proposal tblQP on tblQ.QuoteID = tblQP.QuoteID " +
            " inner join tbl_Proposal tblP on tblQP.ProposalID = tblP.ProposalID " +
            " inner join tbl_Quote_Customer tblQC on tblQ.QuoteID = tblQC.QuoteID and IsChoosed = 1 " +
            " where tblQ.InTime between @FromDate and @ToDate " + Condition + 
            "  order by tblQ.UpdateTime OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY" +
            " ) tbltemp)) tblQ " +
            " inner join tbl_Quote_Proposal tblQP on tblQ.QuoteID = tblQP.QuoteID " +
            " inner join tbl_Proposal tblP on tblQP.ProposalID = tblP.ProposalID " +
            " inner join tbl_Quote_Customer tblQC on tblQ.QuoteID = tblQC.QuoteID and IsChoosed = 1 order by tblQ.UpdateTime ";


            using (var command = new SqlCommand( query, connection))
            {
                AddSqlParameter(command, "@FromDate", _criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", _criteria.pageSize, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                var info = new QuoteInfo();
                int tempQuoteID = 0;
                bool isNeedAdd = false;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (tempQuoteID != GetDbReaderValue<int>(reader["QuoteID"])) {
                            if (info.QuoteID != 0)
                            {
                                isNeedAdd = false;
                                result.Add(info);
                            }
                            info = new QuoteInfo();
                            info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                            info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                            info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                            info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                            info.proposalCodes = GetDbReaderValue<string>(reader["ProposalCode"]);
                            info.InTime = GetDbReaderValue<DateTime>(reader["InTime"]);
                            tempQuoteID = info.QuoteID;
                            isNeedAdd = true;
                        }
                        else
                        {

                            string proCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                            if (info.proposalCodes.Contains(proCode) == false) {
                                info.proposalCodes += " , " + proCode;
                            }
                            tempQuoteID = info.QuoteID;

                        }
                    }
                }
                if (isNeedAdd)
                {
                    result.Add(info);
                }
                return result;
            }
        }

        public QuoteInfo getQuote(SqlConnection connection, int QuoteID,string _userID)
        {
            QuoteInfo result = new QuoteInfo();

            //get Quote details

            using (var command = new SqlCommand("select tblQ.QuoteID," +
                " tblQ.QuoteCode , tblQ.IsVAT , tblQ.VATNumber," +
                " tblQ.Intime from tbl_Quote tblQ  " +
                "  where tblQ.QuoteID = @QuoteID ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( tblQ.UserAssign = " + _userID + " ) or ( tblQ.UserI = " + _userID + " )";

                }
                AddSqlParameter(command, "@QuoteID", QuoteID, SqlDbType.Int);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        result.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        result.InTime = GetDbReaderValue<DateTime>(reader["Intime"]);
                        result.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        result.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                    }
                }

            }
            //get proposal details vs item

            using (var command = new SqlCommand(
                "  select * from 	" +
                "  tbl_Quote_Proposal tblQP " +
                "  inner Join " +
                "  tbl_Proposal tblP" +
                "  on tblQP.ProposalID = tblP.ProposalID  " +
                "  inner join " +
                "  tbl_department tblD on tblD.DepartmentID = tblP.DepartmentID " +
                "  where tblQP.QuoteID = @QuoteID  "
                , connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, SqlDbType.Int);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        QuoteSimpleProposalInfo proInfo = new QuoteSimpleProposalInfo();
                        proInfo.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        proInfo.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        proInfo.ProposalId = GetDbReaderValue<int>(reader["ProposalID"]);
                        proInfo.inTtime = GetDbReaderValue<DateTime>(reader["InTime"]);
                        result.lstProposal.Add(proInfo);
                    }
                }

            }

            for (int i = 0; i < result.lstProposal.Count; i++)
            {
                result.lstProposal[i].items = ProposalDataLayer.GetInstance().GetPropsalItems(connection, result.lstProposal[i].ProposalId);
            }

            //get customer ID details vs item

            using (var command = new SqlCommand(
              "  select * from 	" +
              "  tbl_Quote_Customer tblQC " +
              "  inner Join " +
              "  tbl_Customer tblC" +
              "  on tblQC.CustomerID = tblC.CustomerID  " +
              "  where tblQC.QuoteID = @QuoteID  "
              , connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        QuotCustomerInfo info = new QuotCustomerInfo();
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        info.CustomerCode = GetDbReaderValue<string>(reader["CustomerCode"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.IsChoosed = GetDbReaderValue<bool>(reader["IsChoosed"]);
                        result.Quotes.Add(info);
                    }
                }
            }

            return result;
        }

        public QuoteInfo GetQuoteByCode(SqlConnection connection, string code,string _userID)
        {
            QuoteInfo result = null;
            using (var command = new SqlCommand(" " +
                "select Q.*, C.* , P.ProposalCode, C.CustomerName, C.Address, C.Phone, C.Email from (Select Q.* " +
                " from tbl_Quote Q where  Q.QuoteCode = @QuoteCode) as Q " +
                " left join tbl_Quote_Customer QC on QC.QuoteID = Q.QuoteID and QC.IsChoosed = 1 " +
                " left join tbl_Quote_Proposal QP on QP.QuoteID = Q.QuoteID " +
                " left join tbl_Proposal P on QP.ProposalID = P.ProposalID " +
                " LEFT JOIN tbl_Customer C on C.CustomerID  = QC.CustomerID ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( Q.UserAssign = " + _userID + " ) or ( Q.UserI = " + _userID + " )";

                }
                AddSqlParameter(command, "@QuoteCode", code, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new QuoteInfo();
                        result.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        result.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);

                        // result.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        result.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        result.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        result.Address = GetDbReaderValue<string>(reader["Address"]);
                        result.Phone = GetDbReaderValue<string>(reader["Phone"]);
                        result.Email = GetDbReaderValue<string>(reader["Email"]);
                        result.Cost = GetDbReaderValue<double>(reader["Cost"]);
                        result.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        result.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        result.TotalCost = GetDbReaderValue<double>(reader["TotalCost"]);
                        result.Comment = GetDbReaderValue<string>(reader["Comment"]);
                        //   result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);

                        result.DeliveryTime = GetDbReaderValue<int>(reader["DeliveryTime"]);
                        result.DeliveryLocation = GetDbReaderValue<string>(reader["DeliveryLocation"]);
                        result.DeliveryTermAndCondition = GetDbReaderValue<string>(reader["DeliveryTermAndCondition"]);
                        result.WarrantyTermAndCondition = GetDbReaderValue<string>(reader["WarrantyTermAndCondition"]);
                        result.ValidTime = GetDbReaderValue<int>(reader["ValidTime"]);
                        result.AccountantCode = GetDbReaderValue<string>(reader["AccountantCode"]);
                        result.BankNumber = GetDbReaderValue<string>(reader["BankNumber"]);
                        result.BankName = GetDbReaderValue<string>(reader["BankName"]);
                        result.Surrogate = GetDbReaderValue<string>(reader["Surrogate"]);
                        result.Position = GetDbReaderValue<string>(reader["Position"]);
                        result.QuoteType = GetDbReaderValue<int>(reader["QuoteType"]);
                    }
                }
                return result;
            }
        }



        public int getTotalRecords(SqlConnection connection, QuoteSeachCriteria _criteria,string _userID)
        {
            int result = 0;
            string Condition = "";
            if (_criteria.ProposalCode != "" && _criteria.ProposalCode != null)
            {
                Condition += " and tblP.ProposalCode like  '%" + _criteria.ProposalCode + "%' ";
            }
            if (_criteria.QuoteCode != null && _criteria.QuoteCode != "")
            {
                Condition += " and tblQ.QuoteCode  like '%" + _criteria.QuoteCode + "%'";
            }
            if (_criteria.CustomerID != 0)
            {
                Condition += " and tblQC.CustomerID =" + _criteria.CustomerID + " ";
            }
            if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
            {
                Condition += " and ( tblQ.UserAssign = " + _userID + " ) or ( tblQ.UserI = " + _userID + " )";
            }
            string query = $"select count(QuoteID) as TotalRecords from(select DISTINCT  tblQ.QuoteID, tblQ.UpdateTime" +
            " from tbl_Quote tblQ inner join tbl_Quote_Proposal tblQP on tblQ.QuoteID = tblQP.QuoteID " +
            " inner join tbl_Proposal tblP on tblQP.ProposalID = tblP.ProposalID " +
            " inner join tbl_Quote_Customer tblQC on tblQ.QuoteID = tblQC.QuoteID and IsChoosed = 1 " +
            " where tblQ.InTime between @FromDate and @ToDate " + Condition + 
            " ) tbltemp";

            using (var command = new SqlCommand(query, connection))
            {
                AddSqlParameter(command, "@FromDate", _criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);


                command.CommandText += " order by tblQ.UpdateTime Desc";
                WriteLogExecutingCommand(command);
                var info = new QuoteInfo();
                int tempQuoteID = 0;
                bool isNeedAdd = false;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = GetDbReaderValue<int>(reader["TotalRecords"]);
                    }
                }

                return result;
            }
        }


        public List<ItemInfo> GetQuoteItems(SqlConnection connection, int quoteID)
        {
            List<ItemInfo> ret = new List<ItemInfo>();

            using (var command = new SqlCommand("select QuoteItemID, tqi.ItemID, tqi.Amount, tqi.Description, tqi.WarrantyYears " +
                " , tqi.ItemPrice , tqi.TotalPrice, i.ItemName , i.ItemCode  ,   i.Unit " +

                " from tbl_Quote_Item tqi " +
                " inner join  tbl_items as i on tqi.ItemID = i.ItemID " +
                " where QuoteID= @QuoteID", connection))
            {
                AddSqlParameter(command, "@QuoteID", quoteID, System.Data.SqlDbType.Int);

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new ItemInfo();
                        item.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        item.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.ItemUnit = GetDbReaderValue<string>(reader["Unit"]);

                        item.Description = GetDbReaderValue<string>(reader["Description"]);
 
                        item.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        item.TotalPrice = GetDbReaderValue<double>(reader["TotalPrice"]);

                        item.WarrantyYears = GetDbReaderValue<int>(reader["WarrantyYears"]);
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }

        public void UpdateQuote(SqlConnection connection, int _id, QuoteInfo _Quote, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Quote \n" +
                            " SET   ProposalID = @ProposalID ," +
                            "CustomerID = @CustomerID ," +
                            "Cost = @Cost ," +
                            "IsVAT = @IsVAT ," +
                            "VATNumber = @VATNumber ," +
                            "TotalCost = @TotalCost , " +
                            "Comment = @Comment, " +

                            "UserU=@UserU," +
                            "DeliveryTime=@DeliveryTime , " +
                            "DeliveryLocation=@DeliveryLocation , " +
                            "DeliveryTermAndCondition=@DeliveryTermAndCondition, " +
                            "WarrantyTermAndCondition=@WarrantyTermAndCondition , " +
                            "ValidTime=@ValidTime , " +
                            "QuoteType=@QuoteType , " +
                            "UpdateTime=getdate() \n" +
                            " WHERE (QuoteID = @QuoteID) ", connection))
            {
                AddSqlParameter(command, "@QuoteID", _id, System.Data.SqlDbType.Int);
                //  AddSqlParameter(command, "@QuoteCode", _Quote.QuoteCode, System.Data.SqlDbType.NVarChar);

                AddSqlParameter(command, "@CustomerID", _Quote.CustomerID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Cost", _Quote.Cost, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@IsVAT", _Quote.IsVAT, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@VATNumber", _Quote.VATNumber, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@TotalCost", _Quote.TotalCost, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@Comment", _Quote.Comment, System.Data.SqlDbType.NVarChar);
                //     AddSqlParameter(command, "@DateIn", _Quote.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@DeliveryTime", _Quote.DeliveryTime, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DeliveryLocation", _Quote.DeliveryLocation, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DeliveryTermAndCondition", _Quote.WarrantyTermAndCondition, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@WarrantyTermAndCondition", _Quote.WarrantyTermAndCondition, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ValidTime", _Quote.ValidTime, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@QuoteType", _Quote.QuoteType, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void Delete(SqlConnection connection, int _QuoteID)
        {
            using (var command = new SqlCommand(" Delete tbl_Quote where QuoteID=@QuoteID ; " +
                " delete tbl_Quote_Proposal where QuoteID=@QuoteID ; " +
                " delete tbl_Quote_Customer where QuoteID=@QuoteID ; " +
                " delete tbl_Quote_Item where QuoteID=@QuoteID ; ", connection))
            {
                AddSqlParameter(command, "@QuoteID", _QuoteID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            using (var command = new SqlCommand(" Update tbl_Proposal_Process set QuoteID = Null where QuoteID = @QuoteID  ", connection))
            {
                AddSqlParameter(command, "@QuoteID", _QuoteID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

        }

        public void DeleteMuti(SqlConnection connection, string _QuoteIDs)
        {
            using (var command = new SqlCommand(" Delete tbl_Quote where QuoteID in (" + _QuoteIDs + ")   ; " +
                " delete tbl_Quote_Proposal  where QuoteID in (" + _QuoteIDs + ")   ; " +
                " delete tbl_Quote_Customer  where QuoteID in (" + _QuoteIDs + ")   ; " +
                " delete tbl_Quote_Item  where QuoteID in (" + _QuoteIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            using (var command = new SqlCommand(" Update tbl_Proposal_Process set QuoteID = Null where QuoteID in (" + _QuoteIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public List<SelectItem> getListQuoteCode(SqlConnection connection, string quoteCode , bool isContract)
        {
            var result = new List<SelectItem>();

            string query = " Select TOP 10 QuoteCode ,  QuoteID from tbl_Quote where QuoteCode like '%" + quoteCode + "%' ";

            if (isContract)
            {
                query = " Select TOP 10 QuoteCode ,  tbl_Quote.QuoteID from tbl_Quote  " +
                    "inner join tbl_Negotiation on tbl_Quote.QuoteID  = tbl_Negotiation.QuoteID" +
                    " where QuoteCode like '%" + quoteCode + "%' ";

            }

            using (var command = new SqlCommand(
                query
                , connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SelectItem temp = new SelectItem();
                        temp.Label = GetDbReaderValue<string>(reader["QuoteCode"]);
                        temp.Value = ( GetDbReaderValue<int>(reader["QuoteID"])).ToString();
                        result.Add(temp);
                    }
                }
                return result;
            }
        }
        public void DeleteQuoteItemsByQuotesID(SqlConnection connection, string quoteIds)
        {
            using (var command = new SqlCommand(" delete tbl_Quote_Item where QuoteID in (" + quoteIds + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


        public void InsertQuoteProposal(SqlConnection connection, int quoteID, int proposalID)
        {
            var currenttime = DateTime.Now.Date;
            string strAddNewCode = " Insert into [dbo].[tbl_Quote_Proposal] (ProposalID, QuoteID ) " +
                " VALUES(@ProposalID, @QuoteID  ) ";
            using (var command = new SqlCommand(strAddNewCode))
            {
                AddSqlParameter(command, "@proposalID", proposalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@quoteID", quoteID, System.Data.SqlDbType.Int);
                db.ExcuteScalar(connection, command);
            }

            using (var command = new SqlCommand("update tbl_Proposal_Process " +
                   "set QuoteID=@QuoteID , QuoteTime=@QuoteTime where ProposalID=@ProposalID", connection))
            {
                AddSqlParameter(command, "@QuoteID", quoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ProposalID", proposalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@QuoteTime", currenttime, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@CurrentFeature", "Quote", System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }


        }
        public void InsertQuoteCustomer(SqlConnection connection, int quoteID, int CustomerID, bool IsChoosed)
        {
            string strAddNewCode = " Insert into [dbo].[tbl_Quote_Customer] ( QuoteID , CustomerID , IsChoosed) " +
               " VALUES( @QuoteID , @CustomerID , @IsChoosed) ";
            using (var command = new SqlCommand(strAddNewCode))
            {
                AddSqlParameter(command, "@QuoteID", quoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerID", CustomerID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@IsChoosed", IsChoosed, System.Data.SqlDbType.Bit);
                db.ExcuteScalar(connection, command);
            }
        }
        public int InsertQuoteWithOutDetails(SqlConnection connection, string _userI)
        {

            int lastestInserted = 0;
            string QuoteCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Quote] (QuoteCode, UserI  , isVAT , VATNUmber)" +
                    "VALUES(@QuoteCode ,  @UserI  , 1 , 10) " +
                    "select IDENT_CURRENT('dbo.tbl_Quote') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@QuoteCode", QuoteCode , System.Data.SqlDbType.VarChar);
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
        public List<int> getExitsQuoteProposal(SqlConnection connection, int[] proposalIDs, int[] customerIDs, string _userI)
        {
            string query = "select QuoteID "
                + " as ID from tbl_Quote_Proposal where  ";
            List<int> result = new List<int>();
            int index = 0;
            foreach (int customerID in customerIDs)
            {
                foreach (int proposalId in proposalIDs)
                {
                    if (index == 0) {
                        query += " (ProposalID = " + proposalId + " )";
                    }
                    else
                    {
                        query += " or (ProposalID = " + proposalId + " )";
                    }

                    index++;
                }
            }

            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(GetDbReaderValue<int>(reader["ID"]));
                    }
                }
            }
            return result;
        }


        public void insertQuoteItem(SqlConnection connection, string itemName, string itemDescription, string itemUnit
                                   , string itemPrice, string amount, string itemTotalPrice, int QuoteID, int CustomerID)
        {
            string query = " insert into tbl_Quote_Item   (QuoteID, CustomerID, ItemName, Description ,ItemUnit ,ItemPrice , Amount , TotalPrice  )  " +
                    " VALUES(@QuoteID, @CustomerID, @ItemName, @Description , @ItemUnit , @ItemPrice , @Amount , @TotalPrice) ";


            using (var command = new SqlCommand(query, connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerID", CustomerID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ItemName", itemName, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Description", itemDescription, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@ItemUnit", itemUnit, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@ItemPrice", Double.Parse(itemPrice), System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@Amount", Double.Parse(amount), System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@TotalPrice", Double.Parse(itemTotalPrice), System.Data.SqlDbType.Float);
                db.ExcuteScalar(connection, command);
            }
        }


        public void deleteQuoteItem(SqlConnection connection, int QuoteID, int CustomerID)
        {
            string query = " Delete from tbl_Quote_Item where QuoteID = @QuoteID and CustomerID = @CustomerID ";


            using (var command = new SqlCommand(query, connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerID", CustomerID, System.Data.SqlDbType.Int);
                db.ExcuteScalar(connection, command);
            }

        }



        public QuoteAuditInfo GetChoosedQuoteDetails(SqlConnection connection, int QuoteID )
        {
            QuoteAuditInfo ret = new QuoteAuditInfo();

            using (var command = new SqlCommand("" +
                "select tblQ.QuoteID, tblQ.QuoteCode, tblQ.IsVAT, tblQ.VATNumber, tblC.CustomerID , tblC.CustomerName , tblQ.Intime from( " +
                   "  select * from tbl_Quote_Customer " +
                   "  where QuoteID = QuoteID " +
                   "  and IsChoosed = 1 " +
                   "  ) as tblQC " +
                 "    inner join tbl_Quote tblQ " +
                   "  on tblQC.QuoteID = tblQ.QuoteID " +
                 "    inner join tbl_Customer tblC " +
                 "    on tblQC.CustomerID = tblC.CustomerID "
                , connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        ret.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        ret.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        ret.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        ret.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        ret.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        ret.InTime = GetDbReaderValue<DateTime>(reader["InTime"]);
                    }
                }
            }
            return ret;
        }


        public List<String> GetListProsalCode(SqlConnection connection, int QuoteID)
        {
            List<String> ret = new List<String>();

            using (var command = new SqlCommand(
               " select tblP.ProposalCode  " +
               "    from( " +
               "    select * from tbl_Quote_Proposal  " +
               "             where tbl_Quote_Proposal.QuoteID = @QuoteID  " +
               "    ) tblQP  " +
               "    inner join  tbl_Proposal tblP  " +
               "    on tblQP.ProposalID = tblP.ProposalID  " 
                , connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(GetDbReaderValue<string>(reader["ProposalCode"]));
                    }
                }
            }
            return ret;
        }


        public List<ItemInfo> GetQuoteItems(SqlConnection connection, int quoteID , int CustomerID)
        {
            List<ItemInfo> ret = new List<ItemInfo>();
            using (var command = new SqlCommand("select * from [tbl_Quote_Item]  " +
                " where QuoteID= @QuoteID  and CustomerID=@CustomerID ", connection))
            {
                AddSqlParameter(command, "@QuoteID", quoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerID", CustomerID, System.Data.SqlDbType.Int);


                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new ItemInfo();
                        item.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        item.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.ItemUnit = GetDbReaderValue<string>(reader["ItemUnit"]);
                        item.Description = GetDbReaderValue<string>(reader["Description"]);
                        item.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        item.WarrantyYears = GetDbReaderValue<int>(reader["WarrantyYears"]);
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        item.TotalPrice = item.ItemPrice * item.Amount;
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }
        public QuoteInfo getQuoteByCode(SqlConnection connection, string QuoteCode,string _userID)
        {
            QuoteInfo result = new QuoteInfo();

            //get Quote details

            using (var command = new SqlCommand("select tblQ.QuoteID," +
                " tblQ.QuoteCode , tblQ.IsVAT , tblQ.VATNumber," +
                " tblQ.Intime from tbl_Quote tblQ  " +
                "  where tblQ.QuoteCode = @QuoteCode ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( tblQ.UserAssign = " + _userID + " ) or ( tblQ.UserI = " + _userID + " )";

                }
                AddSqlParameter(command, "@QuoteCode", QuoteCode, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                        result.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        result.InTime = GetDbReaderValue<DateTime>(reader["Intime"]);
                        result.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        result.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                    }
                }

            }
            //get proposal details vs item

            using (var command = new SqlCommand(
                "  select * from 	" +
                "  tbl_Quote_Proposal tblQP " +
                "  inner Join " +
                "  tbl_Proposal tblP" +
                "  on tblQP.ProposalID = tblP.ProposalID  " +
                 "  inner Join " +
                "  tbl_Quote tblQ" +
                "  on tblQ.QuoteID = tblQP.QuoteID  " +
                "  inner join " +
                "  tbl_department tblD on tblD.DepartmentID = tblP.DepartmentID " +
                "  where tblQ.QuoteCode = @QuoteCode  "
                , connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( tblQ.UserAssign = " + _userID + " ) or ( tblQ.UserI = " + _userID + " )";

                }
                AddSqlParameter(command, "@QuoteCode", QuoteCode, SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        QuoteSimpleProposalInfo proInfo = new QuoteSimpleProposalInfo();
                        proInfo.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        proInfo.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        proInfo.ProposalId = GetDbReaderValue<int>(reader["ProposalID"]);
                        proInfo.inTtime = GetDbReaderValue<DateTime>(reader["InTime"]);
                        result.lstProposal.Add(proInfo);
                    }
                }

            }

            for (int i = 0; i < result.lstProposal.Count; i++)
            {
                result.lstProposal[i].items = ProposalDataLayer.GetInstance().GetPropsalItems(connection, result.lstProposal[i].ProposalId);
            }

            //get customer ID details vs item

            using (var command = new SqlCommand(
              "  select * from 	" +
              "  tbl_Quote_Customer tblQC " +
              "  inner Join " +
              "  tbl_Customer tblC" +
              "  on tblQC.CustomerID = tblC.CustomerID  " +
              "  inner Join " +
              "  tbl_Quote tblQ" +
              "  on tblQ.QuoteID = tblQC.QuoteID  " +
              "  where tblQ.QuoteCode = @QuoteCode  "
              , connection))

            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( tblQ.UserAssign = " + _userID + " ) or ( tblQ.UserI = " + _userID + " )";

                }
                AddSqlParameter(command, "@QuoteCode", QuoteCode, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        QuotCustomerInfo info = new QuotCustomerInfo();
                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        info.CustomerCode = GetDbReaderValue<string>(reader["CustomerCode"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        info.IsChoosed = GetDbReaderValue<bool>(reader["IsChoosed"]);
                        result.Quotes.Add(info);
                    }
                }
            }

            return result;
        }

        public SelectedQuoteInfo getQuoteInfo(SqlConnection connection, int quoteID,string _userID)
        {
            SelectedQuoteInfo ret = new SelectedQuoteInfo();
            using (var command = new SqlCommand("select tblC.CustomerID, tblC.CustomerName , " +
            "       tblQ.IsVAT , tblQ.VATNumber, BP.BidExpirated, Bp.BidExpiratedUnit, Bp.BidType, Bp.BidMethod  from ( " + 
            "      select * from tbl_Quote_Customer "+
            "               where QuoteID = @QuoteID " +
            "      and IsChoosed = 1 " +
            "      ) tblQC " +
            "      inner join tbl_Customer  tblC " +
            "      on tblC.CustomerID = tblQC.CustomerID " +
            "      inner join tbl_Quote tblQ " +
            "      on tblQC.QuoteID = tblQ.QuoteID" +
            " left join tbl_BidPlan BP on tblQC.QuoteID = BP.QuoteID ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and ( tblQ.UserAssign = " + _userID + " ) or ( tblQ.UserI = " + _userID + " )";

                }
                AddSqlParameter(command, "@QuoteID", quoteID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        ret.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        ret.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        ret.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        ret.BidType = GetDbReaderValue<string>(reader["BidType"]);
                        ret.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
                        ret.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);
                        ret.BidMethod = GetDbReaderValue<int>(reader["BidMethod"]);
                    }
                }
            }
            return ret;
        }

        public List<ItemInfo> getSelectedItemsQuote(SqlConnection connection, int quoteID)
        {
            List<ItemInfo> ret = new List<ItemInfo>();
            using (var command = new SqlCommand(
            "     select * from( " +
            "      select * from tbl_Quote_Customer " +
            "      where QuoteID = @QuoteID " +
            "      and IsChoosed = 1 " +
            "      ) tblQC " +
            "      inner join[tbl_Quote_Item] tblQI " +
            "    on tblQI.CustomerID = tblQC.CustomerID " +
            "    and tblQI.QuoteID = tblQC.QuoteID "
                , connection))
            {
                AddSqlParameter(command, "@QuoteID", quoteID, System.Data.SqlDbType.Int);

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new ItemInfo();
                        item.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        item.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.ItemUnit = GetDbReaderValue<string>(reader["ItemUnit"]);
                        item.Description = GetDbReaderValue<string>(reader["Description"]);
                        item.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        item.WarrantyYears = GetDbReaderValue<int>(reader["WarrantyYears"]);
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        item.TotalPrice = item.ItemPrice * item.Amount;
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }
        public void updateQuoteNew(SqlConnection connection , int QuoteID, QuoteUpdateRequest data)
        {
            int isVat = 0;
            string query = " update tbl_Quote set ISVAT=@ISVAT , VATNumber=@VATNumber , QuoteCode = @QuoteCode  where QuoteID = @QuoteID ";
            if (data.IsVAT) isVat = 1;

            using (var command = new SqlCommand(query, connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ISVAT", isVat, System.Data.SqlDbType.Bit);

                AddSqlParameter(command, "@VATNumber", data.VATNumber, System.Data.SqlDbType.Float);

                AddSqlParameter(command, "@QuoteCode", data.QuoteCode, System.Data.SqlDbType.Text);

                db.ExcuteScalar(connection, command);
            }

        }


        public void chooseQuote(SqlConnection connection, int QuoteID, int CustomerID)
        {
  
            string query = "update tbl_Quote_Customer set IsChoosed=0  where QuoteID = @QuoteID " +
                "update tbl_Quote_Customer set IsChoosed=1  where QuoteID = @QuoteID  and CustomerID = @CustomerID ;";      
            using (var command = new SqlCommand(query, connection))
            {
                AddSqlParameter(command, "@QuoteID", QuoteID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerID", CustomerID, System.Data.SqlDbType.Int);
                db.ExcuteScalar(connection, command);
            }

        }


        public List<searchQuoteRespone> searchQuoteCanCreateAudit(SqlConnection connection, string text)
        {
            List<searchQuoteRespone> ret = new List<searchQuoteRespone>();
            using (var command = new SqlCommand("" +
                "select tblQ.QuoteID, tblQ.QuoteCode,  tblC.CustomerName, tblP.ProposalCode   ,tblQI.ItemName " +
               "  from tbl_Quote tblQ " +
               "   inner join ( select QuoteID  FROM [tbl_Proposal_Process] where QuoteID is not null and AuditID is null) tblProcess " +
               "   on tblQ.QuoteID = tblProcess.QuoteID" + 
               "   inner join tbl_Quote_Customer tblQC " +
               "   on tblQ.QuoteID = tblQC.QuoteID and tblQC.IsChoosed = 1 " +
               "   inner join tbl_Quote_Proposal tblQP " +
               "   on tblQ.QuoteID = tblQP.QuoteID " +
               "   inner join tbl_Proposal tblP " +
               "   on tblP.ProposalID = tblQP.ProposalID " +
               "   inner join tbl_Customer tblC " +
               "   on tblC.CustomerID = tblQC.CustomerID " +
               "   inner join tbl_Department tblD " +
               "   on tblD.DepartmentID = tblP.DepartmentID " +
               "   inner join tbl_Quote_Item tblQI " +
               "   on(" +
               "   tblQI.CustomerID = tblQC.CustomerID and " +
               "   tblQI.QuoteID = tblQC.QuoteID and " +
               "   tblQC.IsChoosed = 1 " +
               "   ) " +
               "  where tblQ.QuoteCode like '%" + text + "%' or tblQI.ItemName like '%" + text + "%' or  tblP.ProposalCode like '%" + text + "%'  or tblC.CustomerName like '%" + text + "%'"
              , connection))
            {

                var info = new searchQuoteRespone();
                int tempQuoteID = 0;
                bool isNeedAdd = false;
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (tempQuoteID != GetDbReaderValue<int>(reader["QuoteID"]))
                        {
                            if (info.QuoteID != 0)
                            {
                                isNeedAdd = false;
                                ret.Add(info);
                            }
                            info = new searchQuoteRespone();
                            info.QuoteID = GetDbReaderValue<int>(reader["QuoteID"]);
                            info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);

                            
                            info.ProposalCodes = GetDbReaderValue<string>(reader["ProposalCode"]);
                            info.CustomerNames = GetDbReaderValue<string>(reader["CustomerName"]);
                            info.ItemNames = GetDbReaderValue<string>(reader["ItemName"]);
                            tempQuoteID = info.QuoteID;
                            isNeedAdd = true;
                        }
                        else
                        {

                            string proCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                            if (info.ProposalCodes.Contains(proCode) == false)
                            {
                                info.ProposalCodes += " , " + proCode;
                            }
                            string CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                            if (info.CustomerNames.Contains(CustomerName) == false)
                            {
                                info.CustomerNames += " , " + CustomerName;
                            }
                            string ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                            if (info.ItemNames.Contains(ItemName) == false)
                            {
                                info.ItemNames += " , " + ItemName;
                            }
                            tempQuoteID = info.QuoteID;

                        }
                    }
                }
                if (isNeedAdd)
                {
                    ret.Add(info);
                }
                return ret;
            }
        }

        public List<searchProposalItemRespone> searchQuoteItem(SqlConnection connection, string text)
        {
            List<searchProposalItemRespone> ret = new List<searchProposalItemRespone>();
            
            using (var command = new SqlCommand("" +
                 " select DRI.QuoteItemID, DRI.ItemName, DRI.Amount, DR.[DeliveryReceiptID], P.ProposalCode, DRI.Amount, DRI.Amount - isnull(ava.Amount, 0) as AvaAmount " +
                " from tbl_DeliveryReceipt_Items DRI " +
                " join tbl_DeliveryReceipt DR on DR.[DeliveryReceiptID] = DRI.[DeliveryReceiptID] " +
                " join tbl_Proposal P on P.ProposalID = DR.ProposalID" +
                " left join (select A.[DeliveryReceiptID], A.QuoteItemID, count(*) as Amount from tbl_Analyzer A group by A.QuoteItemID, A.[DeliveryReceiptID]) " +
                " ava on  ava.[DeliveryReceiptID] = DR.[DeliveryReceiptID] and ava.QuoteItemID = DRI.QuoteItemID " +
                "  where DRI.Amount - isnull(ava.Amount,0) > 0" 
              , connection))
            {

                if (!string.IsNullOrEmpty(text))
                {
                    command.CommandText += " and P.ProPosalCode like '%" + text + "%' or DRI.ItemName like N'%" + text + "%'  ";
                }
                var info = new searchProposalItemRespone();
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        info = new searchProposalItemRespone();
                        info.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        info.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        info.ItemNames = GetDbReaderValue<string>(reader["ItemName"]);
                        info.ProposalCodes = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        info.AvaAmount = GetDbReaderValue<double>(reader["AvaAmount"]);
                        ret.Add(info);
                    }
                }
                return ret;
            }
        }


        public List<SelectItem> getListquoteCodeCanCreateBiplan(SqlConnection connection, string quoteCode)
        {
            var result = new List<SelectItem>();

            string query = " Select TOP 10  tblProcess.QuoteID, QuoteCode from( select DISTINCT QuoteID FROM tbl_Proposal_Process where  QuoteID is not null and BidPlanID is Null" + //and AuditID is not null -- không kiểm giá vẫn cho tạo kế hoahcj nhà cung cấp
                "    ) as tblProcess" +
                    " inner join tbl_Quote on tblProcess.QuoteID = tbl_Quote.QuoteID where QuoteCode like '%" + quoteCode + "%' ";
            using (var command = new SqlCommand(
                query
                , connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SelectItem temp = new SelectItem();
                        temp.Label = GetDbReaderValue<string>(reader["QuoteCode"]);
                        temp.Value = (GetDbReaderValue<int>(reader["QuoteID"])).ToString();
                        result.Add(temp);
                    }
                }
                return result;
            }
        }

        public List<SelectItem> getListquoteCodeCanCreateNegotiation(SqlConnection connection, string quoteCode)
        {
            var result = new List<SelectItem>();

            string query = " Select TOP 10  tblProcess.QuoteID, QuoteCode from( select DISTINCT PP.QuoteID, sum(QI.ItemPrice) as TotalPrice FROM tbl_Proposal_Process PP join tbl_Quote_Item QI on QI.QUoteID = PP.QUoteID " +
                "  where  PP.QuoteID is not null  and (BidPlanID is not Null or TotalPrice < 20000000) and NegotiationID is null " + //and AuditID is not null -- hiện tại chưa cần khóa
                "    group by PP.QuoteID) as tblProcess" +
                    " inner join tbl_Quote on tblProcess.QuoteID = tbl_Quote.QuoteID where QuoteCode like '%" + quoteCode + "%' ";
            using (var command = new SqlCommand(
                query
                , connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SelectItem temp = new SelectItem();
                        temp.Label = GetDbReaderValue<string>(reader["QuoteCode"]);
                        temp.Value = (GetDbReaderValue<int>(reader["QuoteID"])).ToString();
                        result.Add(temp);
                    }
                }
                return result;
            }
        }

        public List<SelectItem> getListquoteCodeCanCreateDecision(SqlConnection connection, string quoteCode)
        {
            var result = new List<SelectItem>();

            string query = " Select TOP 10  tblProcess.QuoteID, QuoteCode from( select DISTINCT PP.QuoteID, sum(QI.ItemPrice) as TotalPrice FROM tbl_Proposal_Process PP join tbl_Quote_Item QI on QI.QUoteID = PP.QUoteID " +
                " where  PP.QuoteID is not null  and (BidPlanID is not Null or TotalPrice < 20000000) and NegotiationID is not null and  DecisionID is null" + //and AuditID is not null
                "   group by PP.QuoteID  ) as tblProcess" +
                    " inner join tbl_Quote on tblProcess.QuoteID = tbl_Quote.QuoteID where QuoteCode like '%" + quoteCode + "%' ";
            using (var command = new SqlCommand(
                query
                , connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SelectItem temp = new SelectItem();
                        temp.Label = GetDbReaderValue<string>(reader["QuoteCode"]);
                        temp.Value = (GetDbReaderValue<int>(reader["QuoteID"])).ToString();
                        result.Add(temp);
                    }
                }
                return result;
            }
        }


        public List<SelectItem> getListquoteCodeCanCreateContract(SqlConnection connection, string quoteCode)
        {
            var result = new List<SelectItem>();

            string query = " Select TOP 10  tblProcess.QuoteID, QuoteCode from( select DISTINCT PP.QuoteID, sum(QI.ItemPrice) as TotalPrice FROM tbl_Proposal_Process PP join tbl_Quote_Item QI on QI.QUoteID = PP.QUoteID " +
                " where  PP.QuoteID is not null  and (BidPlanID is not Null or TotalPrice < 20000000) and NegotiationID is not null and  DecisionID is not null" + //and AuditID is not null
                " and ContractID is null " +
                "   group by PP.QuoteID ) as tblProcess" +
                    " inner join tbl_Quote on tblProcess.QuoteID = tbl_Quote.QuoteID where QuoteCode like '%" + quoteCode + "%' ";
            using (var command = new SqlCommand(
                query
                , connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SelectItem temp = new SelectItem();
                        temp.Label = GetDbReaderValue<string>(reader["QuoteCode"]);
                        temp.Value = (GetDbReaderValue<int>(reader["QuoteID"])).ToString();
                        result.Add(temp);
                    }
                }
                return result;
            }
        }
    }

}

