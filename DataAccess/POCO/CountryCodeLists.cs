using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class CountryCodeLists
	{
		[Key]
		public int ID { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public int IsDeleted { get; set; }
	}
}