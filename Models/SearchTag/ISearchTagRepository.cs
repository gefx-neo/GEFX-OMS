using DataAccess.POCO;
using PagedList;
using System.Collections.Generic;

namespace GreatEastForex.Models
{
	public interface ISearchTagRepository
	{
		IList<SearchTags> GetAll();

		SearchTags GetSingle(int id);

		SearchTags GetLast();

		bool Add(SearchTags data);

		bool Delete(int id);

		bool Update(int id, SearchTags data);
	}
}