using DataAccess;
using DataAccess.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreatEastForex.Models
{
	public class Temp_CustomerCustomRateRepository : ITemp_CustomerCustomRateRepository
	{
		private DataAccess.GreatEastForex db;

		public Temp_CustomerCustomRateRepository()
		{
			db = new DataAccess.GreatEastForex();
		}

		public IQueryable<Temp_CustomerCustomRates> Select()
		{
			var result = from c in db.Temp_CustomerCustomRates select c;

			return result;
		}

		public Temp_CustomerCustomRates GetSingle(int id)
		{
			try
			{
				IQueryable<Temp_CustomerCustomRates> records = Select();

				return records.Where(e => e.ID == id).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public IList<Temp_CustomerCustomRates> GetCustomRates(int CustomerParticularId)
		{
			try
			{
				IQueryable<Temp_CustomerCustomRates> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId).ToList();
			}
			catch
			{
				throw;
			}
		}

		public Temp_CustomerCustomRates GetCustomerProductRate(int CustomerParticularId, int productid)
		{
			try
			{
				IQueryable<Temp_CustomerCustomRates> records = Select();

				return records.Where(e => e.CustomerParticularId == CustomerParticularId && e.ProductId == productid).FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public bool Add(List<CustomerCustomRate> addData)
		{
			try
			{
				Temp_CustomerCustomRates temp = new Temp_CustomerCustomRates();
				List<Temp_CustomerCustomRates> templist = new List<Temp_CustomerCustomRates>();

				foreach (CustomerCustomRate rate in addData)
				{
					temp.CustomerParticularId = rate.CustomerParticularId;
					temp.ProductId = rate.ProductId;
					temp.BuyRate = rate.BuyRate;
					temp.SellRate = rate.SellRate;
					temp.EncashmentRate = rate.EncashmentRate;

					templist.Add(temp);
					temp = new Temp_CustomerCustomRates();
				}

				db.Temp_CustomerCustomRates.AddRange(templist);

				db.SaveChanges();

				return true;
			}
			catch
			{
				throw;
			}
		}

		public bool AddSingle(Temp_CustomerCustomRates addData)
		{
			try
			{
				Temp_CustomerCustomRates temp = new Temp_CustomerCustomRates();

				temp.CustomerParticularId = addData.CustomerParticularId;
				temp.ProductId = addData.ProductId;
				temp.BuyRate = addData.BuyRate;
				temp.SellRate = addData.SellRate;
				temp.EncashmentRate = addData.EncashmentRate;

				db.Temp_CustomerCustomRates.Add(temp);

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
				Temp_CustomerCustomRates data = db.Temp_CustomerCustomRates.Where(e => e.CustomerParticularId == id).FirstOrDefault();

				db.Temp_CustomerCustomRates.Remove(data);

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
				db.Temp_CustomerCustomRates.Where(e => e.CustomerParticularId == id).ToList().ForEach(tccr => db.Temp_CustomerCustomRates.Remove(tccr));
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