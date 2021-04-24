using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;

namespace Consoller.Areas.Auth.Controllers
{
    public class LogsController : Controller
    {
        dbcontext db = new dbcontext();
        // GET: Auth/Logs
        [Authorize(Roles ="Franchisee,Admin,Receptionist")]
        public ActionResult Index()
        {
            DateTime date = System.DateTime.Now;
            string a = HttpContext.User.Identity.Name;
            if (User.IsInRole("Admin"))
            {
                return View(db.Fees_Master.Where(x => x.AlertDate <= date).ToList());
            }
            else if (User.IsInRole("Franchisee"))
            {
                return View(db.Fees_Master.Where(x => x.AlertDate <= date && x.franchid == a && x.Status == true).ToList());
            }
            else
            {
                tblreceptionist rr = db.tblreceptionists.Where(x => x.rid == a).First();
                string b = rr.franchid.ToString();
                var studentdata = db.Fees_Master.Where(x => x.AlertDate <= date && x.franchid == b && x.Status==true).ToList();
                return View(studentdata);
               // return View(db.Fees_Master.Where(x => x.AlertDate <= date).ToList());
            }    
            
        }

        // GET: Auth/Logs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Auth/Logs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Logs/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Auth/Logs/Edit/5
        public ActionResult Edit(int id)
        {
            return RedirectToAction("Create", "Deposit", new { roll = id });
        }
        public ActionResult SMS(int id,Helper Help)
        {
            tblstudentdata ss = db.tblstudentdata.FirstOrDefault(x => x.rollno == id);
            Fees_Master fm = db.Fees_Master.FirstOrDefault(x => x.RollNo == id);
            tblsms sms = db.tblsms.FirstOrDefault();
            float pending = fm.TotalFees - fm.PaidFees;
            if (ss.phone != null)
            {
                string msg = "Dear " + Convert.ToString(ss.name) + ". Reminder That Your Course Fee is still Due Rs."+pending+". Thanks for Joining Us. visit www.englishtreemoga.com.";
                string result = Help.apicall("http://sms.officialsolutions.in/sendSMS?username=" + sms.Username + "&message=" + msg + "&sendername=" + sms.Senderid + "&smstype=TRANS&numbers=" + ss.phone + "&apikey=" + sms.Api + "");

                TempData["Success"] = "SMS Send Successfully";
            }
            return RedirectToAction("Index");
        }
        // POST: Auth/Logs/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Auth/Logs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Auth/Logs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        
    }
}
