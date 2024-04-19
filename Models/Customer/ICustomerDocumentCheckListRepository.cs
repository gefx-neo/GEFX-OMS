using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
    public interface ICustomerDocumentCheckListRepository
    {
        CustomerDocumentCheckList GetSingle(int id);

        CustomerDocumentCheckList GetDocumentCheckList(int CustomerParticularId);

        bool Add(CustomerDocumentCheckList addData);

		bool AddReject(Temp_CustomerDocumentCheckLists addData);

		bool AddApproveKYC(KYC_CustomerDocumentCheckLists addData);

		bool Update(int id, CustomerDocumentCheckList updateData);

		bool UpdateReject(int id, Temp_CustomerDocumentCheckLists updateData);

		bool UpdateApproveKYC(int id, KYC_CustomerDocumentCheckLists updateData);

		bool Delete(int id);

		bool DeleteAll(int id);

	}
}