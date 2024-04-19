using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
	public class Temp_CustomerAppointmentOfStaffs
	{
		[Key]
		public int ID { get; set; }

		public int CustomerParticularId { get; set; }

		[Display(Name = "Full Name*:")]
		public string FullName { get; set; }

		[Display(Name = "IC/Passport No*:")]
		public string ICPassportNo { get; set; }

		[Display(Name = "Nationality*:")]
		public string Nationality { get; set; }

		[Display(Name = "Job Title*:")]
		public string JobTitle { get; set; }

		[Display(Name = "Specimen Signature*:")]
		public string SpecimenSignature { get; set; }

	}
}