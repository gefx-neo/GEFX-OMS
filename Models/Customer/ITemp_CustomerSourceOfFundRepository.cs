using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerSourceOfFundRepository
	{
		Temp_CustomerSourceOfFunds GetSingle(int id);

		Temp_CustomerSourceOfFunds GetSourceOfFund(int CustomerParticularId);

		bool Add(CustomerSourceOfFund addData);

		bool Update(int id, CustomerSourceOfFund updateData);

		bool Delete(int id);
	}
}