using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace AlphaPilar.Controllers
{
    public partial class ButtonController : Controller
    {
        //
        // GET: /ButtonDefault/
        public ActionResult ButtonFeatures()
        {
            List<object> items = new List<object>();
            items.Add(new
            {
                text = "Cut"
            });
            items.Add(new
            {
                text = "Copy"
            });
            items.Add(new
            {
                text = "Paste"
            });
            ViewBag.items = items;
            return View();
        }
    }
}
