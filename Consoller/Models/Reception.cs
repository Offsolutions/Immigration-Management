using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Consoller.Models
{
    public class Reception
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
            public int franchid { get; set; }
            [Display(Name = "Status")]
            public bool status { get; set; }
        
    }
}