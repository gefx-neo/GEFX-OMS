using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class Temp_CustomerCustomRates
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
		[Required(ErrorMessage = "Encashment Rate is required!")]
		public decimal? EncashmentRate { get; set; }

		[ForeignKey("ProductId")]
		public virtual Product Products { get; set; }

		public Temp_CustomerCustomRates()
		{
			EncashmentRate = Convert.ToDecimal(1.005);
		}
	}
}