using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consoller.Models
{
    public class Data
    {
        dbcontext db = new dbcontext();
        string id = HttpContext.Current.User.Identity.Name;
        SQLHelper objsql = new SQLHelper();
        public List<Country> counrtry
        {
            get
            {
                List<Country> country = db.Countries.ToList();
                return country;
            }
        }
        public tblreceptionist Profile
        {
            get
            {

                tblreceptionist profile = db.tblreceptionists.FirstOrDefault(x => x.rid == id);
                return profile;
            }
        }
        public int inquiry
        {
            get
            {
                var count = db.onlines.Where(x => x.teacher == id).Count();
                return count;
            }
        }
        //public int Inprocess
        //{
        //    get
        //    {
        //        var count = db.Assigns.Where(x => x.teacher == id && x.Status == "InProcess").Count();
        //        return count;
        //    }
        //}
        public int Confirm
        {
            get
            {
                var count = db.onlines.Where(x => x.teacher == id && x.Sign == "Confirm").Count();
                return count;
            }
        }
        public string Sale()
        {

            string total = objsql.GetSingleValue("SELECT sum(amount) FROM applicationrecipts WHERE MONTH(date) = '" + System.DateTime.Now.Month + "' AND YEAR(date) = '" + System.DateTime.Now.Year + "'").ToString();
            return total;

        }
        public int SaleDifference()
        {
            string Thismonth = "0", previousmonth = "0";
            int a = System.DateTime.Now.Month - 1;
            int percentage;

            Thismonth = objsql.GetSingleValue("SELECT sum(amount) FROM applicationrecipts WHERE MONTH(date) = '" + System.DateTime.Now.Month + "' AND YEAR(date) = '" + System.DateTime.Now.Year + "'").ToString();
            previousmonth = objsql.GetSingleValue("SELECT sum(amount) FROM applicationrecipts WHERE MONTH(date) = '" + a + "' AND YEAR(date) = '" + System.DateTime.Now.Year + "'").ToString();
            if (previousmonth=="")
            {
                previousmonth = "0";
            }
            if(Thismonth=="")
            {
                Thismonth = "0";
            }
            int total = ((Convert.ToInt32(Thismonth)-(Convert.ToInt32(previousmonth))) * Convert.ToInt32(100));
            if(total>0)
            {
                percentage = (total / Convert.ToInt32(Thismonth));
            }
            else
            {
                percentage = 0;
            }
            
            return percentage;

        }
    }
}