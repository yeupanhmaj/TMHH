using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Services;
using AdminPortal.Models;
using Microsoft.AspNetCore.Authorization;
using AdminPortal.Models.Common;

namespace AdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("signin")]
        public AuthenticateModelRespone signin([FromBody]AuthenticateModel auth)
        {
            AuthenticateModelRespone ret = new AuthenticateModelRespone();
            try
            {
                ret = AuthServices.GetInstance().SignIn(auth.userName, auth.password);
            }
            catch(Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
        [Authorize]
        [HttpGet("check")]
        public ActionMessage check() 
        {
            ActionMessage ret = new ActionMessage();
            ret.isSuccess = true;
         //  string userName = "123";
         //  string password = "123";
            return ret;
        }
    }
}
