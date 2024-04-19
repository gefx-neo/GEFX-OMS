using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IKYC_CustomerDocumentCheckListRepository
	{
		KYC_CustomerDocumentCheckLists GetSingle(int id);

		KYC_CustomerDocumentCheckLists GetDocumentCheckList(int CustomerParticularId);

		bool Add(List<CustomerDocumentCheckList> addData);

		bool Update(int id, CustomerDocumentCheckList updateData);

		bool Delete(int id);
	}
}