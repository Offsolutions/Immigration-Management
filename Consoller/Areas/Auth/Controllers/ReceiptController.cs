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
    public class ReceiptController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/Receipt
        public ActionResult Index()
        {
            return View(db.tblReceipt.ToList());
        }

        // GET: Auth/Receipt/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReceipt tblReceipt = db.tblReceipt.Find(id);
            if (tblReceipt == null)
            {
                return HttpNotFound();
            }
            return View(tblReceipt);
        }

        // GET: Auth/Receipt/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Receipt/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Start_no,End_no,Date,Current_Recipt,Status")] tblReceipt tblReceipt)
        {
            if (ModelState.IsValid)
            {
                db.tblReceipt.Add(tblReceipt);
                db.SaveChanges();
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Index");
            }

            return View(tblReceipt);
        }

        // GET: Auth/Receipt/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReceipt tblReceipt = db.tblReceipt.Find(id);
            if (tblReceipt == null)
            {
                return HttpNotFound();
            }
            return View(tblReceipt);
        }

        // POST: Auth/Receipt/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Start_no,End_no,Date,Current_Recipt,Status")] tblReceipt tblReceipt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblReceipt).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(tblReceipt);
        }

        // GET: Auth/Receipt/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReceipt tblReceipt = db.tblReceipt.Find(id);
            if (tblReceipt == null)
            {
                return HttpNotFound();
            }
            return View(tblReceipt);
        }

        // POST: Auth/Receipt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblReceipt tblReceipt = db.tblReceipt.Find(id);
            db.tblReceipt.Remove(tblReceipt);
            db.SaveChanges();
            TempData["Success"] = "Deleted Successfully";
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
