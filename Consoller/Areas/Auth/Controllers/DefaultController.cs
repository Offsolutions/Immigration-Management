using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using System.Dynamic;
using onlineportal.Areas.AdminPanel.Models;
using System.Data.Entity;
using System.Data;
using Newtonsoft.Json;
using Consoller.Models;
using System.Globalization;

namespace Consoller.Areas.Auth.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Auth/Default
        private dbcontext db = new dbcontext();
        DateTime ab1 = System.DateTime.Now.AddDays(-3);
        SQLHelper objsql = new SQLHelper();
        Consoller.Models. Helper help = new Consoller.Models.Helper();
        public int e = 0;
        public ActionResult Index()
        {
            List<ExpenseReport> Reports = new List<ExpenseReport>();
            DataTable dt = new DataTable();
            // DateTime todaydate = System.DateTime.Now.ToShortDateString();
            int expense = Convert.ToInt32(objsql.GetSingleValue("select COALESCE(sum(amount),0) from expenses where date='" + System.DateTime.Now.ToShortDateString() + "'"));
            int immfee = Convert.ToInt32(objsql.GetSingleValue("select COALESCE(sum(amount),0) from ApplicationRecipts where date='" + System.DateTime.Now.ToShortDateString() + "'"));
            int ieltsfee = Convert.ToInt32(objsql.GetSingleValue("select COALESCE(sum(amount),0) from Recipt_Details where date='" + System.DateTime.Now.ToShortDateString() + "'"));

            //var immfee = db.ApplicationRecipts.Where(x => x.Date == System.DateTime.Now.ToShortDateString()).Select(x=>x.Amount).DefaultIfEmpty(0).Sum();
            //var ieltsfee = db.Recipt_Details.Where(x => x.Date == System.DateTime.Now).Select(x=>x.Amount).DefaultIfEmpty(0).Sum();
            int total = (Convert.ToInt32(immfee) + Convert.ToInt32(ieltsfee));
            double profit = total - expense;
            Reports.Add(new ExpenseReport("Expense", expense));
            Reports.Add(new ExpenseReport("Total", total));
            Reports.Add(new ExpenseReport("Profit", profit));

            ViewBag.Report = JsonConvert.SerializeObject(Reports);
            var lastSixMonths = Enumerable.Range(0, 6).Select(i => DateTime.Now.AddMonths(i - 6).ToString("MM"));

            List<DataPoint> dataPoints1 = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();
            List<DataPoint> dataPoints3 = new List<DataPoint>();
            foreach (var monthAndYear in lastSixMonths)
            {
                e += 1;
                int a = Convert.ToInt32(monthAndYear);
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(a);
                int expense2 = Convert.ToInt32(objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM expenses WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, -" + e+", getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, -"+e+", getdate()))"));
                int immigration = Convert.ToInt32(objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM ApplicationRecipts WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, -" + e + ", getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, -" + e + ", getdate()))"));
                int ielts = Convert.ToInt32(objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM Recipt_Details WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, -" + e + ", getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, -" + e + ", getdate()))"));


                dataPoints1.Add(new DataPoint(monthName, expense2));
                dataPoints2.Add(new DataPoint(monthName, immigration));
                dataPoints3.Add(new DataPoint(monthName, ielts));
            }
      

            

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);


            return View();
        }
        public ActionResult continuecourse(int id)
        {
            string franch = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            StudentCourse cc = db.StudentCourses.FirstOrDefault(x => x.RollNo == id && x.Uid==franch); // coursr id
            DateTime dateTime1 =Convert.ToDateTime(cc.Admitdate);
            DateTime dateTime2 = Convert.ToDateTime(cc.enddate);
            TimeSpan difference = dateTime2 - dateTime1;
            var days = difference.TotalDays;
            //  string newdate = dateTime2.AddDays(days).ToShortDateString();
            DateTime newdate = dateTime2.AddDays(days);
            var fee = Convert.ToInt32(cc.Fees) * 2;
            SQLHelper objsql = new SQLHelper();
            cc.enddate = newdate;
            cc.Fees =fee.ToString();
            db.Entry(cc).State = EntityState.Modified;
            db.SaveChanges();
            // objsql.ExecuteNonQuery("update StudentCourses set enddate='" + newdate + "',fees='" + fee + "' where rollno='" + id + "' and courseid='" + cc.CourseId + "'");
            objsql.ExecuteNonQuery("update fees_master set totalfees='" + fee + "' where rollno='" + id + "' and franchid='"+franch+"'");
            return RedirectToAction("Index"); 
        }
        public ActionResult CourseFinishAlert()

        {
            // var studentcourse = db.StudentCourses.Where(x => x.Status == true && x.enddate == System.DateTime.Now.AddDays(-3) || x.enddate == System.DateTime.Now.AddDays(-2) || x.enddate == System.DateTime.Now.AddDays(-1) || x.enddate == System.DateTime.Now);

            //var studentcourse = db.StudentCourses.Where(x => x.Status == true && x.enddate == System.DateTime.Now.AddDays(-3));
            //return View(studentcourse);
            string franch = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            var ab = System.DateTime.Now.AddDays(3);
            DateTime c = Convert.ToDateTime(ab);
           // var studentcourse = db.StudentCourses.Where(x => x.Status == true && x.enddate <= c && x.Status == true && x.Uid==franch).ToList();
            List<StudentCourse> sturecord = new List<StudentCourse>();
            //var receiptdetail = from tblstudentdata in db.tblstudentdata
            //                    join StudentCourse in db.StudentCourses on tblstudentdata.rollno equals StudentCourse.RollNo
            //                    where tblstudentdata.Status == true && StudentCourse.RollNo == tblstudentdata.rollno && StudentCourse.enddate<=c
            var receiptdetail = from StudentCourse in db.StudentCourses.Where(x=>x.Uid==franch && x.Status==true)
                                join tblstudentdata in db.tblstudentdata.Where(x=>x.uid== franch && x.Status == true) on StudentCourse.RollNo equals tblstudentdata.rollno
                                where StudentCourse.enddate <= c
                                select new
                                {
                                    ID = StudentCourse.Id,
                                    Roll = StudentCourse.RollNo,
                                    Enddate = StudentCourse.enddate,
                                    Course = StudentCourse.CourseId,
                                   
                                    


                                };
            foreach (var item in receiptdetail)
            {
                sturecord.Add(new StudentCourse()
                {
                    Id = item.ID,
                    RollNo = item.Roll,
                    enddate = item.Enddate,
                    CourseId = item.Course,
                    
                });
            }
            //var a = db.StudentCourses.Where(x => x.Status == true && x.enddate <= c && x.Status == true && x.Uid == franch).ToList();
            //return View(db.StudentCourses.Where(x => x.Status == true && x.enddate <= c && x.Status == true && x.Uid== franch).ToList());
            return View(sturecord);
        }
        public ActionResult FeeAlert()
        {
            //var ab = System.DateTime.Now.AddDays(-3);
            //var c = Convert.ToDateTime(ab);
            ////var cour = db.Fees_Master.Where(x=>x.Status==true).Join(db.tblstudentdata, c => c.Status == true,c.R
            //var feemaster = from Fees_Master in db.Fees_Master
            //                join tblstudentdata in db.tblstudentdata on Fees_Master.RollNo equals tblstudentdata.rollno where tblstudentdata.Status==true && Fees_Master.AlertDate<= c
            //                select new
            //                {
            //                    ID=Fees_Master.Id,Fees_Master.RollNo,Fees_Master.PaidFees,Fees_Master.AlertDate,Fees_Master.discount,Fees_Master.TotalFees,tblstudentdata.name,tblstudentdata.fathername,tblstudentdata.phone
            //                };
            //return View(feemaster.ToList());
            return View();
        }
        public ActionResult Search()
        {
            return View(db.StudentCourses.Where(x => x.RollNo == 0).ToList());
        }
        [HttpPost]
        public ActionResult Search(string search, StudentCourse student)
        {
            int roll = Convert.ToInt32(search);
            List<StudentCourse> Studentcourse = new List<StudentCourse>();
            Studentcourse = db.StudentCourses.Where(x => x.RollNo == roll).ToList();
            ViewBag.RollNo = search;
            return View(Studentcourse);
        }
        public ActionResult StudentReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StudentReport(DateTime fromdate, DateTime todate, Stu_rec sturec)
        {

            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            

                //dynamic expando = new ExpandoObject();
                //var marksModel = expando as IDictionary<string, object>;
                List<Stu_rec> sturecord = new List<Stu_rec>();
            var receiptdetail = from Recipt_Details in db.Recipt_Details.Where(x=>x.franchid==a)
                                join tblstudentdata in db.tblstudentdata.Where(x=>x.uid==a) on Recipt_Details.RollNo equals tblstudentdata.rollno
                                join Course in db.Courses.Where(x=>x.franchid==a) on Recipt_Details.CourseId equals Course.CourseId
                                where tblstudentdata.Status == true && Recipt_Details.Date >= fromdate && Recipt_Details.Date <= todate
                                select new
                                {
                                    ID = Recipt_Details.Id,
                                    Recipt_Details.RollNo,
                                    Recipt_Details.Amount,
                                    Recipt_Details.Date,
                                    Recipt_Details.ReciptNo,
                                    Course.CourseName,
                                    tblstudentdata.name,

                                };
            foreach (var item in receiptdetail)
            {
                sturecord.Add(new Stu_rec()
                {
                    id = item.ID,
                    name = item.name,
                    RollNo=item.RollNo,
                    Amount=item.Amount,
                    CourseName=item.CourseName,
                    Date=item.Date,
                    ReciptNo=item.ReciptNo
                });
            }
            return View(sturecord);
          
        }
        public ActionResult TotalReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TotalReport(DateTime fromdate, DateTime todate, Stu_rec sturec)
        {

            //string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();


            ////dynamic expando = new ExpandoObject();
            ////var marksModel = expando as IDictionary<string, object>;
            //List<Stu_rec> sturecord = new List<Stu_rec>();
            //var receiptdetail = from Recipt_Details in db.Recipt_Details.Where(x => x.franchid == a)
            //                    join tblstudentdata in db.tblstudentdata.Where(x => x.uid == a) on Recipt_Details.RollNo equals tblstudentdata.rollno
            //                    join Course in db.Courses.Where(x => x.franchid == a) on Recipt_Details.CourseId equals Course.CourseId
            //                    where tblstudentdata.Status == true && Recipt_Details.Date >= fromdate && Recipt_Details.Date <= todate
            //                    select new
            //                    {
            //                        ID = Recipt_Details.Id,
            //                        Recipt_Details.RollNo,
            //                        Recipt_Details.Amount,
            //                        Recipt_Details.Date,
            //                        Recipt_Details.ReciptNo,
            //                        Course.CourseName,
            //                        tblstudentdata.name,

            //                    };
            //foreach (var item in receiptdetail)
            //{
            //    sturecord.Add(new Stu_rec()
            //    {
            //        id = item.ID,
            //        name = item.name,
            //        RollNo = item.RollNo,
            //        Amount = item.Amount,
            //        CourseName = item.CourseName,
            //        Date = item.Date,
            //        ReciptNo = item.ReciptNo
            //    });
            //}
            return View(db.tblstudentdata.Where(x=>x.Status==true && x.date>=fromdate && x.date<=todate).OrderBy(x=>x.date));

        }

        public ActionResult Follow(int id)
        {

            db.Configuration.ProxyCreationEnabled = false;
            tblfeedback lstcity = new tblfeedback();
           
            var a = db.tblfeedback.Where(x=>x.Id==id).First();
            lstcity.inquiryid = a.inquiryid;
            ViewBag.EmployeeList = lstcity;
           
            return PartialView("~/Areas/Auth/Views/Default/FollowUp.cshtml", lstcity);
        }
        public ActionResult increasedays(int id)
        {

            db.Configuration.ProxyCreationEnabled = false;
            StudentCourse lstcity = new StudentCourse();
           
            lstcity= db.StudentCourses.Where(x => x.Id == id).First();
          //  lstcity.rollno = a.rollno;
            ViewBag.EmployeeList = lstcity;

            return PartialView("~/Areas/Auth/Views/Default/Days.cshtml", lstcity);
        }
        public ActionResult followCreate([Bind(Include = "Id,date,inquiryid,feedback,days,type,nextfollow,status,loginid")] tblfeedback tblfeedback)
        {
            objsql.ExecuteNonQuery("update tblfeedbacks set status='0' where inquiryid='" + tblfeedback.inquiryid + "'");
            tblfeedback.nextfollow = System.DateTime.Now.AddDays(tblfeedback.days);
            tblfeedback.loginid = User.IsInRole("Franchisee") ? help.Receptionist() : help.Franchisee();
            db.tblfeedback.Add(tblfeedback);

            db.SaveChanges();
            TempData["Success"] = "Update Inquiry Successfully";
            return RedirectToAction("Index", "Auth/Default/Index");
        }
        public ActionResult Admin()
        {
            return View(db.tblreceptionists.Where(x=>x.Type=="Franchisee"));
        }
        public ActionResult AddDays(StudentCourse course)
        {
            string a = help.Permission();
            StudentCourse ss = db.StudentCourses.FirstOrDefault(x => x.Token == course.Token && x.Uid == a && x.Status==true);
            Course cc = db.Courses.FirstOrDefault(x => x.CourseId == ss.CourseId && x.franchid == a);
            string daysfee = (Convert.ToInt32(course.Days) * Convert.ToInt32(cc.Fees)).ToString();
            string fee = (Convert.ToInt32(ss.Fees) + Convert.ToInt32(daysfee)).ToString();
            ss.Fees = fee;
            ss.enddate = Convert.ToDateTime(ss.enddate).AddDays(course.Days);
            db.Entry(ss).State = EntityState.Modified;
            db.SaveChanges();
            Fees_Master fm = db.Fees_Master.FirstOrDefault(x => x.Token == course.Token && x.franchid == a && x.Status==true);
            fm.TotalFees = Convert.ToInt32(fee);
            fm.AlertDate = System.DateTime.Now.AddDays(2);
            db.Entry(fm).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CourseFinishAlert","Default");
        }
        public ActionResult FullDetails(int id)
        {
            TempData["Fid"] = id;
            FranchDetails fb = new FranchDetails();
            return View(fb);
        }
    }
}