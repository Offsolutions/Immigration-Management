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
using System.Web.Script.Serialization;
using System.Transactions;

namespace Consoller.Areas.Auth.Controllers
{
    public class studentdatasController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string img, roles = "";
       public Helper help = new  Helper();
        SQLHelper objsql = new SQLHelper();
        // GET: Auth/studentdatas
        [Authorize(Roles ="Franchisee,Admin,Receptionist")]
        public ActionResult Index()
        {
            string a = HttpContext.User.Identity.Name;

            if (User.IsInRole("Admin"))
            {
                var studentdata = db.tblstudentdata.Where(x => x.Status == true).ToList();
                return View(studentdata);
            }
            else if (User.IsInRole("Franchisee"))
            {
                var studentdata = db.tblstudentdata.Where(x => x.Status == true && x.uid == a).ToList();
                return View(studentdata);

            }
            else
            {
                tblreceptionist rr = db.tblreceptionists.Where(x => x.rid == a).First();
                var studentdata = db.tblstudentdata.Where(x => x.Status == true && x.uid == (rr.franchid).ToString()).ToList();
                return View(studentdata);
            }        
            
            
        }
        public ActionResult DeactiveStudents()
        {
            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            
                var studentdata = db.tblstudentdata.Where(x => x.Status == false && x.uid == a).ToList();
                return View(studentdata);
           
            
        }
        
