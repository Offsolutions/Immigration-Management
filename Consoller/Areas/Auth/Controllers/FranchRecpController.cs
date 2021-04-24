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
    public class FranchRecpController : Controller
    {
        private dbcontext db = new dbcontext();

        // GET: Auth/FranchRecp
        [Authorize(Roles ="Franchisee")]
        public async Task<ActionResult> Index()
        {
            string a =HttpContext.User.Identity.Name;
            return View(await db.tblreceptionists.Where(x=>x.franchid==a).ToListAsync());
        }

        // GET: Auth/FranchRecp/Details/5
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

        // GET: Auth/FranchRecp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/FranchRecp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,date,name,email,contact,login,password,rid,image,Type,franchid,status")] tblreceptionist tblreceptionist)
        {
            if (ModelState.IsValid)
            {

                var rid = db.tblreceptionists.Max(x => x.rid);
                tblreceptionist.rid = (Convert.ToInt32(rid) + 1).ToString();
                tblreceptionist.franchid = HttpContext.User.Identity.Name;
                db.tblreceptionists.Add(tblreceptionist);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tblreceptionist);
        }

        // GET: Auth/FranchRecp/Edit/5
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

        // POST: Auth/FranchRecp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,date,name,email,contact,login,password,rid,image,Type,franchid,status")] tblreceptionist tblreceptionist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblreceptionist).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tblreceptionist);
        }

        // GET: Auth/FranchRecp/Delete/5
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

        // POST: Auth/FranchRecp/Delete/5
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
