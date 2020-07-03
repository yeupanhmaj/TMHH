using AdminPortal.DataLayers;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Services
{
    public class CategoryService : BaseService<CategoryService>
    {
        public List<CategoryInfo> GetListCategory()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<CategoryInfo> ListUserInfo = CategoryDataLayer.GetInstance().GetListCategory(connection);
                return ListUserInfo;
            }
        }
    }
}
