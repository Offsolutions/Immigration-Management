using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Areas.Auth.Controllers
{
    public class AttendController : Controller
    {
        dbcontext db = new dbcontext();
        // GET: Auth/Attend
        public ActionResult Index()
        {
            return View(db.tblstudentdata.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(DateTime date)
        {
            string a = date.ToString("dd/MM/yyyy");
            return View(db.MorningAttendeces.Where(x => x.Date == a).ToList());
        }
    }
}