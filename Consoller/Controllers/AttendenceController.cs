using Consoller.Areas.Auth.Models;
using Consoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Controllers
{
    public class AttendenceController : Controller
    {
        dbcontext db = new dbcontext();
        Helper help = new Helper();
        // GET: Attendence
        public ActionResult Create()
        {
            string a = User.IsInRole("Franchisee") ? help.Franchisee() : help.Receptionist();
            ViewBag.TimeId = new SelectList(db.Timings.Where(x => x.franchid == a), "Tid", "BatchTime");
            string date = System.DateTime.Now.ToString("MM/dd/yyyy");
            return View(db.MorningAttendeces.Where(x=>x.Date==date && x.Role==a));
        }
    }
}