using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;
namespace Consoller.Areas.Auth.Controllers
{
    public class onlinesController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/onlines
        Helper help = new Helper();
        public async Task<ActionResult> Index()
        {
            string permission = help.Permission();
            return View(await db.onlines.Where(x=>x.franchid==permission).ToListAsync());
        }

        // GET: Auth/onlines/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            online online = await db.onlines.FindAsync(id);
            if (online == null)
            {
                return HttpNotFound();
            }
            return View(online);
        }

        // GET: Auth/onlines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/onlines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Oid,Date,Refusal,RefusalCountry,Name,Mobile,Email,City,Marital,Gender,VisaType,Country,Source,Qualification,PassingYear,Percentage,Paper,TestType,Band,Docs,Passport,inquiryid,franchid,Reading,Listening,Writing,Speaking,RefusalNo,Others,Alternative")] online online, Helper help)
        {
            if (ModelState.IsValid)
            {
                string franch = help.Permission();
                online.franchid = franch;
                online ol = db.onlines.FirstOrDefault(x => x.franchid == franch);
                if (ol == null)
                {
                    online.inquiryid = 1001;
                }
                else
                {
                    var inquiry = db.onlines.Where(x => x.franchid == franch).Max(x => x.inquiryid);
                    online.inquiryid = Convert.ToInt32(inquiry) + 1;
                }
                online.Date = System.DateTime.Now;
                db.onlines.Add(online);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(online);
        }

        // GET: Auth/onlines/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            online online = await db.onlines.FindAsync(id);
            if (online == null)
            {
                return HttpNotFound();
            }
            return View(online);
        }

        // POST: Auth/onlines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Oid,Date,Refusal,RefusalCountry,Name,Mobile,Email,City,Marital,Gender,VisaType,Country,Source,Qualification,PassingYear,Percentage,Paper,TestType,Band,Docs,Passport,inquiryid,franchid,Reading,Listening,Writing,Speaking,RefusalNo,Others")] online online)
        {
            if (ModelState.IsValid)
            {
                db.Entry(online).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(online);
        }

        // GET: Auth/onlines/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            online online = await db.onlines.FindAsync(id);
            if (online == null)
            {
                return HttpNotFound();
            }
            return View(online);
        }

        // POST: Auth/onlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            online online = await db.onlines.FindAsync(id);
            db.onlines.Remove(online);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Form([Bind(Include = "Mobile")] online online1, Helper help)
        {
            online check = db.onlines.FirstOrDefault(x => x.Mobile == online1.Mobile);
            if (check == null)
            {
                string franch = help.Permission();
                string mobile = online1.Mobile;
                string message = franch + '/' + mobile;
                string msg = "http://inquiry.officialsolutions.in/Default?f=" + message;

                help.sendsms(mobile, msg);
                TempData["Success"] = "Send Message";
                return View();
            }
            else
            {
                TempData["danger"] = "Mobile No Already Register";
                return View();
            }
        }
        public ActionResult IeltsForm([Bind(Include = "Mobile")] online online1, Helper help)
        {
            
                string franch = help.Permission();
                string mobile = online1.Mobile;
                string message = franch + '/' + mobile;
                string msg = "http://inquiry.officialsolutions.in/Ielts?f=" + message;

                help.sendsms(mobile, msg);
                TempData["Success"] = "Send Message";
                return RedirectToAction("Form","onlines");
            
           
        }
        public ActionResult OtpForm()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult OtpForm([Bind(Include = "Mobile,franchid,checkotp")] otp Otp, Helper help)
        //{
        //    Otp.franchid = help.Permission();
        //    Otp.checkotp = otp();
        //    db.otps.Add(Otp);
        //    db.SaveChanges();

        //    help.sendsms(Otp.Mobile, "Dear Customer, your OTP for BlueBird Registration is " + Otp.checkotp + " only valid for 20 min");
        //    TempData["Success"] = "Send Message";
        //    return View();

        //}
        //public string otp()
        //{
        //    string numbers = "0123456789";
        //    Random objrandom = new Random();
        //    string strrandom = string.Empty;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        int temp = objrandom.Next(0, numbers.Length);
        //        strrandom += temp;
        //    }
        //    return strrandom;
        //}
        public ActionResult Assign(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //online online = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            //if (online != null)
            //{
            //    return PartialView("~/Areas/Auth/Views/onlines/AssignUpdate.cshtml", online);
            //}
            //else
            //{
            //    online on = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            //    return PartialView("~/Areas/Auth/Views/onlines/Assign.cshtml", on);
            //}
            online on = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            return PartialView("~/Areas/Auth/Views/onlines/Assign.cshtml", on);

        }
        public ActionResult ReAssign(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            string franchid = help.Permission();
            online online = db.onlines.FirstOrDefault(x => x.inquiryid == id && x.franchid==franchid);
            return PartialView("~/Areas/Auth/Views/onlines/AssignUpdate.cshtml", online);
           

        }
        public ActionResult UpdateAssign([Bind(Include = "inquiryid")] online online, string assign,Assign assign1,Helper help)
        {

            assign1.Date = System.DateTime.Now;
            assign1.inquiryid = online.inquiryid;
            assign1.teacher = assign;
            assign1.franchid = help.Permission();
            assign1.Status = true;
            db.Assigns.Add(assign1);
            db.SaveChanges();
            var result = db.onlines.SingleOrDefault(b => b.inquiryid == online.inquiryid && b.franchid==assign1.franchid);
            if (result != null)
            {
                result.teacher = assign;
                db.SaveChanges();
            }
            TempData["Success"] = "Assign Inquiry Successfully";

            return RedirectToAction("Index","Auth/onlines");
        }
        public ActionResult UpdateAssignTeacher([Bind(Include = "inquiryid")] online online, string assign, Assign assign1, Helper help, tblreceptionist tblreceptionist)
        {
            
            SQLHelper objsql = new SQLHelper();
            objsql.ExecuteNonQuery("update assigns set status='0' where inquiryid='" + assign1.inquiryid + "'");

            assign1.Date = System.DateTime.Now;
            assign1.inquiryid = online.inquiryid;
            assign1.teacher = assign1.teacher;
            assign1.franchid = help.Permission();
            assign1.Status = true;
            db.Assigns.Add(assign1);
            db.SaveChanges();
            var result1 = db.onlines.SingleOrDefault(b => b.inquiryid == online.inquiryid && b.franchid==assign1.franchid);
            if (result1 != null)
            {
                result1.teacher = assign1.teacher;
                db.SaveChanges();
            }
            TempData["Success"] = "Update Lead Successfully";

            return RedirectToAction("Index", "Auth/onlines");
        }

    }
}
