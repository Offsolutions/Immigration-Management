using Consoller.Areas.Auth.Models;
using Consoller.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Consoller.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application
        Data dd = new Data();
        dbcontext db = new dbcontext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int id)
        {
            Application app = db.Applications.FirstOrDefault(x => x.InquiryId == id);
            if(app!=null)
            {
                Process pro = db.Process.FirstOrDefault(x => x.ApplicationNo == app.ApplicationNo);
                if(pro==null)
                {
                    Process pp = new Process();
                    pp.ApplicationNo = app.ApplicationNo;
                    pp.Registration = true;
                    db.Process.Add(pp);
                    db.SaveChanges();
                }
            }
            return View(db.onlines.FirstOrDefault(x=>x.inquiryid==id));
        }
        public ActionResult AddOffer(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            online online = db.onlines.FirstOrDefault(x => x.inquiryid == id);
            return PartialView("~/Views/Application/AddOffer.cshtml", online);
        }
        public ActionResult GetCityList(int stateID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<College> lstcity = new List<College>();

            lstcity = (db.Colleges.Where(x => x.Cid == stateID && x.Status==true)).ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstcity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Amount(int stateID,int Appid)
        {
            string result = "";
            db.Configuration.ProxyCreationEnabled = false;
            List<PaymentStep> lstcity = new List<PaymentStep>();

            lstcity = (db.PaymentSteps.Where(x => x.Pid == stateID)).ToList();
            string type = lstcity[0].Pid.ToString();
            var a = db.ApplicationRecipts.FirstOrDefault(x => x.ApplicationNo == Appid && x.Type == type);
            if (a != null)
            {
                var Recived = db.ApplicationRecipts.Where(x => x.ApplicationNo == Appid && x.Type == type).Sum(x => x.Amount);
                lstcity[0].Pending = (Convert.ToInt32(lstcity[0].Amount) - Convert.ToInt32(Recived));
                JavaScriptSerializer javaScriptSerializer1 = new JavaScriptSerializer();
                result = javaScriptSerializer1.Serialize(lstcity);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            result = javaScriptSerializer.Serialize(lstcity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Course(int stateID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<CollegeCourse> lstcity = new List<CollegeCourse>();

            lstcity = (db.CollegeCourses.Where(x => x.Oid == stateID && x.Status==true)).ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstcity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Intake(int stateID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Intake> lstcity = new List<Intake>();

            lstcity = (db.Intakes.Where(x => x.CCid == stateID && x.Status == true)).ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstcity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult InsertOffer(Offerletter off,int country,int college,int course)
        //{

        //    off.Cid = country;
        //    off.Oid = college;
        //    off.CCid = course;
        //    db.Offerletter.Add(off);
        //    db.SaveChanges();
        //    TempData["Success"] = "Offerletter Detail Successfully";
        //    return View();
        //}

        public JsonResult Payment(Helper Help, ApplicationRecipt Recipt,Leader ll,Helper help)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Recipt.Date = System.DateTime.Now;
                    var inquiryid = db.Applications.FirstOrDefault(x => x.ApplicationNo == Recipt.ApplicationNo).InquiryId;
                    online onlines = db.onlines.FirstOrDefault(x => x.inquiryid == inquiryid);
                    string franchise = Help.Consoller();
                   // Recipt.ApplicationNo = Emp.ApplicationNo;
                    Recipt.Leadger = db.Ledgers.FirstOrDefault(x => x.ApplicationNo == Recipt.ApplicationNo && x.Franchid==franchise).LeaderNo;
                  
                    Recipt.ReciptNo = Help.recipt();
                    
                    Recipt.RecivedBy = Help.Teacher();
                    db.ApplicationRecipts.Add(Recipt);
                    db.SaveChanges();
                    ll = db.Ledgers.FirstOrDefault(x => x.ApplicationNo == Recipt.ApplicationNo);
                    if(ll!=null)
                    {
                        int recived = ll.Received;
                        ll.Received = recived + Recipt.Amount;
                        db.Entry(ll).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    Process pr = db.Process.FirstOrDefault(x => x.ApplicationNo == Recipt.ApplicationNo);
                    if (pr.Offerletter == false)
                    {
                        var Process = db.Process.SingleOrDefault(x => x.ApplicationNo == Recipt.ApplicationNo);
                        if (Process != null)
                        {
                            Process.Offerletter = true;

                            db.SaveChanges();
                        }
                    }
                    if (Recipt.sms == true)
                    {

                        help.sendsms(onlines.Mobile, "Hello," + onlines.Name + " Thank you For Deposit  ");
                    }
                    transaction.Commit();
                    return Json(Recipt, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    throw;
                } 
            }
        }
        public JsonResult InsertOffer(Offerletter Emp, ApplicationRecipt Recipt,Helper Help) // Record Insert  
        {

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Emp.Show = true;

                    db.Offerletter.Add(Emp);
                    db.SaveChanges();
                    Recipt.Date = System.DateTime.Now;
                    Recipt.ApplicationNo = Emp.ApplicationNo;
                    Recipt.Leadger = db.Ledgers.FirstOrDefault(x => x.ApplicationNo == Emp.ApplicationNo).LeaderNo;
                    Recipt.Type = "Offer Letter";
                    Recipt.ReciptNo = Help.recipt();
                    Recipt.Amount = Emp.Amount;
                    Recipt.RecivedBy = Help.Teacher();
                    db.ApplicationRecipts.Add(Recipt);
                    db.SaveChanges();
                    Process pr = db.Process.FirstOrDefault(x => x.ApplicationNo == Emp.ApplicationNo);
                    if (pr.Medical == false)
                    {
                        var Process = db.Process.SingleOrDefault(x => x.ApplicationNo == Emp.ApplicationNo);
                        if (Process != null)
                        {
                            Process.Offerletter = true;

                            db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                    TempData["Success"] = "Data Save Sucessfully";
                    return Json(Emp, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    string a = e.Message;
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public ActionResult GetOfferLetter(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Offerletter> off = db.Offerletter.Where(x => x.ApplicationNo == id).ToList();
            return PartialView("~/Views/Application/ListOfferLetter.cshtml", off);
        }
        public ActionResult UpdateOffer(int id)
        {
           
            return View(db.Offerletter.FirstOrDefault(x=>x.Lid==id));
        }
        public ActionResult UpdateLetter(Offerletter Emp, ApplicationRecipt Recipt, Helper Help) // Record Insert 
        {

            db.Entry(Emp).State = EntityState.Modified;
            db.SaveChanges();
            var inquiry = db.Applications.FirstOrDefault(x => x.ApplicationNo == Emp.ApplicationNo).InquiryId;
            return RedirectToAction("Create", "Application", new { id = inquiry });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadDocs(Document docs)
        {
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
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
                    docs.Doc = _imgname;
                    db.Documents.Add(docs);
                    db.SaveChanges();
                   
                }
            }
                return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDocument(int id)
        {


            db.Configuration.ProxyCreationEnabled = false;
            List<Document> doc = db.Documents.Where(x => x.ApplicationNo == id).ToList();
            return PartialView("~/Views/Application/DocumentList.cshtml", doc);
        }
        public ActionResult GetPayment(int id)
        {


            db.Configuration.ProxyCreationEnabled = false;
            List<ApplicationRecipt> doc = db.ApplicationRecipts.Where(x => x.ApplicationNo == id).OrderByDescending(x=>x.Rid).ToList();
            return PartialView("~/Views/Application/FeeDetails.cshtml", doc);
        }
        public ActionResult GetMedical(int id)
        {


            db.Configuration.ProxyCreationEnabled = false;
            List<Medicals> doc = db.Medicals.Where(x => x.ApplicationNo == id).ToList();
            return PartialView("~/Views/Medicals/Partial_Medical.cshtml", doc);
        }
        public ActionResult GetGic(int id)
        {


            db.Configuration.ProxyCreationEnabled = false;
            List<GIC> doc = db.Gic.Where(x => x.ApplicationNo == id).ToList();
            return PartialView("~/Views/Processing/Partial_Gic.cshtml", doc);
        }
        public ActionResult GetSub(int id)
        {


            db.Configuration.ProxyCreationEnabled = false;
            List<SubmitFile> doc = db.SubmitFiles.Where(x => x.ApplicationNo == id).ToList();
            return PartialView("~/Views/SubmitFiles/Partial_Submit.cshtml", doc);
        }
        public ActionResult Search(Application app)
        {
            Application application = db.Applications.FirstOrDefault(x => x.TrackingId == app.TrackingId);
            if(application!=null)
            {
                return RedirectToAction("Create", "Application", new { id = application.InquiryId });
            }
            else
            {
                TempData["danger"] = "Application Not Found !";
                return RedirectToAction("Dashboard", "Home");
            }
        }
     //   [Authorize(Roles ="Processing")]
        public ActionResult Gic([Bind(Include = "Date,AccountNo,ApplicationNo,BankName,Status,CreatedBy,Amount,Sms,Email,Show")] GIC gic, Helper help, int application)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                gic.ApplicationNo = application;
                var inquiryid = db.Applications.FirstOrDefault(x => x.ApplicationNo == gic.ApplicationNo).InquiryId;
                try
                {
                    
                    GIC gg = db.Gic.FirstOrDefault(x => x.ApplicationNo == gic.ApplicationNo);
                    if (gg == null)
                    {
                       
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
                        // return Json(gic, JsonRequestBehavior.AllowGet);
                        TempData["Success"] = "Your Gic Account Add Successfully";
                        return RedirectToAction("Create","Application", new { id= inquiryid });
                    }
                    else
                    {
                        TempData["danger"] = "Sorry Gic Already Add";
                        //  return Json(gic, JsonRequestBehavior.AllowGet);
                        return RedirectToAction("Create", "Application", new { id = inquiryid });
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
                    if (mm == null)
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
        public JsonResult SubmitFile([Bind(Include = "SubmitDate,FileDoc,SubmitBy,ApplicationNo,Sms,Email,Show,Uci,Appno,Gckey,Password,Question1,Answer1,Question2,Answer2,Question3,Answer3,Question4,Answer4")]SubmitFile sub, Helper help)
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
        public ActionResult Invoice(int id)
        {
            return View(db.ApplicationRecipts.FirstOrDefault(x=>x.Rid==id));
        }
        public ActionResult Profile(int id)
        {
            return View(db.Applications.FirstOrDefault(x => x.ApplicationNo == id));
        }
        public ActionResult DeleteDocument(int id)
        {

            Document offerletter = db.Documents.Find(id);
            var on = db.Applications.FirstOrDefault(x => x.ApplicationNo == offerletter.ApplicationNo).InquiryId;
            db.Documents.Remove(offerletter);
            db.SaveChangesAsync();
            return RedirectToAction("Create","Application", new { id=on});
        }

    }
}