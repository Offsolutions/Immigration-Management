using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;
using System.Data.Entity;
using System.Net;
using System.Transactions;
using System.Data;

namespace Consoller.Areas.Auth.Controllers
{
    public class DepositController : Controller
    {
        public dbcontext db = new dbcontext();
        public static int rollno, recp1=0;
        public static string receiptno;
        Helper help = new Helper();
        SQLHelper objsql = new SQLHelper();
        // GET: Auth/Deposit
        public ActionResult Index(int roll)
        {
            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            TempData["roll"] = roll;
            Session["roll"] = roll;
            var recp = db.Recipt_Details.Where(x => x.RollNo == roll && x.franchid==a && x.Active==true).ToList();
            return View(recp);
        }

        // GET: Auth/Deposit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Auth/Deposit/Create
        public ActionResult Create(int roll)
        {
            TempData["roll"] = roll;
            Session["roll"] = roll;
            rollno = roll;
            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            StudentCourse course = db.StudentCourses.Where(x => x.RollNo == roll && x.Status == true).FirstOrDefault();
            var courses = db.Courses.Where(x => x.CourseId == course.CourseId && x.franchid==a);
            var list = db.StudentCourses.Where(x=>x.RollNo==roll && x.Uid==a && x.Status==true)
                .Join(db.Courses.Where(x=>x.franchid==a),
                        c => c.CourseId,
                        o => o.CourseId,
                        (c, o) => new
                        {
                            CourseId = o.CourseId,
                            CourseName = o.CourseName
                        });
            ViewBag.CourseId = new SelectList(list, "CourseId", "CourseName");
     //       ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.franchid == a), "CourseId", "CourseName");
            ViewBag.RollNo = roll;
            Recipt_Details receiptd = db.Recipt_Details.FirstOrDefault(x=>x.franchid==a);
            if (receiptd == null)
            {
                
                var receip = db.tblReceipt.FirstOrDefault();
                if (receip == null)
                {
                    ViewBag.Receipt = 1;
                }
                else
                {
                    var recp = receip.Start_no;
                    ViewBag.Receipt = recp;
                    recp1 =Convert.ToInt32(recp);
                }
            }
            else
            {
                // Recipt_Details ab=db.Recipt_Details.Where(x => x.franchid == a).Max();
                //DataTable dt = new DataTable();
                //dt= objsql.GetTable("select reciptno from Recipt_Details where id=(select max(id) from Recipt_Details where franchid='" + a + "')");
                //if(dt.Rows.Count>0)
                //{

                //}
                //else
                //{

                //}
                string ab = objsql.GetSingleValue("select reciptno from Recipt_Details where id=(select max(id) from Recipt_Details where franchid='"+a+"')").ToString();
              //  var ab = db.Recipt_Details.Where(x => x.franchid == a).Max(x => x.ReciptNo);
                ViewBag.Receipt = Convert.ToInt32(ab) + 1;
            }
            Recipt_Details rr = new Recipt_Details();
            receiptno = ViewData["Receipt"].ToString();
            rr.Date = System.DateTime.Now;
            
            return View(rr);
        }

