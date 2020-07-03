using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class AnalyzerDataLayer : BaseLayerData<AnalyzerDataLayer>
    {

        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả tài sản
        /// </summary>
        /// <param name="connection"> </param>
        /// <returns>Return List<AnalyzerInfo></returns>
        /// 
        public List<AnalyzerInfo> GetAllAnalyzer(SqlConnection connection)
        {
            var result = new List<AnalyzerInfo>();
            using (var command = new SqlCommand("Select AN.*, DR.DepartmentCode as DepartmentRootCode, DR.DepartmentName as DepartmentRootName, D.DepartmentCode as DepartmentCode, D.DepartmentName as DepartmentName  " +
                " from tbl_Analyzer AN  " +
                " LEFT JOIN tbl_department DR  on DR.DepartmentID  = AN.DepartmentRootID " +
                " LEFT JOIN tbl_department D  on D.DepartmentID  = AN.DepartmentID " +
                " where  1 = 1 order by AN.UpdateTime Desc ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new AnalyzerInfo();
                        info.AnalyzerID = GetDbReaderValue<int>(reader["AnalyzerID"]);
                        info.AnalyzerCode = GetDbReaderValue<string>(reader["AnalyzerCode"]);
                        info.AnalyzerAccountantCode = GetDbReaderValue<string>(reader["AnalyzerAccountantCode"]);
                        info.AnalyzerName = GetDbReaderValue<string>(reader["AnalyzerName"]);
                        info.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        info.Description = GetDbReaderValue<string>(reader["Description"]);
                        info.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        info.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        info.TotalPrice = GetDbReaderValue<double>(reader["TotalPrice"]);
                        info.AnalyzerType = GetDbReaderValue<int>(reader["AnalyzerType"]);

                        info.DepartmentRootID = GetDbReaderValue<int>(reader["DepartmentRootID"]);
                        info.DepartmentRootCode = GetDbReaderValue<string>(reader["DepartmentRootCode"]);
                        info.DepartmentRootName = GetDbReaderValue<string>(reader["DepartmentRootName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);

                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.UserIContract = GetDbReaderValue<string>(reader["UserIContract"]);

                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);

                        info.Serial = GetDbReaderValue<string>(reader["Serial"]);
                        info.ExpirationDate = GetDbReaderValue<DateTime?>(reader["ExpirationDate"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        
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
        /// <summary>
        /// Hàm lấy danh sách tài sản
        /// </summary>
        /// <param Criteria="_criteria"> </param>
        /// <returns>Return List<AnalyzerInfo></returns>
        /// 
        public List<AnalyzerInfo> getAnalyzer(SqlConnection connection, AnalyzerSeachCriteria _criteria)
        {
            var result = new List<AnalyzerInfo>();
            using (var command = new SqlCommand(" Select AN.*, DR.DepartmentCode as DepartmentRootCode, DR.DepartmentName as DepartmentRootName, D.DepartmentCode as DepartmentCode, D.DepartmentName as DepartmentName " +
                " from (Select " +
                "AN.* " +
                " from tbl_Analyzer AN where  1 = 1 and AN.InTime between @FromDate and @ToDate", connection)) 
            {
                AddSqlParameter(command, "@FromDate",_criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);
             
                if (!string.IsNullOrEmpty(_criteria.AnalyzerCode))
                {
                    command.CommandText += " and AN.AnalyzerCode = @AnalyzerCode";
                    AddSqlParameter(command, "@AnalyzerCode", _criteria.AnalyzerCode, System.Data.SqlDbType.NVarChar);
                }

                if (!string.IsNullOrEmpty(_criteria.DepartmentID))
                {
                    command.CommandText += " and AN.DepartmentID = @DepartmentID";
                    AddSqlParameter(command, "@DepartmentID", _criteria.DepartmentID, System.Data.SqlDbType.Int);
                }

                if (!string.IsNullOrEmpty(_criteria.CustomerID))
                {
                    command.CommandText += " and AN.CustomerID = @CustomerID";
                    AddSqlParameter(command, "@CustomerID", _criteria.CustomerID, System.Data.SqlDbType.Int);
                }

                command.CommandText += " ) as AN " +
                " LEFT JOIN tbl_department DR  on DR.DepartmentID  = AN.DepartmentRootID " +
                " LEFT JOIN tbl_department D  on D.DepartmentID  = AN.DepartmentID " +
                " where  1 = 1 ";

                command.CommandText += " order by AN.UpdateTime Desc ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new AnalyzerInfo();
                        info.AnalyzerID = GetDbReaderValue<int>(reader["AnalyzerID"]);
                        info.AnalyzerCode = GetDbReaderValue<string>(reader["AnalyzerCode"]);
                        info.AnalyzerAccountantCode = GetDbReaderValue<string>(reader["AnalyzerAccountantCode"]);
                        info.AnalyzerName = GetDbReaderValue<string>(reader["AnalyzerName"]);
                        info.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        info.Description = GetDbReaderValue<string>(reader["Description"]);
                        info.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        info.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        info.TotalPrice = GetDbReaderValue<double>(reader["TotalPrice"]);
                        info.AnalyzerType = GetDbReaderValue<int>(reader["AnalyzerType"]);

                        info.DepartmentRootID = GetDbReaderValue<int>(reader["DepartmentRootID"]);
                        info.DepartmentRootCode = GetDbReaderValue<string>(reader["DepartmentRootCode"]);
                        info.DepartmentRootName = GetDbReaderValue<string>(reader["DepartmentRootName"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);

                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.UserIContract = GetDbReaderValue<string>(reader["UserIContract"]);

                        info.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        info.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);

                        info.Serial = GetDbReaderValue<string>(reader["Serial"]);
                        info.ExpirationDate = GetDbReaderValue<DateTime?>(reader["ExpirationDate"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);

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

        /// <summary>
        /// Hàm lấy tài sản theo ID
        /// </summary>
        /// <param ID="_ID"></param>
        /// <returns>Return List<AnalyzerInfo></returns>
        /// 
        public AnalyzerInfo getAnalyzer(SqlConnection connection, int _ID)
        {
            AnalyzerInfo result = null;
            using (var command = new SqlCommand(
                " Select AN.*, DR.DepartmentCode as DepartmentRootCode, DR.DepartmentName as DepartmentRootName, D.DepartmentCode as DepartmentCode, D.DepartmentName as DepartmentName " +
                " from (Select AN.* from tbl_Analyzer AN where AN.AnalyzerID = @AnalyzerID) " +
                "as AN  " +
                " LEFT JOIN tbl_department DR  on DR.DepartmentID  = AN.DepartmentRootID " +
                " LEFT JOIN tbl_department D  on D.DepartmentID  = AN.DepartmentID " 
            , connection))
            {
                AddSqlParameter(command, "@AnalyzerID", _ID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new AnalyzerInfo();
                        result.AnalyzerID = GetDbReaderValue<int>(reader["AnalyzerID"]);
                        result.AnalyzerCode = GetDbReaderValue<string>(reader["AnalyzerCode"]);
                        result.AnalyzerAccountantCode = GetDbReaderValue<string>(reader["AnalyzerAccountantCode"]);
                        result.AnalyzerName = GetDbReaderValue<string>(reader["AnalyzerName"]);
                        result.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        result.Description = GetDbReaderValue<string>(reader["Description"]);
                        result.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        result.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        result.TotalPrice = GetDbReaderValue<double>(reader["TotalPrice"]);
                        result.AnalyzerType = GetDbReaderValue<int>(reader["AnalyzerType"]);

                        result.DepartmentRootID = GetDbReaderValue<int>(reader["DepartmentRootID"]);
                        result.DepartmentRootCode = GetDbReaderValue<string>(reader["DepartmentRootCode"]);
                        result.DepartmentRootName = GetDbReaderValue<string>(reader["DepartmentRootName"]);
                        result.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        result.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        result.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);

                        result.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        result.UserIContract = GetDbReaderValue<string>(reader["UserIContract"]);

                        result.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        result.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);

                        result.Serial = GetDbReaderValue<string>(reader["Serial"]);
                        result.ExpirationDate = GetDbReaderValue<DateTime?>(reader["ExpirationDate"]);
                        result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);

                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Hàm lấy tài sản theo Code
        /// </summary>
        /// <param AnalyzerCode="_code"></param>
        /// <returns>Return List<AnalyzerInfo></returns>
        /// 
        public AnalyzerInfo GetAnalyzerByCode(SqlConnection connection, string _code)
        {
            AnalyzerInfo result = null;
            using (var command = new SqlCommand(
               "Select AN.*, DR.DepartmentCode as DepartmentRootCode, DR.DepartmentName as DepartmentRootName, D.DepartmentCode as DepartmentCode, D.DepartmentName as DepartmentName  " +
               " from (Select AN.* from tbl_Analyzer AN where AN.AnalyzerCode = @AnalyzerCode) " +
               "as AN    " +
                 " LEFT JOIN tbl_department DR  on DR.DepartmentID  = AN.DepartmentRootID " +
                " LEFT JOIN tbl_department D  on D.DepartmentID  = AN.DepartmentID " 
           , connection))
            {
                AddSqlParameter(command, "@AnalyzerCode", _code, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new AnalyzerInfo();
                        result.AnalyzerID = GetDbReaderValue<int>(reader["AnalyzerID"]);
                        result.AnalyzerCode = GetDbReaderValue<string>(reader["AnalyzerCode"]);
                        result.AnalyzerAccountantCode = GetDbReaderValue<string>(reader["AnalyzerAccountantCode"]);
                        result.AnalyzerName = GetDbReaderValue<string>(reader["AnalyzerName"]);
                        result.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        result.Description = GetDbReaderValue<string>(reader["Description"]);
                        result.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        result.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        result.TotalPrice = GetDbReaderValue<double>(reader["TotalPrice"]);
                        result.AnalyzerType = GetDbReaderValue<int>(reader["AnalyzerType"]);

                        result.DepartmentRootID = GetDbReaderValue<int>(reader["DepartmentRootID"]);
                        result.DepartmentRootCode = GetDbReaderValue<string>(reader["DepartmentRootCode"]);
                        result.DepartmentRootName = GetDbReaderValue<string>(reader["DepartmentRootName"]);
                        result.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        result.DepartmentCode = GetDbReaderValue<string>(reader["DepartmentCode"]);
                        result.DepartmentName = GetDbReaderValue<string>(reader["DepartmentName"]);

                        result.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        result.UserIContract = GetDbReaderValue<string>(reader["UserIContract"]);

                        result.CustomerID = GetDbReaderValue<int>(reader["CustomerID"]);
                        result.CustomerName = GetDbReaderValue<string>(reader["CustomerName"]);

                        result.Serial = GetDbReaderValue<string>(reader["Serial"]);
                        result.ExpirationDate = GetDbReaderValue<DateTime?>(reader["ExpirationDate"]);
                        result.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        result.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);

                        result.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        result.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        result.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        result.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                    }
                }
                return result;
            }
        }


        //public AnalyzerPrint getAnalyzerInfoByQuote(SqlConnection connection, int QuoteInfo)
        //{
        //    AnalyzerPrint ret = new AnalyzerPrint();
        //    using (var command = new SqlCommand(
        //        " select * from tbl_Analyzer " +
        //        "  where tbl_Analyzer.QuoteID = " + QuoteInfo,
        //        connection))
        //    {
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                ret.BidExpirated = GetDbReaderValue<int>(reader["BidExpirated"]);
        //                ret.BidExpiratedUnit = GetDbReaderValue<string>(reader["BidExpiratedUnit"]);
        //                ret.AnalyzerCode = GetDbReaderValue<string>(reader["AnalyzerCode"]);
        //                ret.BidType = GetDbReaderValue<string>(reader["BidType"]);
        //            }
        //        }
        //    }
        //    return ret;
        //}


        /// <summary>
        /// Hàm lấy số record theo điều kiện
        /// </summary>
        /// <param Criteria="_criteria"></param>
        /// <returns>Return List<AnalyzerInfo></returns>
        /// 
        public int getTotalRecords(SqlConnection connection, AnalyzerSeachCriteria _criteria)
        {
            if (_criteria != null)
            {
                using (var command = new SqlCommand("Select count(AN.AnalyzerID) as TotalRecords from (Select AN.* " +
                " from tbl_Analyzer AN where  1 = 1 and AN.InTime between @FromDate and @ToDate", connection))
                {
                    AddSqlParameter(command, "@FromDate",_criteria.FromDate.Value.ToString("yyyy-MM-dd 00:00:00"), System.Data.SqlDbType.DateTime);
                    AddSqlParameter(command, "@ToDate", _criteria.ToDate.Value.ToString("yyyy-MM-dd 23:59:59"), System.Data.SqlDbType.DateTime);

                    if (!string.IsNullOrEmpty(_criteria.AnalyzerCode))
                    {
                        command.CommandText += " and AN.AnalyzerCode = @AnalyzerCode";
                        AddSqlParameter(command, "@AnalyzerCode", _criteria.AnalyzerCode, System.Data.SqlDbType.NVarChar);
                    }

                    if (!string.IsNullOrEmpty(_criteria.DepartmentID))
                    {
                        command.CommandText += " and AN.DepartmentID = @DepartmentID";
                        AddSqlParameter(command, "@DepartmentID", _criteria.DepartmentID, System.Data.SqlDbType.Int);
                    }

                    if (!string.IsNullOrEmpty(_criteria.CustomerID))
                    {
                        command.CommandText += " and AN.CustomerID = @CustomerID";
                        AddSqlParameter(command, "@CustomerID", _criteria.CustomerID, System.Data.SqlDbType.Int);
                    }

                    command.CommandText += " ) as AN" +
                         " where 1 = 1 ";

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
                using (var command = new SqlCommand("Select count(*) as TotalRecords  from tbl_Analyzer where 1 = 1 ", connection))
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

        /// <summary>
        /// Hàm Insert Tài sản
        /// </summary>
        /// <param AnalyzerInfo="_Analyzer"></param>
        /// <param userInput="_userI"></param>
        /// <returns>Return List<AnalyzerInfo></returns>
        /// 
        public int InsertAnalyzer(SqlConnection connection, AnalyzerInfo _Analyzer, int seq, string _userI)
        {
            int lastestInserted = 0;
            var currenttime = DateTime.Now.Date;
            if (_Analyzer.AnalyzerCode == null || _Analyzer.AnalyzerCode == "") _Analyzer.AnalyzerCode = DateTime.Now.ToString("yyMMddHHmmssfff");
            using (var command = new SqlCommand("Insert into [dbo].[tbl_Analyzer] " +
                "(AnalyzerCode,AnalyzerAccountantCode,AnalyzerName, QuoteItemID, Description,Amount, ItemPrice, TotalPrice" +
                ", DepartmentRootID, DepartmentID, ContractCode, UserIContract, CustomerID, CustomerName, ExpirationDate, DateIn" +
                ", DeliveryReceiptID, Serial, UserI, UserU, UpdateTime, Seq, AnalyzerType)" +
        "VALUES(@AnalyzerCode,@AnalyzerAccountantCode, @AnalyzerName, @QuoteItemID , @Description,@Amount, @ItemPrice, @TotalPrice" +
                ", @DepartmentRootID, @DepartmentID, @ContractCode, @UserIContract, @CustomerID, @CustomerName, @ExpirationDate, @DateIn" +
                ", @DeliveryReceiptID, @Serial, @UserI, @UserI, Getdate(), @Seq, @AnalyzerType) " +
                "select IDENT_CURRENT('dbo.tbl_Analyzer') as LastInserted ", connection))
            {
               
                AddSqlParameter(command, "@AnalyzerCode", "", System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@AnalyzerAccountantCode", _Analyzer.AnalyzerAccountantCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@AnalyzerName", _Analyzer.AnalyzerName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@AnalyzerType", _Analyzer.AnalyzerType, System.Data.SqlDbType.Int);

                AddSqlParameter(command, "@QuoteItemID", _Analyzer.QuoteItemID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Description", _Analyzer.Description, System.Data.SqlDbType.NVarChar);

                AddSqlParameter(command, "@Amount", _Analyzer.Amount, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@ItemPrice", _Analyzer.ItemPrice, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@TotalPrice", _Analyzer.TotalPrice, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@DepartmentRootID", _Analyzer.DepartmentRootID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DepartmentID", _Analyzer.DepartmentID, System.Data.SqlDbType.Int);

                AddSqlParameter(command, "@ContractCode", _Analyzer.ContractCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@UserIContract", _Analyzer.UserIContract, System.Data.SqlDbType.VarChar);

                AddSqlParameter(command, "@CustomerID", _Analyzer.CustomerID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@CustomerName", _Analyzer.CustomerName, System.Data.SqlDbType.NVarChar);

                AddSqlParameter(command, "@ExpirationDate", _Analyzer.ExpirationDate, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@DateIn", _Analyzer.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@DeliveryReceiptID", _Analyzer.DeliveryReceiptID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Serial", _Analyzer.Serial, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Seq", seq, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
  
                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }

            return lastestInserted;
        }


        /// <summary>
        /// Hàm Update Tài sản
        /// </summary>
        /// <param AnalyzerInfo="_Analyzer"></param>
        /// <param userInput="_userI"></param>
        /// <returns>Return List<AnalyzerInfo></returns>
        /// 
        public void UpdateAnalyzer(SqlConnection connection, int _id, string _anaCode)
        {
            using (var command = new SqlCommand(" update tbl_analyzer set AnalyzerCode = @AnalyzerCode WHERE (AnalyzerID = @AnalyzerID)    ", connection))
            {
                AddSqlParameter(command, "@AnalyzerID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@AnalyzerCode", _anaCode, System.Data.SqlDbType.VarChar);

                WriteLogExecutingCommand(command);
                command.ExecuteScalar();

            }
        }

        /// <summary>
        /// Hàm Update Tài sản
        /// </summary>
        /// <param AnalyzerInfo="_Analyzer"></param>
        /// <param userInput="_userI"></param>
        /// <returns>Return List<AnalyzerInfo></returns>
        /// 
        public void UpdateAnalyzer(SqlConnection connection, int _id, AnalyzerInfo _Analyzer, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_Analyzer \n" +
                            " SET  AnalyzerCode = @AnalyzerCode ,AnalyzerAccountantCode = @AnalyzerAccountantCode " +
                            ",  AnalyzerName = @AnalyzerName , QuoteItemID= @QuoteItemID,Description = @Description  " +
                            ", Amount=@Amount ,ItemPrice = @ItemPrice ,TotalPrice = @TotalPrice " +
                            ", DepartmentRootID=@DepartmentRootID ,DepartmentID = @DepartmentID ,ContractCode = @ContractCode ,UserIContract = @UserIContract " +
                            ", CustomerID=@CustomerID ,ExpirationDate = @ExpirationDate" +
                            ", DateIn=@DateIn , DeliveryReceiptID = @DeliveryReceiptID " +
                            ", Serial=@Serial , UserU=@UserU,UpdateTime=getdate()" +
                            ", AnalyzerType = @AnalyzerType  \n" +
                            " WHERE (AnalyzerID = @AnalyzerID) ", connection))
            
            {
                AddSqlParameter(command, "@AnalyzerID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@AnalyzerCode", _Analyzer.AnalyzerCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@AnalyzerAccountantCode", _Analyzer.AnalyzerAccountantCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@AnalyzerName", _Analyzer.AnalyzerName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@AnalyzerType", _Analyzer.AnalyzerType, System.Data.SqlDbType.Int);

                AddSqlParameter(command, "@QuoteItemID", _Analyzer.QuoteItemID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Description", _Analyzer.Description, System.Data.SqlDbType.NVarChar);

                AddSqlParameter(command, "@Amount", _Analyzer.Amount, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@ItemPrice", _Analyzer.ItemPrice, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@TotalPrice", _Analyzer.TotalPrice, System.Data.SqlDbType.Float);
                AddSqlParameter(command, "@DepartmentRootID", _Analyzer.DepartmentRootID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@DepartmentID", _Analyzer.DepartmentID, System.Data.SqlDbType.Int);

                AddSqlParameter(command, "@ContractCode", _Analyzer.ContractCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@UserIContract", _Analyzer.UserIContract, System.Data.SqlDbType.VarChar);

                AddSqlParameter(command, "@CustomerID", _Analyzer.CustomerID, System.Data.SqlDbType.Int);

                AddSqlParameter(command, "@ExpirationDate", _Analyzer.ExpirationDate, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@DateIn", _Analyzer.DateIn, System.Data.SqlDbType.DateTime);
                AddSqlParameter(command, "@DeliveryReceiptID", _Analyzer.DeliveryReceiptID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Serial", _Analyzer.Serial, System.Data.SqlDbType.VarChar);

                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void Delete(SqlConnection connection,int _AnalyzerID)
        {
            using (var command = new SqlCommand(" Delete tbl_Analyzer where AnalyzerID=@AnalyzerID ", connection))
            {
                AddSqlParameter(command, "@AnalyzerID", _AnalyzerID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteMuti(SqlConnection connection, string _AnalyzerIDs)
        {
            using (var command = new SqlCommand(" Delete tbl_Analyzer where AnalyzerID in (" + _AnalyzerIDs + ")  ", connection))
            {
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public List<string> getListAnalyzerCode(SqlConnection connection, string AnalyzerCode)
        {
            var result = new List<string>();
            using (var command = new SqlCommand(" Select TOP 10 AnalyzerCode from tbl_Analyzer where AnalyzerCode like '%" + AnalyzerCode + "%'", connection))
            {
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string _AnalyzerCode = GetDbReaderValue<string>(reader["AnalyzerCode"]);
                        result.Add(_AnalyzerCode);
                    }
                }
                return result;
            }
        }

        public List<string> GetAnalyzerByQuoteIds(SqlConnection connection, string quoteIDs)
        {
            List<string> result = new List<string>();
            using (var command = new SqlCommand("select AnalyzerID as ID from tbl_Analyzer  where QuoteID in (" + quoteIDs + ")", connection))
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
        //Analyzer by date
        //Nguyen Minh Hoang
        public List<AnalyzerInfo> GetAnalyzerByDate(SqlConnection connection, AnalyzerSeachCriteria _criteria)
        {
            List<AnalyzerInfo> result = new List<AnalyzerInfo>();
            using (var command = new SqlCommand(
               " select *,D.DepartmentName as CurentDepartment, DR.DeliveryReceiptDate from " +
               " tbl_Analyzer A left join tbl_Department D on A.DepartmentID = D.DepartmentID  " +
               " left join tbl_DeliveryReceipt DR on A.DeliveryReceiptID = DR.DeliveryReceiptID " +
               " where 1=1 "
           , connection))
            {
                command.CommandText += " and A.DateIn between @start and @end ";
                AddSqlParameter(command, "@start", _criteria.FromDate, System.Data.SqlDbType.Date);
                AddSqlParameter(command, "@end", _criteria.ToDate, System.Data.SqlDbType.Date);

                if(!string.IsNullOrEmpty(_criteria.DepartmentName))
                {
                    command.CommandText += " and D.DepartmentName like N'%"+_criteria.DepartmentName+"%' ";
                }
                if (!string.IsNullOrEmpty(_criteria.CustomerName))
                {
                    command.CommandText += " and D.CustomerName like N'%" + _criteria.CustomerName + "%' ";
                }

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new AnalyzerInfo();
                        info.AnalyzerID = GetDbReaderValue<int>(reader["AnalyzerID"]);
                        info.AnalyzerCode = GetDbReaderValue<string>(reader["AnalyzerCode"]);
                        info.AnalyzerAccountantCode = GetDbReaderValue<string>(reader["AnalyzerAccountantCode"]);
                        info.AnalyzerName = GetDbReaderValue<string>(reader["AnalyzerName"]);
                        info.AnalyzerGroupID = GetDbReaderValue<int>(reader["AnalyzerGroupID"]);
                        info.QuoteItemID = GetDbReaderValue<int>(reader["QuoteItemID"]);
                        info.Description = GetDbReaderValue<string>(reader["Description"]);
                        info.Amount = GetDbReaderValue<double>(reader["Amount"]);
                        info.ItemPrice = GetDbReaderValue<double>(reader["ItemPrice"]);
                        info.TotalPrice = GetDbReaderValue<double>(reader["TotalPrice"]);
                        info.DepartmentRootID = GetDbReaderValue<int>(reader["DepartmentRootID"]);
                        info.DepartmentID = GetDbReaderValue<int>(reader["DepartmentID"]);
                        info.DepartmentName = GetDbReaderValue<string>(reader["CurentDepartment"]);
                        info.ContractCode = GetDbReaderValue<string>(reader["ContractCode"]);
                        info.UserIContract = GetDbReaderValue<string>(reader["UserIContract"]);
                        info.CustomerName = GetDbReaderValue<string?>(reader["CustomerName"]);
                        info.ExpirationDate = GetDbReaderValue<DateTime>(reader["ExpirationDate"]);
                        info.DateIn = GetDbReaderValue<DateTime>(reader["DateIn"]);
                        info.DeliveryReceiptID = GetDbReaderValue<int>(reader["DeliveryReceiptID"]);
                        info.Serial = GetDbReaderValue<string>(reader["Serial"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        info.DeliveryReceiptDate = GetDbReaderValue<DateTime>(reader["DeliveryReceiptDate"]);
                        info.AnalyzerType = GetDbReaderValue<int>(reader["AnalyzerType"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int GetMaxPropCode(SqlConnection connection, int year)
        {
            string strlSQLGetMaxNumber = "select isnull(max(seq), 0) " +
                "from tbl_Analyzer  " +
                "where year(Datein) = @year ";
            using (var command = new SqlCommand(strlSQLGetMaxNumber))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SqlParameter("@year", year));
                int maxCode = (int)db.ExcuteScalar(connection, command);
                maxCode++;
                return maxCode;
            }
        }
    }
}
