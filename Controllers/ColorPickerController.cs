using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace AlphaPilar.Controllers
{
    public partial class ColorPickerController : Controller
    {
        //
        // GET: /Default/
        public IActionResult ColorPickerFeatures()
        {
            return View();
        }
    }
}
