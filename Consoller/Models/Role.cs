using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consoller.Models
{
    public class Role
    {
        dbcontext db = new dbcontext();
        Helper help = new Helper();
        public string RoleName()
        {
            var name = (HttpContext.Current.User.Identity.IsAuthenticated? HttpContext.Current.User.Identity.Name : "Guest");
                    tblreceptionist rr = db.tblreceptionists.Where(x => x.rid == name).FirstOrDefault();
            return rr.name;
    
        }
        //public int alert()
        //{
        //    DateTime date = System.DateTime.Now;
        //    string a = help.Permission();
        //    return db.Fees_Master.Where(x => x.AlertDate <= date && x.franchid == a && x.Status == true).Count();
        //}
        
    }
}