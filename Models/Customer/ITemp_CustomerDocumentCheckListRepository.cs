using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface ITemp_CustomerDocumentCheckListRepository
	{
		Temp_CustomerDocumentCheckLists GetSingle(int id);

		Temp_CustomerDocumentCheckLists GetDocumentCheckList(int CustomerParticularId);

		bool Add(List<CustomerDocumentCheckList> addData);

		bool AddSingle(CustomerDocumentCheckList addData);

		bool Update(int id, CustomerDocumentCheckList updateData);

		bool Delete(int id);

		bool DeleteAll(int id);
	}
}