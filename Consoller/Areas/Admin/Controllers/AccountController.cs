using Consoller.Areas.Auth.Models;
using Consoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Consoller.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account
        public ActionResult Index(int id)
        {
            return View();
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Credential", new { area = "" });
        }
        public ActionResult ChangeRole(tblreceptionist model, string returnUrl, Helper Help, int id)
        {
            dbcontext db = new dbcontext();
            var passw = model.password;
            string ids = id.ToString();
            var dataItem = db.tblreceptionists.Where(x => x.rid == ids).FirstOrDefault();
            if (dataItem != null)
            {

                FormsAuthentication.SetAuthCookie(dataItem.rid, false);

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                         && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    TempData["Success"] = "Login Successfully";
                    Session["User"] = dataItem.Id;
                    Session["admin"] = dataItem.Id;
                    return RedirectToAction("Index", "Dashboard");


                }

            }
            else
            {
                // ModelState.AddModelError("", "Invalid user/pass");
                TempData["Success"] = "Invalid user/pass";
                return View();
            }
        }
    }
}