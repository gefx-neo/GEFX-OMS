using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class KYC_CustomerCustomRates
	{
		[Key]
		public int ID { get; set; }

		public int CustomerParticularId { get; set; }

		public int ProductId { get; set; }

		[Display(Name = "Buy Rate")]
		//[Required(ErrorMessage = "Buy Rate is required!")]
		public decimal? BuyRate { get; set; }

		[Display(Name = "Sell Rate")]
		//[Required(ErrorMessage = "Sell Rate is required!")]
		public decimal? SellRate { get; set; }

		[Display(Name = "Encashment Rate")]
		public decimal? EncashmentRate { get; set; }


	}
}