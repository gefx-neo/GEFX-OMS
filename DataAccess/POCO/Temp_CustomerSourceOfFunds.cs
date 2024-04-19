using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class Temp_CustomerSourceOfFunds
	{
		[Key]
		public int ID { get; set; }

		public int CustomerParticularId { get; set; }

		[Display(Name = "Source of Funds*:")]
		//[Required(ErrorMessage = "Source of Funds is required!")]
		public string Company_SourceOfFund { get; set; }

		[Display(Name = "If Others")]
		public string Company_SourceOfFundIfOthers { get; set; }

		[Display(Name = "Is the benficial owner or has the beneficial owner ever been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?*")]
		//[Required(ErrorMessage = "This field is required!")]
		public string Company_PoliticallyExposedIndividuals_1 { get; set; }

		[Display(Name = "Is the benficial owner or has the beneficial owner ever been a parent/ step-parent /step-child, adopted child/ spouse/ sibling/ step-sibling/ adopted sibling of anyone who is or has been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?*")]
		//[Required(ErrorMessage = "This field is required!")]
		public string Company_PoliticallyExposedIndividuals_2 { get; set; }

		[Display(Name = "Is the beneficial owner or has the beneficial owner ever been closely connected, either socially or professionally with anyone who is or has been entrusted with prominent public figure functions, whether in Singapore, in a foreign country, or in an international organisation?*")]
		//[Required(ErrorMessage = "This field is required!")]
		public string Company_PoliticallyExposedIndividuals_3 { get; set; }

		public string Company_ServiceLikeToUse { get; set; }

		public string Company_PurposeOfIntendedTransactions { get; set; }

		public string Company_HearAboutUs { get; set; }

		[Display(Name = "Source of Funds*:")]
		//[Required(ErrorMessage = "Source of Funds is required!")]
		public string Natural_SourceOfFund { get; set; }

		[Display(Name = "If Others")]
		public string Natural_SourceOfFundIfOthers { get; set; }

		[Display(Name = "Annual Income*:")]
		//[Required(ErrorMessage = "Annual Income is required!")]
		public string Natural_AnnualIncome { get; set; }

		[Display(Name = "Are you or have you ever been entrusted with prominent figure functions, whether in Singapore, in a foreign country, or in an international organisation?*")]
		//[Required(ErrorMessage = "This field is required!")]
		public string Natural_PoliticallyExposedIndividuals_1 { get; set; }

		[Display(Name = "Are you or have you ever been parent/step-parent/step-child, adopted child/spouse/sibling/step-sibling/adopted sibling of anyone who is or has been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?*")]
		//[Required(ErrorMessage = "This field is required!")]
		public string Natural_PoliticallyExposedIndividuals_2 { get; set; }

		[Display(Name = "Are you closely connected, either socially or professionally with anyone who is or has been entrusted with prominent public functions, whether in Singapore, in a foreign country, or in an international organisation?*")]
		//[Required(ErrorMessage = "This field is required!")]
		public string Natural_PoliticallyExposedIndividuals_3 { get; set; }

		public string Natural_ServiceLikeToUse { get; set; }

		public string Natural_PurposeOfIntendedTransactions { get; set; }

		public string Natural_HearAboutUs { get; set; }

	}
}