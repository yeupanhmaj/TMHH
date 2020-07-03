using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.DataLayer;
using AdminPortal.DataLayers.Common;
using AdminPortal.Entities;
using AdminPortal.Models.Common;
using Microsoft.AspNetCore.Http;

namespace AdminPortal.Services
{
    public class FileService : BaseService<FileService>
    {

        public async Task<ActionMessage> ImportQuote(int id, IFormFile file , string UserID)
        {
            ActionMessage ret = new ActionMessage();
          

            return ret;

        }
    }
}
