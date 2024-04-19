using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.POCO
{
	public class ApprovalHistorys
	{
		[Key]
		public long ID { get; set; }

		public string Application { get; set; }

		public int ObjectId { get; set; }

		public int ApprovalUserId { get; set; }

		public string ApprovalUserName { get; set; }

		public string Role { get; set; }

		public DateTime DateTimeAction { get; set; }

		public string Action { get; set; }

		public string Description { get; set; }

	}
}