using AdminPortal.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.DataLayers
{
    public class CategoryDataLayer: BaseLayerData<CategoryDataLayer>
    {
        public List<CategoryInfo> GetListCategory(SqlConnection connection)
        {
            var result = new List<CategoryInfo>();
            using (var command = new SqlCommand("select CategoryID,CategoryName,CategoryCode from tbl_Category " +
                " where 1=1", connection))
            {              
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new CategoryInfo();
                        info.CategoryID = GetDbReaderValue<int>(reader["CategoryID"]);
                        info.CategoryName = GetDbReaderValue<string>(reader["CategoryName"]);
                        info.CategoryCode = GetDbReaderValue<string>(reader["CategoryCode"]);
                        //      info.Disable = GetDbReaderValue<byte>(reader["GroupID"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
    }
}
