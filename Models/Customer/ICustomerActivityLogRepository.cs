using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ICustomerActivityLogRepository
	{
		IList<CustomerActivityLog> GetAll(int customerParticularId);

		CustomerActivityLog GetSingle(int id);

		bool Add(CustomerActivityLog addData);

		bool AddReject(IList<Temp_CustomerActivityLogs> addData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}