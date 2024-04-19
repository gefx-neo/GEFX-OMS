using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class Setting
    {
        [Key]
        public int ID { get; set; }

        public string Code { get; set; }

        public string Value { get; set; }
    }
}