using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Areas.Admin.Controllers
{
    public class AdminReportController : Controller
    {
        // GET: Admin/AdminReport
        dbcontext db = new dbcontext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            online online = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            return PartialView("~/Areas/Admin/Views/Default/InquiryDetails.cshtml", online);

        }
    }
}