using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Areas.Auth.Controllers
{
    public class AttendenceController : Controller
    {
        // GET: Auth/Attendence
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            DailyAttendence dm = new DailyAttendence();
            return View(dm);
        }
        //[HttpPost]
        //public ActionResult Create()
        //{
        //    DailyAttendence dm = new DailyAttendence();
        //    return View(dm);
        //}
    }
}