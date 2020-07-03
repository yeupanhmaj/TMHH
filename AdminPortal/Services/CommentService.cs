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
    public class CommentService : BaseService<CommentService>
    {

        public List<CommentInfo> getComment(CommentSeachCriteria _criteria)
        {
            if (!string.IsNullOrEmpty(_criteria.TableName) && !string.IsNullOrEmpty(_criteria.PreferId))
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    List<CommentInfo> ListComment = CommentDataLayer.GetInstance().getComment(connection, _criteria);
                    return ListComment;
                }
            }
            else return null;
        }

        public int InsertComment(CommentInfo commentInfo, string _userI)
        {
            int ret = -1;
            if (!(string.IsNullOrEmpty(commentInfo.TableName)))
            {
                SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        ret = CommentDataLayer.GetInstance().insertComment(connection, commentInfo, _userI);
                    }
                    catch (Exception ex)
                    {
                        ret = -1;
                    }
                }
                return ret;
            }
            else
            {
                return -1;
            }
        }
        public ActionMessage deleteComment(int id)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    CommentDataLayer.GetInstance().DeleteComment(connection, id);
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
        public ActionMessage deleteComments(string ids)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    CommentDataLayer.GetInstance().DeleteComments(connection, ids);
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
    }
}
