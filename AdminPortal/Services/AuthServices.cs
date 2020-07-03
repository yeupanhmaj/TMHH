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

namespace AdminPortal.Services
{
    public class AuthServices : BaseService<AuthServices>
    {
        public AuthenticateModelRespone SignIn(string _UserID, string _Password)
        {
            AuthenticateModelRespone ret =  new AuthenticateModelRespone();
            ret.isSuccess = false;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                DataTable dtLogin = UsersDataLayer.GetInstance().Get_UserPassword(connection, _UserID);
             
                if (dtLogin.Rows.Count > 0)
                {
                    var strPass = dtLogin.Rows[0][1].ToString();

                    //Have a Password
                    if (strPass.Length > 0)
                    {
                        string applicationId = EncryptionUtils.GetApplicationId();
                        string applicationName = EncryptionUtils.GetApplicationName();

                        strPass = EncryptionUtils.Decrypt(strPass, applicationName, applicationId).Trim();

                        //Corect Password
                        if (strPass.Equals(_Password))
                        {
                            DataTable dtLogin2 = UsersDataLayer.GetInstance().GetUserByUserId(connection, _UserID);
                            ret.isSuccess = true;
                            ret.name = dtLogin2.Rows[0][1].ToString();
                            ret.userID = dtLogin2.Rows[0][0].ToString();

                            ret.role = UsersDataLayer.GetInstance().GetRoleByUserId(connection, _UserID);


                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(SerectContext.Secret);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                    new Claim(ClaimTypes.NameIdentifier, _UserID)
                                }),
                                Expires = DateTime.UtcNow.AddDays(7),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            ret.token = tokenHandler.WriteToken(token);
                            return ret;
                        }
                    }
                }
                ret.err.msgCode = "001";
                ret.err.msgString = "Sai thông tin password và acccount";
                return ret;
            }
        }

    }
}
