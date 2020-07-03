using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models.Common;
using System.IO;
using AdminPortal.DataLayer;
using AdminPortal.Entities;
using AdminPortal.Services;

namespace AdminPortal.Helpers
{
    public static class FilesHelpers
    {

        public static string GetContentType(string path)
        {/*
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext]*/;
            return "text/plain";
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx",  "text/plain"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                 {".sql", "text/plain"},
            };
        }

        public static async Task<ActionMessage> UploadFiles(string feature, string preferId, List<IFormFile> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                var rootFolder = Utils.getRootFolder();
                var imageFolder = rootFolder + Utils.uploadFolder();
                var destinationFolder = imageFolder + @"/" + feature + @"/" + preferId + @"/";

                bool exists = System.IO.Directory.Exists(destinationFolder);

                if (!exists)
                    System.IO.Directory.CreateDirectory(destinationFolder);


                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = destinationFolder + formFile.FileName;
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }

                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }

        public static void DeleteFiles(string feature, string preferId, List<string> files)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                if (!string.IsNullOrEmpty(feature) || !string.IsNullOrEmpty(preferId))
                {
                    if (files.Count > 0)
                    {
                        var rootFolder = Utils.getRootFolder();
                        var imageFolder = rootFolder + Utils.uploadFolder();
                        var destinationFolder = imageFolder + @"/" + feature + @"/" + preferId + @"/";

                        bool exists = System.IO.Directory.Exists(destinationFolder);

                        if (!exists)
                            return;
                        foreach(string file in files)
                        {
                            var filePath = destinationFolder + file;
                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);
                        }
                      
                    }
                }
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
        }
        public static void DeleteFile(string feature, string preferId, string file)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                if (!string.IsNullOrEmpty(feature) || !string.IsNullOrEmpty(preferId))
                {

                        var rootFolder = Utils.getRootFolder();
                        var imageFolder = rootFolder + Utils.uploadFolder();
                        var destinationFolder = imageFolder + @"/" + feature + @"/" + preferId + @"/";

                        bool exists = System.IO.Directory.Exists(destinationFolder);

                        if (!exists)
                            return;                
                            var filePath = destinationFolder + file;
                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);           
                }
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
        }


        public static void DeleteFolder(string feature, string preferId)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                if (!string.IsNullOrEmpty(feature) || !string.IsNullOrEmpty(preferId))
                {

                    var rootFolder = Utils.getRootFolder();
                    var imageFolder = rootFolder + Utils.uploadFolder();
                    var destinationFolder = imageFolder + @"/" + feature + @"/" + preferId;

                    bool exists = System.IO.Directory.Exists(destinationFolder);

                    if (!exists)
                    {
                        return;
                    }
                    else
                    {
                        System.IO.Directory.Delete(destinationFolder,true);
                    }                                            
                }
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
        }

        public static async Task<ActionMessage> UploadFile(string feature, string preferId, IFormFile file, string subLink)
        {
            ActionMessage ret = new ActionMessage();
            try
            {
                if(!string.IsNullOrEmpty(feature) || !string.IsNullOrEmpty(preferId))
                {
                    var rootFolder = Utils.getRootFolder();
                    var imageFolder = rootFolder + Utils.uploadFolder();
                    var destinationFolder = imageFolder + @"/" + feature + @"/" + preferId + @"/";

                    bool exists = System.IO.Directory.Exists(destinationFolder);

                    if (!exists)
                        System.IO.Directory.CreateDirectory(destinationFolder);

                    if (file.Length > 0)
                    {
                        var filePath = destinationFolder + subLink;
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
                ret.isSuccess = true;
            }
            catch (Exception ex)
            {
                ret.isSuccess = false;
                ret.err.msgCode = "001";
                ret.err.msgString = ex.ToString();
            }
            return ret;
        }
    }
}
