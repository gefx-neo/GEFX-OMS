using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class Temp_CustomerActivityLogRepository : ITemp_CustomerActivityLogRepository
	{
		private DataAccess.GreatEastForex db;

		public Temp_CustomerActivityLogRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Temp_CustomerActivityLogs> Select()
		{
			var result = from c in db.Temp_CustomerActivityLogs select c;

			return result;
		}

		public IList<Temp_CustomerActivityLogs> GetAll(int customerParticularId)
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

		public Temp_CustomerActivityLogs GetSingle(int id)
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

		public bool Add(List<CustomerActivityLog> addData)
		{
			try
			{
				Temp_CustomerActivityLogs temp = new Temp_CustomerActivityLogs();
				List<Temp_CustomerActivityLogs> templist = new List<Temp_CustomerActivityLogs>();

				foreach (CustomerActivityLog log in addData)
				{
					temp.CustomerParticularId = log.CustomerParticularId;
					temp.Title = log.Title;
					temp.ActivityLog_DateTime = log.ActivityLog_DateTime;
					temp.ActivityLog_Note = log.ActivityLog_Note;

					templist.Add(temp);
					temp = new Temp_CustomerActivityLogs();
				}

				db.Temp_CustomerActivityLogs.AddRange(templist);

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
				Temp_CustomerActivityLogs data = db.Temp_CustomerActivityLogs.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				db.Temp_CustomerActivityLogs.Remove(data);

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
				db.Temp_CustomerActivityLogs.Where(e => e.CustomerParticularId == id).ToList().ForEach(tal => db.Temp_CustomerActivityLogs.Remove(tal));
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