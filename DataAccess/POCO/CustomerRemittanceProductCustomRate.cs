using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class CustomerRemittanceProductCustomRate
	{
		[Key]
		public int ID { get; set; }

		public int CustomerParticularId { get; set; }

		public int RemittanceProductId { get; set; }

		[Display(Name = "Pay Rate Adjustment")]
		//[Required(ErrorMessage = "Transaction Fee is required!")]
		public decimal? PayRateAdjustment { get; set; }

		[Display(Name = "Get Rate Adjustment")]
		//[Required(ErrorMessage = "Transaction Fee is required!")]
		public decimal? GetRateAdjustment { get; set; }

		[Display(Name = "Transaction Fee")]
		[Required(ErrorMessage = "Transaction Fee is required!")]
		public decimal? Fee { get; set; }

		[ForeignKey("CustomerParticularId")]
		public virtual CustomerParticular CustomerParticulars { get; set; }

		[ForeignKey("RemittanceProductId")]
		public virtual RemittanceProducts RemittanceProduct { get; set; }
	}
}