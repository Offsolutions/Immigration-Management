using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Consoller.Models
{
    public class dbcontext : DbContext
    {
        public dbcontext() : base("dbcontext")
        {
            //  Database.SetInitializer<dbcontext>(new CreateDatabaseIfNotExists<dbcontext>());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<dbcontext, Consoller.Migrations.Configuration>("dbcontext"));

        }

        public System.Data.Entity.DbSet<Consoller.Models.tblroom> tblrooms { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Course> Courses { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.tblstudentdata> tblstudentdata { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.tblreceptionist> tblreceptionists { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Fees_Master> Fees_Master { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.tblReceipt> tblReceipt { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.StudentCourse> StudentCourses { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Recipt_Details> Recipt_Details { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.tblinquiry> tblinquiries { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.tblfeedback> tblfeedback { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.tblfill> tblfills { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.tblsms> tblsms { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.tbldetail> tbldetails { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Franchisee> Franchisees { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Expense> Expenses { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Reception> Receptions { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Timing> Timings { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.MorningAttendence> MorningAttendeces { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.online> onlines { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Logs> Logs { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Assign> Assigns { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Country> Countries { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.College> Colleges { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.CollegeCourse> CollegeCourses { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Year> Years { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Month> Months { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Offerletter> Offerletter { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Application> Applications { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Leader> Ledgers { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Concession> Concessions { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.ApplicationRecipt> ApplicationRecipts { get; set; }
        public System.Data.Entity.DbSet<Consoller.Models.Document> Documents { get; set; }

        public System.Data.Entity.DbSet<Consoller.Models.PaymentStep> PaymentSteps { get; set; }
    }
}