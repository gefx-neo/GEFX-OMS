using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class BeneficiaryList
    {
        [Key]
        public long ID { get; set; }

        public string BeneficiaryFullName { get; set; }

        public string BeneficiaryFriendlyName { get; set; }

        public string BankAccountNo { get; set; }

        public string CountryName { get; set; }

        public string Status { get; set; }
    }
}