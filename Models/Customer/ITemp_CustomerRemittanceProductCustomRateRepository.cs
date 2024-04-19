using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerRemittanceProductCustomRateRepository
	{
		Temp_CustomerRemittanceProductCustomRates GetSingle(int id);

		IList<Temp_CustomerRemittanceProductCustomRates> GetCustomRates(int CustomerParticularId);

		Temp_CustomerRemittanceProductCustomRates GetCustomerProductRate(int CustomerParticularId, int productid);

		bool Add(List<CustomerRemittanceProductCustomRate> addData);

		bool AddSingle(Temp_CustomerRemittanceProductCustomRates addData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}