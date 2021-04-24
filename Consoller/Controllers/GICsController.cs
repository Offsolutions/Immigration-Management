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
using Consoller.Models;

namespace Consoller.Controllers
{
    public class GICsController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string img = "",thm="",ca="";
        // GET: GICs
        public async Task<ActionResult> Index()
        {
            return View(await db.Gic.ToListAsync());
        }

        // GET: GICs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIC gIC = await db.Gic.FindAsync(id);
            if (gIC == null)
            {
                return HttpNotFound();
            }
            return View(gIC);
        }

        // GET: GICs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GICs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Gid,Date,ApplicationNo,AccountNo,BankName,Amount,Status,Fundstatus,CreatedBy")] GIC gIC)
        {
            if (ModelState.IsValid)
            {
                db.Gic.Add(gIC);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(gIC);
        }

        // GET: GICs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIC gIC = await db.Gic.FindAsync(id);
            img = gIC.Files;
            thm = gIC.Certificate;
            ca = gIC.Caq;
            if (gIC == null)
            {
                return HttpNotFound();
            }
            return View(gIC);
        }

        // POST: GICs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Gid,Date,ApplicationNo,AccountNo,BankName,Amount,Status,Fundstatus,CreatedBy,Files,Certificate,Username,Password,SecurityQuestion,Answer,Caq,CaqUser,CaqPassword,EmailId,EmailPassword,ArrimaUser,ArrimaPassword")] GIC gIC, HttpPostedFileBase file, HttpPostedFileBase thumb, HttpPostedFileBase Caq,Helper help)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (file==null || thumb==null || Caq==null)
                        {

                            gIC.Files = file != null ? help.uploadfile(file) : img;
                            gIC.Certificate = thumb != null ? help.uploadfile(thumb) : img;
                            gIC.Caq = Caq != null ? help.uploadfile(Caq) : ca;
                            db.Entry(gIC).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            Application app = db.Applications.FirstOrDefault(x => x.ApplicationNo == gIC.ApplicationNo);
                            TempData["Success"] = "Gic Account Updated Successfully";
                            transaction.Commit();
                            return RedirectToAction("Create", "Application", new { id = app.InquiryId });
                        }
                        else if(file.ContentType == "application/pdf")
                        {
                            gIC.Files = file != null ? help.uploadfile(file) : img;
                            gIC.Certificate = thumb != null ? help.uploadfile(thumb) : img;
                            gIC.Caq = Caq != null ? help.uploadfile(Caq) : ca;
                            db.Entry(gIC).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            Application app = db.Applications.FirstOrDefault(x => x.ApplicationNo == gIC.ApplicationNo);
                            TempData["Success"] = "Gic Account Updated Successfully";
                            transaction.Commit();
                            return RedirectToAction("Create", "Application", new { id = app.InquiryId });
                        }
                        else
                        {
                            TempData["danger"] = "upload only pdf file";
                            transaction.Commit();
                            return RedirectToAction("Edit", "Gics", new { id = gIC.Gid });
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        Application app = db.Applications.FirstOrDefault(x => x.ApplicationNo == gIC.ApplicationNo);
                        TempData["danger"] = "Sorry Gic Update Failed";
                        return RedirectToAction("Create", "Application", new { id = app.InquiryId });
                        throw;
                    } 
                }
            }
            return View(gIC);
        }

        // GET: GICs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIC gIC = await db.Gic.FindAsync(id);
            if (gIC == null)
            {
                return HttpNotFound();
            }
            return View(gIC);
        }

        // POST: GICs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GIC gIC = await db.Gic.FindAsync(id);
            db.Gic.Remove(gIC);
            await db.SaveChangesAsync();
            return RedirectToAction("Dashboard", "Processing", new { id = gIC.ApplicationNo });
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
