using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerCustomRateRepository
	{
		Temp_CustomerCustomRates GetSingle(int id);

		IList<Temp_CustomerCustomRates> GetCustomRates(int CustomerParticularId);

		Temp_CustomerCustomRates GetCustomerProductRate(int CustomerParticularId, int productid);

		bool Add(List<CustomerCustomRate> addData);

		bool AddSingle(Temp_CustomerCustomRates addData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}