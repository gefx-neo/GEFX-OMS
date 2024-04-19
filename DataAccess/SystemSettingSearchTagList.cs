using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAccess
{
	public class SystemSettingSearchTagList
	{
		public int ID { get; set; }

		public int SearchTagId { get; set; }

		public string TagName { get; set; }

		public bool isNewTag { get; set; }
	}
}