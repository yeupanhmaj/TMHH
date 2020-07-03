using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Models.Common;

namespace AdminPortal.Services
{
    public class ItemsServices : BaseService<ItemsServices>
    {

        public List<ItemInfo> GetItemsByCondition(string name, string code)
        {
            try
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    List<ItemInfo> ret = ItemsDataLayer.GetInstance().GetItemsByCondition(connection, name, code, "VT");
                     return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
    
        }

        public ItemRequest CreateItem(string name, string code, string unit, string userID)
        {
            try
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    int id = ItemsDataLayer.GetInstance().CreateItem(connection, name, code, unit , userID);
                    ItemRequest ret = new ItemRequest();
                    ret.ItemID = id;
                    ret.ItemName = name;
                    ret.ItemCode = code;
                    ret.ItemUnit = unit;

                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}