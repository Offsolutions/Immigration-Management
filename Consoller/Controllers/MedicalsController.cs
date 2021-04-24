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
    public class MedicalsController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string img = "";
        // GET: Medicals
        public async Task<ActionResult> Index()
        {
            return View(await db.Medicals.ToListAsync());
        }

        // GET: Medicals/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicals medicals = await db.Medicals.FindAsync(id);
            if (medicals == null)
            {
                return HttpNotFound();
            }
            return View(medicals);
        }

        // GET: Medicals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Mid,ApplicationNo,Date,HospitalName,BookedBy,Note")] Medicals medicals)
        {
            if (ModelState.IsValid)
            {
                db.Medicals.Add(medicals);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(medicals);
        }

        // GET: Medicals/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicals medicals = await db.Medicals.FindAsync(id);
            img = medicals.Files;
            if (medicals == null)
            {
                return HttpNotFound();
            }
            return View(medicals);
        }

        // POST: Medicals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Mid,ApplicationNo,Date,HospitalName,BookedBy,Note,Files")] Medicals medicals, HttpPostedFileBase file, Helper help)
        {
            if (ModelState.IsValid)
            {
                medicals.Files = file != null ? help.uploadfile(file):img;
                db.Entry(medicals).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Success"] = "Medical Updated Successfully";
                return RedirectToAction("Dashboard", "Processing", new { id = medicals.ApplicationNo });
            }
            return View(medicals);
        }

        // GET: Medicals/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicals medicals = await db.Medicals.FindAsync(id);
            if (medicals == null)
            {
                return HttpNotFound();
            }
            return View(medicals);
        }

        // POST: Medicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Medicals medicals = await db.Medicals.FindAsync(id);
            db.Medicals.Remove(medicals);
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
