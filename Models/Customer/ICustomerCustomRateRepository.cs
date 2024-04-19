using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICustomerCustomRateRepository
    {
        CustomerCustomRate GetSingle(int id);

        IList<CustomerCustomRate> GetCustomRates(int CustomerParticularId);

        CustomerCustomRate GetCustomerProductRate(int CustomerParticularId, int productid);

        bool Add(CustomerCustomRate addData);

		bool AddReject(IList<Temp_CustomerCustomRates> addData);

		bool Delete(int id);

		bool DeleteAll(int id);

	}
}