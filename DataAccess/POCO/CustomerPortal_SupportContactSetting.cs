using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class CustomerPortal_SupportContactSetting
	{
		[Key]
		public int ID { get; set; }

		public string Description { get; set; }

		public Byte[] Image { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		[Display(Name = "Phone No.")]
		public string PhoneNo { get; set; }
	}
}