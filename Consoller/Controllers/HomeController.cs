using Consoller.Areas.Auth.Models;
using Consoller.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Controllers
{
    public class HomeController : Controller
    {
        Data dd = new Data();
        public ActionResult Index()
        {
            

            return View();
        }
        public ActionResult Dashboard()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            
            dataPoints.Add(new DataPoint("Application", dd.inquiry));
            dataPoints.Add(new DataPoint("Confirm", dd.Confirm));
            dataPoints.Add(new DataPoint("Cancel", 4));
          

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View(dd);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult GetRecord(string prefix)
        {
           // database_Access_Layer.db dblayer = new database_Access_Layer.db();
            DataSet ds = GetName(prefix);

            List<online> searchlist = new List<online>();

            foreach (DataRow dr in ds.Tables[0].Rows)

            {

                searchlist.Add(new online
                {

                    Name = dr["Name"].ToString(),

                    inquiryid =Convert.ToInt32(dr["InquiryId"])

                });

            }

            return Json(searchlist, JsonRequestBehavior.AllowGet);

        }
        public DataSet GetName(string prefix)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            SqlCommand com = new SqlCommand("Select * from onlines where Name like '%'+@prefix+'%'", con);

            com.Parameters.AddWithValue("@prefix", prefix);

            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(com);

            da.Fill(ds);

            return ds;

        }
        public JsonResult GetNotificationContacts()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetContacts(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


    }
}