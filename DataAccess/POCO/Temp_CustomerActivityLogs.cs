using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class Temp_CustomerActivityLogs
	{
		[Key]
		public int ID { get; set; }

		public int CustomerParticularId { get; set; }

		[Display(Name = "IP Address")]
		public string Title { get; set; }

		[Display(Name = "Activity Log DateTime")]
		public DateTime ActivityLog_DateTime { get; set; }

		[Display(Name = "Note")]
		public string ActivityLog_Note { get; set; }
	}
}