using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IKYC_CustomerOtherRepository
	{
		KYC_CustomerOthers GetSingle(int id);

		KYC_CustomerOthers GetOthers(int CustomerParticularId);

		bool Add(CustomerOther addData, string NewStatus);

		bool Update(int id, CustomerOther updateData, string NewStatus);

		bool ApproveCustomer(int id, string status);

		bool Delete(int id);
	}
}