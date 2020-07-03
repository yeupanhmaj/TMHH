using EncryptionLibrary;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AdminPortal.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace AdminPortal.Services
{
    public class UserService : BaseService<UserService>
    {
        public void TrackUserAction(UserLogInfo userLoginfo)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                UsersDataLayer.GetInstance().TrackingUserBehavior(connection, userLoginfo);
            }     
        }

        public List<UserInfo> GetList(UserCriteria conditions)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<UserInfo> ListUserInfo = UsersDataLayer.GetInstance().Getlist(connection, conditions);
                return ListUserInfo;
            }
        }

        public int getTotalRecords(UserCriteria conditions)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return UsersDataLayer.GetInstance().GetTotalRecords(connection, conditions);
            }
        }

        public UserInfo GetDetail(string id)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            UserInfo record = new UserInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = UsersDataLayer.GetInstance().GetUser(connection, id);

                return record;
            }
        }

        public void CreateUser(UserInfo _user)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            UserInfo record = new UserInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                string applicationId = EncryptionUtils.GetApplicationId();
                string applicationName = EncryptionUtils.GetApplicationName();
                _user.Password = EncryptionUtils.EncryptData(_user.Password, applicationName, applicationId);
                UsersDataLayer.GetInstance().CreateUser(connection, _user);
            }
        }
        public void UpdateUser(string id, UserInfo _user)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            UserInfo record = new UserInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                    UserInfo data =  UsersDataLayer.GetInstance().GetUser(connection,id);
                if(data.Password != _user.Password)
                {
                    string applicationId = EncryptionUtils.GetApplicationId();
                    string applicationName = EncryptionUtils.GetApplicationName();

                    _user.Password = EncryptionUtils.EncryptData(_user.Password, applicationName, applicationId);
                }
                UsersDataLayer.GetInstance().UpdateUser(connection, id , _user);
            }
        }
        public void DeleteUser(string id)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
 
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                UsersDataLayer.GetInstance().DeleteUser(connection, id);
                UserGroupDataLayer.GetInstance().DeleteGroupOfUser(connection, id);
            }
        }
        public void DeleteUsers(string ids)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
          
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                string[] IDsarray = ids.Split(',');
                foreach (string id in IDsarray)
                {
                    UsersDataLayer.GetInstance().DeleteUser(connection, id);
                }
            }
        }

        public void ChangePassword(string _user, string pass)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            UserInfo record = new UserInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                string applicationId = EncryptionUtils.GetApplicationId();
                string applicationName = EncryptionUtils.GetApplicationName();
                String password = EncryptionUtils.EncryptData(pass, applicationName, applicationId);
                UsersDataLayer.GetInstance().ChangePass(connection, _user , password);
            }
        }
    }
}
