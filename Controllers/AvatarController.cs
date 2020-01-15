using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace AlphaPilar.Controllers
{
    public partial class AvatarController : Controller
    {
        //
        // GET: /AvatarDefault/
        public ActionResult AvatarFeatures()
        {
            return View();
        }
    }
}
