﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace AlphaPilar.Controllers
{
    public partial class BadgeController : Controller
    {
        //
        // GET: /BadgeDefault/
        public ActionResult BadgeFeatures()
        {
            return View();
        }
    }
}
