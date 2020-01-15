using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace AlphaPilar.Controllers
{
    public partial class DocumentEditorController : Controller
    {
        //
        // GET: /Default/
        public IActionResult DocumentEditorFeatures()
        {
            return View();
        }
    }
}
