using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class ItemsDataLayer : BaseLayerData<ItemsDataLayer>
    {
        DataProvider db = new DataProvider();
        public List<ItemInfo> GetItemsByCondition(SqlConnection connection, string name, string code, string ItemTypeCode)
        {
            var result = new List<ItemInfo>();
            var sqlQuery = @"
                        Select TOP 10 i.itemID, i.itemCode, i.itemName , i.Unit from
                        (SELECT ClassID 
                          FROM tbl_Class
                          where ClassCode = @ItemTypeCode or ClassCode like '%UNKNOW%') as  p
                        inner join 
                        (select ClassID, CategoryID from tbl_Category ) as c
                        on c.ClassID = p.ClassID
                        inner join tbl_items as i
                        on i.CategoryID = c.CategoryID
                                        
                        ";
            if (name != "")
            {
                sqlQuery += "\n and FREETEXT(i.itemName,@name)";
            }
            if (code != "")
            {
                sqlQuery += "\n and i.itemCode like @code ";
            }
            sqlQuery = Regex.Replace(sqlQuery, @"\n", "");
            sqlQuery = Regex.Replace(sqlQuery, @"\r", "");
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            sqlQuery = regex.Replace(sqlQuery, " ");
      
            using (var command = new SqlCommand(sqlQuery, connection))
            {

                if (name != "")
                {
                    AddSqlParameter(command, "@name", '%' + name + '%' , System.Data.SqlDbType.NVarChar);
                }
                if (code != "")
                {
                    AddSqlParameter(command, "@code", '%' + code + '%', System.Data.SqlDbType.VarChar);
                }
                AddSqlParameter(command, "@ItemTypeCode", ItemTypeCode , System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ItemInfo();
                        info.ItemID = GetDbReaderValue<int>(reader["itemID"]);
                        info.ItemCode = GetDbReaderValue<string>(reader["itemCode"]);
                        info.ItemName = GetDbReaderValue<string>(reader["itemName"]);
                        info.ItemUnit = GetDbReaderValue<string>(reader["Unit"]);
              
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public int CreateItem(SqlConnection connection, string name , string code , string unit, string userID)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_items] (itemCode, itemName, Unit, CategoryID, InStockMin, InStockMax , ItemType,CheckExpire , StandarCost ,RoundRatio , " +
                "SpecialItem,  UserI )" +
                   "VALUES(@itemCode,@itemName, @Unit, 2037,0 , 0 , 7 , 0, 0,0,0, @UserI  )  " +
                   " select IDENT_CURRENT('dbo.tbl_items') as LastInserted ", connection))
            {
                AddSqlParameter(command, "@itemCode", code, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@itemName", name, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Unit", unit , System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserI", userID, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                var lastInsertedRaw = command.ExecuteScalar();
                if (lastInsertedRaw != null && !DBNull.Value.Equals(lastInsertedRaw))
                {
                    lastestInserted = Convert.ToInt32(lastInsertedRaw);
                }
            }

            return lastestInserted;
        }
    }
}
