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
    public class ProposalTypeService : BaseService<ProposalTypeService>
    {
        public int getNewId()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ProposalTypeInfo> ListProposalType = ProposalTypeDataLayer.GetInstance().GetAllProposalType(connection);
                ProposalTypeInfo lastProposalType = ListProposalType.Last();
                if (lastProposalType != null) return (lastProposalType.TypeID + 1);
                return 1;
            }
        }
        public int getTotalRecords(List<ProposalTypeInfo> _listProposalType)
        {
            return _listProposalType.Count;
        }
       
        public List <ProposalTypeInfo> getAllProposalType(int pageSize,int pageIndex)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ProposalTypeInfo> ListProposalType = ProposalTypeDataLayer.GetInstance().GetAllProposalType(connection);
                int min = pageIndex * pageSize;
                int max = pageIndex * pageSize + pageSize;

                if (min > ListProposalType.Count) return new List<ProposalTypeInfo>();
                if (max >= ListProposalType.Count) pageSize = ListProposalType.Count - min;
                if (pageSize <= 0) return new List<ProposalTypeInfo>();
                return ListProposalType.GetRange(min, pageSize);
            }
        }

        public ProposalTypeInfo getProposalTypebyId(int _ID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ProposalTypeInfo> ListProposalType = ProposalTypeDataLayer.GetInstance().GetAllProposalType(connection);
                ProposalTypeInfo findProposalType = ListProposalType.Where(i => i.TypeID == _ID).First();
                return findProposalType;
            }
        }

        public List<ProposalTypeInfo> getProposalType(ProposalTypeSeachCriteria _criteria)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ProposalTypeInfo> ListProposalType = ProposalTypeDataLayer.GetInstance().getProposalType(connection, _criteria);
                return ListProposalType;
            }
        }

        public ActionMessage createProposalType(ProposalTypeInfo _ProposalType, string _userI)
        {
            ActionMessage ret = new ActionMessage();
            if (_ProposalType.TypeCode.Trim() == string.Empty)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error";
                ret.err.msgString = "Chưa nhập mã loại đề xuất ";
            }
            //else if (_ProposalType.SourceID == -1)
            //{
            //}
            else
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    ProposalTypeSeachCriteria _criteria = new ProposalTypeSeachCriteria();
                    _criteria.TypeCode = _ProposalType.TypeCode;
                    var chkProposalTypeInfo = getProposalType(_criteria);
                    if (chkProposalTypeInfo.Count > 0)
                    {
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString = "Trùng mã ";
                    }
                    else
                    {
                        try
                        {
                            ProposalTypeDataLayer.GetInstance().InsertProposalType(connection, _ProposalType, _userI);
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
        public ActionMessage editProposalType(int id, ProposalTypeInfo _ProposalType, string _userU)
        {
            ActionMessage ret = new ActionMessage();
            if (_ProposalType.TypeCode.Trim().Length == 0)
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
                    ProposalTypeSeachCriteria _criteria = new ProposalTypeSeachCriteria();
                    _criteria.TypeCode = _ProposalType.TypeCode;
                    var chkProposalTypeInfo = getProposalType(_criteria);
                    if (chkProposalTypeInfo.Count > 0)
                    {
                        
                        ret.isSuccess = false;
                        ret.err.msgCode = "Internal Error";
                        ret.err.msgString = "Trùng mã Khoa phòng";
                    }
                    else
                    {
                        try
                        {
                            ProposalTypeDataLayer.GetInstance().UpdateProposalType(connection, id, _ProposalType, _userU);
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

        public ActionMessage deleteProposalType(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    ProposalTypeDataLayer.GetInstance().DeleteProposalType(connection, id);
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
