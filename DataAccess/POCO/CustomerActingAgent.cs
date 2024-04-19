using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class CustomerActingAgent
    {
        [Key]
        public int ID { get; set; }

        public int CustomerParticularId { get; set; }

        [Display(Name = "Are you an agent acting on behalf of the customer?*")]
        [Required(ErrorMessage = "Please answer this question!")]
        public string ActingAgent { get; set; }

        [Display(Name = "If yes, Customer Type*:")]
        public string Company_CustomerType { get; set; }

        [Display(Name = "Address*:")]
        public string Company_Address { get; set; }

        [Display(Name = "Place of Registration*:")]
        public string Company_PlaceOfRegistration { get; set; }

        [Display(Name = "Registration No./Identification No.*:")]
        public string Company_RegistrationNo { get; set; }

        [Display(Name = "Date of Registration/Date of Birth*:")]
        public DateTime? Company_DateOfRegistration { get; set; }

        [Display(Name = "Name*:")]
        public string Natural_Name { get; set; }

        [Display(Name = "Permanent Address*:")]
        public string Natural_PermanentAddress { get; set; }

        [Display(Name = "Nationality*:")]
        public string Natural_Nationality { get; set; }

        [Display(Name = "IC/Passport No.*:")]
        public string Natural_ICPassportNo { get; set; }

        [Display(Name = "Date of Birth*:")]
        public DateTime? Natural_DOB { get; set; }

        [Display(Name = "Relationship between Agent(s) and Client*:")]
        public string Relationship { get; set; }

        [Display(Name = "Basis of Authority*:")]
        public string BasisOfAuthority { get; set; }

        [ForeignKey("CustomerParticularId")]
        public virtual CustomerParticular CustomerParticulars { get; set; }
    }
}