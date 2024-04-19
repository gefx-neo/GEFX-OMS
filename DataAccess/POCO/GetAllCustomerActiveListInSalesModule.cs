using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class GetAllCustomerActiveListInSalesModule
	{
		public int ID { get; set; }

		public long IsSubAccount { get; set; }

		public string IsDeleted { get; set; }

		public string CustomerType { get; set; }

		public string Company_RegisteredName { get; set; }

		public string Natural_Name { get; set; }

		public string Customer_Profile { get; set; }

		public string CustomerCode { get; set; }

		public string SearchTags { get; set; }

		public string Natural_EmployedEmployerName { get; set; }

		public string Natural_SelfEmployedBusinessName { get; set; }

		public string Natural_EmploymentType { get; set; }

		public string Company_ContactName { get; set; }

		public string Company_TelNo { get; set; }

		public string Natural_ContactNoH { get; set; }

		public string Status { get; set; }
	}
}