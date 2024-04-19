using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class Temp_CustomerRemittanceProductCustomRateRepository : ITemp_CustomerRemittanceProductCustomRateRepository
	{
		private DataAccess.GreatEastForex db;

		public Temp_CustomerRemittanceProductCustomRateRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Temp_CustomerRemittanceProductCustomRates> Select()
		{
			var result = from c in db.Temp_CustomerRemittanceProductCustomRates select c;

			return result;
		}

		public Temp_CustomerRemittanceProductCustomRates GetSingle(int id)
		{
			try
			{
				IQueryable<Temp_CustomerRemittanceProductCustomRates> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public IList<Temp_CustomerRemittanceProductCustomRates> GetCustomRates(int CustomerParticularId)
		{
			try
			{
				IQueryable<Temp_CustomerRemittanceProductCustomRates> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId).ToList();
			}
			catch
			{
				throw;
			}
		}

		public Temp_CustomerRemittanceProductCustomRates GetCustomerProductRate(int CustomerParticularId, int productid)
		{
			try
			{
				IQueryable<Temp_CustomerRemittanceProductCustomRates> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId && e.RemittanceProductId == productid).FirstOrDefault();
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
				Temp_CustomerRemittanceProductCustomRates data = db.Temp_CustomerRemittanceProductCustomRates.Find(id);

				db.Temp_CustomerRemittanceProductCustomRates.Remove(data);

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
				db.Temp_CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id).ToList().ForEach(ccr => db.Temp_CustomerRemittanceProductCustomRates.Remove(ccr));
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Add(List<CustomerRemittanceProductCustomRate> addData)
		{
			try
			{
				Temp_CustomerRemittanceProductCustomRates temp = new Temp_CustomerRemittanceProductCustomRates();
				List<Temp_CustomerRemittanceProductCustomRates> templist = new List<Temp_CustomerRemittanceProductCustomRates>();

				foreach (CustomerRemittanceProductCustomRate rate in addData)
				{
					temp.CustomerParticularId = rate.CustomerParticularId;
					temp.RemittanceProductId = rate.RemittanceProductId;
					temp.Fee = rate.Fee;
					temp.GetRateAdjustment = rate.GetRateAdjustment;
					temp.PayRateAdjustment = rate.PayRateAdjustment;

					templist.Add(temp);
					temp = new Temp_CustomerRemittanceProductCustomRates();
				}
				db.Temp_CustomerRemittanceProductCustomRates.AddRange(templist);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddSingle(Temp_CustomerRemittanceProductCustomRates addData)
		{
			try
			{
				Temp_CustomerRemittanceProductCustomRates temp = new Temp_CustomerRemittanceProductCustomRates();

				temp.CustomerParticularId = addData.CustomerParticularId;
				temp.RemittanceProductId = addData.RemittanceProductId;
				temp.Fee = addData.Fee;
				temp.GetRateAdjustment = addData.GetRateAdjustment;
				temp.PayRateAdjustment = addData.PayRateAdjustment;

				db.Temp_CustomerRemittanceProductCustomRates.Add(temp);

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