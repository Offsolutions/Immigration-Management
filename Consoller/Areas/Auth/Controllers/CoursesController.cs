using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;

namespace Consoller.Areas.Auth.Controllers
{
    public class CoursesController : Controller
    {
        private dbcontext db = new dbcontext();
        public static int coursid;
        Helper help = new Helper();
        // GET: Auth/Courses
        public ActionResult Index()
        {
            if(User.IsInRole("Franchisee"))
            {
                string a = help.Franchisee();
                return View(db.Courses.Where(x=>x.franchid==a).ToList());
            }
            else
            {
                string a = help.Receptionist();
                return View(db.Courses.Where(x => x.franchid == a).ToList());

            }
           
        }

        // GET: Auth/Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Auth/Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseId,CourseName,Fees")] Course course)
        {
            if (ModelState.IsValid)
            {
                string a = User.IsInRole("franchisee") ? help.Franchisee() : help.Receptionist();
                Course caos = db.Courses.FirstOrDefault(x=>x.franchid==a);
                if (caos == null)
                {
                    course.CourseId = 1001;
                }
                else
                {
                    var ab = db.Courses.Where(x=>x.franchid==a).Max(x => x.CourseId);
                    course.CourseId = (Convert.ToInt32(ab) + 1);
                }
                course.franchid = a;
                db.Courses.Add(course);
                db.SaveChanges();
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Auth/Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            coursid = course.CourseId;
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Auth/Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseId,CourseName,Fees,franchid")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseId = coursid;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Auth/Courses/Delete/5
        [Authorize(Roles ="Franchisee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Auth/Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            TempData["Success"] = "Deleted Successfully";
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
