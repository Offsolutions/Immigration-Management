using Consoller.Areas.Auth.Models;
using Consoller.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Consoller.Areas.Admin.Models
{
    public class AdminData
    {
        dbcontext db = new dbcontext();
        SQLHelper objsql = new SQLHelper();
        Helper help = new Helper();
        DateTime today = System.DateTime.Now.Date;
        DateTime yesterday = System.DateTime.Now.AddDays(-1).Date;
        public int totalstudent
        {
            get
            {
                return db.tblstudentdata.Where(x=>x.Status==true).Count();
            }
        }
        public int Totalcase
        {
            get
            {
                return db.onlines.Count();
            }
        }
        public int Confirmcase
        {
            get
            {
                return db.Applications.Count();
            }
        }
        public List<online> online
        {
            get
            {
                DateTime date =Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
                List<online> online = db.onlines.Where(x => x.Date == date).ToList();
                return online;
            }
        }
        public string Sale()
        {

            string total = objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM applicationrecipts WHERE MONTH(date) = '" + System.DateTime.Now.Month + "' AND YEAR(date) = '" + System.DateTime.Now.Year + "'").ToString();
            return total;

        }
        public string Todayearning()
        {

            string total = objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM applicationrecipts WHERE date = '" + System.DateTime.Now + "'").ToString();
            return total;

        }
        public int inquiry(int id)
        {

            var inquiry= db.Applications.FirstOrDefault(x => x.ApplicationNo == id).InquiryId;
            return inquiry;

        }
        #region Expense
        public float TodayExpenses()
        {
            string franchid = help.Permission();
            Expense ee = new Expense();
            //  DateTime today = System.DateTime.Now;
            if (franchid == "Admin")
            {
                return (db.Expenses.Where(x => x.Date == today).Select(x => x.Amount).DefaultIfEmpty(0)).Sum();
            }
            ee = db.Expenses.FirstOrDefault(x => x.franchid == franchid && x.Date == today);
            if (ee != null)
            {
                return (db.Expenses.Where(x => x.franchid == franchid && x.Date == today).Select(x => x.Amount).DefaultIfEmpty(0)).Sum();
            }
            else
            {
                return 0;
            }


        }
        public float yesterdayExpenses()
        {
            string franchid = help.Permission();
            Expense ee = new Expense();
            if (franchid == "Admin")
            {
                ee = db.Expenses.FirstOrDefault(x => x.Date == yesterday);
                if (ee != null)
                {
                    return (db.Expenses.Where(x => x.Date == yesterday)).Sum(x => x.Amount);
                }
                else
                {
                    return 0;
                }
            }

            ee = db.Expenses.FirstOrDefault(x => x.franchid == franchid && x.Date == yesterday);
            if (ee != null)
            {
                return (db.Expenses.Where(x => x.franchid == franchid && x.Date == yesterday)).Sum(x => x.Amount);
            }
            else
            {
                return 0;
            }


        }
        public double LastMonthExpenses()
        {
            string franchid = help.Permission();
            double total = 0.0;
            if (franchid == "Admin")
            {
                total = Convert.ToDouble(objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM expenses WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, -1, getdate())) "));
                return total;
            }
            total = Convert.ToDouble(objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM expenses WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, -1, getdate())) and franchid='" + franchid + "'"));
            if (total != 0.0)
            {
                return total;
            }
            else
            {
                return 0.0;
            }
        }
        public double ThisMonthExpenses()
        {
            string franchid = help.Permission();
            double total = 0.0;
            if (franchid == "Admin")
            {
                total = Convert.ToDouble(objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM expenses WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, 0, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, 0, getdate())) "));
                if (total != 0.0)
                {
                    return total;
                }
                else
                {
                    return 0.0;
                }
            }
            DataTable dd = new DataTable();
            dd = objsql.GetTable("SELECT COALESCE(sum(amount),0) FROM expenses WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, 0, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, 0, getdate())) and franchid='" + franchid + "'");
            if (dd.Rows.Count > 0)
            {
                total = Convert.ToDouble(objsql.GetSingleValue("SELECT COALESCE(sum(amount),0) FROM expenses WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, 0, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, 0, getdate())) and franchid='" + franchid + "'"));
                if (total != 0.0)
                {
                    return total;
                }
                else
                {
                    return 0.0;
                }
            }
            else
            {
                return 0.0;
            }

        }
        #endregion
        #region IeltsIncome
        public float TodayCase()
        {
            string franchid = help.Permission();
            //  DateTime today = System.DateTime.Now;
            if (franchid == "Admin")
            {
                return (db.ApplicationRecipts.Where(x => x.Date == today)).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            }

            ApplicationRecipt ee = db.ApplicationRecipts.FirstOrDefault(x => x.Date == today);
            if (ee != null)
            {
                return (db.ApplicationRecipts.Where(x => x.Date == today)).Sum(x => x.Amount);
            }
            else
            {
                return 0;
            }


        }
        public float TodayIelts()
        {
            string franchid = help.Permission();
            if (franchid == "Admin")
            {
                return (db.Recipt_Details.Where(x => x.Date == today)).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            }
            //  DateTime today = System.DateTime.Now;
            Recipt_Details ee = db.Recipt_Details.FirstOrDefault(x => x.franchid == franchid && x.Date == today);
            if (ee != null)
            {
                return (db.Recipt_Details.Where(x => x.franchid == franchid && x.Date == today)).Sum(x => x.Amount);
            }
            else
            {
                return 0;
            }


        }
        public float yesterdayIelts()
        {
            string franchid = help.Permission();
            // DateTime today = System.DateTime.Now;
            if (franchid == "Admin")
            {
                Recipt_Details eee = db.Recipt_Details.FirstOrDefault(x => x.Date == yesterday);
                if (eee != null)
                {
                    return (db.Recipt_Details.Where(x => x.Date == yesterday)).Sum(x => x.Amount);
                }
                else
                {
                    return 0;
                }
            }

            Recipt_Details ee = db.Recipt_Details.FirstOrDefault(x => x.franchid == franchid && x.Date == yesterday);
            if (ee != null)
            {
                return (db.Recipt_Details.Where(x => x.franchid == franchid && x.Date == yesterday)).Sum(x => x.Amount);
            }
            else
            {
                return 0;
            }


        }
        public string LastMonthIelts()
        {
            string franchid = help.Permission();

            string total = Convert.ToString(objsql.GetSingleValue("SELECT sum(amount) FROM Recipt_Details WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, -1, getdate()))"));
            if (Convert.ToInt32(total) != 0.0)
            {
                return total;
            }
            else
            {
                return "0";
            }
        }
        public string ThisMonthIelts()
        {
            string franchid = help.Permission();

            string total = Convert.ToString(objsql.GetSingleValue("SELECT sum(amount) FROM Recipt_Details WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, 0, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, 0, getdate()))"));
            if (Convert.ToInt32(total) != 0.0)
            {
                return total;
            }
            else
            {
                return "0";
            }
        }
        public IEnumerable<tblinquiry> Allinquiry
        {
            get
            {
                return db.tblinquiries.Where(x => x.date == today).ToList();
            }

        }

        public IEnumerable<Expense> AllExpense
        {
            get
            {
                return db.Expenses.Where(x => x.Date == today).ToList();
            }

        }
        public IEnumerable<online> onlines
        {
            get
            {
                return db.onlines.Where(x => x.Date == today).ToList();
            }

        }
        public IEnumerable<Recipt_Details> invoice
        {
            get
            {
                return db.Recipt_Details.Where(x => x.Date == today).ToList();
            }

        }
        #endregion
        #region immigration income
        public float TodayImmigration()
        {
            string franchid = help.Permission();
            if (franchid == "Admin")
            {
                return (db.ApplicationRecipts.Where(x => x.Date == today)).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            }
            //  DateTime today = System.DateTime.Now;
            ApplicationRecipt ee = db.ApplicationRecipts.FirstOrDefault(x => x.Date == today);
            if (ee != null)
            {
                return (db.ApplicationRecipts.Where(x => x.Date == today)).Sum(x => x.Amount);
            }
            else
            {
                return 0;
            }


        }
        public float yesterdayImmigration()
        {
            string franchid = help.Permission();
            // DateTime today = System.DateTime.Now;
            if (franchid == "Admin")
            {
                ApplicationRecipt ee2 = db.ApplicationRecipts.FirstOrDefault(x => x.Date == yesterday);
                if (ee2 != null)
                {
                    return (db.ApplicationRecipts.Where(x => x.Date == yesterday)).Sum(x => x.Amount);
                }
                else
                {
                    return 0;
                }
            }

            ApplicationRecipt ee = db.ApplicationRecipts.FirstOrDefault(x => x.Date == yesterday);
            if (ee != null)
            {
                return (db.ApplicationRecipts.Where(x => x.Date == yesterday)).Sum(x => x.Amount);
            }
            else
            {
                return 0;
            }


        }
        public string LastMonthImmgration()
        {
            string franchid = help.Permission();

            string total = Convert.ToString(objsql.GetSingleValue("SELECT sum(amount) FROM ApplicationRecipts WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, -1, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, -1, getdate()))"));
            if (Convert.ToInt32(total) != 0.0)
            {
                return total;
            }
            else
            {
                return "0";
            }
        }
        public string ThisMonthImmgration()
        {
            string franchid = help.Permission();

            string total = Convert.ToString(objsql.GetSingleValue("SELECT sum(amount) FROM ApplicationRecipts WHERE DATEPART(m, date) = DATEPART(m, DATEADD(m, 0, getdate())) AND DATEPART(yyyy, date) = DATEPART(yyyy, DATEADD(m, 0, getdate()))"));
            if (Convert.ToInt32(total) != 0.0)
            {
                return total;
            }
            else
            {
                return "0";
            }
        }
        #endregion
    }
}