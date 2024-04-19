using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class CustomerParticular
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Customer Code*:")]
        [Required(ErrorMessage = "Customer Code is required!")]
        public string CustomerCode {get;set;}

        [Display(Name = "Customer Type*:")]
        [Required(ErrorMessage = "Customer Type is required!")]
        public string CustomerType {get;set;}

        [Display(Name = "Registered Name*:")]
        //[Required(ErrorMessage = "Registered Name is required!")]
        public string Company_RegisteredName {get;set;}

        [Display(Name = "Registered Address*:")]
        //[Required(ErrorMessage = "Registered Address is required!")]
        public string Company_RegisteredAddress { get; set; }

        [Display(Name = "Business Address*:")]
        //[Required(ErrorMessage = "Business Address is required!")]
        public string Company_BusinessAddress1 { get; set; }

        [Display(Name = "Business Address*:")]
        //[Required(ErrorMessage = "Business Address is required!")]
        public string Company_BusinessAddress2 { get; set; }

		public string Company_BusinessAddress3 { get; set; }

		[Display(Name = "Postal Code*:")]
        //[Required(ErrorMessage = "Business Address is required!")]
        public string Company_PostalCode { get; set; }

        [Display(Name = "Contact Name*:")]
        //[Required(ErrorMessage = "Contact Name is required!")]
        public string Company_ContactName { get; set; }

        [Display(Name = "Tel No.*:")]
        //[Required(ErrorMessage = "Tel No is required!")]
        public string Company_TelNo { get; set; }

        [Display(Name = "Fax No.:")]
        //[Required(ErrorMessage = "Fax No is required!")]
        public string Company_FaxNo { get; set; }

        [Display(Name = "Email*:")]
        //[Required(ErrorMessage = "Email is required!")]
        public string Company_Email { get; set; }

        [Display(Name = "Place of Registration*:")]
        //[Required(ErrorMessage = "Place of Registration is required!")]
        public string Company_PlaceOfRegistration { get; set; }

        [Display(Name = "Date of Registration*:")]
        //[Required(ErrorMessage = "Date of Registration is required!")]
        public DateTime? Company_DateOfRegistration { get; set; }

        [Display(Name = "Registration No.*:")]
        //[Required(ErrorMessage = "Registration No is required!")]
        public string Company_RegistrationNo { get; set; }

        [Display(Name = "Type of Entity*:")]
        //[Required(ErrorMessage = "Type of Entity is required!")]
        public string Company_TypeOfEntity { get; set; }

        [Display(Name = "If Others")]
        public string Company_TypeOfEntityIfOthers { get; set; }

        [Display(Name = "Purpose and Intended Nature of Account Relationship and/or Relevant Business Transaction Undertaken")]
        //[Required(ErrorMessage = "Purpose and Intended Nature of Account Relationship and/or Relevant Business Transaction Undertaken is required!")]
        public string Company_PurposeAndIntended { get; set; }

		[Display(Name = "Contact No. (Home)*:")]
		public string Company_ContactNoH { get; set; }

		[Display(Name = "Contact No. (Office)*:")]
		public string Company_ContactNoO { get; set; }

		[Display(Name = "Contact No. (Mobile)*:")]
		public string Company_ContactNoM { get; set; }

		[Display(Name = "IC/Passport No.*:")]
		public string Company_ICPassport { get; set; }

		[Display(Name = "Job Title*:")]
		public string Company_JobTitle { get; set; }

		[Display(Name = "Nationality*:")]
		public string Company_Nationality { get; set; }

		public int Company_Country { get; set; }

		public int Company_CountryCode { get; set; }

		[Display(Name = "Name*:")]
        //[Required(ErrorMessage = "Name is required!")]
        public string Natural_Name { get; set; }

        [Display(Name = "Permanent Address*:")]
        //[Required(ErrorMessage = "Permanent Address is required!")]
        public string Natural_PermanentAddress { get; set; }

		public string Natural_PermanentAddress2 { get; set; }

		public string Natural_PermanentAddress3 { get; set; }

		[Display(Name = "Permanent Postal Code*:")]
		public string Natural_PermanentPostalCode { get; set; }

		[Display(Name = "Mailing Address*:")]
        //[Required(ErrorMessage = "Mailing Address is required!")]
        public string Natural_MailingAddress { get; set; }

		public string Natural_MailingAddress2 { get; set; }

		public string Natural_MailingAddress3 { get; set; }

		[Display(Name = "Nationality*:")]
        //[Required(ErrorMessage = "Nationality is required!")]
        public string Natural_Nationality { get; set; }

        [Display(Name = "IC/Passport No.*:")]
        //[Required(ErrorMessage = "IC/Passport No is required!")]
        public string Natural_ICPassportNo { get; set; }

        [Display(Name = "Date of Birth*:")]
        //[Required(ErrorMessage = "Date of Birth is required!")]
        public DateTime? Natural_DOB { get; set; }

        [Display(Name = "Contact No. (H)*:")]
        //[Required(ErrorMessage = "Contact No. (H) is required!")]
        public string Natural_ContactNoH { get; set; }

        [Display(Name = "Contact No. (O)*:")]
        //[Required(ErrorMessage = "Contact No (O) is required!")]
        public string Natural_ContactNoO { get; set; }

        [Display(Name = "Contact No. (M)*:")]
        //[Required(ErrorMessage = "Contact No (M) is required!")]
        public string Natural_ContactNoM { get; set; }

        [Display(Name = "Email*:")]
        //[Required(ErrorMessage = "Email is required!")]
        public string Natural_Email { get; set; }

        [Display(Name = "Employment Type*:")]
        //[Required(ErrorMessage = "Employment Type is required!")]
        public string Natural_EmploymentType { get; set; }

        [Display(Name = "Name of Employer*:")]
        //[Required(ErrorMessage = "Name of Employer is required!")]
        public string Natural_EmployedEmployerName { get; set; }

        [Display(Name = "Job Title*:")]
        //[Required(ErrorMessage = "Job Title is required!")]
        public string Natural_EmployedJobTitle { get; set; }

        [Display(Name = "Registered Address of Employer*:")]
        //[Required(ErrorMessage = "Registered Address of Employer is required!")]
        public string Natural_EmployedRegisteredAddress { get; set; }

		public string Natural_EmployedRegisteredAddress2 { get; set; }

		public string Natural_EmployedRegisteredAddress3 { get; set; }

		[Display(Name = "Name of Business*:")]
        //[Required(ErrorMessage = "Name of Business is required!")]
        public string Natural_SelfEmployedBusinessName { get; set; }

        [Display(Name = "Business Registration No.*:")]
        //[Required(ErrorMessage = "Business Registration No is required!")]
        public string Natural_SelfEmployedRegistrationNo { get; set; }

        [Display(Name = "Registered Business Address*:")]
        //[Required(ErrorMessage = "Registered Business Address is required!")]
        public string Natural_SelfEmployedBusinessAddress { get; set; }

		[Display(Name = "Principal Place of Business*:")]
        //[Required(ErrorMessage = "Principal Place of Business is required!")]
        public string Natural_SelfEmployedBusinessPrincipalPlace { get; set; }

		[Display(Name = "Mailing Address*:")]
		public string Shipping_Address1 { get; set; }

		[Display(Name = "Mailing Address 2:")]
		public string Shipping_Address2 { get; set; }

		[Display(Name = "Mailing Address 3:")]
		public string Shipping_Address3 { get; set; }

		[Display(Name = "Mailing Postal Code*:")]
		public string Shipping_PostalCode { get; set; }

		[Display(Name = "Mailing Postal Code*:")]
		public string Mailing_PostalCode { get; set; }

		[Display(Name = "Search Tag:")]
		public string SearchTags { get; set; }

		public string Password { get; set; }

		public int hasCustomerAccount { get; set; }

		public int isVerify { get; set; }

		public string VerifyAccountToken { get; set; }

		public int isKYCVerify { get; set; }

		public int lastTempPageUpdate { get; set; }

		public int lastKYCPageUpdate { get; set; }

		[Display(Name = "Birthday*:")]
		public DateTime? DOB { get; set; }

		[Display(Name = "Transaction Allowed Type (Customer Portal):")]
		public string EnableTransactionType { get; set; }

		public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string IsDeleted { get; set; }

		public long IsSubAccount { get; set; }

		public string ResetPasswordToken { get; set; }

		public DateTime? LastPasswordUpdated { get; set; }

		public string Customer_Title { get; set; }

		public string Surname { get; set; }

		public string GivenName { get; set; }

		public virtual List<CustomerSourceOfFund> SourceOfFunds { get; set; }

        public virtual List<CustomerActingAgent> ActingAgents { get; set; }

        public virtual List<CustomerAppointmentOfStaff> AppointmentOfStaffs { get; set; }

        public virtual List<CustomerDocumentCheckList> DocumentCheckLists { get; set; }

        public virtual List<CustomerScreeningReport> PEPScreeningReports { get; set; }

		public virtual List<CustomerActivityLog> ActivityLogs { get; set; }

		public virtual List<CustomerOther> Others { get; set; }

        public virtual List<CustomerCustomRate> CustomRates { get; set; }

		public virtual List<CustomerRemittanceProductCustomRate> CustomerRemittanceProductCustomRates { get; set; }

		[ForeignKey("Company_Country")]
		public virtual Countries Countries { get; set; }

		[ForeignKey("Company_CountryCode")]
		public virtual CountryCodeLists CountryCode { get; set; }
	}
}