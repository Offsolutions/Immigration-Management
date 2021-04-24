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
    public class OfferlettersController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string img = "",imgtt="",imgrecipt="";
        // GET: Offerletters
        public async Task<ActionResult> Index()
        {
            return View(await db.Offerletter.ToListAsync());
        }

        // GET: Offerletters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offerletter offerletter = await db.Offerletter.FindAsync(id);
            if (offerletter == null)
            {
                return HttpNotFound();
            }
            return View(offerletter);
        }

        // GET: Offerletters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offerletters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Lid,ApplicationNo,Applied,Received,Cid,Oid,CCid,Yid,Mid,Amount,Show,Files")] Offerletter offerletter)
        {
            if (ModelState.IsValid)
            {
                db.Offerletter.Add(offerletter);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(offerletter);
        }

        // GET: Offerletters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offerletter offerletter = await db.Offerletter.FindAsync(id);
            img = offerletter.Files;
            imgtt = offerletter.TT;
            imgrecipt = offerletter.Recipt;
            if (offerletter == null)
            {
                return HttpNotFound();
            }
            return View(offerletter);
        }

        // POST: Offerletters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Lid,ApplicationNo,Applied,Received,Cid,Oid,CCid,Yid,Mid,Amount,Show,Files,TT,Recipt")] Offerletter offerletter,HttpPostedFileBase file, HttpPostedFileBase tt, HttpPostedFileBase recipt,Helper Help,int college,int course,Offerletter off)
        {
            if (ModelState.IsValid)
            {
                offerletter.Files = file != null ? Help.uploadfile(file) : img;
                offerletter.TT = tt != null ? Help.uploadfile(tt) : imgtt;
                offerletter.Recipt = recipt != null ? Help.uploadfile(recipt) : imgrecipt;
               // offerletter.Cid = countryid;
                offerletter.CCid = course;
                offerletter.Oid = college;
                offerletter.Mid = off.Mid;
                offerletter.Yid = off.Yid;
                db.Entry(offerletter).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Application app = db.Applications.FirstOrDefault(x => x.ApplicationNo == offerletter.ApplicationNo);
                return RedirectToAction("Create", "Application", new { id = app.InquiryId });
            }
            return View(offerletter);
        }

        // GET: Offerletters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offerletter offerletter = await db.Offerletter.FindAsync(id);
            if (offerletter == null)
            {
                return HttpNotFound();
            }
            return View(offerletter);
        }

        // POST: Offerletters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Offerletter offerletter = await db.Offerletter.FindAsync(id);
            db.Offerletter.Remove(offerletter);
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
