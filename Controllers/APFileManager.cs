using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using AlphaPilar.Models;

namespace AlphaPilar.Controllers
{
    public class APFileManager : Controller
    {
        public IActionResult Index()
        {
            using (var db = new LiteDatabase(@"Alphapillar.db"))
            {
                var query = db.GetCollection<FileManagerModel>("fileManagers");
                var results = query.FindAll();
                ViewBag.datasource = results.ToList();
            }
            return View();
        }
    }
}
