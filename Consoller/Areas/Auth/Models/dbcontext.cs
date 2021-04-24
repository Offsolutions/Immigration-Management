using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Consoller.Areas.Auth.Models
{
    public class dbcontext : DbContext
    {
        public dbcontext() : base("dbcontext")
        {
          // Database.SetInitializer<dbcontext>(new CreateDatabaseIfNotExists<dbcontext>());

           // Database.SetInitializer(new MigrateDatabaseToLatestVersion<dbcontext, Consoller.Migrations.Configuration>("dbcontext"));
            
        }

        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tblroom> tblrooms { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Course> Courses { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tblstudentdata> tblstudentdata { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tblreceptionist> tblreceptionists { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Fees_Master> Fees_Master { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tblReceipt> tblReceipt { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.StudentCourse> StudentCourses { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Recipt_Details> Recipt_Details { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tblinquiry> tblinquiries { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tblfeedback> tblfeedback { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tblfill> tblfills { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tblsms> tblsms { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.tbldetail> tbldetails { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Franchisee> Franchisees { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Expense> Expenses { get; set; }
       // public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Reception> Receptions { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Timing> Timings { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.MorningAttendence> MorningAttendeces { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.online> onlines { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Logs> Logs { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Assign> Assigns { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.AssignIelts> AssignIelts { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Country> Countries { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.College> Colleges { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.CollegeCourse> CollegeCourses { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Year> Years { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Month> Months { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Offerletter> Offerletter { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Application> Applications { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Leader> Ledgers { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Concession> Concessions { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.ApplicationRecipt> ApplicationRecipts { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Document> Documents { get; set; }

        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.PaymentStep> PaymentSteps { get; set; }

        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Agent> Agents { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Process> Process { get; set; }

        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.GIC> Gic { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Medicals> Medicals { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.SubmitFile> SubmitFiles { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.TrackingLogs> TrackingLogs { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Chat> Chats { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Intake> Intakes { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.Section> Sections { get; set; }
        public System.Data.Entity.DbSet<Consoller.Areas.Auth.Models.ExpenseLock> ExpenseLocks { get; set; }
    }
}