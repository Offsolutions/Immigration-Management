using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;

namespace Consoller.Areas.Auth.Controllers
{
    public class OurDetailController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string img;
        // GET: Auth/OurDetail
        public ActionResult Index(Helper help)
        {
            int a =Convert.ToInt32(help.Franchisee());
            if (User.IsInRole("Franchisee"))
            return View(db.tbldetails.Where(x=>x.franchid==a).ToList());
            else
                return View(db.tbldetails.ToList());
        }

        // GET: Auth/OurDetail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbldetail tbldetail = db.tbldetails.Find(id);
            if (tbldetail == null)
            {
                return HttpNotFound();
            }
            return View(tbldetail);
        }

        // GET: Auth/OurDetail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/OurDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,phone,email,address,logo")] tbldetail tbldetail, HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                tbldetail.logo = Help.uploadfile(file);
                tbldetail.franchid =Convert.ToInt32(Help.Franchisee());
                db.tbldetails.Add(tbldetail);
                db.SaveChanges();
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Index");
            }

            return View(tbldetail);
        }

        // GET: Auth/OurDetail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbldetail tbldetail = db.tbldetails.Find(id);
            img = tbldetail.logo;
            if (tbldetail == null)
            {
                return HttpNotFound();
            }
            return View(tbldetail);
        }

        // POST: Auth/OurDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,phone,email,address,logo,franchid")] tbldetail tbldetail, HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                tbldetail.logo = file != null ? Help.uploadfile(file) : img;
                #region delete file
                string fullPath = Request.MapPath("~/UploadedFiles/" + img);
                if (img == tbldetail.logo)
                {
                }
                else
                {
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                #endregion
                db.Entry(tbldetail).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(tbldetail);
        }

        // GET: Auth/OurDetail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbldetail tbldetail = db.tbldetails.Find(id);
            if (tbldetail == null)
            {
                return HttpNotFound();
            }
            return View(tbldetail);
        }

        // POST: Auth/OurDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbldetail tbldetail = db.tbldetails.Find(id);
            img = tbldetail.logo;
            #region delete file
            string fullPath = Request.MapPath("~/UploadedFiles/" + img);
            if (img == tbldetail.logo)
            {
            }
            else
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            #endregion
            db.tbldetails.Remove(tbldetail);
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
