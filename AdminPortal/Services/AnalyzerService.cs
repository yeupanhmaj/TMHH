using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Models.Common;
using Microsoft.AspNetCore.Http;

namespace AdminPortal.Services
{
    public class AnalyzerService : BaseService<AnalyzerService>
    {

        public int getTotalRecords(AnalyzerSeachCriteria _criteria)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return AnalyzerDataLayer.GetInstance().getTotalRecords(connection, _criteria);
            }
        }

        public List <AnalyzerInfo> getAllAnalyzer(int pageSize,int pageIndex)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                List<AnalyzerInfo> ListAnalyzer = AnalyzerDataLayer.GetInstance().GetAllAnalyzer(connection);
                int min = pageIndex * pageSize;
                int max = pageIndex * pageSize + pageSize;

                if (min > ListAnalyzer.Count) return new List<AnalyzerInfo>();
                if (max >= ListAnalyzer.Count) pageSize = ListAnalyzer.Count - min;
                if (pageSize <= 0) return new List<AnalyzerInfo>();
                return ListAnalyzer.GetRange(min, pageSize);
            }
        }

        public List<AnalyzerInfo> getAllAnalyzer(int pageSize, int pageIndex, AnalyzerSeachCriteria _criteria)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {

                List<AnalyzerInfo> ListAnalyzer = AnalyzerDataLayer.GetInstance().getAnalyzer(connection, _criteria);
                int min = pageIndex * pageSize;
                int max = pageIndex * pageSize + pageSize;

                if (min > ListAnalyzer.Count) return new List<AnalyzerInfo>();
                if (max >= ListAnalyzer.Count) pageSize = ListAnalyzer.Count - min;
                if (pageSize <= 0) return new List<AnalyzerInfo>();
                return ListAnalyzer.GetRange(min, pageSize);
            }
        }

        public AnalyzerInfo getAnalyzer(int _ID)
        {
            AnalyzerInfo record = new AnalyzerInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = AnalyzerDataLayer.GetInstance().getAnalyzer(connection, _ID);
                if (record == null)
                {
                    return null;
                }
                return record;
            }          
        }


        public AnalyzerInfo GetAnalyzerByCode(string code)
        {
            AnalyzerInfo record = new AnalyzerInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = AnalyzerDataLayer.GetInstance().GetAnalyzerByCode(connection, code);
                if (record == null)
                {
                    return null;
                }

                return record;
            }
        }


        public ActionMessage createAnalyzer(AnalyzerInfo _Analyzer, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //ret.id = AnalyzerDataLayer.GetInstance().InsertAnalyzer(connection, _Analyzer, _userI);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "123";
                    ret.err.msgString = ex.Message;
                }
            }
            return ret;
        }

        public async Task<ActionMessage> createAnalyzer2(AnalyzerInfo _Analyzer, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //insetId = AnalyzerDataLayer.GetInstance().InsertAnalyzer(connection, _Analyzer, _userI);
                    //TODO: insert member
                    // ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (insetId > -1)
                {
                    ret.id = insetId;
                    ret.isSuccess = true;
                }
                else
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "lỗi thêm tài sản";
                    ret.err.msgString = "lỗi thêm tài sản";
                }
                return ret;

            }
        }

        public async Task<ActionMessage> editAnalyzer(int id, AnalyzerInfo _Analyzer, string _userU)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                var chkAnalyzerInfo = AnalyzerDataLayer.GetInstance().getAnalyzer(connection, id);
                if (chkAnalyzerInfo != null)
                {
                    try
                    {
                        AnalyzerDataLayer.GetInstance().UpdateAnalyzer(connection, id, _Analyzer, _userU);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                ret.isSuccess = true;
                return ret;
            }
        }

        public ActionMessage Delete(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete record
                    AnalyzerDataLayer.GetInstance().Delete(connection, id);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "Internal Error";
                    ret.err.msgString = ex.ToString();
                }
            }
            return ret;
        }

        public ActionMessage DeleteMuti(string ids)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    AnalyzerDataLayer.GetInstance().DeleteMuti(connection, ids);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "Internal Error";
                    ret.err.msgString = ex.Message;
                }
            }
            return ret;
        }

        public List<string> getListAnalyzerCode(string AnalyzerCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<string> ListAnalyzer = AnalyzerDataLayer.GetInstance().getListAnalyzerCode(connection, AnalyzerCode);
                return ListAnalyzer;
            }
        }
        public List<AnalyzerInfo> GetAnalyzerByDate(AnalyzerSeachCriteria date)
        {
            List<AnalyzerInfo> record = new List<AnalyzerInfo>();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = AnalyzerDataLayer.GetInstance().GetAnalyzerByDate(connection, date);
                if (record == null)
                {
                    return null;
                }

                return record;
            }
        }

    }
}
