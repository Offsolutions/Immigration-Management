using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consoller.Models
{
    public class Home
    {

    }
    public class tblstudentdata
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Roll No.")]
        public int rollno { get; set; }
        [Required(ErrorMessage ="Enter Your Name")]
        [Display(Name = "Name")]
        public string name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [Display(Name = "DOB")]
        public Nullable<System.DateTime> dob { get; set; }
        [Display(Name = "Father Name")]
        public string fathername { get; set; }
        [AllowHtml]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required(ErrorMessage ="Please Enter Your 10 Digit Mobile no")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone No.")]
        public string phone { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Father Phone No.")]
        public string fatherphn { get; set; }
        [Display(Name = "Language")]
        public string language { get; set; }
        [Display(Name = "Board")]
        public string board { get; set; }
        [Display(Name = "Qualification")]
        public string qualification { get; set; }
        [Display(Name = "Prior Coaching")]
        public string coaching { get; set; }
        [Display(Name = "Institute Name")]
        public string institutename { get; set; }
        [Display(Name = "Ielts Type")]
        public string type { get; set; }
        [Display(Name = "Reffered By")]
        public string refferedby { get; set; }
        [Display(Name = "Student Image")]
        public string image { get; set; }
        [Display(Name = "User")]
        public string uid { get; set; }
        [Display(Name = "Status")]
        public bool Status { get; set; }
        [Display(Name = "Username")]
        public string username { get; set; }
        [Display(Name = "Password")]
        public string password { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Remarks")]
        public string remarks { get; set; }
        [Display(Name = "Student Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        
        public string email { get; set; }
        [Display(Name = "Discount")]
        public int discount { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       //[DefaultValue(DateTime.Now.ToString("MM-dd-yyyy"))]
        public DateTime date { get; set; }
        public int UserId { get; set; }
        public int Time { get; set; }
    }
    public enum gender
    {
        Male,
        Female
    }
    public enum board
    {
        CBSE,
        PSEB,
        ICSE,
        Other
    }
    public enum InquiryType
    {
       Days
    }
    public enum Status
    {
        Interested,
        NotInterested
    }
    public enum type
    {
        AC,
        GT
    }
    public enum Method
    {
        Cash,
        Cheque,
        NetBanking,
        Other
    }
    public enum UserType
    {
        Admin,
        Franchisee,Receptionist,Teacher,Consoller
    }
    public class Fees_Master
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Roll No.")]
        public int RollNo { get; set; }
        [Display(Name = "Total Fees")]
        public int TotalFees { get; set; }
        [Display(Name = "Paid Fees")]
        public int PaidFees { get; set; }
        [Display(Name = "Alert Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? AlertDate { get; set; }
        [Display(Name = "Date")]
        public Nullable<System.DateTime> Date { get; set; }
        [Display(Name = "Discount")]
        public int discount { get; set; }
        public bool Status { get; set; }
        public string franchid { get; set; }
        public int CourseId { get; set; }
        public int? Token { get; set; }
    }
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Course ID")]
        public int CourseId { get; set; }
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Display(Name = "Course Fees")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Fee must be numeric")]
        [Required]
        public string Fees { get; set; }
        //public virtual ICollection<Student_Course> Student_Course { get; set; }
        public string franchid { get; set; }
    }
    public class StudentCourse
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Roll No.")]
        public int RollNo { get; set; }
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        [Display(Name = "Admission Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Admitdate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> enddate { get; set; }
        [Display(Name = "Course Fees")]
        public string Fees { get; set; }
        [Display(Name = "User")]
        public string Uid { get; set; }
        [NotMapped]
        public int Days { get; set; }

        public int? Token { get; set; }
        public bool Status { get; set; }

    }
    public class Recipt_Details
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Roll No.")]
        public int RollNo { get; set; }
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        [Display(Name = "Recipt No.")]
        public string ReciptNo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        [Display(Name = "Amount")]
        public int Amount { get; set; }
        public bool Active { get; set; }
        public string franchid { get; set; }
    }
    public class tblReceipt
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Start No.")]
        public string Start_no { get; set; }
        [Display(Name = "End No.")]
        public string End_no { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        [Display(Name = "Current Recipt No.")]
        public string Current_Recipt { get; set; }
        public bool Status { get; set; }
    }
    public class tblroom
    {
        [Key]
        public int RoomId { get; set; }
        [Display(Name = "Room")]
        public string room { get; set; }
        [Display(Name = "Status")]
        public bool status { get; set; }
    }
    public class tblreceptionist
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "User Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "You must provide a valid email address.")]
        public string email { get; set; }
        [Display(Name = "Contact No.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string contact { get; set; }
        [Display(Name = "User Name")]
        public string login { get; set; }
        [Display(Name = "Password")]
        public string password { get; set; }
        [Display(Name = "User Id")]
        public string rid { get; set; }
        [Display(Name = "Image")]
        public string image { get; set; }
        [Display(Name = "User Type")]
        public string Type { get; set; }
        public string franchid { get; set; }
        [Display(Name = "Status")]
        public bool status { get; set; }
    }
    public class tblinquiry
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        [Display(Name = "Inquiry No.")]
        public string inquiryid { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Father Name")]
        public string fname { get; set; }
        [Display(Name = "Phone No.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string contact { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Reffered By")]
        public string referedby { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        [Display(Name = "Category")]
        public int Categoryid { get; set; }
        public virtual Category Category { get; set; }
        [Display(Name = "Status")]
        public bool status { get; set; }
        public string franchid { get; set; }
             
    }
    public class tblfeedback
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        [AllowHtml]
        public string inquiryid { get; set; }
        [AllowHtml]
        [Display(Name = "Feedback")]
        public string feedback { get; set; }
        [Display(Name = " ")]
        public int days { get; set; }
        [Display(Name = "Type")]
        public string type { get; set; }
        [Display(Name = "Next FollowUp")]
        public DateTime nextfollow { get; set; }
        [Display(Name = "Status")]
        public bool status { get; set; }
        [Display(Name = "User")]
        public string loginid { get; set; }
    }
    public class tblfill
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Passport No.")]
        public string passport { get; set; }
        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dob { get; set; }
        [Display(Name = "DOE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime doe { get; set; }
        [Display(Name = "First Choice")]
        public string choice1 { get; set; }
        [Display(Name = "Second Choice")]
        public string choice2 { get; set; }
        [Display(Name = "Third Choice")]
        public string choice3 { get; set; }
        [Display(Name = "Module Type")]
        public string module { get; set; }
        [Display(Name = "Venue 1")]
        public string v1 { get; set; }
        [Display(Name = "Venue 2")]
        public string v2 { get; set; }
        [Display(Name = "Venue 3")]
        public string v3 { get; set; }
        [Display(Name = "Cash Mode")]
        public string mode { get; set; }
        [Display(Name = "Institute Name")]
        public string instname { get; set; }
        [Display(Name = "Module")]
        public string status { get; set; }
        [Display(Name = "Test Id")]
        public string fid { get; set; }
        [Display(Name = "Username")]
        public string uname { get; set; }
        [Display(Name = "Password")]
        public string pass { get; set; }
        public string franchid { get; set; }
    }
    public enum module
    {
        GT,
        AC,
        PTE
    }
    public enum status
    {
        Idp,
        British,
        Booked,
        Not_Booked,
        Registered,
        Walkin
    }
    public class Category
    {
        [Key]
        public int Categoryid { get; set; }
        public string Name { get; set; }
        public virtual ICollection<tblinquiry> tblinquiry { get; set; }
    }
    public class tblsms
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "User Name")]
        public string Username { get; set; }
        [Display(Name = "Sender Id")]
        public string Senderid { get; set; }
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Display(Name = "API")]
        public string Api { get; set; }
        [Display(Name = "Alert No.")]
        public string alertno { get; set; }
        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
    public class tbldetail
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Institute Name")]
        public string name { get; set; }
        [Display(Name = "Contact")]
        public string phone { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "You must provide a valid email address.")]
        public string email { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Logo")]
        public string logo { get; set; }
       
        public int franchid { get; set; }
    }
    public class Stu_rec
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "RollNo")]
        public int RollNo { get; set; }
        [Display(Name = "Amount")]
        public int Amount { get; set; }
        [Display(Name = "Date")]
        public Nullable<System.DateTime> Date { get; set; }
        [Display(Name = "Recipt No")]
        public string ReciptNo { get; set; }
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
    }
    public class Franchisee
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage = "You must provide a valid email address.")]
        public string email { get; set; }
        [Display(Name = "Contact No.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string contact { get; set; }
        [Display(Name = "User Name")]
        public string login { get; set; }
        [Display(Name = "Password")]
        public string password { get; set; }
        [Display(Name = "User Id")]
        public string rid { get; set; }
        [Display(Name = "Image")]
        public string image { get; set; }
        [Display(Name = "User Type")]
        public string Type { get; set; }
        [Display(Name = "Status")]
        public bool status { get; set; }
    }
    public class Expense
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        
        public DateTime Date { get; set; }
        [Display(Name ="Item Name")]
        public string Item { get; set; }
        public float Amount { get; set; }
        public string Method { get; set; }
        public string Ref { get; set; }
        public string franchid { get; set; }
    }
    public class DailyAttendence
    {
        [Key]
        public int Aid { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public int Rollno { get; set; }
        public string Attendence { get; set; }
        public string franchid { get; set; }
        public AnswerType AnswerType { get; set; }

    }
    public class Attendence
    {[Key]
        public int Aid { get; set; }
        

        public string Date { get; set; }
        public int Rollno { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public string Reading { get; set; }
        public string Role { get; set; }
        public string Timing { get; set; }
        public string franchid { get; set; }


    }
    public class Timing
    {
        [Key]
        public int Tid { get; set; }
        public string BatchTime { get; set; }
        public string franchid { get; set; }
    }
    public class MorningAttendence
    {
        [Key]
        public int Aid { get; set; }


        public string Date { get; set; }
        public int Rollno { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public string Reading { get; set; }
        public string Speaking { get; set; }
        public string Role { get; set; }
        public int Timing { get; set; }
        public string franchid { get; set; }

        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime Check { get; set; }


    }
    public class online
    {
        [Key]
        public int Oid { get; set; }
        public string Date { get; set; }
        [Required]
        [Display(Name = "Any Refusal")]
        public string Refusal { get; set; }
        [Display(Name = "Refusal Country")]
        public string RefusalCountry { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]

        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "Please Select status")]
        public string Marital { get; set; }
        [Required]
        public string Gender { get; set; }
        public string VisaType { get; set; }
        [Required]
        [Display(Name = "Country You Want to Apply for")]
        public string Country { get; set; }
        public string Source { get; set; }
        [Display(Name = "Your Last Qualification")]
        public string Qualification { get; set; }
        [Display(Name = "Last Qualification Passing Year")]
        public int PassingYear { get; set; }
        [Display(Name = "Your Last Qualification Percentage")]
        public string Percentage { get; set; }
        [Display(Name = "Have you done IELTS or PTE?")]
        public string Paper { get; set; }
        public string TestType { get; set; }
        public string Band { get; set; }
        [Display(Name = "Education Documents")]
        public string Docs { get; set; }
        public string Passport { get; set; }
        public int inquiryid { get; set; }
        public string franchid { get; set; }
        public string teacher { get; set; }
        public string Sign { get; set; }
    }
    public class Logs
    {
        [Key]
        public int Lid { get; set; }
        public string Date { get; set; }
        public int? inquiryid { get; set; }
        public string Description { get; set; }
        public string franchid { get; set; }
        public string teacher { get; set; }
        public string Status { get; set; }

    }
    public class Assign
    {
        [Key]
        public int Aid { get; set; }
        public string Date { get; set; }
        public int? inquiryid { get; set; }
        public string teacher { get; set; }
        public string franchid { get; set; }
        public Boolean Status { get; set; }
    }
    public class otp
    {
        [Key]
        public int oid { get; set; }
        public string Mobile { get; set; }
        public string franchid { get; set; }
        public string checkotp { get; set; }
    }

    public class onlineform
    {
        [Key]
        public int Oid { get; set; }
        public string Date { get; set; }
        [Required]
        [Display(Name = "Any Refusal")]
        public string Refusal { get; set; }
        [Display(Name = "Refusal Country")]
        public string RefusalCountry { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]

        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "Please Select status")]
        public string Marital { get; set; }
        [Required]
        public string Gender { get; set; }
        public string VisaType { get; set; }
        [Required]
        [Display(Name = "Country You Want to Apply for")]
        public string Country { get; set; }
        public string Source { get; set; }
        [Display(Name = "Your Last Qualification")]
        public string Qualification { get; set; }
        [Display(Name = "Last Qualification Passing Year")]
        public int PassingYear { get; set; }
        [Display(Name = "Your Last Qualification Percentage")]
        public string Percentage { get; set; }
        [Display(Name = "Have you done IELTS or PTE?")]
        public string Paper { get; set; }
        public string TestType { get; set; }
        public string Band { get; set; }
        [Display(Name = "Education Documents")]
        public string Docs { get; set; }
        public string Passport { get; set; }
        public int? inquiryid { get; set; }
        public string franchid { get; set; }
    }
    public class Application
    {
        [Key]
        public int Aid { get; set; }
        public string Date { get; set; }
        [Required]
        public int ApplicationNo { get; set; }
        public int InquiryId { get; set; }
        public string Franchid { get; set; }
        public string DealBy { get; set; }
        public string Status { get; set; }
    }
    public class Offerletter
    {[Key]
        public int Lid { get; set; }
        public int ApplicationNo { get; set; }
    
        public string Applied { get; set; }

        public string Received { get; set; }
        public int Cid { get; set; }
        public int Oid { get; set; }
        public int CCid { get; set; }
        public int Yid { get; set; }
        public int Mid { get; set; }
        public int Amount { get; set; }
    }
    public class Leader
    {[Key]
        public int Lid { get; set; }
        public string Date { get; set; }
        public int? ApplicationNo { get; set; }
        public string LeaderNo { get; set; }
        public int Total { get; set; }
        public int Received { get; set; }
        public int Concession { get; set; }
        public string Status { get; set; }
    }
    public class Concession
    {[Key]
        public int Cid { get; set; }
        public string Date { get; set; }
        public int? ApplicationNo { get; set; }
        public int? InquiryId { get; set; }
        public string LeaderNo { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
    public class Country
    {
        [Key]
        public int Cid { get; set; }
        [Required(ErrorMessage = "Enter Country Name")]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }
        public virtual ICollection<College> College { get; set; }
    }
    public class College
    {
        [Key]
        public int Oid { get; set; }
        [Display(Name = "Country")]
        public int Cid { get; set; }
        public virtual Country Countries { get; set; }
        [Required(ErrorMessage = "Enter College Name")]
        [Display(Name = "College/Univ Name")]
        public string Name { get; set; }
    }
    public class CollegeCourse
    {
        [Key]
        public int CCid { get; set; }
        [Display(Name = "Country")]
        public int Cid { get; set; }
        [Display(Name = "College")]
        public int Oid { get; set; }
        [Display(Name = "Course Name")]
        [Required]
        public string Name { get; set; }
    }
    public class ApplicationRecipt
    {
        [Key]
        public int Rid { get; set; }
        public string Date { get; set; }
        public int ApplicationNo { get; set; }
        public string Leadger { get; set; }
        public string Type { get; set; }
        public int ReciptNo { get; set; }
        public int Amount { get; set; }
        public string RecivedBy { get; set; }

    }
    public class Year
    {
        [Key]
        public int Yid { get; set; }
        public string Name { get; set; }
    }
    public class Month
    {
        [Key]
        public int Mid { get; set; }
        public string Name { get; set; }
    }
    public class Document
    {[Key]
        public int Did { get; set; }
        public int ApplicationNo { get; set; }
        public string FileName { get; set; }
        public string Doc { get; set; }
    }

    public class PaymentStep
    {[Key]
        public int Pid { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; } = 0;
    }
    public enum refusal
    {
        Yes, No
    }
    public enum Paper
    {
        Yes, No
    }
    public enum Marital
    {
        Married, Single
    }
    public enum Gender
    {
        Male, Female
    }
    public enum VisaType
    {
        Student,
        [Display(Name = "Open Work Pernit")]
        OpenWorkPermit,
        VisitorVisa
    }
    public enum Testtype
    {
        Ielts, PTE
    }
    public enum Qualification
    {
        [Display(Name = "+2")]
        Twelfth,

        Diploma, Graduation, PostGraduation
    }
    public enum source
    {
        SocialMedia, Newspaper, Others
    }
    public enum ConsollerStatus
    {
        Open,
        Cancel,
        InProcess,
        Complete,
        Confirm
    }


}
public enum AnswerType
{
    Present,
    Absent,
    Leave
}
