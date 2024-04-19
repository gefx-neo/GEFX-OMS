using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class Temp_CustomerScreeningReports
	{
		[Key]
		public int ID { get; set; }

		public int CustomerParticularId { get; set; }

		public DateTime Date { get; set; }

		public DateTime DateOfAcra { get; set; }

		public string ScreenedBy { get; set; }

		public string ScreeningReport_1 { get; set; }

		public string ScreeningReport_2 { get; set; }

		public string Remarks { get; set; }
	}
}