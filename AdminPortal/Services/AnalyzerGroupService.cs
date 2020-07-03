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
    public class AnalyzerGroupService : BaseService<AnalyzerGroupService>
    {

        public int getTotalRecords(AnalyzerGroupSeachCriteria _criteria)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return AnalyzerGroupDataLayer.GetInstance().getTotalRecords(connection, _criteria);
            }
        }

        public List <AnalyzerGroupInfo> getAllAnalyzerGroup(AnalyzerGroupSeachCriteria _criteria)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return AnalyzerGroupDataLayer.GetInstance().GetAllAnalyzerGroup(connection, _criteria);
            }
        }

        public AnalyzerGroupInfo getAnalyzerGroup(int _ID)
        {
            AnalyzerGroupInfo record = new AnalyzerGroupInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = AnalyzerGroupDataLayer.GetInstance().getAnalyzerGroup(connection, _ID);
                if (record == null)
                {
                    return null;
                }
                return record;
            }          
        }


        public AnalyzerGroupInfo GetAnalyzerGroupByCode(string code)
        {
            AnalyzerGroupInfo record = new AnalyzerGroupInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                record = AnalyzerGroupDataLayer.GetInstance().GetAnalyzerGroupByCode(connection, code);
                if (record == null)
                {
                    return null;
                }

                return record;
            }
        }


        public ActionMessage createAnalyzerGroup(AnalyzerGroupInfo _AnalyzerGroup, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    ret.id = AnalyzerGroupDataLayer.GetInstance().InsertAnalyzerGroup(connection, _AnalyzerGroup, _userI);
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

        public async Task<ActionMessage> createAnalyzerGroup2(AnalyzerGroupInfo _AnalyzerGroup, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                   
                    insetId = AnalyzerGroupDataLayer.GetInstance().InsertAnalyzerGroup(connection, _AnalyzerGroup, _userI);
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
                    ret.err.msgCode = "lỗi thêm nhóm tài sản";
                    ret.err.msgString = "lỗi thêm nhóm tài sản";
                }
                return ret;

            }
        }

        public async Task<ActionMessage> editAnalyzerGroup(int id, AnalyzerGroupInfo _AnalyzerGroup, string _userU)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                var chkAnalyzerGroupInfo = AnalyzerGroupDataLayer.GetInstance().getAnalyzerGroup(connection, id);
                if (chkAnalyzerGroupInfo != null)
                {
                    try
                    {
                        AnalyzerGroupDataLayer.GetInstance().UpdateAnalyzerGroup(connection, id, _AnalyzerGroup, _userU);
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
                    AnalyzerGroupDataLayer.GetInstance().Delete(connection, id);
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
                    AnalyzerGroupDataLayer.GetInstance().DeleteMuti(connection, ids);
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

        public List<string> getListAnalyzerGroupCode(string AnalyzerGroupCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<string> ListAnalyzerGroup = AnalyzerGroupDataLayer.GetInstance().getListAnalyzerGroupCode(connection, AnalyzerGroupCode);
                return ListAnalyzerGroup;
            }
        }

    }
}
