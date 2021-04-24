
using Consoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using System.Dynamic;
using System.Data;

namespace Consoller.Areas.Auth.Controllers
{
    public class DailyReportsController : Controller
    {
        dbcontext db = new dbcontext  ();
        DashModel dm = new DashModel();
        DataTable dd = new DataTable();
        // GET: Auth/DailyReports
        [HttpGet]
        public ActionResult ExpenseReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ExpenseReport(Expense ex)
        {
            return View(db.Expenses.Where(x=>x.Date==ex.Date).ToList());
        }

        // GET: Auth/DailyReports/Details/5
        public ActionResult DayEnd( ExpenseLock Lock,Helper help)
        {
            Lock.LockDate = System.DateTime.Now;
            Lock.By = help.Permission();
            db.ExpenseLocks.Add(Lock);
            db.SaveChanges();
            TempData["Success"] = "Day End Successfully";
            return RedirectToAction("ExpenseReport","DailyReports");
        }
        public ActionResult TodayReport()
        {
            return View(dm);
        }
        [HttpGet]
        public ActionResult DateWise()
        {
            Session["Status"] = "False";
            return View();

        }
        [HttpPost]
        public ActionResult DateWise( Expense ex)
        {
            Session["Status"] = "True";
            dynamic mymodel = new ExpandoObject();
            mymodel.expense = db.Expenses.Where(x => x.Date >= ex.Date1 && x.Date <= ex.Date2);
            mymodel.dailyinquiry =db.tblinquiries.Where(x => x.date >= ex.Date1 && x.date <= ex.Date2);
            mymodel.immgrationinquiry = db.onlines.Where(x => x.Date >= ex.Date1 && x.Date <= ex.Date2);
            mymodel.ieltsfee = db.Recipt_Details.Where(x => x.Date >= ex.Date1 && x.Date <= ex.Date2);
            mymodel.ConfirmCase = db.Applications.Where(x => x.Date >= ex.Date1 && x.Date <= ex.Date2);
            mymodel.CasePayment = db.ApplicationRecipts.Where(x => x.Date >= ex.Date1 && x.Date <= ex.Date2);
            TempData["Date1"] = ex.Date1;
            TempData["Date2"] = ex.Date2;
            
          
            return View(mymodel);

        }
        [HttpGet]
        public ActionResult TeacherReport()
        {

            ViewBag.UserId = new SelectList(db.tblreceptionists.Where(x => x.Type == "Teacher"), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult TeacherReport([Bind(Include ="UserId,From,To")] tblstudentdata student)
        {
            SQLHelper objsql = new SQLHelper();
            dd = objsql.GetTable("select r.rollno,r.date,s.name,r.CourseId,r.amount from recipt_details r,tblstudentdatas s where r.date between '" + student.From+"' and '"+student.To+"' and r.rollno=s.rollno and s.status='1' and s.userid='"+student.UserId+"'");
            return View(dd);
        }
        public ActionResult AttendenceReport()
        {

            ViewBag.UserId = new SelectList(db.tblreceptionists.Where(x => x.Type == "Teacher"), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AttendenceReport([Bind(Include = "UserId,From")] tblstudentdata student)
        {
            // MorningAttendence ma=db.MorningAttendeces.Where()
            string date = student.From.ToString("MM/dd/yyyy");
            string id = student.UserId.ToString();
            return View(db.MorningAttendeces.Where(x=>x.Date==date && x.Role==id));
        }
    }
}
