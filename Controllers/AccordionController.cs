using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace AlphaPilar.Controllers
{
    public class AccordionController : Controller
    {
        public IActionResult AccordionFeatures()
        {
          return View();
        }
    }
}
