﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataAccess.POCO
{
    public class BusinessCategoriesLists
    {
        [Key]
        public int ID { get; set; }

		public string Headers { get; set; }

        public string Name { get; set; }

        public int IsDeleted { get; set; }
    }
}