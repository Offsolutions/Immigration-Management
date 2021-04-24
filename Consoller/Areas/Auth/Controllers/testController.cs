using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Areas.Auth.Controllers
{
    public class testController : Controller
    {
        SQLHelper objsql = new SQLHelper();
        dbcontext db = new dbcontext();
        public string to = "";
        // GET: Auth/test
        public ActionResult Index()
        {
            DataTable dt = objsql.GetTable("select * from tblstudentdatas where uid='1010' and status='false'");
           // IEnumerable<StudentCourse> stu = db.StudentCourses.Where(x => x.Uid == "1010" && x.Status == true).OrderBy(x=>x.RollNo);
            if (dt.Rows.Count>0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //string token = objsql.GetSingleValue("select max (token) from studentcourses where uid='1010'").ToString();
                    //if (token != "")
                    //{
                    //    to = (Convert.ToInt32(token) + Convert.ToInt32(1)).ToString();
                    //}
                    objsql.ExecuteNonQuery("update fees_master set status='0' where rollno='" + dr["rollno"] + "' and franchid='1010'");
                    objsql.ExecuteNonQuery("update studentcourses set status='0' where rollno='" + dr["rollno"] + "' and uid='1010'");
                }
            }
            //IEnumerable<StudentCourse> stu1 = db.StudentCourses.Where(x => x.Uid == "1009");
            //if (stu1 != null)
            //{
            //    foreach (var a1 in stu1)
            //    {
            //        objsql.ExecuteNonQuery("update fees_master set courseid='" + a1.CourseId + "' where rollno='" + a1.RollNo + "' and franchid='1009'");

            //    }
            //}
            return View();
        }
        public ActionResult Sms(Helper help)
        {
            IEnumerable<tblstudentdata> ss = db.tblstudentdata.Where(x => x.Status==true);
            if (ss != null)
            {
                foreach (var item in ss)
                {
                    String str = Regex.Replace(item.phone, @"-", "");
                    String msz= "Welcome to Bluebird ! Your account with the username:"+str+" and password:12345 has been created sucessfully at bluebirdimmigrations.com.see you at the site";
                    help.sendsms(str, msz);
                }
            }
            return View();
        }
    }
}