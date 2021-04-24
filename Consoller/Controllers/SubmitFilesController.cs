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

namespace Consoller.Controllers
{
    public class SubmitFilesController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: SubmitFiles
        public async Task<ActionResult> Index()
        {
            return View(await db.SubmitFiles.ToListAsync());
        }

        // GET: SubmitFiles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubmitFile submitFile = await db.SubmitFiles.FindAsync(id);
            if (submitFile == null)
            {
                return HttpNotFound();
            }
            return View(submitFile);
        }

        // GET: SubmitFiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubmitFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Sid,SubmitDate,SubmitBy,FileDoc,ApplicationNo")] SubmitFile submitFile)
        {
            if (ModelState.IsValid)
            {
                db.SubmitFiles.Add(submitFile);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(submitFile);
        }

        // GET: SubmitFiles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubmitFile submitFile = await db.SubmitFiles.FindAsync(id);
            if (submitFile == null)
            {
                return HttpNotFound();
            }
            return View(submitFile);
        }

        // POST: SubmitFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Sid,SubmitDate,SubmitBy,FileDoc,ApplicationNo")] SubmitFile submitFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submitFile).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(submitFile);
        }

        // GET: SubmitFiles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubmitFile submitFile = await db.SubmitFiles.FindAsync(id);
            if (submitFile == null)
            {
                return HttpNotFound();
            }
            return View(submitFile);
        }

        // POST: SubmitFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubmitFile submitFile = await db.SubmitFiles.FindAsync(id);
            db.SubmitFiles.Remove(submitFile);
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
