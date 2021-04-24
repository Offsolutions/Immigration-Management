using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consoller.Areas.Auth.Models
{
    public class FranchDetails
    {
        dbcontext db = new dbcontext();
        DateTime date = System.DateTime.Now;
        public int Totalstudent(string id)
        {
            return db.tblstudentdata.Where(x => x.uid == id).Count();
        }
        public int TotalEnquiry(string id)
        {
            return db.tblinquiries.Where(x => x.franchid == id).Count();
        }
        public float TodayPayment(string id)
        {
            Recipt_Details rr = db.Recipt_Details.FirstOrDefault(x => x.franchid == id);
            if(rr!=null)
            {
                 var total= db.Recipt_Details.Where(x => x.franchid == id && x.Date == date).Sum(x => x.Amount);
                if(total!=null)
                {
                    return total;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
            
        }
        public float TodayExpense(string id)
        {
            Expense ee = db.Expenses.FirstOrDefault(x => x.franchid == id);
            if(ee!=null)
            {
                return db.Expenses.Where(x => x.franchid == id && x.Date == date).Sum(x => x.Amount);
            }
            else
            {
                return 0;
            }

            
        }
        //public float TodayExpense(string id)
        //{
        //    return db.Expenses.Where(x => x.franchid == id && x.Date == date).Sum(x => x.Amount);
        //}
    }
    
}