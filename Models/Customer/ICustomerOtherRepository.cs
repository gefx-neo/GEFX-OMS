using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICustomerOtherRepository
    {
        CustomerOther GetSingle(int id);

        CustomerOther GetOthers(int CustomerParticularId);

        bool Add(CustomerOther addData);

		bool AddReject(Temp_CustomerOthers addData, string UpdateStatus);

		bool AddApproveKYC(KYC_CustomerOthers addData);

		bool Update(int id, CustomerOther updateData);

		bool UpdateStatus(int id, string status);

		bool UpdateEmergency(int id, CustomerOther updateData);

		bool UpdateReject(int id, Temp_CustomerOthers updateData, string UpdateStatus);

		bool UpdateApproveKYC(int id, KYC_CustomerOthers updateData);

		bool ApproveCustomer(int id, string status, int approvalID);

        bool Delete(int id);
    }
}