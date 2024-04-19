using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class ApprovalHistorysRepository : IApprovalHistorysRepository
	{
		private DataAccess.GreatEastForex db;

		public ApprovalHistorysRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<ApprovalHistorys> Select()
		{
			var result = from a in db.ApprovalHistorys select a;

			return result;
		}

		public IList<ApprovalHistorys> GetAll(string application, int objectId)
		{
			try
			{
				var records = Select();

				if (!string.IsNullOrEmpty(application))
				{
					records = records.Where(e => e.Application == application);
				}

				if (objectId > 0)
				{
					records = records.Where(e => e.ObjectId == objectId);
				}

				return records.OrderByDescending(e => e.DateTimeAction).ToList();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(string application, int objectId, int approvalUserId, string approvalUserName, string role, string action, string description)
		{
			try
			{
				ApprovalHistorys newApproval = new ApprovalHistorys();

				newApproval.Application = application;
				newApproval.ObjectId = objectId;
				newApproval.ApprovalUserId = approvalUserId;
				newApproval.ApprovalUserName = approvalUserName;
				newApproval.Role = role;
				newApproval.Action = action;
				newApproval.Description = description;
				newApproval.DateTimeAction = DateTime.Now;

				db.ApprovalHistorys.Add(newApproval);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}
	}
}