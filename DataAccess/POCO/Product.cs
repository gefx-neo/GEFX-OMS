using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Currency Code*:")]
        [Required(ErrorMessage = "Currency Code is required!")]
        public string CurrencyCode { get; set; }

        [Display(Name = "Currency Name*:")]
        [Required(ErrorMessage = "Currency Name is required!")]
        public string CurrencyName { get; set; }

        [Display(Name = "Buy Rate:")]
        public decimal? BuyRate { get; set; }

        [Display(Name = "Sell Rate:")]
        public decimal? SellRate { get; set; }

        [Display(Name = "Encashment Rate*:")]
        [Required(ErrorMessage = "Encashment Rate is required!")]
        public decimal EncashmentRate { get; set; }

        [Display(Name = "Decimal*:")]
        [Required(ErrorMessage = "Decimal is required!")]
        public int Decimal { get; set; }

        [Display(Name = "Symbol*:")]
        [Required(ErrorMessage = "Symbol is required!")]
        public string Symbol { get; set; }

        [Display(Name = "Acceptable Range*:")]
        [Required(ErrorMessage = "Acceptable Range is required!")]
        public decimal AcceptableRange { get; set; }

        [Display(Name = "Unit*:")]
        [Required(ErrorMessage = "Unit is required!")]
        public int Unit { get; set; }

        [Display(Name = "Max Amount*:")]
        [Required(ErrorMessage = "Max Amount is required!")]
        public decimal? MaxAmount { get; set; }

        [Display(Name = "Default Buy Rate Adjustment*:")]
        [Required(ErrorMessage = "Default Buy Rate Adjustment is required!")]
        public decimal? BuyRateAdjustment { get; set; }

        [Display(Name = "Default Sell Rate Adjustment*:")]
        [Required(ErrorMessage = "Default Sell Rate Adjustment is required!")]
        public decimal? SellRateAdjustment { get; set; }

        [Display(Name = "Automated Buy Rate:")]
        public decimal? AutomatedBuyRate { get; set; }

        [Display(Name = "Automated Sell Rate:")]
        public decimal? AutomatedSellRate { get; set; }

        [Display(Name = "Payment Mode Allowed*:")]
        //[Required(ErrorMessage = "Payment Mode Allowed is required!")]
        public string PaymentModeAllowed { get; set; }

        [Display(Name = "Transaction Type Allowed*:")]
        //[Required(ErrorMessage = "Transaction Type Allowed is required!")]
        public string TransactionTypeAllowed { get; set; }

        [Display(Name = "Status*:")]
        [Required(ErrorMessage = "Status is required!")]
        public string Status { get; set; }

		[Display(Name = "Guarantee Rates*:")]
		public int GuaranteeRates { get; set; }

		[Display(Name = "Popular Currencies*:")]
		public int PopularCurrencies { get; set; }

		[Display(Name = "Transaction Type Allowed (Customer Portal):")]
		//[Required(ErrorMessage = "Transaction Type Allowed is required!")]
		public string TransactionTypeAllowedCustomer { get; set; }

		[Display(Name = "Encashment Mapping (Customer Portal)*:")]
		public int EncashmentMapping { get; set; }

		public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string IsDeleted { get; set; }

        public virtual List<ProductInventory> ProductInventories { get; set; }

        public virtual List<Inventory> Inventories { get; set; }

        public virtual List<ProductDenomination> ProductDenominations { get; set; }

        public Product()
        {
            EncashmentRate = Convert.ToDecimal(1.005);
            Symbol = "$";
        }
    }
}