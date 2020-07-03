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
    public class UserGroupService : BaseService<UserGroupService>
    {

        public List<UserGroupInfo> GetList(UserGroupCriteria conditions)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<UserGroupInfo> ListUserInfo = UserGroupDataLayer.GetInstance().Getlist(connection, conditions);
                return ListUserInfo;
            }
        }

        public int getTotalRecords(UserGroupCriteria conditions)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return UserGroupDataLayer.GetInstance().GetTotalRecords(connection, conditions);
            }
        }

        public UserGroupInfo GetDetail(int id)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            UserGroupInfo record = new UserGroupInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = UserGroupDataLayer.GetInstance().GetUserGroup(connection, id);

                return record;
            }
        }

        public void CreateUserGroup(UserGroupInfo _userGroup)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            UserGroupInfo record = new UserGroupInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                UserGroupDataLayer.GetInstance().CreateUserGroup(connection, _userGroup);
            }
        }
        public void UpdateUserGroup(string id, UserGroupInfo _userGroup)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            UserInfo record = new UserInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                UserInfo data = UsersDataLayer.GetInstance().GetUser(connection, id);
                UserGroupDataLayer.GetInstance().UpdateUserGroup(connection, id, _userGroup);
            }
        }
        public void DeleteUserGroup(string id)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                UserGroupDataLayer.GetInstance().DeleteGroupPermission(connection, Int32.Parse(id));
                UserGroupDataLayer.GetInstance().DeleteUserGroup(connection, id);
                UserGroupDataLayer.GetInstance().DeleteUserGroupUserByGroupID(connection, id);
            }
        }
        public void DeleteUserGroups(string ids)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                string[] IDsarray = ids.Split(',');
                foreach (string id in IDsarray)
                {
                    UserGroupDataLayer.GetInstance().DeleteGroupPermission(connection, Int32.Parse(id));
                    UserGroupDataLayer.GetInstance().DeleteUserGroup(connection, id);
                }
            }
        }

        public List<UserGroupPermission> GetListGroupPermission(int GroupID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return UserGroupDataLayer.GetInstance().GetListGroupPermission(connection, GroupID);
            }
        }

        public void UpdateListPermission(int GroupID, List<UserGroupPermission> groupPermissions)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                UserGroupDataLayer.GetInstance().DeleteGroupPermission(connection , GroupID);
                foreach (UserGroupPermission item in groupPermissions){
                    UserGroupDataLayer.GetInstance().InsertGroupPermission(connection, GroupID , item.Feature);
                }
            }
        }

        public List<UserGroupInfo> GetListGroupOfUser(String userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<UserGroupInfo> ListUserInfo = UserGroupDataLayer.GetInstance().GetListGroupOfUser(connection, userID);
                return ListUserInfo;
            }
        }
        public void UpdateGroupOfUser(String userID, List<UserGroupUser> lstGroup)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                UserGroupDataLayer.GetInstance().DeleteGroupOfUser(connection, userID);
                foreach (UserGroupUser item in lstGroup)
                {
                    UserGroupDataLayer.GetInstance().InsertGroupOfUser(connection, userID, item.GroupID);
                }
            }
        }
    }
}
