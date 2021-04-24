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
    public class AssignCourseController : Controller
    {
        private dbcontext db = new dbcontext();
        public static int rollno, last;
        Helper help = new Helper();
        SQLHelper objsql = new SQLHelper();
        // GET: Auth/AssignCourse
        public ActionResult Index(int roll)
        {
            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            rollno = roll;
            TempData["roll"] = roll;
            var course = db.Courses.ToList();
            var room = db.tblrooms.ToList();
            return View(db.StudentCourses.Where(x => x.RollNo == roll && x.Uid==a).ToList());
        }

        // GET: Auth/AssignCourse/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourse student_Course = db.StudentCourses.Find(id);
            if (student_Course == null)
            {
                return HttpNotFound();
            }
            return View(student_Course);
        }

        // GET: Auth/AssignCourse/Create
        public ActionResult Create()
        {
            string a = help.Permission();
            if (a != null)
            {
                ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.franchid == a), "CourseId", "CourseName");
                ViewBag.RoomId = new SelectList(db.tblrooms, "RoomId", "room");
                ViewBag.rollno = rollno;
                StudentCourse ss = new StudentCourse();
                ss.RollNo = rollno;
                return View(ss);
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }
        }

        // POST: Auth/AssignCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RollNo,CourseId,Admitdate,enddate,Fees,Uid,RoomId,Status,Days")] StudentCourse student_Course)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    string a = help.Permission();
                    try
                    {
                        //StudentCourse sc = db.StudentCourses.FirstOrDefault(x => x.CourseId == student_Course.CourseId && x.RollNo == rollno && x.Uid == a);
                        //Fees_Master fm = db.Fees_Master.FirstOrDefault(x => x.CourseId == student_Course.CourseId && x.RollNo == rollno && x.franchid == a);
                        //if (sc!=null)
                        //{
                        //    objsql.ExecuteNonQuery("update studentcourses set status='0' where id='" + sc.Id + "'");
                        //    objsql.ExecuteNonQuery("update fees_master set status='0' where id='" + fm.Id + "'");
                        //}
                        var token = db.Fees_Master.Where(x => x.franchid == a && x.Status == true).Max(x => x.Token);
                        if (token != null)
                        {

                            token += 1;
                            //string a= User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
                            student_Course.Uid = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
                            student_Course.enddate = Convert.ToDateTime(student_Course.Admitdate).AddDays(student_Course.Days);
                            student_Course.Fees = (Convert.ToInt32(student_Course.Fees)).ToString();
                            student_Course.RollNo = rollno;
                            student_Course.Token = token;
                            db.StudentCourses.Add(student_Course);
                            db.SaveChanges();

                            Fees_Master feemaster1 = new Fees_Master();
                            feemaster1.RollNo = student_Course.RollNo;
                            feemaster1.Date = System.DateTime.Now;
                            feemaster1.CourseId = student_Course.CourseId;
                            feemaster1.AlertDate = System.DateTime.Now.AddDays(2);
                            feemaster1.discount = 0;
                            feemaster1.Status = true;
                            feemaster1.TotalFees = int.Parse(student_Course.Fees);
                            feemaster1.franchid = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
                            feemaster1.Token = token;
                            // feemaster.franchid= HttpContext.User.Identity.Name;
                            db.Fees_Master.Add(feemaster1);
                            db.SaveChanges();


                            dbTran.Commit();
                            TempData["Success"] = "Saved Successfully";
                            return RedirectToAction("Index", new { roll = rollno });
                        }
                        else
                        {
                            dbTran.Rollback();
                            TempData["danger"] = "Invalid Token ! Conceren With Developer";

                        }
                    }
                    catch (Exception)
                    {
                        dbTran.Rollback();
                        return RedirectToAction("Index", new { roll = rollno });
                        
                        throw;
                    }
                }
            }

            return View(student_Course);
        }

        // GET: Auth/AssignCourse/Edit/5
        public ActionResult Edit(int? id)
        {
            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourse student_Course = db.StudentCourses.FirstOrDefault(x=>x.Id==id && x.Status==true);
            rollno = student_Course.RollNo;
            last = Convert.ToInt32(student_Course.Fees);
            ViewBag.CourseId = new SelectList(db.Courses.Where(x=>x.franchid==a), "CourseId", "CourseName", student_Course.CourseId);
            ViewBag.RoomId = new SelectList(db.tblrooms, "RoomId", "room");
            if (student_Course == null)
            {
                return HttpNotFound();
            }
            return View(student_Course);
        }

        // POST: Auth/AssignCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseId,Admitdate,enddate,Fees,RoomId,Status,Days,Token")] StudentCourse student_Course)
        {
            if (ModelState.IsValid)
            {
                string a = help.Permission();
                Course cc = db.Courses.First(x=>x.CourseId==student_Course.CourseId && x.franchid==a);
                student_Course.RollNo = rollno;
                student_Course.Uid = a;
                student_Course.enddate = Convert.ToDateTime(student_Course.Admitdate).AddDays(student_Course.Days);
                student_Course.Fees = (Convert.ToInt32(cc.Fees) * Convert.ToInt32(student_Course.Days)).ToString();
                db.Entry(student_Course).State = EntityState.Modified;
                db.SaveChanges();

                Fees_Master feemaster = db.Fees_Master.FirstOrDefault(x => x.RollNo == rollno && x.franchid==a && x.Token==student_Course.Token && x.Status==true);
                //feemaster.RollNo = student_Course.RollNo;
                feemaster.Date = System.DateTime.Now;
                feemaster.CourseId = student_Course.CourseId;
                feemaster.AlertDate = System.DateTime.Now.AddDays(2);
                feemaster.Status = true;
                feemaster.TotalFees = (Convert.ToInt32(cc.Fees) * Convert.ToInt32(student_Course.Days));
              //  feemaster.TotalFees += Convert.ToInt32(student_Course.Fees);
                db.Entry(feemaster).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Updated Successfully";
                ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.franchid == a), "CourseId", "CourseName");
                return RedirectToAction("Index", new { roll = rollno });
            }
            return View(student_Course);
        }

        // GET: Auth/AssignCourse/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentCourse student_Course = db.StudentCourses.Find(id);
            rollno = student_Course.RollNo;
            if (student_Course == null)
            {
                return HttpNotFound();
            }
            return View(student_Course);
        }

        // POST: Auth/AssignCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
                    StudentCourse student_Course = db.StudentCourses.Find(id);
                    Fees_Master feemaster = db.Fees_Master.FirstOrDefault(x => x.RollNo == student_Course.RollNo && x.franchid==a && x.CourseId==student_Course.CourseId);
                    db.Fees_Master.Remove(feemaster);
                    db.SaveChanges();
                    db.StudentCourses.Remove(student_Course);
                    db.SaveChanges();
                    dbTran.Commit();
                    TempData["Success"] = "Deleted Successfully";
                    return RedirectToAction("Index", new { roll = rollno });
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    return RedirectToAction("Index", new { roll = rollno });
                    throw;
                }
            }
                
          
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Active(int id)
        {
            StudentCourse sc = db.StudentCourses.FirstOrDefault(x=>x.Token==id);
            objsql.ExecuteNonQuery("update studentcourses set status='1' where token='" + id + "'");
            objsql.ExecuteNonQuery("update fees_master set status='1' where token='" + id + "'");
            return RedirectToAction("Index", "AssignCourse", new { roll = sc.RollNo });
        }
        public ActionResult DeActive(int id)
        {
            StudentCourse sc = db.StudentCourses.FirstOrDefault(x => x.Token == id);
            objsql.ExecuteNonQuery("update studentcourses set status='0' where token='" + id + "'");
            objsql.ExecuteNonQuery("update fees_master set status='0' where token='"+id+"'");
            return RedirectToAction("Index", "AssignCourse", new { roll = sc.RollNo });
        }
    }
}
