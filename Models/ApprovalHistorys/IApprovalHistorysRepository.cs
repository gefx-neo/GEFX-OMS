using DataAccess.POCO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public interface IApprovalHistorysRepository
	{
		IList<ApprovalHistorys> GetAll(string application, int objectId);

		bool Add(string application, int objectId, int approvalUserId, string approvalUserName, string role, string action, string description);
	}
}