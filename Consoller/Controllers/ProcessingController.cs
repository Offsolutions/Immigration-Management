using Consoller.Areas.Auth.Models;
using Consoller.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Controllers
{
    public class ProcessingController : Controller
    {
        // GET: Processing
        dbcontext db = new dbcontext();
        public ActionResult AllApplication()
        {
            return View(db.Applications.ToList());
        }
        public ActionResult Dashboard(int id)
        {
            return View(db.Applications.FirstOrDefault(x=>x.ApplicationNo==id));
        }
        public ActionResult AlterAgent(Agent ag,int? app)
        {
            var assign = db.Applications.SingleOrDefault(b => b.ApplicationNo==app);
            if (assign != null)
            {
                assign.Agent = ag.Aid;
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard","Processing", new { id=app});
        }
        public ActionResult UpdateOffer(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            Offerletter offer = db.Offerletter.FirstOrDefault(x => x.Lid == id);
            return PartialView("~/Views/Application/ApplyOfferletter.cshtml", offer);
        }

        public ActionResult ApplyOfferletters(Offerletter off,Helper Help)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(off).State = EntityState.Modified;
                    db.SaveChanges();
                    
                    Process pr = db.Process.SingleOrDefault(x => x.ApplicationNo == off.ApplicationNo);
                    if (pr != null)
                    {
                        pr.Offerletter = true;
                        db.SaveChanges();

                    }
                    var inquiry = db.Applications.FirstOrDefault(x => x.ApplicationNo == off.ApplicationNo).InquiryId;
                    transaction.Commit();
                    if (off.Sms == true)
                    {
                        Help.sendsms(Help.contact(off.ApplicationNo), "Hello Your Offerletter has been Applied");
                    }
                    return RedirectToAction("Dashboard", "Processing", new { id = off.ApplicationNo });
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }
        public ActionResult Gic([Bind(Include ="Date,AccountNo,ApplicationNo,BankName,Status,CreatedBy,Amount,Sms,Email,Show")] GIC gic,Helper help)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    GIC gg = db.Gic.FirstOrDefault(x => x.ApplicationNo == gic.ApplicationNo);
                    if (gg == null)
                    {
                        var inquiryid = db.Applications.FirstOrDefault(x => x.ApplicationNo == gic.ApplicationNo).InquiryId;
                        online onlines = db.onlines.FirstOrDefault(x => x.inquiryid == inquiryid);
                        gic.Status = true;
                        gic.CreatedBy = help.Teacher();
                        db.Gic.Add(gic);
                        db.SaveChanges();
                        Process pr = db.Process.FirstOrDefault(x => x.ApplicationNo == gic.ApplicationNo);
                        if (pr.Gic == false)
                        {
                            var Process = db.Process.SingleOrDefault(x => x.ApplicationNo == gic.ApplicationNo);
                            if (Process != null)
                            {
                                Process.Gic = true;

                                db.SaveChanges();
                            }
                        }

                        transaction.Commit();
                        if (gic.Sms == true)
                        {

                            help.sendsms(onlines.Mobile, "Hello," + onlines.Name + " your Gic Account Has Open Sucessfully ");
                        }
                        return Json(gic, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["danger"] = "Sorry Gic Already Add";
                        return Json(gic, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    string a = e.Message;
                    transaction.Rollback();

                    throw;
                  //  return View();
                }
              
            }
        }
        public ActionResult Medical([Bind(Include = "Date,HospitalName,BookedBy,ApplicationNo,Note,Sms,Email,Show")] Medicals Med, Helper help)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Medicals mm = db.Medicals.FirstOrDefault(x => x.ApplicationNo == Med.ApplicationNo);
                    if(mm==null)
                    {
                        var inquiryid = db.Applications.FirstOrDefault(x => x.ApplicationNo == Med.ApplicationNo).InquiryId;
                        online onlines = db.onlines.FirstOrDefault(x => x.inquiryid == inquiryid);
                        Med.BookedBy = help.Teacher();
                        db.Medicals.Add(Med);
                        db.SaveChanges();
                        Process pr = db.Process.FirstOrDefault(x => x.ApplicationNo == Med.ApplicationNo);
                        if (pr.Medical == false)
                        {
                            var Process = db.Process.SingleOrDefault(x => x.ApplicationNo == Med.ApplicationNo);
                            if (Process != null)
                            {
                                Process.Medical = true;

                                db.SaveChanges();
                            }
                        }

                        transaction.Commit();
                        if (Med.Sms == true)
                        {

                            help.sendsms(onlines.Mobile, "Hello," + onlines.Name + " your Medical Registerd Sucessfully ");
                        }
                        return Json(Med, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["danger"] = "Sorry Medical Already Booked";
                        return Json(Med, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    throw;
                }
                return View();
            }
        }
        public ActionResult Med()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SubmitFile([Bind (Include ="SubmitDate,FileDoc,SubmitBy,ApplicationNo,Sms,Email,Show,Uci,Appno")]SubmitFile sub,Helper help)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string _imgname = string.Empty;
                    var inquiryid = db.Applications.FirstOrDefault(x => x.ApplicationNo == sub.ApplicationNo).InquiryId;
                    online onlines = db.onlines.FirstOrDefault(x => x.inquiryid == inquiryid);
                    if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                    {
                        var pic = System.Web.HttpContext.Current.Request.Files["FileDoc"];
                        if (pic.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(pic.FileName);
                            var _ext = Path.GetExtension(pic.FileName);

                            _imgname = Guid.NewGuid().ToString();
                            var _comPath = Server.MapPath("~/Uploadfile/") + _imgname + _ext;
                            _imgname = _imgname + _ext;

                            ViewBag.Msg = _comPath;
                            var path = _comPath;

                            // Saving Image in Original Mode
                            pic.SaveAs(path);


                        }

                    }
                    sub.FileDoc = _imgname;

                    db.SubmitFiles.Add(sub);
                    db.SaveChanges();
                    Process pr = db.Process.FirstOrDefault(x => x.ApplicationNo == sub.ApplicationNo);
                    if (pr.SubmitFile == false)
                    {
                        var Process = db.Process.SingleOrDefault(x => x.ApplicationNo == sub.ApplicationNo);
                        if (Process != null)
                        {
                            Process.SubmitFile = true;

                            db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                    if (sub.Sms == true)
                    {

                        help.sendsms(onlines.Mobile, "Hello," + onlines.Name + " your Your File is Ready For Submit ");
                    }
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
           
        }
        public ActionResult CourseUpdate()
        {
            ViewBag.Cid = new SelectList(db.Countries, "Cid", "CountryName");
            return View();
        }
        public ActionResult Offerletter()
        {
            return View(db.Process.Where(x => x.Offerletter == true));
        }
    }

    
    
}