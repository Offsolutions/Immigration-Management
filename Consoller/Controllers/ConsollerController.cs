using Consoller.Areas.Auth.Models;
using Consoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Controllers
{
    public class ConsollerController : Controller
    {
        // GET: Consoller
        dbcontext db = new dbcontext();
        Helper help = new Helper();

        public ActionResult Index()
        {
            string name = help.Teacher().ToString();
            string permission = help.Consoller();
            return View(db.Assigns.Where(x => x.teacher == name && x.Status == true && x.franchid == permission));
        }
        public ActionResult IeltsInquiry()
        {
            string name = help.Teacher().ToString();
            string permission = help.Consoller();
            return View(db.AssignIelts.Where(x => x.teacher == name && x.Status == true && x.franchid == permission));
        }
        public ActionResult Confirm(Helper help)
        {
            string name = help.Teacher().ToString();
            string permission = help.Consoller();
            return View(db.onlines.Where(x => x.teacher == name && x.Sign == "Confirm" && x.franchid == permission));
        }
        public ActionResult NotConfirm(Helper help)
        {
            string name = help.Teacher().ToString();
            string permission = help.Consoller();
            return View(db.onlines.Where(x => x.teacher == name && x.franchid == permission));
        }
        public ActionResult Details(int? id)
        {
            return View(db.onlines.FirstOrDefault(x => x.inquiryid == id));
        }
        public ActionResult Logs(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            online online = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            return PartialView("~/Views/Consoller/Logs.cshtml", online);
        }
        public ActionResult InsertLogs([Bind(Include = "inquiryid")] online online, Logs log, Helper help, Application app, Leader Leadgers, Application appyear,Process process)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    
                    string franchise = help.Consoller();
                    var result = db.onlines.SingleOrDefault(b => b.inquiryid == online.inquiryid && b.franchid == franchise);
                    if (result != null)
                    {
                        result.Sign = log.Status;

                        db.SaveChanges();
                    }
                    var teacher = help.Teacher();
                    // var role = help.Consoller();
                    var assign = db.Assigns.SingleOrDefault(b => b.inquiryid == online.inquiryid && b.teacher == teacher && b.franchid == franchise && b.Status == true);
                    if (assign != null)
                    {
                        assign.Status = false;
                        db.SaveChanges();
                    }
                    log.Date = System.DateTime.Now.ToString("dd/MM/yyyy");
                    log.inquiryid = online.inquiryid;
                    log.franchid = franchise;
                    log.Description = log.Description;
                    log.teacher = teacher;
                    db.Logs.Add(log);
                    db.SaveChanges();
                    #region Only for When Application Final
                    if (log.Status == "Confirm")
                    {
                        var teach = db.onlines.SingleOrDefault(b => b.inquiryid == online.inquiryid && b.franchid == franchise);
                        if (teach != null)
                        {
                            teach.teacher = help.Permission();

                            db.SaveChanges();
                        }
                        Application applications = db.Applications.FirstOrDefault();
                        if (applications == null)
                        {
                            app.ApplicationNo = 12500;
                        }
                        else
                        {
                            var max = db.Applications.Max(x => x.ApplicationNo);
                            app.ApplicationNo = Convert.ToInt32(max) + 1;

                        }
                        app.Date = System.DateTime.Now;
                        app.InquiryId = online.inquiryid;
                        app.Franchid = franchise;
                        app.DealBy = teacher;
                        app.TrackingId = help.otp();
                        app.Password = "12345";
                        app.Status = "Confirm";
                        app.Year = appyear.Year;
                        app.Month = appyear.Month;

                        db.Applications.Add(app);
                        db.SaveChanges();

                        Leader ll = db.Ledgers.FirstOrDefault();
                        if (ll == null)
                        {
                            Leadgers.LeaderNo = "10010";
                        }
                        else
                        {
                            var leaderno = db.Ledgers.Max(x => x.LeaderNo);
                            Leadgers.LeaderNo = (Convert.ToInt32(leaderno) + Convert.ToInt32(1)).ToString();
                        }
                        Leadgers.Date = System.DateTime.Now;
                        Leadgers.ApplicationNo = app.ApplicationNo;
                        Leadgers.Total = db.PaymentSteps.Find(1).Amount;
                        Leadgers.Received = 0;
                        Leadgers.Concession = 0;
                        Leadgers.Status = "Open";
                        Leadgers.Franchid = franchise;
                        db.Ledgers.Add(Leadgers);
                        db.SaveChanges();
                        process.ApplicationNo = app.ApplicationNo;
                        process.Registration = true;
                        db.Process.Add(process);
                        db.SaveChanges();


                    }
                    transaction.Commit();
                    #endregion
                    TempData["Success"] = "Logs Submitted Sucessfully";

                    help.sendsms(online.Mobile, "Thank You " + online.Name + " for Coming BlueBird Immigration. Your Trackingid is:"+app.TrackingId+" and password:12345  visit on bluebirdimmigrations.com");
                    return RedirectToAction("Index", "Consoller");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    TempData["Danger"] = "Error Occur ! " + e.Message + "";
                    throw;
                }
            }
        }
        public ActionResult LogHistory(int id)
        {
            string permission = help.Consoller();
            return View(db.onlines.FirstOrDefault(x => x.inquiryid == id && x.franchid == permission));

        }
        public ActionResult NoLog(int id)
        {
            string permission = help.Consoller();
            return View(db.onlines.FirstOrDefault(x => x.inquiryid == id && x.franchid == permission));

        }
        public ActionResult EditLogs(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Logs logs = db.Logs.FirstOrDefault(x => x.Lid == id);
            return PartialView("~/Views/Consoller/EditLogs.cshtml", logs);

        }
        public ActionResult UpdateLogs(Logs log, Helper help, Application app, Leader Leadgers, Application appyear, Process process)
        {

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    string franchise = help.Consoller();
                    var result = db.onlines.SingleOrDefault(b => b.inquiryid == log.inquiryid && b.franchid == franchise);
                    if (result != null)
                    {
                        result.Sign = log.Status;

                        db.SaveChanges();
                    }
                    var teacher = help.Teacher();
                    // var role = help.Consoller();
                    var assign = db.Assigns.SingleOrDefault(b => b.inquiryid == log.inquiryid && b.teacher == teacher && b.franchid == franchise && b.Status == true);
                    if (assign != null)
                    {
                        assign.Status = false;
                        db.SaveChanges();
                    }
                    log.Date = System.DateTime.Now.ToString("dd/MM/yyyy");
                    log.inquiryid = log.inquiryid;
                    log.franchid = franchise;
                    log.Description = log.Description;
                    log.teacher = teacher;
                    db.Logs.Add(log);
                    db.SaveChanges();
                    #region Only for When Application Final
                    if (log.Status == "Confirm")
                    {
                        var teach = db.onlines.SingleOrDefault(b => b.inquiryid == log.inquiryid && b.franchid == franchise);
                        if (teach != null)
                        {
                            teach.teacher = help.Permission();

                            db.SaveChanges();
                        }
                        Application applications = db.Applications.FirstOrDefault();
                        if (applications == null)
                        {
                            app.ApplicationNo = 12500;
                        }
                        else
                        {
                            var max = db.Applications.Max(x => x.ApplicationNo);
                            app.ApplicationNo = Convert.ToInt32(max) + 1;

                        }
                        app.Date = System.DateTime.Now;
                        app.InquiryId =Convert.ToInt32(log.inquiryid);
                        app.Franchid = franchise;
                        app.DealBy = teacher;
                        app.TrackingId = help.otp();
                        app.Password = "12345";
                        app.Status = "Confirm";
                        app.Year = appyear.Year;
                        app.Month = appyear.Month;

                        db.Applications.Add(app);
                        db.SaveChanges();

                        Leader ll = db.Ledgers.FirstOrDefault();
                        if (ll == null)
                        {
                            Leadgers.LeaderNo = "10010";
                        }
                        else
                        {
                            var leaderno = db.Ledgers.Max(x => x.LeaderNo);
                            Leadgers.LeaderNo = (Convert.ToInt32(leaderno) + Convert.ToInt32(1)).ToString();
                        }
                        Leadgers.Date = System.DateTime.Now;
                        Leadgers.ApplicationNo = app.ApplicationNo;
                        Leadgers.Total = db.PaymentSteps.Find(1).Amount;
                        Leadgers.Received = 0;
                        Leadgers.Concession = 0;
                        Leadgers.Status = "Open";
                        Leadgers.Franchid = franchise;
                        db.Ledgers.Add(Leadgers);
                        db.SaveChanges();
                        process.ApplicationNo = app.ApplicationNo;
                        process.Registration = true;
                        db.Process.Add(process);
                        db.SaveChanges();


                    }
                    transaction.Commit();
                    #endregion
                    TempData["Success"] = "Logs Submitted Sucessfully";

                    help.sendsms(result.Mobile, "Thank You " + result.Name + " for Coming BlueBird Immigration. Your Trackingid is:" + app.TrackingId + " and password:12345  visit on bluebirdimmigrations.com");
                    return RedirectToAction("NoLog", "Consoller", new { id = log.inquiryid });
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    TempData["Danger"] = "Error Occur ! " + e.Message + "";
                    throw;
                }
            }
          
           
        }

        public ActionResult IDetails(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            online online = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            
            return PartialView("~/Views/Consoller/InquiryDetails.cshtml", online);

        }
        public ActionResult IeDetails(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            string a = id.ToString();
            tblinquiry online = db.tblinquiries.FirstOrDefault(x => x.inquiryid == a);
            return PartialView("~/Views/Consoller/IeltsDetails.cshtml", online);

        }
        public ActionResult IeltsLogs(int id)
        {
            tblfeedback feed = new tblfeedback();
            feed.inquiryid = id.ToString();
            return View(feed);
        }
        [HttpPost]
        public ActionResult IeltsLogs(tblfeedback feed)
        {
            feed.date = System.DateTime.Now;
            feed.type = "Days";
            feed.nextfollow = System.DateTime.Now.AddDays(feed.days);
            if (feed.days > 0)
            {
                feed.status = true;
            }
            else
            {
                feed.status = false;
            }
            feed.loginid = help.Teacher();
            db.tblfeedback.Add(feed);
            db.SaveChanges();
            TempData["Success"] = "Logs Submitted Sucessfully";
            return RedirectToAction("IeltsLogs","Consoller", new { id=feed.inquiryid});
        }


    }
}