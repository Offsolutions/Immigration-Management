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
    public class tblreceptionistsController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string img = "";
        // GET: Auth/tblreceptionists
        public ActionResult Index(Helper help)
        {
            if (User.IsInRole("Franchisee"))
            {
                string r = help.Franchisee();
                string a = help.Franchisee();
                return View(db.tblreceptionists.Where(x => x.rid == r || x.franchid == a));
            }
            else if (User.IsInRole("Receptionist"))
            {
                string r = help.Franchisee();
                return View(db.tblreceptionists.Where(x => x.rid == r));
            }
            return View(db.tblreceptionists.ToList());
        }

        // GET: Auth/tblreceptionists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblreceptionist tblreceptionist = await db.tblreceptionists.FindAsync(id);
            if (tblreceptionist == null)
            {
                return HttpNotFound();
            }
            return View(tblreceptionist);
        }

        // GET: Auth/tblreceptionists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/tblreceptionists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,date,name,email,contact,login,password,rid,image,Type,franchid,status,StartTime,EndTime")] tblreceptionist tblreceptionist, HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                tblreceptionist recp = db.tblreceptionists.FirstOrDefault();
                if (recp == null)
                {
                    tblreceptionist.rid = "1001";
                }
                else
                {
                    var valc = db.tblreceptionists.Max(x => x.rid);

                    tblreceptionist.rid = (Convert.ToInt32(valc) + 1).ToString();
                 
                }
                if (User.IsInRole("Franchisee"))
                {
                    tblreceptionist.franchid = Help.Franchisee();
                }
                else
                {
                    tblreceptionist.franchid = Help.Franchisee();
                }
                tblreceptionist.image = Help.uploadfile(file);
                tblreceptionist.password = tblreceptionist.password;
                db.tblreceptionists.Add(tblreceptionist);
                db.SaveChanges();
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Index");
            }

            return View(tblreceptionist);
        }

        // GET: Auth/tblreceptionists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblreceptionist tblreceptionist = await db.tblreceptionists.FindAsync(id);
            if (tblreceptionist == null)
            {
                return HttpNotFound();
            }
            return View(tblreceptionist);
        }

        // POST: Auth/tblreceptionists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,date,name,email,contact,login,password,rid,image,Type,franchid,status,StartTime,EndTime")] tblreceptionist tblreceptionist,HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                tblreceptionist.image = file != null ? Help.uploadfile(file) : img;
                #region delete file
                string fullPath = Request.MapPath("~/UploadedFiles/" + img);
                if (img == tblreceptionist.image)
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
                db.Entry(tblreceptionist).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(tblreceptionist);
        }

        // GET: Auth/tblreceptionists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblreceptionist tblreceptionist = await db.tblreceptionists.FindAsync(id);
            if (tblreceptionist == null)
            {
                return HttpNotFound();
            }
            return View(tblreceptionist);
        }

        // POST: Auth/tblreceptionists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tblreceptionist tblreceptionist = await db.tblreceptionists.FindAsync(id);
            db.tblreceptionists.Remove(tblreceptionist);
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
    }
}
