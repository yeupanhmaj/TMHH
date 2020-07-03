using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using AdminPortal.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // GET: api/<CatogeryController>
        [HttpGet]
        public ListResponeMessage<CategoryInfo> GetListCategory()
        {
            ListResponeMessage<CategoryInfo> ret = new ListResponeMessage<CategoryInfo>();
            try
            {
                ret.isSuccess = true;
                ret.data = CategoryService.GetInstance().GetListCategory();              
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "005";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        // GET api/<CatogeryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CatogeryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CatogeryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CatogeryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
