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
    public class EmployeeRoleServices : BaseService<EmployeeRoleServices>
    {
        public List<EmployeeRoleInfo> getAllEmployeeRole()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<EmployeeRoleInfo> ListEmployeeRole = EmployeeRoleDataLayer.GetInstance().GetAllEmployeeRole(connection);

                return ListEmployeeRole;
            }
        }

        public ActionMessage createEmployeeRole(EmployeeRoleInfo _employeeRole)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    EmployeeRoleDataLayer.GetInstance().InsertEmployeeRole(connection, _employeeRole);
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
        public ActionMessage editEmployeeRole(int id, EmployeeRoleInfo _employeeRole)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                {
                    try
                    {
                        EmployeeRoleDataLayer.GetInstance().UpdateEmployeeRole(connection, _employeeRole);
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

        public ActionMessage deleteEmployeeRole(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    EmployeeRoleDataLayer.GetInstance().DeleteEmployeeRole(connection, id);
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