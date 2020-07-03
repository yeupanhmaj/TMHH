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
    public class EmployeeServices : BaseService<EmployeeServices>
    {

        public List<EmployeeInfo> GetEmployeesByCondition(string name)
        {
            try
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    List<EmployeeInfo> ret = EmployeeDataLayer.GetInstance().GetEmployeeByCondition(connection, name);
                     return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeInfo> getAllEmployee()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<EmployeeInfo> ListEmployee = EmployeeDataLayer.GetInstance().GetAllEmployee(connection);

                return ListEmployee;
            }
        }

        public EmployeeInfo getEmployeebyId(int _ID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<EmployeeInfo> ListDepartment = EmployeeDataLayer.GetInstance().GetAllEmployeebyID(connection, _ID);
                EmployeeInfo findEmployee = ListDepartment.First();
                return findEmployee;
            }
        }


        public ActionMessage createEmployee(EmployeeInfo _employee, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            if (_employee.Name.Trim() == string.Empty)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập tên";
            }
            //else if (_department.SourceID == -1)
            //{
            //}
            else
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        EmployeeDataLayer.GetInstance().InsertEmployee(connection, _employee, _userI);
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
        public ActionMessage editEmloyee(int id, EmployeeInfo _employee, string _userU)
        {
            ActionMessage ret = new ActionMessage();
            if (_employee.Name.Trim() == string.Empty)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập tên";
            }
            else
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    {
                        try
                        {
                            EmployeeDataLayer.GetInstance().UpdateEmployee(connection, _employee, _userU);
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
            }
            return ret;
        }

        public ActionMessage deleteEmployee(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    EmployeeDataLayer.GetInstance().DeleteEmployee(connection, id);
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