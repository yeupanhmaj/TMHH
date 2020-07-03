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
    public class CustomerService : BaseService<CustomerService>
    {
        public int getNewId()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<CustomerInfo> ListCustomer = CustomerDataLayer.GetInstance().GetAllCustomer(connection);
                CustomerInfo lastCustomer = ListCustomer.Last();
                if (lastCustomer != null) return (lastCustomer.CustomerID + 1);
                return 1;
            }
        }
        public int getTotalRecords(List<CustomerInfo> _listCustomer)
        {
            return _listCustomer.Count;
        }
       
        public List <CustomerInfo> getAllCustomer(int pageSize,int pageIndex)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<CustomerInfo> ListCustomer = CustomerDataLayer.GetInstance().GetAllCustomer(connection);
                //int min = pageIndex * pageSize;
                //int max = pageIndex * pageSize + pageSize;

                //if (min > ListCustomer.Count) return new List<CustomerInfo>();
                //if (max >= ListCustomer.Count) pageSize = ListCustomer.Count - min;
                //if (pageSize <= 0) return new List<CustomerInfo>();
                return ListCustomer;
            }
            //
            //SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            //using (SqlConnection connection = sqlConnection.GetConnection())
            //{
            //    List<CustomerInfo> ListCustomer = CustomerDataLayer.GetInstance().GetAllCustomer(connection);
            //    int min = pageIndex * pageSize;
            //    int max = pageIndex * pageSize + pageSize;

            //    if (min > ListCustomer.Count) return new List<CustomerInfo>();
            //    if (max >= ListCustomer.Count) pageSize = ListCustomer.Count - min;
            //    if (pageSize <= 0) return new List<CustomerInfo>();
            //    return ListCustomer.GetRange(min, pageSize);
            //}
        }

        public CustomerInfo getCustomerbyId(int _ID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                CustomerInfo Customer = CustomerDataLayer.GetInstance().getCustomer(connection, _ID);
                return Customer;
            }
        }
        

        public CustomerInfo getCustomerbyId(string _bidPlanCode)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                CustomerInfo Customer = CustomerDataLayer.GetInstance().getCustomerByBidPlanCode(connection, _bidPlanCode);
                return Customer;
            }
        }

        public List<CustomerInfo> getCustomer(CustomerSeachCriteria _criteria)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<CustomerInfo> ListCustomer = CustomerDataLayer.GetInstance().getCustomer(connection, _criteria);
                return ListCustomer;
            }
        }

        //public ActionMessage createCustomer(CustomerInfo _customer)
        //{
        //    ActionMessage ret = new ActionMessage();
        //    if (_customer.CustomerCode.Trim() == string.Empty)
        //    {
        //        ret.isSuccess = false;
        //        ret.err.msgCode = "Internal Error";
        //        ret.err.msgString = "Chưa nhập Khoa phòng";
        //    }
        //    //else if (_customer.SourceID == -1)
        //    //{
        //    //}
        //    else
        //    {
        //        SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
        //        using (SqlConnection connection = sqlConnection.GetConnection())
        //        {
        //            CustomerSeachCriteria _criteria = new CustomerSeachCriteria();
        //            _criteria.CustomerCode = _customer.CustomerCode;
        //            var chkCustomerInfo = getCustomer(_criteria);
        //            if (chkCustomerInfo.Count > 0)
        //            {
        //                ret.isSuccess = false;
        //                ret.err.msgCode = "Internal Error";
        //                ret.err.msgString = "Trùng mã Khoa phòng";
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    CustomerDataLayer.GetInstance().InsertCustomer(connection, _customer, _userI);
        //                    ret.isSuccess = true;
        //                }
        //                catch(Exception ex)
        //                {
        //                    ret.isSuccess = false;
        //                    ret.err.msgCode = "Internal Error";
        //                    ret.err.msgString =  ex.ToString();
        //                }
        //            }
                    
        //        }
        //    }
        //    return ret;
        //}
        //public ActionMessage editCustomer(int id, CustomerInfo _customer)
        //{
        //    ActionMessage ret = new ActionMessage();
        //    if (_customer.CustomerCode.Trim().Length == 0)
        //    {
        //        ret.isSuccess = false;
        //        ret.err.msgCode = "Internal Error";
        //        ret.err.msgString = "Chưa nhập mã Khoa phòng";
        //    }
        //    else
        //    {
        //        SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
        //        using (SqlConnection connection = sqlConnection.GetConnection())
        //        {
        //            CustomerSeachCriteria _criteria = new CustomerSeachCriteria();
        //            _criteria.CustomerCode = _customer.CustomerCode;
        //            var chkCustomerInfo = getCustomer(_criteria);
        //            if (chkCustomerInfo.Count > 0)
        //            {
                        
        //                ret.isSuccess = false;
        //                ret.err.msgCode = "Internal Error";
        //                ret.err.msgString = "Trùng mã Khoa phòng";
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    CustomerDataLayer.GetInstance().UpdateCustomer(connection, _customer, _userU);
        //                    ret.isSuccess = true;
        //                }
        //                catch (Exception ex)
        //                {
        //                    ret.isSuccess = false;
        //                    ret.err.msgCode = "Internal Error";
        //                    ret.err.msgString =  ex.ToString();
        //                }
        //            }

        //        }
        //    }
        //    return ret;
        //}

        //public ActionMessage deleteCustomer(int id)
        //{
        //    ActionMessage ret = new ActionMessage();

        //    SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
        //    using (SqlConnection connection = sqlConnection.GetConnection())
        //    {
        //        try
        //        {
        //            CustomerDataLayer.GetInstance().DeleteCustomer(connection, id);
        //            ret.isSuccess = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            ret.isSuccess = false;
        //            ret.err.msgCode = "Internal Error";
        //            ret.err.msgString =  ex.ToString();
        //        }
        //    }
        //    return ret;
        //}
    }
}
