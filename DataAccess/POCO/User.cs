using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "NRIC*:")]
        [Required(ErrorMessage = "NRIC is required!")]
        public string NRIC { get; set; }

        [Display(Name = "Name*:")]
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Display(Name = "Email*:")]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        [Display(Name = "Role*:")]
        //[Required(ErrorMessage = "Role is required!")]
        public string Role { get; set; }

        [Display(Name = "Status*:")]
        [Required(ErrorMessage = "Status is required!")]
        public string Status { get; set; }

        [Display(Name = "Password*:")]
        //[Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string IsDeleted { get; set; }

        public DateTime LastLogin { get; set; }

        public string ResetPasswordToken { get; set; }
    }
}