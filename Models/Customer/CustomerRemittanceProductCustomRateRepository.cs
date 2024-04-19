using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class CustomerRemittanceProductCustomRateRepository : ICustomerRemittanceProductCustomRateRepository
	{
		private DataAccess.GreatEastForex db;

		public CustomerRemittanceProductCustomRateRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<CustomerRemittanceProductCustomRate> Select()
		{
			var result = from c in db.CustomerRemittanceProductCustomRates select c;

			return result;
		}

		public CustomerRemittanceProductCustomRate GetSingle(int id)
		{
			try
			{
				IQueryable<CustomerRemittanceProductCustomRate> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public IList<CustomerRemittanceProductCustomRate> GetCustomRates(int CustomerParticularId)
		{
			try
			{
				IQueryable<CustomerRemittanceProductCustomRate> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId).ToList();
			}
			catch
			{
				throw;
			}
		}

		public CustomerRemittanceProductCustomRate GetCustomerProductRate(int CustomerParticularId, int productid)
		{
			try
			{
				IQueryable<CustomerRemittanceProductCustomRate> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId && e.RemittanceProductId == productid).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool AddReject(IList<Temp_CustomerRemittanceProductCustomRates> addData)
		{
			try
			{
				CustomerRemittanceProductCustomRate rate = new CustomerRemittanceProductCustomRate();
				List<CustomerRemittanceProductCustomRate> ratelist = new List<CustomerRemittanceProductCustomRate>();

				foreach (Temp_CustomerRemittanceProductCustomRates temp in addData)
				{
					rate.Fee = temp.Fee;
					rate.PayRateAdjustment = temp.PayRateAdjustment;
					rate.GetRateAdjustment = temp.GetRateAdjustment;
					rate.CustomerParticularId = temp.CustomerParticularId;
					rate.RemittanceProductId = temp.RemittanceProductId;

					ratelist.Add(rate);
					rate = new CustomerRemittanceProductCustomRate();
				}

				db.CustomerRemittanceProductCustomRates.AddRange(ratelist);

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
				CustomerRemittanceProductCustomRate data = db.CustomerRemittanceProductCustomRates.Find(id);

				db.CustomerRemittanceProductCustomRates.Remove(data);

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
				db.CustomerRemittanceProductCustomRates.Where(e => e.CustomerParticularId == id).ToList().ForEach(ccr => db.CustomerRemittanceProductCustomRates.Remove(ccr));
				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool Add(CustomerRemittanceProductCustomRate addData)
		{
			try
			{
				db.CustomerRemittanceProductCustomRates.Add(addData);

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