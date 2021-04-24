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

namespace Consoller.Areas.Auth.Controllers
{
    public class CollegesController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/Colleges
        public async Task<ActionResult> Index()
        {
            var colleges = db.Colleges.Include(c => c.Countries);
            return View(await colleges.ToListAsync());
        }

        // GET: Auth/Colleges/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College college = await db.Colleges.FindAsync(id);
            if (college == null)
            {
                return HttpNotFound();
            }
            return View(college);
        }

        // GET: Auth/Colleges/Create
        public ActionResult Create()
        {
            ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName");
            return View();
        }

        // POST: Auth/Colleges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Oid,Cid,Name,DLI,Type,Address,Status")] College college)
        {
            if (ModelState.IsValid)
            {
                college.Status = true;
                db.Colleges.Add(college);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName", college.Cid);
            return View(college);
        }

        // GET: Auth/Colleges/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College college = await db.Colleges.FindAsync(id);
            if (college == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName", college.Cid);
            return View(college);
        }

        // POST: Auth/Colleges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Oid,Cid,Name,DLI,Type,Address,Status")] College college)
        {
            if (ModelState.IsValid)
            {
                college.Status = true;
                db.Entry(college).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName", college.Cid);
            return View(college);
        }

        // GET: Auth/Colleges/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College college = await db.Colleges.FindAsync(id);
            if (college == null)
            {
                return HttpNotFound();
            }
            return View(college);
        }

        // POST: Auth/Colleges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            College college = await db.Colleges.FindAsync(id);
            db.Colleges.Remove(college);
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
