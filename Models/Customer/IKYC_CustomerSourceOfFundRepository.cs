using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IKYC_CustomerSourceOfFundRepository
	{
		KYC_CustomerSourceOfFunds GetSingle(int id);

		KYC_CustomerSourceOfFunds GetSourceOfFund(int CustomerParticularId);

		bool Add(CustomerSourceOfFund addData);

		bool Update(int id, CustomerSourceOfFund updateData);

		bool Delete(int id);
	}
}