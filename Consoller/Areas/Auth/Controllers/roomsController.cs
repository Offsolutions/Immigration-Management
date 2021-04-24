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
    public class roomsController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/rooms
        public ActionResult Index()
        {
            return View(db.tblrooms.ToList());
        }

        // GET: Auth/rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblroom tblroom = db.tblrooms.Find(id);
            if (tblroom == null)
            {
                return HttpNotFound();
            }
            return View(tblroom);
        }

        // GET: Auth/rooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,room,status")] tblroom tblroom)
        {
            if (ModelState.IsValid)
            {
                db.tblrooms.Add(tblroom);
                db.SaveChanges();
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Index");
            }

            return View(tblroom);
        }

        // GET: Auth/rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblroom tblroom = db.tblrooms.Find(id);
            if (tblroom == null)
            {
                return HttpNotFound();
            }
            return View(tblroom);
        }

        // POST: Auth/rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,room,status")] tblroom tblroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblroom).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(tblroom);
        }

        // GET: Auth/rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblroom tblroom = db.tblrooms.Find(id);
            if (tblroom == null)
            {
                return HttpNotFound();
            }
            return View(tblroom);
        }

        // POST: Auth/rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblroom tblroom = db.tblrooms.Find(id);
            db.tblrooms.Remove(tblroom);
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
