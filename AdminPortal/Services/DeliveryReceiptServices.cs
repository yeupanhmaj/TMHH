using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Services
{
    public class DeliveryReceiptServices : BaseService<DeliveryReceiptServices>
    {
        public List<DeliveryReceiptInfo> GetList(DeliveryReceiptCriteria conditions, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                List<DeliveryReceiptInfo> ListAcceptanceInfo = DeliveryReceiptDataLayer.GetInstance().Getlist(connection, conditions,_userID);
                return ListAcceptanceInfo;
            }
        }

        public int getTotalRecords(DeliveryReceiptCriteria conditions, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                return DeliveryReceiptDataLayer.GetInstance().GetTotalRecords(connection, conditions,_userID);
            }
        }

        public DeliveryReceiptInfo GetDetail(int id, string _userID)
        {
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            DeliveryReceiptInfo record = new DeliveryReceiptInfo();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {              
                record = DeliveryReceiptDataLayer.GetInstance().GetDetail(connection, id,_userID);
                record.Items = DeliveryReceiptDataLayer.GetInstance().getSelectedItems(connection, id,_userID);
                record.ListComment = new List<CommentInfo>();

                //Get document
                DocumentSeachCriteria documentCriteria = new DocumentSeachCriteria();
                documentCriteria.TableName = TableFile.DeliveryReceipt.ToString();
                documentCriteria.PreferId = id.ToString();
                record.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria);


                record.Employees = new List<DeliveryReceiptEmployeeInfo>();
                record.Employees = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptEmployeesById(connection, record.DeliveryReceiptID.ToString());

                //get Comment
                CommentSeachCriteria commentCriteria = new CommentSeachCriteria();
                commentCriteria.TableName = TableFile.DeliveryReceipt.ToString();
                commentCriteria.PreferId = id.ToString();
                record.ListComment = CommentService.GetInstance().getComment(commentCriteria);
                foreach (var item in record.ListComment)
                {
                    DocumentSeachCriteria documentCriteria2 = new DocumentSeachCriteria();
                    documentCriteria2.TableName = TableFile.Comment.ToString();
                    documentCriteria2.PreferId = item.AutoID.ToString();
                    item.ListDocument = DocumentService.GetInstance().GetDocument(documentCriteria2);
                }
            }
            return record;
        }

        public async Task<ActionMessage> Create(DeliveryReceiptInfo obj, [FromForm] List<IFormFile> files, string _userI, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                  
                    insetId = DeliveryReceiptDataLayer.GetInstance().Create(connection, obj, _userI);       
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (insetId > -1)
            {
                ret.id = insetId;
                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        if (obj.Items != null)
                        {
                           // insertEmployees(insetId, obj.Employees);
                            foreach (DeliveryReceiptItemInfoNew item in obj.Items)
                            {
                                item.DeliveryReceiptID = insetId;
                                if(item.Amount > 0)
                                {

                                    //input tai san
                                    DeliveryReceiptWithDepartment deliveryReceipt = DeliveryReceiptDataLayer.GetInstance().getItemCreateAna(connection, item.DeliveryReceiptID,_userID);
                                    AnalyzerInfo analyzerInfo = new AnalyzerInfo()
                                    {
                                        AnalyzerAccountantCode = "",
                                        AnalyzerName = item.ItemName,
                                        AnalyzerType = 0,

                                        QuoteItemID = item.QuoteItemID,
                                        Description = "",

                                        Amount = item.Amount,
                                        ItemPrice = item.ItemPrice,
                                        TotalPrice = item.TotalPrice,
                                        DepartmentRootID = deliveryReceipt.DepartmentID,
                                        DepartmentID = deliveryReceipt.DepartmentID,

                                        ContractCode = deliveryReceipt.ContractCode,
                                        UserIContract = deliveryReceipt.UserIContract,
                                        CustomerID = deliveryReceipt.CustomerID,
                                        CustomerName = deliveryReceipt.CustomerName,

                                        ExpirationDate = DateTime.Now,
                                        DateIn = obj.DeliveryReceiptDate,
                                        DeliveryReceiptID = item.DeliveryReceiptID,
                                        Serial = "",
                                    };

                                    int seq = AnalyzerDataLayer.GetInstance().GetMaxPropCode(connection, analyzerInfo.DateIn.Year);
                                    int insertID = AnalyzerDataLayer.GetInstance().InsertAnalyzer(connection, analyzerInfo, seq, _userI);
                                   
                                    string[] _anaNamesSplit = analyzerInfo.AnalyzerName.Split(' ');
                                    string _anaNameSplit = "";
                                    for (int i = 0; i < _anaNamesSplit.Count(); i++)
                                    {
                                        if (i == 4)
                                            break;
                                        else
                                        {
                                            _anaNameSplit += (_anaNamesSplit[i][0]).ToString().ToUpper();
                                        }
                                    }
                                    string anacode = _anaNameSplit + "." + analyzerInfo.DateIn.Year + "." + String.Format("{0:000000}", seq);
                                    AnalyzerDataLayer.GetInstance().UpdateAnalyzer(connection, insertID, anacode);
                                    // end insert
                                    DeliveryReceiptDataLayer.GetInstance().CreateDeliveryReceiptItem(connection, item, _userI);
                                }
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                using (SqlConnection connection = sqlConnection.GetConnection())
                {
                    try
                    {
                        if (obj.Employees != null)
                        {
                            foreach (DeliveryReceiptEmployeeInfo item in obj.Employees)
                            {
                                item.DeliveryReceiptID = insetId;
                                DeliveryReceiptDataLayer.GetInstance().CreateDeliveryReceiptItemUser(connection, item, _userI);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.DeliveryReceipt.ToString();
                    documentInfo.PreferId = insetId.ToString();
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.FileName = item.FileName;
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.DeliveryReceipt.ToString(), insetId.ToString(), item, documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, _userI.ToString());
                }
                ret.isSuccess = true;
            }
            else
            {
                ret.isSuccess = false;
                ret.err.msgCode = "lỗi thêm phiếu nghiệm thu";
                ret.err.msgString = "lỗi thêm phiếu nghiệm thu";           
            }
            return ret;
        }

        public async Task<ActionMessage> Update(DeliveryReceiptInfo obj, [FromForm] List<IFormFile> files, string _userI)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    DeliveryReceiptDataLayer.GetInstance().Update(connection, obj, _userI);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            /*  using (SqlConnection connection = sqlConnection.GetConnection())
              {
                  try
                  {
                      EditEmployees(obj.DeliveryReceiptID, obj.Employees);
                      foreach (DeliveryReceiptItemInfoNew item in obj.Items)
                      {
                          DeliveryReceiptDataLayer.GetInstance().UpdateDeliveryReceiptItem(connection, item, _userI);
                      }
                  }
                  catch (Exception ex)
                  {
                      throw ex;
                  }
              }*/

            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {

                    DeliveryReceiptDataLayer.GetInstance().DeleteDeliveryReceiptItemUser(connection, obj.DeliveryReceiptID, _userI);


                    if (obj.Employees != null)
                    {
                        foreach (DeliveryReceiptEmployeeInfo item in obj.Employees)
                        {
                            item.DeliveryReceiptID = obj.DeliveryReceiptID;
                            DeliveryReceiptDataLayer.GetInstance().CreateDeliveryReceiptItemUser(connection, item, _userI);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            DocumentService.GetInstance().DeleteDocumentsNotExitsInList(obj.ListDocument, TableFile.DeliveryReceipt.ToString(), obj.DeliveryReceiptID);
            foreach (var item in files)
            {
                DocumentInfo documentInfo = new DocumentInfo();
                documentInfo.TableName = TableFile.DeliveryReceipt.ToString();
                documentInfo.PreferId = obj.DeliveryReceiptID.ToString();
                documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                documentInfo.FileName = item.FileName;
                documentInfo.Length = item.Length.ToString();
                documentInfo.Type = item.ContentType;
                ret = await FilesHelpers.UploadFile(TableFile.DeliveryReceipt.ToString(), obj.DeliveryReceiptID.ToString(), item, documentInfo.Link);
                DocumentService.GetInstance().InsertDocument(documentInfo, _userI.ToString());
            }
            ret.isSuccess = true;
            
         
            return ret;
        }

        public ActionMessage Delete(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentByFeature(TableFile.DeliveryReceipt.ToString(), id);

                    //delete commet 
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.DeliveryReceipt.ToString();
                    _criteria.PreferId = id.ToString();
                    CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);

                    //delete employees
                    List<DeliveryReceiptEmployeeInfo> currentItems = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptEmployeesById(connection, id.ToString());
                    if (currentItems.Count > 0)
                    {
                        string autoIds = "";
                        foreach (var item in currentItems)
                        {
                            autoIds = autoIds + item.AutoID + ',';
                        }
                        autoIds = autoIds.Remove(autoIds.Length - 1);
                        DeliveryReceiptDataLayer.GetInstance().DeleteDeliveryReceiptEmployees(connection, autoIds);
                    }

                    //delete record

                    List<string> quoteID = DeliveryReceiptDataLayer.GetInstance().GetQuoteByDeliveryReceiptIds(connection, id.ToString());
                    QuoteService.GetInstance().deleteProcess(connection, "DeliveryReceipt", String.Join(", ", quoteID.ToArray()), _userID);
                    DeliveryReceiptDataLayer.GetInstance().Delete(connection, id);
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
        public ActionMessage DeleteMuti(string ids, string _userID)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    //delete employees 
                    List<DeliveryReceiptEmployeeInfo> currentItems = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptEmployeesByIds(connection, ids);
                    if (currentItems.Count > 0)
                    {
                        string autoIds = "";
                        foreach (var item in currentItems)
                        {
                            autoIds = autoIds + item.AutoID + ',';
                        }
                        autoIds = autoIds.Remove(autoIds.Length - 1);
                        DeliveryReceiptDataLayer.GetInstance().DeleteDeliveryReceiptEmployees(connection, autoIds);
                    }

                    //delete comments
                    CommentSeachCriteria _criteria = new CommentSeachCriteria();
                    _criteria.TableName = TableFile.DeliveryReceipt.ToString();
                    string[] IDsarray = ids.Split(',');
                    foreach (string id in IDsarray)
                    {
                        _criteria.PreferId = id;
                        CommentDataLayer.GetInstance().DeleteComment(connection, _criteria);
                    }

                    //delete attach files and DB of attach files
                    DocumentService.GetInstance().DeleteDocumentsByFeature(TableFile.DeliveryReceipt.ToString(), ids);
                    //delete records
                    /* List<string> acceptanceSids = AcceptanceDataLayer.GetInstance().getAcceptanceByDeliveryReceiptids(connection,ids);
                     if (acceptanceSids.Count > 0) {
                         AcceptanceDataLayer.GetInstance().DeleteMuti(connection,String.Join(", ", acceptanceSids.ToArray()));
                     }*/
                    List<string> quoteID = DeliveryReceiptDataLayer.GetInstance().GetQuoteByDeliveryReceiptIds(connection, ids);
                    QuoteService.GetInstance().deleteProcess(connection, "DeliveryReceipt", String.Join(", ", quoteID.ToArray()), _userID);
                    DeliveryReceiptDataLayer.GetInstance().DeleteMuti(connection, ids);
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

        public ActionMessage insertEmployees(int deliveryReceiptID, List<DeliveryReceiptEmployeeInfo> items)
        {
            ActionMessage ret = new ActionMessage();
            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    foreach (var item in items)
                    {
                        DeliveryReceiptDataLayer.GetInstance().InsertDeliveryReceiptEmployee(connection, deliveryReceiptID, item);
                    }

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

        public ActionMessage EditEmployees(int deliveryReceiptID, List<DeliveryReceiptEmployeeInfo> _items)
        {
            ActionMessage ret = new ActionMessage();

            SqlConnectionFactory sqlConnection = new SqlConnectionFactory();
            using (SqlConnection connection = sqlConnection.GetConnection())
            {
                try
                {
                    List<DeliveryReceiptEmployeeInfo> currentItems = DeliveryReceiptDataLayer.GetInstance().GetDeliveryReceiptEmployeesById(connection, deliveryReceiptID.ToString());
                    var listDeleteItem = new List<DeliveryReceiptEmployeeInfo>();
                    var listInsertItems = new List<DeliveryReceiptEmployeeInfo>();
                    if (_items == null)
                    {
                        listDeleteItem = currentItems;
                        listInsertItems = new List<DeliveryReceiptEmployeeInfo>();
                    }
                    else
                    {
                        listDeleteItem = currentItems.Except(_items).ToList();
                        listInsertItems = _items.Except(currentItems).ToList();
                    }
                    //add new Items
                    foreach (var item in listInsertItems)
                    {
                        DeliveryReceiptDataLayer.GetInstance().InsertDeliveryReceiptEmployee(connection, deliveryReceiptID, item);
                    }
                    string autoIds = "";
                    //delete Old Items

                    if (listDeleteItem.Count > 0)
                    {
                        foreach (var item in listDeleteItem)
                        {
                            autoIds = autoIds + item.AutoID + ',';
                        }
                        autoIds = autoIds.Remove(autoIds.Length - 1);
                        DeliveryReceiptDataLayer.GetInstance().DeleteDeliveryReceiptEmployees(connection, autoIds);
                    }


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
