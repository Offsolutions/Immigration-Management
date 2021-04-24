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
    public class TrackingLogsController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string img = "";
        // GET: TrackingLogs
        public async Task<ActionResult> Index()
        {
            return View(await db.TrackingLogs.ToListAsync());
        }

        // GET: TrackingLogs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackingLogs trackingLogs = await db.TrackingLogs.FindAsync(id);
            if (trackingLogs == null)
            {
                return HttpNotFound();
            }
            return View(trackingLogs);
        }

        // GET: TrackingLogs/Create
        public ActionResult Create(int id)
        {
            TrackingLogs tl = new TrackingLogs();
            tl.ApplicationNo = id;
            tl.Date = System.DateTime.Now;
            return View(tl);
        }

        // POST: TrackingLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Tid,Tracking,Description,FileS,Pid,Show,Submitby,Date,ApplicationNo")] TrackingLogs trackingLogs, HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                trackingLogs.Tracking = db.Applications.FirstOrDefault(x => x.ApplicationNo == trackingLogs.ApplicationNo).TrackingId;
                trackingLogs.Files = Help.uploadfile(file);
                trackingLogs.Submitby = Help.Teacher();
                db.TrackingLogs.Add(trackingLogs);
                await db.SaveChangesAsync();
                return RedirectToAction("Create","TrackingLogs", new { id=trackingLogs.ApplicationNo});
            }

            return View(trackingLogs);
        }

        // GET: TrackingLogs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackingLogs trackingLogs = await db.TrackingLogs.FindAsync(id);
            img = trackingLogs.Files;
            if (trackingLogs == null)
            {
                return HttpNotFound();
            }
            return View(trackingLogs);
        }

        // POST: TrackingLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Tid,Tracking,Description,FileS,Pid,Show,Submitby,Date,ApplicationNo")] TrackingLogs trackingLogs,HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                trackingLogs.Files = file != null ? Help.uploadfile(file) : img;
                trackingLogs.Submitby = Help.Teacher();
                db.Entry(trackingLogs).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Create", "TrackingLogs", new { id = trackingLogs.ApplicationNo });
            }
            return View(trackingLogs);
        }

        // GET: TrackingLogs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackingLogs trackingLogs = await db.TrackingLogs.FindAsync(id);
            if (trackingLogs == null)
            {
                return HttpNotFound();
            }
            return View(trackingLogs);
        }

        // POST: TrackingLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TrackingLogs trackingLogs = await db.TrackingLogs.FindAsync(id);
            db.TrackingLogs.Remove(trackingLogs);
            await db.SaveChangesAsync();
            return RedirectToAction("Create", "TrackingLogs", new { id = trackingLogs.ApplicationNo });
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
