using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdminPortal.DataLayers;
using AdminPortal.Entities;
using EncryptionLibrary;


namespace AdminPortal.DataLayer
{
    public class ProposalTypeDataLayer : BaseLayerData<ProposalTypeDataLayer>
    {
        DataProvider db = new DataProvider();
        /// <summary>
        /// Hàm lấy tất cả khoa phòng
        /// </summary>
        /// <returns>Return List<ProposalTypeInfo></returns>
        /// 
        public List<ProposalTypeInfo> GetAllProposalType(SqlConnection connection)
        {
            var result = new List<ProposalTypeInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_ProposalType where  1 = 1 order by TypeID ", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalTypeInfo();
                        info.TypeID = GetDbReaderValue<int>(reader["TypeID"]);
                        info.TypeCode = GetDbReaderValue<string>(reader["TypeCode"]);
                        info.TypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public List<ProposalTypeInfo> getProposalType(SqlConnection connection, ProposalTypeSeachCriteria _criteria)
        {
            var result = new List<ProposalTypeInfo>();
            using (var command = new SqlCommand("Select * " +
                " from tbl_ProposalType where  1 = 1 ", connection))
            {
                if (!string.IsNullOrEmpty(_criteria.TypeID))
                {
                    command.CommandText += " and TypeID = @PTypeID";
                    AddSqlParameter(command, "@TypeID", _criteria.TypeID, System.Data.SqlDbType.Int);
                }
                if (!string.IsNullOrEmpty(_criteria.TypeCode))
                {
                    command.CommandText += " and TypeCode = @TypeCode";
                    AddSqlParameter(command, "@TypeCode", _criteria.TypeCode, System.Data.SqlDbType.NVarChar);
                }
                if (!string.IsNullOrEmpty(_criteria.TypeName))
                {
                    command.CommandText += " and TypeName like N'%" + _criteria.TypeName + "%' ";
                }
                command.CommandText += " order by TypeID  ";
                WriteLogExecutingCommand(command);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var info = new ProposalTypeInfo();
                        info.TypeID = GetDbReaderValue<int>(reader["TypeID"]);
                        info.TypeCode = GetDbReaderValue<string>(reader["TypeCode"]);
                        info.TypeName = GetDbReaderValue<string>(reader["TypeName"]);
                        info.UserI = GetDbReaderValue<string>(reader["UserI"]);
                        info.InTime = GetDbReaderValue<DateTime?>(reader["InTime"]);
                        info.UserU = GetDbReaderValue<string>(reader["UserU"]);
                        info.UpdateTime = GetDbReaderValue<DateTime>(reader["UpdateTime"]);
                        result.Add(info);
                    }
                }
                return result;
            }
        }
        public int InsertProposalType(SqlConnection connection, ProposalTypeInfo _ProposalType, string _userI)
        {
            int lastestInserted = 0;
            using (var command = new SqlCommand("Insert into [dbo].[tbl_ProposalType] (TypeCode, TypeName, UserI)" +
                    "VALUES(@ProposalTypeCode, @ProposalTypeName, @UserI)  ", connection))
            {
                AddSqlParameter(command, "@ProposalTypeCode", _ProposalType.TypeCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@ProposalTypeName", _ProposalType.TypeName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserI", _userI, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }

            return lastestInserted;
        }

        public void UpdateProposalType(SqlConnection connection,int _id, ProposalTypeInfo _ProposalType, string _userU)
        {
            using (var command = new SqlCommand("UPDATE tbl_ProposalType \n" +
                            "SET TypeCode = @TypeCode, TypeName = @TypeName, UserU=@UserU,UpdateTime=getdate() \n" +
                            "WHERE (TypeID = @TypeID)", connection))
            {
                AddSqlParameter(command, "@TypeID", _id, System.Data.SqlDbType.Int);
                AddSqlParameter(command, "@TypeCode", _ProposalType.TypeCode, System.Data.SqlDbType.VarChar);
                AddSqlParameter(command, "@TypeName", _ProposalType.TypeName, System.Data.SqlDbType.NVarChar);
                AddSqlParameter(command, "@UserU", _userU, System.Data.SqlDbType.VarChar);
                WriteLogExecutingCommand(command);

                command.ExecuteScalar();
            }
        }

        public void DeleteProposalType(SqlConnection connection,int _ProposalTypeID)
        {
            using (var command = new SqlCommand("delete tbl_ProposalType where TypeID=@ProposalTypeID", connection))
            {
                AddSqlParameter(command, "@ProposalTypeID", _ProposalTypeID, System.Data.SqlDbType.Int);
                WriteLogExecutingCommand(command);
                command.ExecuteScalar();
            }
        }
    }
}
