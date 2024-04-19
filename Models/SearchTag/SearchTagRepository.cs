using DataAccess;
using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreatEastForex.Models
{
	public class SearchTagRepository : ISearchTagRepository
	{
		private DataAccess.GreatEastForex db;

		public SearchTagRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<SearchTags> SelectAll()
		{
			var result = from u in db.SearchTags select u;

			return result;
		}

		public IQueryable<SearchTags> Select()
		{
			var result = from u in db.SearchTags where u.IsDeleted == "N" select u;

			return result;
		}

		public IList<SearchTags> GetAll()
		{
			try
			{
				var records = Select();

				return records.ToList();
			}
			catch
			{
				throw;
			}
		}

		public SearchTags GetSingle(int id)
		{
			try
			{
				var records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public SearchTags GetLast()
		{
			try
			{
				var records = Select();

				return records.OrderByDescending(e => e.ID).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(SearchTags data)
		{
			try
			{
				data.CreatedOn = DateTime.Now;
				data.UpdatedOn = DateTime.Now;
				data.IsDeleted = "N";

				db.SearchTags.Add(data);
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Update(int id, SearchTags data)
		{
			try
			{
				SearchTags searchTags = db.SearchTags.Find(id);

				searchTags.TagName = data.TagName;
				searchTags.UpdatedOn = DateTime.Now;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Delete(int id)
		{
			try
			{
				SearchTags searchTag = db.SearchTags.Find(id);

				searchTag.IsDeleted = "Y";
				searchTag.UpdatedOn = DateTime.Now;

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}
	}
}