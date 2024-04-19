using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class CustomerActivityLogRepository : ICustomerActivityLogRepository
	{
		private DataAccess.GreatEastForex db;

		public CustomerActivityLogRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<CustomerActivityLog> Select()
		{
			var result = from c in db.CustomerActivityLogs select c;

			return result;
		}

		public IList<CustomerActivityLog> GetAll(int customerParticularId)
		{
			try
			{
				var records = Select();

				return records.Where(e => e.CustomerParticularId == customerParticularId).ToList();
			}
			catch
			{
				throw;
			}
		}

		public CustomerActivityLog GetSingle(int id)
		{
			try
			{
				var records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(CustomerActivityLog addData)
		{
			try
			{
				db.CustomerActivityLogs.Add(addData);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddReject(IList<Temp_CustomerActivityLogs> addData)
		{
			try
			{
				CustomerActivityLog log = new CustomerActivityLog();
				List<CustomerActivityLog> logList = new List<CustomerActivityLog>();

				foreach (Temp_CustomerActivityLogs temp in addData)
				{
					log.ActivityLog_DateTime = temp.ActivityLog_DateTime;
					log.ActivityLog_Note = temp.ActivityLog_Note;
					log.CustomerParticularId = temp.CustomerParticularId;
					log.Title = temp.Title;
					logList.Add(log);
					log = new CustomerActivityLog();
				}

				db.CustomerActivityLogs.AddRange(logList);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Delete(int id)
		{
			try
			{
				CustomerActivityLog data = db.CustomerActivityLogs.Find(id);

				db.CustomerActivityLogs.Remove(data);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool DeleteAll(int id)
		{
			try
			{
				db.CustomerActivityLogs.Where(e => e.CustomerParticularId == id).ToList().ForEach(al => db.CustomerActivityLogs.Remove(al));
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