using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICustomerActingAgentRepository
    {
        CustomerActingAgent GetSingle(int id);

        CustomerActingAgent GetActingAgent(int CustomerParticularId);

        bool Add(CustomerActingAgent addData);

        bool Update(int id, CustomerActingAgent updateData);

		bool UpdateReject(int id, Temp_CustomerActingAgents updateData);

		bool UpdateApproveKYC(int id, KYC_CustomerActingAgents updateData);

		bool Delete(int id);

		bool DeleteAll(int id);

		bool AddSingle(int id, Temp_CustomerActingAgents updateData);

	}
}