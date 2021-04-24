using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Areas.Auth.Controllers
{
    public class ChangePassController : Controller
    {
        dbcontext db = new dbcontext();
        public string id = "";
        SQLHelper objsql = new SQLHelper();
        tblreceptionist tt = new tblreceptionist();
        // GET: Auth/ChangePass
        public ActionResult Index()
        {
            return View();
        }

        // GET: Auth/ChangePass/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Auth/ChangePass/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/ChangePass/Create
        [HttpPost]
        public ActionResult Create(string password)
        {
            string a = HttpContext.User.Identity.Name;
            try
            {

                objsql.ExecuteNonQuery("update tblreceptionists set password='" + password + "' where rid='" + a + "'");
                TempData["Success"] = "Password Change Successfully";
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Auth/ChangePass/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Auth/ChangePass/Edit/5
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

        // GET: Auth/ChangePass/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Auth/ChangePass/Delete/5
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
