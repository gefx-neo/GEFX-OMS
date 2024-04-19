using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
	public class SaleListingModel
	{
		public int SalesID { get; set; }

		public string SalesMemoID { get; set; }

		public int SalesCreatedBy { get; set; }

		public DateTime SalesCreatedOn { get; set; }

		public int Sales_CustomerParticularID { get; set; }

		public string CP_CustomerCode { get; set; }

		public string CP_CompanyRegisteredName { get; set; }

		public string CP_NaturalName { get; set; }

		public string Sales_TransactionType { get; set; }

		public string Sales_Status { get; set; }
	}
}