using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class RemittanceProducts
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Currency Code *:")]
        public string CurrencyCode { get; set; }

        [Display(Name = "Currency Name *:")]
        public string CurrencyName { get; set; }

        [Display(Name = "GET Rate:")]
        public decimal? GetRate { get; set; }

        [Display(Name = "Automated GET Rate:")]
        public decimal? AutomatedGetRate { get; set; }

        [Display(Name = "PAY Rate:")]
        public decimal? PayRate { get; set; }

        [Display(Name = "Automated PAY Rate:")]
        public decimal? AutomatedPayRate { get; set; }

        [Display(Name = "Decimal:")]
        public int ProductDecimal { get; set; }

        [Display(Name = "Symbol:")]
        public string ProductSymbol { get; set; }

        [Display(Name = "Acceptable Range *:")]
        public decimal AcceptableRange { get; set; }

        [Display(Name = "Guarantee Rates*:")]
        public int GuaranteeRates { get; set; }

        [Display(Name = "Popular Currencies*:")]
        public int PopularCurrencies { get; set; }

        [Display(Name = "Max Amount:")]
        public decimal MaxAmount { get; set; }

		[Display(Name = "Default Pay Rate Adjustment*:")]
		[Required(ErrorMessage = "Default Pay Rate Adjustment is required!")]
		public decimal? BuyRateAdjustment { get; set; }

		[Display(Name = "Default Get Rate Adjustment*:")]
		[Required(ErrorMessage = "Default Buy Rate Adjustment is required!")]
		public decimal? SellRateAdjustment { get; set; }

		[Display(Name = "Transaction Fee:")]
        public decimal TransactionFee { get; set; }

        [Display(Name = "Payment Mode Allowed:")]
        public string PaymentModeAllowed { get; set; }

        [Display(Name = "Transaction Type Allowed:")]
        public string TransactionTypeAllowed { get; set; }

        [Display(Name = "Status *:")]
        public string Status { get; set; }

        public int IsBaseProduct { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string IsDeleted { get; set; }
    }
}