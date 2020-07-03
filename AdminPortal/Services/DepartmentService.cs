using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Models.Common;

namespace AdminPortal.Services
{
    public class DepartmentService : BaseService<DepartmentService>
    {
        public int getNewId()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<DepartmentInfo> ListDepartment = DepartmentDataLayer.GetInstance().GetAllDepartment(connection);
                DepartmentInfo lastDepartment = ListDepartment.Last();
                if (lastDepartment != null) return (lastDepartment.DepartmentID + 1);
                return 1;
            }
        }
        public int getTotalRecords(List<DepartmentInfo> _listDepartment)
        {
            return _listDepartment.Count;

        }

       

        public List <DepartmentInfo> getAllDepartment(int pageSize,int pageIndex)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<DepartmentInfo> ListDepartment = DepartmentDataLayer.GetInstance().GetAllDepartment(connection);
                //int min = pageIndex * pageSize;
                //int max = pageIndex * pageSize + pageSize;

                //if (min > ListDepartment.Count) return new List<DepartmentInfo>();
                //if (max >= ListDepartment.Count) pageSize = ListDepartment.Count - min;
                //if (pageSize <= 0) return new List<DepartmentInfo>();
                return ListDepartment;
            }
            //using (SqlConnection connection = sqlConnection.GetConnection())
            //{
            //    List<DepartmentInfo> ListDepartment = DepartmentDataLayer.GetInstance().GetAllDepartment(connection);
            //    int min = pageIndex * pageSize;
            //    int max = pageIndex * pageSize + pageSize;

            //    if (min > ListDepartment.Count) return new List<DepartmentInfo>();
            //    if (max >= ListDepartment.Count) pageSize = ListDepartment.Count - min;
            //    if (pageSize <= 0) return new List<DepartmentInfo>();
            //    return ListDepartment.GetRange(min, pageSize);
            //}
        }

        public DepartmentInfo getDepartmentbyId(int _ID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<DepartmentInfo> ListDepartment = DepartmentDataLayer.GetInstance().GetAllDepartment(connection);
                DepartmentInfo findDepartment = ListDepartment.Where(i => i.DepartmentID == _ID).First();
                return findDepartment;
            }
        }

        public List<DepartmentInfo> getDepartment(DepartmentSeachCriteria _criteria)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<DepartmentInfo> ListDepartment = DepartmentDataLayer.GetInstance().getDepartment(connection, _criteria);
                return ListDepartment;
            }
        }

        public ActionMessage createDepartment(DepartmentInfo _department, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            if (_department.DepartmentCode.Trim() == string.Empty)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập Khoa phòng";
            }
            //else if (_department.SourceID == -1)
            //{
            //}
            else
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    DepartmentSeachCriteria _criteria = new DepartmentSeachCriteria();
                    _criteria.DepartmentCode = _department.DepartmentCode;
                    var chkDepartmentInfo = getDepartment(_criteria);
                    if (chkDepartmentInfo.Count > 0)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString = "Trùng mã Khoa phòng";
                    }
                    else
                    {
                        try
                        {
                            DepartmentDataLayer.GetInstance().InsertDepartment(connection, _department, _userI);
                            ret.isSuccess = true;
                        }
                        catch(Exception ex)
                        {
                            ret.isSuccess = false;
                            ret.err.msgCode = "Internal Error";
                            ret.err.msgString =  ex.ToString();
                        }
                    }
                    
                }
            }
            return ret;
        }
        public ActionMessage editDepartment(int id, DepartmentInfo _department, string _userU)
        {
            ActionMessage ret = new ActionMessage();
            if (_department.DepartmentCode.Trim().Length == 0)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập mã Khoa phòng";
            }
            else
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    DepartmentSeachCriteria _criteria = new DepartmentSeachCriteria();
                    _criteria.DepartmentCode = _department.DepartmentCode;
                    var chkDepartmentInfo = getDepartment(_criteria);
                    if (chkDepartmentInfo.Count > 0)
                    {
                        
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString = "Trùng mã Khoa phòng";
                    }
                    else
                    {
                        try
                        {
                            DepartmentDataLayer.GetInstance().UpdateDepartment(connection, _department, _userU);
                            ret.isSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            ret.isSuccess = false;
                            ret.err.msgCode = "Internal Error";
                            ret.err.msgString =  ex.ToString();
                        }
                    }

                }
            }
            return ret;
        }

        public ActionMessage deleteDepartment(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    DepartmentDataLayer.GetInstance().DeleteDepartment(connection, id);
                    ret.isSuccess = true;
                }
                catch (Exception ex)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "Internal Error";
                    ret.err.msgString =  ex.ToString();
                }
            }
            return ret;
        }
    }
}
