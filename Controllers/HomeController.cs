#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlphaPilar.Models;
using AlphaPilar.Helpers;
using LiteDB;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AlphaPilar.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login(string ReturnUrl)
        {
            string curUrl = HttpContext.Request.QueryString.Value;
            string url = System.Net.WebUtility.UrlDecode(curUrl);

            var index = url.IndexOf('?', url.IndexOf('?') + 1);

            if (index > -1)
            {
                var ouput = string.Concat(url.Substring(0, index), "&", url.Substring(index + 1));
                var parsed = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(ouput);
                if (parsed["error"] != "")
                {
                    var error = parsed["error"].ToString();
                    ViewBag.error = error;
                }
            }
            ViewBag.returnUrl = ReturnUrl;
            return View();

        }

        [HttpPost]
        public IActionResult DoLogin(string userName, string password, string returnUrl)
        {


            if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Login");
            }
            //Check the user name and password  
            //Here can be implemented checking logic from the database  
            var db = new LiteDatabase(@"Alfapilar.db");
            var query = db.GetCollection<Account>("accounts");
            Account result = query.Find(Query.EQ("Username", userName)).FirstOrDefault();

            if (result == null)
            {
                return RedirectToAction("Login", "Home", new
                {
                    error = "InvalidUsername"
                });
            }
            else
            {
                if (PasswordHasher.VerifyPassword(password, result.Salt, result.Password))
                {

                    //Create the identity for the user  
                    var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, result.Username),
                    new Claim(ClaimTypes.Role, result.RoleName)
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
            }


            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
