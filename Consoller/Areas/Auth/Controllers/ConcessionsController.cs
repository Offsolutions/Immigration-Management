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
    public class ConcessionsController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/Concessions
        public async Task<ActionResult> Index()
        {
            return View(await db.Concessions.ToListAsync());
        }

        // GET: Auth/Concessions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concession concession = await db.Concessions.FindAsync(id);
            if (concession == null)
            {
                return HttpNotFound();
            }
            return View(concession);
        }

        // GET: Auth/Concessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Concessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Cid,Date,ApplicationNo,InquiryId,LeaderNo,Amount,Reason,Status")] Concession concession)
        {
            if (ModelState.IsValid)
            {
                db.Concessions.Add(concession);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(concession);
        }

        // GET: Auth/Concessions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concession concession = await db.Concessions.FindAsync(id);
            if (concession == null)
            {
                return HttpNotFound();
            }
            return View(concession);
        }

        // POST: Auth/Concessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Cid,Date,ApplicationNo,InquiryId,LeaderNo,Amount,Reason,Status")] Concession concession)
        {
            if (ModelState.IsValid)
            {
                db.Entry(concession).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(concession);
        }

        // GET: Auth/Concessions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concession concession = await db.Concessions.FindAsync(id);
            if (concession == null)
            {
                return HttpNotFound();
            }
            return View(concession);
        }

        // POST: Auth/Concessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Concession concession = await db.Concessions.FindAsync(id);
            db.Concessions.Remove(concession);
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
