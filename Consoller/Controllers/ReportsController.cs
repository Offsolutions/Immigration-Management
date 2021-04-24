using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        dbcontext db = new dbcontext();
        DataTable dt = new DataTable();
        SQLHelper objsql = new SQLHelper();
        public ActionResult MonthlyReport()
        {
            dt = objsql.GetTable("SELECT * FROM applicationrecipts WHERE MONTH(date) = '" + System.DateTime.Now.Month + "' AND YEAR(date) = '" + System.DateTime.Now.Year + "'");
            return View(dt);
        }
    }
}