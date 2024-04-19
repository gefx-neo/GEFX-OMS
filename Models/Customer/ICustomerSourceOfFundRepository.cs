using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICustomerSourceOfFundRepository
    {
        CustomerSourceOfFund GetSingle(int id);

        CustomerSourceOfFund GetSourceOfFund(int CustomerParticularId);

        bool Add(CustomerSourceOfFund addData);

		bool AddReject(Temp_CustomerSourceOfFunds addData);

		bool AddApproveKYC(KYC_CustomerSourceOfFunds addData);

		bool Update(int id, CustomerSourceOfFund updateData);

		bool UpdateReject(int id, Temp_CustomerSourceOfFunds updateData);

		bool UpdateApproveKYC(int id, KYC_CustomerSourceOfFunds updateData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}