using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : BaseController
    {
        // GET: api/Items
        [HttpGet]
        public ListResponeMessage<ItemInfo>GetAll()
        {
            ListResponeMessage<ItemInfo> ret = new ListResponeMessage<ItemInfo>();
           // ret.data = ItemsService.getAllItems();
            ret.isSuccess = true;

            return ret;
        }
        [HttpGet("GetListItem")]
        public ListResponeMessage<ItemInfo> GetListItem(string name="", string code="")
        {
            ListResponeMessage<ItemInfo> ret = new ListResponeMessage<ItemInfo>();
            try
            {        
                ret.data = ItemsServices.GetInstance().GetItemsByCondition(name , code);
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


      

        // GET: api/Items/5
        [HttpGet("{id}")]
        public SingleResponeMessage<ItemInfo> Get(int id)
        {
            SingleResponeMessage<ItemInfo> ret = new SingleResponeMessage<ItemInfo>();
           // ret.item = ItemService.getItembyId(id);
            ret.isSuccess = true;
           
            return ret;
        }

        // POST: api/Items
        [HttpPost]
        public SingleResponeMessage<ItemRequest> Post([FromBody] ItemRequest data)
        {
            SingleResponeMessage<ItemRequest> ret = new SingleResponeMessage<ItemRequest>();
            try
            {
                ret.item = ItemsServices.GetInstance().CreateItem(data.ItemName, data.ItemCode, data.ItemUnit, GetUserId());
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

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
