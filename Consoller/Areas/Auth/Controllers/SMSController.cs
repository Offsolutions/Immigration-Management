using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;

namespace Consoller.Areas.Auth.Controllers
{
    public class SMSController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/SMS
        public ActionResult Index()
        {
            return View(db.tblsms.ToList());
        }

        // GET: Auth/SMS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblsms tblsms = db.tblsms.Find(id);
            if (tblsms == null)
            {
                return HttpNotFound();
            }
            return View(tblsms);
        }

        // GET: Auth/SMS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/SMS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Username,Senderid,Type,Api,alertno,Status")] tblsms tblsms)
        {
            if (ModelState.IsValid)
            {
                db.tblsms.Add(tblsms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblsms);
        }

        // GET: Auth/SMS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblsms tblsms = db.tblsms.Find(id);
            if (tblsms == null)
            {
                return HttpNotFound();
            }
            return View(tblsms);
        }

        // POST: Auth/SMS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Username,Senderid,Type,Api,alertno,Status")] tblsms tblsms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblsms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblsms);
        }

        // GET: Auth/SMS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblsms tblsms = db.tblsms.Find(id);
            if (tblsms == null)
            {
                return HttpNotFound();
            }
            return View(tblsms);
        }

        // POST: Auth/SMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblsms tblsms = db.tblsms.Find(id);
            db.tblsms.Remove(tblsms);
            db.SaveChanges();
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
    }
}
