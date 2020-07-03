using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class DeliveryReceiptDataLayer : BaseLayerData<DeliveryReceiptDataLayer>
    {
        DataProvider db = new DataProvider();

        public List<DeliveryReceiptInfo> Getlist(SqlConnection connection, DeliveryReceiptCriteria criteria,string _userID)
        {
            var result = new List<DeliveryReceiptInfo>();
            using (var command = new SqlCommand("Select DR.*, P.ProposalCode, D.DepartmentName , D1.DepartmentName as CurDepartmentName" +
                " from tbl_DeliveryReceipt DR  " +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = DR.ProposalID " +
                " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
                " left join tbl_Department D1 on P.CurDepartmentID = D1.DepartmentID " +
                " where   DR.ProposalID <> 0 ", connection))
            {

                if(criteria.proposalCode != "" && criteria.proposalCode!= null)
                {
                    command.CommandText += " and P.ProposalCode like  '%" + criteria.proposalCode + "%' ";
                }
                if (criteria.departmentID !=0)
                {
                    command.CommandText += " and ( P.departmentID = @departmentID ";
                    command.CommandText += " or  P.CurDepartmentID = @departmentID ) ";
                    AddSqlParameter(command, "@departmentID", criteria.departmentID, System.Data.SqlDbType.Int);
                }
                if (criteria.fromDate != null  && criteria.toDate != null)
                {
                    command.CommandText += " and DR.DateIn between @FromDate and @ToDate ";
                    AddSqlParameter(command, "@FromDate", criteria.fromDate, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@ToDate", criteria.toDate, System.Data.SqlDbType.DateTime);
                }
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (DR.UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }

                if (criteria.pageSize == 0) criteria.pageSize = 10;
                var offSet = criteria.pageIndex * criteria.pageSize;
                command.CommandText += " order by DR.UpdateTime ";
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", criteria.pageSize, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new DeliveryReceiptInfo(); 
                        info.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        info.DeliveryReceiptType = GetDbReaderValue<int>(reader["DeliveryReceiptType"]);
                        info.DeliveryReceiptCode = GetDbReaderValue<string>(reader["DeliveryReceiptCode"]);
                        info.DeliveryReceiptDate = GetDbReaderValue<DateTime>(reader["DeliveryReceiptDate"]);
                        info.DeliveryReceiptNumber = GetDbReaderValue<int>(reader["DeliveryReceiptNumber"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        info.DeliveryReceiptPlace = GetDbReaderValue<string>(reader["DeliveryReceiptPlace"]);
                   
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.CreateTime = GetDbReaderValue<DateTime>(reader["CreateTime"]);                   
                        result.Add(info);
                    }
                }
                return result;
            }
        }


        public List<DeliveryReceiptItemInfoNew> getSelectedItems(SqlConnection connection, int id,string _userID)
        {
            List<DeliveryReceiptItemInfoNew> ret = new List<DeliveryReceiptItemInfoNew>();
            using (var command = new SqlCommand(
              "select * from [tbl_DeliveryReceipt_Items] where DeliveryReceiptID = @id"
                , connection))
            {
                AddSqlParameter(command, "@id", id, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new DeliveryReceiptItemInfoNew();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        item.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.ItemUnit = GetDbReaderValue<string>(reader["ItemUnit"]);
                        item.Description = GetDbReaderValue<string>(reader["Description"]);
                        item.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);                      
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        item.TotalPrice = item.ItemPrice * item.Amount;
                        item.AcceptanceResult = GetDbReaderValue<bool>(reader["AcceptanceResult"]);
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }


        public List<DeliveryReceiptItemInfoNew> getItemsByQuoteIDs(SqlConnection connection, string id)
        {
            List<DeliveryReceiptItemInfoNew> ret = new List<DeliveryReceiptItemInfoNew>();
            using (var command = new SqlCommand(
              "select * from [tbl_DeliveryReceipt_Items] where QuoteItemID in  " + id
                , connection))
            {
                
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new DeliveryReceiptItemInfoNew();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        item.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.ItemUnit = GetDbReaderValue<string>(reader["ItemUnit"]);
                        item.Description = GetDbReaderValue<string>(reader["Description"]);
                        item.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        item.TotalPrice = item.ItemPrice * item.Amount;
                        item.AcceptanceResult = GetDbReaderValue<bool>(reader["AcceptanceResult"]);
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }



        public List<DeliveryReceiptItemInfoNew> getDetailsAcceptanceByProposalID(SqlConnection connection, int id)
        {
            List<DeliveryReceiptItemInfoNew> ret = new List<DeliveryReceiptItemInfoNew>();
            using (var command = new SqlCommand(
              "  select * from [tbl_DeliveryReceipt_Items] where  DeliveryReceiptID in ( "+
              "    select DeliveryReceiptID from tbl_DeliveryReceipt where ProposalID = " +  id + 
              "    ) "
                , connection))
            {

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new DeliveryReceiptItemInfoNew();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        item.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.ItemUnit = GetDbReaderValue<string>(reader["ItemUnit"]);
                        item.Description = GetDbReaderValue<string>(reader["Description"]);
                        item.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        item.TotalPrice = item.ItemPrice * item.Amount;
                        item.AcceptanceResult = GetDbReaderValue<bool>(reader["AcceptanceResult"]);
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }
        public void CreateDeliveryReceiptItem(SqlConnection connection,
            DeliveryReceiptItemInfoNew itemObj, string userID)
        {
            /* DateTime localDate = DateTime.Now;

             string strAddNewCode = "Insert into tbl_DeliveryReceipt_Items " +
                 " (ItemID, ItemCode,ItemManufactureCountry,ItemUnit, Amount, Price," +
                 " TotalPrice , ShipCost ,TestCost ,ItemDocument , IsSub ,DeliveryReceiptID  ,UserU , CreateTime , DeliveryNote , ManufactureYear , StartUseYear)" +
                     "VALUES  (@ItemID, @ItemCode, @ItemManufactureCountry, @ItemUnit, @Amount, @Price, " +
                 " @TotalPrice , @ShipCost , @TestCost , @ItemDocument , @IsSub , @DeliveryReceiptID  , @UserU , @CreateTime , @DeliveryNote , @ManufactureYear , @StartUseYear)";

             using (var command = new SqlCommand(strAddNewCode))
             {
                 AddSqlParameter(command, "@ItemID", itemObj.ItemID, System.Data.SqlDbType.Int);
                 AddSqlParameter(command, "@ItemCode", itemObj.ItemCode, System.Data.SqlDbType.VarChar);
                 AddSqlParameter(command, "@ItemManufactureCountry", itemObj.ItemManufactureCountry, System.Data.SqlDbType.NVarChar);
                 AddSqlParameter(command, "@DeliveryNote", itemObj.DeliveryNote, System.Data.SqlDbType.NVarChar);
                 AddSqlParameter(command, "@ItemUnit", itemObj.ItemUnit, System.Data.SqlDbType.NVarChar);
                 AddSqlParameter(command, "@Amount", itemObj.Amount, System.Data.SqlDbType.Float);
                 AddSqlParameter(command, "@Price", itemObj.Price, System.Data.SqlDbType.Float);
                 AddSqlParameter(command, "@TotalPrice", itemObj.TotalPrice, System.Data.SqlDbType.Float);
                 AddSqlParameter(command, "@ShipCost", itemObj.ShipCost, System.Data.SqlDbType.Float);
                 AddSqlParameter(command, "@TestCost", itemObj.TestCost, System.Data.SqlDbType.Float);
                 AddSqlParameter(command, "@ItemDocument", itemObj.ItemDocument, System.Data.SqlDbType.NVarChar);
                 AddSqlParameter(command, "@IsSub", itemObj.IsSub, System.Data.SqlDbType.Bit);
                 AddSqlParameter(command, "@DeliveryReceiptID", itemObj.DeliveryReceiptID, System.Data.SqlDbType.Int);
                 AddSqlParameter(command, "@UserU", itemObj.UserU, System.Data.SqlDbType.VarChar);
                 AddSqlParameter(command, "@CreateTime", localDate, System.Data.SqlDbType.DateTime);
                 AddSqlParameter(command, "@ManufactureYear", itemObj.ManufactureYear, System.Data.SqlDbType.Int);
                 AddSqlParameter(command, "@StartUseYear", itemObj.StartUseYear, System.Data.SqlDbType.Int);
                 db.ExcuteScalar(connection, command);
             }*/
            string query = " insert into tbl_DeliveryReceipt_Items   " +
                "(DeliveryReceiptID , QuoteItemID, ItemName, Description ,ItemUnit ,ItemPrice , Amount , TotalPrice )  " +
                  " VALUES(@DeliveryReceiptID , @QuoteItemID, @ItemName, @Description , @ItemUnit , @ItemPrice , @Amount , @TotalPrice) ";


            using (var command = new SqlCommand(query, connection))
            {
                AddSqlParameter(command, "@QuoteItemID", itemObj.QuoteItemID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DeliveryReceiptID", itemObj.DeliveryReceiptID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ItemName", itemObj.ItemName, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@Description", itemObj.Description, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@ItemUnit", itemObj.ItemUnit, System.Data.SqlDbType.NText);
                AddSqlParameter(command, "@ItemPrice",itemObj.ItemPrice, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@Amount",itemObj.Amount, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@TotalPrice", itemObj.TotalPrice, System.Data.SqlDbType.Float);
                db.ExcuteScalar(connection, command);
            }
        }
        public void UpdateDeliveryReceiptItem(SqlConnection connection, DeliveryReceiptItemInfoNew itemObj, string userID)
        {
           /* string strAddNewCode =
                "UPDATE tbl_DeliveryReceipt_Items " +
                " SET  ItemID = @ItemID, ItemCode = @ItemCode , ItemManufactureCountry = @ItemManufactureCountry, " +
                " ItemUnit = @ItemUnit, Amount = @Amount, Price = @Price, " +
                " TotalPrice = @TotalPrice, ShipCost = @ShipCost, TestCost = @TestCost, " +
                " ItemDocument = @ItemDocument, IsSub = @IsSub , UserU=@UserU , DeliveryNote=@DeliveryNote , ManufactureYear=@ManufactureYear , StartUseYear=@StartUseYear " + 
                " WHERE AutoID = @AutoID";

            using (var command = new SqlCommand(strAddNewCode))
            {
                AddSqlParameter(command, "@ItemID", itemObj.ItemID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ItemCode", itemObj.ItemCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ItemUnit", itemObj.ItemUnit, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Amount", itemObj.Amount, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@Price", itemObj.ItemPrice, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@TotalPrice", itemObj.TotalPrice, System.Data.SqlDbType.Float);
               
                AddSqlParameter(command, "@DeliveryReceiptID", itemObj.DeliveryReceiptID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserU", itemObj.UserU, System.Data.SqlDbType.VarChar);
              
                AddSqlParameter(command, "@AutoID", itemObj.AutoID, System.Data.SqlDbType.VarChar);
                db.ExcuteScalar(connection, command);
            }*/
        }
        public void CreateDeliveryReceiptItemUser(SqlConnection connection, DeliveryReceiptEmployeeInfo itemObj, string userID)
        {
            DateTime localDate = DateTime.Now;
            string strAddNewCode = "Insert into tbl_DeliveryReceiptEmployee " +
                " (EmployeeID, Role , DeliveryReceiptID )" +
                    "VALUES  (@EmployeeID, @Role, @DeliveryReceiptID )";
            using (var command = new SqlCommand(strAddNewCode))
            {
                AddSqlParameter(command, "@EmployeeID", itemObj.EmployeeID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Role", itemObj.Role, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DeliveryReceiptID", itemObj.DeliveryReceiptID, System.Data.SqlDbType.Int);

   
                db.ExcuteScalar(connection, command);
            }
        }

        public void UpdateDeliveryReceiptItemUser(SqlConnection connection, DeliveryReceiptEmployeeInfo itemObj, string userID)
        {
            string strAddNewCode = "UPDATE tbl_DeliveryReceiptEmployee " +
                           " SET  EmployeeID = @EmployeeID, Role = @Role , UserU = @UserU " +
                           " WHERE AutoID = @AutoID";
            using (var command = new SqlCommand(strAddNewCode)) 
            { 
                AddSqlParameter(command, "@EmployeeID", itemObj.EmployeeID, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@Role", itemObj.Role, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserU", userID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@AutoID", itemObj.AutoID, System.Data.SqlDbType.Int);
                db.ExcuteScalar(connection, command);
            }
        }
        public void DeleteDeliveryReceiptItemUser(SqlConnection connection,  int deliveryReceiptID, string userID)
        {
            string strAddNewCode = "delete tbl_DeliveryReceiptEmployee where DeliveryReceiptID = @DeliveryReceiptID ";
            using (var command = new SqlCommand(strAddNewCode))
            {
                
                AddSqlParameter(command, "@DeliveryReceiptID", deliveryReceiptID, System.Data.SqlDbType.Int);
              
      
                db.ExcuteScalar(connection, command);
            }
        }
        public List<DeliveryReceiptItemInfoNew> GetItems(SqlConnection connection, int id)
        {
            List<DeliveryReceiptItemInfoNew> ret = new List<DeliveryReceiptItemInfoNew>();
            using (var command = new SqlCommand(@"select tdi.* , i.ItemName ,   tdi.ItemUnit, ISNULL(tdi.ItemCode, i.ItemCode) as ItemCode    from
                (select * from tbl_DeliveryReceipt_Items   where DeliveryReceiptID = @DeliveryReceiptID) tdi
                inner join  tbl_items as i on tdi.ItemID = i.ItemID
                ", connection))
            {
                AddSqlParameter(command, "@DeliveryReceiptID", id, System.Data.SqlDbType.Int);

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new DeliveryReceiptItemInfoNew();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.ItemID = GetDbReaderValue<int>(reader["ItemID"]);
                        item.ItemCode = GetDbReaderValue<string>(reader["ItemCode"]);
                        item.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        item.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                       item.ItemUnit = GetDbReaderValue<string>(reader["ItemUnit"]);
                        item.ItemName = GetDbReaderValue<string>(reader["ItemName"]);
                        item.ItemPrice = GetDbReaderValue<double>(reader["Price"]);
                        item.TotalPrice = GetDbReaderValue<double>(reader["TotalPrice"]);
                      
                        ret.Add(item);
                    }
                }
            }
            return ret;
        }
        public int GetTotalRecords(SqlConnection connection, DeliveryReceiptCriteria criteria,string _userID)
        {
          
                using (var command = new SqlCommand("Select count(temp.DeliveryReceiptID) as TotalRecords from " +
                    " (Select DR.*, P.ProposalCode, D.DepartmentName " +
               " from tbl_DeliveryReceipt DR  " +
               " LEFT JOIN tbl_Proposal P on P.ProposalID  = DR.ProposalID " +
               " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
               " where  DR.DeliveryReceiptID <> 0   ", connection))
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
                        command.CommandText += " and (DR.UserAssign = @UserID )";
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

            return 0;
        }

        public DeliveryReceiptInfo GetDetail(SqlConnection connection, int id,string _userID)
        {
            DeliveryReceiptInfo info = new DeliveryReceiptInfo();
            using (var command = new SqlCommand("Select DR.*, Q.IsVAT, Q.QuoteCode, Q.VATNumber, " +
                " D1.DepartmentCode as CurDepartmentCode , D.DepartmentCode,   c.ContractCode ," +
                " P.ProposalCode, D1.DepartmentName as CurDepartmentName, D.DepartmentName ," +
                " P.DateIn as ProposalTime  " +
                " from tbl_DeliveryReceipt DR  " +
                " LEFT JOIN tbl_Quote_Proposal tblQP on tblQP.ProposalID  = DR.ProposalID " +
                " LEFT JOIN tbl_Quote Q  on Q.QuoteID  = tblQP.QuoteID " +
                " LEFT JOIN tbl_Proposal P on P.ProposalID  = DR.ProposalID " +
                " LEFT JOIN tbl_Department D on D.DepartmentID  = P.DepartmentID  " +
                " LEFT JOIN tbl_Contract C on C.ContractID = DR.ProposalID  " +
                " LEFT JOIN tbl_Department D1 on P.CurDepartmentID  = D1.DepartmentID " +
                " where  DR.DeliveryReceiptID = @DeliveryReceiptID order by DR.UpdateTime ", connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (DR.UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@DeliveryReceiptID", id, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {                     
                        info.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        info.DeliveryReceiptType = GetDbReaderValue<int>(reader["DeliveryReceiptType"]);
                        info.DeliveryReceiptCode = GetDbReaderValue<string>(reader["DeliveryReceiptCode"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.ContractID = GetDbReaderValue<int>(reader["ContractID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.CurDepartmentCode = GetDbReaderValue<string>(reader["CurDepartmentCode"]);
                        info.QuoteCode = GetDbReaderValue<string>(reader["QuoteCode"]);
                        info.DeliveryReceiptDate = GetDbReaderValue<DateTime>(reader["DeliveryReceiptDate"]);
                        info.DeliveryReceiptNumber = GetDbReaderValue<int>(reader["DeliveryReceiptNumber"]);
                        info.ProposalID = GetDbReaderValue<int>(reader["ProposalID"]);
                        info.ProposalCode = GetDbReaderValue<string>(reader["ProposalCode"]);
                        info.IsVAT = GetDbReaderValue<bool>(reader["IsVAT"]);
                        info.VATNumber = GetDbReaderValue<double>(reader["VATNumber"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);
                        info.CurDepartmentName = GetDbReaderValue<string>(reader["CurDepartmentName"]);
                        info.DeliveryReceiptPlace = GetDbReaderValue<string>(reader["DeliveryReceiptPlace"]);
         
                       
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.CreateTime = GetDbReaderValue<DateTime>(reader["CreateTime"]);

                        info.ProposalTime = GetDbReaderValue<DateTime>(reader["ProposalTime"]);

                    }
                }
            }
            return info;
        }

        public int Create(SqlConnection connection, DeliveryReceiptInfo obj, string userID)
        {
            var currenttime = DateTime.Now.Date;
            int lastestInserted = 0;
            if (obj.DeliveryReceiptCode == null || obj.DeliveryReceiptCode == "") obj.DeliveryReceiptCode = "GN-" + obj.ProposalCode;
            DateTime localDate = DateTime.Now;
            using (var command = new SqlCommand("Insert into tbl_DeliveryReceipt " +
                " (DeliveryReceiptType, DeliveryReceiptCode,DeliveryReceiptDate, DeliveryReceiptNumber, " +
                "ProposalID, DeliveryReceiptPlace , ContractID " +
                "  , UserU , CreateTime, UserI)" +
                    "VALUES (@DeliveryReceiptType, @DeliveryReceiptCode,@DeliveryReceiptDate, @DeliveryReceiptNumber, " +
                    "@ProposalID, @DeliveryReceiptPlace , @ContractID ,@UserU, @CreateTime, @UserI )" +
                    " select IDENT_CURRENT('dbo.tbl_DeliveryReceipt') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@DeliveryReceiptType", obj.DeliveryReceiptType, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DeliveryReceiptCode", obj.DeliveryReceiptCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@DeliveryReceiptDate", obj.DeliveryReceiptDate, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ProposalID", obj.ProposalID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@ContractID", obj.ContractID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DeliveryReceiptNumber", obj.DeliveryReceiptNumber, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DeliveryReceiptPlace", !string.IsNullOrEmpty(obj.DeliveryReceiptPlace) ? obj.DeliveryReceiptPlace : " ", System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserU", userID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@UserI", userID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@CreateTime", localDate, System.Data.SqlDbType.DateTime);             
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
                    " set DeliveryReceiptID=@DeliveryReceiptID  , DeliveryReceiptTime=@DeliveryReceiptTime ,  CurrentFeature=@CurrentFeature where ProposalID=@ProposalID", connection))
                {
                    AddSqlParameter(command, "@ProposalID", obj.ProposalID, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@DeliveryReceiptID", lastestInserted, System.Data.SqlDbType.Int);
                    AddSqlParameter(command, "@DeliveryReceiptTime", currenttime, System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@CurrentFeature", "DeliveryReceipt", System.Data.SqlDbType.VarChar);
                    WriteLogExecutingCommand(command);
                    command.ExecuteScalar();
                }
            }

            return lastestInserted;
        }
        
        public int Update(SqlConnection connection, DeliveryReceiptInfo obj, string userID)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Update tbl_DeliveryReceipt " +
                " SET DeliveryReceiptCode = @DeliveryReceiptCode , DeliveryReceiptDate = @DeliveryReceiptDate, DeliveryReceiptType = @DeliveryReceiptType, DeliveryReceiptNumber = @DeliveryReceiptNumber , " +
                " DeliveryReceiptPlace = @DeliveryReceiptPlace, UserU = @UserU , ContractID=@ContractID " +
                    " where DeliveryReceiptID = @DeliveryReceiptID ", connection))
            {
                AddSqlParameter(command, "@DeliveryReceiptDate", obj.DeliveryReceiptDate, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@DeliveryReceiptType", obj.DeliveryReceiptType, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DeliveryReceiptCode", obj.DeliveryReceiptCode, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@DeliveryReceiptNumber", obj.DeliveryReceiptNumber, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DeliveryReceiptPlace", obj.DeliveryReceiptPlace, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@ContractID", obj.ContractID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserU", userID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@DeliveryReceiptID", obj.DeliveryReceiptID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);

                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }
            return lastestInserted;
        }


        public void Delete(SqlConnection connection, int _deliveryReceiptID)
        {
            
            using (var command = new SqlCommand("delete tbl_DeliveryReceipt_Items where DeliveryReceiptID = @DeliveryReceiptID ", connection))
            {
                AddSqlParameter(command, "@DeliveryReceiptID", _deliveryReceiptID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand("delete tbl_DeliveryReceiptEmployee where DeliveryReceiptID = @DeliveryReceiptID ", connection))
            {
                AddSqlParameter(command, "@DeliveryReceiptID", _deliveryReceiptID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand("delete tbl_DeliveryReceipt where DeliveryReceiptID = @DeliveryReceiptID ", connection))
            {
                AddSqlParameter(command, "@DeliveryReceiptID", _deliveryReceiptID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand("update  tbl_Proposal_Process " +
               "set DeliveryReceiptID=null, DeliveryReceiptTime=null, AcceptanceID=null  , AcceptanceTime=null ,  CurrentFeature=@CurrentFeature where DeliveryReceiptID=@DeliveryReceiptID", connection))
            {
                AddSqlParameter(command, "@DeliveryReceiptID", _deliveryReceiptID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CurrentFeature", "Contract", System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public void DeleteMuti(SqlConnection connection, string _deliveryReceiptIDs)
        {         
            using (var command = new SqlCommand("delete tbl_DeliveryReceipt_Items where DeliveryReceiptID in (" + _deliveryReceiptIDs + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand("delete tbl_DeliveryReceiptEmployee where DeliveryReceiptID in (" + _deliveryReceiptIDs + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand("delete tbl_DeliveryReceipt where DeliveryReceiptID in (" + _deliveryReceiptIDs + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
            using (var command = new SqlCommand("update  tbl_Proposal_Process " +
            "set DeliveryReceiptID=null, DeliveryReceiptTime=null, AcceptanceID=null  , AcceptanceTime=null ,  CurrentFeature=@CurrentFeature where DeliveryReceiptID in (" + _deliveryReceiptIDs + ")", connection))
            {
                AddSqlParameter(command, "@CurrentFeature", "Contract", System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public List<DeliveryReceiptEmployeeInfo> GetDeliveryReceiptEmployeesById(SqlConnection connection, string id)
        {
            List<DeliveryReceiptEmployeeInfo> ret = new List<DeliveryReceiptEmployeeInfo>();
            using (var command = new SqlCommand(@"select DE.AutoID, DE.DeliveryReceiptID, DE.EmployeeID,  DE.Role 
                ,E.Title, E.RoleName, E.Name, E.Generic , E.DepartmentID , E.UserID from
                (select * from tbl_DeliveryReceiptEmployee where DeliveryReceiptID = @DeliveryReceiptID ) DE
                left join  tbl_Employee as E on E.EmployeeID = DE.EmployeeID
                ", connection))
            {
                AddSqlParameter(command, "@DeliveryReceiptID", id, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new DeliveryReceiptEmployeeInfo();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        item.EmployeeID = GetDbReaderValue<int>(reader["EmployeeID"]);
                        item.Role = GetDbReaderValue<int>(reader["Role"]);
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

        public List<DeliveryReceiptEmployeeInfo> GetDeliveryReceiptEmployeesByIds(SqlConnection connection, string ids)
        {
            List<DeliveryReceiptEmployeeInfo> ret = new List<DeliveryReceiptEmployeeInfo>();
            using (var command = new SqlCommand(@"select DE.AutoID, DE.DeliveryReceiptID, DE.EmployeeID, DE.Role, E.Title, E.RoleName, E.Name, E.Generic , E.DepartmentID , E.UserID from
                (select * from tbl_DeliveryReceiptEmployee where DeliveryReceiptID in (" + ids + @")) DE
                left join  tbl_Employee as E on E.EmployeeID = DE.EmployeeID
                ", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var item = new DeliveryReceiptEmployeeInfo();
                        item.AutoID = GetDbReaderValue<int>(reader["AutoID"]);
                        item.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        item.EmployeeID = GetDbReaderValue<int>(reader["EmployeeID"]);
                     
                        item.Role = GetDbReaderValue<int>(reader["Role"]);
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

        public void InsertDeliveryReceiptEmployee(SqlConnection connection, int _DeliveryReceiptID, DeliveryReceiptEmployeeInfo item)
        {
            using (var command = new SqlCommand("Insert into [dbo].[tbl_DeliveryReceiptEmployee] ([DeliveryReceiptID], [EmployeeID], Role)" +
                   "VALUES(@DeliveryReceiptID,@EmployeeID,Role)", connection))
            {

                AddSqlParameter(command, "@DeliveryReceiptID", _DeliveryReceiptID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@EmployeeID", item.EmployeeID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Role", item.Role, System.Data.SqlDbType.Int);
     
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


        public void DeleteDeliveryReceiptEmployees(SqlConnection connection, string _DeliveryReceiptEmployeeID)
        {
            using (var command = new SqlCommand(" delete tbl_DeliveryReceiptEmployee where AutoID in (" + _DeliveryReceiptEmployeeID + ")", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


        public List<string> getDeliveryReceiptByContractids(SqlConnection connection, string contractids)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select DeliveryReceiptID as ID from tbl_DeliveryReceipt  where ContractID in (" + contractids + ")", connection))
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

        public DeliveryReceiptWithDepartment getItemCreateAna(SqlConnection connection, int id,string _userID)
        {
            DeliveryReceiptWithDepartment ret = new DeliveryReceiptWithDepartment();
            using (var command = new SqlCommand(
              " select P.DepartmentID, QC.CustomerID, CT.CustomerName, C.ContractCode, C.UserI as UserIContract  from tbl_DeliveryReceipt DR " +
              " join tbl_proposal_process  PP on DR.DeliveryReceiptID = PP.DeliveryReceiptID " +
              " join tbl_Quote_Customer QC on QC.QuoteID = PP.QuoteID and QC.IsChoosed = 1 " +
               " join tbl_Customer CT on CT.CustomerID = QC.CustomerID  " +
              " join tbl_proposal P  on P.ProposalID = PP.ProposalID  " +
              " join tbl_Contract C on C.ContractID = PP.ContractID  " +
              " where DR.DeliveryReceiptID = @id"
                , connection))
            {
                if (!string.IsNullOrEmpty(_userID) && _userID != "admin")
                {
                    command.CommandText += " and (DR.UserAssign = @UserID )";
                    AddSqlParameter(command, "@UserID", _userID, SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@id", id, SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        ret.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        ret.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);
                        ret.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        ret.UserIContract = GetDbReaderValue<string>(reader["UserIContract"]);
                    }
                }
            }
            return ret;
        }

        public List<string> GetDeliveryReceiptByQuoteIds(SqlConnection connection, string quoteids)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select DeliveryReceiptID as ID from tbl_Proposal_Process  where QuoteID in (" + quoteids + ")", connection))
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

        public List<string> GetQuoteByDeliveryReceiptIds(SqlConnection connection, string deliveryReceiptIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select QuoteID as ID from tbl_Proposal_Process  where DeliveryReceiptID in (" + deliveryReceiptIDs + ")", connection))
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
