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
    public class PracticeService : BaseService<PracticeService>
    {
        public PracticeInfo GetDetail(string id)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            PracticeInfo record = new PracticeInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = PracticeDataLayer.GetInstance().GetUser(connection, id);

                return record;
            }
        }
        public List<PracticeInfo> GetList(PracticeCriteria conditions)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<PracticeInfo> ListUserInfo = PracticeDataLayer.GetInstance().Getlist(connection, conditions);
                return ListUserInfo;
            }
        }
        public int getTotalRecords(PracticeCriteria conditions)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return PracticeDataLayer.GetInstance().GetTotalRecords(connection, conditions);
            }
        }
        public void DeleteUser(string id)
        {
            //connection
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                //gọi tương tác tới datalayer
                PracticeDataLayer.GetInstance().DeleteUser(connection, id);                
            }
        }
        public void DeleteAllUser(string ids)
        {
            //connection
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                //gọi tương tác tới datalayer
                PracticeDataLayer.GetInstance().DeleteAllUser(connection, ids);
            }
        }
        public void CreateUser(PracticeInfo value)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            PracticeInfo record = new PracticeInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                //string applicationId = EncryptionUtils.GetApplicationId();
                //string applicationName = EncryptionUtils.GetApplicationName();
                
                PracticeDataLayer.GetInstance().CreateUser(connection, value);
            }
        }
        public void CreateConfirm(ConfirmInfo value)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            PracticeInfo record = new PracticeInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                //string applicationId = EncryptionUtils.GetApplicationId();
                //string applicationName = EncryptionUtils.GetApplicationName();

                PracticeDataLayer.GetInstance().CreateConfirm(connection, value);
            }
        }
        public void UpdateUser(PracticeInfo _user)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            PracticeLogInfo record = new PracticeLogInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {              
                PracticeDataLayer.GetInstance().UpdateUser(connection,_user);
            }
        }
    }
}
