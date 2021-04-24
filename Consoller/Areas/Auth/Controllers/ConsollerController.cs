using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;
namespace Consoller.Areas.Auth.Controllers
{
    public class ConsollerController : Controller
    {
        // GET: Auth/Consoller
        dbcontext db = new dbcontext();
        public ActionResult Index(Helper help)
        {
            string name = help.Teacher().ToString();
            return View(db.Assigns.Where(x=>x.teacher==name));
        }
        public ActionResult Details(int? id)
        {
            return View(db.onlines.FirstOrDefault(x=>x.inquiryid==id));
        }
        public ActionResult Logs(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            online online = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            return PartialView("~/Areas/Auth/Views/Consoller/Logs.cshtml", online);
        }
        public ActionResult InsertLogs([Bind(Include = "inquiryid")] online online, Logs log,Helper help)
        {
            log.Date = System.DateTime.Now.ToString("dd/MM/yyyy");
            log.inquiryid = online.inquiryid;
            log.franchid = help.Receptionist();
            log.Description = log.Description;
            log.teacher = help.Permission();
            db.Logs.Add(log);
            db.SaveChanges();
            TempData["Success"] = "Logs Submitted Sucessfully";

            return RedirectToAction("Index","Consoller");
        }
        public ActionResult LogHistory(int id)
        {

            return View(db.onlines.FirstOrDefault(x=>x.inquiryid==id));

        }
        public ActionResult IDetails(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            online online = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            return PartialView("~/Areas/Auth/Views/Consoller/InquiryDetails.cshtml", online);

        }
    }
}