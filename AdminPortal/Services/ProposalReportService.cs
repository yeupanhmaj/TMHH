using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Services
{
    public class ProposalReportService
    {
        public ProposalInfo GetDetail(string id)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            ProposalInfo record = new ProposalInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                //record = PracticeDataLayer.GetInstance().GetUser(connection, id);

                return null;
            }
        }
        public List<ProposalInfo> GetList()
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<ProposalInfo> ListUserInfo = ProposalDataLayer.GetInstance().GetAllOutdateProposal(connection, "");
                return ListUserInfo;
            }
        }
    }
}
