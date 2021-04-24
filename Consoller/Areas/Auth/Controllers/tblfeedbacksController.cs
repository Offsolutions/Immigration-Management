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
    public class tblfeedbacksController : Controller
    {
        private dbcontext db = new dbcontext();
        Helper help = new Helper();
        SQLHelper objsql = new SQLHelper();
        // GET: Auth/tblfeedbacks
        public async Task<ActionResult> Index(string id)
        {
            //DateTime date = System.DateTime.Now;
            //if (User.IsInRole("Franchisee"))
            //{
            //    string a = help.Franchisee();
            //    return View(await db.tblfeedback.Where(x=>x.loginid==a && x.nextfollow<=date).ToListAsync());
            //}
            //else if(User.IsInRole("Receptionist"))
            //{
            //    string a = help.Receptionist();
            //    return View(await db.tblfeedback.Where(x => x.loginid == a && x.nextfollow <= date).ToListAsync());
            //}
            //else
            //{
            //    return View(await db.tblfeedback.Where(x=>x.nextfollow==date).ToListAsync());
            //}
            return View(await db.tblfeedback.Where(x => x.inquiryid==id).ToListAsync());

        }

        // GET: Auth/tblfeedbacks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblfeedback tblfeedback = await db.tblfeedback.FindAsync(id);
            if (tblfeedback == null)
            {
                return HttpNotFound();
            }
            return PartialView("pop", id);
        }

        // GET: Auth/tblfeedbacks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/tblfeedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,date,inquiryid,feedback,days,type,nextfollow,status,loginid")] tblfeedback tblfeedback)
        {
            if (ModelState.IsValid)
            {
                db.tblfeedback.Add(tblfeedback);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tblfeedback);
        }

        // GET: Auth/tblfeedbacks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblfeedback tblfeedback = await db.tblfeedback.FindAsync(id);
            if (tblfeedback == null)
            {
                return HttpNotFound();
            }
            return View(tblfeedback);
        }

        // POST: Auth/tblfeedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,date,inquiryid,feedback,days,type,nextfollow,status,loginid")] tblfeedback tblfeedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblfeedback).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tblfeedback);
        }

        // GET: Auth/tblfeedbacks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblfeedback tblfeedback = await db.tblfeedback.FindAsync(id);
            if (tblfeedback == null)
            {
                return HttpNotFound();
            }
            return View(tblfeedback);
        }

        // POST: Auth/tblfeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tblfeedback tblfeedback = await db.tblfeedback.FindAsync(id);
            db.tblfeedback.Remove(tblfeedback);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public ActionResult pop()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult follow(int id)
        {
            tblfeedback feed = new tblfeedback();
            var fee = db.tblfeedback.FirstOrDefault(x => x.Id == id).inquiryid;
            feed.inquiryid = fee;
            return View(feed);
        }
        [HttpPost]
        public ActionResult followCreate([Bind(Include = "Id,date,inquiryid,feedback,days,type,nextfollow,status,loginid")] tblfeedback tblfeedback)
        {
            objsql.ExecuteNonQuery("update tblfeedbacks set status='NotInterested' where inquiryid='" + tblfeedback.inquiryid + "'");
            tblfeedback.nextfollow = System.DateTime.Now.AddDays(tblfeedback.days);
            tblfeedback.loginid = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            db.tblfeedback.Add(tblfeedback);

            db.SaveChanges();
            TempData["Success"] = "Update Inquiry Successfully";
            return RedirectToAction("Index", "Default");
        }
    }
}
