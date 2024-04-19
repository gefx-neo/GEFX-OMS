using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IKYC_CustomerActingAgentRepository
	{
		KYC_CustomerActingAgents GetSingle(int id);

		KYC_CustomerActingAgents GetActingAgent(int CustomerParticularId);

		bool Add(List<CustomerActingAgent> addData);

		bool Update(int id, CustomerActingAgent updateData);

		bool Delete(int id);
	}
}