using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.POCO
{
	public class SearchTags
	{
		[Key]
		public int ID { get; set; }

		public string TagName { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime UpdatedOn { get; set; }

		public string IsDeleted { get; set; }
	}
}