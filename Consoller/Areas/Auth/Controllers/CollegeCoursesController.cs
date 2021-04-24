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
    public class CollegeCoursesController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string code = "";
        // GET: Auth/CollegeCourses
        public async Task<ActionResult> Index()
        {

            return View(await db.CollegeCourses.ToListAsync());
        }

        // GET: Auth/CollegeCourses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollegeCourse collegeCourse = await db.CollegeCourses.FindAsync(id);
            if (collegeCourse == null)
            {
                return HttpNotFound();
            }
            return View(collegeCourse);
        }

        // GET: Auth/CollegeCourses/Create
        public ActionResult Create()
        {
            ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName");
            ViewBag.Oid = new SelectList(db.Colleges, "Oid", "Name");
            return View();
        }

        // POST: Auth/CollegeCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CCid,Cid,Oid,Name,Description,ProgramLength,ApplicationFee,TuitionFee,LivingCost,Code,Status")] CollegeCourse collegeCourse, string[] take, Intake section)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                   
                        collegeCourse.Code = uniquecode();
                        collegeCourse.Status = true;
                        db.CollegeCourses.Add(collegeCourse);

                        await db.SaveChangesAsync();
                        //  return RedirectToAction("Index");
                    
                    for (int i = 0; i <= take.Length-1; i++)
                    {
                        section.CCid = collegeCourse.Code;
                        section.IntakeName = take[i].ToString();
                        section.Status = true;
                        db.Intakes.Add(section);
                        db.SaveChanges();
                    }
                    ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName", collegeCourse.Cid);
                    ViewBag.Oid = new SelectList(db.Colleges, "Oid", "Name", collegeCourse.Oid);
                    transaction.Commit();
                    return RedirectToAction("Index","CollegeCourses");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // GET: Auth/CollegeCourses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollegeCourse collegeCourse = await db.CollegeCourses.FindAsync(id);
            if (collegeCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName", collegeCourse.Cid);
            ViewBag.Oid = new SelectList(db.Colleges, "Oid", "Name", collegeCourse.Oid);
            return View(collegeCourse);
        }

        // POST: Auth/CollegeCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CCid,Cid,Oid,Name,Description,ProgramLength,ApplicationFee,TuitionFee,LivingCost,Code,Status")] CollegeCourse collegeCourse, string[] take, Intake section)
        {
            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {
                    db.Entry(collegeCourse).State = EntityState.Modified;
                    await db.SaveChangesAsync();
             //       return RedirectToAction("Index");

                    db.Intakes.RemoveRange(db.Intakes.Where(x => x.CCid == collegeCourse.Code));
                    db.SaveChanges();
                    for (int i = 0; i <= take.Length - 1; i++)
                    {
                        section.CCid = collegeCourse.Code;
                        section.IntakeName = take[i].ToString();
                        section.Status = true;
                        db.Intakes.Add(section);
                        db.SaveChanges();
                    }
                    ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName", collegeCourse.Cid);
                    ViewBag.Oid = new SelectList(db.Colleges, "Oid", "Name", collegeCourse.Oid);
                    transaction.Commit();
                    return View(collegeCourse);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // GET: Auth/CollegeCourses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollegeCourse collegeCourse = await db.CollegeCourses.FindAsync(id);
            if (collegeCourse == null)
            {
                return HttpNotFound();
            }
            return View(collegeCourse);
        }

        // POST: Auth/CollegeCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CollegeCourse collegeCourse = await db.CollegeCourses.FindAsync(id);
            db.CollegeCourses.Remove(collegeCourse);
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
        public int uniquecode()
        {
            CollegeCourse coursecode = db.CollegeCourses.FirstOrDefault();
          
            if (coursecode == null)
            {
                coursecode.Code = 1050;
            }
            else
            {
                var ab = db.CollegeCourses.Max(x => x.Code);
                coursecode.Code = Convert.ToInt32(ab) + 1;
            }
            return coursecode.Code;
        }
    }
}