        // POST: Auth/Deposit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="Type")]  Recipt_Details receiptdetail, int Amount, int Discount, int CourseId, DateTime? Alert, DateTime date,Helper Help)
        {
            using (TransactionScope ts=new TransactionScope ())
            {
                try
                {
                    DateTime today = System.DateTime.Now;
                    DataTable dt = objsql.GetTable("select * from expenselocks where lockdate='" + today + "'");

                    if (dt.Rows.Count > 0)
                    {

                        TempData["danger"] = "Sorry DayEnd";
                        return View();
                    }
                    else
                    {
                        string franchid = help.Permission();
                        tblreceptionist re = db.tblreceptionists.FirstOrDefault(x => x.rid == franchid);
                        if (re.StartTime != null || re.EndTime != null)
                        {

                            if (help.Checklock() == true)
                            {

                                #region data
                                string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
                                // TODO: Add insert logic here
                                Fees_Master feesmaster = db.Fees_Master.FirstOrDefault(x => x.RollNo == rollno && x.franchid == a && x.Status == true && x.CourseId == CourseId);
                                int pending = (Convert.ToInt32(feesmaster.TotalFees) - Convert.ToInt32(feesmaster.PaidFees));
                                if (feesmaster.PaidFees < feesmaster.TotalFees)
                                {

                                    #region condition greater than
                                    feesmaster.discount = (Convert.ToInt32(feesmaster.discount) + Convert.ToInt32(Discount));
                                    feesmaster.Date = date;
                                    feesmaster.AlertDate = Alert;
                                    feesmaster.PaidFees += Amount;
                                    feesmaster.Status = true;
                                    db.Entry(feesmaster).State = EntityState.Modified;
                                    db.SaveChanges();

                                    receiptdetail.RollNo = rollno;
                                    receiptdetail.ReciptNo = receiptno;
                                    receiptdetail.CourseId = CourseId;
                                    receiptdetail.Date = date;
                                    receiptdetail.Amount = Amount;
                                    receiptdetail.Active = true;
                                    receiptdetail.franchid = User.IsInRole("Franchisee") ? Help.Franchisee() : Help.Receptionist();
                                    db.Recipt_Details.Add(receiptdetail);
                                    db.SaveChanges();
                                    TempData["roll"] = rollno;
                                    ViewData["Receipt"] = recp1;
                                    TempData["Success"] = "Saved Successfully";
                                    tblsms sms = db.tblsms.FirstOrDefault();
                                    tblstudentdata ss = db.tblstudentdata.FirstOrDefault(x => x.rollno == rollno);

                                    if (ss.phone != null)
                                    {
                                        if (sms != null)
                                        {
                                            string msg = "Dear, " + Convert.ToString(ss.name) + ". Thank You for Deposit Rs." + Amount + ". Thanks for Joining Us.";
                                            string result = Help.apicall("http://sms.sms.officialsolutions.in/sendSMS?username=" + sms.Username + "&message=" + msg + "&sendername=" + sms.Senderid + "&smstype=TRANS&numbers=" + ss.phone + "&apikey=" + sms.Api + "");
                                        }
                                        TempData["Success"] = "SMS Send Successfully";
                                    }
                                    StudentCourse course = db.StudentCourses.Where(x => x.RollNo == rollno && x.Status == true).FirstOrDefault();
                                    var courses = db.Courses.Where(x => x.CourseId == course.CourseId);

                                    ViewBag.CourseId = new SelectList(courses, "CourseId", "CourseName");
                                    ts.Complete();
                                    ts.Dispose();
                                    return RedirectToAction("invoice", new { id = receiptno });
                                    #endregion


                                }
                                else
                                {
                                    TempData["danger"] = "Please Check Amount First";
                                    return View();
                                }
                                #endregion
                            }
                            else
                            {
                                TempData["danger"] = "Sorry DayEnd";
                                return View();
                            }
                        }
                        else
                        {
                            #region data
                            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
                            // TODO: Add insert logic here
                            Fees_Master feesmaster = db.Fees_Master.FirstOrDefault(x => x.RollNo == rollno && x.franchid == a && x.Status == true && x.CourseId == CourseId);
                            int pending = (Convert.ToInt32(feesmaster.TotalFees) - Convert.ToInt32(feesmaster.PaidFees));
                            if (feesmaster.PaidFees < feesmaster.TotalFees)
                            {

                                #region condition greater than
                                feesmaster.discount = (Convert.ToInt32(feesmaster.discount) + Convert.ToInt32(Discount));
                                feesmaster.Date = date;
                                feesmaster.AlertDate = Alert;
                                feesmaster.PaidFees += Amount;
                                feesmaster.Status = true;
                                db.Entry(feesmaster).State = EntityState.Modified;
                                db.SaveChanges();

                                receiptdetail.RollNo = rollno;
                                receiptdetail.ReciptNo = receiptno;
                                receiptdetail.CourseId = CourseId;
                                receiptdetail.Date = date;
                                receiptdetail.Amount = Amount;
                                receiptdetail.Active = true;
                                receiptdetail.franchid = User.IsInRole("Franchisee") ? Help.Franchisee() : Help.Receptionist();
                                db.Recipt_Details.Add(receiptdetail);
                                db.SaveChanges();
                                TempData["roll"] = rollno;
                                ViewData["Receipt"] = recp1;
                                TempData["Success"] = "Saved Successfully";
                                tblsms sms = db.tblsms.FirstOrDefault();
                                tblstudentdata ss = db.tblstudentdata.FirstOrDefault(x => x.rollno == rollno);

                                if (ss.phone != null)
                                {
                                    if (sms != null)
                                    {
                                        string msg = "Dear, " + Convert.ToString(ss.name) + ". Thank You for Deposit Rs." + Amount + ". Thanks for Joining Us.";
                                        string result = Help.apicall("http://sms.sms.officialsolutions.in/sendSMS?username=" + sms.Username + "&message=" + msg + "&sendername=" + sms.Senderid + "&smstype=TRANS&numbers=" + ss.phone + "&apikey=" + sms.Api + "");
                                    }
                                    TempData["Success"] = "SMS Send Successfully";
                                }
                                StudentCourse course = db.StudentCourses.Where(x => x.RollNo == rollno && x.Status == true).FirstOrDefault();
                                var courses = db.Courses.Where(x => x.CourseId == course.CourseId);

                                ViewBag.CourseId = new SelectList(courses, "CourseId", "CourseName");
                                ts.Complete();
                                ts.Dispose();
                                return RedirectToAction("invoice", new { id = receiptno });
                                #endregion


                            }
                            else
                            {
                                TempData["danger"] = "Please Check Amount First";
                                return View();
                            }
                            #endregion
                        }


                    }
                }
                catch (Exception a)
                {
                    return View();
                } 
            }
        }

        // GET: Auth/Deposit/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Auth/Deposit/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Auth/Deposit/Delete/5
        [Authorize(Roles ="Franchisee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipt_Details receiptdetail = db.Recipt_Details.Find(id);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            if (receiptdetail == null)
            {
                return HttpNotFound();
            }
            return View(receiptdetail);
        }

        // POST: Auth/Inquiry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipt_Details receiptdetail = db.Recipt_Details.Find(id);
            db.Recipt_Details.Remove(receiptdetail);
            db.SaveChanges();

            Fees_Master feesmaster = db.Fees_Master.FirstOrDefault(x => x.RollNo == receiptdetail.RollNo && x.CourseId==receiptdetail.CourseId && x.Status==true && x.franchid==receiptdetail.franchid);
            feesmaster.PaidFees = (Convert.ToInt32(feesmaster.PaidFees) - Convert.ToInt32(receiptdetail.Amount));
            db.Entry(feesmaster).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Success"] = "Deleted Successfully";
            return RedirectToAction("Index", new { roll = receiptdetail.RollNo });
        }
        //public ActionResult invoice(int id)
        //{
        //    return View();
        //}
        public ActionResult invoice(int id)
        {
            Helper help = new Helper();
            string a = id.ToString();
            string franch = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            Recipt_Details receiptdetail = db.Recipt_Details.FirstOrDefault(x => x.ReciptNo == a && x.franchid==franch);
            ViewBag.courseid = receiptdetail.CourseId;
            return View(receiptdetail);
        }
    }
}
