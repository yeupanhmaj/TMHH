using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Commons;
using EncryptionLibrary;
using AdminPortal.Helpers;

namespace AdminPortal.DataLayer
{
    public class UsersDataLayer : BaseLayerData<UsersDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả user trừ user admin, mdx, config
        /// </summary>
        /// <returns>Return DataTable</returns>
        /// 
        public List<UserInfo> Getlist(SqlConnection connection, UserCriteria criteria)
        {
            var result = new List<UserInfo>();
            using (var command = new SqlCommand("Select U.*  " +
                " from tbl_User U " +
                " where   1=1 and U.UserID <> 'admin' ", connection))
            {

                if (criteria.UserName != "" && criteria.UserName != null)
                {
                    command.CommandText += " and U.UserName like  '%" + criteria.UserName + "%' ";
                }

                if (criteria.UserID != "" && criteria.UserID != null)
                {
                    command.CommandText += " and U.UserID like  '%" + criteria.UserID + "%' ";
                }


                if (criteria.pageSize == 0) criteria.pageSize = 10;
                var offSet = criteria.pageIndex * criteria.pageSize;
                command.CommandText += " order by U.UserName ";
                command.CommandText += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY ";
                AddSqlParameter(command, "@OFFSET", offSet, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@PAGESIZE", criteria.pageSize, System.Data.SqlDbType.Int);
              

                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new UserInfo();
                        info.UserName = GetDbReaderValue<string>(reader["UserName"]);
                        info.UserID = GetDbReaderValue<string>(reader["UserID"]);
                  //      info.Disable = GetDbReaderValue<byte>(reader["GroupID"]);
                        info.Email = GetDbReaderValue<string>(reader["Email"]);
                        info.Disable = GetDbReaderValue<Boolean>(reader["Disable"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }

        public int GetTotalRecords(SqlConnection connection, UserCriteria criteria)
        {
            if (criteria != null)
            {
                var result = new List<UserInfo>();
                using (var command = new SqlCommand("Select count(*)  as TotalRecords  " +
                    " from tbl_User U " +
                    " where   1=1  and U.UserID <> 'admin' ", connection))
                {

                    if (criteria.UserName != "" && criteria.UserName != null)
                    {
                        command.CommandText += " and U.UserName like  '%" + criteria.UserName + "%' ";
                    }



                    if (criteria.UserID != "" && criteria.UserID != null)
                    {
                        command.CommandText += " and U.UserID = @UserID ";
                        AddSqlParameter(command, "@UserID", criteria.UserID, System.Data.SqlDbType.VarChar);
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
                using (var command = new SqlCommand("Select count(*) as TotalRecords  from tbl_User where 1 = 1 ", connection))
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
        public DataTable GetUser(SqlConnection connection)
        {
            string strSQL = "Select rtrim(UserID) as UserID,UserName, Email, Password, Disable, ExportDetail  " +
                " from tbl_user where  UserID <> 'admin' and UserID <> 'mdx' and UserID <> 'config'";

            DataTable dt = db.ExcuteDataSet(connection, strSQL).Tables[0];
            return dt;
        }
        public void CreateUser(SqlConnection connection, UserInfo user)
        {
            using (var command = new SqlCommand("Insert into tbl_User " +
                 " (UserID ,UserName, Password, GroupID, Email, Disable , ExportDetail)" +
                     "VALUES " +
                     "(@UserID,@UserName,@Password, @GroupID, @Email, @Disable, @ExportDetail ) ", connection))
            {
                AddSqlParameter(command, "@UserID", user.UserID, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserName", user.UserName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Password", user.Password, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@GroupID", user.GroupID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Email", user.Email, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Disable", false, System.Data.SqlDbType.Bit) ;
                AddSqlParameter(command, "@ExportDetail", false, System.Data.SqlDbType.Bit);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();            
            }

        }
        public void UpdateUser(SqlConnection connection ,string id ,  UserInfo user)
        {
            using (var command = new SqlCommand("update  tbl_User " +
                 " SET  UserName=@UserName, GroupID=@GroupID, Password=@Password, Email=@Email, Disable=@Disable , ExportDetail=@ExportDetail " +
                 "where UserID=@UserId and UserID <> 'admin' "
                 , connection))
            {
                AddSqlParameter(command, "@UserID", user.UserID, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserName", user.UserName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Password", user.Password, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@GroupID", user.GroupID, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Email", user.Email, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Disable", user.Disable, System.Data.SqlDbType.Bit);
                AddSqlParameter(command, "@ExportDetail", false, System.Data.SqlDbType.Bit);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }

        public void DeleteUser(SqlConnection connection, string id)
        {
            using (var command = new SqlCommand("delete  tbl_User " +
                 " where  UserID=@UserID "
                 , connection))
            {
                AddSqlParameter(command, "@UserID", id, System.Data.SqlDbType.NVarChar);
           
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
        /// <summary>
        /// Hàm lấy tất cả user trừ user admin, mdx, config
        /// </summary>
        /// <returns>Return DataTable</returns>
        /// 

        public UserInfo GetUser(SqlConnection connection, string strUser)
        {
  
            var info = new UserInfo();
            using (var command = new SqlCommand("Select rtrim(UserID) as UserID,UserName, Email, Password, Disable, ExportDetail  " +
                " from tbl_user  where userid=@userid  and UserID <> 'admin' "   , connection))
            {
                AddSqlParameter(command, "@userid", strUser, System.Data.SqlDbType.VarChar);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    info.Password = GetDbReaderValue<string>(reader["Password"]);
                    info.UserName = GetDbReaderValue<string>(reader["UserName"]);
                    info.UserID = GetDbReaderValue<string>(reader["UserID"]);
                    info.Disable = GetDbReaderValue<Boolean>(reader["Disable"]);
                    info.Email = GetDbReaderValue<string>(reader["Email"]);
                }
                return info;
            }
        }

        public DataTable GetUserByUserId(SqlConnection connection, string strUser)
        {
            string strSQL = "Select rtrim(UserID) as UserID,UserName, Email, Password, Disable, ExportDetail  " +
                " from tbl_user  where userid=@userid   ";
            DataTable dt = db.GetDataTable(connection, strSQL,
                CommandType.Text,
                new string[1] { "userid" },
                new string[1] { strUser });
            return dt;
        }


        public String GetRoleByUserId(SqlConnection connection, string UserID)
        {
            var ret = "";
            using (var command = new SqlCommand("Select UGP.Feature " +
                " from tbl_user U "  +
                " inner join tbl_User_Group_User UGU " +
                " on U.UserID = UGU.UserID " +
                " inner join tbl_UserGroupPermission  UGP " +
                " on UGP.UserGroupID = UGU.GroupID " +
                " where U.UserID=@userid  ", connection))
            {
                AddSqlParameter(command, "@userid", UserID, System.Data.SqlDbType.VarChar);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int role = GetDbReaderValue<int>(reader["Feature"]);

                    ret += HardData.ROLE_HARD[role-1] + "," ;
                }
                return ret;
            }
        }

        

        /// <summary>
        /// Hàm lấy tất cả user
        /// </summary>
        /// <returns>Return DataTable</returns>
        public DataTable GetUserAll(SqlConnection connection)
        {

            string strSQL = "Select rtrim(UserID) as UserID,UserName,Password from tbl_user where UserID <> 'admin'";

            db = new DataProvider();
            DataTable dt = db.ExcuteDataSet(connection, strSQL).Tables[0];
            return dt;
        }
        /// <summary>
        /// Hàm thay đổi mật khẩu user
        /// </summary>
        /// <param name="strUserID">User Name</param>
        /// <param name="strNewPassword">New Password</param>
        public void ChangePassword(SqlConnection connection, string strUserID, string strNewPassword)
        {
            string applicationId = EncryptionUtils.GetApplicationId();
            string applicationName = EncryptionUtils.GetApplicationName();

            string strPasswordConvert = EncryptionUtils.EncryptData(strNewPassword, applicationName, applicationId);
            string strSQL = "Update tbl_user set Password=@password where UserID=@userid";

            db.ExecuteSqlQuery(connection, strSQL,
                CommandType.Text,
                new string[2] { "password", "userid" },
                new string[2] { strPasswordConvert, strUserID });
        }
        /// <summary>
        /// Hàm lấy password của user
        /// </summary>
        /// <param name="strUser">User name</param>
        /// <returns>Return password</returns>
        public DataTable Get_UserPassword(SqlConnection connection, string strUser)
        {
            string strUserConvert = Utils.ConvertString(strUser);
            string strSQL = "select userid,password from tbl_user where userid=@userid and Disable=0 ";
            DataTable dt = db.GetDataTable(connection, strSQL,
                CommandType.Text,
                new string[1] { "userid" },
                new string[1] { strUser });
            return dt;
        }
        /// <summary>
        /// Hàm lấy dữ liệu các chức năng của user
        /// </summary>
        /// <returns>Return DataTable</returns>
        public DataTable Get_Function(SqlConnection connection)
        {
            string strSQL = "Select Rtrim(FunctionID) as FunctionID,Rtrim(Description) as Description from tbl_function order by Description";
            DataTable dt = db.ExcuteDataSet(connection, strSQL).Tables[0];
            return dt;
        }

        public void TrackingUserBehavior(SqlConnection connection, UserLogInfo info)
        {
            using (var command = new SqlCommand("Insert into tbl_User_Tracking " +
                " (UserID,UserName, Action, Description, Feature, Time )" +
                    "VALUES (@UserID,@UserName, @Action, @Description, @Feature, @Time)"
                   , connection))
            {
                AddSqlParameter(command, "@UserID", info.UserID, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@UserName", info.UserName, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Action", info.Action, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@Description", info.Description, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Feature", info.Feature, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@Time", info.Time, System.Data.SqlDbType.DateTime);

                WriteLogExecutingCommand(command);

                command.ExecuteScalar();

            }
        }
        public void ChangePass(SqlConnection connection, string userI , string password)
        {
            using (var command = new SqlCommand("update  tbl_User " +
                " set Password = @Password where UserID= @UserID"
                , connection))
            {
                AddSqlParameter(command, "@UserID", userI, System.Data.SqlDbType.NVarChar);         
                AddSqlParameter(command, "@Password", password, System.Data.SqlDbType.NVarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

        }
    }
}
