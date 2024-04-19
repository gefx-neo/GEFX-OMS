using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class PaymentModeLists
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

		[Display(Name = "Instruction Text")]
		public string InstructionText { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int IsDeleted { get; set; }
    }
}