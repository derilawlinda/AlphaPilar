using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using AlphaPilar.Models;
using Syncfusion.EJ2.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authorization;

namespace AlphaPilar.Controllers
{
    public class APFileManager : Controller
    {
        [Authorize(Roles = "Administrator")]
        public IActionResult FileManager()
        {
            using (var db = new LiteDatabase(@"Alfapilar.db"))
            {
                var query = db.GetCollection<FileManagerModel>("fileManagers");
                var results = query.FindAll();
                ViewBag.datasource = results.ToList();
            }
            return View();
        }

        [HttpPost]
        public JsonResult FileCreate(string Section, string Procedure, string Form, string Title, string Element, string URL, string EksternalUrl, string Work)
        {
            string urlFileName = "";
            try
            {

                using (var db = new LiteDatabase(@"Alfapilar.db"))
                {
                    // Get customer collection
                    var files = db.GetCollection<FileManagerModel>("fileManagers");


                    var fileData = new FileManagerModel
                    {
                        Section = Section,
                        Procedure = Procedure,
                        Form = Form,
                        Title = Title,
                        Element = Element,
                        URL = URL,
                        EksternalUrl = EksternalUrl,
                        Work = Work

                    };

                    // Insert new customer document (Id will be auto-incremented)
                    files.Insert(fileData);
                }
            }
            catch (Exception exception)
            {
                return Json(new { success = false, responseText = exception.Message });
            }

            return Json(new { success = true });
        }

        public IActionResult AddFile([FromBody] CRUDModel<FileManagerModel> value)
        {
            using (var db = new LiteDatabase(@"Alfapilar.db"))
            {
                var query = db.GetCollection<FileManagerModel>("fileManagers");
                var maxId = query.Max(x => x.Id);
                ViewBag.maxId = maxId.AsInt32 + 1;
            }
            return PartialView("_AddFile");
        }

        [HttpPost]
        public IActionResult ShowFileEditorForm([FromBody] CRUDModel<FileManagerModel> value)
        {
            FileManagerModel result;
            using (var db = new LiteDatabase(@"Alfapilar.db"))
            {
                var query = db.GetCollection<FileManagerModel>("fileManagers");
                result = query.FindById(value.Value.Id);
                ViewBag.datasource = result;
            }
            return PartialView("_EditFile", result);
        }

        [HttpPost]
       
        public JsonResult FileUpdate(Int32 Id, string Section, string Procedure, string Form, string Title, string URL, string Element, string EksternalUrl, string Work)
        {

            try
            {
                using (var db = new LiteDatabase(@"Alfapilar.db"))
                {



                    var collection = db.GetCollection<FileManagerModel>("fileManagers");
                    var fileUpdate = collection.FindById(Id);
                    fileUpdate.Section = Section;
                    fileUpdate.Procedure = Procedure;
                    fileUpdate.Form = Form;
                    fileUpdate.Title = Title;
                    fileUpdate.Element = Element;
                    fileUpdate.URL = URL;
                    fileUpdate.EksternalUrl = EksternalUrl;
                    fileUpdate.Work = Work;
                    collection.Update(fileUpdate);

                }
            }
            catch (Exception exception)
            {
                return Json(new { success = false, responseText = exception.Message });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult FileDelete(Int32 id)
        {
            try
            {

                using (var db = new LiteDatabase(@"Alfapilar.db"))
                {
                    // Get customer collection
                    var collection = db.GetCollection<FileManagerModel>("fileManagers");
                    collection.Delete(id);

                }
            }
            catch (Exception exception)
            {
                return Json(new { success = false, responseText = exception.Message });
            }

            return Json(new { success = true });
        }

        private IHostingEnvironment hostingEnv;
        public void Save(IList<IFormFile> chunkFile, IList<IFormFile> UploadFiles)
        {
            long size = 0;
            try
            {
                // for chunk-upload
                foreach (var file in chunkFile)
                {
                    var filename = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                    filename = hostingEnv.WebRootPath + $@"\{filename}";
                    size += file.Length;

                    if (!System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        using (FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Append))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }

            // for normal upload
            try
            {
                foreach (var file in UploadFiles)
                {
                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    filename = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", filename);
                    size += file.Length;
                    if (!System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
    }
}
