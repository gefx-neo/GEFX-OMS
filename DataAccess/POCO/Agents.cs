using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class Agents
	{
		[Key]
		public int ID { get; set; }

		[Display(Name = "Agent ID*:")]
		public string AgentId { get; set; }

		[Display(Name = "Company Name*:")]
		public string CompanyName { get; set; }

		[Display(Name = "Contact Person*:")]
		public string ContactPerson { get; set; }

		[Display(Name = "Contact Number*:")]
		public string ContactNumber { get; set; }

		[Display(Name = "Status*:")]
		public string Status { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime UpdatedOn { get; set; }

		public string IsDeleted { get; set; }

	}
}