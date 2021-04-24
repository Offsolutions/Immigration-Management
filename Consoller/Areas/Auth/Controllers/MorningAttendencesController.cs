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
    public class MorningAttendencesController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/MorningAttendences
        public async Task<ActionResult> Index()
        {
            return View(await db.MorningAttendeces.ToListAsync());
        }

        // GET: Auth/MorningAttendences/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MorningAttendence morningAttendence = await db.MorningAttendeces.FindAsync(id);
            if (morningAttendence == null)
            {
                return HttpNotFound();
            }
            return View(morningAttendence);
        }

        // GET: Auth/MorningAttendences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/MorningAttendences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Aid,Date,Rollno,Reading,Speaking,Role,Timing,franchid")] MorningAttendence morningAttendence)
        {
            if (ModelState.IsValid)
            {
                db.MorningAttendeces.Add(morningAttendence);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(morningAttendence);
        }

        // GET: Auth/MorningAttendences/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MorningAttendence morningAttendence = await db.MorningAttendeces.FindAsync(id);
            if (morningAttendence == null)
            {
                return HttpNotFound();
            }
            return View(morningAttendence);
        }

        // POST: Auth/MorningAttendences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Aid,Date,Rollno,Reading,Speaking,Role,Timing,franchid")] MorningAttendence morningAttendence)
        {
            if (ModelState.IsValid)
            {
                db.Entry(morningAttendence).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(morningAttendence);
        }

        // GET: Auth/MorningAttendences/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MorningAttendence morningAttendence = await db.MorningAttendeces.FindAsync(id);
            if (morningAttendence == null)
            {
                return HttpNotFound();
            }
            return View(morningAttendence);
        }

        // POST: Auth/MorningAttendences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MorningAttendence morningAttendence = await db.MorningAttendeces.FindAsync(id);
            db.MorningAttendeces.Remove(morningAttendence);
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
