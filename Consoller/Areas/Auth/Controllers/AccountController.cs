using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;
using System.Web.Security;

namespace Consoller.Areas.Auth.Controllers
{
    public class AccountController : Controller
    {
        private dbcontext db = new dbcontext();
        public static string img;
        // GET: Auth/Account
        //[Authorize(Roles ="Admin")]
        public ActionResult Index(Helper help)
        {
            if(User.IsInRole("Franchisee"))
            {
                string r = help.Franchisee();
                string a = help.Franchisee();
                return View(db.tblreceptionists.Where(x=>x.rid==r || x.franchid==a));
            }
            else if (User.IsInRole("Receptionist"))
            {
                string r = help.Franchisee();
                return View(db.tblreceptionists.Where(x => x.rid == r));
            }
            return View(db.tblreceptionists.ToList());
        }

        // GET: Auth/Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblreceptionist tblreceptionist = db.tblreceptionists.Find(id);
            if (tblreceptionist == null)
            {
                return HttpNotFound();
            }
            return View(tblreceptionist);
        }

        // GET: Auth/Account/Create
        [Authorize(Roles ="Franchisee,Admin")]
        public ActionResult Create()
        {
            tblreceptionist receptionist = new tblreceptionist();
            receptionist.date = System.DateTime.Now;
            return View(receptionist);
        }

        // POST: Auth/Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,date,name,email,contact,login,password,rid,image,Type,status,StartTime,EndTime")] tblreceptionist tblreceptionist, HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                tblreceptionist recp = db.tblreceptionists.FirstOrDefault();
                if (recp == null)
                {
                    tblreceptionist.rid = "1001";
                }
                else
                {
                    var valc = db.tblreceptionists.Max(x => x.rid);
                    
                    tblreceptionist.rid = (Convert.ToInt32(valc) + 1).ToString();
                }
                if (User.IsInRole("Franchisee"))
                {
                    tblreceptionist.franchid =Help.Franchisee();
                }
                else
                {
                    tblreceptionist.franchid = Help.Franchisee();
                }
                tblreceptionist.status =true;
                tblreceptionist.image = Help.uploadfile(file);
                tblreceptionist.password = tblreceptionist.password;
                db.tblreceptionists.Add(tblreceptionist);
                db.SaveChanges();
                TempData["Success"] = "Saved Successfully";
                return RedirectToAction("Index");
            }

            return View(tblreceptionist);
        }

        // GET: Auth/Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblreceptionist tblreceptionist = db.tblreceptionists.Find(id);
            img = tblreceptionist.image;

            if (tblreceptionist == null)
            {
                return HttpNotFound();
            }
            return View(tblreceptionist);
        }

        // POST: Auth/Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit([Bind(Include = "Id,date,name,email,contact,login,password,rid,image,Type,status,StartTime,EndTime")] tblreceptionist tblreceptionist, HttpPostedFileBase file, Helper Help)
        {
            if (ModelState.IsValid)
            {
                tblreceptionist.image = file != null ? Help.uploadfile(file) : img;
                #region delete file
                string fullPath = Request.MapPath("~/UploadedFiles/" + img);
                if (img == tblreceptionist.image)
                {
                }
                else
                {
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                #endregion
                db.Entry(tblreceptionist).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(tblreceptionist);
        }

        // GET: Auth/Account/Delete/5
        [Authorize(Roles = "Franchisee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblreceptionist tblreceptionist = db.tblreceptionists.Find(id);
            img = tblreceptionist.image;
            if (tblreceptionist == null)
            {
                return HttpNotFound();
            }
            return View(tblreceptionist);
        }

        // POST: Auth/Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblreceptionist tblreceptionist = db.tblreceptionists.Find(id);
            img = tblreceptionist.image;
            #region delete file
            string fullPath = Request.MapPath("~/UploadedFiles/" + img);
            if (img == tblreceptionist.image)
            {
            }
            else
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            #endregion
            db.tblreceptionists.Remove(tblreceptionist);
            db.SaveChanges();
            TempData["Success"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tblreceptionist model, string returnUrl, Helper Help,string ddltype)
        {
            dbcontext db = new dbcontext();
            var passw =model.password;
            var dataItem = db.tblreceptionists.Where(x => x.login == model.login && x.password == passw && x.Type==ddltype).FirstOrDefault();
            if (dataItem != null)
            {
                
                FormsAuthentication.SetAuthCookie(dataItem.rid, false);
             
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                         && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else if (ddltype == "Admin")
                {
                    TempData["Success"] = "Login Successfully";
                    Session["User"] = dataItem.Id;
                    Session["admin"] = dataItem.Id;
                    return RedirectToAction("Index", "tblreceptionists");
                    Session["Franchisee"] = dataItem.franchid;
                   
                }
                else if (ddltype=="Consoller")
                {
                    TempData["Success"] = "Login Successfully";
                    return RedirectToAction("Index", "Consoller");
                }
                else
                {
                    TempData["Success"] = "Login Successfully";
                    Session["User"] = dataItem.Id;

                    Session["Franchisee"] = dataItem.franchid;
                    return RedirectToAction("Index", "Default");

                }
            }
            else
            {
               // ModelState.AddModelError("", "Invalid user/pass");
                TempData["Success"] = "Invalid user/pass";
                return View();
            }
        }

        //[Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Credential", new { area = "" });
        }
    }
}
