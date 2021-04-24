using Consoller.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Controllers
{
    public class SearchProgramController : Controller
    {
        dbcontext db = new dbcontext();
        // GET: SearchProgram
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string CustomerId,int? CountryId)
        {
            if(CustomerId!=null && CountryId!=null)
            {
                
                dbcontext entities = new dbcontext();
                IEnumerable<CollegeCourse> ccourse = entities.CollegeCourses.Where(x => x.Name.Contains(CustomerId) && x.Cid == CountryId).ToList();
                return View(ccourse);
            }
            else if(CountryId!=null)
            {
                dbcontext entities = new dbcontext();
                IEnumerable<CollegeCourse> ccourse = entities.CollegeCourses.Where(x => x.Cid==CountryId).ToList();
                return View(ccourse);
            }
            else
            {
                dbcontext entities = new dbcontext();
                IEnumerable<CollegeCourse> ccourse = entities.CollegeCourses.Where(x => x.Name.Contains(CustomerId)).ToList();
                return View(ccourse);
            }
           
        }
        [HttpPost]
        public JsonResult SearchCourse(string prefix)
        {
            dbcontext entities = new dbcontext();
            var customers = (from customer in entities.CollegeCourses
                             where customer.Name.Contains(prefix) && customer.Status==true
                             select new
                             {
                                 label = customer.Name,
                                 val = customer.Oid
                             }).ToList();

            return Json(customers);
        }
        [HttpPost]
        public JsonResult SearchCountry(string prefix)
        {
            dbcontext entities = new dbcontext();
            var customers = (from country in entities.Countries
                             select new
                             {
                                 label = country.CountryName,
                                 val = country.Cid
                             }).ToList();

            return Json(customers);
        }
        public ActionResult ProgramDetails(int id)
        {
            return View(db.CollegeCourses.Find(id));
        }
        public ActionResult CollegeDetails(int id)
        {
            return View(db.Colleges.Find(id));
        }

    }
}