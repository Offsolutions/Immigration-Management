using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Consoller.Models;
using Consoller.Areas.Auth.Models;

namespace Consoller.Controllers
{
    public class PaymentStepsController : Controller
    {
        dbcontext db = new dbcontext();

        // GET: PaymentSteps
        public async Task<ActionResult> Index()
        {
            return View(await db.PaymentSteps.ToListAsync());
        }

        // GET: PaymentSteps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentStep paymentStep = await db.PaymentSteps.FindAsync(id);
            if (paymentStep == null)
            {
                return HttpNotFound();
            }
            return View(paymentStep);
        }

        // GET: PaymentSteps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Pid,Name,Amount")] PaymentStep paymentStep)
        {
            if (ModelState.IsValid)
            {
                db.PaymentSteps.Add(paymentStep);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paymentStep);
        }

        // GET: PaymentSteps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentStep paymentStep = await db.PaymentSteps.FindAsync(id);
            if (paymentStep == null)
            {
                return HttpNotFound();
            }
            return View(paymentStep);
        }

        // POST: PaymentSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Pid,Name,Amount")] PaymentStep paymentStep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentStep).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paymentStep);
        }

        // GET: PaymentSteps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentStep paymentStep = await db.PaymentSteps.FindAsync(id);
            if (paymentStep == null)
            {
                return HttpNotFound();
            }
            return View(paymentStep);
        }

        // POST: PaymentSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PaymentStep paymentStep = await db.PaymentSteps.FindAsync(id);
            db.PaymentSteps.Remove(paymentStep);
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
