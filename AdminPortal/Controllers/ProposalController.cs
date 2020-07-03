using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Entities;
using AdminPortal.Helpers;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Commons;
using AdminPortal.DataLayer;
using System.IO;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProposalController));

        // GET: api/Proposal
        [HttpGet]
        public ListResponeMessage<ProposalInfo> GetList(int pageSize = 10, int pageIndex = 0)
        {
            ProposalSeachCriteria _criteria = new ProposalSeachCriteria();
            _criteria.pageIndex = 0;
            _criteria.pageSize = 10;
            _criteria.FromDate = Helpers.Utils.StartOfDate(DateTime.Now);
            _criteria.ToDate = Helpers.Utils.EndOfDate(DateTime.Now);
            return GetListWithCondition(_criteria);
        }
       

        // GET: api/Proposal/byconditions
        [HttpGet("byconditions")]
        public ListResponeMessage<ProposalInfo> GetListWithCondition([FromQuery]
            ProposalSeachCriteria _criteria)
        {
            ListResponeMessage<ProposalInfo> ret = new ListResponeMessage<ProposalInfo>();
            try
            {
                string _userID = GetUserId();
                ret.isSuccess = true;
                ret.data = ProposalService.GetInstance().getAllProposal(_criteria.pageSize, _criteria.pageIndex, _criteria, _userID);
                ret.totalRecords = ProposalService.GetInstance().getTotalRecords(_criteria, _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Proposal/getListProposalCode
        [HttpGet("getListProposalCode")]
        public ListResponeMessage<string> getListProposalCode(string proposalCode = "")
        {
            ListResponeMessage<string> ret = new ListResponeMessage<string>();
            try
            {
                string _userID = GetUserId();
                ret.isSuccess = true;
                ret.data = ProposalService.GetInstance().getListProposalCode(proposalCode, _userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET: api/Proposal/5
        [HttpGet("{id}")]
        public SingleResponeMessage<ProposalDetailInfo> Get(int id)
        {
            SingleResponeMessage<ProposalDetailInfo> ret = new SingleResponeMessage<ProposalDetailInfo>();
            try
            {
                string _userID = GetUserId();
                ProposalDetailInfo item = ProposalService.GetInstance().getDetailProposal(id, _userID);
                if(item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no proposal found";
                    return ret;
                }
                ret.item = item;
                ret.isSuccess = true;
            }
           catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpGet("getbycode")]
        public SingleResponeMessage<ProposalDetailInfo> GetbyCode(string code)
        {
            SingleResponeMessage<ProposalDetailInfo> ret = new SingleResponeMessage<ProposalDetailInfo>();
            try
            {
                string _userID = GetUserId();
                ProposalDetailInfo item = ProposalService.GetInstance().GetDetailProposalByCode(code, _userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no proposal found";
                    return ret;
                }
                ret.item = item;
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // POST: api/Proposal
        [HttpPost]
        public ActionMessage Post([FromBody] ProposalInfo _proposal)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                string user = GetUserId();
                ret = ProposalService.GetInstance().createProposal(_proposal, user);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }/*
        public ProposalInfo propsalObj { get; set; }
        public List<IFormFile> files { get; set; }*/
        //POST: api/Proposal/withDFile
       [RequestSizeLimit(5242880)]
       [HttpPost("withDFile")]
        public async Task<ActionMessage> PostwithAttFile( [FromForm] ProposalInfo propsalObj, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            int insetId = -1;
            try
            {
                ret.isSuccess = true;
                insetId = ProposalService.GetInstance().createProposal2(propsalObj, GetUserId());
                if (insetId > -1)
                {
                    // upload file
                    //ret = await FilesHelpers.UploadFiles("proposal", insetId.ToString(), files);
                    // update list file
                    ret = ProposalService.GetInstance().insertItems(insetId, propsalObj.Items, propsalObj.DateIn, propsalObj.DepartmentID);
                    if(ret.isSuccess == false)
                    {
                        return ret;
                    }
                    foreach (var item in files)
                    {
                        DocumentInfo documentInfo = new DocumentInfo();
                        documentInfo.TableName = TableFile.Proposal.ToString();
                        documentInfo.PreferId = insetId.ToString();
                        documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                        documentInfo.FileName = item.FileName;
                        documentInfo.Length = item.Length.ToString();
                        documentInfo.Type = item.ContentType;
                        ret = await FilesHelpers.UploadFile(TableFile.Proposal.ToString(), insetId.ToString(), item, documentInfo.Link);
                        DocumentService.GetInstance().InsertDocument(documentInfo, GetUserId());                 
                    }
                }
                else
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "lỗi thêm proposal";
                    ret.err.msgString = "lỗi thêm proposal";
                }
              
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        // PUT: api/Proposal/5
        [HttpPut("{id}")]
        public ActionMessage Put(int id, [FromForm] ProposalInfo _proposal)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ProposalService.GetInstance().editProposal(id, _proposal, GetUserId());
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        //PUT: api/Proposal/5/withDFile
       [HttpPut("{id}/withDFile")]
        public async Task<ActionMessage> Put(int id, [FromForm] ProposalInfo _proposal, [FromForm] List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
            try
            {
                ret = ProposalService.GetInstance().editProposal(id, _proposal, GetUserId());
                //update list file
                //ProposalService.GetInstance().DelteDocument(id.ToString());
                if (ret.isSuccess == false) return ret;
                //++edit item
                ret = ProposalService.GetInstance().EditItems(id, _proposal.Items);
                if (ret.isSuccess == false) return ret;


                //++EDIT document
                //delete old document
                DocumentService.GetInstance().DeleteDocumentsNotExitsInList(_proposal.Documents, TableFile.Proposal.ToString(), id);
                //add new document
                foreach (var item in files)
                {
                    DocumentInfo documentInfo = new DocumentInfo();
                    documentInfo.TableName = TableFile.Proposal.ToString();
                    documentInfo.PreferId = id.ToString();
                    documentInfo.FileName = item.FileName;
                    documentInfo.Link = DateTime.Now.ToString("yyMMddHHmmssfff") + "-" + Utils.ChuyenTVKhongDau(item.FileName);
                    documentInfo.Length = item.Length.ToString();
                    documentInfo.Type = item.ContentType;
                    ret = await FilesHelpers.UploadFile(TableFile.Proposal.ToString(), _proposal.ProposalID.ToString(), item , documentInfo.Link);
                    DocumentService.GetInstance().InsertDocument(documentInfo, GetUserId());
                }
                //--EDIT document
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        // DELETE: api/Proposal/5
        [HttpDelete("{id}")]
        public ActionMessage Delete(int id, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ProposalService.GetInstance().DeleteProposal(id , GetUserId(),_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        // DELETE: api/Proposal?proposalIDs=
        // [Authorize]
        [HttpDelete()]
        public ActionMessage DeleteAll(string proposalIDs, string _userID)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                ret = ProposalService.GetInstance().DeleteProposals(proposalIDs, GetUserId(),_userID);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


        [HttpGet("getProposalWithContactItemsByCode")]
        public SingleResponeMessage<ProposalDetailWithContactItemsInfo> GetProposalWithContactItemsByCode(string code)
        {
            SingleResponeMessage<ProposalDetailWithContactItemsInfo> ret = new SingleResponeMessage<ProposalDetailWithContactItemsInfo>();
            try
            {
                string _userID = GetUserId();
                ProposalDetailWithContactItemsInfo item = ProposalService.GetInstance().GetProposalWithContactItemsByCode(code, _userID);
                if (item == null)
                {
                    ret.isSuccess = false;
                    ret.err.msgCode = "001";
                    ret.err.msgString = "no proposal found";
                    return ret;
                }
                ret.item = item;
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


   


        // GET: api/Proposal/getRelateData
        [HttpGet("getRelateData")]
        public SingleResponeMessage<ProposalRelatedData> GetRelateData(int id)
        {
            SingleResponeMessage<ProposalRelatedData> ret = new SingleResponeMessage<ProposalRelatedData>();
            try
            {
                ret.isSuccess = true;
                ret.item = ProposalService.GetInstance().GetRelateData(id);
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //report 
        [HttpGet("countByStatus")]
        public ListResponeMessage<StatusCountReport> CountByStatus()
        {
            ListResponeMessage<StatusCountReport> ret = new ListResponeMessage<StatusCountReport>();
            try
            {
                ret.data =  ProposalService.GetInstance().GetCountByStatus();                    
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //report 
        [HttpGet("getProposalCanCreateQuote")]
        public ListResponeMessage<ProposalWithItems> getProposalCanCreateQuote(string proposalCode, string itemName )
        {
            ListResponeMessage<ProposalWithItems> ret = new ListResponeMessage<ProposalWithItems>();
            try
            {
                ret.data = ProposalService.GetInstance().getProposalCanCreateQuote(proposalCode, itemName );
                ret.totalRecords = ret.data.Count;
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


        //report 
        [HttpGet("getListProsalHaveContractCode")]
        public ListResponeMessage<ProposalSelectItem> GetListProsalHaveQuote(string code)
        {
            ListResponeMessage<ProposalSelectItem> ret = new ListResponeMessage<ProposalSelectItem>();
            try
            {
                ret.data = ProposalService.GetInstance().GetListProsalHaveQuote(code);
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


        //report 
        [HttpGet("getListProsalHaveAcceptance")]
        public ListResponeMessage<ProposalSelectItem> GetListProsalHaveAcceptance(string code)
        {
            ListResponeMessage<ProposalSelectItem> ret = new ListResponeMessage<ProposalSelectItem>();
            try
            {
                ret.data = ProposalService.GetInstance().GetListProsalCanCreateAcceptance(code);
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        [HttpGet("getDetailsAcceptanceByProposalID")]
        public SingleResponeMessage<DRFillDetailInfo> getDetailsAcceptanceByProposalID(int id)
        {
            SingleResponeMessage<DRFillDetailInfo> ret = new SingleResponeMessage<DRFillDetailInfo>();
            try
            {
                ret.item = ProposalService.GetInstance().getDetailsAcceptanceByProposalID(id);
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }


        [HttpGet("getDetailsForDR")]
        public SingleResponeMessage<DRFillDetailInfo> getDetailsForDR(int id)
        {
            SingleResponeMessage<DRFillDetailInfo> ret = new SingleResponeMessage<DRFillDetailInfo>();
            try
            {
                ret.item = ProposalService.GetInstance().getDetailsForDR(id);
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        //report 
        [HttpGet("getListProsalCanCreateDR")]
        public ListResponeMessage<ProposalSelectItem> getListProsalCanCreateDR(string code)
        {
            ListResponeMessage<ProposalSelectItem> ret = new ListResponeMessage<ProposalSelectItem>();
            try
            {
                ret.data = ProposalService.GetInstance().getListProsalCanCreateDR(code);
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "Internal Error !!!";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

    }
}
