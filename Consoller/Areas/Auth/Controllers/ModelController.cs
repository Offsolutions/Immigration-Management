using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Consoller.Areas.Auth.Controllers
{
    public class ModelController : Controller
    {
        dbcontext db = new dbcontext();
        
        // GET: Auth/Model
        public ActionResult Index()
        {
            return View();
        }
    
        public ActionResult EmployeePartial(int stateID)
        {
            db.Configuration.ProxyCreationEnabled = false;
           List<tblinquiry> lstcity = new List<tblinquiry>();
            
            lstcity = db.tblinquiries.ToList();
            ViewBag.EmployeeList = lstcity;
            // JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            // string result = javaScriptSerializer.Serialize(lstcity);
            // // return PartialView("_EmpPartial", "Hello Gagan");
            // TempData["data"] = "hello";
            //// return Json(result, JsonRequestBehavior.AllowGet);
            // return Json(new { result = Url.Action("_EmpPartial", result) });
            return PartialView("~/Areas/Auth/Views/Model/_EmpPartial.cshtml", lstcity);
        }

        // GET: Auth/Model/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Auth/Model/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Model/Create
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

        // GET: Auth/Model/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Auth/Model/Edit/5
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

        // GET: Auth/Model/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Auth/Model/Delete/5
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
