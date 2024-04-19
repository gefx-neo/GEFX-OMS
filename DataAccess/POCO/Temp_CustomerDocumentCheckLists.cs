using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class Temp_CustomerDocumentCheckLists
	{
		[Key]
		public int ID { get; set; }

		public int CustomerParticularId { get; set; }

		[Display(Name = "Account opening form completed and signed by Authorizing Director:")]
		public string Company_AccountOpeningForm { get; set; }

		[Display(Name = "Photocopy of Identity Card (or valid Working Pass for non-Singaporean) with photograph of all the <u>Authorised Trading Persons</u>:")]
		public string Company_ICWithAuthorizedTradingPersons { get; set; }

		[Display(Name = "Photocopy of Identity Card (or Passport for non-Singaporean) with photograph of <u>Director(s)</u>:")]
		public string Company_ICWithDirectors { get; set; }

		[Display(Name = "Company business profile from ACRA of not later than 6 months:")]
		public string Company_BusinessProfileFromAcra { get; set; }

		[Display(Name = "Image of Photo ID of yourself (Passport AND valid Working Pass for non-Singaporean):")]
		public string Company_SelfiePassporWorkingPass { get; set; }

		[Display(Name = "Selfie of you and your Photo ID:")]
		public string Company_SelfiePhotoID { get; set; }

		[Display(Name = "Photocopy of IC (or valid Working Pass for non-Singaporean) with photograph:")]
		public string Natural_ICOfCustomer { get; set; }

		[Display(Name = "Business/Name Card:")]
		public string Natural_BusinessNameCard { get; set; }

		[Display(Name = "Signed KYC for natural person authorisation form:")]
		public string Natural_KYCForm { get; set; }

		[Display(Name = "Selfie of you and your Photo ID:")]
		public string Natural_SelfiePhotoID { get; set; }
	}
}