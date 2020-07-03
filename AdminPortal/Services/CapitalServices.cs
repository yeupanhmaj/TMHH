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
    public class CapitalServices : BaseService<CapitalServices>
    {

        public List<CapitalInfo> getAllCapital()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<CapitalInfo> ListCapital = CapitalDataLayer.GetInstance().GetAllCapital(connection);

                return ListCapital;
            }
        }
        public List<CapitalInfo> GetByName(string name)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<CapitalInfo> ListCapital = CapitalDataLayer.GetInstance().GetByName(connection, name);

                return ListCapital;
            }
        }
        public CapitalInfo getCapitalbyId(int _ID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<CapitalInfo> ListDepartment = CapitalDataLayer.GetInstance().GetAllCapitalbyID(connection, _ID);
                CapitalInfo findCapital = ListDepartment.First();
                return findCapital;
            }
        }


        public ActionMessage createCapital(CapitalInfo _capital)
        {
            ActionMessage ret = new ActionMessage();
            if (_capital.CapitalName.Trim() == string.Empty)
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
                        CapitalDataLayer.GetInstance().InsertCapital(connection, _capital);
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
        public ActionMessage editEmloyee(int id, CapitalInfo _capital)
        {
            ActionMessage ret = new ActionMessage();
            if (_capital.CapitalName.Trim() == string.Empty)
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
                            CapitalDataLayer.GetInstance().UpdateCapital(connection, _capital);
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

        public ActionMessage deleteCapital(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    CapitalDataLayer.GetInstance().DeleteCapital(connection, id);
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