using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Areas.Auth.Controllers
{
    public class TotalStudentController : Controller
    {
        // GET: Auth/TotalStudent
        dbcontext db = new dbcontext();
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.tblstudentdata.Where(x => x.Status == true).ToList());
        }
        [HttpPost]
        public ActionResult Index(DateTime fromdate, DateTime todate)
        {
            return View(db.tblstudentdata.Where(x=>x.Status==true && x.date>=fromdate && x.date<=todate).ToList());
        }

        // GET: Auth/TotalStudent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Auth/TotalStudent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/TotalStudent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Auth/TotalStudent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Auth/TotalStudent/Edit/5
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

        // GET: Auth/TotalStudent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Auth/TotalStudent/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
