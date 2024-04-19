using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerActivityLogRepository
	{
		IList<Temp_CustomerActivityLogs> GetAll(int customerParticularId);

		Temp_CustomerActivityLogs GetSingle(int id);

		bool Add(List<CustomerActivityLog> addData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}