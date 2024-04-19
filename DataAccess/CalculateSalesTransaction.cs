using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess
{
    
    public class CalculateSalesTransaction
    {
        [Key]
        public int PrimaryID { get; set; }

        public int TotalCount { get; set; }
    }
}