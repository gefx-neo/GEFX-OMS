using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ICustomerRemittanceProductCustomRateRepository
	{
		CustomerRemittanceProductCustomRate GetSingle(int id);

		IList<CustomerRemittanceProductCustomRate> GetCustomRates(int CustomerParticularId);

		CustomerRemittanceProductCustomRate GetCustomerProductRate(int CustomerParticularId, int productid);

		bool Add(CustomerRemittanceProductCustomRate addData);

		bool AddReject(IList<Temp_CustomerRemittanceProductCustomRates> addData);

		bool Delete(int id);

		bool DeleteAll(int id);

	}
}