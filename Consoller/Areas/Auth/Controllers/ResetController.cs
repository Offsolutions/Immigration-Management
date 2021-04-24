using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Areas.Auth.Controllers
{
    public class ResetController : Controller
    {
        dbcontext db = new dbcontext();
        public string id = "";
        SQLHelper objsql = new SQLHelper();
        tblreceptionist tt = new tblreceptionist();
        // GET: Auth/Reset
        public ActionResult Index()
        {
            return View();
        }

        // GET: Auth/Reset/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Auth/Reset/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Reset/Create
        [HttpPost]
        public ActionResult Create(string password,Helper help)
        {
            string a = HttpContext.User.Identity.Name;
            try
            {
                
                objsql.ExecuteNonQuery("update tblreceptionists set password='" + password + "' where rid='" + a + "'");
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Auth/Reset/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Auth/Reset/Edit/5
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

        // GET: Auth/Reset/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Auth/Reset/Delete/5
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
