using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.DataLayers.Common;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class UserGroupDataLayer : BaseLayerData<UserGroupDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả user trừ user admin, mdx, config
        /// </summary>
        /// <returns>Return DataTable</returns>
        /// 
        public List<UserGroupInfo> Getlist(SqlConnection connection, UserGroupCriteria criteria)
        {
            var result = new List<UserGroupInfo>();
            using (var command = new SqlCommand("Select UG.*  " +
                " from tbl_User_Group UG " +
                " where   1=1  ", connection))
            {

                if (criteria.GroupName != "" && criteria.GroupName != null)
                {
                    command.CommandText += " and UG.Name like  '%" + criteria.GroupName + "%' ";
                }

                if (criteria.pageSize == 0) criteria.pageSize = 10;
                var offSet = criteria.pageIndex * criteria.pageSize;
                command.CommandText += " order by UG.Name ";
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", criteria.pageSize, System.Data.SqlDbType.Int);
              

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new UserGroupInfo();
                        info.GroupID = GetDbReaderValue<int>(reader["ID"]);
                        info.GroupName = GetDbReaderValue<string>(reader["Name"]);
                      

                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int GetTotalRecords(SqlConnection connection, UserGroupCriteria criteria)
        {
            if (criteria != null)
            {
                var result = new List<UserGroupInfo>();
                using (var command = new SqlCommand("Select count(*)  as TotalRecords  " +
                   " from tbl_User_Group UG " +
                    " where   1=1 ", connection))
                {

                    if (criteria.GroupName != "" && criteria.GroupName != null)
                    {
                        command.CommandText += " and UG.Name like  '%" + criteria.GroupName + "%' ";
                    }

           
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
                using (var command = new SqlCommand("Select count(*) as TotalRecords  from tbl_User_Group where 1 = 1 ", connection))
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
       
        public void CreateUserGroup(SqlConnection connection, UserGroupInfo userGroup)
        {
            using (var command = new SqlCommand("Insert into tbl_User_Group " +
                 " (Name)" +
                     "VALUES " +
                     "(@GroupName) ", connection))
            {
                AddSqlParameter(command, "@GroupName", userGroup.GroupName, System.Data.SqlDbType.NVarChar);
              
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();            
            }

        }
        public void UpdateUserGroup(SqlConnection connection ,string id , UserGroupInfo userGroup)
        {
            using (var command = new SqlCommand("update  tbl_User_Group " +
                 " SET  Name=@GroupName, " +
                 "where ID=@GroupID "
                 , connection))
            {
                AddSqlParameter(command, "@GroupID", userGroup.GroupID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@GroupName", userGroup.GroupName, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteUserGroup(SqlConnection connection, string id)
        {
            using (var command = new SqlCommand("delete  tbl_User_Group " +
                 " where  ID=@GroupID "
                 , connection))
            {
                AddSqlParameter(command, "@GroupID", id, System.Data.SqlDbType.Int);
           
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


        public void DeleteUserGroupUserByGroupID(SqlConnection connection, string id)
        {
            using (var command = new SqlCommand("delete  tbl_User_Group_User " +
                 " where  ID=@GroupID "
                 , connection))
            {
                AddSqlParameter(command, "@GroupID", id, System.Data.SqlDbType.Int);

                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        /// <summary>
        /// Hàm lấy tất cả user trừ user admin, mdx, config
        /// </summary>
        /// <returns>Return DataTable</returns>
        /// 
        public UserGroupInfo GetUserGroup(SqlConnection connection, int userGroupID)
        {
            var info = new UserGroupInfo();
            using (var command = new SqlCommand("Select *  " +
                " from tbl_User_Group  where ID=@GroupID  " , connection))
            {
                AddSqlParameter(command, "@GroupID", userGroupID, System.Data.SqlDbType.Int);
                var reader = command.ExecuteReader();
                reader.Read();
                info.GroupID = GetDbReaderValue<int>(reader["ID"]);
                info.GroupName = GetDbReaderValue<string>(reader["Name"]);
                return info;
            }
        }

        public List<UserGroupPermission> GetListGroupPermission(SqlConnection connection, int GroupID)
        {
            var result = new List<UserGroupPermission>();
            using (var command = new SqlCommand("Select UGP.*  " +
                " from tbl_UserGroupPermission UGP " +
                " where   UserGroupID=@GroupID  ", connection))
            {
                AddSqlParameter(command, "@GroupID", GroupID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new UserGroupPermission();
                        info.GroupID = GetDbReaderValue<int>(reader["UserGroupID"]);
                        info.Feature = GetDbReaderValue<int>(reader["Feature"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public void DeleteGroupPermission(SqlConnection connection, int GroupID)
        {
            using (var command = new SqlCommand(
                " delete  tbl_UserGroupPermission " +
                " where   UserGroupID=@UserGroupID  ", connection))
            {
                AddSqlParameter(command, "@UserGroupID", GroupID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        public void InsertGroupPermission(SqlConnection connection, int groupID , int feature)
        {
            using (var command = new SqlCommand("Insert into tbl_UserGroupPermission " +
                 " (UserGroupID, Feature )" +
                     "VALUES " +
                     "(@UserGroupID, @Feature) ", connection))
            {
                AddSqlParameter(command, "@UserGroupID", groupID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@Feature", feature, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public List<UserGroupInfo> GetListGroupOfUser(SqlConnection connection, String userId)
        {
            var result = new List<UserGroupInfo>();
            using (var command = new SqlCommand("Select UG.ID , UG.Name   " +
                " from tbl_User_Group_User UGU " +
                " inner join tbl_User_Group UG " +
                " on UGU.GroupID = UG.ID" +
                " where   UGU.UserID=@UserID  ", connection))
            {                
                AddSqlParameter(command, "@UserID", userId, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new UserGroupInfo();
                        info.GroupID = GetDbReaderValue<int>(reader["ID"]);
                        info.GroupName = GetDbReaderValue<string>(reader["Name"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public void DeleteGroupOfUser(SqlConnection connection, String userId)
        {
            using (var command = new SqlCommand(
                " delete  tbl_User_Group_User " +
                " where   UserID=@UserID  ", connection))
            {
                AddSqlParameter(command, "@UserID", userId, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void InsertGroupOfUser(SqlConnection connection, String userId , int GroupID)
        {
            using (var command = new SqlCommand("Insert into tbl_User_Group_User " +
                 " (GroupID, UserID )" +
                     "VALUES " +
                     "(@GroupID, @UserID) ", connection))
            {
                AddSqlParameter(command, "@GroupID", GroupID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserID", userId , System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }


    }
}