        // GET: Auth/studentdatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblstudentdata tblstudentdata = db.tblstudentdata.Find(id);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            if (tblstudentdata == null)
            {
                return HttpNotFound();
            }
            return View(tblstudentdata);
        }

        // GET: Auth/studentdatas/Create
        [Authorize(Roles ="Receptionist,Franchisee")]
        public ActionResult Create()
        {
            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            tblstudentdata student = new tblstudentdata();
            student.date = System.DateTime.Now;
            ViewBag.CourseId = new SelectList(db.Courses.Where(x=>x.franchid==a), "CourseId", "CourseName");
            ViewBag.UserId = new SelectList(db.tblreceptionists.Where(x => x.franchid == a && x.Type=="Teacher"), "Id", "Name");
            ViewBag.Time = new SelectList(db.Timings.Where(x => x.franchid == a), "Tid", "BatchTime");
            return View(student);
        }

        // POST: Auth/studentdatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,rollno,name,dob,fathername,address,phone,fatherphn,language,board,qualification,coaching,institutename,type,refferedby,image,uid,Status,username,password,gender,remarks,email,discount,date,Time,UserId,Material")] tblstudentdata tblstudentdata, HttpPostedFileBase file, Helper Help,[Bind(Include ="CourseId,AdmitDate,Fees,enddate,Days")] StudentCourse scourse,int CourseId)
        {

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        scourse.CourseId = CourseId;
                        string a = help.Permission();
                        StudentCourse studentcourse = new StudentCourse();
                        #region insert student table
                        tblstudentdata studentdata = db.tblstudentdata.FirstOrDefault(x => x.uid == a && x.Status == true);
                        #region Create Rollno
                        if (studentdata == null)
                        {
                            tblstudentdata.rollno = 1;
                        }
                        else
                        {
                            var ab = db.tblstudentdata.Where(x => x.uid == a).Max(x => x.rollno);
                            tblstudentdata.rollno = Convert.ToInt32(ab) + 1;
                        }
                        #endregion
                        tblstudentdata.uid = help.Permission(); //franchid Detail
                        tblstudentdata.image = Help.uploadfile(file);

                        db.tblstudentdata.Add(tblstudentdata);
                        db.SaveChanges();
                        #endregion

                        #region Insert Fee table and studentcourse
                        var token = db.Fees_Master.Where(x => x.franchid == a && x.Status == true).Max(x => x.Token);
                        if (token != null)
                        {
                            token += 1;
                            Fees_Master feemaster = new Fees_Master();
                            feemaster.RollNo = tblstudentdata.rollno;
                            feemaster.Date = tblstudentdata.date;
                            feemaster.CourseId = scourse.CourseId;
                            //feemaster.CourseId = CourseId;
                            feemaster.AlertDate = System.DateTime.Now.AddDays(2);
                            feemaster.discount = tblstudentdata.discount;
                            feemaster.Status = tblstudentdata.Status;
                            feemaster.TotalFees = (Convert.ToInt32(scourse.Fees) * Convert.ToInt32(scourse.Days));
                            feemaster.franchid = help.Permission();
                            feemaster.Token = token;

                            db.Fees_Master.Add(feemaster);
                            db.SaveChanges();

                            
                            studentcourse.RollNo = tblstudentdata.rollno;
                            studentcourse.CourseId = scourse.CourseId;
                            studentcourse.Admitdate = scourse.Admitdate;
                            studentcourse.enddate = Convert.ToDateTime(scourse.Admitdate).AddDays(Convert.ToInt32(scourse.Days));
                            studentcourse.Fees = (Convert.ToInt32(scourse.Fees) * Convert.ToInt32(scourse.Days)).ToString();
                            studentcourse.Uid = help.Permission();
                            studentcourse.Token = token;

                            studentcourse.Status = tblstudentdata.Status;
                            db.StudentCourses.Add(studentcourse);
                            db.SaveChanges();
                        }
                        else
                        {
                            token = 1;
                            Fees_Master feemaster = new Fees_Master();
                            feemaster.RollNo = tblstudentdata.rollno;
                            feemaster.Date = tblstudentdata.date;
                            feemaster.CourseId = scourse.CourseId;
                            //feemaster.CourseId = CourseId;
                            feemaster.AlertDate = System.DateTime.Now.AddDays(2);
                            feemaster.discount = tblstudentdata.discount;
                            feemaster.Status = tblstudentdata.Status;
                            feemaster.TotalFees = (Convert.ToInt32(scourse.Fees) * Convert.ToInt32(scourse.Days));
                            feemaster.franchid = help.Permission();
                            feemaster.Token = token;

                            db.Fees_Master.Add(feemaster);
                            db.SaveChanges();


                            studentcourse.RollNo = tblstudentdata.rollno;
                            studentcourse.CourseId = scourse.CourseId;
                            studentcourse.Admitdate = scourse.Admitdate;
                            studentcourse.enddate = Convert.ToDateTime(scourse.Admitdate).AddDays(Convert.ToInt32(scourse.Days));
                            studentcourse.Fees = (Convert.ToInt32(scourse.Fees) * Convert.ToInt32(scourse.Days)).ToString();
                            studentcourse.Uid = help.Permission();
                            studentcourse.Token = token;

                            studentcourse.Status = tblstudentdata.Status;
                            db.StudentCourses.Add(studentcourse);
                            db.SaveChanges();
                        }
                        #endregion
                        #region Send Message To Student
                        tbldetail dd = db.tbldetails.FirstOrDefault();
                       //help.sendsms(studentdata.phone, "Dear " + Convert.ToString(tblstudentdata.name) + ". Welcome to " + dd.name + " . Your Course EndDate is (" + Convert.ToString(studentcourse.enddate) + "). Thanks for Joining Us.");
                        TempData["Success"] = "Saved Successfully";
                        ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.franchid == help.Permission()), "CourseId", "CourseName", scourse.CourseId);
                       // ViewBag.UserId = new SelectList(db.tblreceptionists.Where(x => x.franchid == help.Permission() && x.Type == "Teacher"), "Id", "Name", studentdata.UserId);
                      //  ViewBag.Time = new SelectList(db.Timings.Where(x => x.franchid == help.Permission()), "Tid", "BatchTime", studentdata.Time);
                        transaction.Commit();
                        return RedirectToAction("Index");
                        #endregion
                    
                        TempData["Success"] = "Student Register Successfully";
                    }

                    return View(tblstudentdata);
                }
                catch (Exception e)
                {
                    var mesaage = e.Message;
                    transaction.Rollback();
                    TempData["danger"] = "Error Occur ! "+mesaage;
                    throw;
                } 
            }
        }

        // GET: Auth/studentdatas/Edit/5
        public ActionResult Edit(int? id)
        {
            Helper help = new Helper();
            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblstudentdata tblstudentdata = db.tblstudentdata.Find(id);
            //if (tblstudentdata.password != null)
            //{
            //    tblstudentdata.password = help.DecryptData(tblstudentdata.password);
            //}
            img = tblstudentdata.image;
            ViewBag.UserId = new SelectList(db.tblreceptionists.Where(x => x.franchid == a && x.Type == "Teacher" ), "Id", "Name",tblstudentdata.UserId);
            ViewBag.Time = new SelectList(db.Timings.Where(x => x.franchid == a), "Tid", "BatchTime",tblstudentdata.Time);
      
            if (tblstudentdata == null)
            {
                return HttpNotFound();
            }
            return View(tblstudentdata);
        }

        // POST: Auth/studentdatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,rollno,name,dob,fathername,address,phone,fatherphn,language,board,qualification,coaching,institutename,type,refferedby,image,uid,Status,username,password,gender,remarks,email,discount,date,UserId,Time,Material")] tblstudentdata tblstudentdata, HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();

                tblstudentdata.image = file != null ? Help.uploadfile(file) : img;
                if (tblstudentdata.password != null)
                {
                    tblstudentdata.password = Help.EncryptData(tblstudentdata.password);
                }
                #region delete file
                string fullPath = Request.MapPath("~/UploadedFiles/" + img);
                if (img == tblstudentdata.image)
                {
                }
                else
                {
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                ViewBag.TimeId = new SelectList(db.Timings.Where(x => x.franchid == a ), "Tid", "BatchTime");
                ViewBag.CourseId = new SelectList(db.tblreceptionists.Where(x => x.franchid == a && x.Type=="Teacher"), "Id", "Name");
                #endregion
                db.Entry(tblstudentdata).State = EntityState.Modified;
                db.SaveChanges();

                objsql.ExecuteNonQuery("update fees_master set status='0' where rollno='" + tblstudentdata.rollno + "' and franchid='" + a + "'");
                objsql.ExecuteNonQuery("update studentcourses set status='0' where rollno='" + tblstudentdata.rollno + "' and uid='" + a + "'");

                //Fees_Master feemaster = db.Fees_Master.FirstOrDefault(x => x.RollNo == tblstudentdata.rollno && x.franchid==a);
                //feemaster.Status = tblstudentdata.Status;
                //db.Entry(feemaster).State = EntityState.Modified;
                //db.SaveChanges();

                //StudentCourse studentcourse = db.StudentCourses.FirstOrDefault(x => x.RollNo == tblstudentdata.rollno && x.Uid==a);
                //    studentcourse.Status = tblstudentdata.Status;
                //    db.Entry(studentcourse).State = EntityState.Modified;
                //    db.SaveChanges();

                TempData["Success"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(tblstudentdata);
        }

        // GET: Auth/studentdatas/Delete/5
        [Authorize(Roles ="Franchisee")]
        public ActionResult Delete(int? id)
        {
            Helper help = new Helper();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblstudentdata tblstudentdata = db.tblstudentdata.Find(id);
            if (tblstudentdata.password != null)
            {
                tblstudentdata.password = help.DecryptData(tblstudentdata.password);
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            if (tblstudentdata == null)
            {
                return HttpNotFound();
            }
            return View(tblstudentdata);
        }

        // POST: Auth/studentdatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Receptionist"))
            {
                roles = help.Receptionist();
            }
            else
            {
                roles = help.Franchisee();
            }
          
            tblstudentdata tblstudentdata = db.tblstudentdata.Find(id);
            int roll = tblstudentdata.rollno;
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            img = tblstudentdata.image;
            if (tblstudentdata.password != null)
            {
                tblstudentdata.password = help.DecryptData(tblstudentdata.password);
            }
            #region delete file
            string fullPath = Request.MapPath("~/UploadedFiles/" + img);
            if (img == tblstudentdata.image)
            {
            }
            else
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            #endregion
            db.tblstudentdata.Remove(tblstudentdata);
            db.SaveChanges();

            Fees_Master feemaster = new Fees_Master();
            feemaster = db.Fees_Master.Where(x => x.RollNo == roll && x.franchid==roles).FirstOrDefault();
            if (feemaster != null)
            {
                db.Fees_Master.Remove(feemaster);
                db.SaveChanges();
            }
            StudentCourse studentcourse = new StudentCourse();
            studentcourse = db.StudentCourses.Where(x => x.RollNo == roll && x.Uid == roles).FirstOrDefault();
            if (studentcourse != null)
            {
                db.StudentCourses.Remove(studentcourse);
                db.SaveChanges();
            }
            Recipt_Details rr=db.Recipt_Details.Where(x => x.RollNo == roll && x.franchid == roles).FirstOrDefault();
            if (rr != null)
            {
                db.Recipt_Details.Remove(rr);
                db.SaveChanges();
            }
            TempData["Success"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }
 
        public ActionResult View(int roll)
        {
            Session["roll"] = roll;
            return RedirectToAction("Index", "AssignCourse", new { roll = roll });
        }
        public ActionResult Deposit(int roll)
        {
            return RedirectToAction("Create", "Deposit", new { roll = roll });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Fee(int stateID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Course> Course = new List<Course>();
            string a = User.IsInRole("Franchisee") ? help.Franchisee(): help.Receptionist();
            Course = (db.Courses.Where(x => x.CourseId == stateID && x.franchid==a)).ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(Course);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
