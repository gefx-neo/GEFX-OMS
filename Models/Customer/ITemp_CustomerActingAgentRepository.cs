using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerActingAgentRepository
	{
		Temp_CustomerActingAgents GetSingle(int id);

		Temp_CustomerActingAgents GetActingAgent(int CustomerParticularId);

		bool Add(List<CustomerActingAgent> addData);

		bool Update(int id, CustomerActingAgent updateData);

		bool Delete(int id);

		bool AddSingle(CustomerActingAgent addData);
	}
}