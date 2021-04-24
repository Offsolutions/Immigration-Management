using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Consoller.Areas.Auth.Models;
using onlineportal.Areas.AdminPanel.Models;

namespace Consoller.Areas.Auth.Controllers
{
    public class ExpensesController : Controller
    {
        private dbcontext db = new dbcontext();
        Helper help = new Helper();
        SQLHelper objsql = new SQLHelper();
      
        // GET: Auth/Expenses
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Franchisee"))
            {
                string a = help.Franchisee();
                return View(await db.Expenses.Where(x => x.franchid == a).ToListAsync());
            }
            else
            {
                string a = help.Receptionist();
                return View(await db.Expenses.Where(x=>x.franchid==a).ToListAsync());
            }
        }

        // GET: Auth/Expenses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = await db.Expenses.FindAsync(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // GET: Auth/Expenses/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Auth/Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Date,Item,Amount,Method,Ref,franchid,Name")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                DateTime today = System.DateTime.Now;
               // ExpenseLock Locks = db.ExpenseLocks.FirstOrDefault(x => x.LockDate == today);
                DataTable dt = objsql.GetTable("select * from expenselocks where lockdate='" + today + "'");

                if(dt.Rows.Count>0)
                {

                    TempData["danger"] = "Sorry DayEnd";
                    return View();
                }
                else
                {
                    string franchid = help.Permission();
                    tblreceptionist re = db.tblreceptionists.FirstOrDefault(x => x.rid == franchid);
                    if(re.StartTime!=null || re.EndTime!=null)
                    {
                      
                       if(help.Checklock()==true)
                       {
                            db.Expenses.Add(expense);
                            expense.franchid = User.IsInRole("franchisee") ? help.Franchisee() : help.Receptionist();
                            await db.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["danger"] = "Sorry DayEnd";
                            return View();
                        }
                    }
                    else
                    {
                        db.Expenses.Add(expense);
                        expense.franchid = User.IsInRole("franchisee") ? help.Franchisee() : help.Receptionist();
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                 

                   
                   
                }
               
            }

            return View(expense);
        }

        // GET: Auth/Expenses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = await db.Expenses.FindAsync(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: Auth/Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Date,Item,Amount,Method,Ref,franchid,Name")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        // GET: Auth/Expenses/Delete/5
        [Authorize(Roles ="Franchisee")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = await db.Expenses.FindAsync(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: Auth/Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Expense expense = await db.Expenses.FindAsync(id);
            db.Expenses.Remove(expense);
            await db.SaveChangesAsync();
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
    }
}
