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
    public class TimingsController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/Timings
        public async Task<ActionResult> Index()
        {
            return View(await db.Timings.ToListAsync());
        }

        // GET: Auth/Timings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timing timing = await db.Timings.FindAsync(id);
            if (timing == null)
            {
                return HttpNotFound();
            }
            return View(timing);
        }

        // GET: Auth/Timings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Timings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Tid,BatchTime,franchid")] Timing timing,Helper help)
        {
            if (ModelState.IsValid)
            {
                
                string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
                timing.franchid = a;
                db.Timings.Add(timing);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(timing);
        }

        // GET: Auth/Timings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timing timing = await db.Timings.FindAsync(id);
            if (timing == null)
            {
                return HttpNotFound();
            }
            return View(timing);
        }

        // POST: Auth/Timings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Tid,BatchTime,franchid")] Timing timing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timing).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(timing);
        }

        // GET: Auth/Timings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timing timing = await db.Timings.FindAsync(id);
            if (timing == null)
            {
                return HttpNotFound();
            }
            return View(timing);
        }

        // POST: Auth/Timings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Timing timing = await db.Timings.FindAsync(id);
            db.Timings.Remove(timing);
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
