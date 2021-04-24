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
    public class FranchiseesController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/Franchisees
        public async Task<ActionResult> Index()
        {
            return View(await db.Franchisees.ToListAsync());
        }

        // GET: Auth/Franchisees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Franchisee franchisee = await db.Franchisees.FindAsync(id);
            if (franchisee == null)
            {
                return HttpNotFound();
            }
            return View(franchisee);
        }

        // GET: Auth/Franchisees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Franchisees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,date,name,email,contact,login,password,rid,image,Type,status")] Franchisee franchisee)
        {
            if (ModelState.IsValid)
            {
                db.Franchisees.Add(franchisee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(franchisee);
        }

        // GET: Auth/Franchisees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Franchisee franchisee = await db.Franchisees.FindAsync(id);
            if (franchisee == null)
            {
                return HttpNotFound();
            }
            return View(franchisee);
        }

        // POST: Auth/Franchisees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,date,name,email,contact,login,password,rid,image,Type,status")] Franchisee franchisee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(franchisee).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(franchisee);
        }

        // GET: Auth/Franchisees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Franchisee franchisee = await db.Franchisees.FindAsync(id);
            if (franchisee == null)
            {
                return HttpNotFound();
            }
            return View(franchisee);
        }

        // POST: Auth/Franchisees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Franchisee franchisee = await db.Franchisees.FindAsync(id);
            db.Franchisees.Remove(franchisee);
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
