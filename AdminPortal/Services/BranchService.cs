using AdminPortal.DataLayers;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Services
{
    public class BranchService:BaseService<BranchService>
    {
        public List<BranchInfo> search(string query, int page, int size)
        {
            List<BranchInfo> result = new List<BranchInfo>();
            int position = (page - 1) * size;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                result = BranchDataLayer.GetInstance().Search(connection,query,position,size);
            }
            return result;
        }
        public int totalRecordSearch(string query)
        {
            List<BranchInfo> result = new List<BranchInfo>();
            int total = 0;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                total = BranchDataLayer.GetInstance().totalRecordSearch(connection, query);
            }
            return total;
        }
        public BranchInfo getBranchById(int branchId)
        {
            BranchInfo result = new BranchInfo();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                result = BranchDataLayer.GetInstance().GetBranchById(connection, branchId);
            }
            return result;
        }
        public List<BranchInfo> getAllBranch(int page,int size)
        {
            List<BranchInfo> result = new List<BranchInfo>();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            int position = (page - 1) * size;
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                result = BranchDataLayer.GetInstance().GetAllBranch(connection,position,size);
            }
            return result;
        }
        public int GetTotalRecordBranch()
        {
            int total = 0;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                total = BranchDataLayer.GetInstance().GetTatalRecordBranch(connection);
            }
            return total;
        }
        public ActionMessage createBranch(BranchInfo branch)
        {
            ActionMessage ret = new ActionMessage();
            if (string.IsNullOrEmpty(branch.BranchName.Trim()))
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập Tên chi nhánh";
            }
            else if (string.IsNullOrEmpty(branch.BranchAddress.Trim()))
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập Địa chỉ";
            }
            else
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        BranchDataLayer.GetInstance().InsertBranch(connection, branch);
                        ret.isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString = ex.ToString();
                    }
                }
            }
            return ret;
        }

        public ActionMessage editBranch(BranchInfo branch)
        {
            ActionMessage ret = new ActionMessage();
            if (string.IsNullOrEmpty(branch.BranchName.Trim()))
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập Tên chi nhánh";
            }
            else if (string.IsNullOrEmpty(branch.BranchAddress.Trim()))
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập Địa chỉ";
            }
            else
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        BranchInfo branchInfo = BranchDataLayer.GetInstance().GetBranchById(connection, branch.BranchID);
                        if (branchInfo == null)
                        {
                            ret.isSuccess = false;
                            ret.err.msgCode = "Chi nhánh này không tồn tại";
                            return ret;
                        }
                        BranchDataLayer.GetInstance().UpdateBranch(connection, branch);
                        ret.isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString = ex.ToString();
                    }
                }
            }
            return ret;
        }

        public ActionMessage deleteBranch(int branchId)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    BranchInfo branchInfo = BranchDataLayer.GetInstance().GetBranchById(connection, branchId);
                    if (branchInfo == null)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "Chi nhánh này không tồn tại";
                        return ret;
                    }
                    BranchDataLayer.GetInstance().DeleteBranch(connection, branchId);
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

        public ActionMessage deleteManyBranch(string ids)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    BranchDataLayer.GetInstance().DeleteManyBranch(connection, ids);
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
    }
}
