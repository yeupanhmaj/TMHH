using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using EncryptionLibrary;
using Microsoft.VisualBasic;

namespace AdminPortal.DataLayer
{
    public class ConnectString
    {
        /*private static SqlConnection cnn;
        private static string strUser = LabsoftEncryption.EnDeScript.DeScriptPass("w|xl{y}/02QacOO");
        private static string strPassword = LabsoftEncryption.EnDeScript.DeScriptPass("npvv~pqy5WOKKc");
        /// <summary>
        /// Hàm tạo chuổi kết nối cơ sở dữ liệu SQL Server
        /// </summary>
        /// <returns>OdbcConnection trả về</returns>
        public static SqlConnection GetConnection()
        {
            string _serverName = ConfigurationManager.AppSettings["ServerName"].ToString();
            string _dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            //ODBC = LabconnBB (Labconn Blood bank)
            cnn = new SqlConnection("server=" + _serverName + ";database=" + _dbName + ";uid=" + Strings.RTrim(strUser) + ";pwd=" + Strings.RTrim(strPassword) + ";");
            if (cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }
            cnn.Open();
            return cnn;
        }*/
    }
}
