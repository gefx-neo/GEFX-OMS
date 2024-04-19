using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IAgentRepository
	{
		IList<Agents> GetAll();

		Agents GetSingle(int id);

		Agents FindAgentId(string AgentId);

		Agents FindAgentIdNotOwnSelf(int id, string AgentId);

		IPagedList<Agents> GetPaged(string keyword, int page, int size);

		bool Add(Agents addData);

		bool Update(int id, Agents updateData);

		bool Delete(int id);
	}
}